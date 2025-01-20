using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinTracker.Application.Interfaces;
using FinTracker.Domain.Entities;
using FinTracker.Domain.Interfaces;
using FinTracker.Application.Models;
using System.Formats.Asn1;
using System.Globalization;
using System.Transactions;
using CsvHelper;
using CsvHelper.Configuration;
using FinTracker.Common.Shared.Enums;

namespace FinTracker.Application.Services
{
    public class TransactionCSVService : ITransactionCSVService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<BankTransaction> _repository;

        public TransactionCSVService(IConfiguration configuration, IRepository<BankTransaction> repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task ProcessCsvAsync(Stream csvStream, Guid Id)
        {
            List<BankTransaction> transactions;

            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BankTransactionMap>();
                transactions = csv.GetRecords<BankTransaction>().ToList();
            }

            transactions.RemoveAll(transaction => transaction.Description.Contains("SALDO AWAL"));
            foreach (var transaction in transactions)
            {
                transaction.BankAccountId = Id;
                transaction.TransactionType = transaction.TransactionAmount < 0 ? TransactionType.Debit.ToString() : TransactionType.Credit.ToString();
            }

            await _repository.CreateMultipleAsync(transactions);
        }
    }

    public sealed class BankTransactionMap : ClassMap<BankTransaction>
    {
        public BankTransactionMap()
        {
            Map(m => m.TransactionDate).Index(0).TypeConverterOption.Format("dd/MM");
            Map(m => m.Description).Index(1);
            Map(m => m.TransactionAmount).Index(3).TypeConverterOption.NullValues(string.Empty);
        }
    }
}
