using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonitorRedCore.Core.Interfaces;
using MonitorRedCore.Core.Models;
using MonitorRedCore.Infraestructure.Data;

namespace MonitorRedCore.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MONITOREDContext _context;

        public UserRepository(MONITOREDContext context)
        {
            _context = context;
        }

        public Users GetUser(string Email)
        {
            var user = _context.Users.FirstOrDefaultAsync(x => x.Email == Email).Result;

            return user;
        }

        public IList<Users> GetUsers()
        {
            var users = _context.Users.ToListAsync().Result;

            return users;
        }

        public async Task RegisterUser(Users user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                System.Console.Write("Trono alv: ", ex.Message.ToString());
            }
        }
    }
}
