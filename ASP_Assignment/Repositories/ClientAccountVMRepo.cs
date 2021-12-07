using ASP_Assignment.Data;
using ASP_Assignment.Models;
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
       
        public IQueryable<ClientAccountVM> GetAll()
        {
            var query = from c in _context.Clients
                        from b in _context.BankAccounts
                        from ca in _context.ClientAccounts
                        where ca.accountNum == b.accountNum
                        where c.clientID == ca.clientID
                        orderby b.accountNum descending
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

        public IQueryable<ClientAccountVM> GetLists(string email)
        {
            var query = GetAll()
                        .Where(es => es.email ==email)
                        
                        ;
            return query;

        }

        public ClientAccount Add(ClientAccountVM caVM)
        {

            BankAccountRepo baRepo = new BankAccountRepo(_context);
            BankAccount bankAccount = new BankAccount()
            {
                balance = caVM.balance,
                accountType = caVM.accountType
            };
           int bankNum= baRepo.Add(bankAccount);
 
            ClientRepo clientRepo = new ClientRepo(_context);
            int clientID = clientRepo.GetID(caVM.email);
            ClientAccount clientAccount = new ClientAccount()
            {
                clientID = clientID,
                accountNum = bankNum,
            };
            _context.ClientAccounts.Add(clientAccount);
            _context.SaveChanges();
            return clientAccount;
        }
        public bool AddRegister(MyRegisteredUser myRegisteredUser)
        {
            BankAccountRepo baRepo = new BankAccountRepo(_context);
            BankAccount bankAccount = new BankAccount()
            {
                balance = myRegisteredUser.Balance,
                accountType = myRegisteredUser.AccountType
            };
            int bankNum = baRepo.Add(bankAccount);

            ClientRepo clientRepo = new ClientRepo(_context);
            Client client = new Client()
            {
                email = myRegisteredUser.Email,
                lastName = myRegisteredUser.LastName,
                firstName = myRegisteredUser.FirstName,
            };
            int clientID = clientRepo.AddAccounts(client);
            ClientAccount clientAccount = new ClientAccount()
            {
                clientID = clientID,
                accountNum = bankNum,
            };
            _context.ClientAccounts.Add(clientAccount);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int clientID,int accountNum)
        {
            ClientAccount clientAccount = new ClientAccount()
            {
                clientID = clientID,
                accountNum = accountNum,
            };
            _context.ClientAccounts.Remove(clientAccount);
            _context.SaveChanges();
            BankAccountRepo baRepo = new BankAccountRepo(_context);
            baRepo.Delete(accountNum);
            return true;
        }
        internal void Add(ClientAccount clientAccount)
        {
            throw new NotImplementedException();
        }

        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }

        internal void Add(BankAccount bankAccount)
        {
            throw new NotImplementedException();
        }
    }
}
