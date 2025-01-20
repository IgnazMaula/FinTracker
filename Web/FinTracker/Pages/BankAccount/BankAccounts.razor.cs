using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.BankAccount
{
    public partial class BankAccounts : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private List<BankAccountModel> BankAccountList = new List<BankAccountModel>();

        public Guid SelectedId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetBankAccounts();
            DataLoaded = true;
            StateHasChanged();
        }

        private async Task GetBankAccounts()
        {
            var (model, urlLookupResult, statusCode) = await GetBankAccountListDataAsync();
            if (statusCode == HttpStatusCode.OK) { BankAccountList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }


        private async void DeleteBankAccountHandler()
        {
            var result = BankAccountList.RemoveAll(BankAccount => BankAccount.Id == SelectedId);
            if (result != 0) { await Toast.ShowSuccess("Bank Account successfuly deleted"); }
            else { await Toast.ShowError("Failed to delete data"); }
            await CloseModal("deleteModal");
            //await GetBankAccount();
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
