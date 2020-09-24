using JsonSrcGen;

namespace JsonSrcGen.Samples.CoreRT
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