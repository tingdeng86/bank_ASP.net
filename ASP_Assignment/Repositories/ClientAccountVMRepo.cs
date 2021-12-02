using ASP_Assignment.Data;
using ASP_Assignment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Assignment.Repositories
{
    public class ClientAccountVMRepo
    {
        private readonly ApplicationDbContext _context;
        public ClientAccountVMRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<ClientAccountVM>GetAll(string accountType)
        {
            var query = from c in _context.Clients
                        from b in _context.BankAccounts
                        from ca in _context.ClientAccounts
                        where ca.accountNum == b.accountNum
                        where c.clientID == ca.clientID
                        where b.accountType == accountType
                        orderby c.lastName
                        select new ClientAccountVM()  // Create new object using
                        {                             // our view model class.
                            accountNum = b.accountNum,
                            firstName= c.firstName == null ? "" : c.firstName,
                            lastName = c.lastName == null ? "" : c.lastName,
                            accountType = b.accountType,

                        };
            return query;
        }
        public IQueryable<ClientAccountVM> GetAll()
        {
            var query = from c in _context.Clients
                        from b in _context.BankAccounts
                        from ca in _context.ClientAccounts
                        where ca.accountNum == b.accountNum
                        where c.clientID == ca.clientID
                        orderby c.lastName
                        select new ClientAccountVM()  // Create new object using
                        {                             // our view model class.
                            accountNum = b.accountNum,
                            firstName = c.firstName == null ? "" : c.firstName,
                            lastName = c.lastName == null ? "" : c.lastName,
                            accountType = b.accountType,
                            email = c.email,
                            balance = (decimal)b.balance,

                        };
            return query;
        }
        //public ClientAccountVM Get(string accountType)
        //{
        //    var query = GetAll()
        //                .Where(es => es.accountType == accountType)
        //                .FirstOrDefault();
        //    return query;

        //}

    }
}
