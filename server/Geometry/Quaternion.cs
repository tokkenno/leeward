namespace Leeward.Geometry
{
    public class Quaternion : Vector4<float>
    {
        public Quaternion() : base(0, 0, 0, 0)
        {
        }

        public Quaternion(float x, float y, float z, float w) : base(x, y, z, w)
        {
        }
    }
}