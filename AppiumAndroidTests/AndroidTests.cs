using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;

namespace AppiumAndroidTests
{
    public class AndroidTests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactsBookUrl = "https://contactbook.nakov.repl.co/api";
        private const string appLocation = @"C:\Work\contactbook-androidclient.apk";

        private AndroidDriver<AndroidElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void StartApp()
        {
            options = new AppiumOptions() { PlatformName = "Android" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [TearDown]
        public void CloseApp()
        {
            driver.Quit();
        }

        [Test]
        public void Test_SearchContact_VerifyFirstResult()
        {
            //Arrange
            var contactBookApi = driver.FindElement(By.Id("contactbook.androidclient:id/editTextApiUrl"));
            contactBookApi.Clear();
            contactBookApi.SendKeys(ContactsBookUrl);
            
            //Act
            driver.FindElement(By.Id("contactbook.androidclient:id/buttonConnect")).Click(); ;
           
            var SearchField = driver.FindElement(By.Id("contactbook.androidclient:id/editTextKeyword"));
            SearchField.Click();
            SearchField.SendKeys("steve");
            var searchButton = driver.FindElement(By.Id("contactbook.androidclient:id/buttonSearch"));
            searchButton.Click();
            
            
            //Assert
            var firstName = driver.FindElement(By.Id("contactbook.androidclient:id/textViewFirstName")).Text;
            var lastName = driver.FindElement(By.Id("contactbook.androidclient:id/textViewLastName")).Text;

            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));

        }
    }
}