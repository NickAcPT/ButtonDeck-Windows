using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NickAc.Backend.Utils
{
    //Code taken and adapted from https://stackoverflow.com/a/6944605.
    //All credits go to Repo Man (https://stackoverflow.com/users/140126/repo-man)
    public static class ReflectiveEnumerator
    {
        public static T DeepClone<T>(this T obj)
        {
            
            return (T)XMLUtils.FromXML<T>(XMLUtils.ToXML(obj));
        }

        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            return objects;
        }
    }
}
