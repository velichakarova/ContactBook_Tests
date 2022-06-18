using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;



namespace ApiTests
{
    public class ApiTests
    {
        private const string url = "http://localhost:8080/api/contacts";
       // private const string url = "https://contactbook.nakov.repl.co/api/contacts";
        private RestClient client;
        private RestRequest request;

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_GetAllClients_CheckFirstClient()
        {
            // Arrange
            this.request = new RestRequest(url);

            // Act
            var response = this.client.Execute(request);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.GreaterThan(0));
            Assert.That(contacts[0].firstName, Is.EqualTo("Steve"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Jobs"));
        }
        [Test]
        public void Test_FindContact_ValidName()
        {
            // Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "albert");

            // Act
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts[0].firstName, Is.EqualTo("Albert"));
            Assert.That(contacts[0].lastName, Is.EqualTo("Einstein"));
        }
        [Test]
        public void Test_FindContact_InvalidName()
        {
            // Arrange
            this.request = new RestRequest(url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "invalid2635");

            // Act
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.EqualTo(0));


        }
        [Test]
        public void Test_CreateContact_InvalidContact()
        {
            // Arrange
            this.request = new RestRequest(url);
          
            var body = new
               {
                   firstName = "Babi",
                   email = "babi@abv.bg",
                   phone = "12345678"
               };
            request.AddJsonBody(body);

        // Act
        var response = this.client.Execute(request, Method.Post);
          
         // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Last name cannot be empty!\"}"));

        }
        [Test]
        public void Test_CreateContact_ValidContact()
        {
            // Arrange
            this.request = new RestRequest(url);

            var body = new
            {
                firstName = "Dafne",
                lastName = "Ivanova",
                email = "dafne@abv.bg",
                phone = "12345678"
            };
            request.AddJsonBody(body);

            // Act
            var response = this.client.Execute(request, Method.Post);
            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var allContacts = this.client.Execute(request,Method.Get);
            var contacts = JsonSerializer.Deserialize<List<Contacts>>(allContacts.Content);
            var lastContact =contacts.Last();

            // Assert
        ;
            Assert.That(lastContact.firstName, Is.EqualTo(body.firstName));
            Assert.That(lastContact.lastName, Is.EqualTo(body.lastName));

        }
    }
}