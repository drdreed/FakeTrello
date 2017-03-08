using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.ReverseModels
{
    public class AspNetUsers
    {

        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }
        
        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        public bool TwoFactorEnabled { get; set; }

        public DateTime LockoutEndDateUtc { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        [Required]
        public int AccessFailedCount { get; set; }

        [MaxLength(256)]
        [Required]
        public string UserName { get; set; }
    }
}