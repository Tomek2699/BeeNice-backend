namespace BeeNice.Models.Dtos
{
    public class ReviewDto
    {
        public long Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public long FamilyId { get; set; }
        public string FamilyState { get; set; }
        public string Comment { get; set; }
    }
}
