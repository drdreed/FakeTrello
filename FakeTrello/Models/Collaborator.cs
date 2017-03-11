using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FakeTrello.Models
{
    public class Collaborator
    {
        [Key]
        public int CollaboratorId { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public int CardId { get; set; }
    }
}