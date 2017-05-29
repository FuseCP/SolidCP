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

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;

namespace SolidCP.Templates
{
    internal class BuiltinFunctions
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public TemplateContext Context { get; set; }

        public bool IsDefined(string variable)
        {
            return Context.Variables.ContainsKey(variable);
        }

        public object IfDefined(string variable, object val)
        {
            return IsDefined(variable) ? val : "";
        }

        public new bool Equals(object objA, object objB)
        {
            return Object.Equals(objA, objB);
        }

        public bool NotEquals(object objA, object objB)
        {
            return !Equals(objA, objB);
        }

        public bool IsEven(object number)
        {
            try
            {
                Int32 num = Convert.ToInt32(number);
                return num % 2 == 0;
            }
            catch
            {
                throw new ParserException("Cannot convert IsEven() function parameter to integer", Line, Column);
            }
        }

        public bool IsOdd(object number)
        {
            try
            {
                Int32 num = Convert.ToInt32(number);
                return num % 2 == 1;
            }
            catch
            {
                throw new ParserException("Cannot convert IsOdd() function parameter to integer", Line, Column);
            }
        }

        public bool IsEmpty(string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public bool IsNotEmpty(string str)
        {
            return !IsEmpty(str);
        }

        public bool IsNumber(object val)
        {
            try
            {
                int number = Convert.ToInt32(val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string ToUpper(string str)
        {
            if (str == null)
                return null;

            return str.ToUpper();
        }

        public string ToLower(string str)
        {
            if (str == null)
                return null;

            return str.ToLower();
        }

        public int Len(string str)
        {
            return String.IsNullOrEmpty(str) ? 0 : str.Length;
        }

        public string ToList(object collection, string propertyName, string delimiter)
        {
            IEnumerable list = collection as IEnumerable;
            if (list == null)
                throw new ParserException("Supplied collection must implement IEnumerable", Line, Column);

            // get property descriptor
            List<string> items = new List<string>();
            foreach (object item in list)
            {
                PropertyInfo prop = null;
                if(!String.IsNullOrEmpty(propertyName))
                {
                    prop = item.GetType().GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                }

                if (prop != null)
                    items.Add(prop.GetValue(item, null).ToString());
                else
                    items.Add(item.ToString());
            }
            return String.Join(delimiter, items.ToArray());
        }

        public bool IsNull(object val)
        {
            return val == null;
        }

        public object IIf(bool condition, object trueValue, object falseValue)
        {
            return condition ? trueValue : falseValue;
        }

        public object Format(object val, string format)
        {
            if (val == null || !(val is IFormattable))
                return val;

            if (val is Int32)
                return ((Int32)val).ToString(format);
            else if (val is Decimal)
                return ((Decimal)val).ToString(format);
            else
                return val.ToString();
        }

        public string Trim(string str)
        {
            return str != null ? str.Trim() : null;
        }

        public int Compare(object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null)
                throw new ParserException("Function argument cannot be NULL", Line, Column);

            if (!(obj1 is IComparable) || !(obj2 is IComparable))
                throw new ParserException("Function arguments must implement IComparable", Line, Column);

            return ((IComparable)obj1).CompareTo(obj2);
        }

        public bool CompareNoCase(string str1, string str2)
        {
            return String.Compare(str1, str2, true) == 0;
        }

        public string StripNewLines(string str)
        {
            return str != null ? str.Replace("\r\n", " ") : null;
        }

        public string TypeOf(object obj)
        {
            return obj != null ? obj.GetType().Name : null;
        }

        public Int32 CInt(object val)
        {
            try
            {
                return Convert.ToInt32(val);
            }
            catch
            {
                throw new ParserException("Value cannot be converted to Int32", Line, Column);
            }
        }

        public Double CDouble(object val)
        {
            try
            {
                return Convert.ToDouble(val);
            }
            catch
            {
                throw new ParserException("Value cannot be converted to Double", Line, Column);
            }
        }

        public DateTime CDate(object val)
        {
            try
            {
                return Convert.ToDateTime(val);
            }
            catch
            {
                throw new ParserException("Value cannot be converted to DateTime", Line, Column);
            }
        }
    }
}
