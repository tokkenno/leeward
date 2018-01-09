namespace Leeward.Geometry
{
    public class Vector2<TE>
    {
        public TE X { get; set; }

        public TE Y { get; set; }

        public Vector2() : this(default(TE), default(TE))
        {
        }

        public Vector2(TE x, TE y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}