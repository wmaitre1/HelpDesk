using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models
{
    [Table("Period")]
    public partial class Period
    {
        public Period()
        {
            Ticket = new HashSet<Ticket>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } 
        public bool Status { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
