using UdemyProject.Models.DTO;

namespace UdemyProject.Models.Domain
{
    public class WalkDomain
    {
        public Guid Id { get; set; }

        public string Name { get; set; }    

        public double Length { get; set; }

        public Guid RegionId { get; set; }

        public Guid DifficultyId { get; set; }

        // Navigation Properties
        public RegionDomain Region { get; set; }
        public DifficultyDomain Difficulty { get; set; }

    }
}
