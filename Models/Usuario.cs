using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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

    [JsonIgnore]
    public ICollection<Todo> todos = new List<Todo>();
  }
}