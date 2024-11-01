using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsportsRoomAttendance.Pages
{
    public class CreateModel : PageModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Uuid { get; set; } // Optional field for updates

        public void OnGet()
        {
        }
    }
}
