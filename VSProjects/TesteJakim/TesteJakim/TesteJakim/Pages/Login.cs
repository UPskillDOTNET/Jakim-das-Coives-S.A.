using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteJakim.Pages
{
    class Login
    {
        public Login(IWebDriver driver)
        {
            Driver = driver;
        }

        private IWebDriver Driver { get; }

        IWebElement txtUserName => Driver.FindElement(By.Name("Input.Email"));
        IWebElement txtPassword => Driver.FindElement(By.Name("Input.Password"));
        IWebElement btnLogin => Driver.FindElement(By.ClassName("btnAccountSubmit"));

        public void Logins(string userName, string password)
        {
            txtUserName.SendKeys(userName);
            txtPassword.SendKeys(password);
            btnLogin.Submit(); 
        }

    }
}
