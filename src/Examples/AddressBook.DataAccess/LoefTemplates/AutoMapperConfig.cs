


namespace RestCake.AddressBook.DataAccess
{
	/// <summary>
	/// Currently only supports Navigation properties, not complex properties.
	/// The .ForMember() calls prevent triggering lazy loading EF navigation properties.  If an EntityReference or
	/// EntityCollection has not been loaded, it will be ignored.
	/// </summary>
	public static class AutoMapperConfig
	{
		private static bool s_isInit = false;
		
		public static void CreateMappings()
		{
			if (s_isInit)
				return;
			s_isInit = true;

			// AutoMapper config for Address => AddressDto
			AutoMapper.Mapper.CreateMap<Address, AddressDto>()
				.ForMember(dto => dto.Person,
					options => options.MapFrom(obj => obj.PersonReference.IsLoaded ? obj.Person : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<AddressDto, Address>();

			// AutoMapper config for EmailAddress => EmailAddressDto
			AutoMapper.Mapper.CreateMap<EmailAddress, EmailAddressDto>()
				.ForMember(dto => dto.EmailType,
					options => options.MapFrom(obj => obj.EmailTypeReference.IsLoaded ? obj.EmailType : null))
				.ForMember(dto => dto.Person,
					options => options.MapFrom(obj => obj.PersonReference.IsLoaded ? obj.Person : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<EmailAddressDto, EmailAddress>();

			// AutoMapper config for EmailType => EmailTypeDto
			AutoMapper.Mapper.CreateMap<EmailType, EmailTypeDto>()
				.ForMember(dto => dto.Emails,
					options => options.MapFrom(obj => obj.Emails.IsLoaded ? obj.Emails : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<EmailTypeDto, EmailType>();

			// AutoMapper config for Group => GroupDto
			AutoMapper.Mapper.CreateMap<Group, GroupDto>()
				.ForMember(dto => dto.People,
					options => options.MapFrom(obj => obj.People.IsLoaded ? obj.People : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<GroupDto, Group>();

			// AutoMapper config for Person => PersonDto
			AutoMapper.Mapper.CreateMap<Person, PersonDto>()
				.ForMember(dto => dto.Emails,
					options => options.MapFrom(obj => obj.Emails.IsLoaded ? obj.Emails : null))
				.ForMember(dto => dto.Phones,
					options => options.MapFrom(obj => obj.Phones.IsLoaded ? obj.Phones : null))
				.ForMember(dto => dto.Websites,
					options => options.MapFrom(obj => obj.Websites.IsLoaded ? obj.Websites : null))
				.ForMember(dto => dto.Groups,
					options => options.MapFrom(obj => obj.Groups.IsLoaded ? obj.Groups : null))
				.ForMember(dto => dto.Addresses,
					options => options.MapFrom(obj => obj.Addresses.IsLoaded ? obj.Addresses : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<PersonDto, Person>();

			// AutoMapper config for Phone => PhoneDto
			AutoMapper.Mapper.CreateMap<Phone, PhoneDto>()
				.ForMember(dto => dto.Person,
					options => options.MapFrom(obj => obj.PersonReference.IsLoaded ? obj.Person : null))
				.ForMember(dto => dto.PhoneType,
					options => options.MapFrom(obj => obj.PhoneTypeReference.IsLoaded ? obj.PhoneType : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<PhoneDto, Phone>();

			// AutoMapper config for PhoneType => PhoneTypeDto
			AutoMapper.Mapper.CreateMap<PhoneType, PhoneTypeDto>()
				.ForMember(dto => dto.Phones,
					options => options.MapFrom(obj => obj.Phones.IsLoaded ? obj.Phones : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<PhoneTypeDto, PhoneType>();

			// AutoMapper config for Website => WebsiteDto
			AutoMapper.Mapper.CreateMap<Website, WebsiteDto>()
				.ForMember(dto => dto.Person,
					options => options.MapFrom(obj => obj.PersonReference.IsLoaded ? obj.Person : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<WebsiteDto, Website>();

		}
	}
}

