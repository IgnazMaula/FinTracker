using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.Account
{
    public partial class Accounts : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private List<AccountModel> AccountList = new List<AccountModel>();

        public Guid SelectedId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAccounts();
            DataLoaded = true;
            StateHasChanged();
        }

        private async Task GetAccounts()
        {
            var (model, urlLookupResult, statusCode) = await GetAccountListDataAsync();
            if (statusCode == HttpStatusCode.OK) { AccountList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }


        private async void DeleteAccountHandler()
        {
            var result = AccountList.RemoveAll(Account => Account.Id == SelectedId);
            if (result != 0) { await Toast.ShowSuccess("Account successfuly deleted"); }
            else { await Toast.ShowError("Failed to delete data"); }
            await CloseModal("deleteModal");
            //await GetAccount();
            //StateHasChanged();
        }

        private void SetId(Guid Id)
        {
            SelectedId = Id;
        }

        private async Task CloseModal(string modalId)
        {
            await JS.InvokeVoidAsync("closeModal", modalId);
        }
    }
}
