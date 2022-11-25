using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avensia.CvlToEntity.Interfaces
{
    public interface ICvlToEntityService
    {
        void CreateOrUpdateEntity(string cvlId, string cvlKey);
        void DeleteEntity(string cvlId, string cvlKey);
    }
}
