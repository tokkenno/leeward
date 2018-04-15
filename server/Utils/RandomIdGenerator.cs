using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Leeward.Utils
{
    internal class RandomIdGenerator : IdGenerator
    {
        private const int Limit = 100000000;
        
        private readonly uint _gap;
        private readonly Random _generator;
        private readonly List<uint> _ids = new List<uint>();

        public RandomIdGenerator(uint gap = 0)
        {
            this._gap = gap;
            this._generator = new Random();
        }
        
        public uint Next()
        {
            uint newId = (uint) this._generator.Next(Limit - (int) this._gap) + this._gap;

            lock (this._ids)
            {
                if (this._ids.Contains(newId)) return Next();
                else
                {
                    this._ids.Add(newId);
                    return newId;
                }
            }
        }

        public int NextInt()
        {
            return (int) this.Next(); 
        }

        public void Free(uint id)
        {
            lock (this._ids)
            {
                if (this._ids.Contains(id)) this._ids.Remove(id);
            }
        }
        
        public void FreeInt(int id)
        {
            this.Free((uint) id);
        }
    }
}