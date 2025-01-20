using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FinTracker.Pages.BankTransaction
{
    public partial class BankTransactionDetail : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }

        [Parameter] public Guid Id { get; set; }

        private BankTransactionModel BankTransaction = new BankTransactionModel();

        protected override async Task OnInitializedAsync() => await GetBankTransaction();

        private async Task GetBankTransaction()
        {
            var (model, urlLookupResult, statusCode) = await GetBankTransactionDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { BankTransaction = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }
    }
}
