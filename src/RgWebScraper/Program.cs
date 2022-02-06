using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RgWebScraper
{
    public class Program
    {
        private static string email;
        private static string password;

        private static ChromeDriver chromeDriver;

        private static async Task GetAccountDetails()
        {
            string[] lines = await File.ReadAllLinesAsync("Account.txt");

            email = lines[0];
            password = lines[1];
        }

        private static void SetupDriver()
        {
            chromeDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory)
            {
                Url = "https://www.researchgate.net/login?_sg=lISBBaecxrbK5CzzqaJWG-BMVZPU_5beI-xo58qDceWN3vH9As11qGbZxs4Esd52bidNgJqEobs0gWQNJiN4YQ"
            };
        }

        private static void Login()
        {
            IWebElement element = chromeDriver.FindElement(By.Id("input-login"));
            element.SendKeys(email);

            element = chromeDriver.FindElement(By.Id("input-password"));
            element.SendKeys(password);

            element = chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div/div/div/div/form/div/div[4]/button"));
            element.Click();
        }

        private static async Task Main()
        {
            // Guide
            // http://www.macaalay.com/2016/07/06/scraping-website-data-that-needs-you-to-log-in/

            // Get the account details for login
            await GetAccountDetails();

            // Setup the browser driver
            SetupDriver();

            // Leave this here for now
            chromeDriver.Navigate();

            // Login to allow certain pages to show, only needs to happen once as the browser will remember the login
            // unless we somehow close the window
            Login();

            // Wait for a few seconds to prevent the bot check from finding the scraper
            // 5 seconds
            Thread.Sleep(5000);

            // --- Main loop should start here
            // Read in URLs and go through them here

            // Make sure to append the extra bit of the URL
            chromeDriver.Navigate().GoToUrl("https://www.researchgate.net/profile/James-Swan-3" + "/scores");

            // Should store these values
            // RG Score
            Console.WriteLine(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[2]/div/div/div[2]/div[1]/div/div[2]")).Text);

            // h-index
            Console.WriteLine(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[3]/div/div/div/div[2]/div[1]/div/div[2]")).Text);

            // h-index excluding self-citations
            Console.WriteLine(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[3]/div/div/div/div[2]/div[2]/div/div[2]")).Text);

            // Do this at the end of every read, 10 to 15 seconds should be good
            // Wait for a few seconds to prevent the bot check from finding the scraper
            // 5 seconds
            Thread.Sleep(5000);

            // --- Main loop ends here
            // Should write the results out to a .csv file

            // Close the driver when done
            chromeDriver.Quit();

            // Display alert message
            Console.WriteLine("Success");

            // Wait for input then close the program
            Console.ReadKey(true);
        }
    }
}
