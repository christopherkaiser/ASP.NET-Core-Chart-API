using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Models
{
    public class ChartDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfValuePairs
        {
            get
            {
                return ValuePairs.Count;
            }
        }

        public ICollection<ValuePairDto> ValuePairs { get; set; }
            = new List<ValuePairDto>();
    }
}
