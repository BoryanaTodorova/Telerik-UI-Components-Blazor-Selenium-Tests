using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace UIBlazorComponentsSeleniumTests
{
    public class GridAndMenuServerAppSeleniumUITests
    {
        public IWebDriver driver;
        public WebDriverWait wait;
        const string AppBaseUrl = "https://localhost:44351";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test_Grid_Paging()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/grid");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[@aria-label='2']")));

            try
            {
                var pageTwo = driver.FindElement(By.XPath("//a[@aria-label='2']"));
                pageTwo.Click();
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(1000);
                var pageTwo = driver.FindElement(By.XPath("//a[@aria-label='2']"));
                pageTwo.Click();
            }

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[1]")));
            var firstIdOnPageTwo = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[1]")).Text;

            var lastIdOnPageTwo = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[20]/td[1]")).Text;

            var itemsDisplayed = driver.FindElement(By.XPath("//div[@role='navigation']/span[.='21 - 40 of 150 items']")).Text;

            //Assert         
            Assert.AreEqual("21", firstIdOnPageTwo);
            Assert.AreEqual("40", lastIdOnPageTwo);
            Assert.AreEqual("21 - 40 of 150 items", itemsDisplayed);
        }

        [Test]
        public void Test_Grid_Sorting_Descending()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/grid");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//span[@class='k-column-title'][contains(.,'Id')]")));

            try
            {
                var tableHeaderId = driver.FindElement(By.XPath("//span[@class='k-column-title'][contains(.,'Id')]"));
                tableHeaderId.Click();
                tableHeaderId.Click();
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(500);
                var tableHeaderId = driver.FindElement(By.XPath("//span[@class='k-column-title'][contains(.,'Id')]"));
                tableHeaderId.Click();
                tableHeaderId.Click();
            }

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[1]")));
            var firstIdOnPageOne = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[1]"));

            //Assert         
            Assert.AreEqual("150", firstIdOnPageOne.Text);
        }

        [Test]
        public void Test_Grid_Filtering_By_Id()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/grid");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//thead/tr[1]/th[1]/div[1]/span[1]")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//thead/tr[1]/th[1]/div[1]/span[1]")));
            var idFilter = driver.FindElement(By.XPath("//thead/tr[1]/th[1]/div[1]/span[1]"));
            idFilter.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("(//input[contains(@aria-disabled,'false')])[1]")));
            var inputFieldIsEqualTo = driver.FindElement(By.XPath("(//input[contains(@aria-disabled,'false')])[1]"));
            inputFieldIsEqualTo.SendKeys("45");

            var buttonFilter = driver.FindElement(By.XPath("//button[@class='k-button k-primary'][contains(.,'Filter')]"));
            buttonFilter.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[1]")));
            var idOnPageOne = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[1]")).Text;

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@role='navigation']/span[.='1 - 1 of 1 items']")));
            var itemsDisplayed = driver.FindElement(By.XPath("//div[@role='navigation']/span[.='1 - 1 of 1 items']")).Text;

            //Assert         
            Assert.AreEqual("45", idOnPageOne);
            Assert.AreEqual("1 - 1 of 1 items", itemsDisplayed);
        }

        [Test]
        public void Test_Grid_Add_Forecast()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/grid");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@class='k-button telerik-blazor k-button-icontext k-primary'][contains(.,'Add Forecast')]")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[@class='k-button telerik-blazor k-button-icontext k-primary'][contains(.,'Add Forecast')]")));
            var buttonAddForecast = driver.FindElement(By.XPath("//button[@class='k-button telerik-blazor k-button-icontext k-primary'][contains(.,'Add Forecast')]"));
            buttonAddForecast.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//input[contains(@aria-haspopup,'true')]")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@aria-haspopup,'true')]")));
            var date = driver.FindElement(By.XPath("//input[contains(@aria-haspopup,'true')]"));
            date.SendKeys(Keys.Delete);
            date.SendKeys("1/1/2022");

            var tempC = driver.FindElement(By.XPath("(//input[contains(@aria-disabled,'false')])[2]"));
            tempC.SendKeys("15");

            var summary = driver.FindElement(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[5]/input"));
            summary.SendKeys("Chilly");

            var buttonUpdate = driver.FindElement(By.XPath("//button[@class='k-button telerik-blazor k-button-icontext'][contains(.,'Update')]"));
            buttonUpdate.Click();

            Thread.Sleep(3000);
            var firstIdOnPageOne = driver.FindElement(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[1]")).Text;
            var firstRowDate = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[2]")).Text;
            var firstRowTempC = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[3]")).Text;
            var firstRowTempF = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[4]")).Text;
            var firstRowSummary = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[5]")).Text;

            //Assert         
            Assert.AreEqual("151", firstIdOnPageOne);
            Assert.AreEqual("Saturday, 01 Jan 2022", firstRowDate);
            Assert.AreEqual("15.0", firstRowTempC);
            Assert.AreEqual("59.0", firstRowTempF);
            Assert.AreEqual("Chilly", firstRowSummary);
        }

        [Test]
        public void Test_Grid_Edit_Forecast()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/grid");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(.,'Edit')][1]")));

            try
            {
                var buttonEditForecastFirstRow = driver.FindElement(By.XPath("//button[contains(.,'Edit')][1]"));
                buttonEditForecastFirstRow.Click();
            }
            catch (StaleElementReferenceException)
            {
                var buttonEditForecastFirstRow = driver.FindElement(By.XPath("//button[contains(.,'Edit')][1]"));
                buttonEditForecastFirstRow.Click();
            }

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("(//input[contains(@aria-disabled,'false')])[2]")));
            var tempC = driver.FindElement(By.XPath("(//input[contains(@aria-disabled,'false')])[2]"));

            // selecting all text
            string s = Keys.Control + "a";
            tempC.SendKeys(s);
            // sending DELETE key
            tempC.SendKeys(Keys.Delete);
            tempC.SendKeys("30");

            var summary = driver.FindElement(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[5]/input"));
            summary.SendKeys(s);
            // sending DELETE key
            summary.SendKeys(Keys.Delete);
            summary.SendKeys("auto_test");

            var buttonUpdate = driver.FindElement(By.XPath("//button[@class='k-button telerik-blazor k-button-icontext'][contains(.,'Update')]"));
            buttonUpdate.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[3]")));
            var firstRowTempC = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[3]")).Text;
            var firstRowSummary = driver.FindElement(By.XPath("//tbody[@role='rowgroup']/tr[1]/td[5]")).Text;

            //Assert         
            Assert.AreEqual("30.0", firstRowTempC);
            Assert.AreEqual("auto_test", firstRowSummary);
        }

        [Test]
        public void Test_Grid_Delete_First_Forecast_In_Table()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/grid");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[1]")));
            var idForDelete = driver.FindElement(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[1]")).Text;

            var buttonDelete = driver.FindElement(By.XPath("(//button[contains(.,'Delete')])[1]"));
            buttonDelete.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[1]")));
            var firstIdInTableAfterDelete = driver.FindElement(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[1]")).Text;

            int attempts = 3;
            while (attempts <= 3)
            {
                if (!idForDelete.Equals(firstIdInTableAfterDelete))
                {
                    break;
                }
                Thread.Sleep(500);
                firstIdInTableAfterDelete = driver.FindElement(By.XPath("//table[@role='grid']/tbody[@role='rowgroup']/tr[1]/td[1]")).Text;
                attempts++;
            }

            //Assert         
            Assert.AreNotEqual(idForDelete, firstIdInTableAfterDelete);
        }

        [Test]
        public void Test_Chart_Min_Max_Value()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/chart");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("svg > g > g:nth-of-type(2) > g > text")));
            var chartTitle = driver.FindElement(By.CssSelector("svg > g > g:nth-of-type(2) > g > text"));
            string actualChartTitleText = chartTitle.Text;
            var expectedChartTitle = "Min and Max temperatures for the upcoming weeks";

            //Assert
            Assert.AreEqual(expectedChartTitle, actualChartTitleText);
        }

        [Test]
        public void Test_Form_Submit_Form()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/form");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("FNameTb")));
            var firstName = driver.FindElement(By.Id("FNameTb"));
            firstName.SendKeys("Boryana");

            var lastName = driver.FindElement(By.Id("LNameTb"));
            lastName.SendKeys("Todorova");

            var email = driver.FindElement(By.Id("EmailTb"));
            email.SendKeys("bptodorova@test.bg");

            var dropDownPrefferdTeam = driver.FindElement(By.Id("TeamDDL"));
            dropDownPrefferdTeam.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("(//div[contains(@class,'k-popup k-reset k-list-container')])[2]")));
            var phyton = driver.FindElement(By.XPath("(//div[contains(@class,'k-popup k-reset k-list-container')])[2]"));
            phyton.Click();

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(.,'Submit')]")));
            var buttonSubmit = driver.FindElement(By.XPath("//button[contains(.,'Submit')]"));
            buttonSubmit.Click();

            //Assert         
            Assert.AreEqual("", firstName.Text);
        }

        [Test]
        public void Test_Form_Cancel_Button()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/form");

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("FNameTb")));
            var firstName = driver.FindElement(By.Id("FNameTb"));
            firstName.SendKeys("Boryana");

            var lastName = driver.FindElement(By.Id("LNameTb"));
            lastName.SendKeys("Todorova");

            var email = driver.FindElement(By.Id("EmailTb"));
            email.SendKeys("bptodorova@test.bg");

            var dropDownPrefferdTeam = driver.FindElement(By.Id("TeamDDL"));
            dropDownPrefferdTeam.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("(//div[contains(@class,'k-popup k-reset k-list-container')])[2]")));
            var phyton = driver.FindElement(By.XPath("(//div[contains(@class,'k-popup k-reset k-list-container')])[2]"));
            phyton.Click();

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(.,'Submit')]")));
            var buttonCancel = driver.FindElement(By.XPath("//button[contains(.,'Cancel')]"));
            buttonCancel.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("FNameTb")));
            var actualFirstName = driver.FindElement(By.Id("FNameTb")).Text;
            var actualLastName = driver.FindElement(By.Id("LNameTb")).Text;
            var actualEmail = driver.FindElement(By.Id("EmailTb")).Text;
            var actualPreferredTeam = driver.FindElement(By.Id("TeamDDL")).Text;

            //Assert         
            Assert.AreEqual("", actualFirstName);
            Assert.AreEqual("", actualLastName);
            Assert.AreEqual("", actualEmail);
            Assert.AreEqual("Preferred team", actualPreferredTeam);
        }

        [Test]
        public void Test_Form_Required_Fields()
        {
            //Arrange
            driver.Navigate().GoToUrl(AppBaseUrl + "/form");
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("FNameTb")));

            try
            {
                var firstName = driver.FindElement(By.Id("FNameTb"));
                firstName.SendKeys("Boryana");
            }
            catch (StaleElementReferenceException)
            {
                var firstName = driver.FindElement(By.Id("FNameTb"));
                firstName.SendKeys("Boryana");
            }

            var lastName = driver.FindElement(By.Id("LNameTb"));
            lastName.SendKeys("Todorova");

            //Act
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(.,'Submit')]")));
            var buttonSubmit = driver.FindElement(By.XPath("//button[contains(.,'Submit')]"));
            buttonSubmit.Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//li[@class='validation-message'][contains(.,'An email is required')]")));
            var actualEmailValidationMessage = driver.FindElement(By.XPath("//li[@class='validation-message'][contains(.,'An email is required')]")).Text;
            var expectedEmailValidationMessage = "An email is required";

            var actualTeamValidationMessage = driver.FindElement(By.XPath("//li[@class='validation-message'][contains(.,'Choose the team and technology you want to work on')]")).Text;
            var expectedTeamValidationMessage = "Choose the team and technology you want to work on";

            //Assert      
            Assert.AreEqual(expectedEmailValidationMessage, actualEmailValidationMessage);
            Assert.AreEqual(expectedTeamValidationMessage, actualTeamValidationMessage);
        }

        [TearDown]
        public void ShutDown()
        {
            driver.Quit();
        }
    }
}