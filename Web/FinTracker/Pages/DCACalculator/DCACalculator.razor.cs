using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace FinTracker.Pages.DCACalculator
{
    public partial class DCACalculator : BasePage
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        private DCACalculatorModel DCA = new DCACalculatorModel();
        private List<DCAResultModel> DCAResultList = new List<DCAResultModel>();

        private async Task SubmitDCACalculator(EditContext editContext)
        {
            if (!editContext.Validate())
            {
                return;
            }

            var (model, urlLookupResult, statusCode) = await SubmitDCACalculatorDataAsync(DCA);
            if (statusCode == HttpStatusCode.Created)
            {
                DCAResultList = model;
                PageStatus = string.Empty;
                PageIsValid = true;
            }
            else
            {
                PageStatus = urlLookupResult.Message;
                PageIsValid = false;
                await Toast.ShowError("failed to calculate DCA");
            }

            StateHasChanged();
        }
    }
}
