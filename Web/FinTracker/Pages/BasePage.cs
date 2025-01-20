using FinTracker.Common.Model;
using FinTracker.Common.Shared.Model;
using FinTracker.Utilities.ApiSecurity.Helper;
using FinTracker.Utilities.Repository;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net;
using static FinTracker.Utilities.Repository.RepositoryHelper;

namespace FinTracker.Pages
{
    public class BasePage : ComponentBase
    {
        [Inject] private IConfiguration Configuration { get; set; }

        protected string PageStatus { get; set; } = "Loading page, please wait...";
        protected bool PageIsValid { get; set; } = true;

        #region Dropdown Content
        protected List<string> TransactionTypeList = new List<string>
        {
            "Bank","Cryptocurrency","Mutual Funds"
        };

        protected List<string> CurrencyList = new List<string>
        {
            "IDR", "USD", "EUR", "JPY", "GBP", "AUD", 
            "CAD", "CNY", "SGD", "HKD", "KRW", "RUB",
        };
        #endregion

        #region Bank Account Helper Methods
        //-----------------------------------------------------------------
        // Bank Account Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<BankAccountModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankAccountListDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<BankAccountModel>($"{Configuration["BaseUrl"]}{Configuration["BankAccountUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankAccountDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<BankAccountModel>($"{Configuration["BaseUrl"]}{Configuration["BankAccountUrl"]}/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostBankAccountDataAsync(BankAccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<BankAccountModel>($"{Configuration["BaseUrl"]}{Configuration["BankAccountUrl"]}", request);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PutBankAccountDataAsync(Guid id, BankAccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PutEntity<BankAccountModel>($"{Configuration["BaseUrl"]}{Configuration["BankAccountUrl"]}", id, request);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> DeleteBankAccountDataAsync(Guid id)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.DeleteEntity<BankAccountModel>($"{Configuration["BaseUrl"]}{Configuration["BankAccountUrl"]}", id);
            return (model, urlLookupResult, statusCode);
        }
        #endregion

        #region Bank Transaction Helper Methods
        //-----------------------------------------------------------------
        // Bank Transaction Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<BankTransactionModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankTransactionListDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<BankTransactionModel>($"{Configuration["BaseUrl"]}{Configuration["BankTransactionUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankTransactionModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankTransactionDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<BankTransactionModel>($"{Configuration["BaseUrl"]}{Configuration["BankTransactionUrl"]}/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankTransactionModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostBankTransactionDataAsync(BankTransactionModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<BankTransactionModel>($"{Configuration["BaseUrl"]}{Configuration["BankTransactionUrl"]}", request);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(FileUploadResultModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostTransactionCsvUpload(Guid Id, FileUploadModel request)
        {
            var model = new FileUploadResultModel();
            var urlLookupResult = new UrlLookupResult();
            var statusCode = HttpStatusCode.RequestTimeout;
            //var accessToken = await ApiSecurityHelper.GetAuthApiTokenAsync($"{AppSettingsManager.Instance.GetUrlSettings().IdentityServerUrl}");
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var response = await httpClient.PostAsync($"{Configuration["BaseUrl"]}{Configuration["BankTransactionUrl"]}/Upload/{Id}", request.Content);
                var responseContent = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<FileUploadResultModel>(responseContent);
                statusCode = response.StatusCode;
                urlLookupResult.Message = response.RequestMessage != null ? response.RequestMessage.ToString() : "";
                urlLookupResult.Url = response.RequestMessage != null && response.RequestMessage.RequestUri != null ? response.RequestMessage.RequestUri.ToString() : $"{Configuration["BaseUrl"]}{Configuration["BankTransactionUrl"]}/Upload";
            }
            return (model, urlLookupResult, statusCode);
        }
        #endregion

        #region File Upload Helper Methods
        //-----------------------------------------------------------------
        // File Upload Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(FileUploadResultModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostFileUpload(FileUploadModel request)
        {
            var model = new FileUploadResultModel();
            var urlLookupResult = new UrlLookupResult();
            var statusCode = HttpStatusCode.RequestTimeout;
            //var accessToken = await ApiSecurityHelper.GetAuthApiTokenAsync($"{AppSettingsManager.Instance.GetUrlSettings().IdentityServerUrl}");
            using (var httpClient = new HttpClient())
            {
                //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                var response = await httpClient.PostAsync($"{Configuration["BaseUrl"]}{Configuration["FileUploadUrl"]}", request.Content);
                var responseContent = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<FileUploadResultModel>(responseContent);
                statusCode = response.StatusCode;
                urlLookupResult.Message = response.RequestMessage != null ? response.RequestMessage.ToString() : "";
                urlLookupResult.Url = response.RequestMessage != null && response.RequestMessage.RequestUri != null ? response.RequestMessage.RequestUri.ToString() : $"{Configuration["BaseUrl"]}{Configuration["FileUploadUrl"]}";
            }
            return (model, urlLookupResult, statusCode);
        }
        #endregion
    }
}
