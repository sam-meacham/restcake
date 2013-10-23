
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RestCake.AddressBook.DataAccess
{

	/// <summary>
	/// TODO: Decide if I want to use FixupCollection{T} classes, with ICollection{T}, or if I just want these to be as plain as possible (manual fixup, etc).
	/// Also, do I want the FK fixup too?  Could be nice...
	/// </summary>
	[DataContract]
	public  partial class EmailAddressDto
	{
		/// <summary>Parameterless constructor (important for serialization)</summary>
		public EmailAddressDto()
		{
		}
		
		public EmailAddressDto StripCycles()
		{
			Dictionary<object, string> graphObjs = new Dictionary<object, string>();
			StripCycles(graphObjs, "");
			return this;
		}



		internal bool StripCycles(Dictionary<object, string> graphObjs, string path)
		{
			if (graphObjs.ContainsKey(this))
				return false;
			graphObjs.Add(this, path);

			// Single navigation property references
			if (EmailType != null)
				if (!EmailType.StripCycles(graphObjs, path + ".EmailType"))
					EmailType = null;
				

			// Single navigation property references
			if (Person != null)
				if (!Person.StripCycles(graphObjs, path + ".Person"))
					Person = null;
				

			return true;
		}
		


		// ********************************************************************************
		// *** Primitive properties *******************************************************
		// ********************************************************************************

		[DataMember] public int ID { get; set; }
		[DataMember] public int PersonID { get; set; }
		[DataMember] public string Email { get; set; }
		[DataMember] public int EmailTypeID { get; set; }


		// ********************************************************************************
		// *** Complex properties *********************************************************
		// ********************************************************************************
	
		// ********************************************************************************
		// *** Navigation properties ******************************************************
		// ********************************************************************************

		[DataMember] public EmailTypeDto EmailType { get; set; }

		[DataMember] public PersonDto Person { get; set; }

	} // end class
} // end namespace

