namespace UdemyProject.Models.DTO
{
    public class RegionDTO
    {
        public Guid RegionId { get; set; }

        public string RegionCode { get; set; }

        public string RegionName { get; set; }

        public double RegionArea { get; set; }

        public double RegionLat { get; set; }

        public double RegionLong { get; set; }

        public long RegionPopulation { get; set; }
    }
}
