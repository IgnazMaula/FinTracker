using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.InvestmentAccount
{
    public partial class InvestmentAccounts : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private List<InvestmentAccountModel> InvestmentAccountList = new List<InvestmentAccountModel>();

        public Guid SelectedId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetInvestmentAccounts();
            DataLoaded = true;
            StateHasChanged();
        }

        private async Task GetInvestmentAccounts()
        {
            var (model, urlLookupResult, statusCode) = await GetInvestmentAccountListDataAsync();
            if (statusCode == HttpStatusCode.OK) { InvestmentAccountList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }


        private async void DeleteInvestmentAccountHandler()
        {
            var result = InvestmentAccountList.RemoveAll(InvestmentAccount => InvestmentAccount.Id == SelectedId);
            if (result != 0) { await Toast.ShowSuccess("Investment Account successfuly deleted"); }
            else { await Toast.ShowError("Failed to delete data"); }
            await CloseModal("deleteModal");
            //await GetInvestmentAccount();
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
