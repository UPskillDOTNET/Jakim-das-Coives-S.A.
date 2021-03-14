using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteJakim.Pages
{
    public class HomePage
    {

        public HomePage (IWebDriver webDriver)
        {
            Driver = webDriver;
        }

        private IWebDriver Driver { get; }


        //Procurar o Link do Login
        IWebElement lnkLogin => Driver.FindElement(By.LinkText("Iniciar Sessão"));

        //Procurar Prava como fez Login
        IWebElement welcomeMsg => Driver.FindElement(By.LinkText("Bem-vindo Administrador!"));

        //Procurar Icon da NavBar
        IWebElement lnkNavBar => Driver.FindElement(By.ClassName("navbar-toggler-icon"));

        //Procurar Saldo na NavBar
        IWebElement lnkSaldo => Driver.FindElement(By.LinkText("Saldo"));

        //Ação pra fazer Login
        public void ClickLogin() => lnkLogin.Click();

        //Ação pra procar que fez login
        public bool WelcomeMsgLogin() => welcomeMsg.Displayed;

        //Ação para Carregar na NavBar
        public void ClickNavBar() => lnkNavBar.Click();

        //Ação para Carregar Saldo na NavBar
        public void ClickSaldoNavBar() => lnkSaldo.Click();
    }
}
