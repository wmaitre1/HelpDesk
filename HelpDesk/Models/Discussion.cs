using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models
{
    [Table("Discussion")]
    public partial class Discussion
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? TicketId { get; set; }
        public int? AccountId { get; set; }
    }
}
