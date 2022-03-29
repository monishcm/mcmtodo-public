using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Todo.Repo.Models;

namespace Todo.Repo.Services
{
    public interface IDocumentDbService
    {   Task InitializeAsync();
        Task AddItemAsync(TodoItem item);
        Task DeleteItemAsync(string id, string userId);
        Task<TodoItem> GetItemAsync(string id, string userId);
        Task<IEnumerable<TodoItem>> GetItemsAsync(Expression<Func<TodoItem, bool>> predicate);
        Task UpdateItemAsync(TodoItem item);
        Task<IEnumerable<TodoItem>> QueryItemsAsync(string query);
    }
}
