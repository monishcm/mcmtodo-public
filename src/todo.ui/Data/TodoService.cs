using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Todo.UI.Models;

namespace Todo.UI.Data
{
    public class TodoService
    {
        private HttpClient _httpClient;
        public TodoService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("TodoAPIClient");
        }

        public async Task<IEnumerable<TodoItem>> GetAll(string userId)
        {
            // List<TodoItem> todoItems = new List<TodoItem>();

            // using (var client = new HttpClient())
            // {
            //     client.BaseAddress = new Uri("http://localhost:5000");
            //     client.DefaultRequestHeaders.Clear();
            //     //client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            //     using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/Item/GetAll?userId={userId}");
            //     var response = await client.SendAsync(requestMessage);
            //     response.EnsureSuccessStatusCode();

            //     using var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            //     todoItems = await JsonSerializer.DeserializeAsync<List<TodoItem>>(content, new JsonSerializerOptions{
            //         PropertyNameCaseInsensitive = true
            //     }).ConfigureAwait(false);
            // }

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/Item/GetAll?userId={userId}");
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var todoItems = await JsonSerializer.DeserializeAsync<IEnumerable<TodoItem>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).ConfigureAwait(false);

            return todoItems;
        }

        public async Task Create(TodoItem item)
        {
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                item.Id = Guid.NewGuid().ToString();
            }

            using var content = new StringContent(JsonSerializer.Serialize(item, typeof(TodoItem), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }),
            System.Text.Encoding.Unicode, "application/json");

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/Item/Create/")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Edit(TodoItem item)
        {
            using var content = new StringContent(JsonSerializer.Serialize(item), System.Text.Encoding.Unicode, "application/json");
            using var requestMessage = new HttpRequestMessage(HttpMethod.Put, "/Item/Edit/")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public async Task Delete(string id, string userId)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"/Item/GetAll?id={id}&userId={userId}");
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }

        public async Task<TodoItem> GetDetails(string id, string userId)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"/Item/GetAll?id={id}&userId={userId}");
            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();

            using var content = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var todoItem = await JsonSerializer.DeserializeAsync<TodoItem>(content).ConfigureAwait(false);

            return todoItem;
        }

    }
}
