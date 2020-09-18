using System;

namespace JsonSrcGen
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class GenerationOutputFolderAttribute : Attribute
    {
        public GenerationOutputFolderAttribute(string path)
        {
            Path = path;
        }

        public string Path {get;}
    }
}