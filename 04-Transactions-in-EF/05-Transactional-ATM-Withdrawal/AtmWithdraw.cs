// This project is common for 05 and 06 problem.
// Please create ATM database in SQL server with 06-Problem-ATM-DB-with-TransactionHistory.sql script
// it contains CardAccounts and TransactionHistory tables

namespace _05_Transactional_ATM_Withdrawal
{
    using System;
    using System.Linq;

    public class AtmWithdraw
    {
        public static void Main()
        {
            var context = new ATMEntities();
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                try
                {
                    Console.Write("Enter card number: ");
                    string cardNumber = Console.ReadLine();
                    Console.Write("Enter card PIN: ");
                    string cardPin = Console.ReadLine();
                    Console.Write("Enter ammount to withdraw: ");
                    decimal amount = decimal.Parse(Console.ReadLine());

                    var cardAccount = context.CardAccounts
                        .FirstOrDefault(c => c.CardNumber == cardNumber);
                    if (cardAccount == null)
                    {
                        throw new InvalidOperationException("No such account!");
                    }

                    if (cardPin != cardAccount.CardPIN)
                    {
                        throw new InvalidOperationException("Pin does not match!");
                    }

                    if (amount > cardAccount.CardCash)
                    {
                        throw new InvalidOperationException("Not enough money in account!");
                    }

                    cardAccount.CardCash = cardAccount.CardCash - amount;
                    context.SaveChanges();

                    var history = new TransactionHistory
                    {
                        Amount = amount,
                        CardNumber = cardNumber,
                        TransactionDate = DateTime.Now
                    };

                    context.TransactionHistories.Add(history);
                    context.SaveChanges();

                    dbContextTransaction.Commit();
                    Console.WriteLine("Success!");
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine(ioe.Message);
                    dbContextTransaction.Rollback();
                }
            }
        }
    }
}
