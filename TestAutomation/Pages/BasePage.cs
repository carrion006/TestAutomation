using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomation.Utilities;

namespace TestAutomation.Pages
{
    public class BasePage : BaseClass
    {
        public IWebElement WaitUntilElementExists(IWebDriver driver, By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(8));
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
        }

        public void ScrollTillElement(IWebDriver driver, IWebElement element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public IWebElement FindElement(IWebDriver driver, string element)
        {
            return driver.FindElement(By.XPath(element));
        }
        public void AssertElementContainsText(IWebElement element, string expectedText)
        {
            string actualText = element.Text;
            Assert.That(actualText, Does.Contain(expectedText));
            TestContext.Progress.WriteLine("Assertion Passed");
        }
    }
}
