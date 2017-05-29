using System;
using System.IO;
using AutoMapper;
using SolidCP.Providers.HostedSolution;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Extensions;
using SolidCP.WebDavPortal.Constants;
using SolidCP.WebDavPortal.FileOperations;
using SolidCP.WebDavPortal.Models.Account;
using SolidCP.WebDavPortal.Models.FileSystem;

namespace SolidCP.WebDavPortal.Mapping.Profiles.Account
{
    public class UserProfileProfile : Profile
    {
        /// <summary>
        ///     Gets the name of the profile.
        /// </summary>
        /// <value>
        ///     The name of the profile.
        /// </value>
        public override string ProfileName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        ///     Override this method in a derived class and call the CreateMap method to associate that map with this profile.
        ///     Avoid calling the <see cref="T:AutoMapper.Mapper" /> class from this method.
        /// </summary>
        protected override void Configure()
        {
            Mapper.CreateMap<OrganizationUser, UserProfile>()
                .ForMember(ti => ti.PrimaryEmailAddress, x => x.MapFrom(hi => hi.PrimaryEmailAddress))
                .ForMember(ti => ti.DisplayName, x => x.MapFrom(hi => hi.DisplayName))
                .ForMember(ti => ti.DisplayName, x => x.MapFrom(hi => hi.DisplayName))
                .ForMember(ti => ti.AccountName, x => x.MapFrom(hi => hi.AccountName))
                .ForMember(ti => ti.FirstName, x => x.MapFrom(hi => hi.FirstName))
                .ForMember(ti => ti.Initials, x => x.MapFrom(hi => hi.Initials))
                .ForMember(ti => ti.LastName, x => x.MapFrom(hi => hi.LastName))
                .ForMember(ti => ti.JobTitle, x => x.MapFrom(hi => hi.JobTitle))
                .ForMember(ti => ti.Company, x => x.MapFrom(hi => hi.Company))
                .ForMember(ti => ti.Department, x => x.MapFrom(hi => hi.Department))
                .ForMember(ti => ti.Office, x => x.MapFrom(hi => hi.Office))
                .ForMember(ti => ti.BusinessPhone, x => x.MapFrom(hi => hi.BusinessPhone))
                .ForMember(ti => ti.Fax, x => x.MapFrom(hi => hi.Fax))
                .ForMember(ti => ti.HomePhone, x => x.MapFrom(hi => hi.HomePhone))
                .ForMember(ti => ti.MobilePhone, x => x.MapFrom(hi => hi.MobilePhone))
                .ForMember(ti => ti.Pager, x => x.MapFrom(hi => hi.Pager))
                .ForMember(ti => ti.WebPage, x => x.MapFrom(hi => hi.WebPage))
                .ForMember(ti => ti.Address, x => x.MapFrom(hi => hi.Address))
                .ForMember(ti => ti.City, x => x.MapFrom(hi => hi.City))
                .ForMember(ti => ti.State, x => x.MapFrom(hi => hi.State))
                .ForMember(ti => ti.Zip, x => x.MapFrom(hi => hi.Zip))
                .ForMember(ti => ti.Country, x => x.MapFrom(hi => hi.Country))
                .ForMember(ti => ti.Notes, x => x.MapFrom(hi => hi.Notes))
                .ForMember(ti => ti.PasswordExpirationDateTime, x => x.MapFrom(hi => hi.PasswordExpirationDateTime))
                .ForMember(ti => ti.ExternalEmail, x => x.MapFrom(hi => hi.ExternalEmail));
        }
    }
}

