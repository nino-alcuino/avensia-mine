using System.IO;
using Avensia.EntityAutomation.Constants;
using Avensia.ModelMaster.Common.Logging.AppenderFactories;
using inRiver.Remoting.Extension;
using inRiverCommunity.Extensions.Core.Settings;
using Newtonsoft.Json;

namespace Avensia.Pim.Extensions.Runner.Runners
{
    public class TestEntityAutomation
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

        public void TestMergeField(inRiverContext context)
        {
            var entityAutomation = new EntityAutomation.MergeFieldsAutomation();
            var message = File.ReadAllText(@"C:\Projects\mine\Avensia.Pim.Extensions.Runner\Files\MergeFieldsAutomationSettings.json");
            var myDeserializedClass = JsonConvert.DeserializeObject<MergeFieldsAutomationSettings>(message);
            context.Settings = context.GetSettingsAsDictionary(myDeserializedClass);
            entityAutomation.Context = context;
            entityAutomation.ConfigureLocalContainer<IAppenderFactory, ColoredConsoleAppenderFactory>();
            var fields = new[] { "ProductName" };
            entityAutomation.EntityUpdated(16325, fields);
        }

        public void TestParentEntityImage(inRiverContext context)
        {
            var entityAutomation = new EntityAutomation.ParentEntityImageAutomation();
            var message = File.ReadAllText(@"C:\Projects\mine\Avensia.Pim.Extensions.Runner\Files\MergeFieldsAutomationSettings.json");
            var myDeserializedClass = JsonConvert.DeserializeObject<MergeFieldsAutomationSettings>(message);
            context.Settings = context.GetSettingsAsDictionary(myDeserializedClass);
            entityAutomation.Context = context;
            entityAutomation.ConfigureLocalContainer<IAppenderFactory, ColoredConsoleAppenderFactory>();
            entityAutomation.LinkCreated(123, 16325, 16327, "ProductItem", null);
            entityAutomation.LinkCreated(123, 16327, 16326, "ItemResource", null);
        }
    }
}
