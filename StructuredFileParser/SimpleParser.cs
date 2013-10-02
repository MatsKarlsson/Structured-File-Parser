using System;
using System.Collections.Generic;
using System.Reflection;

namespace FlatFileParser
{
	public class SimpleParser<T> where T : class
	{

		private readonly Type _type = typeof(T);
		private List<SimpleProperty> _properties;

		public T ParseLine(string line, string delimiter)
		{
			var arr = line.Split(new[] { delimiter }, StringSplitOptions.None);

			if (_properties == null)
				_properties = GetProperties();

			var inst = Activator.CreateInstance(_type);

			foreach (var simpleProperty in _properties)
			{
				SetPropertyValue(inst, simpleProperty, arr);
			}
			return inst as T;
		}

		private void SetPropertyValue(object inst, SimpleProperty simpleProperty, string[] arr)
		{
			var propType = simpleProperty.PropertyInfo.PropertyType;
			if (propType == typeof(string))
			{
				if (arr.Length > simpleProperty.Index) 
					simpleProperty.PropertyInfo.SetValue(inst, arr[simpleProperty.Index], null);
			}

		}

		private List<SimpleProperty> GetProperties()
		{
			var simpleProperties = new List<SimpleProperty>();

			var propertyInfos = _type.GetProperties();
			foreach (var propertyInfo in propertyInfos)
			{
				var attrs = propertyInfo.GetCustomAttributes(typeof(ParameterAttribute), false) as ParameterAttribute[];

				if (attrs != null && attrs.Length > 0)
				{
					simpleProperties.Add(new SimpleProperty
					{
						Index = attrs[0].Index,
						PropertyInfo = propertyInfo
					});
				}
			}

			return simpleProperties;
		}
	}

	internal class SimpleProperty
	{
		public int Index { get; set; }
		public PropertyInfo PropertyInfo { get; set; }
	}
}