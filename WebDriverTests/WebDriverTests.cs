using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace WebDriverTests
{
    public class WebDriverTests

    {
        //private const string url = "https://contactbook.nakov.repl.co/";
        private const string url = "http://localhost:8080/";
        private WebDriver driver;

        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void Close()
        {
            driver.Quit();
        }

        [Test]
        public void Test_ConatctList_FirstContact()
        {
            //Arrange
            this.driver.Navigate().GoToUrl(url);
            var viewContactsIcon = driver.FindElement(By.LinkText("Contacts"));

            //Act
            viewContactsIcon.Click();

            var contactTable = driver.FindElement(By.CssSelector("#contact1"));
            contactTable.Click();

            var idNumber = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.contactid > td")).Text;

            var firstName =driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td")).Text;
            
            

            //Assert

            Assert.That(idNumber, Is.EqualTo("1"));
            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));
        }
        [Test]
        public void TestSearch_ValidContact()
        {
            //Arrange
            this.driver.Navigate().GoToUrl(url);
            
            var searchContact = driver.FindElement(By.LinkText("Search"));

            //Act
            searchContact.Click();

            var searchFild = driver.FindElement(By.Id("keyword"));
            searchFild.Click();
            searchFild.SendKeys("albert");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

         
            var firstName = driver.FindElement(By.CssSelector("tbody > tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("tbody > tr.lname > td")).Text;

            //Assert

            Assert.That(firstName, Is.EqualTo("Albert"));
            Assert.That(lastName, Is.EqualTo("Einstein"));
        }
        [Test]
        public void TestSearch_InValidContact()
        {
            //Arrange
            this.driver.Navigate().GoToUrl(url);

            var searchContact = driver.FindElement(By.LinkText("Search"));

            //Act
            searchContact.Click();

            var searchFild = driver.FindElement(By.Id("keyword"));
            searchFild.Click();
            searchFild.SendKeys("invalid2635");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            var searchResult = driver.FindElement(By.Id("searchResult")).Text;

            //Assert

            Assert.That(searchResult, Is.EqualTo("No contacts found."));
           
        }
        [Test]
        public void TestAddContact_InvalidData()
        {
            //Arrange
            this.driver.Navigate().GoToUrl(url);

            var createContact = driver.FindElement(By.LinkText("Create"));

            //Act
            createContact.Click();

            var firstNameFild = driver.FindElement(By.Id("firstName"));
            firstNameFild.Click();
            firstNameFild.SendKeys("Ivan");

            var emailField = driver.FindElement(By.Id("email"));
            emailField.Click();
            emailField.SendKeys("aaa@Aaa.com");

            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();


            //Assert
            var createResult = driver.FindElement(By.CssSelector("body > main > div.err")).Text;
            Assert.That(createResult, Is.EqualTo("Error: Last name cannot be empty!"));

        }
        [Test]
        public void Test_CreateContacts_ValidData()
        {
            // Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var firstName = "FirstName" + DateTime.Now.Ticks;
            var lastName = "LastName" + DateTime.Now.Ticks;
            var email = DateTime.Now.Ticks + "gulia@abv.bg";
            var phone = "12345";

            // Act
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("phone")).SendKeys(phone);

            var buttonCreate = driver.FindElement(By.Id("create"));
            buttonCreate.Click();


            // Assert
            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();

            var firstNameLabel = lastContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastNameLabel = lastContact.FindElement(By.CssSelector("tr.lname > td")).Text;

            Assert.That(firstNameLabel, Is.EqualTo(firstName));
            Assert.That(lastNameLabel, Is.EqualTo(lastName));
        }
    }
}