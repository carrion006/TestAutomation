using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomation.Utilities;

namespace TestAutomation.Pages
{
    internal class ElementsPage : BasePage
    {
        public string webTablesButton = "//span[contains(text(),'Web')]/ancestor::li";
        public string webTable = "//h1[contains(text(),'Web Tables')]/parent::div";
        public string inputButton = "//input[contains(@placeholder,'search')]";
        public string noRowElement = "//div[contains(text(),'No rows')]";
        internal void ClickOnWebTables(IWebDriver driver)
        {
            By webTableLocator = By.XPath(webTablesButton);
            IWebElement webTablesElement = WaitUntilElementExists(driver, webTableLocator);
            webTablesElement.Click();
        }

        internal void ValidateNoDataWebTables(IWebDriver driver) {
            By webTableBodyLocator = By.XPath(webTable);
            IWebElement inputElement = FindElement(driver, inputButton);
            ScrollTillElement(driver, inputElement);
            inputElement.SendKeys(AppConfigReader.GetConfig("Data:TestData1"));
            By noRowLocator = By.XPath(noRowElement);
            IWebElement noRowText = WaitUntilElementExists(driver, noRowLocator);
            ScrollTillElement(driver, noRowText);
            AssertElementContainsText(noRowText, AppConfigReader.GetConfig("Data:NoSearchResultTable"));
        }
    }
}
