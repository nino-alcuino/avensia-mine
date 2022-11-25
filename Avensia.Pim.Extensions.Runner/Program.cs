using System;
using Avensia.Pim.Extensions.Runner.Runners;
using ExtensionTester;
using inRiver.Remoting;
using inRiver.Remoting.Extension;

namespace Avensia.Pim.Extensions.Runner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var container = InRiverLocator.CreateContainer().WithAppender<ColoredConsoleAppenderFactory>();

            //var context = container.GetInstance<inRiverContext>();
            var context = new inRiverContext(RemoteManager.CreateInstance("https://demoremoting.productmarketingcloud.com", "demoa1@inriver.com", "!Demoa1!"), new ConsoleLogger());
            
            TestCvlToEntity(context);
            //TestPimLinkAutomation(context);
            //TestPimAutomation(context);
            Console.ReadKey();
        }

        //public static void TestPimAutomation(inRiverContext context)
        //{
        //    var test = new TestEntityAutomation();
        //    test.TestMergeField(context);
        //}

        //public static void TestPimLinkAutomation(inRiverContext context)
        //{
        //    var test = new TestEntityAutomation();
        //    test.TestParentEntityImage(context);
        //}

        public static void TestCvlToEntity(inRiverContext context)
        {
            var test = new TestCvlToEntity();
            test.Test(context);
        }
    }
}
