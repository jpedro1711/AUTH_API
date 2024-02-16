namespace authAPI
{
  public class UserDto
  {
    public string Username {get; set;}
    public string Password {get; set;}
    public int RoleId {get; set;}

    public Usuario ToEntity()
    {
      return new Usuario {
        Username = Username,
        RoleId = RoleId
      };
    }
  }
}