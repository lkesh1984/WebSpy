using System;

using System.IO;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using System.Collections.ObjectModel;

using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Firefox;
using WebSpyPageRecorder.WebDriver;
using WebSpyPageRecorder.WebDriver.JsCommand;

using System.Xml;
using System.Xml.Linq;

using System.Windows.Forms;
using System.Diagnostics;


namespace WebSpyPageRecorder.UI
{
    public class FullHtmlSourceTabPresenter: IPresenter<FullHtmlSourceTabView>
    {
        private FullHtmlSourceTabView view;

        public void InitWithView(FullHtmlSourceTabView view)
        {
            this.view = view;

            // Subscribe to WebDriverUtils events
            WebSpyBrowser.OnDriverStarted += InitControls;
            WebSpyBrowser.OnDriverClosed += InitControls;
            InitControls();
        }

        private void InitControls()
        {
            var shouldControlBeEnabled = WebSpyBrowser.IsWorking;
            view.btnGetHtmlSource.Enabled = shouldControlBeEnabled;
            view.txtHtmlPageSource.Enabled = shouldControlBeEnabled;
        }

        internal void DisplayHtmlPageSource()
        {
            string htmlSource = WebSpyBrowser.GetHtml();
            view.FillHtmlCodeBox(htmlSource);
        }



        internal void TidyHtml(string htmlContent)
        {

            throw new NotImplementedException();
        }
    }
}
