namespace BeeNice.Models.Dtos
{
    public class QueenDto
    {
        public long Id { get; set; }
        public string QueenNumber { get; set; }
        public string Race { get; set; }
        public DateTime HatchDate { get; set; }
        public string State { get; set; }
        public long HiveId { get; set; }
    }
}
