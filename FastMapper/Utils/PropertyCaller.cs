using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace FastMapper.Utils
{
    public sealed class PropertyCaller<T, K> where T : class
    {

        private static Dictionary<Type, Dictionary<Type, Dictionary<string, GenGetter>>> dGets =

            new Dictionary<Type, Dictionary<Type, Dictionary<string, GenGetter>>>();

        private static Dictionary<Type, Dictionary<Type, Dictionary<string, GenSetter>>> dSets =

            new Dictionary<Type, Dictionary<Type, Dictionary<string, GenSetter>>>();



        public delegate void GenSetter(T target, K value);

        public delegate K GenGetter(T target);



        private PropertyCaller()
        {

        }



        public static GenGetter CreateGetMethod(PropertyInfo pi)
        {

            //Create the locals needed.

            Type classType = typeof(T);

            Type returnType = typeof(K);

            string propertyName = pi.Name;



            //Let’s return the cached delegate if we have one.

            if (dGets.ContainsKey(classType))
            {

                if (dGets[classType].ContainsKey(returnType))
                {

                    if (dGets[classType][returnType].ContainsKey(propertyName))

                        return dGets[classType][returnType][propertyName];

                }

            }



            //If there is no getter, return nothing

            MethodInfo getMethod = pi.GetGetMethod();

            if (getMethod == null)

                return null;



            //Create the dynamic method to wrap the internal get method

            Type[] arguments = new Type[1];

            arguments[0] = typeof(T);



            DynamicMethod getter = new DynamicMethod(

                String.Concat("Get", pi.Name, "_"),

                typeof(K),

                new Type[] { typeof(T) },

                pi.DeclaringType);

            ILGenerator gen = getter.GetILGenerator();

            gen.DeclareLocal(typeof(K));

            gen.Emit(OpCodes.Ldarg_0);

            gen.Emit(OpCodes.Castclass, pi.DeclaringType);

            gen.EmitCall(OpCodes.Callvirt, getMethod, null);

            gen.Emit(OpCodes.Ret);



            //Create the delegate and return it

            GenGetter genGetter = (GenGetter)getter.CreateDelegate(typeof(GenGetter));



            //Cache the delegate for future use.

            Dictionary<Type, Dictionary<string, GenGetter>> tempDict = null;

            Dictionary<string, GenGetter> tempPropDict = null;

            if (!dGets.ContainsKey(classType))
            {

                tempPropDict = new Dictionary<string, GenGetter>();

                tempPropDict.Add(propertyName, genGetter);

                tempDict = new Dictionary<Type, Dictionary<string, GenGetter>>();

                tempDict.Add(returnType, tempPropDict);

                dGets.Add(classType, tempDict);

            }

            else
            {

                if (!dGets[classType].ContainsKey(returnType))
                {

                    tempPropDict = new Dictionary<string, GenGetter>();

                    tempPropDict.Add(propertyName, genGetter);

                    dGets[classType].Add(returnType, tempPropDict);

                }

                else
                {

                    if (!dGets[classType][returnType].ContainsKey(propertyName))

                        dGets[classType][returnType].Add(propertyName, genGetter);

                }

            }

            //Return delegate to the caller.

            return genGetter;

        }



        public static GenSetter CreateSetMethod(PropertyInfo pi)
        {

            //Create the locals needed.

            Type classType = typeof(T);

            Type returnType = typeof(K);

            string propertyName = pi.Name;



            //Let’s return the cached delegate if we have one.

            if (dSets.ContainsKey(classType))
            {

                if (dSets[classType].ContainsKey(returnType))
                {

                    if (dSets[classType][returnType].ContainsKey(propertyName))

                        return dSets[classType][returnType][propertyName];

                }

            }



            //If there is no setter, return nothing

            MethodInfo setMethod = pi.GetSetMethod();

            if (setMethod == null)

                return null;



            //Create dynamic method

            Type[] arguments = new Type[2];

            arguments[0] = typeof(T);

            arguments[1] = typeof(K);



            DynamicMethod setter = new DynamicMethod(

                String.Concat("_Set", pi.Name, "_"),

                typeof(void),

                arguments,

                pi.DeclaringType);

            ILGenerator gen = setter.GetILGenerator();

            gen.Emit(OpCodes.Ldarg_0);

            gen.Emit(OpCodes.Castclass, pi.DeclaringType);

            gen.Emit(OpCodes.Ldarg_1);



            if (pi.PropertyType.IsClass)

                gen.Emit(OpCodes.Castclass, pi.PropertyType);



            gen.EmitCall(OpCodes.Callvirt, setMethod, null);

            gen.Emit(OpCodes.Ret);



            //Create the delegate

            GenSetter genSetter = (GenSetter)setter.CreateDelegate(typeof(GenSetter));



            //Cache the delegate for future use.

            Dictionary<Type, Dictionary<string, GenSetter>> tempDict = null;

            Dictionary<string, GenSetter> tempPropDict = null;

            if (!dSets.ContainsKey(classType))
            {

                tempPropDict = new Dictionary<string, GenSetter>();

                tempPropDict.Add(propertyName, genSetter);

                tempDict = new Dictionary<Type, Dictionary<string, GenSetter>>();

                tempDict.Add(returnType, tempPropDict);

                dSets.Add(classType, tempDict);

            }

            else
            {

                if (!dSets[classType].ContainsKey(returnType))
                {

                    tempPropDict = new Dictionary<string, GenSetter>();

                    tempPropDict.Add(propertyName, genSetter);

                    dSets[classType].Add(returnType, tempPropDict);

                }

                else
                {

                    if (!dSets[classType][returnType].ContainsKey(propertyName))

                        dSets[classType][returnType].Add(propertyName, genSetter);

                }

            }

            //Return delegate to the caller.

            return genSetter;

        }

    }

    public static class PropertyCaller<T>
    {
        public delegate void GenSetter(T target, System.Object value);
        public delegate System.Object GenGetter(T target);

        private static Dictionary<Type, Dictionary<Type, Dictionary<string, GenGetter>>> dGets = new Dictionary<Type, Dictionary<Type, Dictionary<string, GenGetter>>>();
        private static Dictionary<Type, Dictionary<Type, Dictionary<string, GenSetter>>> dSets = new Dictionary<Type, Dictionary<Type, Dictionary<string, GenSetter>>>();

        public static GenGetter CreateGetMethod(PropertyInfo pi)
        {
            var classType = typeof(T);
            var propType = pi.PropertyType;
            var propName = pi.Name;

            Dictionary<Type, Dictionary<string, GenGetter>> i1 = null;
            if (dGets.TryGetValue(classType, out i1))
            {
                Dictionary<string, GenGetter> i2 = null;
                if (i1.TryGetValue(propType, out i2))
                {
                    GenGetter i3 = null;
                    if (i2.TryGetValue(propName, out i3))
                    {
                        return i3;
                    }
                }
            }

            //If there is no getter, return nothing
            var getMethod = pi.GetGetMethod();
            if (getMethod == null)
            {
                return null;
            }

            //Create the dynamic method to wrap the internal get method
            var arguments = new Type[1] { classType };

            var getter = new DynamicMethod(String.Concat("_Get", pi.Name, "_"), typeof(object), new Type[] { typeof(T) }, classType);
            var generator = getter.GetILGenerator();
            generator.DeclareLocal(propType);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, classType);
            generator.EmitCall(OpCodes.Callvirt, getMethod, null);
            if (!propType.IsClass)
                generator.Emit(OpCodes.Box, propType);
            generator.Emit(OpCodes.Ret);

            //Create the delegate and return it
            GenGetter genGetter = (GenGetter)getter.CreateDelegate(typeof(GenGetter));

            Dictionary<Type, Dictionary<string, GenGetter>> tempDict = null;
            Dictionary<string, GenGetter> tempPropDict = null;
            if (!dGets.ContainsKey(classType))
            {
                tempPropDict = new Dictionary<string, GenGetter>();
                tempPropDict.Add(propName, genGetter);
                tempDict = new Dictionary<Type, Dictionary<string, GenGetter>>();
                tempDict.Add(propType, tempPropDict);
                dGets.Add(classType, tempDict);
            }
            else
            {
                if (!dGets[classType].ContainsKey(propType))
                {
                    tempPropDict = new Dictionary<string, GenGetter>();
                    tempPropDict.Add(propName, genGetter);
                    dGets[classType].Add(propType, tempPropDict);
                }
                else
                {
                    if (!dGets[classType][propType].ContainsKey(propName))
                    {
                        dGets[classType][propType].Add(propName, genGetter);
                    }
                }
            }
            return genGetter;
        }

        public static GenSetter CreateSetMethod(PropertyInfo pi)
        {
            Type classType = typeof(T);
            Type propType = pi.PropertyType;
            string propName = pi.Name;

            Dictionary<Type, Dictionary<string, GenSetter>> i1 = null;
            if (dSets.TryGetValue(classType, out i1))
            {
                Dictionary<string, GenSetter> i2 = null;
                if (i1.TryGetValue(propType, out i2))
                {
                    GenSetter i3 = null;
                    if (i2.TryGetValue(propName, out i3))
                    {
                        return i3;
                    }
                }
            }

            //If there is no setter, return nothing
            MethodInfo setMethod = pi.GetSetMethod();
            if (setMethod == null)
            {
                return null;
            }

            //Create dynamic method
            Type[] arguments = new Type[2] { classType, typeof(object) };

            DynamicMethod setter = new DynamicMethod(String.Concat("_Set", pi.Name, "_"), typeof(void), arguments, classType);
            ILGenerator generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, classType);
            generator.Emit(OpCodes.Ldarg_1);

            if (propType.IsClass)
                generator.Emit(OpCodes.Castclass, propType);
            else
                generator.Emit(OpCodes.Unbox_Any, propType);

            generator.EmitCall(OpCodes.Callvirt, setMethod, null);
            generator.Emit(OpCodes.Ret);

            //Create the delegate and return it
            GenSetter genSetter = (GenSetter)setter.CreateDelegate(typeof(GenSetter));

            Dictionary<Type, Dictionary<string, GenSetter>> tempDict = null;
            Dictionary<string, GenSetter> tempPropDict = null;
            if (!dSets.ContainsKey(classType))
            {
                tempPropDict = new Dictionary<string, GenSetter>();
                tempPropDict.Add(propName, genSetter);
                tempDict = new Dictionary<Type, Dictionary<string, GenSetter>>();
                tempDict.Add(propType, tempPropDict);
                dSets.Add(classType, tempDict);
            }
            else
            {
                if (!dSets[classType].ContainsKey(propType))
                {
                    tempPropDict = new Dictionary<string, GenSetter>();
                    tempPropDict.Add(propName, genSetter);
                    dSets[classType].Add(propType, tempPropDict);
                }
                else
                {
                    if (!dSets[classType][propType].ContainsKey(propName))
                    {
                        dSets[classType][propType].Add(propName, genSetter);
                    }
                }
            }

            return genSetter;
        }
    }

    public delegate void GenericSetter(object target, object value);
    public delegate object GenericGetter(object target);

    public static class PropertyCaller
    {
        ///
        /// Creates a dynamic setter for the property
        ///
        public static GenericSetter CreateSetMethod(PropertyInfo propertyInfo)
        {
            /*
            * If there's no setter return null
            */
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            if (setMethod == null)
                return null;

            /*
            * Create the dynamic method
            */
            Type[] arguments = new Type[2];
            arguments[0] = arguments[1] = typeof(object);

            DynamicMethod setter = new DynamicMethod(
              String.Concat("_Set", propertyInfo.Name, "_"),
              typeof(void), arguments, propertyInfo.DeclaringType);
            ILGenerator generator = setter.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.Emit(OpCodes.Ldarg_1);

            if (propertyInfo.PropertyType.IsClass)
                generator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
            else
                generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);

            generator.EmitCall(OpCodes.Callvirt, setMethod, null);
            generator.Emit(OpCodes.Ret);

            /*
            * Create the delegate and return it
            */
            return (GenericSetter)setter.CreateDelegate(typeof(GenericSetter));
        }

        ///
        /// Creates a dynamic getter for the property
        ///
        public static GenericGetter CreateGetMethod(PropertyInfo propertyInfo)
        {
            /*
            * If there's no getter return null
            */
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
                return null;

            /*
            * Create the dynamic method
            */
            Type[] arguments = new Type[1];
            arguments[0] = typeof(object);

            DynamicMethod getter = new DynamicMethod(
              String.Concat("_Get", propertyInfo.Name, "_"),
              typeof(object), arguments, propertyInfo.DeclaringType);
            ILGenerator generator = getter.GetILGenerator();
            generator.DeclareLocal(typeof(object));
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
            generator.EmitCall(OpCodes.Callvirt, getMethod, null);

            if (!propertyInfo.PropertyType.IsClass)
                generator.Emit(OpCodes.Box, propertyInfo.PropertyType);

            generator.Emit(OpCodes.Ret);

            /*
            * Create the delegate and return it
            */
            return (GenericGetter)getter.CreateDelegate(typeof(GenericGetter));
        }
    }
}
