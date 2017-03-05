using System;
using System.Linq;
using NUnit.Framework;
using System.Reflection;
using System.IO;

namespace WebSpyPageRecorder.Tests
{
    [TestFixture]
    public abstract class MyTest
    {

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        [SetUp]
        public void MyTestInitialize() 
        {
            Console.WriteLine(AssemblyDirectory);
            Directory.SetCurrentDirectory(AssemblyDirectory);
        }

    }
}
