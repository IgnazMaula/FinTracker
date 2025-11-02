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

        protected List<string> AssetList = new List<string>
        {
            // Cryptocurrencies  
            "BTC-USD", "ETH-USD", "SOL-USD", "XRP-USD", "BNB-USD", "DOGE-USD",
            "ADA-USD", "DOT-USD", "MATIC-USD", "LTC-USD", "AVAX-USD", "SHIB-USD",
            "LINK-USD", "ATOM-USD", "UNI-USD", "XLM-USD", "TRX-USD", "WBTC-USD",  

            // Stocks - S&P 500 & Tech Giants  
            "SPY", "AAPL", "NVDA", "MSFT", "AMZN", "TSLA",
            "GOOGL", "META", "NFLX", "AMD", "BRK.B", "V",
            "JPM", "XOM", "UNH", "PG", "MA", "HD", "ABBV",
            "DIS", "PFE", "KO", "PEP", "INTC", "COST",  

            // Other High-Growth Stocks  
            "SNOW", "PLTR", "RBLX", "SQ", "SOFI", "UPST",
            "ARKK", "TSM", "SHOP", "CRWD", "NET", "DOCU",
        };


        protected List<string> PeriodList = new List<string>
        {
            "Monthly", "Weekly",
        };

        protected List<string> MonthList = new List<string>
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        #endregion

        #region Bank Account Helper Methods
        //-----------------------------------------------------------------
        // Bank Account Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<BankAccountModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankAccountListDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<BankAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankAccountUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankAccountDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<BankAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankAccountUrl"]}/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostBankAccountDataAsync(BankAccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<BankAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankAccountUrl"]}", request);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PutBankAccountDataAsync(Guid id, BankAccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PutEntity<BankAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankAccountUrl"]}", id, request);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> DeleteBankAccountDataAsync(Guid id)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.DeleteEntity<BankAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankAccountUrl"]}", id);
            return (model, urlLookupResult, statusCode);
        }
        #endregion

        #region Bank Transaction Helper Methods
        //-----------------------------------------------------------------
        // Bank Transaction Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<BankTransactionModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankTransactionListDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<BankTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankTransactionUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankTransactionModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankTransactionDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<BankTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankTransactionUrl"]}/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(List<BankTransactionModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBankTransactionByBankIdDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<BankTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankTransactionUrl"]}/ByBankId/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(List<MonthlyTransactionModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetMonthlyBankTransactionByUserIdDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<MonthlyTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankTransactionUrl"]}/GetMonthlyBankTransactionByUserId/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(BankTransactionModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostBankTransactionDataAsync(BankTransactionModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<BankTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankTransactionUrl"]}", request);
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
                var response = await httpClient.PostAsync($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankTransactionUrl"]}/Upload/{Id}", request.Content);
                var responseContent = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<FileUploadResultModel>(responseContent);
                statusCode = response.StatusCode;
                urlLookupResult.Message = response.RequestMessage != null ? response.RequestMessage.ToString() : "";
                urlLookupResult.Url = response.RequestMessage != null && response.RequestMessage.RequestUri != null ? response.RequestMessage.RequestUri.ToString() : $"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BankTransactionUrl"]}/Upload";
            }
            return (model, urlLookupResult, statusCode);
        }
        #endregion

        #region Investment Account Helper Methods
        //-----------------------------------------------------------------
        // Investment Account Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<InvestmentAccountModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetInvestmentAccountListDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<InvestmentAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentAccountUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(InvestmentAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetInvestmentAccountDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<InvestmentAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentAccountUrl"]}/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(InvestmentAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostInvestmentAccountDataAsync(InvestmentAccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<InvestmentAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentAccountUrl"]}", request);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(InvestmentAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PutInvestmentAccountDataAsync(Guid id, InvestmentAccountModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PutEntity<InvestmentAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentAccountUrl"]}", id, request);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(InvestmentAccountModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> DeleteInvestmentAccountDataAsync(Guid id)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.DeleteEntity<InvestmentAccountModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentAccountUrl"]}", id);
            return (model, urlLookupResult, statusCode);
        }
        #endregion

        #region Investment Transaction Helper Methods
        //-----------------------------------------------------------------
        // Investment Transaction Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<InvestmentTransactionModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetInvestmentTransactionListDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<InvestmentTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentTransactionUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(InvestmentTransactionModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetInvestmentTransactionDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<InvestmentTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentTransactionUrl"]}/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(List<InvestmentTransactionModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetInvestmentTransactionByInvestmentIdDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<InvestmentTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentTransactionUrl"]}/ByInvestmentId/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(List<MonthlyTransactionModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetMonthlyInvestmentTransactionByUserIdDataAsync(Guid id, string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<MonthlyTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentTransactionUrl"]}/GetMonthlyInvestmentTransactionByUserId/{id}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        protected async Task<(InvestmentTransactionModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> PostInvestmentTransactionDataAsync(InvestmentTransactionModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<InvestmentTransactionModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:InvestmentTransactionUrl"]}", request);
            return (model, urlLookupResult, statusCode);
        }
        #endregion

        #region DCA Calculator Helper Methods
        //-----------------------------------------------------------------
        // DCA Calculator Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<DCAResultModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> SubmitDCACalculatorDataAsync(DCACalculatorModel request)
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.PostEntity<DCACalculatorModel, List<DCAResultModel>>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:DCACalculatorUrl"]}", request);
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
                var response = await httpClient.PostAsync($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:FileUploadUrl"]}", request.Content);
                var responseContent = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<FileUploadResultModel>(responseContent);
                statusCode = response.StatusCode;
                urlLookupResult.Message = response.RequestMessage != null ? response.RequestMessage.ToString() : "";
                urlLookupResult.Url = response.RequestMessage != null && response.RequestMessage.RequestUri != null ? response.RequestMessage.RequestUri.ToString() : $"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:FileUploadUrl"]}";
            }
            return (model, urlLookupResult, statusCode);
        }
        #endregion

        #region Cryptocurrency Helper Methods
        //-----------------------------------------------------------------
        // Cryptocurrency Helper Methods
        //-----------------------------------------------------------------
        protected async Task<(List<CoinPriceModel> model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetCryptocurrencyListAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntityList<CoinPriceModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:CryptocurrencyUrl"]}", queryString);
            return (model, urlLookupResult, statusCode);
        }

        protected async Task<(BinancePortfolioModel model, RepositoryHelper.UrlLookupResult urlLookupResult, HttpStatusCode statusCode)> GetBinancePortfolioDataAsync(string queryString = "")
        {
            var (model, urlLookupResult, statusCode) = await RepositoryHelper.GetEntity<BinancePortfolioModel>($"{Configuration["UrlSettings:BaseUrl"]}{Configuration["UrlSettings:BinancePortfolioUrl"]}{queryString}");
            return (model, urlLookupResult, statusCode);
        }
        #endregion
    }
}
