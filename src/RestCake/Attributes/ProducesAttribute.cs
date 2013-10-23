using System;

namespace RestCake
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ProducesAttribute : Attribute
	{
		public string ContentType { get; set; }

		public ProducesAttribute(string contentType)
		{
			ContentType = contentType;
		}
	}
}
