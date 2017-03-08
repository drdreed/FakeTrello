using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.ReverseModels
{
    public class AspNetRoles
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }
    }
}