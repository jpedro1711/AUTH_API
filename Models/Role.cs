using System.ComponentModel.DataAnnotations;

namespace  authAPI
{
  public class Role 
  {
    [Key]
    public int RoleId { get; set; }
    public string Name { get; set; }
  }
}