namespace BeeNice.WebApi.Entities
{
    public class Hive
    {
        public long Id { get; set; }
        public string HiveNumber { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public long ApiaryId { get; set; }
        public Apiary Apiary { get; set; }

        public List<BeeFamily> BeeFamilies { get; set; }
    }
}
