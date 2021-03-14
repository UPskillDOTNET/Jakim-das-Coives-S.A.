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
            webDriver.Navigate().GoToUrl("https://localhost:44372/");

        }

        [Test]
        public void Login()
            //Por o Programa a correr
        {
            HomePage homePage = new HomePage(webDriver);
            homePage.ClickLogin();

            LoginPage loginPage = new LoginPage(webDriver);
            loginPage.Login("sistemacentraljakim@gmail.com", "123Pa$$word");

            Assert.That(homePage.WelcomeMsgLogin, Is.True);

        }
    }
}
