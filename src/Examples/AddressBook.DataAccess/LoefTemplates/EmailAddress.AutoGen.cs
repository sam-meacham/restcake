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
	public partial class EmailAddress 
	{
		// Parameterless constructor, important for serialization
		public EmailAddress()
		{}
		
	
		public EmailAddressDto ToDto()
		{
			return AutoMapper.Mapper.Map<EmailAddress, EmailAddressDto>(this);
		}
		
		
		public static EmailAddress GetByKey(Int32 key)
		{
			return AddressBookDal.Instance.EmailAddresses.Where(obj => obj.ID == key).SingleOrDefault();
		}
		
		
		public static EmailAddress GetByKey(Int32 key, params string[] includes)
		{
			ObjectQuery<EmailAddress> q =
				(ObjectQuery<EmailAddress>)AddressBookDal.Instance.EmailAddresses
				.Where(obj => obj.ID == key);
			
			q = q.DoIncludes(String.Join(",", includes));
			return q.SingleOrDefault();
		}
		
		
		public static EmailAddress GetByKey(Int32 key, params Expression<Func<IncludeChain, IncludeChainBase>>[] expressions)
		{
			ObjectQuery<EmailAddress> q =
				(ObjectQuery<EmailAddress>)AddressBookDal.Instance.EmailAddresses
				.Where(obj => obj.ID == key);

			foreach (Expression<Func<IncludeChain, IncludeChainBase>> exp in expressions)
			{
				IncludeChainBase chain = exp.Compile().Invoke(s_includes);
				q = q.Include(chain.Value);
			}

			return q.SingleOrDefault();
		}


		private static readonly PropertyDescriptorCollection s_properties = TypeDescriptor.GetProperties(typeof (EmailAddress));

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
			private static readonly string _EmailType = "EmailType";
			public EmailType.IncludeChain EmailType
			{
				get { return Chain<EmailType.IncludeChain>(_EmailType); }
			}
			
			private static readonly string _Person = "Person";
			public Person.IncludeChain Person
			{
				get { return Chain<Person.IncludeChain>(_Person); }
			}
			
		}
		
		public static readonly IncludeChain s_includes = new IncludeChain();
	}
}	

