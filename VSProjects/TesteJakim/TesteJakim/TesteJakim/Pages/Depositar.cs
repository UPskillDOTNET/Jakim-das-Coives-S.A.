using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace TesteJakim.Pages
{
    class DepositarPage
    {
        public DepositarPage(IWebDriver driver)
        {
            Driver = driver;
        }

        private IWebDriver Driver { get; }

        //Procurar Caixa Inserir Créditos a Adicionar Carteira
        IWebElement lnkEscolherValorCarteira => Driver.FindElement(By.Id("Valor"));

        //Procurar Botão Adicionar Créditos
        IWebElement lnkAdicionarCreditos => Driver.FindElement(By.Id("pesquisa"));


        //Ação para Carregar na NavBar
        public void EscolherValorCarteira() => lnkEscolherValorCarteira.Click();

        //Ação para Carregar Saldo na NavBar
        public void ClickAdicionarCreditos() => lnkAdicionarCreditos.Click();
    }
}