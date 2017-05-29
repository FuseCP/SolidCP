using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AutoMapper;
using SolidCP.WebDav.Core.Client;
using SolidCP.WebDav.Core.Config;
using SolidCP.WebDav.Core.Extensions;
using SolidCP.WebDavPortal.Constants;
using SolidCP.WebDavPortal.FileOperations;
using SolidCP.WebDavPortal.Models.FileSystem;

namespace SolidCP.WebDavPortal.Mapping.Profiles.Webdav
{
    public class ResourceTableItemProfile : Profile
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
            var openerManager = new FileOpenerManager();

            Mapper.CreateMap<WebDavResource, ResourceTableItemModel>()
                .ForMember(ti => ti.DisplayName, x => x.MapFrom(hi => hi.DisplayName.Trim('/')))
                .ForMember(ti => ti.Href, x => x.MapFrom(hi => hi.Href))
                .ForMember(ti => ti.Type, x => x.MapFrom(hi => hi.ItemType.GetDescription().ToLowerInvariant()))
                .ForMember(ti => ti.IconHref, x => x.MapFrom(hi => hi.ItemType == ItemType.Folder ? WebDavAppConfigManager.Instance.FileIcons.FolderPath.Trim('~') : WebDavAppConfigManager.Instance.FileIcons[Path.GetExtension(hi.DisplayName.Trim('/'))].Trim('~')))
                .ForMember(ti => ti.IsTargetBlank, x => x.MapFrom(hi => openerManager.GetIsTargetBlank(hi)))
                .ForMember(ti => ti.LastModified, x => x.MapFrom(hi => hi.LastModified))
                .ForMember(ti => ti.LastModifiedFormated, x => x.MapFrom(hi => hi.LastModified == DateTime.MinValue ? "--" : (new WebDavResource(null, hi)).LastModified.ToString(Formats.DateFormatWithTime)))

                .ForMember(ti => ti.Summary, x => x.MapFrom(hi => hi.Summary))
                .ForMember(ti => ti.IsRoot, x => x.MapFrom(hi => hi.IsRootItem))
                .ForMember(ti => ti.Size, x => x.MapFrom(hi => hi.ContentLength))
                .ForMember(ti => ti.Quota, x => x.MapFrom(hi => hi.AllocatedSpace))
                .ForMember(ti => ti.Url, x => x.Ignore())
                .ForMember(ti => ti.FolderUrlAbsoluteString, x => x.Ignore())
                .ForMember(ti => ti.FolderUrlLocalString, x => x.Ignore())
                .ForMember(ti => ti.FolderName, x => x.Ignore())
                .ForMember(ti => ti.IsFolder, x => x.MapFrom(hi => hi.ItemType == ItemType.Folder));
        }
    }
}