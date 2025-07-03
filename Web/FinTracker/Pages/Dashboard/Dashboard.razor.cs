using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.Dashboard
{
    public partial class Dashboard : BasePage
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private DashboardModel DashboardData = new DashboardModel();
        private BankAccountModel BankAccount = new BankAccountModel();
        private List<CustomerModel> CustomerList = new List<CustomerModel>();
        private List<BankTransactionModel> BankTransactionList = new List<BankTransactionModel>();
        private List<CoinPriceModel> CoinPriceList = new List<CoinPriceModel>();
        private List<MonthlyTransactionModel> BankMonthlyTransaction = new List<MonthlyTransactionModel>();

        //Chart
        private int ChartIndex = -1;
        public double[] ChartData = new double[3];
        public string[] ChartLabels = { "Saving", "Investment", "Reserve" };

        protected override async Task OnInitializedAsync()
        {
            await GetMonthlyTransactionByUserId();
            await GetBankTransactions();
            await GetBankAccount();
            await GetDashboardData();
            await GetCryptoCurrencyList();
            //await GetCustomer();

            DataLoaded = true;
        }

        private async Task GetDashboardData()
        {
            var result = await HttpClient.GetFromJsonAsync<DashboardModel>("sample-data/dashboard.json");
            if (result != null)
            {
                DashboardData = result;

                ChartData[0] = DashboardData.CustomerCount;
                ChartData[1] = DashboardData.ProductCount;
                ChartData[2] = DashboardData.LocationCount;
            }
            else { await Toast.ShowError("Failed to fetch data"); }
        }

        //private async Task GetCustomer()
        //{
        //    var result = await HttpClient.GetFromJsonAsync<List<CustomerModel>>("sample-data/customer.json");
        //    if (result != null) { CustomerList = result; }
        //    else { await Toast.ShowError("Failed to fetch data"); }
        //}

        private async Task GetBankAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetBankAccountDataAsync(new Guid("a9134c3f-b36d-4c31-9dc1-6b56884e2382"));
            if (statusCode == HttpStatusCode.OK) { BankAccount = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task GetBankTransactions()
        {
            var (model, urlLookupResult, statusCode) = await GetBankTransactionListDataAsync();
            if (statusCode == HttpStatusCode.OK) { BankTransactionList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task GetMonthlyTransactionByUserId()
        {
            var (model, urlLookupResult, statusCode) = await GetMonthlyBankTransactionByUserIdDataAsync(new Guid("f7a3d8dd-70b1-4b98-be0c-219672025281"));
            if (statusCode == HttpStatusCode.OK) { BankMonthlyTransaction = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task GetCryptoCurrencyList()
        {
            var (model, urlLookupResult, statusCode) = await GetCryptocurrencyListAsync();
            if (statusCode == HttpStatusCode.OK) { CoinPriceList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }
    }
}
