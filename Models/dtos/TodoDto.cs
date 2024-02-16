namespace authAPI
{
  public class TodoDto
  {
    public string Description {get; set;}
    public DateTime Deadline {get; set;}
    public int UserId {get; set;}
    public bool Done {get; set;}

    public Todo ToEntity()
    {
        return new Todo {
            Description = Description,
            Deadline = Deadline,
            UserId = UserId,
            Done = Done
        };
    }
  }
}