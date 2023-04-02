using Newtonsoft.Json;
using UdemyProject.Models.Domain;

namespace UdemyProject.Models.DTO
{
    public class WalkDTO
    {
        public Guid WalkId { get; set; }

        public string WalkName { get; set; }

        public double WalkLength { get; set; }

        public Guid WalkRegionId { get; set; }

        public Guid WalkDifficultyId { get; set; }

        public RegionDomain WalkRegion { get; set; }

        public DifficultyDomain WalkDifficulty { get; set; }
    }
}
