using inRiver.Remoting.Objects;

namespace Avensia.Pim.Extensions.Runner.Models
{
    public class NewFieldTypeParam
    {
        public string Id { get; set; }
        public string EntityTypeId { get; set; }
        public string FieldDataType { get; set; } = DataType.String;
        public string CvlId { get; set; } = null;
        public string CategoryId { get; set; } = "General";
        public bool MultiValue { get; set; } = false;
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
