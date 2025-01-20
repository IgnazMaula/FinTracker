using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.BankTransaction
{
    public partial class BankTransactions : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private List<BankTransactionModel> BankTransactionList = new List<BankTransactionModel>();

        public Guid SelectedId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetBankTransactions();
            DataLoaded = true;
            StateHasChanged();
        }

        private async Task GetBankTransactions()
        {
            var (model, urlLookupResult, statusCode) = await GetBankTransactionListDataAsync();
            if (statusCode == HttpStatusCode.OK) { BankTransactionList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }


        private async void DeleteBankTransactionHandler()
        {
            var result = BankTransactionList.RemoveAll(BankTransaction => BankTransaction.Id == SelectedId);
            if (result != 0) { await Toast.ShowSuccess("Bank Transaction successfuly deleted"); }
            else { await Toast.ShowError("Failed to delete data"); }
            await CloseModal("deleteModal");
            //await GetBankTransaction();
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
