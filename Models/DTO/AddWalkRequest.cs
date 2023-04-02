namespace UdemyProject.Models.DTO
{
    public class AddWalkRequest
    {
        public string Name { get; set; }

        public double Length { get; set; }

        public Guid RegionId { get; set; }

        public Guid DifficultyId { get; set; }

    }
}
