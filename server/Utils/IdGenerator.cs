using System;
using System.Collections.Generic;
using System.Linq;

namespace Leeward.Utils
{
    internal interface IDGenerator
    {
        int Next();
        void Free(int id);
    }

    internal class SequentialIdGenerator : IDGenerator
    {
        private readonly List<int> _ids = new List<int>();
        private int _current = -1;
        
        public int Next()
        {
            lock (this._ids)
            {
                if (_ids.Count == _current + 1)
                {
                    this._current += 1;
                    this._ids.Add(this._current);
                    return this._current;
                }
                else
                {
                    this._ids.Sort();
                    int setNum = this._ids.TakeWhile(((elem, ind) => elem == ind)).Count();
                    this._ids.Add(setNum);
                    return setNum;
                }
            }
        }

        public void Free(int id)
        {
            lock (this._ids)
            {
                if (this._ids.Contains(id))
                    this._ids.Remove(id);
            }
        }
    }
}