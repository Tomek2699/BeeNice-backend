namespace BeeNice.WebApi.Entities
{
    public class Queen
    {
        public long Id { get; set; }
        public string QueenNumber { get; set; }
        public string Race { get; set; }
        public DateTime HatchDate { get; set; }
        public string State { get; set; }
        public long HiveId { get; set; }
        public Hive Hive { get; set; }
    }
}
