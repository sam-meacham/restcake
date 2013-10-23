using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RestCake.Util;

namespace RestCake.UnitTests
{
	[TestFixture]
	public class UtilUnitTests
	{
		[Test]
		public void ParseStringArray1()
		{
			string[] r1 = StringUtil.ParseStringArray("a,b,c");
			Assert.That(r1.Length == 3);
			Assert.That(r1[0] == "a");
			Assert.That(r1[1] == "b");
			Assert.That(r1[2] == "c");
		}


		[Test]
		public void ParseStringArray2()
		{
			string[] r1 = StringUtil.ParseStringArray("'foo,bar','b','c'");
			Assert.That(r1.Length == 3);
			Assert.That(r1[0] == "foo,bar");
			Assert.That(r1[1] == "b");
			Assert.That(r1[2] == "c");
		}

		[Test]
		public void ParseStringArray3()
		{
			string[] r1 = StringUtil.ParseStringArray("\"foo,\\\"bar\\\"\",\"b\",\"c\"");
			Assert.That(r1.Length == 3);
			Assert.That(r1[0] == "foo,\\\"bar\\\"");
			Assert.That(r1[1] == "b");
			Assert.That(r1[2] == "c");
		}

		[Test]
		public void ParseStringArray4()
		{
			string[] r1 = StringUtil.ParseStringArray("[a,b,c]");
			Assert.That(r1.Length == 3);
			Assert.That(r1[0] == "a");
			Assert.That(r1[1] == "b");
			Assert.That(r1[2] == "c");
		}

		[Test]
		public void ParseStringArray5()
		{
			string[] r1 = StringUtil.ParseStringArray("['foo,bar','b','c']");
			Assert.That(r1.Length == 3);
			Assert.That(r1[0] == "foo,bar");
			Assert.That(r1[1] == "b");
			Assert.That(r1[2] == "c");
		}

		[Test]
		public void ParseStringArray6()
		{
			string[] r1 = StringUtil.ParseStringArray("[\"foo,\\\"bar\\\"\",\"b\",\"c\"]");
			Assert.That(r1.Length == 3);
			Assert.That(r1[0] == "foo,\\\"bar\\\"");
			Assert.That(r1[1] == "b");
			Assert.That(r1[2] == "c");
		}


	}
}
