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
        private static async Task Main()
        {
            // Guide
            // http://www.macaalay.com/2016/07/06/scraping-website-data-that-needs-you-to-log-in/

            string[] lines = await File.ReadAllLinesAsync("Account.txt");

            ChromeDriver chromeDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory)
            {
                Url = "https://www.researchgate.net/login?_sg=lISBBaecxrbK5CzzqaJWG-BMVZPU_5beI-xo58qDceWN3vH9As11qGbZxs4Esd52bidNgJqEobs0gWQNJiN4YQ"
            };

            chromeDriver.Navigate();

            IWebElement element = chromeDriver.FindElement(By.Id("input-login"));
            element.SendKeys(lines[0]);

            element = chromeDriver.FindElement(By.Id("input-password"));
            element.SendKeys(lines[1]);

            element = chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div/div/div/div/form/div/div[4]/button"));
            element.Click();

            Thread.Sleep(5000);

            chromeDriver.Navigate().GoToUrl("https://www.researchgate.net/profile/James-Swan-3" + "/scores");

            // RG Score
            Console.WriteLine(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[2]/div/div/div[2]/div[1]/div/div[2]")).Text);

            // h-index
            Console.WriteLine(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[3]/div/div/div/div[2]/div[1]/div/div[2]")).Text);

            // h-index excluding self-citations
            Console.WriteLine(chromeDriver.FindElement(By.XPath("/html/body/div[1]/div[3]/div[1]/div/div/div[3]/div[1]/div/div/div[3]/div/div/div/div[2]/div[2]/div/div[2]")).Text);

            Thread.Sleep(5000);

            chromeDriver.Quit();

            Console.ReadKey(true);
        }
    }
}
