
using TestAutomation.Utilities;
using TestAutomation.Pages;

namespace TestAutomation.Tests
{
    internal class Test1 : BaseClass
    {
        HomePage homePage = new HomePage();
        [Test]
        public void TestElements() {
            homePage.ClickOnElementsButton(driver);    
        }

    }
}
