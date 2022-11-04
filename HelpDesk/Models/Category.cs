using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models
{
    [Table("Category")]
    public partial class Category
    {
        public Category()
        {
            Ticket = new HashSet<Ticket>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Id  { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
