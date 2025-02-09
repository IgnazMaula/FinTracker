using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace FinTracker.Pages.BankAccount
{
    public partial class CreateBankAccount : BasePage
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        private BankAccountModel BankAccount = new BankAccountModel();

        private async Task PostBankAccount(EditContext editContext)
        {
            if (!editContext.Validate())
            {
                return;
            }

            //Placeholder
            BankAccount.UserId = new Guid("f7a3d8dd-70b1-4b98-be0c-219672025281");

            var (model, urlLookupResult, statusCode) = await PostBankAccountDataAsync(BankAccount);
            if (statusCode == HttpStatusCode.Created)
            {
                PageStatus = string.Empty;
                PageIsValid = true;
                await Toast.ShowSuccess("Bank Account successfuly created");
                NavManager.NavigateTo("/BankAccounts");
            }
            else
            {
                PageStatus = urlLookupResult.Message;
                PageIsValid = false;
                await Toast.ShowError("failed to create Bank Account");
            }

            StateHasChanged();
        }
    }
}
