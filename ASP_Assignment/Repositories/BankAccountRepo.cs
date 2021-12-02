﻿using ASP_Assignment.Data;
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
            bankAccount.balance =(decimal) balance;

            _context.SaveChanges();
            return true;
        }
    }
}
