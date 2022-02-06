using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RgWebScraper
{
    public class Program
    {
        private static string email;
        private static string password;

        private static ChromeDriver chromeDriver;

        private static List<Page> pages;
        // This list will need to be able to accept new users as the program runs
        private static List<User> users = new List<User>();

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

        private static void GetPages()
        {
            // Remember to add file and header otherwise an exception will be thrown
            using (StreamReader streamReader = new StreamReader("pages.csv"))
            {
                using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    pages = csvReader.GetRecords<Page>().ToList();
                }
            }
        }

        private static async Task Main()
        {
            // Guide
            // http://www.macaalay.com/2016/07/06/scraping-website-data-that-needs-you-to-log-in/

            // Get the links of the pages
            GetPages();

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

            foreach (var page in pages)
            {
                User user = new User();

                // Make sure to append the extra bit of the URL
                chromeDriver.Navigate().GoToUrl(page.Url + "/scores");

                // Should store these values
                // Name and surname
                user.NameAndSurname = chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[1]/div[2]/div/div/div/div[2]/div/div[1]/div")).Text;

                // RG Score
                user.Score = float.Parse(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[2]/div/div/div[2]/div[1]/div/div[2]")).Text);

                // h-index
                user.HIndex = int.Parse(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[3]/div/div/div/div[2]/div[1]/div/div[2]")).Text);

                // h-index excluding self-citations
                user.HIndexExcluding = int.Parse(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[3]/div/div/div/div[2]/div[2]/div/div[2]")).Text);

                // Add new user to the list of other users, holds all their data
                users.Add(user);

                // Do this at the end of every read, 10 to 15 seconds should be good
                // Wait for a few seconds to prevent the bot check from finding the scraper
                // 5 seconds
                Thread.Sleep(10000);
            }

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
