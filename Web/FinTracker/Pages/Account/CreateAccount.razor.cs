using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace FinTracker.Pages.Account
{
    public partial class CreateAccount : BasePage
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        private AccountModel Account = new AccountModel();

        private List<string> AccountTypeList = Enum.GetNames(typeof(AccountType)).ToList();

        private async Task PostAccount(EditContext editContext)
        {
            if (!editContext.Validate())
            {
                return;
            }

            //Placeholder
            Account.UserId = new Guid("32B125C1-F3B0-4ECC-9F39-FCC7A9234918");

            var (model, urlLookupResult, statusCode) = await PostAccountDataAsync(Account);
            if (statusCode == HttpStatusCode.Created)
            {
                PageStatus = string.Empty;
                PageIsValid = true;
                await Toast.ShowSuccess("Account successfuly created");
                NavManager.NavigateTo("/Accounts");
            }
            else
            {
                PageStatus = urlLookupResult.Message;
                PageIsValid = false;
                await Toast.ShowError("failed to create Account");
            }

            StateHasChanged();
        }
    }
}
