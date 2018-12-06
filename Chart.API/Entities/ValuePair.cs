using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chart.API.Entities
{
    public class ValuePair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public double xValue { get; set; }

        [Required]
        public double yValue { get; set; }

        [ForeignKey("ChartId")]
        public Chart Chart { get; set; }
        public int ChartId { get; set; }
    }
}
