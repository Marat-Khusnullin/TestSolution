using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System.Collections.Generic;

namespace TestProject1
{
    public class Tests
    {
        private WebDriver driver;
        private string baseUrl;
        private WikipediaPage wikipediaPage;
        private Logger logger;
        private int stepsCount = 0;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            baseUrl = "https://ru.wikipedia.org/";
            logger = new Logger("logs.txt");
        }

        [Test]
        public void FindPhilosophyWordTest()
        {
            driver.Navigate().GoToUrl(baseUrl);
            wikipediaPage = new WikipediaPage(driver);
            wikipediaPage.ClickRandomArticleButton();
            List<string> nodes = new List <string>();

            do
            {
                //���������, ��������� �� ���� �� ������. � ������, ���� ������� �� ������ ��� ��� - ������������ �� ���������.
                if (nodes.Contains(wikipediaPage.GetArticleName()))
                {
                    wikipediaPage.ClickRandomArticleButton();
                    logger.Log(stepsCount++, wikipediaPage.GetArticleName(), driver.Url, "���������� �� ����! ������ ���������� �� ���������!");
                    nodes.Clear();
                }
                nodes.Add(wikipediaPage.GetArticleName());

                //������� �� ������ ������ � �������� �������� ������. ������� ������, ��� ��� ������ ������ �������������� �������.
                //� ������ ������ ��������� �� ��������� ������.
                //����� � ����� ������ �� ������ ���� �����-�� ������ (��� ������� ���������� �� ������ � �������)
                try
                {
                    ClickOnFirstLinkInMainInfo();
                    logger.Log(stepsCount++, wikipediaPage.GetArticleName(), driver.Url);
                }
                catch (NoSuchElementException e)
                {
                    wikipediaPage.ClickRandomArticleButton();
                    logger.Log(stepsCount++, wikipediaPage.GetArticleName(), driver.Url, "������ ���������� ������� :(. ������ ���������� �� ���������!");
                    nodes.Clear();
                }
            } while (!wikipediaPage.GetArticleName().ToLower().Contains("���������"));

            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        //����� ��� ������ � �������� �� ���������� ������ � ������. ��������������� ������ ������ ��� ��������� ������.
        //� ������� � �����-�� ���� ������� "�� ������ ������ ������� ����� ������", � �� ����� ���� ��� ������ �� �������� ������ �� ������.
        //� ������ ���������, ���� �� � ������ ��������� ������ ������. ���� ��� - ����� �� �������. ����� ������� �� ������� �����������.
        public void ClickOnFirstLinkInMainInfo()
        {
            //������� ������� ����� ������ ����� XPath, �� ������� � �� ���� ���������� �� �����. ��� ������-�� ������ ������ � ���, ��� ���������� ��������������
            //mainInfo.FindElement(By.XPath("//*/text()[contains(.,'�')]/following::a[1]")).Click();
            var elements = wikipediaPage.GetMainInfo().FindElements(By.TagName("a"));
            if (elements.Count == 0 || elements.All(p => !CheckLinkElement(p)))
            {
                elements = wikipediaPage.GetSecondaryInfo().FindElements(By.TagName("a"));
            }
                
            foreach (WebElement element in elements)
            {
                if (CheckLinkElement(element))
                {
                    element.Click();
                    break;
                }
            }
        }

        //��� �����������, ��� ������ �� �������� ������� �� �������� �����, �� �������� �������, �� �������� ������� �� �����/�����/�����������,
        //��� �������� ������ �� ������� � �� �������� ������� �� �������������� ������. 
        private bool CheckLinkElement(IWebElement element)
        {
            return IsStringRussian(element.Text) && !element.GetAttribute("Title").Contains(".ogg") && !element.GetAttribute("href").Contains(".oga") && !element.GetAttribute("class").Equals("new");
        }

        //�������� ����������, ��� ������ �������� ������ �� �������, ����� ����� ������� � �����.
        private bool IsStringRussian(string text)
        {
            return Regex.IsMatch(text, "^[�-��-�\\s-]+$");
        }

    }
}