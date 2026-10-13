
namespace Imperialism2SaveEditor.Models
{
    public class GameNation
    {
        public string Name { get; set; } = string.Empty;

        public int NationId { get; set; } = -1;

        public bool IsHumanControlled { get; set; }

        public int TreasuryOffset { get; set; } = -1;

        public int WarehouseOffset { get; set; } = -1;
    }
}
