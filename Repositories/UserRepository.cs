using System.Net;
using Microsoft.EntityFrameworkCore;

namespace authAPI
{
    public class UserRepository
    {
        private readonly AuthApiContext _context;
        public UserRepository() 
        {
            _context = new AuthApiContext();
        }

        public Usuario Create(Usuario entity) {
            _context.Usuario.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public List<Usuario> FindAll() {
            return _context.Usuario.ToList();
        }

        public Usuario FindById(int id)
        {
            var result = _context.Usuario.FirstOrDefault(u => u.UserId == id);
            if (result == null) 
            {
                throw new EntityException("User not found with given id", HttpStatusCode.NotFound);
            }
            return result;
        }

        public Usuario GetByUsername(string username) {
            var result = _context.Usuario.Include("Role").FirstOrDefault(u => u.Username == username);
            if (result == null)
            {
                throw new EntityException("User not found with given username", HttpStatusCode.NotFound);
            }
            return result;
        }

        public Usuario Update(int id, Usuario usuario)
        {
            Usuario existing = FindById(id);

            if (existing == null)
            {
                throw new EntityException("User not found with given id", HttpStatusCode.NotFound);
            }
            _context.Entry(existing).CurrentValues.SetValues(usuario);
            _context.SaveChanges();
            return existing;
        }

        public void Delete(int id)
        {
            Usuario existing = FindById(id);

            if (existing == null)
            {
                throw new EntityException("User not found with given id", HttpStatusCode.NotFound);
            }

            _context.Remove(existing);
            _context.SaveChanges();
        }
    }
}