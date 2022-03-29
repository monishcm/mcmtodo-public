namespace Todo.Repo.Controllers
{
    using System.Threading.Tasks;
    using Todo.Repo.Services;
    using Microsoft.AspNetCore.Mvc;
    using Todo.Repo.Models;
    using System.Collections.Generic;
    using System;

    [ApiController]
    [Route("[controller]")]
    public class ItemController : Controller
    {
        private readonly IDocumentDbService _documentDbService;

        public ItemController(IDocumentDbService documentDbService)
        {
            _documentDbService = documentDbService;
        }

        /// <summary>
        /// Gets list of all the Todos for the given user
        /// </summary>
        /// <returns>List of Todos</returns>
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll(string userId)
        {
            
            try
            {
                var result = await _documentDbService.GetItemsAsync(d => !d.Completed && d.UserId == userId);
                //var result = await _documentDbService.QueryItemsAsync($"select * from todoitems as c where c.userId = '{userId}' and c.completed = false");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a Todo item
        /// </summary>
        /// <param name="item">The Todo item to be created</param>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(TodoItem item)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(item.Id))
                {
                    item.Id = Guid.NewGuid().ToString();
                }

                await _documentDbService.AddItemAsync(item);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// Modifies a Todo item
        /// </summary>
        /// <param name="item">The Todo item to be modified</param>
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(TodoItem item)
        {
            try
            {
                await _documentDbService.UpdateItemAsync(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a Todo item
        /// </summary>
        /// <param name="id">The id of the Todo to be deleted</param>
        /// <param name="userId">The User Id</param>
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string id, string userId)
        {
            try
            {
                await _documentDbService.DeleteItemAsync(id, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets the details of a Todo item
        /// </summary>
        /// <param name="id">The id of the Todo to be deleted</param>
        /// <param name="userId">The User Id</param>
        [HttpGet("GetDetails")]
        public async Task<ActionResult<TodoItem>> GetDetails(string id, string userId)
        {
            try
            {
                var result = await _documentDbService.GetItemAsync(id, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}