using System.Net;
using Microsoft.EntityFrameworkCore;

namespace authAPI
{
    public class TodoRepository
    {
        private readonly AuthApiContext _context;
        public TodoRepository() 
        {
            _context = new AuthApiContext();
        }

        public Todo Create(Todo entity) {
            _context.Todo.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public List<Todo> FindAll() {
            return _context.Todo.Include("User").ToList();
        }

        public Todo FindById(int id)
        {
            var result =  _context.Todo.Include("User").FirstOrDefault(u => u.TodoId == id);
            if (result == null) {
                throw new EntityException("Todo item not found with given id", HttpStatusCode.NotFound);
            }
            return result;
        }

        public Todo Update(int id, Todo todo)
        {
            Todo existing = FindById(id);

            if (existing == null)
            {
                throw new EntityException("Todo item not found with given id", HttpStatusCode.NotFound);
            }

            existing.Description = todo.Description;
            existing.Deadline = todo.Deadline;
            existing.UserId = todo.UserId;
            existing.Done = todo.Done;
            _context.SaveChanges();
            return existing;
        }

        public void Delete(int id)
        {
            Todo existing = FindById(id);

            if (existing == null)
            {
                throw new EntityException("Todo item not found with given id", HttpStatusCode.NotFound);
            }

            _context.Remove(existing);
            _context.SaveChanges();
        }

        public List<Todo> GetTodosByUser(int id)
        {
            return _context.Todo.Include("User").Where(t => t.UserId == id).ToList();
        }
    }
}