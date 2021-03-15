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
    class RegistoTests
    {

        //Abrir Browser (Se n abrir confirmar versao do browser)
        IWebDriver webDriver = new ChromeDriver(/*versao do browser*/);

        [SetUp]
        public void Setup()
        {
            //Entrar no Site
            webDriver.Navigate().GoToUrl("https://app-frontenddeploy.azurewebsites.net/");

        }

        [Test]
        public void Validation()
            //Por o Programa a correr
        {
            IWebElement registerButton = webDriver.FindElement(By.XPath("/html/body/header/nav/div/div/div/ul/li[1]/a"));
            registerButton.Click();

            IWebElement emailTxtBox = webDriver.FindElement(By.Id("Input_Email"));
            emailTxtBox.SendKeys("a"); //testar um email inválido

            IWebElement pwdTxtBox = webDriver.FindElement(By.Id("Input_Password"));
            pwdTxtBox.SendKeys("a"); //testar uma password inválida

            IWebElement confPwdTxtBox = webDriver.FindElement(By.Id("Input_ConfirmPassword"));
            confPwdTxtBox.SendKeys("b"); //testar uma password diferente

            IWebElement nomeTxtBox = webDriver.FindElement(By.Id("Input_NomeUtilizador"));
            nomeTxtBox.SendKeys("");; //testar nome vazio

            IWebElement nifTxtBox = webDriver.FindElement(By.Id("Input_Nif"));
            nifTxtBox.SendKeys("b"); //testar um nif inválido

            IWebElement metodoPagElement = webDriver.FindElement(By.Id("Input_MetodoId"));
            SelectElement metodoPagSelector = new SelectElement(metodoPagElement);
            metodoPagSelector.SelectByIndex(0); //testar um nif inválido

            IWebElement submitRegisterButton = webDriver.FindElement(By.XPath("/html/body/div/main/div/div[1]/form/button"));
            submitRegisterButton.Click();
            submitRegisterButton.Submit();


            /*
             
            ASSERT ERROR MESSAGES SHOWING 

             */



            IWebElement emailErrorText = webDriver.FindElement(By.Id("Input_Email-error"));
            Assert.IsTrue(emailErrorText.Text == "The Email field is not a valid e-mail address.");

            IWebElement pwErrorText = webDriver.FindElement(By.Id("Input_Password-error"));
            Assert.IsTrue(pwErrorText.Text == "The Password must be at least 6 and at max 100 characters long.");

            IWebElement cPwErrorText = webDriver.FindElement(By.Id("Input_ConfirmPassword-error"));
            Assert.IsTrue(cPwErrorText.Text == "As passwords não coincidem. Por favor insire novamente.");

            IWebElement nuErrorText = webDriver.FindElement(By.Id("Input_NomeUtilizador-error"));
            Assert.IsTrue(nuErrorText.Text == "The Nome Completo field is required.");

            IWebElement nifErrorText = webDriver.FindElement(By.Id("Input_Nif-error"));
            Assert.IsTrue(nifErrorText.Text == "NIF inválido");

            IWebElement metodoPagErrorText = webDriver.FindElement(By.Id("Input_MetodoId-error"));
            Assert.IsTrue(metodoPagErrorText.Text == "The field Método de Pagamento Preferencial must be between 1 and 3.");

            IWebElement summaryErrors = webDriver.FindElement(By.ClassName("validation-summary-errors"));
            Assert.NotNull(summaryErrors);
        }

        [Test]
        public void RegisterLocalAccount()
        //Por o Programa a correr
        {
            IWebElement registerButton = webDriver.FindElement(By.XPath("/html/body/header/nav/div/div/div/ul/li[1]/a"));
            registerButton.Click();

            IWebElement emailTxtBox = webDriver.FindElement(By.Id("Input_Email"));
            emailTxtBox.SendKeys("endtoend@test.com");

            IWebElement pwdTxtBox = webDriver.FindElement(By.Id("Input_Password"));
            pwdTxtBox.SendKeys("123Pa$$word");

            IWebElement confPwdTxtBox = webDriver.FindElement(By.Id("Input_ConfirmPassword"));
            confPwdTxtBox.SendKeys("123Pa$$word");

            IWebElement nomeTxtBox = webDriver.FindElement(By.Id("Input_NomeUtilizador"));
            nomeTxtBox.SendKeys("End To End Test User"); ;

            IWebElement nifTxtBox = webDriver.FindElement(By.Id("Input_Nif"));
            nifTxtBox.SendKeys("888888888");

            IWebElement metodoPagElement = webDriver.FindElement(By.Id("Input_MetodoId"));
            SelectElement metodoPagSelector = new SelectElement(metodoPagElement);
            metodoPagSelector.SelectByIndex(3); //PAYPAL

            IWebElement payPalEmailTxtBox = webDriver.FindElement(By.Id("Input_EmailPayPal"));
            payPalEmailTxtBox.SendKeys("endtoend@test.com");

            IWebElement payPalPwdTxtBox = webDriver.FindElement(By.Id("Input_PasswordPayPal"));
            payPalPwdTxtBox.SendKeys("Pa$$wordzinha123");

            IWebElement submitRegisterButton = webDriver.FindElement(By.XPath("/html/body/div/main/div/div[1]/form/button"));
            submitRegisterButton.Submit();



            IWebElement loggedInText = webDriver.FindElement(By.XPath("/html/body/header/nav/div/div/div/ul/li[1]/a"));
            Assert.IsTrue(loggedInText.Text == "Bem-vindo End To End Test User!");
        }

    }
}
