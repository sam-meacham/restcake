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
	public partial class Phone 
	{
		// Parameterless constructor, important for serialization
		public Phone()
		{}
		
	
		public PhoneDto ToDto()
		{
			return AutoMapper.Mapper.Map<Phone, PhoneDto>(this);
		}
		
		
		public static Phone GetByKey(Int32 key)
		{
			return AddressBookDal.Instance.Phones.Where(obj => obj.ID == key).SingleOrDefault();
		}
		
		
		public static Phone GetByKey(Int32 key, params string[] includes)
		{
			ObjectQuery<Phone> q =
				(ObjectQuery<Phone>)AddressBookDal.Instance.Phones
				.Where(obj => obj.ID == key);
			
			q = q.DoIncludes(String.Join(",", includes));
			return q.SingleOrDefault();
		}
		
		
		public static Phone GetByKey(Int32 key, params Expression<Func<IncludeChain, IncludeChainBase>>[] expressions)
		{
			ObjectQuery<Phone> q =
				(ObjectQuery<Phone>)AddressBookDal.Instance.Phones
				.Where(obj => obj.ID == key);

			foreach (Expression<Func<IncludeChain, IncludeChainBase>> exp in expressions)
			{
				IncludeChainBase chain = exp.Compile().Invoke(s_includes);
				q = q.Include(chain.Value);
			}

			return q.SingleOrDefault();
		}


		private static readonly PropertyDescriptorCollection s_properties = TypeDescriptor.GetProperties(typeof (Phone));

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
			
			private static readonly string _PhoneType = "PhoneType";
			public PhoneType.IncludeChain PhoneType
			{
				get { return Chain<PhoneType.IncludeChain>(_PhoneType); }
			}
			
		}
		
		public static readonly IncludeChain s_includes = new IncludeChain();
	}
}	

