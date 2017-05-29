using System;

namespace SolidCP.WebDav.Core.Security.Authorization.Enums
{
    [Flags]
    public enum WebDavPermissions
    {
        Empty = 0,
        None = 1,
        Read = 2,
        Write = 4,
        OwaRead = 8,
        OwaEdit = 16
    }
}