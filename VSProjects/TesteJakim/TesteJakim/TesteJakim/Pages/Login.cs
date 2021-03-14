using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteJakim.Pages
{
    class LoginPage
    {
        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        private IWebDriver Driver { get; }


        //Procurar os elemento necessários na Pagina de Login
        IWebElement txtUserName => Driver.FindElement(By.Name("Input.Email"));
        IWebElement txtPassword => Driver.FindElement(By.Name("Input.Password"));
        IWebElement btnLogin => Driver.FindElement(By.ClassName("btnAccountSubmit"));


        //Ações a fazer aos elementos da página Login

        public void Login(string userName, string password)
        {
            txtUserName.SendKeys(userName);
            txtPassword.SendKeys(password);
            btnLogin.Submit(); 
        }

    }
}
