// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;

namespace SolidCP.Setup
{
    #region Security enums
    public enum ControlFlags
    {
        SE_OWNER_DEFAULTED = 0x1,
        SE_GROUP_DEFAULTED = 0x2,
        SE_DACL_PRESENT = 0x4,
        SE_DACL_DEFAULTED = 0x8,
        SE_SACL_PRESENT = 0x10,
        SE_SACL_DEFAULTED = 0x20,
        SE_DACL_AUTO_INHERIT_REQ = 0x100,
        SE_SACL_AUTO_INHERIT_REQ = 0x200,
        SE_DACL_AUTO_INHERITED = 0x400,
        SE_SACL_AUTO_INHERITED = 0x800,
        SE_DACL_PROTECTED = 0x1000,
        SE_SACL_PROTECTED = 0x2000,
        SE_SELF_RELATIVE = 0x8000
    }

    [Flags]
    public enum SystemAccessMask : uint
    {
        // Grants the right to read data from the file. For a directory, this value grants 
        // the right to list the contents of the directory.
        FILE_LIST_DIRECTORY = 0x1,

        // Grants the right to write data to the file. For a directory, this value grants
        // the right to create a file in the directory.
        FILE_ADD_FILE = 0x2,

        // Grants the right to append data to the file. For a directory, this value grants
        // the right to create a subdirectory.
        FILE_ADD_SUBDIRECTORY = 0x4,

        // Grants the right to read extended attributes. 
        FILE_READ_EA = 0x8,

        // Grants the right to write extended attributes.
        FILE_WRITE_EA = 0x10,

        // Grants the right to execute a file. For a directory, the directory can be traversed.
        FILE_TRAVERSE = 0x20,

        // Grants the right to delete a directory and all the files it contains (its children),
        // even if the files are read-only.
        FILE_DELETE_CHILD = 0x40,

        // Grants the right to read file attributes.
        FILE_READ_ATTRIBUTES = 0x80,

        // Grants the right to change file attributes.
        FILE_WRITE_ATTRIBUTES = 0x100,

        // Grants delete access. 
        DELETE = 0x10000,

        // Grants read access to the security descriptor and owner.
        READ_CONTROL = 0x20000,

        // Grants write access to the discretionary ACL.
        WRITE_DAC = 0x40000,

        // Assigns the write owner.
        WRITE_OWNER = 0x80000,

        // Synchronizes access and allows a process to wait for an object to enter the signaled state.
        SYNCHRONIZE = 0x100000
    }

    [Flags]
	public enum AceFlags : uint
    {
        // Non-container child objects inherit the ACE as an effective ACE. For child objects that are containers, 
        // the ACE is inherited as an inherit-only ACE unless the NO_PROPAGATE_INHERIT_ACE bit flag is also set.
        OBJECT_INHERIT_ACE = 0x1,

        // Child objects that are containers, such as directories, inherit the ACE as an effective ACE. The inherited 
        // ACE is inheritable unless the NO_PROPAGATE_INHERIT_ACE bit flag is also set. 
        CONTAINER_INHERIT_ACE = 0x2,

        // If the ACE is inherited by a child object, the system clears the OBJECT_INHERIT_ACE and CONTAINER_INHERIT_ACE
        // flags in the inherited ACE. This prevents the ACE from being inherited by subsequent generations of objects. 
        NO_PROPAGATE_INHERIT_ACE = 0x4,

        // Indicates an inherit-only ACE which does not control access to the object to which it is attached. If this 
        // flag is not set, the ACE is an effective ACE which controls access to the object to which it is attached. 
        // Both effective and inherit-only ACEs can be inherited depending on the state of the other inheritance flags.
        INHERIT_ONLY_ACE = 0x8,

        // The system sets this bit when it propagates an inherited ACE to a child object. 
        INHERITED_ACE = 0x10,

        // Used with system-audit ACEs in a SACL to generate audit messages for successful access attempts. 
        SUCCESSFUL_ACCESS_ACE_FLAG = 0x40,

        // Used with system-audit ACEs in a SACL to generate audit messages for failed access attempts.
        FAILED_ACCESS_ACE_FLAG = 0x80
    }

    /// <summary>
    /// Change options
    /// </summary>
    public enum ChangeOption
    {
        /// <summary>Change the owner of the logical file.</summary>
        CHANGE_OWNER_SECURITY_INFORMATION = 1,

        /// <summary>Change the group of the logical file.</summary>  
        CHANGE_GROUP_SECURITY_INFORMATION = 2,

        /// <summary>Change the ACL of the logical file.</summary>
        CHANGE_DACL_SECURITY_INFORMATION = 4,

        /// <summary>Change the system ACL of the logical file.</summary> 
        CHANGE_SACL_SECURITY_INFORMATION = 8
    }

    /// <summary>
    /// Ace types
    /// </summary>
    public enum AceType
    {
        /// <summary>Allowed</summary>
        Allowed = 0,
        /// <summary>Denied</summary>
        Denied = 1,
        /// <summary>Audit</summary>
        Audit = 2
    }

    /// <summary>
    /// NTFS permissions
    /// </summary>
    [Flags]
    [Serializable]
    public enum NtfsPermission : int
    {
        /// <summary>FullControl</summary>
        FullControl = 0x1,// = 0x1F01FF,
        /// <summary>Modify</summary>
        Modify = 0x2,// = 0x1301BF,
        /// <summary>Execute</summary>
        Execute = 0x4,// = 0x1200A9,
        /// <summary>ListFolderContents</summary>
        ListFolderContents = 0x8,// = 0x1200A9,
        /// <summary>Read</summary>
        Read = 0x10,// = 0x120089,
        /// <summary>Write</summary>
        Write = 0x20// = 0x100116
    }

    /// <summary>
    /// Well-known SIDs
    /// </summary>
    public class SystemSID
    {
        /// <summary>"Administrators" SID</summary>
        public const string ADMINISTRATORS = "S-1-5-32-544";

        /// <summary>"Local System (SYSTEM)" SID</summary>
        public const string SYSTEM = "S-1-5-18";

        /// <summary>"NETWORK SERVICE" SID</summary>
        public const string NETWORK_SERVICE = "S-1-5-20";
    }
    #endregion
}
