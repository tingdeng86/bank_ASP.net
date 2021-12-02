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
                            clientID=c.clientID,
                            email=c.email,
                            balance = b.balance,
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
                            balance =b.balance,
                            clientID = c.clientID,
                            

                        };
            return query;
        }
        public ClientAccountVM GetDetail(int clientID, int accountNum)
        {
            var query = GetAll()
                        .Where(es => es.accountNum == accountNum && es.clientID == clientID)
                        .FirstOrDefault();
            return query;

        }
        public bool Update(ClientAccountVM caVM)
        {
            ClientRepo clientRepo = new ClientRepo(_context);
            clientRepo.Update(caVM.clientID, caVM.firstName, caVM.lastName);
            BankAccountRepo baRepo = new BankAccountRepo(_context);
            baRepo.Update(caVM.accountNum, caVM.balance);
            return true;
        }
    }
}
