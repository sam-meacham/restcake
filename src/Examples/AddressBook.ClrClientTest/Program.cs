using System;
using RestCakeExamples;


namespace AddressBook.ClrClientTest
{
	class Program
	{
		static void Main(string[] args)
		{
			MathServiceClient client = new MathServiceClient("http://localhost.restcake.net/AddressBook.Services/math/");

			double result = client.divide(100, 3);
			Console.WriteLine("Divide result: " + result);

			
			try
			{
				// This will throw an exception
				result = client.divide(10, 0);
				Console.WriteLine("Divide result: " + result);
			}
			catch (Exception ex)
			{
				Console.WriteLine(client.LastResponse.StatusCode);
				Console.WriteLine(client.LastResponse.Content);
			}
			
		}
	}
}
