using System;
using System.Collections.Generic;
using System.Text;

namespace RestCake.Util
{
	public static class StringUtil
	{
		/// <summary>
		/// Can parse a string representing a string[] into an actual string[].
		/// You can delimit strings with nothing (though commas can't be in the string values), ' or ".
		/// Surrounding [] chars are optional.
		/// Examples:
		///		a,b,c
		///		'a','b','c'
		///		"a","b","c"
		///		[a,b,c]
		///		['a','b','c']
		///		["a","b","c"]
		/// A space after a comma is ok, but if you aren't using a string delimiter, a space will be added to the next string's beginning (" b").
		/// Escaped characters also work, such as ["foo, \"bar\"", "\"quoted string\"", "'single quoted with \" delimiter'"]
		/// You can't mix delimiters.  Use all nothing, all ', or all ".  The first delimiter found will be used (at char index 0 or 1, depending on the wrapping []s presence)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string[] ParseStringArray(string input)
		{
			if (String.IsNullOrWhiteSpace(input))
				return null;
			
			input = input.Trim();

			if (input.ToLower() == "null")
				return null;
			if (input == "[]")
				return new string[0];
			
			bool brackets = input[0] == '[' && input[input.Length - 1] == ']';
			
			// String delimeter can be ' or ", or nothing.
			// Note that without a string delimeter, you can't have commas in your string, cause that's what we'll split on.
			char delim = 'X';
			// Depending on if we're brackets in []s or not, we'll look for the string delimiter at index 0 or 1
			int delimIndex = brackets ? 1 : 0;
			
			if (input[delimIndex] == '\'')
				delim = '\'';
			else if (input[delimIndex] == '"')
				delim = '"';

			string[] result;
			if (delim == 'X')
			{
				// easiest case, split on ,
				if (brackets)
					// Get rid of the wrapping []'s
					result =  input.Substring(1, input.Length - 2).Split(',');
				else
					result =  input.Split(',');
			}
			else
			{
				// From here on, we KNOW we have a string delimiter of ' or "
				bool inString = false;
				bool escapeNext = false;
				StringBuilder sb = new StringBuilder();
				List<string> strings = new List<string>();
				
				for(int i = 0; i < input.Length; ++i)
				{
					// Skip any possible wrapping [] chars
					if (brackets && (i == 0 || i == input.Length - 1))
						continue;

					char c = input[i];

					if (escapeNext)
					{
						escapeNext = false;
						sb.Append(c);
					}
					else if (c == '\\' && inString)
					{
						escapeNext = true;
						sb.Append(c);
					}
					else if (c == delim)
					{
						inString = !inString;
					}
					else if (c == ',')
					{
						if (inString)
						{
							// This is a comma in the string
							sb.Append(c);
						}
						else
						{
							// This comma separates one string from another
							strings.Add(sb.ToString());
							sb.Clear();
						}
					}
					else if (inString)
					{
						sb.Append(c);
					}
				}
				// The last string wasn't added, because it gets added when a comma is encountered, and there's no last comma
				strings.Add(sb.ToString());
				result = strings.ToArray();
			}
			return result;
		}

		public static object GetEnum(string strValue, Type enumType)
		{
			object returnValue;
			// enums can be passed in as ints or strings. As ints, they won't be quoted no matter what (even if they are part of a json request body),
			// since number types aren't quoted in json.
			int i;
			if (int.TryParse(strValue, out i))
			{
				returnValue = Enum.ToObject(enumType, i);
			}
			else
			{
				// Wrapped in " or ' chars?
				//if (Source == RequestParameterSource.RequestBodyJson)
				if (strValue.StartsWith("\"") || strValue.StartsWith("'"))
				{
					// returnValue is wrapped in " or ' chars
					strValue = strValue.Substring(1, strValue.Length - 2);
					returnValue = Enum.Parse(enumType, strValue, true);
				}
				else
				{
					// Not wrapped.
					returnValue = Enum.Parse(enumType, strValue, true);
				}
			}
			return returnValue;
		}

	}
}
