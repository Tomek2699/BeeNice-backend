﻿namespace BeeNice.WebApi.Entities
{
    public class TherapeuticTreatment
    {
        public long Id { get; set; }
        public DateTime TreatmentDate { get; set; }
        public string Medicine { get; set; }
        public string Dose { get; set; }
        public string Comment { get; set; }
        public long HiveId { get; set; }
        public Hive Hive { get; set; }
    }
}
