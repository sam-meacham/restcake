
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RestCake.AddressBook.DataAccess
{

	/// <summary>
	/// TODO: Decide if I want to use FixupCollection{T} classes, with ICollection{T}, or if I just want these to be as plain as possible (manual fixup, etc).
	/// Also, do I want the FK fixup too?  Could be nice...
	/// </summary>
	[DataContract]
	public  partial class AddressDto
	{
		/// <summary>Parameterless constructor (important for serialization)</summary>
		public AddressDto()
		{
		}
		
		public AddressDto StripCycles()
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
		[DataMember] public string Address1 { get; set; }
		[DataMember] public string Address2 { get; set; }
		[DataMember] public string City { get; set; }
		[DataMember] public string State { get; set; }
		[DataMember] public string Zip { get; set; }
		[DataMember] public System.DateTime DateCreated { get; set; }
		[DataMember] public System.DateTime DateModified { get; set; }


		// ********************************************************************************
		// *** Complex properties *********************************************************
		// ********************************************************************************
	
		// ********************************************************************************
		// *** Navigation properties ******************************************************
		// ********************************************************************************

		[DataMember] public PersonDto Person { get; set; }

	} // end class
} // end namespace

