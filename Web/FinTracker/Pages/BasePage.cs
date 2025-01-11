using FinTracker.Common.Model;
using FinTracker.Utilities.Repository;
using Microsoft.AspNetCore.Components;
using System.Net;
using static FinTracker.Utilities.Repository.RepositoryHelper;

namespace FinTracker.Pages
{
    public class BasePage : ComponentBase
    {
        [Inject] private IConfiguration Configuration { get; set; }

        protected string PageStatus { get; set; } = "Loading page, please wait...";
        protected bool PageIsValid { get; set; } = true;

        #region Account Helper Methods
        //-----------------------------------------------------------------
        // Account Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<AccountModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetAccountListDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<AccountModel>($"{Configuration["BaseUrl"]}{Configuration["AccountUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }

        protected async Task<(AccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetAccountDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<AccountModel>($"{Configuration["BaseUrl"]}{Configuration["AccountUrl"]}/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }

        protected async Task<(AccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostAccountDataAsync(AccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<AccountModel>($"{Configuration["BaseUrl"]}{Configuration["AccountUrl"]}", request);
                return (model, urlLookupResult, statusCode);
        }

        protected async Task<(AccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PutAccountDataAsync(Guid id, AccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PutEntity<AccountModel>($"{Configuration["BaseUrl"]}{Configuration["AccountUrl"]}", id, request);
            return (model, urlLookupResult, statusCode);
        }

        protected async Task<(AccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> DeleteAccountDataAsync(Guid id)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.DeleteEntity<AccountModel>($"{Configuration["BaseUrl"]}{Configuration["AccountUrl"]}", id);
            return (model, urlLookupResult, statusCode);
        }
        #endregion
    }
}
