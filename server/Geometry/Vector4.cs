namespace Leeward.Geometry
{
    public class Vector4<TE> : Vector3<TE>
    {
        public TE W { get; set; }

        public Vector4() : this(default(TE), default(TE), default(TE), default(TE))
        {
        }

        public Vector4(TE x, TE y, TE z, TE w) : base(x, y, z)
        {
            this.W = w;
        }
    }
}