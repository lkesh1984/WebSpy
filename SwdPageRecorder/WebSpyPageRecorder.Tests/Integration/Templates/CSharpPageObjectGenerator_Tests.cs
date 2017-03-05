using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

using WebSpyPageRecorder.WebDriver;
using WebSpyPageRecorder.TestModel;
using WebSpyPageRecorder.UI;

using FluentAssertions;
using System.IO;

namespace WebSpyPageRecorder.Tests.Integration.Templates
{
    [TestFixture]
    public class CSharpPageObjectGenerator_Tests: MyTest    
    {


        [Test(Description = "Templates")]
        [TestCase("[CSharp] Explicit Finders - Driver.FindElement(By).cshtml")]
        [TestCase("[CSharp] Locator constants in the static class.cshtml")]
        [TestCase("[CSharp] WebPageSpy Starter PageObjects.cshtml")]
        [TestCase("[CSharp] WebPageSpy Starter PageObjects with DataForm.cshtml")]
        [TestCase("[CSharp] WebDriver.Support PageObject (Default).cshtml")]
        [TestCase("[Java] WebPageSpy StarterJ PageObjects.cshtml")]
        [TestCase("[Java] WebDriver.Support PageObject.cshtml")]
        [TestCase("[Python] Simple PageObject.cshtml")]
        [TestCase("[Ruby] PageObject Gem.cshtml")]
        [TestCase("Run External Tool.cshtml")]
        public void CSharpPageObjectGenerator_Generate(string templateFileName)
        {
            CSharpPageObjectGenerator engine = new CSharpPageObjectGenerator();
            string templatesFolder = Path.Combine(Helper.AssemblyDirectory(), "CodeTemplates");
            string fileName = Path.Combine(templatesFolder, templateFileName);

            WebSpyPageObject pageObject = new WebSpyPageObject();
            string expectedName = "ExpectedName123456";
            pageObject.PageObjectName = expectedName;

            string[] output = engine.Generate(pageObject, fileName);

            string actual = string.Join("", output);
            actual.Should().Contain(expectedName);
            Console.WriteLine(actual);
        }
    }
}
