using System;
using System.Collections.Generic;
using System.Linq;

namespace Leeward.Utils
{
    internal class SequentialIdGenerator : IdGenerator
    {
        private readonly List<uint> _ids = new List<uint>();
        private uint _current = 0;
        
        public uint Next()
        {
            lock (this._ids)
            {
                if (_ids.Count == _current)
                {
                    this._current += 1;
                    this._ids.Add(this._current);
                    return this._current;
                }
                else
                {
                    this._ids.Sort();
                    uint setNum = (UInt32) this._ids.TakeWhile(((elem, ind) => elem == ind)).Count();
                    this._ids.Add(setNum);
                    return setNum;
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
                if (this._ids.Contains(id))
                    this._ids.Remove(id);
            }
        }
        
        public void FreeInt(int id)
        {
            this.Free((uint) id);
        }
    }
}