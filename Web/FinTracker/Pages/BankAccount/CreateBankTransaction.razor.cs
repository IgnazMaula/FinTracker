using FinTracker.Common.Enums;
using FinTracker.Common.Model;
using FinTracker.Common.Shared.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Net.Http.Headers;

namespace FinTracker.Pages.BankAccount
{
    public partial class CreateBankTransaction : BasePage
    {
        [Inject] private ToastService Toast { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        [Parameter] public Guid Id { get; set; }

        //private string SelectedBank { get; set; }
        private BankTransactionModel UploadBankTransaction = new BankTransactionModel();
        private FileUploadModel SelectedFile { get; set; } = new FileUploadModel();

        private BankAccountModel BankAccount = new BankAccountModel();


        protected override async Task OnInitializedAsync()
        {
            await GetBankAccount();
            StateHasChanged();
        }

        private async Task GetBankAccount()
        {
            var (model, urlLookupResult, statusCode) = await GetBankAccountDataAsync(Id);
            if (statusCode == HttpStatusCode.OK) { BankAccount = model; PageStatus = string.Empty; PageIsValid = true; }
            else { PageStatus = urlLookupResult.Message; ; PageIsValid = false; }
            StateHasChanged();
        }

        private async Task PostBankTransaction(EditContext editContext)
        {
            if (!editContext.Validate())
            {
                return;
            }

            UploadBankTransaction.BankAccountId = Id;

            var (model, urlLookupResult, statusCode) = await PostTransactionCsvUpload(UploadBankTransaction.BankAccountId, SelectedFile);
            if (statusCode == HttpStatusCode.OK)
            {
                PageStatus = string.Empty;
                PageIsValid = true;
                await Toast.ShowSuccess("Bank Transaction successfuly added");
                NavManager.NavigateTo($"BankAccountDetail/{Id}");
            }
            else
            {
                PageStatus = urlLookupResult.Message;
                PageIsValid = false;
                await Toast.ShowError("failed to add Bank Transaction");
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
