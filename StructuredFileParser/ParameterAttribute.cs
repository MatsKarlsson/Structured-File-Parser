using System;

namespace FlatFileParser
{
	public class ParameterAttribute : Attribute
	{
		public ParameterAttribute(int index)
		{
			Index = index;
		}

		public ParameterAttribute()
		{
			Index = 0;
		}

		public int Index { get; set; }
	}
}