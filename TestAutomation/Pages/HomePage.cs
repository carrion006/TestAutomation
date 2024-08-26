using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAutomation.Pages
{
    internal class HomePage : BasePage
    {

        public string Elements = "//div[contains(@class, 'mt-4')]//h5[text() = 'Elements']";

        public void ClickOnElementsButton(IWebDriver driver)
        {
            By ElementsLocator = By.XPath(Elements);
            IWebElement ElementsButton = WaitUntilElementExists(driver, ElementsLocator);
            ScrollTillElement(driver, ElementsButton);
            ElementsButton.Click();
        }
       
    }
}
