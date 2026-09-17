using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace GestorTareas.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? DueDate { get; set; }

    public string UserId { get; set; } = string.Empty;

    public IdentityUser? User { get; set; }

}
