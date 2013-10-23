
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RestCake.AddressBook.DataAccess
{

	/// <summary>
	/// TODO: Decide if I want to use FixupCollection{T} classes, with ICollection{T}, or if I just want these to be as plain as possible (manual fixup, etc).
	/// Also, do I want the FK fixup too?  Could be nice...
	/// </summary>
	[DataContract]
	public  partial class GroupDto
	{
		/// <summary>Parameterless constructor (important for serialization)</summary>
		public GroupDto()
		{
		}
		
		public GroupDto StripCycles()
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
			if (People != null)
				for(int i = 0; i < People.Count; ++i)
					if (!People[i].StripCycles(graphObjs, path + ".People[" + i + "]"))
						// Old way: assigned the ref to null
						//People[i] = null;
						// New way: remove the object completely
						// I know, I know, modifying the indexer is probably bad.  But this was generated from a template.
						// Anyway, for anyone unfamiliar with this, we remove at index i, which is the object that creates a cycle, THEN decrement i,
						// since the "next" object in the list just dropped an index number.  If we didn't decrement i, we'd skip the object after the one we're removing.
						People.RemoveAt(i--);
			return true;
		}
		


		// ********************************************************************************
		// *** Primitive properties *******************************************************
		// ********************************************************************************

		[DataMember] public int ID { get; set; }
		[DataMember] public string Name { get; set; }


		// ********************************************************************************
		// *** Complex properties *********************************************************
		// ********************************************************************************
	
		// ********************************************************************************
		// *** Navigation properties ******************************************************
		// ********************************************************************************

		[DataMember] public List<PersonDto> People { get; set; }

	} // end class
} // end namespace

