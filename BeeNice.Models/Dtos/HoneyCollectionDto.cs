namespace BeeNice.Models.Dtos
{
    public class HoneyCollectionDto
    {
        public long Id { get; set; }
        public DateTime CollectionDate { get; set; }
        public long HiveId { get; set; }
        public float HoneyQuantity { get; set; }
        public string TypeOfHoney { get; set; }
    }
}
