using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using Hyper.ComponentModel;
using Loef;

namespace RestCake.AddressBook.DataAccess
{
	[TypeDescriptionProvider(typeof(HyperTypeDescriptionProvider))]
	public partial class Person 
	{
		// Parameterless constructor, important for serialization
		public Person()
		{}
		
	
		public PersonDto ToDto()
		{
			return AutoMapper.Mapper.Map<Person, PersonDto>(this);
		}
		
		
		public static Person GetByKey(Int32 key)
		{
			return AddressBookDal.Instance.People.Where(obj => obj.ID == key).SingleOrDefault();
		}
		
		
		public static Person GetByKey(Int32 key, params string[] includes)
		{
			ObjectQuery<Person> q =
				(ObjectQuery<Person>)AddressBookDal.Instance.People
				.Where(obj => obj.ID == key);
			
			q = q.DoIncludes(String.Join(",", includes));
			return q.SingleOrDefault();
		}
		
		
		public static Person GetByKey(Int32 key, params Expression<Func<IncludeChain, IncludeChainBase>>[] expressions)
		{
			ObjectQuery<Person> q =
				(ObjectQuery<Person>)AddressBookDal.Instance.People
				.Where(obj => obj.ID == key);

			foreach (Expression<Func<IncludeChain, IncludeChainBase>> exp in expressions)
			{
				IncludeChainBase chain = exp.Compile().Invoke(s_includes);
				q = q.Include(chain.Value);
			}

			return q.SingleOrDefault();
		}


		private static readonly PropertyDescriptorCollection s_properties = TypeDescriptor.GetProperties(typeof (Person));

		public void ApplyValues(IEnumerable<KeyValuePair<string, object>> values, bool throwOnBadProp = false)
		{
			foreach(KeyValuePair<string, object> pair in values)
			{
				try
				{
					PropertyDescriptor prop = s_properties[pair.Key];
					prop.SetValue(this, pair.Value);
				}
				catch (Exception)
				{
					if (throwOnBadProp)
						throw;
				}
			}
		}
		
		
		// ********************************************************************************
		// *** Include chain helpers ******************************************************
		// ********************************************************************************
		
		public class IncludeChain : IncludeChainBase
		{
			private static readonly string _Emails = "Emails";
			public EmailAddress.IncludeChain Emails
			{
				get { return Chain<EmailAddress.IncludeChain>(_Emails); }
			}
			
			private static readonly string _Phones = "Phones";
			public Phone.IncludeChain Phones
			{
				get { return Chain<Phone.IncludeChain>(_Phones); }
			}
			
			private static readonly string _Websites = "Websites";
			public Website.IncludeChain Websites
			{
				get { return Chain<Website.IncludeChain>(_Websites); }
			}
			
			private static readonly string _Groups = "Groups";
			public Group.IncludeChain Groups
			{
				get { return Chain<Group.IncludeChain>(_Groups); }
			}
			
			private static readonly string _Addresses = "Addresses";
			public Address.IncludeChain Addresses
			{
				get { return Chain<Address.IncludeChain>(_Addresses); }
			}
			
		}
		
		public static readonly IncludeChain s_includes = new IncludeChain();
	}
}	

