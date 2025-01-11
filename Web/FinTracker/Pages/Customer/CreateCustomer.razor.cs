using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;

namespace FinTracker.Pages.Customer
{
    public partial class CreateCustomer
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        private CustomerModel Customer = new CustomerModel();
        private List<string> GenderList = new List<string> { "Male", "Female" };

        private async void CreateCustomerHandler()
        {
            await Toast.ShowSuccess("Customer successfuly created");
            NavManager.NavigateTo("Customers");
        }
    }
}
