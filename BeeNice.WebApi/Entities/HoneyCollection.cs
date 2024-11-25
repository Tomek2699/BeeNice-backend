namespace BeeNice.WebApi.Entities
{
    public class HoneyCollection
    {
        public long Id { get; set; }
        public DateTime CollectionDate { get; set; }
        public long HiveId { get; set; }
        public Hive Hive { get; set; }
        public float HoneyQuantity { get; set; }
        public string TypeOfHoney { get; set; }
    }
}
