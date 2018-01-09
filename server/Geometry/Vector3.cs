namespace Leeward.Geometry
{
    public class Vector3<TE> : Vector2<TE>
    {
        public TE Z { get; set; }

        public Vector3() : this(default(TE), default(TE), default(TE))
        {
        }

        public Vector3(TE x, TE y, TE z) : base(x, y)
        {
            this.Z = z;
        }
    }
}