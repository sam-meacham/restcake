
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RestCake.AddressBook.DataAccess
{

	/// <summary>
	/// TODO: Decide if I want to use FixupCollection{T} classes, with ICollection{T}, or if I just want these to be as plain as possible (manual fixup, etc).
	/// Also, do I want the FK fixup too?  Could be nice...
	/// </summary>
	[DataContract]
	public  partial class PersonDto
	{
		/// <summary>Parameterless constructor (important for serialization)</summary>
		public PersonDto()
		{
		}
		
		public PersonDto StripCycles()
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

			// Collections
			if (Emails != null)
				for(int i = 0; i < Emails.Count; ++i)
					if (!Emails[i].StripCycles(graphObjs, path + ".Emails[" + i + "]"))
						// Old way: assigned the ref to null
						//Emails[i] = null;
						// New way: remove the object completely
						// I know, I know, modifying the indexer is probably bad.  But this was generated from a template.
						// Anyway, for anyone unfamiliar with this, we remove at index i, which is the object that creates a cycle, THEN decrement i,
						// since the "next" object in the list just dropped an index number.  If we didn't decrement i, we'd skip the object after the one we're removing.
						Emails.RemoveAt(i--);
			// Collections
			if (Phones != null)
				for(int i = 0; i < Phones.Count; ++i)
					if (!Phones[i].StripCycles(graphObjs, path + ".Phones[" + i + "]"))
						// Old way: assigned the ref to null
						//Phones[i] = null;
						// New way: remove the object completely
						// I know, I know, modifying the indexer is probably bad.  But this was generated from a template.
						// Anyway, for anyone unfamiliar with this, we remove at index i, which is the object that creates a cycle, THEN decrement i,
						// since the "next" object in the list just dropped an index number.  If we didn't decrement i, we'd skip the object after the one we're removing.
						Phones.RemoveAt(i--);
			// Collections
			if (Websites != null)
				for(int i = 0; i < Websites.Count; ++i)
					if (!Websites[i].StripCycles(graphObjs, path + ".Websites[" + i + "]"))
						// Old way: assigned the ref to null
						//Websites[i] = null;
						// New way: remove the object completely
						// I know, I know, modifying the indexer is probably bad.  But this was generated from a template.
						// Anyway, for anyone unfamiliar with this, we remove at index i, which is the object that creates a cycle, THEN decrement i,
						// since the "next" object in the list just dropped an index number.  If we didn't decrement i, we'd skip the object after the one we're removing.
						Websites.RemoveAt(i--);
			// Collections
			if (Groups != null)
				for(int i = 0; i < Groups.Count; ++i)
					if (!Groups[i].StripCycles(graphObjs, path + ".Groups[" + i + "]"))
						// Old way: assigned the ref to null
						//Groups[i] = null;
						// New way: remove the object completely
						// I know, I know, modifying the indexer is probably bad.  But this was generated from a template.
						// Anyway, for anyone unfamiliar with this, we remove at index i, which is the object that creates a cycle, THEN decrement i,
						// since the "next" object in the list just dropped an index number.  If we didn't decrement i, we'd skip the object after the one we're removing.
						Groups.RemoveAt(i--);
			// Collections
			if (Addresses != null)
				for(int i = 0; i < Addresses.Count; ++i)
					if (!Addresses[i].StripCycles(graphObjs, path + ".Addresses[" + i + "]"))
						// Old way: assigned the ref to null
						//Addresses[i] = null;
						// New way: remove the object completely
						// I know, I know, modifying the indexer is probably bad.  But this was generated from a template.
						// Anyway, for anyone unfamiliar with this, we remove at index i, which is the object that creates a cycle, THEN decrement i,
						// since the "next" object in the list just dropped an index number.  If we didn't decrement i, we'd skip the object after the one we're removing.
						Addresses.RemoveAt(i--);
			return true;
		}
		


		// ********************************************************************************
		// *** Primitive properties *******************************************************
		// ********************************************************************************

		[DataMember] public int ID { get; set; }
		[DataMember] public string Fname { get; set; }
		[DataMember] public string Lname { get; set; }
		[DataMember] public string Title { get; set; }
		[DataMember] public string Company { get; set; }
		[DataMember] public Nullable<System.DateTime> Birthday { get; set; }
		[DataMember] public string Notes { get; set; }
		[DataMember] public System.DateTime DateCreated { get; set; }
		[DataMember] public System.DateTime DateModified { get; set; }


		// ********************************************************************************
		// *** Complex properties *********************************************************
		// ********************************************************************************
	
		// ********************************************************************************
		// *** Navigation properties ******************************************************
		// ********************************************************************************

		[DataMember] public List<EmailAddressDto> Emails { get; set; }

		[DataMember] public List<PhoneDto> Phones { get; set; }

		[DataMember] public List<WebsiteDto> Websites { get; set; }

		[DataMember] public List<GroupDto> Groups { get; set; }

		[DataMember] public List<AddressDto> Addresses { get; set; }

	} // end class
} // end namespace

