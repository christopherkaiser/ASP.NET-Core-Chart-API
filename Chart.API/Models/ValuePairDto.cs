using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Models
{
    public class ValuePairDto
    {
        public int Id { get; set; }
        public double xValue { get; set; }
        public double yValue { get; set; }
    }
}
