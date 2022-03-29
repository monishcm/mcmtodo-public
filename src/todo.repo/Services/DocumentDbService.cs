namespace Todo.Repo.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Todo.Repo.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Linq;
    using System.Text.Json;

    public class DocumentDbService : IDocumentDbService
    {
        private readonly CosmosDbSettings _settings;
        private Container _container;

        public DocumentDbService(IConfigurationSection configuration)
        {
            _settings = new CosmosDbSettings(configuration);
            //See https://docs.microsoft.com/en-us/azure/cosmos-db/performance-tips for performance tips
        }

        public async Task InitializeAsync()
        {
            CosmosClient dbClient = new CosmosClient(_settings.ConnectionString, new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });

            Database database = await dbClient.CreateDatabaseIfNotExistsAsync(_settings.DatabaseName);

            _container = await database.CreateContainerIfNotExistsAsync(_settings.CollectionName, _settings.PartitionKey);
        }

        public async Task AddItemAsync(TodoItem item)
        {
            //string jsonText = JsonSerializer.Serialize(item);
            if(string.IsNullOrWhiteSpace(item.Id))
            {
                item.Id = Guid.NewGuid().ToString();
            }

            await _container.CreateItemAsync(item, new PartitionKey(item.UserId));
        }

        public async Task DeleteItemAsync(string id, string userId)
        {
            await _container.DeleteItemAsync<TodoItem>(id, new PartitionKey(userId));
        }

        public async Task<TodoItem> GetItemAsync(string id, string userId)
        {
            return await _container.ReadItemAsync<TodoItem>(id, new PartitionKey(userId));
        }

        public async Task<IEnumerable<TodoItem>> GetItemsAsync(Expression<Func<TodoItem, bool>> predicate)
        {
            List<TodoItem> items = new List<TodoItem>();

            FeedIterator<TodoItem> resultIterator = _container.GetItemLinqQueryable<TodoItem>(true).Where(predicate).ToFeedIterator();

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<TodoItem> currentResultSet = await resultIterator.ReadNextAsync();
                foreach (TodoItem item in currentResultSet)
                {
                    items.Add(item);
                }
            }
            return items;

        }

        public async Task UpdateItemAsync(TodoItem item)
        {
            await _container.ReplaceItemAsync(item, item.Id, new PartitionKey(item.UserId));
        }

        public async Task<IEnumerable<TodoItem>> QueryItemsAsync(string query)
        {
            QueryDefinition queryDefinition = new QueryDefinition(query);

            List<TodoItem> items = new List<TodoItem>();

            FeedIterator<TodoItem> resultIterator = _container.GetItemQueryIterator<TodoItem>(queryDefinition);

            while (resultIterator.HasMoreResults)
            {
                FeedResponse<TodoItem> currentResultSet = await resultIterator.ReadNextAsync();
                foreach (TodoItem item in currentResultSet)
                {
                    items.Add(item);
                }
            }
            return items;
        }
    }
}
