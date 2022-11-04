using Microsoft.AspNetCore.Mvc.Rendering;


namespace HelpDesk.Models.ViewModels
{
    public class AccountViewModel
    {
        public Account Account { get; set; }

        public SelectList Roles {get; set;}
    }
}
