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

using System;
using CryptSharp.Internal;
using CryptSharp.Utility;

namespace CryptSharp
{
    /// <summary>
    /// The key type for options.
    /// </summary>
    public class CrypterOptionKey
    {
        /// <summary>
        /// Creates a new option key.
        /// </summary>
        /// <param name="description">A description of the option.</param>
        /// <param name="valueType">The type of the option's value.</param>
        public CrypterOptionKey(string description, Type valueType)
        {
            Check.Null("description", description);
            Check.Null("valueType", valueType);

            Description = description;
            ValueType = valueType;
        }

        /// <summary>
        /// Throws an exception if the value is incompatible with this option.
        /// </summary>
        /// <param name="value">The value to check.</param>
        public void CheckValue(object value)
        {
            if (value == null)
            {
                throw Exceptions.ArgumentNull(null);
            }

            if (!ValueType.IsAssignableFrom(value.GetType()))
            {
                throw Exceptions.Argument(null, "Value is incompatible with type {0}.", ValueType);
            }

            OnCheckValue(value);
        }

        /// <summary>
        /// Override this to provide additional validation for an option.
        /// </summary>
        /// <param name="value">The value to check.</param>
        protected virtual void OnCheckValue(object value)
        {

        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Description;
        }

        /// <summary>
        /// A description of the option.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// The type of the option's value.
        /// </summary>
        public Type ValueType
        {
            get;
            private set;
        }
    }
}
