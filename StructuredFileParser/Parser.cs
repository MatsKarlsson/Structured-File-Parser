using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FlatFileParser
{
    public class Parser : IParser
    {
        private Regex _rowRegex;

        public ObjectInstance ParseLine(ParseEntry parseEntry, string line)
        {
            if (parseEntry == null)
            {
                throw new ArgumentException("ParseEntry parameter needed to parse line", "parseEntry");
            }
            if (line == null)
            {
                throw new ArgumentException("Line to parse needs to be specified", "line");
            }

            var match = parseEntry.ParseFormat.Match(line);
            if (match.Groups.Count == 1)
            {
                return null;
            }

            if (parseEntry.PropertyInfos == null)
            {
                parseEntry.PropertyInfos = parseEntry.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }

            //Create instance
            var inst = Activator.CreateInstance(parseEntry.Type);

            foreach (var propertyInfo in parseEntry.PropertyInfos)
            {
                SetPropertyValue(match.Groups[propertyInfo.Name].Value, inst, propertyInfo);
            }

            return new ObjectInstance { Instance = inst, PropertyInfos = parseEntry.PropertyInfos};

        }

        public bool TryParseGenericList(ParseEntry parseEntry, ObjectInstance newObjectInstance, ObjectInstance objectInstance)
        {
            var types = objectInstance.PropertyInfos.Where(
                d =>
                d.PropertyType.IsGenericType && d.PropertyType.GetGenericArguments().Length == 1 &&
                d.PropertyType.GetGenericArguments()[0] == parseEntry.Type).ToArray();

            foreach (var propertyInfo in types)
            {
                var typeArgs = new[] { propertyInfo.PropertyType.GetGenericArguments()[0] };
                var listType = typeof(List<>).MakeGenericType(typeArgs);

                if (listType == propertyInfo.PropertyType)
                {
                    if (!string.IsNullOrEmpty(parseEntry.ParentAttributeName) && parseEntry.ParentAttributeName != propertyInfo.Name)
                        continue;

                    var propInfo = objectInstance.Instance.GetType().GetProperties().First(d => d.PropertyType == listType);

                    var listInstance = propInfo.GetValue(objectInstance.Instance, null);
                    if (listInstance == null)
                    {
                        listInstance = Activator.CreateInstance(listType);
                        propInfo.SetValue(objectInstance.Instance, listInstance, null);
                    }

                    var addMethod = listType.GetMethod("Add");
                    addMethod.Invoke(listInstance, new[] { newObjectInstance.Instance });
                    return true;
                }
            }
            return false;
        }


        private static void SetPropertyValue(string value, object inst, PropertyInfo propertyInfo)
        {
            if ((propertyInfo.PropertyType == typeof(double) || propertyInfo.PropertyType == typeof(double?)) && !string.IsNullOrEmpty(value))
            {
                propertyInfo.SetValue(inst, Convert.ToDouble(value), null);
                return;
            }

            if ((propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?)) && !string.IsNullOrEmpty(value))
            {
                propertyInfo.SetValue(inst, Convert.ToInt32(value), null);
                return;
            }

            if (propertyInfo.PropertyType != typeof(string))
            {
                return;
            }

            propertyInfo.SetValue(inst, value, null);
        }


        public ObjectInstance LoadRoot(Type rootNode)
        {
            //Get rootnode-attrs and parse it
            //var rootNode = typeof (T);
            var attrs = rootNode.GetCustomAttributes(typeof(ParseRootAttribute), false);
            if (attrs.Length != 1)
            {
                throw new InvalidOperationException("Invalid Root Node T, needs to have a ParseRootAttribute");
            }

            //Remember row Regex that extracts row-node
            _rowRegex = new Regex(((ParseRootAttribute)attrs[0]).Format);


            //Create instance and add to bottom of stack
            var rootInstance = Activator.CreateInstance(rootNode);
            var propertyInfos = rootNode.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return new ObjectInstance {Instance = rootInstance, PropertyInfos = propertyInfos};
        }

        public string GetKeyFromLine(string line)
        {
            var match = _rowRegex.Match(line);
            return match.Groups[1].Value;
        }
    }
}