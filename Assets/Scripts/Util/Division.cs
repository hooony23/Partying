namespace Util.Division
{
    public class Division2
    {
        private float x, y;
        public float X { get => x; set => x = value; }

        public float Y { get => y; set => y = value; }
        public override string ToString() => $"{x},{y}";
    }

    public class Division3
    {
        private float x, y, z;
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }
        public override string ToString() => $"{x},{y},{z}";
    }
}