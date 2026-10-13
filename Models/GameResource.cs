
namespace Imperialism2SaveEditor.Models
{
    public class GameResource
    {
        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public int Offset { get; set; }

        public GameResource(
            string name,
            int quantity,
            int offset)
        {
            Name = name;
            Quantity = quantity;
            Offset = offset;
        }
    }
}
