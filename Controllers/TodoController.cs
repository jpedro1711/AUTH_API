using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authAPI
{
  [ApiController]
  [Route("api/[controller]")]
  public class TodoController : Controller
  {
    private readonly TodoRepository repository;


    public TodoController() {
      repository = new TodoRepository();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create([FromBody] TodoDto dto)
    {
      repository.Create(dto.ToEntity());

      return Created();
    }

    [Authorize]
    [HttpGet("User/{id}")]
    public List<Todo> GetTodosByUser(int id)
    {
        return repository.GetTodosByUser(id);
    }


    [Authorize(Roles = "admin")]
    [HttpGet]
    public List<Todo> GetAll()
    {
        return repository.FindAll();
    }

    [Authorize]
    [HttpGet("{id}")]
    public Todo GetById(int id)
    {
        var result = repository.FindById(id);

        if (result != null) 
        {
            return result;
        }
        throw new Exception("Todo not found");
    }

    [Authorize]
    [HttpPut("{id}")]
    public Todo Update(int id, TodoDto dto)
    {
        return repository.Update(id, dto.ToEntity());
    }

    [Authorize]
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        repository.Delete(id);
    }
  }
}