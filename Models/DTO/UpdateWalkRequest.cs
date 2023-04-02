namespace UdemyProject.Models.DTO
{
    public class UpdateWalkRequest
    {
        public string Name { get; set; }

        public double Length { get; set; }

        public Guid RegionId { get; set; }

        public Guid DifficultyId { get; set; }
    }
}
