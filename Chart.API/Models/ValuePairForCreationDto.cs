using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Models
{
    public class ValuePairForCreationDto
    {
        [Required]
        public double xValue { get; set; }
        [Required]
        public double yValue { get; set; }
    }
}
