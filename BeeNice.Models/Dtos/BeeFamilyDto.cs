namespace BeeNice.Models.Dtos
{
    public class BeeFamilyDto
    {
        public long Id { get; set; }
        public string FamilyNumber { get; set; }
        public string Race { get; set; }
        public string FamilyState { get; set; }
        public DateTime CreationDate { get; set; }
        public long HiveId { get; set; }
    }
}
