

using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq.Expressions;
using Loef;


namespace RestCake.AddressBook.DataAccess
{
	public static class EntityExtensionMethods
	{
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static AddressDto[] ToDtos(this IEnumerable<Address> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<Address>, AddressDto[]>(entities);
    	}
		
		public static ObjectQuery<Address> EagerLoad(this ObjectQuery<Address> query, Expression<Func<Address.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(Address.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static EmailAddressDto[] ToDtos(this IEnumerable<EmailAddress> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<EmailAddress>, EmailAddressDto[]>(entities);
    	}
		
		public static ObjectQuery<EmailAddress> EagerLoad(this ObjectQuery<EmailAddress> query, Expression<Func<EmailAddress.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(EmailAddress.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static EmailTypeDto[] ToDtos(this IEnumerable<EmailType> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<EmailType>, EmailTypeDto[]>(entities);
    	}
		
		public static ObjectQuery<EmailType> EagerLoad(this ObjectQuery<EmailType> query, Expression<Func<EmailType.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(EmailType.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static GroupDto[] ToDtos(this IEnumerable<Group> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<Group>, GroupDto[]>(entities);
    	}
		
		public static ObjectQuery<Group> EagerLoad(this ObjectQuery<Group> query, Expression<Func<Group.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(Group.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static PersonDto[] ToDtos(this IEnumerable<Person> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<Person>, PersonDto[]>(entities);
    	}
		
		public static ObjectQuery<Person> EagerLoad(this ObjectQuery<Person> query, Expression<Func<Person.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(Person.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static PhoneDto[] ToDtos(this IEnumerable<Phone> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<Phone>, PhoneDto[]>(entities);
    	}
		
		public static ObjectQuery<Phone> EagerLoad(this ObjectQuery<Phone> query, Expression<Func<Phone.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(Phone.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static PhoneTypeDto[] ToDtos(this IEnumerable<PhoneType> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<PhoneType>, PhoneTypeDto[]>(entities);
    	}
		
		public static ObjectQuery<PhoneType> EagerLoad(this ObjectQuery<PhoneType> query, Expression<Func<PhoneType.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(PhoneType.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
		/// <summary>
    	/// Easily create DTO objects from actual domain entity objects
    	/// </summary>
    	public static WebsiteDto[] ToDtos(this IEnumerable<Website> entities)
    	{
    		return AutoMapper.Mapper.Map<IEnumerable<Website>, WebsiteDto[]>(entities);
    	}
		
		public static ObjectQuery<Website> EagerLoad(this ObjectQuery<Website> query, Expression<Func<Website.IncludeChain, IncludeChainBase>> expression)
		{
			IncludeChainBase chain = expression.Compile().Invoke(Website.s_includes);
			query = query.Include(chain.Value);
			return query;
		}
		
	}
}

