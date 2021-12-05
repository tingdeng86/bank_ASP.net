using ASP_Assignment.Data;
using ASP_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASP_Assignment.Data.ApplicationDbContext;

namespace ASP_Assignment.Repositories
{
    public class ClientRepo
    {
        private readonly ApplicationDbContext _context;
        public ClientRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Update(int id, string first, string last)
        {
            Client client = _context.Clients
                .Where(e => e.clientID == id).FirstOrDefault();

            // Remember you update the primary key without 
            // causing trouble.  Just update the first and 
            // last names for now.
             client.firstName= first;
            client.lastName = last;
            _context.SaveChanges();
            return true;
        }

    }
}
