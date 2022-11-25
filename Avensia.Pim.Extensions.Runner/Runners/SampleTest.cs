using System.IO;
using Avensia.ModelMaster.Common.Logging.AppenderFactories;
using inRiver.Remoting.Extension;

namespace Avensia.Pim.Extensions.Runner.Runners
{
    public class SampleTest
    {
        public void Test(inRiverContext context)
        {
            //var faqImportScheduled = new FaqImportScheduled();
            //context.Settings = faqImportScheduled.DefaultSettings;
            //faqImportScheduled.Context = context;
            //faqImportScheduled.ConfigureLocalContainer<IAppenderFactory, ColoredConsoleAppenderFactory>();
            ////var message = GetJsonData();
            //faqImportScheduled.Execute(force: true);
        }
        private static string GetJsonData()
        {
            return File.ReadAllText(@"C:\Users\ninalc\Downloads\faq.json");
        }
    }
}
