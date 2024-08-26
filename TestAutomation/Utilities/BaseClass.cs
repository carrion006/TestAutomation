using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


namespace TestAutomation.Utilities
{
    public class BaseClass
    {
        public IWebDriver driver;

        [SetUp]
        public void SetUpFixture()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = AppConfigReader.GetConfig("APP:URL");
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                Thread.Sleep(3000);
                driver.Dispose();
            }

        }
    }
}
