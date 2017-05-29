using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidCP.EnterpriseServer.Base.HostedSolution
{
    public class OrganizationStatistics
    {
        private int allocatedUsers;
        private int createdUsers;

        private int allocatedGroups;
        private int createdGroups;

        private int allocatedDomains;
        private int createdDomains;

        private int allocatedMailboxes;
        private int createdMailboxes;

        private int allocatedContacts;
        private int createdContacts;

        private int allocatedDistributionLists;
        private int createdDistributionLists;

        private int allocatedPublicFolders;
        private int createdPublicFolders;

        private int allocatedDiskSpace;
        private int usedDiskSpace;

        private int allocatedLitigationHoldSpace;
        private int usedLitigationHoldSpace;


        private int allocatedSharePointSiteCollections;
        private int createdSharePointSiteCollections;

        private int allocatedSharePointEnterpriseSiteCollections;
        private int createdSharePointEnterpriseSiteCollections;

        private int createdCRMUsers;
        private int allocatedCRMUsers;

        private int createdLimitedCRMUsers;
        private int allocatedLimitedCRMUsers;

        private int createdESSCRMUsers;
        private int allocatedESSCRMUsers;

        private long usedCRMDiskSpace;
        private long allocatedCRMDiskSpace;

        private int createdEnterpriseStorageFolders;
        private int allocatedEnterpriseStorageFolders;

        private int allocatedEnterpriseStorageSpace;
        private int usedEnterpriseStorageSpace;

        private int createdProfessionalCRMUsers;
        private int allocatedProfessionalCRMUsers;

        private int allocatedDeletedUsers;
        private int deletedUsers;

        private int allocatedDeletedUsersBackupStorageSpace;
        private int usedDeletedUsersBackupStorageSpace;

        public int CreatedProfessionalCRMUsers
        {
            get { return createdProfessionalCRMUsers; }
            set { createdProfessionalCRMUsers = value; }
        }

        public int AllocatedProfessionalCRMUsers
        {
            get { return allocatedProfessionalCRMUsers; }
            set { allocatedProfessionalCRMUsers = value; }
        }


        private int createdBasicCRMUsers;
        private int allocatedBasicCRMUsers;

        public int CreatedBasicCRMUsers
        {
            get { return createdBasicCRMUsers; }
            set { createdBasicCRMUsers = value; }
        }

        public int AllocatedBasicCRMUsers
        {
            get { return allocatedBasicCRMUsers; }
            set { allocatedBasicCRMUsers = value; }
        }

        private int createdEssentialCRMUsers;
        private int allocatedEssentialCRMUsers;

        public int CreatedEssentialCRMUsers
        {
            get { return createdEssentialCRMUsers; }
            set { createdEssentialCRMUsers = value; }
        }

        public int AllocatedEssentialCRMUsers
        {
            get { return allocatedEssentialCRMUsers; }
            set { allocatedEssentialCRMUsers = value; }
        }


        public int CreatedCRMUsers
        {
            get { return createdCRMUsers; }
            set { createdCRMUsers = value; }
        }

        public int AllocatedCRMUsers
        {
            get { return allocatedCRMUsers; }
            set { allocatedCRMUsers = value; }
        }

        public int CreatedLimitedCRMUsers
        {
            get { return createdLimitedCRMUsers; }
            set { createdLimitedCRMUsers = value; }
        }

        public int AllocatedLimitedCRMUsers
        {
            get { return allocatedLimitedCRMUsers; }
            set { allocatedLimitedCRMUsers = value; }
        }

        public int CreatedESSCRMUsers
        {
            get { return createdESSCRMUsers; }
            set { createdESSCRMUsers = value; }
        }

        public int AllocatedESSCRMUsers
        {
            get { return allocatedESSCRMUsers; }
            set { allocatedESSCRMUsers = value; }
        }

        public int AllocatedUsers
        {
            get { return allocatedUsers; }
            set { allocatedUsers = value; }
        }

        public long UsedCRMDiskSpace
        {
            get { return usedCRMDiskSpace; }
            set { usedCRMDiskSpace = value; }
        }

        public long AllocatedCRMDiskSpace
        {
            get { return allocatedCRMDiskSpace; }
            set { allocatedCRMDiskSpace = value; }
        }

        public int CreatedUsers
        {
            get { return createdUsers; }
            set { createdUsers = value; }
        }

        public int AllocatedMailboxes
        {
            get { return allocatedMailboxes; }
            set { allocatedMailboxes = value; }
        }

        public int CreatedMailboxes
        {
            get { return createdMailboxes; }
            set { createdMailboxes = value; }
        }

        public int AllocatedContacts
        {
            get { return allocatedContacts; }
            set { allocatedContacts = value; }
        }

        public int CreatedContacts
        {
            get { return createdContacts; }
            set { createdContacts = value; }
        }

        public int AllocatedDistributionLists
        {
            get { return allocatedDistributionLists; }
            set { allocatedDistributionLists = value; }
        }

        public int CreatedDistributionLists
        {
            get { return createdDistributionLists; }
            set { createdDistributionLists = value; }
        }

        public int AllocatedPublicFolders
        {
            get { return allocatedPublicFolders; }
            set { allocatedPublicFolders = value; }
        }

        public int CreatedPublicFolders
        {
            get { return createdPublicFolders; }
            set { createdPublicFolders = value; }
        }

        public int AllocatedDomains
        {
            get { return allocatedDomains; }
            set { allocatedDomains = value; }
        }

        public int CreatedDomains
        {
            get { return createdDomains; }
            set { createdDomains = value; }
        }

        public int AllocatedDiskSpace
        {
            get { return allocatedDiskSpace; }
            set { allocatedDiskSpace = value; }
        }

        public int UsedDiskSpace
        {
            get { return usedDiskSpace; }
            set { usedDiskSpace = value; }
        }

        public int AllocatedLitigationHoldSpace
        {
            get { return allocatedLitigationHoldSpace; }
            set { allocatedLitigationHoldSpace = value; }
        }

        public int UsedLitigationHoldSpace
        {
            get { return usedLitigationHoldSpace; }
            set { usedLitigationHoldSpace = value; }
        }

        public int AllocatedSharePointSiteCollections
        {
            get { return allocatedSharePointSiteCollections; }
            set { allocatedSharePointSiteCollections = value; }
        }

        public int CreatedSharePointSiteCollections
        {
            get { return createdSharePointSiteCollections; }
            set { createdSharePointSiteCollections = value; }
        }

        public int AllocatedSharePointEnterpriseSiteCollections
        {
            get { return allocatedSharePointEnterpriseSiteCollections; }
            set { allocatedSharePointEnterpriseSiteCollections = value; }
        }

        public int CreatedSharePointEnterpriseSiteCollections
        {
            get { return createdSharePointEnterpriseSiteCollections; }
            set { createdSharePointEnterpriseSiteCollections = value; }
        }

        public int CreatedBlackBerryUsers { get; set; }
        public int AllocatedBlackBerryUsers { get; set; }

        public int CreatedOCSUsers { get; set; }
        public int AllocatedOCSUsers { get; set; }

        public int CreatedLyncUsers { get; set; }
        public int AllocatedLyncUsers { get; set; }

        public int CreatedSfBUsers { get; set; }
        public int AllocatedSfBUsers { get; set; }


        public int CreatedEnterpriseStorageFolders
        {
            get { return createdEnterpriseStorageFolders; }
            set { createdEnterpriseStorageFolders = value; }
        }

        public int AllocatedEnterpriseStorageFolders
        {
            get { return allocatedEnterpriseStorageFolders; }
            set { allocatedEnterpriseStorageFolders = value; }
        }

        public int AllocatedEnterpriseStorageSpace
        {
            get { return allocatedEnterpriseStorageSpace; }
            set { allocatedEnterpriseStorageSpace = value; }
        }

        public int UsedEnterpriseStorageSpace
        {
            get { return usedEnterpriseStorageSpace; }
            set { usedEnterpriseStorageSpace = value; }
        }

        public int AllocatedGroups
        {
            get { return allocatedGroups; }
            set { allocatedGroups = value; }
        }

        public int CreatedGroups
        {
            get { return createdGroups; }
            set { createdGroups = value; }
        }

        int allocatedArchingStorage;
        public int AllocatedArchingStorage
        {
            get { return allocatedArchingStorage; }
            set { allocatedArchingStorage = value; }
        }

        int usedArchingStorage;
        public int UsedArchingStorage
        {
            get { return usedArchingStorage; }
            set { usedArchingStorage = value; }
        }

        int allocatedSharedMailboxes;
        public int AllocatedSharedMailboxes
        {
            get { return allocatedSharedMailboxes; }
            set { allocatedSharedMailboxes = value; }
        }

        int createdSharedMailboxes;
        public int CreatedSharedMailboxes
        {
            get { return createdSharedMailboxes; }
            set { createdSharedMailboxes = value; }
        }

        int allocatedResourceMailboxes;
        public int AllocatedResourceMailboxes
        {
            get { return allocatedResourceMailboxes; }
            set { allocatedResourceMailboxes = value; }
        }

        int createdResourceMailboxes;
        public int CreatedResourceMailboxes
        {
            get { return createdResourceMailboxes; }
            set { createdResourceMailboxes = value; }
        }

        public int AllocatedDeletedUsers
        {
            get { return allocatedDeletedUsers; }
            set { allocatedDeletedUsers = value; }
        }

        public int DeletedUsers
        {
            get { return deletedUsers; }
            set { deletedUsers = value; }
        }

        public int AllocatedDeletedUsersBackupStorageSpace
        {
            get { return allocatedDeletedUsersBackupStorageSpace; }
            set { allocatedDeletedUsersBackupStorageSpace = value; }
        }
        public int UsedDeletedUsersBackupStorageSpace
        {
            get { return usedDeletedUsersBackupStorageSpace; }
            set { usedDeletedUsersBackupStorageSpace = value; }
        }

        public int CreatedRdsServers { get; set; }
        public int CreatedRdsCollections { get; set; }
        public int CreatedRdsUsers { get; set; }
        public int AllocatedRdsServers { get; set; }
        public int AllocatedRdsCollections { get; set; }
        public int AllocatedRdsUsers { get; set; }

        public List<QuotaValueInfo> ServiceLevels { get; set; }
    }
}
