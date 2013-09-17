using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FlatFileParser
{
	public class RegExParser<T> where T : class, new()
	{
		private IParseEntries _parseEntries;
		private readonly IParser _parser;

		private readonly Stack<ObjectInstance> _objectInstanceStack = new Stack<ObjectInstance>();

		private readonly List<ParseErrors> _parseErrors = new List<ParseErrors>();

		public IList<ParseErrors> ParseErrors { get { return _parseErrors; } }

		public RegExParser()
		{
			_parseEntries = new ParseEntries();
			_parser = new Parser();
		}

		public RegExParser(IParseEntries parseEntries, IParser parser)
		{
			_parseEntries = parseEntries;
			_parser = parser;
		}

		private T Initialize()
		{
			//Flush entries
			_parseEntries.Clear();
			_objectInstanceStack.Clear();
			_parseErrors.Clear();


			var objectInstance = _parser.LoadRoot(typeof(T));

			_objectInstanceStack.Push(objectInstance);

			_parseEntries.Load(typeof(T));
			return (T)objectInstance.Instance;
		}

		public T Parse(Stream stream)
		{
			var rootInstance = Initialize();
			var rootNamespace = rootInstance.GetType().Namespace;

			using (var sr = new StreamReader(stream))
			{
				string line;
				int row = 1;
				while ((line = sr.ReadLine()) != null)
				{
					ProcessLine(rootNamespace, row++, line);
				}
			}
			return rootInstance;
		}

		public T ParseString(string data)
		{
			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
				return Parse(ms);
		}

		public T Parse(string file)
		{
			using (var stream = File.Open(file, FileMode.Open))
				return Parse(stream);
		}

		public IList<T> ParseArray(string file)
		{
			using (var stream = File.Open(file, FileMode.Open))
				return ParseArray(stream);
		}

		public IList<T> ParseStringAsArray(string data)
		{
			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
				return ParseArray(ms);
		}


		public IList<T> ParseArray(Stream stream)
		{
			var list = new List<T>();

			var type = typeof(T);
			var attr = type.GetCustomAttributes(typeof(ParseFormatAttribute), false)[0] as ParseFormatAttribute;
			if (attr == null)
			{
				AddError(string.Format("Object of type {0} doesn't have any ParseFormatAttributes", typeof(T).Name), 0);
				return null;
			}

			var parseEntry = new ParseEntry { Type = type, ParseFormat = new Regex(attr.Format), ParentAttributeName = attr.ParentAttributeName };


			using (var sr = new StreamReader(stream))
			{
				string line;
				int row = 1;

				while ((line = sr.ReadLine()) != null)
				{
					var parseType = _parser.ParseLine(parseEntry, line);
					if (parseType == null)
					{
						AddError(string.Format("Problem adding node for line: {0}", line), row);
						continue;
					}
					list.Add(parseType.Instance as T);
					row++;
				}
			}
			return list;
		}

		private void ProcessLine(string rootNamespace, int row, string line)
		{
			var rowIdentifier = string.Format("{0}.{1}", rootNamespace, _parser.GetKeyFromLine(line));

			if (_parseEntries.ContainsRowIdentifier(rowIdentifier))
			{
				var parseEntry = _parseEntries[rowIdentifier];
				if (!ParseLineAndSetNode(parseEntry, line))
				{
					AddError(string.Format("Problem adding node for line: {0}", line), row);
				}
			}
			else
			{
				AddError(string.Format("Node not found for row: {0}", line), row);
			}
		}

		private bool ParseLineAndSetNode(ParseEntry parseEntry, string line)
		{
			var newObjectInstance = _parser.ParseLine(parseEntry, line);
			if (newObjectInstance == null)
			{
				return false;
			}

			while (_objectInstanceStack.Count >= 1)
			{
				var objectInstanceFromStack = _objectInstanceStack.Peek();
				var propInfos = objectInstanceFromStack.PropertyInfos.Where(d => d.PropertyType == parseEntry.Type).ToArray();

				foreach (var propInfo in propInfos)
				{
					if (!string.IsNullOrEmpty(parseEntry.ParentAttributeName) && parseEntry.ParentAttributeName != propInfo.Name)
					{
						continue;
					}

					//Already set then throw
					if (propInfo.GetValue(objectInstanceFromStack.Instance, null) != null)
						throw new InvalidOperationException("Already set");

					propInfo.SetValue(objectInstanceFromStack.Instance, newObjectInstance.Instance, null);

					_objectInstanceStack.Push(newObjectInstance);
					return true;
				}


				if (_parser.TryParseGenericList(parseEntry, newObjectInstance, objectInstanceFromStack))
				{
					if (objectInstanceFromStack != newObjectInstance)
						_objectInstanceStack.Push(newObjectInstance);
					return true;
				}

				if (_objectInstanceStack.Count == 1)
					return false;

				//Just remove it
				_objectInstanceStack.Pop();
			}
			return false;
		}

		public void AddError(string message, int row)
		{
			_parseErrors.Add(new ParseErrors { Message = message, Row = row });
		}
	}

	public class ParseErrors
	{
		public int Row { get; set; }
		public string Message { get; set; }
	}
}
