namespace Todo.Repo.Models
{
    using System;
    using Microsoft.Extensions.Configuration;

    public class CosmosDbSettings
    {
        public CosmosDbSettings(IConfiguration configuration)
        {
            try
            {
                ConnectionString = configuration.GetSection("ConnectionString").Value;
                DatabaseName = configuration.GetSection("DatabaseName").Value;
                CollectionName = configuration.GetSection("CollectionName").Value;
                PartitionKey = configuration.GetSection("PartitionKey").Value;
            }
            catch
            {
                throw new MissingFieldException("IConfiguration missing a valid Azure Cosmos DB field appsettings.json");
            }
        }
        public string DatabaseName { get; private set; }
        public string CollectionName { get; private set; }
        public string ConnectionString { get; set; }
        public string PartitionKey { get; set; }
    }
}
