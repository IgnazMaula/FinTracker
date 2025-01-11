using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FinTracker.Pages.Account
{
    public partial class AccountDetail : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }

        [Parameter] public Guid Id { get; set; }

        private AccountModel Account = new AccountModel();

        protected override async Task OnInitializedAsync() => await GetAccount();

        private async Task GetAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetAccountDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { Account = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }
    }
}
