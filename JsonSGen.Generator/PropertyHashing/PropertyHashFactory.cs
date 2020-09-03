using System.Collections.Generic;
using System.Linq;
using System;

namespace JsonSGen.Generator.PropertyHashing
{
    internal class PropertyHashFactory
    {
        internal PropertyHash FindBestHash(string[] properties)
        {
            var bestHash = new PropertyHash()
            {
                Column = 0,
                ModValue = int.MaxValue,
                CollisionCount = int.MaxValue
            };

            //try column values
            var sortedColumnCollisions = FindColumnCollisions(properties).OrderBy(collisions => collisions.NumberOfCollisions);
            foreach(var columnCollision in sortedColumnCollisions.Take(3))
            {
                (var bestMod, var collisionCount) = FindBestMod(properties, property => property[columnCollision.ColumnIndex % property.Length]);
                var candidateHash = new PropertyHash()
                {
                    Column = columnCollision.ColumnIndex,
                    ModValue = bestMod,
                    CollisionCount = collisionCount,
                };
                if(bestHash.IsBetterHash(candidateHash))
                {
                    bestHash = candidateHash;
                }
            }

            //try length
            (var bestModForLength, var collisionCountLength) = FindBestMod(properties, property => property.Length);
            var lengthCandidateHash = new PropertyHash()
            {
                UseLength = true,
                ModValue = bestModForLength,
                CollisionCount = collisionCountLength
            };
            if(bestHash.IsBetterHash(lengthCandidateHash))
            {
                bestHash = lengthCandidateHash;
            }

            return bestHash;
        }

        (int, int) FindBestMod(string[] properties, Func<string, int> getHash)
        {
            int[] primesToTry = new int[]
            {
                2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 
                103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 
                211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 
                331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 
                449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 
                587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 
                709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 
                853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 
                991, 997
            };

            int bestMod = 0;
            int numberOfCollisions = int.MaxValue;
            foreach(var modToTry in new int[]{properties.Length}.Concat(primesToTry))
            {
                if(modToTry < properties.Length)
                {
                    continue;
                }
                var collisions = CollisionsForMod(properties, getHash, modToTry);
                if(collisions == 0)
                {
                    return (modToTry, collisions); // found a perfect mod
                }
                if(collisions < numberOfCollisions)
                {
                    //found a better mod
                    bestMod = modToTry;
                    numberOfCollisions = collisions;
                }
            }
            return (bestMod, numberOfCollisions);
        }

        int CollisionsForMod(string[] properties, Func<string, int> getHash, int mod)
        {
            var hashes = new HashSet<int>();
            int collisionsFound = 0;
            foreach(var property in properties)
            {
                var hash = getHash(property) % mod;
                if(!hashes.Add(hash))
                {
                    collisionsFound++;
                }
            }
            return collisionsFound;
        }

        ColumnCollisions[] FindColumnCollisions(string[] properties)
        {
            int longestProperty = properties.Max(property => property.Length);
            var collisions = new ColumnCollisions[longestProperty];
            for(int index = 0; index < longestProperty; index++)
            {
                collisions[index] = new ColumnCollisions(index);
            }
            for(int columnIndex = 0; columnIndex < longestProperty; columnIndex++)
            {
                var charactersFound = new HashSet<char>();
                foreach(var property in properties)
                {
                    var character = property[columnIndex % property.Length];
                    if(!charactersFound.Add(character))
                    {
                        collisions[columnIndex].AddCollision();
                    }
                }
            }
            return collisions;
        }
    }
}