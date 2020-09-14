namespace JsonSrcGen.Generator.TypeGenerators
{
    public static class UniqueNumberGenerator
    {
        static int _number = 0;

        public static int UniqueNumber
        {
            get
            {
                var number = _number;
                _number++;
                return number;
            }
        }
    }
}