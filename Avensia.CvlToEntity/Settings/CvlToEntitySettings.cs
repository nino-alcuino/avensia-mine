using System.Collections.Generic;
using inRiver.Remoting.Query;
using inRiverCommunity.Extensions.Core.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Avensia.CvlToEntity.Settings
{
    public class CvlToEntitySettings
    {
        [ExtensionSetting(JsonSerialized = true)]
        public List<CvlEntity> CvlEntities { get; set; }
        
    }

    public class DefaultValueField
    {
        public string FieldTypeId { get; set; }
        public string FieldTypeValue { get; set; }
    }

    public class CvlEntity
    {
        public string CvlId { get; set; }
        public string EntityTypeId { get; set; }
        public string CvlKeyFieldTypeId { get; set; }
        public string CvlValueFieldTypeId { get; set; }
        public string CvlParentKeyFieldTypeId { get; set; }
        public bool IsAllowCvlEntityDeleted { get; set; } = false;
        public List<DefaultValueField> DefaultValueFields { get; set; }
    }
}
