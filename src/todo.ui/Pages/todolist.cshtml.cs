using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Todo.UI.Data;
using Todo.UI.Models;

namespace Todo.UI
{
    public class TodolistModel : PageModel
    {
        private TodoService _todoService;
        public TodolistModel(TodoService todoService)
        {
            _todoService = todoService;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TodoItem Todo { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _todoService.Create(Todo);

            return RedirectToPage("./todolist");


        }
    }
}
