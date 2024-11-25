using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeeNice.WebApi.Entities
{
    public class Apiary
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }

        public IdentityUser User { get; set; }
        public List<Hive> Hives { get; set; }
    }
}
