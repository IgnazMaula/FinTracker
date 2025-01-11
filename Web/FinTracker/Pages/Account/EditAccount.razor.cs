using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace FinTracker.Pages.Account
{
    public partial class EditAccount : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public Guid Id { get; set; }

        private AccountModel Account = new AccountModel();
        private List<string> AccountTypeList = Enum.GetNames(typeof(AccountType)).ToList();

        protected override async Task OnInitializedAsync() => await GetAccount();

        private async Task GetAccount()
        {
            var result = await HttpClient.GetFromJsonAsync<List<AccountModel>>("sample-data/customer.json");
            if (result != null) { Account = result.Where(w => w.Id == Id).FirstOrDefault(); }
            else { await Toast.ShowError("Failed to fetch data"); }
        }

        private async void EditAccountHandler()
        {
            await Toast.ShowSuccess("Account successfuly edited");
            NavManager.NavigateTo("Accounts");
        }
    }
}
