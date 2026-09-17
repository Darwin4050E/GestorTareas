using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GestorTareas.Data;
using GestorTareas.Models;
using System.Reflection.Metadata;

namespace GestorTareas.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly GestorTareas.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(GestorTareas.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<TaskItem> PendingTasks { get; set; } = default!;
        public IList<TaskItem> CompletedTasks { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var all = await _context.TaskItems
                .Where(t => t.UserId == _userManager.GetUserId(User))
                .OrderBy(t => t.DueDate ?? DateTime.MaxValue)
                .ThenByDescending(t => t.CreatedAt)
                .ToListAsync();

            PendingTasks = all.Where(t => !t.IsCompleted).ToList();
            CompletedTasks = all.Where(t => t.IsCompleted).ToList();
        }

        public async Task<IActionResult> OnPostToggleCompleteAsync(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null) return NotFound();
            if (task.UserId != _userManager.GetUserId(User)) return Forbid();
            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}