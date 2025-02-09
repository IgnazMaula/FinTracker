using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FinTracker.Pages.BankAccount
{
    public partial class BankAccountDetail : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }

        [Parameter] public Guid Id { get; set; }

        private BankAccountModel BankAccount = new BankAccountModel();

        public bool DataLoaded { get; set; }
        private List<BankTransactionModel> BankTransactionList = new List<BankTransactionModel>();

        protected override async Task OnInitializedAsync()
        {
            await GetBankAccount();
            await GetBankTransactions();
            DataLoaded = true;
            StateHasChanged();
        }

        private async Task GetBankAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetBankAccountDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { BankAccount = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task GetBankTransactions()
        {
            var (model, urlLookupResult, statusCode) = await GetBankTransactionByBankIdDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { BankTransactionList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }
    }
}
