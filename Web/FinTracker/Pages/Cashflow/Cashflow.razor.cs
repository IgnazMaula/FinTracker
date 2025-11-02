using FinTracker.Common.Model;
using FinTracker.Common.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.Cashflow
{
    public partial class Cashflow : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private DashboardModel DashboardData = new DashboardModel();
        private BinancePortfolioModel BinancePortfolio = new BinancePortfolioModel();


        private string SelectedMarket = "cryptocurrency";


        //Chart
        private int ChartIndex = -1;
        public double[] ChartData = new double[3];
        public string[] ChartLabels = { "Saving", "Cashflow", "Reserve" };

        protected override async Task OnInitializedAsync()
        {
            await GetBinancePortfolio();
            DataLoaded = true;
        }

        private async Task GetBinancePortfolio()
        {
            var (model, urlLookupResult, statusCode) = await GetBinancePortfolioDataAsync();
            if (statusCode == HttpStatusCode.OK) { BinancePortfolio = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private void OnMarketTypeChanged(ChangeEventArgs e)
        {
            SelectedMarket = e.Value?.ToString() ?? "cryptocurrency";
        }
    }
}
