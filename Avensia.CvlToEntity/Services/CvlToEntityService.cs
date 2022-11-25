using System.Globalization;
using System.Linq;
using Avensia.CvlToEntity.Interfaces;
using Avensia.CvlToEntity.Settings;
using Avensia.ModelMaster.Common.Extensions;
using Avensia.ModelMaster.Common.Logging;
using inRiver.Remoting;
using inRiver.Remoting.Extension;
using inRiver.Remoting.Objects;
using inRiverCommunity.Extensions.Core.Settings;
using log4net;

namespace Avensia.CvlToEntity.Services
{
    public class CvlToEntityService : ICvlToEntityService
    {
        private readonly CvlToEntitySettings _settings;
        private readonly CultureInfo _masterLanguage;
        private static readonly ILog Log = LogMaster.GetLogger();
        private readonly IDataService _dataService;
        private readonly IModelService _modelService;


        public CvlToEntityService(inRiverContext context)
        {
            _dataService = context.ExtensionManager.DataService;
            _modelService = context.ExtensionManager.ModelService;
            _settings = context.GetSettings<CvlToEntitySettings>();
            _masterLanguage = context.ExtensionManager.UtilityService.GetMasterLanguage();
        }

        public void CreateOrUpdateEntity(string cvlId, string cvlKey)
        {
            var settingsCvlToEntity = _settings.CvlEntities.Where(q => q.CvlId.Equals(cvlId)).ToList();
            if (!settingsCvlToEntity.Any()) return;

            var cvlValue = _modelService.GetCVLValueByKey(cvlKey, cvlId);
            if (cvlValue == null)
            {
                //Log.Warn($"CreateOrUpdateEntity: Couldn't find {cvlKey}");
                return;
            }
            //Log.Info($"CreateOrUpdateEntity: cvlId:{cvlId} cvlKey:{cvlKey} triggered!");

            foreach (var cvlEntity in settingsCvlToEntity)
            {
                var entity = _dataService.GetEntityByUniqueValue(cvlEntity.CvlKeyFieldTypeId, cvlKey, LoadLevel.DataOnly);
                if (entity == null)
                {
                    var entityType = _modelService.GetEntityType(cvlEntity.EntityTypeId);
                    if (entityType == null)
                    {
                        //Log.Warn($"CreateOrUpdateEntity: CvlId({cvlEntity.CvlId}) EntityType {cvlEntity.EntityTypeId} not found!");
                        continue;
                    }

                    entity = Entity.CreateEntity(entityType);

                    if (!string.IsNullOrEmpty(cvlEntity.CvlKeyFieldTypeId))
                        entity.GetField(cvlEntity.CvlKeyFieldTypeId).Data = cvlKey;
                    else
                    {
                        //Log.Warn($"CreateOrUpdateEntity: CvlId({cvlEntity.CvlId}) CvlKeyFieldTypeId is empty!");
                        continue;
                    }

                    foreach (var otherField in cvlEntity.DefaultValueFields)
                    {
                        var entityField = entity.GetField(otherField.FieldTypeId);
                        if (entityField == null) continue;
                        entityField.Data = ToLocaleOrString(entityField, otherField.FieldTypeValue);
                    }
                }

                if (!string.IsNullOrEmpty(cvlEntity.CvlValueFieldTypeId))
                {
                    var entityField = entity.GetField(cvlEntity.CvlValueFieldTypeId);
                    if (entityField == null) continue;
                    entityField.Data = ToLocaleOrString(entityField, cvlValue.Value);
                }

                if (!string.IsNullOrEmpty(cvlEntity.CvlParentKeyFieldTypeId))
                {
                    var entityField = entity.GetField(cvlEntity.CvlParentKeyFieldTypeId);
                    if (entityField == null) continue;
                    entityField.Data = ToLocaleOrString(entityField, cvlValue.ParentKey);
                }

                _dataService.AddOrUpdateEntity(entity);
            }
        }
        public void DeleteEntity(string cvlId, string cvlKey)
        {
            var settingsCvlToEntity = _settings.CvlEntities.Where(q => q.CvlId.Equals(cvlId)).ToList();
            if (!settingsCvlToEntity.Any()) return;

            var cvlValue = _modelService.GetCVLValueByKey(cvlKey, cvlId);
            if (cvlValue == null)
            {
                //Log.Warn($"DeleteEntity: Couldn't find {cvlKey}");
                return;
            }

            foreach (var cvlEntity in settingsCvlToEntity)
            {
                if (!cvlEntity.IsAllowCvlEntityDeleted) continue;
                var entity = _dataService.GetEntityByUniqueValue(cvlEntity.CvlKeyFieldTypeId, cvlKey, LoadLevel.Shallow);
                if (entity != null)
                    _dataService.DeleteEntity(entity.Id);
            }
        }

        private object ToLocaleOrString(Field field, object cvlValue)
        {
            if (field == null)
            {
                //Log.Warn($"CreateOrUpdateEntity: Couldn't find FieldTypeId");
                return null;
            }

            if (field.FieldType.DataType == DataType.LocaleString)
                return ToLocaleString(cvlValue);
            return LocaleToString(cvlValue);
        }
        private static LocaleString ToLocaleString(object cvlValue)
        {
            switch (cvlValue)
            {
                case LocaleString ls:
                    return ls;
                case string s:
                    return s.ToLocaleString();
                default:
                    return null;
            }
        }
        private string LocaleToString(object value)
        {
            if (value is LocaleString name)
                return name[_masterLanguage];
            return value.ToString();
        }
    }
}
