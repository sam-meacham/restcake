using System.IO;
using System.Linq;
using System.Reflection;
using RestCake.Metadata;

namespace RestCake.Clients
{
	public abstract class ClientWriterBase
	{
		private readonly TextWriter m_textWriter;
		protected TextWriter TextWriter { get { return m_textWriter; } }


		protected ClientWriterBase(TextWriter textWriter)
		{
			m_textWriter = textWriter;
		}

		/// <summary>
		/// Closes the <see cref="TextWriter" />
		/// </summary>
		public virtual void Close()
		{
			m_textWriter.Close();
		}

		/// <summary>
		/// All method params.  Both url and data params.
		/// </summary>
		/// <returns></returns>
		public static string[] GetParamNames(MethodMetadata method)
		{
			return method.Parameters.Select(param => param.Name).ToArray();
		}

		/// <summary>
		/// Just params that are part of the UriTemplate
		/// </summary>
		/// <returns></returns>
		public static string[] GetUrlParamNames(MethodMetadata method)
		{
			return method.Parameters.Select(param => param.Name)
				.Where(param => method.UriTemplate.Contains("{" + param + "}")).ToArray();
		}

		/// <summary>
		/// Just params that are part of the UriTemplate
		/// </summary>
		/// <returns></returns>
		public static ParameterInfo[] GetUrlParams(MethodMetadata method)
		{
			return method.Parameters
				.Where(param => method.UriTemplate.Contains("{" + param.Name + "}")).ToArray();
		}

		/// <summary>
		/// Params that are in the UriTemplate are part of the "MethodUrl".
		/// Other params are sent as data (content). 
		/// </summary>
		/// <returns></returns>
		public static string[] GetDataParamNames(MethodMetadata method)
		{
			return method.Parameters.Select(param => param.Name)
				.Where(param => !method.UriTemplate.Contains("{" + param + "}")).ToArray();
		}


		protected abstract string GetReturnType(MethodMetadata method);



		/// <summary>
		/// Returns a string like "arg1, arg2, arg3", etc (joins all params with a comma)
		/// </summary>
		/// <returns></returns>
		internal abstract string GetArgsListAsString(MethodMetadata method);


		protected abstract string GetMethodUrl(MethodMetadata method);


		/// <summary>
		/// Don't think http headers or anything like that.
		/// This method just writes out files, snippets, etc that are only to be written once, no matter how many
		/// service classes or service methods there are.  This method writes out code that is common among all of them.
		/// </summary>
		public abstract void WriteClientHeaders();

		protected abstract string GetMethodBody(MethodMetadata method);

		public abstract void WriteServiceClient(ServiceMetadata service);
	}
}
