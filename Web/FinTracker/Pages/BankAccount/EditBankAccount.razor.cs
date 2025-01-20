using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace FinTracker.Pages.BankAccount
{
    public partial class EditBankAccount : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public Guid Id { get; set; }

        private BankAccountModel BankAccount = new BankAccountModel();

        protected override async Task OnInitializedAsync() => await GetBankAccount();

        private async Task GetBankAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetBankAccountDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { BankAccount = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async void EditBankAccountHandler()
        {
            await Toast.ShowSuccess("Bank Account successfuly edited");
            NavManager.NavigateTo("BankAccounts");
        }
    }
}
