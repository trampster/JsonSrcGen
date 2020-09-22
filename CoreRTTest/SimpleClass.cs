using JsonSrcGen;

namespace CoreRTTest
{
    [Json]
    public class SimpleClass
    {
        public int Age
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}