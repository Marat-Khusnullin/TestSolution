using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace TestProject1
{
    internal class WikipediaPage
    {
        [FindsBy(How = How.Id, Using = "n-randompage")]
        private IWebElement randomArticleButton;

        [FindsBy(How = How.Id, Using = "firstHeading")]
        private IWebElement articleName;
        
        [FindsBy(How = How.XPath, Using = "//*[@id='mw-content-text']/div[1]/p[1]")]
        private IWebElement mainInfo;

        [FindsBy(How = How.XPath, Using = "//*[@id='mw-content-text']/div[1]/p[2]")]
        private IWebElement secondaryInfo;

        private WebDriver driver;

        public WikipediaPage(WebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void ClickRandomArticleButton()
        {
            randomArticleButton.Click();
        }

        public string GetArticleName()
        {
            return articleName.Text;
        }

        public string GetMainInfoText()
        {
            return mainInfo.Text;   
        }

        public IWebElement GetMainInfo()
        {
            return mainInfo;
        }

        public string GetSecondaryInfoText()
        {
            return secondaryInfo.Text;
        }

        public IWebElement GetSecondaryInfo()
        {
            return secondaryInfo;
        }
    }
}
