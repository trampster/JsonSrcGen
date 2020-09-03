using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace JsonSGen.Generator.PropertyHashing
{
    internal class PropertyHash
    {
        internal int Column {get;set;}
        internal bool UseLength{get;set;}
        internal int ModValue{get;set;}
        internal int CollisionCount {get;set;}

        internal int Hash(string property)
        {
            if(UseLength) 
            {
                return property.Length % ModValue;
            }
            return property[Column % property.Length] % ModValue;
        }

        internal string GenerateHashCode()
        {
            if(UseLength)
            {
                return $"propertyName.Length % {ModValue}";
            }
            return $"propertyName[{Column} % propertyName.Length] % {ModValue}";
        }

        internal bool IsBetterHash(PropertyHash otherHash)
        {
            if(otherHash.CollisionCount < CollisionCount)
            {
                return true;
            }
            if(otherHash.CollisionCount > CollisionCount)
            {
                return false;
            }
            //same number collisions, use the one with the smallest mod
            if(otherHash.ModValue > ModValue)
            {
                return false;
            }
            return true;
        }
    }
}