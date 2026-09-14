using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestorTareas.Data;
using GestorTareas.Models;

namespace GestorTareas.Pages.Tasks
{
    public class DeleteModel : PageModel
    {
        private readonly GestorTareas.Data.ApplicationDbContext _context;

        public DeleteModel(GestorTareas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskItem TaskItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskitem = await _context.TaskItems.FirstOrDefaultAsync(m => m.Id == id);

            if (taskitem == null)
            {
                return NotFound();
            }
            else
            {
                TaskItem = taskitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskitem = await _context.TaskItems.FindAsync(id);
            if (taskitem != null)
            {
                TaskItem = taskitem;
                _context.TaskItems.Remove(TaskItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
