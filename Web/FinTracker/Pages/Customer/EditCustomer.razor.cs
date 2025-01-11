using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace FinTracker.Pages.Customer
{
    public partial class EditCustomer
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public int Id { get; set; }

        private CustomerModel Customer = new CustomerModel();
        private List<string> GenderList = new List<string> { "Male", "Female" };

        protected override async Task OnInitializedAsync() => await GetCustomer();

        private async Task GetCustomer()
        {
            var result = await HttpClient.GetFromJsonAsync<List<CustomerModel>>("sample-data/customer.json");
            if (result != null) { Customer = result.Where(w => w.Id == Id).FirstOrDefault(); }
            else { await Toast.ShowError("Failed to fetch data"); }
        }

        private async void EditCustomerHandler()
        {
            await Toast.ShowSuccess("Customer successfuly edited");
            NavManager.NavigateTo("Customers");
        }
    }
}
