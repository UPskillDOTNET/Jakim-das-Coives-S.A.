using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using TesteJakim.Pages;

namespace TesteJakim.Tests
{
    class SaldoTest
    {

        //Abrir Browser (Se n abrir confirmar versao do browser)
        IWebDriver webDriver = new ChromeDriver(/*versao do browser*/);

        [SetUp]
        public void Setup()
        {
            //Entrar no Site
            webDriver.Navigate().GoToUrl("https://localhost:44372/");

            HomePage homePage = new HomePage(webDriver);
            homePage.ClickLogin();

            LoginPage loginPage = new LoginPage(webDriver);
            loginPage.Login("sistemacentraljakim@gmail.com", "123Pa$$word");

        }

        [Test]
        public void VerSaldo()
        {
            HomePage homePage = new HomePage(webDriver);
            homePage.ClickNavBar();
            homePage.ClickSaldoNavBar();

            SaldoPage saldoPage = new SaldoPage(webDriver);
            Assert.That(saldoPage.TextAdicionarCreditos, Is.True);
        }
    }
}
