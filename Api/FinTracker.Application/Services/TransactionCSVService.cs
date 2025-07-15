using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FinTracker.Application.Interfaces;
using FinTracker.Common.Shared.Enums;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace FinTracker.Application.Services
{
    public class TransactionCSVService : ITransactionCSVService
    {
        private readonly IConfiguration _configuration;
        private readonly IBankTransactionRepository _bankTransactionRepository;
        private readonly IRepository<BankAccount> _bankAccountRepository;

        public TransactionCSVService(IConfiguration configuration, IBankTransactionRepository bankTransactionRepository, IRepository<BankAccount> bankAccountRepository)
        {
            _configuration = configuration;
            _bankTransactionRepository = bankTransactionRepository;
            _bankAccountRepository = bankAccountRepository;
        }

        public async Task ProcessCsvAsync(Stream csvStream, int year, Guid Id)
        {
            List<BankTransaction> transactions;

            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap(new BankTransactionMap(year));
                transactions = csv.GetRecords<BankTransaction>().ToList();
            }

            // Remove transactions that contain "SALDO AWAL"
            transactions.RemoveAll(transaction => transaction.Description.Contains("SALDO AWAL", StringComparison.OrdinalIgnoreCase));

            // Retrieve existing transactions from the database for the given bank account
            var existingTransactions = await _bankTransactionRepository.GetBankTransactionByBankIdAsync(Id);

            // Filter out duplicates (transactions with the same Date and Description)
            var uniqueTransactions = transactions
                .Where(newTransaction =>
                    !existingTransactions.Any(existing =>
                        existing.TransactionDate.Date == newTransaction.TransactionDate.Date &&
                        existing.Description.Trim().Equals(newTransaction.Description.Trim(), StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (!uniqueTransactions.Any())
                return;

            var bank = await _bankAccountRepository.GetByIdAsync(Id);

            foreach (var transaction in uniqueTransactions)
            {
                transaction.Id = Guid.NewGuid();
                transaction.BankAccount = bank;
                transaction.BankAccountId = Id;
                transaction.TransactionType = transaction.TransactionAmount < 0 ?
                    TransactionType.Debit.ToString() : TransactionType.Credit.ToString();
            }

            // Insert only unique transactions
            await _bankTransactionRepository.CreateMultipleAsync(uniqueTransactions);

            //Edit current balance based on the latest transaction
            var allTransactions = existingTransactions
            .Concat(uniqueTransactions)
            .OrderBy(t => t.TransactionDate)
            .ToList();

            bank.CurrentBalance = allTransactions.Last().Balance ?? 0;
            await _bankAccountRepository.UpdateAsync(bank);
        }
    }

    public sealed class BankTransactionMap : ClassMap<BankTransaction>
    {
        public BankTransactionMap(int year)
        {
            Map(m => m.TransactionDate)
                .Index(0)
                .TypeConverter(new TransactionDateConverter(year));
            Map(m => m.Description).Index(1);
            Map(m => m.TransactionAmount).Index(3).TypeConverterOption.NullValues(string.Empty);
            Map(m => m.Balance).Index(4).TypeConverterOption.NullValues(string.Empty);
        }
    }

    public class TransactionDateConverter : DefaultTypeConverter
    {
        private readonly int _year;

        public TransactionDateConverter(int year)
        {
            _year = year;
        }

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (DateTime.TryParseExact(
                    $"{text}/{_year}", // Append the year
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                return date;
            }

            throw new Exception($"Invalid date format: {text}. Expected format is 'dd/MM'.");
        }
    }
}
