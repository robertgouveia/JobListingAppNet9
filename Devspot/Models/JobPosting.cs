using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Devspot.Models;

public class JobPosting
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public string Company { get; set; }
    
    [Required]
    public string Location { get; set; }
    
    public DateTime PostedDate { get; set; } = DateTime.Now;
    
    public bool IsApproved { get; set; }
    
    [Required]
    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }
}