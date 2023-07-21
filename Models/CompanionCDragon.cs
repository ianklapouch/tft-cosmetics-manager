namespace tft_cosmetics_manager.Models
{
    public class CompanionCDragon
    {
        public string? ContentId { get; set; }
        public int ItemId { get; set; }
        public string? Name { get; set; }
        public string? LoadoutsIcon { get; set; }
        public string? Description { get; set; }
        public int Level { get; set; }
        public string? SpeciesName{ get; set; }
        public int SpeciesId { get; set; }
        public string? Rarity { get; set; }
        public int RarityValue {get; set; }
        public bool IsDefault { get; set; }
        public string[]? Upgrades{ get; set; }
        public bool TFTOnly { get; set; }
    }
}