using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // 1(a) Transaction record
    public record Transaction(
        int Id,
        DateTime Date,
        decimal Amount,
        string Category
    );

    // 1(b) Transaction processor interface
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // 1(c) Bank transfer processor
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Bank Transfer: GHS {transaction.Amount} processed for {transaction.Category}."
            );
        }
    }

    // 1(c) Mobile money processor
    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Mobile Money: GHS {transaction.Amount} processed for {transaction.Category}."
            );
        }
    }

    // 1(c) Crypto wallet processor
    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Crypto Wallet: GHS {transaction.Amount} processed for {transaction.Category}."
            );
        }
    }

    // 1(d) Base Account class
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;

            Console.WriteLine(
                $"Transaction applied. Current Balance: GHS {Balance}"
            );
        }
    }

    // 1(e) Sealed SavingsAccount class
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(
            string accountNumber,
            decimal initialBalance)
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                Balance -= transaction.Amount;

                Console.WriteLine(
                    $"Updated Balance: GHS {Balance}"
                );
            }
        }
    }

    // 1(f) Finance application
    public class FinanceApp
    {
        private List<Transaction> _transactions =
            new List<Transaction>();

        public void Run()
        {
            SavingsAccount account =
                new SavingsAccount("ACC1001", 1000m);

            Transaction transaction1 =
                new Transaction(
                    1,
                    DateTime.Now,
                    150m,
                    "Groceries"
                );

            Transaction transaction2 =
                new Transaction(
                    2,
                    DateTime.Now,
                    250m,
                    "Utilities"
                );

            Transaction transaction3 =
                new Transaction(
                    3,
                    DateTime.Now,
                    800m,
                    "Entertainment"
                );

            MobileMoneyProcessor mobileMoney =
                new MobileMoneyProcessor();

            BankTransferProcessor bankTransfer =
                new BankTransferProcessor();

            CryptoWalletProcessor cryptoWallet =
                new CryptoWalletProcessor();

            Console.WriteLine(
                "===== PROCESSING TRANSACTIONS ====="
            );

            Console.WriteLine();

            mobileMoney.Process(transaction1);
            account.ApplyTransaction(transaction1);
            _transactions.Add(transaction1);

            Console.WriteLine();

            bankTransfer.Process(transaction2);
            account.ApplyTransaction(transaction2);
            _transactions.Add(transaction2);

            Console.WriteLine();

            cryptoWallet.Process(transaction3);
            account.ApplyTransaction(transaction3);
            _transactions.Add(transaction3);

            Console.WriteLine();

            Console.WriteLine(
                "===== TRANSACTION SUMMARY ====="
            );

            foreach (Transaction transaction in _transactions)
            {
                Console.WriteLine(
                    $"ID: {transaction.Id} | " +
                    $"Date: {transaction.Date.ToShortDateString()} | " +
                    $"Category: {transaction.Category} | " +
                    $"Amount: GHS {transaction.Amount}"
                );
            }
        }
    }

    // Main method
    public class Program
    {
        public static void Main(string[] args)
        {
            FinanceApp app =
                new FinanceApp();

            app.Run();
        }
    }
}


