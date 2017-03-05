using System;
using System.Linq;
using System.Collections.Generic;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

using WebSpyPageRecorder.WebDriver;
using WebSpyPageRecorder.TestModel;

using FluentAssertions;
using WebSpyPageRecorder.ConfigurationManagement.Profiles;

namespace WebSpyPageRecorder.Tests.Integration.WebSpyPageRecorder.WebDriver
{
    [TestFixture]
    public class WebSpyBrowser_Tests : MyTest    
    {

        /// <summary>
        /// WebSpyBrowser.Initialize should be able to connect to the Remote Hub 
        /// Tests: @WebSpyBrowser.Initialize, @WebDriverOptions, @WebSpyBrowser.TestRemoteHub, @WebSpyBrowser.RunStandaloneServer
        /// 
        /// 1. The test verifies if the remote hub started with @WebSpyBrowser.TestRemoteHub and starts the server
        /// 2. Then it verifies if the HtmlUnitDriver is active
        /// </summary>
        [Test(Description = "WebDriver")]
        public void Initialize_should_be_able_to_start_new_browser()
        {
            Profile browserProfile = new Profile();
            browserProfile.ProfileConfig.activation.browserName = WebDriverOptions.browser_HtmlUnitWithJavaScript;

            WebDriverOptions options = new WebDriverOptions()
            {
                 BrowserProfile = browserProfile,
                 IsRemote = true,
                 RemoteUrl = "http://localhost:4444/wd/hub/",
            };

            bool isSeleniumServerAvailable = true;

            try
            {
                WebSpyBrowser.TestRemoteHub(options.RemoteUrl);
            }
            catch (Exception e)
            {
                isSeleniumServerAvailable = false;
                Console.WriteLine("FAILED: " + e.Message);
            }

            if (!isSeleniumServerAvailable)
            {
                WebSpyBrowser.RunStandaloneServer("start_selenium_server.bat");
            }

            WebSpyBrowser.Initialize(options);

            var rempteDriver = (RemoteWebDriver) WebSpyBrowser.GetDriver();

            rempteDriver.Capabilities.BrowserName.Should().Be("htmlunit");

            WebSpyBrowser.CloseDriver();
        }


        // ===========================================================================
        private static string[] GetDesktopWindowsWithSpecialTitle(string specialTitle)
        {
            var specialWindows = (from title in Helper.GetAllMainWindowTitlesOnDesktop()
                                  where title.Contains(specialTitle)
                                  select title).ToArray();

            return specialWindows;
        }

        /// <summary>
        /// WebSpyBrowser.CloseDriver() should close the opened browser window
        /// Tests: @WebSpyBrowser.CloseDriver, 
        /// 
        /// 1. It opens a FireFox browser
        /// 2. Executes a special JavaScript which sets the document title to "WebSpyBrowser.CloseDriver TEST TEST"
        /// 3. And verifies the window was opened
        /// 4. Closes the browser with @WebSpyBrowser.CloseDriver
        /// 5. Verifies there are no windows with such title on Windows Desktop
        /// </summary>
        [Test(Description = "WebDriver")]
        public void CloseDriver_should_close_the_opened_browser_instance()
        {
            Profile browserProfile = new Profile();
            browserProfile.ProfileConfig.activation.browserName = WebDriverOptions.browser_Firefox;

            WebDriverOptions options = new WebDriverOptions()
            {
                BrowserProfile = browserProfile,
                IsRemote = false,
            };

            string specialTitle = "WebSpyBrowser.CloseDriver TEST TEST";

            string[] specialWindows = new string[] { };
            
            specialWindows = GetDesktopWindowsWithSpecialTitle(specialTitle);
            specialWindows.Length.Should().Be(0, "Expected no windows with title <{0}> at the beginning", specialTitle);

            WebSpyBrowser.Initialize(options);

            string changeTitleScript = string.Format("document.title = '{0}'", specialTitle);
            WebSpyBrowser.ExecuteJavaScript(changeTitleScript);

            specialWindows = GetDesktopWindowsWithSpecialTitle(specialTitle);
            specialWindows.Length.Should().Be(1, "Expected only 1 window with title <{0}> after new driver was created", specialTitle);

            WebSpyBrowser.CloseDriver();

            specialWindows = GetDesktopWindowsWithSpecialTitle(specialTitle);
            specialWindows.Length.Should().Be(0, "Expected no windows with title <{0}> after the driver was closed", specialTitle);


        }



        [Test(Description = "WebDriver")]
        public void Enumerate_Windows_Tabs()
        {
            Helper.RunDefaultBrowser();
            Helper.LoadTestFile("page_opens_several_tabs.html");

            Helper.ClickId("openTab2");
            Helper.ClickId("openTab3");

            BrowserWindow[] actualWindows = WebSpyBrowser.GetBrowserWindows();

            string[] expectedWindowTitles = new string[]
            {
                "Page Tab1",
                "Page Tab3",
                "Page Tab2",
            };

            string[] actualTitles = actualWindows.Select(w => w.Title).ToArray();

            actualTitles.Should().Contain(expectedWindowTitles);

            WebSpyBrowser.CloseDriver();
        }

        [Test(Description = "WebDriver")]
        public void Enumerate_Windows_Popup()
        {
            Helper.RunDefaultBrowser();
            Helper.LoadTestFile("page_opens_several_tabs.html");

            Helper.ClickId("openTab2");
            Helper.ClickId("openTab3");
            Helper.ClickId("openJavaScriptPopup");
            

            BrowserWindow[] actualWindows = WebSpyBrowser.GetBrowserWindows();

            string[] expectedWindowTitles = new string[]
            {
               "Page Tab1", 
               "JavaScript Popup", 
               "Page Tab3", 
               "Page Tab2"
            };

            string[] actualTitles = actualWindows.Select(w => w.Title).ToArray();

            actualTitles.Should().Contain(expectedWindowTitles);

            WebSpyBrowser.CloseDriver();
        }

        [Test(Description = "WebDriver")]
        public void Get_Frames_Tree()
        {

            string[] expectedTitles = new string[]
            {
              "DefaultContent", 
              "firstFrame", 
              "secondFrame", 
              "secondFrame.idIframeInsideSecondFrame", 
              "thirdFrame", 
              "thirdFrame.0"
            };

            
            Helper.RunDefaultBrowser();
            Helper.LoadTestFile("page_with_frames.html");


            BrowserPageFrame rootFrame = WebSpyBrowser.GetPageFramesTree();
            List<BrowserPageFrame> allFrames = rootFrame.ToList();

            string[] actualTitles = allFrames.Select(i => i.ToString()).ToArray();

            actualTitles.Should().Equal(expectedTitles);

            WebSpyBrowser.CloseDriver();
            
        }

    }
}
