using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            Discussion = new HashSet<Discussion>();
            TicketEmployee = new HashSet<Ticket>();
            TicketSupporter = new HashSet<Ticket>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; } 
        public string Password { get; set; } 
        public string FullName { get; set; } 
        public bool Status { get; set; }
        public string Email { get; set; } 
        public int RoleId { get; set; }
        public string ? Phone { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Discussion> Discussion { get; set; }

        [NotMapped]
        public virtual ICollection<Ticket> TicketEmployee { get; set; }

        [NotMapped]
        public virtual ICollection<Ticket> TicketSupporter { get; set; }
       


    }
}
