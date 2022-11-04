using Microsoft.AspNetCore.Mvc.Rendering;


namespace HelpDesk.Models.ViewModels
{
    public class TicketViewModel
    {
        public Ticket ticket { get; set; }

        public SelectList Categories {get; set;}
        public SelectList Statuses { get; set; }
        public SelectList Periods { get; set; }
    }
}
