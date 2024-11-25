namespace BeeNice.WebApi.Entities
{
    public class Review
    {
        public long Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public long FamilyId { get; set; }
        public BeeFamily BeeFamily { get; set; }
        public string FamilyState { get; set; }
        public string Comment { get; set; }
    }
}
