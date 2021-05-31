using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKinetic_CoinJar.Models
{
    public class CoinJars
    {
        public long ID { get; set; }

        public decimal CurrentValue { get; set; } 

        public decimal CurrentVolume { get; set; }

        public long ResetCounter { get; set; }

        public long CoinID { get; set; }
        [NotMapped]
        public string CoinName { get; set; } 

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ResetDate { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> CoinsSl { get; set; } 
    }
}
