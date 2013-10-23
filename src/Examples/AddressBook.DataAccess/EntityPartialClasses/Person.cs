
namespace RestCake.AddressBook.DataAccess
{
	public partial class Person
	{
		/// <summary>
		/// This is its own method, because we have to delete a bunch of related data in order to delete a person:
		/// Websites,  Emails, Phones, Address relations (and subsequently orphaned addresses), and entries from Groups.
		/// </summary>
		public void Delete()
		{
			// Rather than fetch all the related data just so we can delete it, we'll just call a SP that does it for us.
			AddressBookDal.Instance.usp_deletePerson(ID);
		}

	}
}
