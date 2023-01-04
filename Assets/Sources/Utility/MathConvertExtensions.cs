namespace Utility
{
    public static class MathConvertExtensions
    {
        public static System.Numerics.Vector3 Convert(this UnityEngine.Vector3 vector3) =>
            new(vector3.x, vector3.y, vector3.z);

        public static UnityEngine.Vector3 Convert(this System.Numerics.Vector3 vector3) =>
            new(vector3.X, vector3.Y, vector3.Z);
    }
}