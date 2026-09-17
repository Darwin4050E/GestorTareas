using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GestorTareas.Data;
using GestorTareas.Models;
using System.Reflection.Metadata;
using System.Security.Principal;

namespace GestorTareas.Pages.Tasks
{
    public class EditModel : PageModel
    {
        private readonly GestorTareas.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(GestorTareas.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskitem =  await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id && m.UserId != _userManager.GetUserId(User));
            if (taskitem == null)
            {
                return NotFound();
            }
            TaskItem = taskitem;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var taskItemUpdate = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == TaskItem.Id && t.UserId == _userManager.GetUserId(User));
            if (taskItemUpdate == null) return NotFound();

            taskItemUpdate.Title = TaskItem.Title;
            taskItemUpdate.Description = TaskItem.Description;
            taskItemUpdate.DueDate = TaskItem.DueDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskItemExists(TaskItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskItemExists(int id)
        {
            return _context.TaskItems.Any(e => e.Id == id && e.UserId == _userManager.GetUserId(User));
        }
    }
}
