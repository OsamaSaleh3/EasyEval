using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EasyEval
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--remote-allow-origins=*");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                try
                {
                    driver.Navigate().GoToUrl("https://app2.bau.edu.jo:7799/eval/Login.jsp");

                    Console.WriteLine("=== Automatic Evaluation System ===");

                    Console.Write("Enter number of subjects: ");
                    int subjects = int.Parse(Console.ReadLine());

                    Console.Write("Enter Student ID: ");
                    string studentId = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    Console.Write("Enter National Number: ");
                    string nationalNumber = Console.ReadLine();

                    driver.FindElement(By.Id("tbstdno")).SendKeys(studentId);
                    driver.FindElement(By.Id("tbstdpass")).SendKeys(password);
                    driver.FindElement(By.Id("tbstdnatno")).SendKeys(nationalNumber);

                    driver.FindElement(By.CssSelector("input[type='submit']")).Click();

                    Thread.Sleep(3000);

                    for (int i = 0; i < subjects; i++)
                    {
                        Console.WriteLine($"Starting evaluation for subject #{i + 1}");

                        for(int j=0;j<19;j++)
                        {
                            var radios = driver.FindElements(By.CssSelector("input[type='radio'][value='5']"));

                            if (radios.Count > 0)
                            {
                                radios[0].Click();

                                Thread.Sleep(500);

                                try
                                {
                                    var nextButton = driver.FindElement(By.Id("btnNext"));
                                    nextButton.Click();
                                }
                                catch (NoSuchElementException)
                                {
                                }

                                Thread.Sleep(1000);
                            }
                            else
                            {
                                var submitButtons = driver.FindElements(By.CssSelector("input[type='submit']"));
                                if (submitButtons.Count > 0) break;
                            }
                        }
                        Console.WriteLine("Questions finished. Please solve Captcha manually and click Save in the browser.");
                        Console.WriteLine("After saving, press Enter here to continue to the next subject...");
                        Console.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }

                Console.WriteLine("Process completed successfully. Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}