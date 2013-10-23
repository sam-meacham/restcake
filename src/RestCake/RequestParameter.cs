using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using RestCake.Util;

namespace RestCake
{
	public class RequestParameter
	{
		public RequestParameterSource Source { get; set; }
		public string Name { get; set; }
		public string StringValue { get; set; }
		public ParameterInfo ParameterInfo { get; set; }
		public object Value { get; set; }

		/// <summary>
		/// This will populate the Value property from the StringValue property.
		/// TODO: There are edge cases and special handling for certain types. There is still a problem when those types are used in crazy generics compositions.
		/// Example: There is special handling for enum types, so what about a Dictionary{string, List{SomeEnum}}? Not sure how to deal with that...for now, we'll keep
		/// the input parameter types simple...(and call it a known bug I guess)
		/// </summary>
		public void CalcValue(JsonSerializer serializer)
		{
			Type type = ParameterInfo.ParameterType;
			Type[] genArgs = type.GetGenericArguments();

			// If the value is null, use the default value for the target type (null for ref types, a new instance of value types, which will have the default value)
			if (String.IsNullOrEmpty(StringValue))
			{
				Value = type.IsValueType ? Activator.CreateInstance(type) : null;
			}
			// special handling for string, string[] and IList<string>
			else if (type == typeof(string))
			{
				if (StringValue == "null")
					Value = null;
				else if (Source == RequestParameterSource.RequestBodyJson)
					// Value is wrapped in " or ' chars
					//Value = StringValue.Substring(1, StringValue.Length - 2);
					Value = serializer.Deserialize(new StringReader(StringValue), typeof (string));
				else
					// Not wrapped
					Value = StringValue;
			}
			else if (type == typeof(string[]))
			{
				string[] arStrings = StringUtil.ParseStringArray(StringValue);
				Value = arStrings;
			}
			else if (typeof(IList<string>).IsAssignableFrom(type))
			{
				string[] arStrings = StringUtil.ParseStringArray(StringValue);
				Value = new List<string>(arStrings);
			}
			// special handling for bool, bool[] and IList<bool>
			// (Json.NET can't deserialize unless the bools are all lowercase, so "True" fails, but "true" suceeds).
			else if (type == typeof(bool) || type == typeof(bool[]) || typeof(IList<bool>).IsAssignableFrom(type))
			{
				string sval = StringValue;
				if (ReflectionHelper.NeedsBrackets(type) && !sval.StartsWith("["))
					sval = "[" + sval + "]";
				sval = sval.ToLower(); // force Json.NET to deserialize bool types successfully
				Value = serializer.Deserialize(new StringReader(sval), type);
			}
			// enums
			else if (type.IsEnum)
			{
				Value = StringUtil.GetEnum(StringValue, type);
			}
			// enum array
			else if (type.IsArray && type.GetElementType().IsEnum)
			{
				string[] strings = StringUtil.ParseStringArray(StringValue);
				object[] objArray = strings.Select(str => StringUtil.GetEnum(str, type.GetElementType())).ToArray();
				Array arr = (Array)Activator.CreateInstance(type, strings.Length);
				for (int i = 0; i < strings.Length; i++)
					arr.SetValue(objArray[i], i);
				Value = arr;
			}
			// IList<Enum>
			else if (genArgs.Length == 1 && genArgs[0].IsEnum && typeof(IList<>).MakeGenericType(genArgs).IsAssignableFrom(type))
			{
				string[] strings = StringUtil.ParseStringArray(StringValue);
				object[] objArray = strings.Select(str => StringUtil.GetEnum(str, genArgs[0])).ToArray();
				Type listType = typeof(List<>).MakeGenericType(genArgs[0]);
				IList list = (IList)Activator.CreateInstance(listType);
				foreach(object obj in objArray)
					list.Add(obj);
				Value = list;
			}
			// Everything else! (goes through the normal json.net deserialization)
			else
			{
				string sval = StringValue;
				if (ReflectionHelper.NeedsBrackets(type) && !sval.StartsWith("["))
					sval = "[" + sval + "]";
				Value = serializer.Deserialize(new StringReader(sval), type);
			}
		}
	}

}
