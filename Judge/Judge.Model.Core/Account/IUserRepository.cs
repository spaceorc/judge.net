using System.Collections.Generic;
using Judge.Model.Core.Entities;

namespace Judge.Model.Core.Account
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers(IEnumerable<long> users);

        User GetUser(long id);
    }
}