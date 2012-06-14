using System.Reflection;

namespace FlatFileParser
{
    public class ObjectInstance
    {
        public PropertyInfo[] PropertyInfos { get; set; }

        public object Instance { get; set; }
    }
}