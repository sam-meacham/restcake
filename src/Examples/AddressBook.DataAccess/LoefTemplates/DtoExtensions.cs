

using System.Collections.Generic;

namespace RestCake.AddressBook.DataAccess
{

	public static class DtoExtensionMethods
	{
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static Address[] ToEntities(this IEnumerable<AddressDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<AddressDto>, Address[]>(dtos);
		}
		
		public static IEnumerable<AddressDto> StripCycles(this IEnumerable<AddressDto> dtos)
		{
			foreach(AddressDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static Address ToEntity(this AddressDto dto)
		{
			return AutoMapper.Mapper.Map<AddressDto, Address>(dto);
		}
		
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static EmailAddress[] ToEntities(this IEnumerable<EmailAddressDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<EmailAddressDto>, EmailAddress[]>(dtos);
		}
		
		public static IEnumerable<EmailAddressDto> StripCycles(this IEnumerable<EmailAddressDto> dtos)
		{
			foreach(EmailAddressDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static EmailAddress ToEntity(this EmailAddressDto dto)
		{
			return AutoMapper.Mapper.Map<EmailAddressDto, EmailAddress>(dto);
		}
		
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static EmailType[] ToEntities(this IEnumerable<EmailTypeDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<EmailTypeDto>, EmailType[]>(dtos);
		}
		
		public static IEnumerable<EmailTypeDto> StripCycles(this IEnumerable<EmailTypeDto> dtos)
		{
			foreach(EmailTypeDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static EmailType ToEntity(this EmailTypeDto dto)
		{
			return AutoMapper.Mapper.Map<EmailTypeDto, EmailType>(dto);
		}
		
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static Group[] ToEntities(this IEnumerable<GroupDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<GroupDto>, Group[]>(dtos);
		}
		
		public static IEnumerable<GroupDto> StripCycles(this IEnumerable<GroupDto> dtos)
		{
			foreach(GroupDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static Group ToEntity(this GroupDto dto)
		{
			return AutoMapper.Mapper.Map<GroupDto, Group>(dto);
		}
		
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static Person[] ToEntities(this IEnumerable<PersonDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<PersonDto>, Person[]>(dtos);
		}
		
		public static IEnumerable<PersonDto> StripCycles(this IEnumerable<PersonDto> dtos)
		{
			foreach(PersonDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static Person ToEntity(this PersonDto dto)
		{
			return AutoMapper.Mapper.Map<PersonDto, Person>(dto);
		}
		
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static Phone[] ToEntities(this IEnumerable<PhoneDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<PhoneDto>, Phone[]>(dtos);
		}
		
		public static IEnumerable<PhoneDto> StripCycles(this IEnumerable<PhoneDto> dtos)
		{
			foreach(PhoneDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static Phone ToEntity(this PhoneDto dto)
		{
			return AutoMapper.Mapper.Map<PhoneDto, Phone>(dto);
		}
		
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static PhoneType[] ToEntities(this IEnumerable<PhoneTypeDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<PhoneTypeDto>, PhoneType[]>(dtos);
		}
		
		public static IEnumerable<PhoneTypeDto> StripCycles(this IEnumerable<PhoneTypeDto> dtos)
		{
			foreach(PhoneTypeDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static PhoneType ToEntity(this PhoneTypeDto dto)
		{
			return AutoMapper.Mapper.Map<PhoneTypeDto, PhoneType>(dto);
		}
		
		/// <summary>
		/// Easily create DTO objects from actual domain entity objects
		/// </summary>
		public static Website[] ToEntities(this IEnumerable<WebsiteDto> dtos)
		{
			return AutoMapper.Mapper.Map<IEnumerable<WebsiteDto>, Website[]>(dtos);
		}
		
		public static IEnumerable<WebsiteDto> StripCycles(this IEnumerable<WebsiteDto> dtos)
		{
			foreach(WebsiteDto dto in dtos)
				dto.StripCycles();
			return dtos;
		}


		public static Website ToEntity(this WebsiteDto dto)
		{
			return AutoMapper.Mapper.Map<WebsiteDto, Website>(dto);
		}
		
	}
}

