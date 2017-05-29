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

﻿namespace SolidCP.EnterpriseServer
{
    public class VirtualizationErrorCodes
    {
        // common
        public const string JOB_START_ERROR = "VPS_JOB_START_ERROR";
        public const string JOB_FAILED_ERROR = "VPS_JOB_FAILED_ERROR";

        // quotas
        public const string QUOTA_EXCEEDED_SERVERS_NUMBER = "VPS_QUOTA_EXCEEDED_SERVERS_NUMBER";
        public const string QUOTA_EXCEEDED_CPU = "VPS_QUOTA_EXCEEDED_CPU";
        public const string QUOTA_EXCEEDED_RAM = "VPS_QUOTA_EXCEEDED_RAM";
        public const string QUOTA_WRONG_RAM = "VPS_QUOTA_WRONG_RAM";
        public const string QUOTA_NOT_IN_DYNAMIC_RAM = "VPS_QUOTA_NOT_IN_DYNAMIC_RAM";
        public const string QUOTA_EXCEEDED_HDD = "VPS_QUOTA_EXCEEDED_HDD";
        public const string QUOTA_WRONG_HDD = "VPS_QUOTA_WRONG_HDD";
        public const string QUOTA_EXCEEDED_SNAPSHOTS = "VPS_QUOTA_EXCEEDED_SNAPSHOTS";
        public const string QUOTA_WRONG_SNAPSHOTS = "VPS_QUOTA_WRONG_SNAPSHOTS";

        public const string QUOTA_EXCEEDED_DVD_ENABLED = "VPS_QUOTA_EXCEEDED_DVD_ENABLED";
        public const string QUOTA_EXCEEDED_CD_ALLOWED = "VPS_QUOTA_EXCEEDED_CD_ALLOWED";

        public const string QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED = "VPS_QUOTA_EXCEEDED_START_SHUTDOWN_ALLOWED";
        public const string QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED = "VPS_QUOTA_EXCEEDED_PAUSE_RESUME_ALLOWED";
        public const string QUOTA_EXCEEDED_REBOOT_ALLOWED = "VPS_QUOTA_EXCEEDED_REBOOT_ALLOWED";
        public const string QUOTA_EXCEEDED_RESET_ALOWED = "VPS_QUOTA_EXCEEDED_RESET_ALOWED";
        public const string QUOTA_EXCEEDED_REINSTALL_ALLOWED = "VPS_QUOTA_EXCEEDED_REINSTALL_ALLOWED";

        public const string QUOTA_EXCEEDED_EXTERNAL_NETWORK_ENABLED = "VPS_QUOTA_EXCEEDED_EXTERNAL_NETWORK_ENABLED";
        public const string QUOTA_EXCEEDED_EXTERNAL_ADDRESSES_NUMBER = "VPS_QUOTA_EXCEEDED_EXTERNAL_ADDRESSES_NUMBER";
        public const string QUOTA_EXCEEDED_PRIVATE_NETWORK_ENABLED = "VPS_QUOTA_EXCEEDED_PRIVATE_NETWORK_ENABLED";
        public const string QUOTA_EXCEEDED_PRIVATE_ADDRESSES_NUMBER = "VPS_QUOTA_EXCEEDED_PRIVATE_ADDRESSES_NUMBER";
        public const string QUOTA_EXCEEDED_MANAGEMENT_NETWORK = "VPS_QUOTA_EXCEEDED_MANAGEMENT_NETWORK";

        public const string QUOTA_TEMPLATE_DISK_MINIMAL_SIZE = "VPS_QUOTA_TEMPLATE_DISK_MINIMAL_SIZE";

        // general
        public const string CREATE_ERROR = "VPS_CREATE_ERROR";
        public const string IMPORT_ERROR = "VPS_IMPORT_ERROR";
        public const string DELETE_ERROR = "VPS_DELETE_ERROR";
        public const string DELETE_VM_FILES_ERROR = "VPS_DELETE_VM_FILES_ERROR";
        public const string GET_OS_TEMPLATES_ERROR = "VPS_GET_OS_TEMPLATES_ERROR";
        public const string CREATE_TASK_START_ERROR = "VPS_CREATE_TASK_START_ERROR";
        public const string CREATE_META_ITEM_ERROR = "VPS_CREATE_META_ITEM_ERROR";

        // mail
        public const string SEND_SUMMARY_LETTER = "VPS_SEND_SUMMARY_LETTER";
        public const string SEND_SUMMARY_LETTER_CODE = "VPS_SEND_SUMMARY_LETTER_CODE";
        public const string SUMMARY_TEMPLATE_IS_EMPTY = "VPS_SUMMARY_TEMPLATE_IS_EMPTY";

        public const string CHANGE_ADMIN_PASSWORD_ERROR = "VPS_CHANGE_ADMIN_PASSWORD_ERROR";
        public const string CHANGE_VM_CONFIGURATION = "VPS_CHANGE_VM_CONFIGURATION";
        public const string CONFIG_NETWORK_ADAPTER_KVP = "VPS_CONFIG_NETWORK_ADAPTER_KVP";

        
        public const string CHANGE_VIRTUAL_MACHINE_STATE_GENERAL_ERROR = "VPS_CHANGE_VIRTUAL_MACHINE_STATE_GENERAL_ERROR";
        public const string CANNOT_CHANGE_VIRTUAL_SERVER_STATE = "VPS_CANNOT_CHANGE_VIRTUAL_SERVER_STATE";

        public const string NOT_ENOUGH_UNALLOTTED_IP_ADDRESSES = "VPS_NOT_ENOUGH_UNALLOTTED_IP_ADDRESSES";

        public const string CANNOT_ADD_IP_ADDRESSES_TO_DATABASE = "VPS_CANNOT_ADD_IP_ADDRESSES_TO_DATABASE";
        public const string ALLOCATE_EXTERNAL_ADDRESSES_GENERAL_ERROR = "VPS_ALLOCATE_EXTERNAL_ADDRESSES_GENERAL_ERROR";
        public const string CANNOT_DELLOCATE_EXTERNAL_ADDRESSES = "VPS_CANNOT_DELLOCATE_EXTERNAL_ADDRESSES";
        public const string CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM = "VPS_CANNOT_FIND_VIRTUAL_MACHINE_META_ITEM";

        // networking
        public const string NOT_ENOUGH_PACKAGE_IP_ADDRESSES = "VPS_NOT_ENOUGH_PACKAGE_IP_ADDRESSES";
        public const string ADD_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR = "VPS_ADD_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR";
        public const string SET_VIRTUAL_MACHINE_PRIMARY_EXTERNAL_IP_ADDRESS_ERROR = "VPS_SET_VIRTUAL_MACHINE_PRIMARY_EXTERNAL_IP_ADDRESS_ERROR";
        public const string DELETE_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR = "VPS_DELETE_VIRTUAL_MACHINE_EXTERNAL_IP_ADDRESS_ERROR";

        public const string WRONG_PRIVATE_IP_ADDRESS_FORMAT = "VPS_WRONG_PRIVATE_IP_ADDRESS_FORMAT";
        public const string ADD_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR = "VPS_ADD_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR";
        public const string SET_VIRTUAL_MACHINE_PRIMARY_PRIVATE_IP_ADDRESS_ERROR = "VPS_SET_VIRTUAL_MACHINE_PRIMARY_PRIVATE_IP_ADDRESS_ERROR";
        public const string DELETE_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR = "VPS_DELETE_VIRTUAL_MACHINE_PRIVATE_IP_ADDRESS_ERROR";

        // snapshots
        public const string TAKE_SNAPSHOT_ERROR = "VPS_TAKE_SNAPSHOT_ERROR";
        public const string APPLY_SNAPSHOT_ERROR = "VPS_APPLY_SNAPSHOT_ERROR";
        public const string RENAME_SNAPSHOT_ERROR = "VPS_RENAME_SNAPSHOT_ERROR";
        public const string DELETE_SNAPSHOT_ERROR = "VPS_DELETE_SNAPSHOT_ERROR";
        public const string DELETE_SNAPSHOT_SUBTREE_ERROR = "VPS_DELETE_SNAPSHOT_SUBTREE_ERROR";

        // DVD disks
        public const string INSERT_DVD_DISK_ERROR = "VPS_INSERT_DVD_DISK_ERROR";
        public const string EJECT_DVD_DISK_ERROR = "VPS_EJECT_DVD_DISK_ERROR";

        // Replication
        public const string SET_REPLICA_SERVER_ERROR = "VPS_SET_REPLICA_SERVER_ERROR";
        public const string UNSET_REPLICA_SERVER_ERROR = "VPS_UNSET_REPLICA_SERVER_ERROR";
        public const string NO_REPLICA_SERVER_ERROR = "VPS_NO_REPLICA_SERVER_ERROR";
        public const string SET_REPLICATION_ERROR = "VPS_SET_REPLICATION_ERROR";
        public const string DISABLE_REPLICATION_ERROR = "VPS_DISABLE_REPLICATION_ERROR";
        public const string PAUSE_REPLICATION_ERROR = "VPS_PAUSE_REPLICATION_ERROR";
        public const string RESUME_REPLICATION_ERROR = "VPS_RESUME_REPLICATION_ERROR";
        public const string QUOTA_REPLICATION_ENABLED = "VPS_QUOTA_REPLICATION_ENABLED";

        
        public const string HOST_NAMER_IS_ALREADY_USED = "HOST_NAMER_IS_ALREADY_USED";
        public const string CANNOT_CHECK_HOST_EXISTS = "CANNOT_CHECK_HOST_EXISTS";

        
    }
}
