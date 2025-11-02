using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FinTracker.Pages.InvestmentAccount
{
    public partial class EditInvestmentAccount : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public Guid Id { get; set; }

        private InvestmentAccountModel InvestmentAccount = new InvestmentAccountModel();

        protected override async Task OnInitializedAsync() => await GetInvestmentAccount();

        private async Task GetInvestmentAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetInvestmentAccountDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { InvestmentAccount = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async void EditInvestmentAccountHandler()
        {
            await Toast.ShowSuccess("Investment Account successfuly edited");
            NavManager.NavigateTo("InvestmentAccounts");
        }
    }
}
