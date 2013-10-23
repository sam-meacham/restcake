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
	public partial class Address 
	{
		// Parameterless constructor, important for serialization
		public Address()
		{}
		
	
		public AddressDto ToDto()
		{
			return AutoMapper.Mapper.Map<Address, AddressDto>(this);
		}
		
		
		public static Address GetByKey(Int32 key)
		{
			return AddressBookDal.Instance.Addresses.Where(obj => obj.ID == key).SingleOrDefault();
		}
		
		
		public static Address GetByKey(Int32 key, params string[] includes)
		{
			ObjectQuery<Address> q =
				(ObjectQuery<Address>)AddressBookDal.Instance.Addresses
				.Where(obj => obj.ID == key);
			
			q = q.DoIncludes(String.Join(",", includes));
			return q.SingleOrDefault();
		}
		
		
		public static Address GetByKey(Int32 key, params Expression<Func<IncludeChain, IncludeChainBase>>[] expressions)
		{
			ObjectQuery<Address> q =
				(ObjectQuery<Address>)AddressBookDal.Instance.Addresses
				.Where(obj => obj.ID == key);

			foreach (Expression<Func<IncludeChain, IncludeChainBase>> exp in expressions)
			{
				IncludeChainBase chain = exp.Compile().Invoke(s_includes);
				q = q.Include(chain.Value);
			}

			return q.SingleOrDefault();
		}


		private static readonly PropertyDescriptorCollection s_properties = TypeDescriptor.GetProperties(typeof (Address));

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
			private static readonly string _Person = "Person";
			public Person.IncludeChain Person
			{
				get { return Chain<Person.IncludeChain>(_Person); }
			}
			
		}
		
		public static readonly IncludeChain s_includes = new IncludeChain();
	}
}	

