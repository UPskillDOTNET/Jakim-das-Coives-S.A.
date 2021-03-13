using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using TesteJakim.Pages;

namespace TesteJakim.Tests
{
    class ReservaTests
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
        public void Reservar()
            //Por o Programa a correr
        {


            //LOGIN

            HomePage homePage = new HomePage(webDriver);
            homePage.ClickLogin();

            LoginPage loginPage = new LoginPage(webDriver);
            loginPage.Login("sistemacentraljakim@gmail.com", "123Pa$$word");



            // PESQUISA


            IWebElement freguesiaTxtBox = webDriver.FindElement(By.Name("FreguesiaNome"));
            freguesiaTxtBox.SendKeys("Porto");

            IWebElement inicioDate = webDriver.FindElement(By.Name("Inicio"));
            inicioDate.SendKeys("25052021");
            inicioDate.SendKeys(Keys.Right);

            IWebElement fimDate = webDriver.FindElement(By.Name("Fim"));
            fimDate.SendKeys("27052021");
            fimDate.SendKeys(Keys.Right);

                IWebElement pesquisarButton = webDriver.FindElement(By.Id("pesquisa"));
                pesquisarButton.Submit();


            // SELECIONAR RESERVA


            webDriver.FindElement(By.XPath("/html/body/div/main/table/tbody/tr[1]/td[6]/a")).Click(); // Selecionar primeira reserva na lista



            // PAGAR COM CARTEIRA

            SelectElement metodoSelector = new SelectElement(webDriver.FindElement(By.Name("MetodoId"))); //seletor de metodo de pagamento
            metodoSelector.SelectByIndex(1);

            IWebElement reservarButton = webDriver.FindElement(By.Id("pesquisa"));
            reservarButton.Submit();

        }

    }
}
