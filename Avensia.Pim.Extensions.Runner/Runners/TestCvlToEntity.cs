using System;
using System.IO;
using Avensia.CvlToEntity.Settings;
using Avensia.ModelMaster.Common.Logging.AppenderFactories;
using Avensia.Pim.Extensions.Runner.Models;
using inRiver.Remoting.Extension;
using inRiver.Remoting.Objects;
using inRiverCommunity.Extensions.Core.Settings;
using Newtonsoft.Json;

namespace Avensia.Pim.Extensions.Runner.Runners
{
    public class TestCvlToEntity
    {
        public void Test(inRiverContext context)
        {
            var extension = new CvlToEntity.CvlToEntity();
            var message = File.ReadAllText(@"C:\Projects\mine\Avensia.Pim.Extensions.Runner\Files\CvlToEntitySettings.json");
            var myDeserializedClass = JsonConvert.DeserializeObject<CvlToEntitySettings>(message);
            context.Settings = context.GetSettingsAsDictionary(myDeserializedClass);
            extension.Context = context;
            extension.ConfigureLocalContainer<IAppenderFactory, ColoredConsoleAppenderFactory>();


            var modelService = context.ExtensionManager.ModelService;

            //var entityTypes = modelService.GetAllEntityTypes();
            var testEntityType = "Garment";
            var isGarmentExist = modelService.GetEntityType(testEntityType);
            if (isGarmentExist == null)
            {
                CreateEntityGarment(context, testEntityType);
            }

            extension.CVLValueUpdated("SubCategory", "blazers");
            extension.CVLValueCreated("SubCategory", "jeans");
            extension.CVLValueDeleted("SubCategory", "jeans");
        }

        private static void CreateEntityGarment(inRiverContext context, string entityType)
        {
            var modelService = context.ExtensionManager.ModelService;
            var utilityService = context.ExtensionManager.UtilityService;

            var garment = new EntityType(entityType);
            garment.Name = new LocaleString(utilityService.GetAllLanguages());
            foreach (var ci in utilityService.GetAllLanguages())
            {
                garment.Name[ci] = entityType;
            }
            modelService.AddEntityType(garment);

            var newFieldId = new NewFieldTypeParam
            {
                EntityTypeId = entityType,
                Id = $"{entityType}Id",
                FieldDataType = DataType.String,
                Name = $"{entityType} Id",
                Description = $"{entityType} Id Description"
            };
            AddFieldType(context, newFieldId);
            var newFieldName = new NewFieldTypeParam
            {
                EntityTypeId = entityType,
                Id = $"{entityType}Name",
                FieldDataType = DataType.LocaleString,
                Name = $"{entityType} Name",
                Description = $"{entityType} Name Description"
            };
            AddFieldType(context, newFieldName);
            var newFieldType = new NewFieldTypeParam
            {
                EntityTypeId = entityType,
                Id = $"{entityType}Type",
                FieldDataType = DataType.String,
                Name = $"{entityType} Type",
                Description = $"{entityType} Type Description"
            };
            AddFieldType(context, newFieldType);
            var newFieldDescription = new NewFieldTypeParam
            {
                EntityTypeId = entityType,
                Id = $"{entityType}Description",
                FieldDataType = DataType.String,
                Name = $"{entityType} Description",
                Description = $"{entityType} Description Description"
            };
            AddFieldType(context, newFieldDescription);
            var newFieldSku = new NewFieldTypeParam
            {
                EntityTypeId = entityType,
                Id = $"{entityType}Sku",
                FieldDataType = DataType.String,
                Name = $"{entityType} Sku",
                Description = $"{entityType} Sku Description"
            };
            AddFieldType(context, newFieldSku);

            var garmentId = modelService.GetFieldType($"{entityType}Id");
            if (garmentId != null)
            {
                garmentId.Unique = true;
                modelService.UpdateFieldType(garmentId);
            }
        }
        
        private static void AddFieldType(inRiverContext context, NewFieldTypeParam param)
        {
            var modelService = context.ExtensionManager.ModelService;
            var utilityService = context.ExtensionManager.UtilityService;

            var fieldtype = new FieldType();
            fieldtype.EntityTypeId = param.EntityTypeId;
            fieldtype.Id = param.Id;
            fieldtype.DataType = param.FieldDataType;
            fieldtype.CVLId = param.CvlId;
            fieldtype.CategoryId = param.CategoryId;
            fieldtype.Multivalue = param.MultiValue;

            var name = new LocaleString(utilityService.GetAllLanguages());
            var description = new LocaleString(utilityService.GetAllLanguages());
            foreach (var ci in utilityService.GetAllLanguages())
            {
                name[ci] = param.Name;
                description[ci] = param.Description;
            }
            fieldtype.Name = name;
            fieldtype.Description = description;

            try
            {
                modelService.AddFieldType(fieldtype);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
