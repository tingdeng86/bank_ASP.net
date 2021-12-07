using ASP_Assignment.Data;
using ASP_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASP_Assignment.Data.ApplicationDbContext;

namespace ASP_Assignment.Repositories
{
    public class BankAccountRepo
    {
        private readonly ApplicationDbContext _context;
        public BankAccountRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Update(int id, decimal balance)
        {
            BankAccount bankAccount = _context.BankAccounts
                .Where(e => e.accountNum == id).FirstOrDefault();
            bankAccount.balance =balance;

            _context.SaveChanges();
            return true;
        }

        public int Add( BankAccount bankAccount)
        {

            _context.BankAccounts.Add(bankAccount);
            _context.SaveChanges();
            return bankAccount.accountNum;
        }

        public bool Delete(int id)
        {
            BankAccount bankAccount = _context.BankAccounts
                                      .Where(e => e.accountNum == id).FirstOrDefault();
            _context.Remove(bankAccount);
            _context.SaveChanges();
            return true;
        }
    }
}
