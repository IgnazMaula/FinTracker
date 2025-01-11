using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.Dashboard
{
    public partial class Dashboard
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private DashboardModel DashboardData = new DashboardModel();
        private List<CustomerModel> CustomerList = new List<CustomerModel>();

        //Chart
        private int ChartIndex = -1;
        public double[] ChartData = new double[3];
        public string[] ChartLabels = { "Customer", "Product", "Order" };

        protected override async Task OnInitializedAsync()
        {
            await GetDashboardData();
            await GetCustomer();

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

        private async Task GetCustomer()
        {
            var result = await HttpClient.GetFromJsonAsync<List<CustomerModel>>("sample-data/customer.json");
            if (result != null) { CustomerList = result; }
            else { await Toast.ShowError("Failed to fetch data"); }
        }
    }
}
