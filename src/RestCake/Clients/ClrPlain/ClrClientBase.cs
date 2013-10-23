using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace <#= Namespace #>
{
	public class RestResponse<T>
	{
		public HttpWebRequest Request { get; private set; }
		public HttpWebResponse Response { get; private set; }
		public string ResponseText { get; private set; }
		public bool HasError { get; set; }

		public RestResponse(HttpWebRequest request, HttpWebResponse response)
		{
			Request = request;
			Response = response;
			ResponseText = getResponseText(Response);
		}

		private static string getResponseText(HttpWebResponse response)
		{
			if (response == null)
				return null;
			Stream responseStream = response.GetResponseStream();
			if (responseStream == null)
				throw new Exception("The reponse stream is null");
			StreamReader streamReader = new StreamReader(responseStream);
			string responseText = streamReader.ReadToEnd();
			streamReader.Close();
			response.Close();
			return responseText;
		}

		public T GetValue()
		{
			if (HasError)
				throw new Exception("The server returned an error, so no attempt can be made to deserialize the result to the expected return type");
			return JsonConvert.DeserializeObject<T>(ResponseText);
		}
	}


	public abstract class ClrClientBase
	{
		protected string ServiceUrl { get; private set; }
		public Dictionary<string, string> HeadersToAddToAllRequests { get; private set; }

		protected ClrClientBase(string serviceUrl)
		{
			if(String.IsNullOrWhiteSpace(serviceUrl))
				throw new ArgumentException("You must provide a service url", "serviceUrl");

			if (!serviceUrl.EndsWith("/"))
				serviceUrl += "/";

			ServiceUrl = serviceUrl;
			HeadersToAddToAllRequests = new Dictionary<string, string>();
		}

		private string getFullUrl(string relativeUrl)
		{
			// Make sure the service url is used, and not just a relative url
			if (!relativeUrl.ToLower().StartsWith("http"))
			{
				if (relativeUrl.StartsWith("/"))
					relativeUrl = relativeUrl.Substring(1);
				relativeUrl = ServiceUrl + relativeUrl;
			}
			return relativeUrl;
		}

		protected RestResponse<T> Request<T>(string verb, string url, string body, Action<HttpWebRequest> beforeRequest)
		{
			url = getFullUrl(url);
			HttpWebRequest req = (HttpWebRequest) WebRequest.Create(url);
			req.Method = verb;
			req.ContentType = "application/json; charset=utf-8";
			req.UserAgent = ".NET RestCake Client";
			foreach(var pair in HeadersToAddToAllRequests)
				req.Headers.Add(pair.Key, pair.Value);

			if (!String.IsNullOrWhiteSpace(body))
			{
				req.ContentLength = body.Length;
				Stream requestStream = req.GetRequestStream();
				StreamWriter writer = new StreamWriter(requestStream);
				writer.Write(body);
				writer.Close();
				requestStream.Close();
			}
			else
			{
				req.ContentLength = 0;
			}

			if (beforeRequest != null)
				beforeRequest(req);

			RestResponse<T> restResponse;
			try
			{
				HttpWebResponse webResponse = (HttpWebResponse)req.GetResponse();
				restResponse = new RestResponse<T>(req, webResponse);
			}
			catch (WebException ex)
			{
				restResponse = new RestResponse<T>(req, (HttpWebResponse)ex.Response);
				restResponse.HasError = true;
			}

			return restResponse;
		}

		protected static string ConstructRequestBody(bool wrappedRequest, string[] paramNames, object[] paramValues)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(stringBuilder);
			JsonWriter jwriter = new JsonTextWriter(stringWriter);

			if (wrappedRequest)
			{
				jwriter.WriteStartObject();
				for (int i = 0; i < paramNames.Length; i++)
				{
					jwriter.WritePropertyName(paramNames[i]);
					string json = JsonConvert.SerializeObject(paramValues[i]);
					jwriter.WriteRawValue(json);
				}
				jwriter.WriteEndObject();
			}
			else
			{
				// We have just a single param, bare
				string json = JsonConvert.SerializeObject(paramValues[0]);
				jwriter.WriteRawValue(json);
			}

			jwriter.Close();
			stringWriter.Close();
			return stringBuilder.ToString();
		}

	}
}

