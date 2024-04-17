using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsightAcademy.ApplicationLayer.Models
{
    public class TutorFilter
    {
        public Guid SubjectId { get; set; }
        public string Order { get; set; }
        public string Location { get; set; }

        public decimal MinPrice {  get; set; }
        public decimal MaxPrice {  get; set; }

        public string service { get; set; }

        public List<Guid> SubjectIds { get; set; } = new List<Guid>();

    }
}
