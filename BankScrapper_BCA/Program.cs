using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;

class BCAScraper
{
    static void Main()
    {
        var userId = "IGNAZMAU1003";
        var password = "100300";

        var options = new ChromeOptions();
        // options.AddArgument("--headless"); // Optional: run Chrome headless
        using var driver = new ChromeDriver(options);

        try
        {
            // 1. Open BCA website
            driver.Navigate().GoToUrl("https://ibank.klikbca.com/");

            // 2. Log in
            driver.FindElement(By.Name("txt_user_id")).SendKeys(userId);
            driver.FindElement(By.Name("txt_pswd")).SendKeys(password + Keys.Enter);
            Thread.Sleep(2000);

            // 3. Navigate to Informasi Rekening → Mutasi Rekening
            driver.SwitchTo().Frame("menu");
            driver.FindElement(By.LinkText("Informasi Rekening")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Mutasi Rekening")).Click();

            // 4. Back to main frame
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame("atm");
            Thread.Sleep(1000);

            // 5. Select previous month
            int currentMonth = DateTime.Now.Month;
            int previousMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            var monthSelect = new SelectElement(driver.FindElement(By.Name("value(startMt)")));
            monthSelect.SelectByValue(previousMonth.ToString());

            // 6. Click "Tampilkan" button
            driver.FindElement(By.Name("value(submit1)")).Click();
            Thread.Sleep(3000);

            // 7. Extract transactions
            var rows = driver.FindElements(By.XPath("//table//tr[td]"));

            Console.WriteLine("-------------------------");
            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count == 6)
                {
                    string date = cells[0].Text.Trim();
                    string description = cells[1].Text.Trim().Replace("\r\n", " ").Replace("\n", " ");
                    string amount = cells[3].Text.Trim();
                    string type = cells[4].Text.Trim();
                    string balance = cells[5].Text.Trim();

                    Console.WriteLine("Date: " + date);
                    Console.WriteLine("Description: " + description);
                    Console.WriteLine("Amount: " + amount);
                    Console.WriteLine("Type: " + type);
                    Console.WriteLine("Balance: " + balance);
                    Console.WriteLine("-------------------------");
                }
            }

            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame("header");

            var logoutLink = driver.FindElement(By.XPath("//a[contains(text(), '[ LOGOUT ]')]"));
            logoutLink.Click();
            Thread.Sleep(1000);
            Console.WriteLine("Logged out successfully.");

            driver.Quit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
            driver.Quit();
        }
    }
}
