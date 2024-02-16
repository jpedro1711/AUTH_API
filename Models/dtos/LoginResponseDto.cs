namespace authAPI
{
  public class LoginResponseDto
  {
    public string? Username {get; set;}
    public string? Password {get; set;}
    public string? Token {get; set;}
    public string? Role {get; set;}
    public int? UserID {get; set;}
  }
}