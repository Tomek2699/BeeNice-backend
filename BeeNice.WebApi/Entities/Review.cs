namespace BeeNice.WebApi.Entities
{
    public class Review
    {
        public long Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public string FamilyState { get; set; }
        public string Comment { get; set; }
        public long HiveId { get; set; }
        public Hive Hive { get; set; }
    }
}
