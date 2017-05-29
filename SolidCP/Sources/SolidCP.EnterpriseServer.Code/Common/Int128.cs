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

/*
 * Int128.c: Represents a 128-bit signed integer.
 *
 * Authors:
 *	Simon Mourier
 *
 * Copyright 2010-2012 SoftFluent S.A.S, (http://www.softfluent.com)
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
#if !WINDOWS_PHONE && !SILVERLIGHT
using Microsoft.SqlServer.Server;
#endif

namespace SolidCP.EnterpriseServer
{
    /// <summary>
    /// Represents a 128-bit signed integer.
    /// </summary>
#if !WINDOWS_PHONE && !SILVERLIGHT
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    [TypeConverter(typeof(Int128Converter))]
    public struct Int128 : IComparable<Int128>, IComparable, IEquatable<Int128>, IConvertible, IFormattable
#if !WINDOWS_PHONE && !SILVERLIGHT
, IBinarySerialize
#endif
    {
        private ulong _hi;
        private ulong _lo;

        private const ulong HiNeg = 0x8000000000000000;

        /// <summary>
        /// Gets a value that represents the number 0 (zero).
        /// </summary>
        public static Int128 Zero = GetZero();

        /// <summary>
        /// Represents the largest possible value of an Int128.
        /// </summary>
        public static Int128 MaxValue = GetMaxValue();

        /// <summary>
        /// Represents the smallest possible value of an Int128.
        /// </summary>
        public static Int128 MinValue = GetMinValue();

        private static Int128 GetMaxValue()
        {
            return new Int128(long.MaxValue, ulong.MaxValue);
        }

        private static Int128 GetMinValue()
        {
            return new Int128(0x8000000000000000, 0);
        }

        private static Int128 GetZero()
        {
            return new Int128();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(byte value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public Int128(bool value)
        {
            _hi = 0;
            _lo = (ulong)(value ? 1 : 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(char value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(decimal value)
        {
            if (value < 0)
            {
                Int128 n = -new Int128(-value);
                _hi = n._hi;
                _lo = n._lo;
                return;
            }

            int[] bits = decimal.GetBits(value);
            _hi = (uint)bits[2];
            _lo = (uint)bits[0] | (ulong)bits[1] << 32;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(double value)
            : this((decimal)value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(float value)
            : this((decimal)value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(short value)
        {
            if (value < 0)
            {
                Int128 n = -new Int128(-(value + 1)) - 1;
                _hi = n._hi;
                _lo = n._lo;
                return;
            }

            _hi = 0;
            _lo = (ulong)value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(int value)
        {
            if (value < 0)
            {
                Int128 n = -new Int128(-(value + 1)) - 1;
                _hi = n._hi;
                _lo = n._lo;
                return;
            }

            _hi = 0;
            _lo = (ulong)value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(long value)
        {
            if (value < 0)
            {
                Int128 n = -new Int128(-(value + 1)) - 1;
                _hi = n._hi;
                _lo = n._lo;
                return;
            }

            _hi = 0;
            _lo = (ulong)value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(sbyte value)
        {
            if (value < 0)
            {
                Int128 n = -new Int128(-(value + 1)) - 1;
                _hi = n._hi;
                _lo = n._lo;
                return;
            }

            _hi = 0;
            _lo = (ulong)value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(ushort value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(uint value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(ulong value)
        {
            _hi = 0;
            _lo = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(Guid value)
            : this(value.ToByteArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Int128(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length != 16)
                throw new ArgumentException(null, "value");

            _hi = BitConverter.ToUInt64(value, 8);
            _lo = BitConverter.ToUInt64(value, 0);
        }

        private Int128(ulong hi, ulong lo)
        {
            _hi = hi;
            _lo = lo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int128"/> struct.
        /// </summary>
        /// <param name="sign">The sign.</param>
        /// <param name="ints">The ints.</param>
        public Int128(int sign, uint[] ints)
        {
            if (ints == null)
                throw new ArgumentNullException("ints");

            byte[] lo = new byte[8];
            byte[] hi = new byte[8];

            if (ints.Length > 0)
            {
                Array.Copy(BitConverter.GetBytes(ints[0]), 0, lo, 0, 4);
                if (ints.Length > 1)
                {
                    Array.Copy(BitConverter.GetBytes(ints[1]), 0, lo, 4, 4);
                    if (ints.Length > 2)
                    {
                        Array.Copy(BitConverter.GetBytes(ints[2]), 0, hi, 0, 4);
                        if (ints.Length > 3)
                        {
                            Array.Copy(BitConverter.GetBytes(ints[3]), 0, hi, 4, 4);
                        }
                    }
                }
            }

            _lo = BitConverter.ToUInt64(lo, 0);
            _hi = BitConverter.ToUInt64(hi, 0);

            if (sign < 0)
            {
                _hi |= HiNeg;
            }
            else
            {
                _hi &= ~HiNeg;
            }
        }

        /// <summary>
        /// Gets a number that indicates the sign (negative, positive, or zero) of the current Int128 object.
        /// </summary>
        /// <value>A number that indicates the sign of the Int128 object</value>
        public int Sign
        {
            get
            {
                if (_hi == 0 && _lo == 0)
                    return 0;

                return ((_hi & HiNeg) == 0) ? 1 : -1;
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _hi.GetHashCode() ^ _lo.GetHashCode();
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// true if obj has the same value as this instance; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified Int64 value.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// true if obj has the same value as this instance; otherwise, false.
        /// </returns>
        public bool Equals(Int128 obj)
        {
            return _hi == obj._hi && _lo == obj._lo;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format. Only x, X, g, G, d, D are supported.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            return ToString(format, null);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format. Only x, X, g, G, d, D are supported.</param>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information about this instance.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                formatProvider = CultureInfo.CurrentCulture;
            }

            if (!string.IsNullOrEmpty(format))
            {
                char ch = format[0];
                if ((ch == 'x') || (ch == 'X'))
                {
                    int min;
                    int.TryParse(format.Substring(1).Trim(), out min);
                    return ToHexaString(ch == 'X', min);
                }

                if (((ch != 'G') && (ch != 'g')) && ((ch != 'D') && (ch != 'd')))
                    throw new NotSupportedException("Not supported format: " + format);
            }

            return ToString((NumberFormatInfo)formatProvider.GetFormat(typeof(NumberFormatInfo)));
        }

        private string ToHexaString(bool caps, int min)
        {
            StringBuilder sb = new StringBuilder();
            string x = caps ? "X" : "x";
            if ((min < 0) || (min > 16) || (_hi != 0))
            {
                sb.Append(min > 16 ? _hi.ToString(x + (min - 16)) : _hi.ToString(x));
                sb.Append(_lo.ToString(x + "16"));
            }
            else
            {
                sb.Append(_lo.ToString(x + min));
            }
            return sb.ToString();
        }

        private string ToString(NumberFormatInfo info)
        {
            if (Sign == 0)
                return "0";

            StringBuilder sb = new StringBuilder();
            Int128 ten = new Int128(10);
            Int128 current = this;
            current._hi &= ~HiNeg;
            Int128 r;
            while (true)
            {
                current = DivRem(current, ten, out r);
                if (r._lo > 0 || current.Sign != 0 || (sb.Length == 0))
                {
#if !WINDOWS_PHONE && !SILVERLIGHT
                    sb.Insert(0, (char)('0' + r._lo));
#else
                    sb.Insert(0, new[] { (char)('0' + r._lo) });
#endif
                }
                if (current.Sign == 0)
                    break;
            }

            string s = sb.ToString();
            if ((Sign < 0) && (s != "0"))
                return info.NegativeSign + s;

            return s;
        }

        /// <summary>
        /// Returns the <see cref="T:System.TypeCode"/> for this instance.
        /// </summary>
        /// <returns>
        /// The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Object;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A Boolean value equivalent to the value of this instance.
        /// </returns>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return (bool)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A Unicode character equivalent to the value of this instance.
        /// </returns>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.
        /// </returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.
        /// </returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (int)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (int)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)this;
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)this;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        /// <summary>
        /// Converts the numeric value to an equivalent object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="conversionType">The target conversion type.</param>
        /// <param name="provider">An object that supplies culture-specific information about the conversion.</param>
        /// <param name="value">When this method returns, contains the value that is equivalent to the numeric value, if the conversion succeeded, or is null if the conversion failed. This parameter is passed uninitialized.</param>
        /// <returns>true if this value was converted successfully; otherwise, false.</returns>
        public bool TryConvert(Type conversionType, IFormatProvider provider, out object value)
        {
            if (conversionType == typeof(bool))
            {
                value = (bool)this;
                return true;
            }

            if (conversionType == typeof(byte))
            {
                value = (byte)this;
                return true;
            }

            if (conversionType == typeof(char))
            {
                value = (char)this;
                return true;
            }

            if (conversionType == typeof(decimal))
            {
                value = (decimal)this;
                return true;
            }

            if (conversionType == typeof(double))
            {
                value = (double)this;
                return true;
            }

            if (conversionType == typeof(short))
            {
                value = (short)this;
                return true;
            }

            if (conversionType == typeof(int))
            {
                value = (int)this;
                return true;
            }

            if (conversionType == typeof(long))
            {
                value = (long)this;
                return true;
            }

            if (conversionType == typeof(sbyte))
            {
                value = (sbyte)this;
                return true;
            }

            if (conversionType == typeof(float))
            {
                value = (float)this;
                return true;
            }

            if (conversionType == typeof(string))
            {
                value = ToString(null, provider);
                return true;
            }

            if (conversionType == typeof(ushort))
            {
                value = (ushort)this;
                return true;
            }

            if (conversionType == typeof(uint))
            {
                value = (uint)this;
                return true;
            }

            if (conversionType == typeof(ulong))
            {
                value = (ulong)this;
                return true;
            }

            if (conversionType == typeof(byte[]))
            {
                value = ToByteArray();
                return true;
            }

            if (conversionType == typeof(Guid))
            {
                value = new Guid(ToByteArray());
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Converts the string representation of a number to its Int128 equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <returns>
        /// A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        public static Int128 Parse(string value)
        {
            return Parse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style format to its Int128 equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="style">A bitwise combination of the enumeration values that specify the permitted format of value.</param>
        /// <returns>
        /// A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        public static Int128 Parse(string value, NumberStyles style)
        {
            return Parse(value, style, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Converts the string representation of a number in a culture-specific format to its Int128 equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="provider">An object that provides culture-specific formatting information about value.</param>
        /// <returns>
        /// A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        public static Int128 Parse(string value, IFormatProvider provider)
        {
            return Parse(value, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
        }

        /// <summary>
        /// Converts the string representation of a number in a specified style and culture-specific format to its Int128 equivalent.
        /// </summary>
        /// <param name="value">A string that contains a number to convert.</param>
        /// <param name="style">A bitwise combination of the enumeration values that specify the permitted format of value.</param>
        /// <param name="provider">An object that provides culture-specific formatting information about value.</param>
        /// <returns>A value that is equivalent to the number specified in the value parameter.</returns>
        public static Int128 Parse(string value, NumberStyles style, IFormatProvider provider)
        {
            Int128 result;
            if (!TryParse(value, style, provider, out result))
                throw new ArgumentException(null, "value");

            return result;
        }

        /// <summary>
        /// Tries to convert the string representation of a number to its Int128 equivalent, and returns a value that indicates whether the conversion succeeded..
        /// </summary>
        /// <param name="value">The string representation of a number.</param>
        /// <param name="result">When this method returns, contains the Int128 equivalent to the number that is contained in value, or Int128.Zero if the conversion failed. This parameter is passed uninitialized.</param>
        /// <returns>
        /// true if the value parameter was converted successfully; otherwise, false.
        /// </returns>
        public static bool TryParse(string value, out Int128 result)
        {
            return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        /// <summary>
        /// Tries to convert the string representation of a number in a specified style and culture-specific format to its Int128 equivalent, and returns a value that indicates whether the conversion succeeded..
        /// </summary>
        /// <param name="value">The string representation of a number. The string is interpreted using the style specified by style.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in value. A typical value to specify is NumberStyles.Integer.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information about value.</param>
        /// <param name="result">When this method returns, contains the Int128 equivalent to the number that is contained in value, or Int128.Zero if the conversion failed. This parameter is passed uninitialized.</param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string value, NumberStyles style, IFormatProvider provider, out Int128 result)
        {
            result = Zero;
            if (string.IsNullOrEmpty(value))
                return false;

            if (value.StartsWith("x", StringComparison.OrdinalIgnoreCase))
            {
                style |= NumberStyles.AllowHexSpecifier;
                value = value.Substring(1);
            }
            else if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                style |= NumberStyles.AllowHexSpecifier;
                value = value.Substring(2);
            }

            if ((style & NumberStyles.AllowHexSpecifier) == NumberStyles.AllowHexSpecifier)
                return TryParseHex(value, out result);

            return TryParseNum(value, out result);
        }

        private static bool TryParseHex(string value, out Int128 result)
        {
            if (value.Length > 32)
                throw new OverflowException();

            result = Zero;
            bool hi = false;
            int pos = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                char ch = value[i];
                ulong b;
                if ((ch >= '0') && (ch <= '9'))
                {
                    b = (ulong)(ch - '0');
                }
                else if ((ch >= 'A') && (ch <= 'F'))
                {
                    b = (ulong)(ch - 'A' + 10);
                }
                else if ((ch >= 'a') && (ch <= 'f'))
                {
                    b = (ulong)(ch - 'a' + 10);
                }
                else
                    return false;

                if (hi)
                {
                    result._hi |= b << pos;
                    pos += 4;
                }
                else
                {
                    result._lo |= b << pos;
                    pos += 4;
                    if (pos == 64)
                    {
                        pos = 0;
                        hi = true;
                    }
                }
            }
            return true;
        }

        private static bool TryParseNum(string value, out Int128 result)
        {
            result = Zero;
            foreach (char ch in value)
            {
                byte b;
                if ((ch >= '0') && (ch <= '9'))
                {
                    b = (byte)(ch - '0');
                }
                else
                    return false;

                result = 10 * result;
                result += b;
            }
            return true;
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">The <see cref="T:System.Type"/> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
        /// </returns>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            object value;
            if (TryConvert(conversionType, provider, out value))
                return value;

            throw new InvalidCastException();
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            if (_hi != 0)
                throw new OverflowException();

            return Convert.ToUInt16(_lo);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            if (_hi != 0)
                throw new OverflowException();

            return Convert.ToUInt32(_lo);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies culture-specific formatting information.</param>
        /// <returns>
        /// An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            if (_hi != 0)
                throw new OverflowException();

            return _lo;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="obj"/> is not the same type as this instance. </exception>
        int IComparable.CompareTo(object obj)
        {
            return Compare(this, obj);
        }

        /// <summary>
        /// Compares two Int128 values and returns an integer that indicates whether the first value is less than, equal to, or greater than the second value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>A signed integer that indicates the relative values of left and right, as shown in the following table.</returns>
        public static int Compare(Int128 left, object right)
        {
            if (right is Int128)
                return Compare(left, (Int128)right);

            // NOTE: this could be optimized type per type
            if (right is bool)
                return Compare(left, new Int128((bool)right));

            if (right is byte)
                return Compare(left, new Int128((byte)right));

            if (right is char)
                return Compare(left, new Int128((char)right));

            if (right is decimal)
                return Compare(left, new Int128((decimal)right));

            if (right is double)
                return Compare(left, new Int128((double)right));

            if (right is short)
                return Compare(left, new Int128((short)right));

            if (right is int)
                return Compare(left, new Int128((int)right));

            if (right is long)
                return Compare(left, new Int128((long)right));

            if (right is sbyte)
                return Compare(left, new Int128((sbyte)right));

            if (right is float)
                return Compare(left, new Int128((float)right));

            if (right is ushort)
                return Compare(left, new Int128((ushort)right));

            if (right is uint)
                return Compare(left, new Int128((uint)right));

            if (right is ulong)
                return Compare(left, new Int128((ulong)right));

            byte[] bytes = right as byte[];
            if ((bytes != null) && (bytes.Length != 16))
                return Compare(left, new Int128(bytes));

            if (right is Guid)
                return Compare(left, new Int128((Guid)right));

            throw new ArgumentException();
        }

        /// <summary>
        /// Converts an Int128 value to a byte array.
        /// </summary>
        /// <returns>The value of the current Int128 object converted to an array of bytes.</returns>
        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[16];
            Buffer.BlockCopy(BitConverter.GetBytes(_lo), 0, bytes, 0, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(_hi), 0, bytes, 8, 8);
            return bytes;
        }

        /// <summary>
        /// Compares two 128-bit signed integer values and returns an integer that indicates whether the first value is less than, equal to, or greater than the second value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>
        /// A signed number indicating the relative values of this instance and value.
        /// </returns>
        public static int Compare(Int128 left, Int128 right)
        {
            if (left.Sign < 0)
            {
                if (right.Sign >= 0)
                    return -1;

                ulong xhi = left._hi & ~HiNeg;
                ulong yhi = right._hi & ~HiNeg;
                if (xhi != yhi)
                    return -xhi.CompareTo(yhi);

                return -left._lo.CompareTo(right._lo);
            }

            if (right.Sign < 0)
                return 1;

            if (left._hi != right._hi)
                return left._hi.CompareTo(right._hi);

            return left._lo.CompareTo(right._lo);
        }

        /// <summary>
        /// Compares this instance to a specified 128-bit signed integer and returns an indication of their relative values.
        /// </summary>
        /// <param name="value">An integer to compare.</param>
        /// <returns>A signed number indicating the relative values of this instance and value.</returns>
        public int CompareTo(Int128 value)
        {
            return Compare(this, value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Boolean"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(bool value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Byte"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(byte value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Char"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(char value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Decimal"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Int128(decimal value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Double"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Int128(double value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int16"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(short value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(int value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(long value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.SByte"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(sbyte value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Single"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator Int128(float value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt16"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(ushort value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt32"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(uint value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt64"/> to <see cref="SoftFluent.Int128"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator Int128(ulong value)
        {
            return new Int128(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator bool(Int128 value)
        {
            return value.Sign != 0;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Byte"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator byte(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            if ((value.Sign < 0) || (value._lo > 0xFF))
                throw new OverflowException();

            return (byte)value._lo;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Char"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator char(Int128 value)
        {
            if (value.Sign == 0)
                return (char)0;

            if ((value.Sign < 0) || (value._lo > 0xFFFF))
                throw new OverflowException();

            return (char)(ushort)value._lo;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Decimal"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator decimal(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            return new decimal((int)(value._lo & 0xFFFFFFFF), (int)(value._lo >> 32), (int)(value._hi & 0xFFFFFFFF), value.Sign < 0, 0);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator double(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            double d;
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
            if (!double.TryParse(value.ToString(nfi), NumberStyles.Number, nfi, out d))
                throw new OverflowException();

            return d;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator float(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            float f;
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
            if (!float.TryParse(value.ToString(nfi), NumberStyles.Number, nfi, out f))
                throw new OverflowException();

            return f;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Int16"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator short(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            if (value._lo > 0x8000)
                throw new OverflowException();

            if ((value._lo == 0x8000) && (value.Sign > 0))
                throw new OverflowException();

            return (short)((int)value._lo * value.Sign);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator int(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            if (value._lo > 0x80000000)
                throw new OverflowException();

            if ((value._lo == 0x80000000) && (value.Sign > 0))
                throw new OverflowException();

            return ((int)value._lo * value.Sign);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator long(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            if (value._lo > long.MaxValue)
                throw new OverflowException();

            return ((long)value._lo * value.Sign);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.UInt32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator uint(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            if ((value.Sign < 0) || (value._lo > uint.MaxValue))
                throw new OverflowException();

            return (uint)value._lo;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.UInt16"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator ushort(Int128 value)
        {
            if (value.Sign == 0)
                return 0;

            if ((value.Sign < 0) || (value._lo > ushort.MaxValue))
                throw new OverflowException();

            return (ushort)value._lo;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="SoftFluent.Int128"/> to <see cref="System.UInt64"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static explicit operator ulong(Int128 value)
        {
            if ((value.Sign < 0) || (value._hi != 0))
                throw new OverflowException();

            return value._lo;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >(Int128 left, Int128 right)
        {
            return Compare(left, right) > 0;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <(Int128 left, Int128 right)
        {
            return Compare(left, right) < 0;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator >=(Int128 left, Int128 right)
        {
            return Compare(left, right) >= 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator <=(Int128 left, Int128 right)
        {
            return Compare(left, right) <= 0;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Int128 left, Int128 right)
        {
            return Compare(left, right) != 0;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Int128 left, Int128 right)
        {
            return Compare(left, right) == 0;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int128 operator +(Int128 value)
        {
            return value;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int128 operator -(Int128 value)
        {
            return Negate(value);
        }

        /// <summary>
        /// Negates a specified Int128 value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of the value parameter multiplied by negative one (-1).</returns>
        public static Int128 Negate(Int128 value)
        {
            return new Int128(~value._hi, ~value._lo) + 1;
        }

        /// <summary>
        /// Gets the absolute value this object.
        /// </summary>
        /// <returns>The absolute value.</returns>
        public Int128 ToAbs()
        {
            return Abs(this);
        }

        /// <summary>
        /// Gets the absolute value of an Int128 object.
        /// </summary>
        /// <param name="value">A number.</param>
        /// <returns>
        /// The absolute value.
        /// </returns>
        public static Int128 Abs(Int128 value)
        {
            if (value.Sign < 0)
                return -value;

            return value;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int128 operator +(Int128 left, Int128 right)
        {
            Int128 add = left;
            add._hi += right._hi;
            add._lo += right._lo;
            if (add._lo < left._lo)
            {
                add._hi++;
            }
            return add;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int128 operator -(Int128 left, Int128 right)
        {
            return left + -right;
        }

        /// <summary>
        /// Adds two Int128 values and returns the result.
        /// </summary>
        /// <param name="left">The first value to add.</param>
        /// <param name="right">The second value to add.</param>
        /// <returns>The sum of left and right.</returns>
        public static Int128 Add(Int128 left, Int128 right)
        {
            return left + right;
        }

        /// <summary>
        /// Subtracts one Int128 value from another and returns the result.
        /// </summary>
        /// <param name="left">The value to subtract from (the minuend).</param>
        /// <param name="right">The value to subtract (the subtrahend).</param>
        /// <returns>The result of subtracting right from left.</returns>
        public static Int128 Subtract(Int128 left, Int128 right)
        {
            return left - right;
        }

        /// <summary>
        /// Divides one Int128 value by another and returns the result.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <returns>The quotient of the division.</returns>
        public static Int128 Divide(Int128 dividend, Int128 divisor)
        {
            Int128 integer;
            return DivRem(dividend, divisor, out integer);
        }

        /// <summary>
        /// Divides one Int128 value by another, returns the result, and returns the remainder in an output parameter.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <param name="remainder">When this method returns, contains an Int128 value that represents the remainder from the division. This parameter is passed uninitialized.</param>
        /// <returns>
        /// The quotient of the division.
        /// </returns>
        public static Int128 DivRem(Int128 dividend, Int128 divisor, out Int128 remainder)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            uint[] quotient;
            uint[] rem;
            DivRem(dividend.ToUIn32Array(), divisor.ToUIn32Array(), out quotient, out rem);
            remainder = new Int128(1, rem);
            return new Int128(dividend.Sign * divisor.Sign, quotient);
        }

        private static void DivRem(uint[] dividend, uint[] divisor, out uint[] quotient, out uint[] remainder)
        {
            const ulong hiBit = 0x100000000;
            int divisorLen = GetLength(divisor);
            int dividendLen = GetLength(dividend);
            if (divisorLen <= 1)
            {
                ulong rem = 0;
                uint div = divisor[0];
                quotient = new uint[dividendLen];
                remainder = new uint[1];
                for (int i = dividendLen - 1; i >= 0; i--)
                {
                    rem *= hiBit;
                    rem += dividend[i];
                    ulong q = rem / div;
                    rem -= q * div;
                    quotient[i] = (uint)q;
                }
                remainder[0] = (uint)rem;
                return;
            }

            if (dividendLen >= divisorLen)
            {
                int shift = GetNormalizeShift(divisor[divisorLen - 1]);
                uint[] normDividend = new uint[dividendLen + 1];
                uint[] normDivisor = new uint[divisorLen];
                Normalize(dividend, dividendLen, normDividend, shift);
                Normalize(divisor, divisorLen, normDivisor, shift);
                quotient = new uint[(dividendLen - divisorLen) + 1];
                for (int j = dividendLen - divisorLen; j >= 0; j--)
                {
                    ulong dx = (hiBit * normDividend[j + divisorLen]) + normDividend[(j + divisorLen) - 1];
                    ulong qj = dx / normDivisor[divisorLen - 1];
                    dx -= qj * normDivisor[divisorLen - 1];
                    do
                    {
                        if ((qj < hiBit) && ((qj * normDivisor[divisorLen - 2]) <= ((dx * hiBit) + normDividend[(j + divisorLen) - 2])))
                            break;

                        qj -= 1L;
                        dx += normDivisor[divisorLen - 1];
                    }
                    while (dx < hiBit);

                    long di = 0;
                    long dj;
                    int index = 0;
                    while (index < divisorLen)
                    {
                        ulong dqj = normDivisor[index] * qj;
                        dj = (normDividend[index + j] - ((uint)dqj)) - di;
                        normDividend[index + j] = (uint)dj;
                        dqj = dqj >> 32;
                        dj = dj >> 32;
                        di = ((long)dqj) - dj;
                        index++;
                    }

                    dj = normDividend[j + divisorLen] - di;
                    normDividend[j + divisorLen] = (uint)dj;
                    quotient[j] = (uint)qj;

                    if (dj < 0)
                    {
                        quotient[j]--;
                        ulong sum = 0;
                        for (index = 0; index < divisorLen; index++)
                        {
                            sum = (normDivisor[index] + normDividend[j + index]) + sum;
                            normDividend[j + index] = (uint)sum;
                            sum = sum >> 32;
                        }
                        sum += normDividend[j + divisorLen];
                        normDividend[j + divisorLen] = (uint)sum;
                    }
                }
                remainder = Unnormalize(normDividend, shift);
                return;
            }

            quotient = new uint[0];
            remainder = dividend;
        }

        private static int GetLength(uint[] uints)
        {
            int index = uints.Length - 1;
            while ((index >= 0) && (uints[index] == 0))
            {
                index--;
            }
            return index + 1;
        }

        private static int GetNormalizeShift(uint ui)
        {
            int shift = 0;
            if ((ui & 0xffff0000) == 0)
            {
                ui = ui << 16;
                shift += 16;
            }

            if ((ui & 0xff000000) == 0)
            {
                ui = ui << 8;
                shift += 8;
            }

            if ((ui & 0xf0000000) == 0)
            {
                ui = ui << 4;
                shift += 4;
            }

            if ((ui & 0xc0000000) == 0)
            {
                ui = ui << 2;
                shift += 2;
            }

            if ((ui & 0x80000000) == 0)
            {
                shift++;
            }
            return shift;
        }

        private static uint[] Unnormalize(uint[] normalized, int shift)
        {
            int len = GetLength(normalized);
            uint[] unormalized = new uint[len];
            if (shift > 0)
            {
                int rshift = 32 - shift;
                uint r = 0;
                for (int i = len - 1; i >= 0; i--)
                {
                    unormalized[i] = (normalized[i] >> shift) | r;
                    r = normalized[i] << rshift;
                }
            }
            else
            {
                for (int j = 0; j < len; j++)
                {
                    unormalized[j] = normalized[j];
                }
            }
            return unormalized;
        }

        private static void Normalize(uint[] unormalized, int len, uint[] normalized, int shift)
        {
            int i;
            uint n = 0;
            if (shift > 0)
            {
                int rshift = 32 - shift;
                for (i = 0; i < len; i++)
                {
                    normalized[i] = (unormalized[i] << shift) | n;
                    n = unormalized[i] >> rshift;
                }
            }
            else
            {
                i = 0;
                while (i < len)
                {
                    normalized[i] = unormalized[i];
                    i++;
                }
            }

            while (i < normalized.Length)
            {
                normalized[i++] = 0;
            }

            if (n != 0)
            {
                normalized[len] = n;
            }
        }

        /// <summary>
        /// Performs integer division on two Int128 values and returns the remainder.
        /// </summary>
        /// <param name="dividend">The value to be divided.</param>
        /// <param name="divisor">The value to divide by.</param>
        /// <returns>The remainder after dividing dividend by divisor.</returns>
        public static Int128 Remainder(Int128 dividend, Int128 divisor)
        {
            Int128 remainder;
            DivRem(dividend, divisor, out remainder);
            return remainder;
        }

        /// <summary>
        /// Implements the operator %.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int128 operator %(Int128 dividend, Int128 divisor)
        {
            return Remainder(dividend, divisor);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int128 operator /(Int128 dividend, Int128 divisor)
        {
            return Divide(dividend, divisor);
        }

        /// <summary>
        /// Converts an int128 value to an unsigned long array.
        /// </summary>
        /// <returns>
        /// The value of the current Int128 object converted to an array of unsigned integers.
        /// </returns>
        public ulong[] ToUIn64Array()
        {
            return new[] { _hi, _lo };
        }

        /// <summary>
        /// Converts an int128 value to an unsigned integer array.
        /// </summary>
        /// <returns>The value of the current Int128 object converted to an array of unsigned integers.</returns>
        public uint[] ToUIn32Array()
        {
            uint[] ints = new uint[4];
            byte[] lob = BitConverter.GetBytes(_lo);
            byte[] hib = BitConverter.GetBytes(_hi);

            Buffer.BlockCopy(lob, 0, ints, 0, 4);
            Buffer.BlockCopy(lob, 4, ints, 4, 4);
            Buffer.BlockCopy(hib, 0, ints, 8, 4);
            Buffer.BlockCopy(hib, 4, ints, 12, 4);
            return ints;
        }

        /// <summary>
        /// Returns the product of two Int128 values.
        /// </summary>
        /// <param name="left">The first number to multiply.</param>
        /// <param name="right">The second number to multiply.</param>
        /// <returns>The product of the left and right parameters.</returns>
        public static Int128 Multiply(Int128 left, Int128 right)
        {
            uint[] xInts = left.ToUIn32Array();
            uint[] yInts = right.ToUIn32Array();
            uint[] mulInts = new uint[8];

            for (int i = 0; i < xInts.Length; i++)
            {
                int index = i;
                ulong remainder = 0;
                foreach (uint yi in yInts)
                {
                    remainder = remainder + (ulong)xInts[i] * yi + mulInts[index];
                    mulInts[index++] = (uint)remainder;
                    remainder = remainder >> 32;
                }

                while (remainder != 0)
                {
                    remainder += mulInts[index];
                    mulInts[index++] = (uint)remainder;
                    remainder = remainder >> 32;
                }
            }
            return new Int128(left.Sign * right.Sign, mulInts);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The x.</param>
        /// <param name="right">The y.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Int128 operator *(Int128 left, Int128 right)
        {
            return Multiply(left, right);
        }

        /// <summary>
        /// Implements the operator &gt;&gt;.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="shift">The shift.</param>
        /// <returns>The result of the operator.</returns>
        public static Int128 operator >>(Int128 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value << -shift;

            shift = shift % 128;

            Int128 shifted = new Int128();

            if (shift > 63)
            {
                shifted._lo = value._hi >> (shift - 64);
                shifted._hi = 0;
            }
            else
            {
                shifted._hi = value._hi >> shift;
                shifted._lo = (value._hi << (64 - shift)) | (value._lo >> shift);
            }
            return shifted;
        }

        /// <summary>
        /// Implements the operator &lt;&lt;.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="shift">The shift.</param>
        /// <returns>The result of the operator.</returns>
        public static Int128 operator <<(Int128 value, int shift)
        {
            if (shift == 0)
                return value;

            if (shift < 0)
                return value >> -shift;

            shift = shift % 128;

            Int128 shifted = new Int128();

            if (shift > 63)
            {
                shifted._hi = value._lo << (shift - 64);
                shifted._lo = 0;
            }
            else
            {
                ulong ul = value._lo >> (64 - shift);
                shifted._hi = ul | (value._hi << shift);
                shifted._lo = value._lo << shift;
            }
            return shifted;
        }

        /// <summary>
        /// Implements the operator |.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Int128 operator |(Int128 left, Int128 right)
        {
            if (left == 0)
                return right;

            if (right == 0)
                return left;

            Int128 result = left;
            result._hi |= right._hi;
            result._lo |= right._lo;
            return result;
        }

        /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Int128 operator &(Int128 left, Int128 right)
        {
            if (left == 0 || right == 0)
                return Zero;

            Int128 result = left;
            result._hi &= right._hi;
            result._lo &= right._lo;
            return result;
        }

		    /// <summary>
        /// Implements the operator &amp;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Int128 operator ~(Int128 x)
        {
            Int128 result = x;
            result._hi = ~result._hi;
            result._lo = ~result._lo;
            return result;
        }

#if !WINDOWS_PHONE && !SILVERLIGHT
        void IBinarySerialize.Read(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _hi = reader.ReadUInt64();
            _lo = reader.ReadUInt64();
        }

        void IBinarySerialize.Write(BinaryWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(_hi);
            writer.Write(_lo);
        }
#endif

        /// <summary>
        /// Converts a String type to a Int128 type, and vice versa.
        /// </summary>
        public class Int128Converter : TypeConverter
        {
            /// <summary>
            /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
            /// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;

                return base.CanConvertFrom(context, sourceType);
            }

            /// <summary>
            /// Converts the given object to the type of this converter, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
            /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
            /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
            /// <returns>
            /// An <see cref="T:System.Object"/> that represents the converted value.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">
            /// The conversion cannot be performed.
            /// </exception>
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value != null)
                {
                    Int128 i;
                    if (TryParse(string.Format("{0}", value), out i))
                        return i;
                }
                return new Int128();
            }

            /// <summary>
            /// Returns whether this converter can convert the object to the specified type, using the specified context.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
            /// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to.</param>
            /// <returns>
            /// true if this converter can perform the conversion; otherwise, false.
            /// </returns>
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string))
                    return true;

                return base.CanConvertTo(context, destinationType);
            }

            /// <summary>
            /// Converts the given value object to the specified type, using the specified context and culture information.
            /// </summary>
            /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
            /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/>. If null is passed, the current culture is assumed.</param>
            /// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
            /// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to.</param>
            /// <returns>
            /// An <see cref="T:System.Object"/> that represents the converted value.
            /// </returns>
            /// <exception cref="T:System.ArgumentNullException">
            /// The <paramref name="destinationType"/> parameter is null.
            /// </exception>
            /// <exception cref="T:System.NotSupportedException">
            /// The conversion cannot be performed.
            /// </exception>
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                    return string.Format("{0}", value);

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
