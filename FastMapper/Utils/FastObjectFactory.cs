using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FastMapper.Utils
{
    public static class FastObjectFactory
    {
        private static readonly Hashtable creatorCache = Hashtable.Synchronized(new Hashtable());
        private readonly static Type coType = typeof(CreateObject);
        public delegate object CreateObject();

        /// <summary>
        /// Create a new instance of the specified type
        /// </summary>
        /// <returns></returns>
        public static CreateObject CreateObjectFactory<T>() where T : class
        {
            Type t = typeof(T);
            FastObjectFactory.CreateObject c = creatorCache[t] as FastObjectFactory.CreateObject;
            if (c == null)
            {
                lock (creatorCache.SyncRoot)
                {
                    c = creatorCache[t] as FastObjectFactory.CreateObject;
                    if (c != null)
                    {
                        return c;
                    }
                    DynamicMethod dynMethod = new DynamicMethod("DM$OBJ_FACTORY_" + t.Name, typeof(object), null, t);
                    ILGenerator ilGen = dynMethod.GetILGenerator();

                    ilGen.Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Ret);
                    c = (CreateObject)dynMethod.CreateDelegate(coType);
                    creatorCache.Add(t, c);
                }
            }
            return c;
        }
    }
}
