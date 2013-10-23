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
	public partial class PhoneType 
	{
		// Parameterless constructor, important for serialization
		public PhoneType()
		{}
		
	
		public PhoneTypeDto ToDto()
		{
			return AutoMapper.Mapper.Map<PhoneType, PhoneTypeDto>(this);
		}
		
		
		public static PhoneType GetByKey(Int32 key)
		{
			return AddressBookDal.Instance.PhoneTypes.Where(obj => obj.ID == key).SingleOrDefault();
		}
		
		
		public static PhoneType GetByKey(Int32 key, params string[] includes)
		{
			ObjectQuery<PhoneType> q =
				(ObjectQuery<PhoneType>)AddressBookDal.Instance.PhoneTypes
				.Where(obj => obj.ID == key);
			
			q = q.DoIncludes(String.Join(",", includes));
			return q.SingleOrDefault();
		}
		
		
		public static PhoneType GetByKey(Int32 key, params Expression<Func<IncludeChain, IncludeChainBase>>[] expressions)
		{
			ObjectQuery<PhoneType> q =
				(ObjectQuery<PhoneType>)AddressBookDal.Instance.PhoneTypes
				.Where(obj => obj.ID == key);

			foreach (Expression<Func<IncludeChain, IncludeChainBase>> exp in expressions)
			{
				IncludeChainBase chain = exp.Compile().Invoke(s_includes);
				q = q.Include(chain.Value);
			}

			return q.SingleOrDefault();
		}


		private static readonly PropertyDescriptorCollection s_properties = TypeDescriptor.GetProperties(typeof (PhoneType));

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
			private static readonly string _Phones = "Phones";
			public Phone.IncludeChain Phones
			{
				get { return Chain<Phone.IncludeChain>(_Phones); }
			}
			
		}
		
		public static readonly IncludeChain s_includes = new IncludeChain();
	}
}	

