using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace TesteJakim.Pages
{
    class SaldoPage
    {
        public SaldoPage(IWebDriver driver)
        {
            Driver = driver;
        }

        private IWebDriver Driver { get; }

        //Procurar os elemento necessários na Pagina de Saldo
        IWebElement txtAdicionarCreditos => Driver.FindElement(By.LinkText("Pretende Adicionar Créditos à Carteira?"));

        public bool TextAdicionarCreditos => txtAdicionarCreditos.Displayed;
    }
}
