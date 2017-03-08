using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.ReverseModels
{
    public class AspNetUserRoles
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        [Key]
        [MaxLength(128)]
        public string RoleId { get; set; }

        public ICollection<AspNetUsers> Users { get; set; }
    }
}