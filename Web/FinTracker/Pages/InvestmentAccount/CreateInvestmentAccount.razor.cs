using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace FinTracker.Pages.InvestmentAccount
{
    public partial class CreateInvestmentAccount : BasePage
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        private InvestmentAccountModel InvestmentAccount = new InvestmentAccountModel();

        private async Task PostInvestmentAccount(EditContext editContext)
        {
            if (!editContext.Validate())
            {
                return;
            }

            //Placeholder
            InvestmentAccount.UserId = new Guid("f7a3d8dd-70b1-4b98-be0c-219672025281");

            var (model, urlLookupResult, statusCode) = await PostInvestmentAccountDataAsync(InvestmentAccount);
            if (statusCode == HttpStatusCode.Created)
            {
                PageStatus = string.Empty;
                PageIsValid = true;
                await Toast.ShowSuccess("Investment Account successfuly created");
                NavManager.NavigateTo("/InvestmentAccounts");
            }
            else
            {
                PageStatus = urlLookupResult.Message;
                PageIsValid = false;
                await Toast.ShowError("failed to create Investment Account");
            }

            StateHasChanged();
        }
    }
}
