
using TestAutomation.Utilities;
using TestAutomation.Pages;

namespace TestAutomation.Tests
{
    internal class Test1 : BaseClass
    {
        HomePage homePage = new HomePage();
        ElementsPage elementsPage = new ElementsPage();
        [Test]
        public void TestElements() {
            homePage.ClickOnElementsButton(driver);
            elementsPage.ClickOnWebTables(driver);
            elementsPage.ValidateNoDataWebTables(driver);
        }

    }
}
