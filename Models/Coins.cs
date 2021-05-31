using GlobalKinetic_CoinJar.ExternalClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalKinetic_CoinJar.Models
{
    public class Coins : ICoin 
    {
        

        public long ID { get; set; }

        [Display(Name = "Coin Type")]
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }

        [Display(Name = "Coin Amount in terms of doller")]
        [Required]
        public decimal Amount { get; set; }


        [Display(Name = "Coin Volume in terms of cubic inches")]
        [Required]
        public decimal Volume { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

       
    }
}
