using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avensia.CvlToEntity.Services;
using Avensia.CvlToEntity.Settings;
using Avensia.ModelMaster.Common.Inriver;
using inRiver.Remoting.Extension.Interface;
using inRiverCommunity.Extensions.Core.Settings;

namespace Avensia.CvlToEntity
{
    public class CvlToEntity: InRiverExtensionBase, ICVLListener
    {
        public void CVLValueCreated(string cvlId, string cvlValueKey)
        {
            ExecuteService<CvlToEntityService>(service => service.CreateOrUpdateEntity(cvlId, cvlValueKey));
        }

        public void CVLValueUpdated(string cvlId, string cvlValueKey)
        {
            ExecuteService<CvlToEntityService>(service => service.CreateOrUpdateEntity(cvlId, cvlValueKey));
        }

        public void CVLValueDeleted(string cvlId, string cvlValueKey)
        {
            ExecuteService<CvlToEntityService>(service => service.DeleteEntity(cvlId, cvlValueKey));
        }

        public new Dictionary<string, string> DefaultSettings => Context.GetSettingsAsDictionary<CvlToEntitySettings>();
        public void CVLValueDeletedAll(string cvlId){}
    }
}
