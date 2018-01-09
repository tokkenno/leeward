namespace Leeward.Geometry
{
    public class Bounds<TE>
    {
        public Vector3<TE> Center { get; set; }
        public Vector3<TE> Size { get; set; }

        public Bounds() : this(new Vector3<TE>(), new Vector3<TE>())
        {
        }

        public Bounds(Vector3<TE> center, Vector3<TE> size)
        {
            this.Center = center;
            this.Size = size;
        }
    }
}