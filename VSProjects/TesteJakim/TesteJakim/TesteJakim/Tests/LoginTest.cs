using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using TesteJakim.Pages;

namespace TesteJakim.Tests
{
    class LoginTest
    {
        //Abrir Browser (Se n abrir confirmar versao do browser)
        IWebDriver webDriver = new ChromeDriver(/*versao do browser*/);

        [SetUp]
        public void Setup()
        {
            //Entrar no Site
            webDriver.Navigate().GoToUrl("https://localhost:5055/");

        }

        [Test]
        public void Login()
        {
            Home home = new Home(webDriver);
            home.ClickLogin();

            Login login = new Login(webDriver);
            login.Logins("sistemacentraljakim@gmail.com", "123Pa$$word");

            Assert.That(home.IncertUserLogin, Is.True);

        }
    }
}
