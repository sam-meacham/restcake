// 1/11/2011 - Now detects when you're in a string, and doesn't format json symbols
function formatJson(strJson, indentString)
{
	// Default to 4 spaces for the indent char
	if (typeof (indentString) !== "string")
		indentString = "    ";

	var nextLine_curly = false;
	var nextLine_square = false;

	var indent = 0;

	var inString = false;

	// newline function
	var nl = function (indentAdjust)
	{
		if (typeof (indentAdjust) === "number")
			indent += indentAdjust;

		var s = "\n";
		for (var i = 0; i < indent; ++i)
			s += indentString;
		return s;
	}

	var formatted = "";
	$.each(strJson, function (ix, chr)
	{
		var after = "";

		if (inString)
		{
			formatted += chr;

			// See if we're out of the string yet
			if (strJson[ix] == '"' && strJson[ix - 1] != "\\")
			// Wasn't an escaped quote, so it must be a string delineating one.
				inString = false;
		}
		else
		{
			switch (chr)
			{
				case "{":
					// handle empty objects
					if (strJson[ix + 1] == "}")
					{
						formatted += chr;
					}
					else
					{
						formatted += (nextLine_curly ? nl() : "") + chr;
						formatted += nl(1);
					}
					break;

				case "}":
					// handle empty objects
					if (strJson[ix - 1] == "{")
						formatted += chr;
					else
						formatted += nl(-1) + chr;
					break;

				case "[":
					// handle empty arrays
					if (strJson[ix + 1] == "]")
					{
						formatted += chr;
					}
					else
					{
						formatted += (nextLine_square ? nl() : "") + chr + nl(1);
					}
					break;

				case "]":
					// handle empty arrays
					if (strJson[ix - 1] == "[")
						formatted += chr;
					else
						formatted += nl(-1) + chr;
					break;

				case ",":
					formatted += chr + nl();
					break;

				case ":":
					formatted += chr;
					if (strJson[ix + 1] != " ")
						formatted += " ";
					break;

				// quote       
				case '"':
					if (ix != 0)
					{
						// see if the previous char is a \, effectively escaping the " in the string (and not delineating the start or end of one)
						if (strJson[ix - 1] != "\\")
						// Wasn't an escaped quote, so it must be a string delineating one.
							inString = !inString;
					}

					formatted += chr;
					break;

				default:
					formatted += chr;
			} // switch
		} // else
	});
	return formatted;
}
