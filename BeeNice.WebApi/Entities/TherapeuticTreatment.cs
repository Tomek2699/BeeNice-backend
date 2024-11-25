namespace BeeNice.WebApi.Entities
{
    public class TherapeuticTreatment
    {
        public long Id { get; set; }
        public DateTime TreatmentDate { get; set; }
        public long FamilyId { get; set; }
        public BeeFamily BeeFamily { get; set; }
        public string Medicine { get; set; }
        public string Dose { get; set; }
        public string Comment { get; set; }
    }
}
