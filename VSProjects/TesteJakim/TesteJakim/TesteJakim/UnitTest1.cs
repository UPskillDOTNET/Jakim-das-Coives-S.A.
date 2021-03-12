using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TesteJakim
{
    public class Testes
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            //Abrir Browser (Se n abrir confirmar versao do browser)
            IWebDriver webDriver = new ChromeDriver(/*versao do browser*/);

            //Entrar no Site
            webDriver.Navigate().GoToUrl("https://localhost:5055/");

            //Identificar o Login   
            IWebElement lnkLogin = webDriver.FindElement(By.LinkText("Iniciar Sessão"));
            //Operação
            lnkLogin.Click();

            var userLgn = webDriver.FindElement(By.Name("Input.Email"));

            //Assertion
            Assert.That(userLgn.Displayed, Is.True);

            userLgn.SendKeys("sistemacentraljakim@gmail.com");
            webDriver.FindElement(By.Name("Input.Password")).SendKeys("123Pa$$word");

            webDriver.FindElement(By.ClassName("btnAccountSubmit")).Click();

            IWebElement terminarSessao = webDriver.FindElement(By.LinkText("Bem-vindo Administrador!"));
            Assert.That(terminarSessao.Displayed, Is.True);

        }
    }
}