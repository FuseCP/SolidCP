using System;
using System.Collections.Generic;
using System.Text;

namespace CryptSharp
{
    /// <summary>
    /// Modified versions of the PHPass crypt algorithm.
    /// </summary>
    public enum PhpassCrypterVariant
    {
        /// <summary>
        /// Standard PHPass. WordPress uses this.
        /// </summary>
        Standard,

        /// <summary>
        /// phpBB changes the prefix but the algorithm is otherwise identical.
        /// </summary>
        Phpbb,

        /// <summary>
        /// Drupal 7+ uses SHA512 instead of MD5.
        /// </summary>
        Drupal
    }
}
