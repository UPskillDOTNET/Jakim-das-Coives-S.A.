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

        public IWebElement lnkLogin => Driver.FindElement(By.LinkText("Iniciar Sessão"));

        IWebElement insertUserName => Driver.FindElement(By.Name("Input.Email"));

        public void ClickLogin() => lnkLogin.Click();

        public bool IncertUserLogin() => userLgn.Displayed;
    }
}
