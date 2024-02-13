using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace authAPI
{
  public class Usuario
  {
    [Key]
    public int? UserId { get; set; }
    public string Username {get; set;}
    public string PasswordHash {get; set;} 

    [ForeignKey("RoleId")]
    public int RoleId {get; set;}
    public Role Role {get; set;}
  }
}