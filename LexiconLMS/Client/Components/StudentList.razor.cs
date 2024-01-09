using LexiconLMS.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace LexiconLMS.Client.Components
{
    public partial class StudentList
    {
        [Parameter]
        public List<ApplicationUser>? StudentLst { get; set; }

        [Parameter]
        public Course? Course { get; set; }
    }
}
