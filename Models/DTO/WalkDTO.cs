using Newtonsoft.Json;
using UdemyProject.Models.Domain;

namespace UdemyProject.Models.DTO
{
    public class WalkDTO
    {
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid DifficultyId { get; set; }
    }
}
