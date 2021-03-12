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


        //Ação pra fazer Login
        public void ClickLogin() => lnkLogin.Click();

        //Ação pra procar que fez login
        public bool WelcomeMsgLogin() => welcomeMsg.Displayed;
    }
}
