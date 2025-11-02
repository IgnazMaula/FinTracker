using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FinTracker.Pages.InvestmentAccount
{
    public partial class InvestmentAccountDetail : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }

        [Parameter] public Guid Id { get; set; }

        private InvestmentAccountModel InvestmentAccount = new InvestmentAccountModel();

        public bool DataLoaded { get; set; }
        private List<InvestmentTransactionModel> InvestmentTransactionList = new List<InvestmentTransactionModel>();

        protected override async Task OnInitializedAsync()
        {
            await GetInvestmentAccount();
            await GetInvestmentTransactions();
            DataLoaded = true;
            StateHasChanged();
        }

        private async Task GetInvestmentAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetInvestmentAccountDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { InvestmentAccount = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task GetInvestmentTransactions()
        {
            var (model, urlLookupResult, statusCode) = await GetInvestmentTransactionByInvestmentIdDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { InvestmentTransactionList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }
    }
}
