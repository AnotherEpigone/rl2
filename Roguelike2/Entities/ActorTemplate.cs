namespace Roguelike2.Entities
{
    public class ActorTemplate
    {
        public ActorTemplate(
            string id,
            string name,
            int glyph)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
        }

        public string Id { get; }
        public string Name { get; }
        public int Glyph { get; }
    }
}
