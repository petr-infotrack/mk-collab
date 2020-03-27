using System;
using System.Linq;
using System.Reflection;

namespace AlertsAdmin.Monitor.Filters
{
    public static class FilterUtils
    {
        public static Type GetModelType(string typeName)
        {
            var library = Assembly.GetExecutingAssembly();
            return library.GetTypes().FirstOrDefault(x => x.Name.Equals(typeName));
        }

        public static Type GetModelType(string libraryName, string typeName)
        {
            var library = Assembly.LoadFrom(libraryName);
            return library.GetTypes().FirstOrDefault(x => x.Name.Equals(typeName));
        }
    }
}