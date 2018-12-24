using Judge.Model.Core.Account;
using Judge.Model.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Judge.Data.Core
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly DbSet<User> _usersSet;

        public UserRepository(DataContext context)
        {
            _context = context;
            _usersSet = context.Set<User>();
        }

        public void Create(User user)
        {
            _usersSet.Add(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetUsers(IEnumerable<long> users)
        {
            return _usersSet.Where(o => users.Contains(o.Id)).OrderBy(o => o.Id).AsEnumerable();
        }

        public User GetUser(long id)
        {
            return _usersSet.FirstOrDefault(o => o.Id == id);
        }

        public User GetUser(string email)
        {
            return _usersSet.FirstOrDefault(o => o.Email == email);
        }
    }
}
