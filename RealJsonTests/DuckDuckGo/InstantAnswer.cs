using System;
using JsonSrcGen;

namespace JsonSrcGen.RealJsonTests.DuckDuckGo
{
    [Json]
    public class InstantAnswer
    {
        public string Abstract { get; set; }
        public string AbstractText { get; set; }
        public string Image { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
    }
}