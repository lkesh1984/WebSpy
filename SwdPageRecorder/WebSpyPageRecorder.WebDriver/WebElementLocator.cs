using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace WebSpyPageRecorder.WebDriver
{
    [Serializable]
    public class WebElementLocator
    {
        [DisplayName("Search Method")]
        public LocatorSearchMethod HowToSearch { get; set; }

        [DisplayName("Locator")]
        public string Locator { get; set; }
        
        [DisplayName("CSS Selector")]
        public string CssSelector { get; set; }

    }

}
