using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using FinTracker.Common.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Net.Http.Headers;

namespace FinTracker.Pages.BankTransaction
{
    public partial class CreateBankTransaction : BasePage
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        private string SelectedBank { get; set; }
        private BankTransactionModel UploadBankTransaction = new BankTransactionModel();
        private FileUploadModel SelectedFile { get; set; } = new FileUploadModel();

        private List<BankAccountModel> BankAccountList = new List<BankAccountModel>();


        protected override async Task OnInitializedAsync()
        {
            await GetBankAccounts();
            StateHasChanged();
        }

        private async Task GetBankAccounts()
        {
            var (model, urlLookupResult, statusCode) = await GetBankAccountListDataAsync();
            if (statusCode == HttpStatusCode.OK) { BankAccountList = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task PostBankTransaction(EditContext editContext)
        {
            if (!editContext.Validate())
            {
                return;
            }

            UploadBankTransaction.BankAccountId = new Guid(SelectedBank);

            var (model, urlLookupResult, statusCode) = await PostTransactionCsvUpload(UploadBankTransaction.BankAccountId, SelectedFile);
            if (statusCode == HttpStatusCode.Created)
            {
                PageStatus = string.Empty;
                PageIsValid = true;
                await Toast.ShowSuccess("Bank Transaction successfuly created");
                NavManager.NavigateTo("/BankTransactions");
            }
            else
            {
                PageStatus = urlLookupResult.Message;
                PageIsValid = false;
                await Toast.ShowError("failed to create Bank Transaction");
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
