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
                //Проверяем, получился ли цикл из статей. В случае, если переход на статью уже был - сбрасываемся на случайную.
                if (nodes.Contains(wikipediaPage.GetArticleName()))
                {
                    wikipediaPage.ClickRandomArticleButton();
                    logger.Log(stepsCount++, wikipediaPage.GetArticleName(), driver.Url, "Наткнулись на цикл! Статья сбросилась на случайную!");
                    nodes.Clear();
                }
                nodes.Add(wikipediaPage.GetArticleName());

                //Переход по первой ссылке в основном описании статьи. Обернул ошибку, так как бывают статьи нестандартного формата.
                //В случае ошибки переходим на случайную статью.
                //Решил в таком случае не искать хоть какую-то ссылку (это чревато попаданием на статьи с циклами)
                try
                {
                    ClickOnFirstLinkInMainInfo();
                    logger.Log(stepsCount++, wikipediaPage.GetArticleName(), driver.Url);
                }
                catch (NoSuchElementException e)
                {
                    wikipediaPage.ClickRandomArticleButton();
                    logger.Log(stepsCount++, wikipediaPage.GetArticleName(), driver.Url, "Статья неудачного формата :(. Статья сбросилась на случайную!");
                    nodes.Clear();
                }
            } while (!wikipediaPage.GetArticleName().ToLower().Contains("философия"));

            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        //Метод для поиска и перехода по подходящей ссылке в статье. Рассматриваются только первые два параграфа статьи.
        //В задании в общем-то было указано "по первой ссылке главной формы данных", а не любым путём где угодно на странице нажать на ссылку.
        //В методе проверяем, есть ли в первом параграфе нужная ссылка. Если нет - берем из второго. Далее щёлкаем по первому подходящему.
        public void ClickOnFirstLinkInMainInfo()
        {
            //Попытка сделать поиск ссылки через XPath, до которой я не смог додуматься по итогу. Оно почему-то кидает ошибку о том, что невозможно взаимодействие
            //mainInfo.FindElement(By.XPath("//*/text()[contains(.,'—')]/following::a[1]")).Click();
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

        //Тут проверяется, что ссылка не является ссылкой на страницу языка, не является сноской, не является ссылкой на аудио/видео/изображение,
        //Что является словом на русском и не является ссылкой на несуществующую статью. 
        private bool CheckLinkElement(IWebElement element)
        {
            return IsStringRussian(element.Text) && !element.GetAttribute("Title").Contains(".ogg") && !element.GetAttribute("href").Contains(".oga") && !element.GetAttribute("class").Equals("new");
        }

        //Проверка регуляркой, что ссылка является словом на русском, может иметь пробелы и дефис.
        private bool IsStringRussian(string text)
        {
            return Regex.IsMatch(text, "^[А-Яа-я\\s-]+$");
        }

    }
}