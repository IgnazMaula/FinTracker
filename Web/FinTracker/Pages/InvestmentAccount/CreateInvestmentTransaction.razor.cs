using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using FinTracker.Common.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Net.Http.Headers;

namespace FinTracker.Pages.InvestmentAccount
{
    public partial class CreateInvestmentTransaction : BasePage
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public Guid Id { get; set; }

        //private string SelectedInvestment { get; set; }
        private InvestmentTransactionModel UploadInvestmentTransaction = new InvestmentTransactionModel();
        private FileUploadModel SelectedFile { get; set; } = new FileUploadModel();

        private InvestmentAccountModel InvestmentAccount = new InvestmentAccountModel();


        protected override async Task OnInitializedAsync()
        {
            await GetInvestmentAccount();
            StateHasChanged();
        }

        private async Task GetInvestmentAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetInvestmentAccountDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { InvestmentAccount = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task PostInvestmentTransaction(EditContext editContext)
        {
            if (!editContext.Validate())
            {
                return;
            }

            UploadInvestmentTransaction.InvestmentAccountId = Id;

            var (model, urlLookupResult, statusCode) = await PostTransactionCsvUpload(UploadInvestmentTransaction.InvestmentAccountId, SelectedFile);
            if (statusCode == HttpStatusCode.OK)
            {
                PageStatus = string.Empty;
                PageIsValid = true;
                await Toast.ShowSuccess("Investment Transaction successfuly added");
                NavManager.NavigateTo($"InvestmentAccountDetail/{Id}");
            }
            else
            {
                PageStatus = urlLookupResult.Message;
                PageIsValid = false;
                await Toast.ShowError("failed to add Investment Transaction");
            }

            StateHasChanged();
        }

        private void OnFileSelected(InputFileChangeEventArgs e)
        {
            var maxFileSize = 1024 * 5000; //5mb
            var fileContent = new StreamContent(e.File.OpenReadStream(maxFileSize));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(e.File.ContentType);

            SelectedFile.Content = new MultipartFormDataContent
            {
                { fileContent,
                    "\"file\"",
                    e.File.Name
                }
            };
        }
    }
}
