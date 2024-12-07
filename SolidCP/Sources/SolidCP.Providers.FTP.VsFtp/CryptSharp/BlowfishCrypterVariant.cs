#region License
/*
CryptSharp
Copyright (c) 2013 James F. Bellinger <http://www.zer7.com/software/cryptsharp>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/
#endregion

namespace CryptSharp
{
    /// <summary>
    /// Variations of the Blowfish crypt algorithm.
    /// 
    /// You only need concern yourself with Blowfish crypt variations if you have passwords
    /// generated pre-2011 using the C-language crypt_blowfish library or a port thereof.
    ///
    /// CryptSharp was implemented from specification and is not a port, and therefore never had the bug
    /// these variants pertain to.
    /// </summary>
    public enum BlowfishCrypterVariant
    {
        /// <summary>
        /// The $2a$ prefix indicates nothing about whether or not the crypted password was created with
        /// a pre-2011 version of the C-language crypt_blowfish library. Pre-2011, that library had a
        /// sign extension bug affecting non-ASCII passwords.
        /// 
        /// See <see cref="CryptSharp.Utility.EksBlowfishKeyExpansionFlags.EmulateCryptBlowfishSignExtensionBug"/>
        /// for a more detailed explanation of the bug in question.
        /// </summary>
        Unspecified,

        /// <summary>
        /// The $2x$ prefix indicates that these passwords were generated with pre-2011 crypt_blowfish
        /// or a port originating from it.
        /// 
        /// If you have old crypted non-ASCII passwords you can't re-derive, and still want to verify them with CryptSharp,
        /// ensure that they have the $2x$ prefix instead of the $2a$ prefix. This will indicate to CryptSharp
        /// that it should emulate the bug when verifying the password.
        /// </summary>
        Compatible,

        /// <summary>
        /// The $2y$ prefix indicates that pre-2011 crypt_blowfish's sign extension bug does not affect
        /// these crypted passwords.
        /// 
        /// For passwords crypted with CryptSharp, this has always been true and as such selecting this
        /// variant changes the prefix but otherwise does not affect the output.
        /// </summary>
        Corrected
    }
}
