using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models
{
    [Table("Ticket")]
    public partial class Ticket
    {
        public Ticket()
        {
            Discussion = new HashSet<Discussion>();
            Photo = new HashSet<Photo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; } 
        public DateTime CreatedDate { get; set; }
        public int StatusId { get; set; }
        public int CategoryId { get; set; }
        public int PeriodId { get; set; }
        public int EmployeeId { get; set; }
        public int ? SupporterId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Account Employee { get; set; }
        public virtual Period Period { get; set; }
        public virtual Status Status { get; set; }
        public virtual Account Supporter { get; set; }
        public virtual ICollection<Discussion> Discussion { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
