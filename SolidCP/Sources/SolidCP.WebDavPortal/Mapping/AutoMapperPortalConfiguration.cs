using AutoMapper;
using SolidCP.WebDavPortal.Mapping.Profiles.Account;
using SolidCP.WebDavPortal.Mapping.Profiles.Webdav;

namespace SolidCP.WebDavPortal.Mapping
{
    public class AutoMapperPortalConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(
                config =>
                {
                    config.AddProfile<UserProfileProfile>();
                    config.AddProfile<ResourceTableItemProfile>();
                });
        } 
    }
}