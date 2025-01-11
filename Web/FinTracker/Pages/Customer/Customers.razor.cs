using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Json;

namespace FinTracker.Pages.Customer
{
    public partial class Customers
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IJSRuntime JS { get; set; }
        [Inject] private ToastService Toast { get; set; }

        public bool DataLoaded { get; set; }
        private List<CustomerModel> CustomerList = new List<CustomerModel>();

        public int SelectedId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetCustomers();
            DataLoaded = true;
            StateHasChanged();
        }

        private async Task GetCustomers()
        {
            var result = await HttpClient.GetFromJsonAsync<List<CustomerModel>>("sample-data/customer.json");
            if (result != null) { CustomerList = result; }
            else { await Toast.ShowError("Failed to fetch data"); }
        }

        private async void DeleteCustomerHandler()
        {
            var result = CustomerList.RemoveAll(Customer => Customer.Id == SelectedId);
            if (result != 0) { await Toast.ShowSuccess("Customer successfuly deleted"); }
            else { await Toast.ShowError("Failed to delete data"); }
            await CloseModal("deleteModal");
            //await GetCustomer();
            //StateHasChanged();
        }

        private void SetId(int Id)
        {
            SelectedId = Id;
        }

        private async Task CloseModal(string modalId)
        {
            await JS.InvokeVoidAsync("closeModal", modalId);
        }
    }
}
