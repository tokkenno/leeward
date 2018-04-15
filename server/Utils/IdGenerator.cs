using System;

namespace Leeward.Utils
{
    internal interface IdGenerator
    {
        UInt32 Next();
        Int32 NextInt();
        void Free(UInt32 id);
        void FreeInt(Int32 id);
    }
}