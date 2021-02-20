namespace Util.Division
{
    public class Division2
    {
        private float x, y;
        
        public Division2() :this(0,0){}
        public Division2(float x, float y)
        {
            this.x=x;
            this.y=y;
        }
        public float X { get => x; set => x = value; }

        public float Y { get => y; set => y = value; }
        public override string ToString() => $"{x},{y}";
    }

    public class Division3
    {
        private float x, y, z;
        public Division3() :this(0,0,0){}
        public Division3(float x, float y, float z)
        {
            this.x=x;
            this.y=y;
            this.z=z;
        }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }
        public override string ToString() => $"{x},{y},{z}";
    }
}