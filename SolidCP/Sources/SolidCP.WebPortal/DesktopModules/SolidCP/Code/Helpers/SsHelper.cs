using SolidCP.Providers.StorageSpaces;

namespace SolidCP.Portal
{
    public class SsHelper
    {
        #region Storage Space Levels

        StorageSpaceLevelPaged ssLevels;

        public int GetStorageSpaceLevelsPagedCount(string filterValue)
        {
            return ssLevels.RecordsCount;
        }

        public StorageSpaceLevel[] GetStorageSpaceLevelsPaged(int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            ssLevels = ES.Services.StorageSpaces.GetStorageSpaceLevelsPaged("Name", filterValue, sortColumn, startRowIndex, maximumRows);

            return ssLevels.Levels;
        }

        #endregion 

        #region Storage Spaces

        StorageSpacesPaged sSpaces;

        public int GetStorageSpacePagedCount(string filterValue)
        {
            return sSpaces.RecordsCount;
        }

        public StorageSpace[] GetStorageSpacePaged(int maximumRows, int startRowIndex, string sortColumn, string filterValue)
        {
            sSpaces = ES.Services.StorageSpaces.GetStorageSpacesPaged("Name", filterValue, sortColumn, startRowIndex, maximumRows);

            return sSpaces.Spaces;
        }

        #endregion 
    }
}