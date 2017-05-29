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
using System.Management;
using System.Diagnostics;

namespace SolidCP.Providers.Virtualization
{
    internal class Wmi
    {
        string nameSpace = null;
        string computerName = null;
        ManagementScope scope = null;

        public Wmi(string nameSpace) : this(nameSpace, null)
        {
        }

        public Wmi(string computerName, string nameSpace)
        {
            this.nameSpace = nameSpace;
            this.computerName = computerName;
        }

        internal ManagementObjectCollection ExecuteWmiQuery(string query, params object[] args)
        {
            if (args != null && args.Length > 0)
                query = String.Format(query, args);

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(GetScope(),
                new ObjectQuery(query));
            return searcher.Get();
        }

        internal ManagementObject GetWmiObject(string className, string filter, params object[] args)
        {
            ManagementObjectCollection col = GetWmiObjects(className, filter, args);
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = col.GetEnumerator();
            return enumerator.MoveNext() ? (ManagementObject)enumerator.Current : null;
        }

        internal ManagementObject GetWmiObject(string className)
        {
            return GetWmiObject(className, null);
        }

        internal ManagementObjectCollection GetWmiObjects(string className, string filter, params object[] args)
        {
            string query = "select * from " + className;
            if (!String.IsNullOrEmpty(filter))
                query += " where " + filter;
            return ExecuteWmiQuery(query, args);
        }

        internal ManagementObjectCollection GetWmiObjects(string className)
        {
            return GetWmiObjects(className, null);
        }

        internal ManagementObject GetWmiObjectByPath(string path)
        {
            return new ManagementObject(GetScope(), new ManagementPath(path), null);
        }

        internal ManagementClass GetWmiClass(string className)
        {
            return new ManagementClass(GetScope(), new ManagementPath(className), null);
        }

        internal ManagementObject GetRelatedWmiObject(ManagementObject obj, string className)
        {
            ManagementObjectCollection col = obj.GetRelated(className);
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = col.GetEnumerator();
            enumerator.MoveNext();
            return (ManagementObject)enumerator.Current;
        }

        internal void Dump(ManagementBaseObject obj)
        {
#if DEBUG
            foreach (PropertyData prop in obj.Properties)
            {
                string typeName = prop.Value == null ? "null" : prop.Value.GetType().ToString();
                Debug.WriteLine(prop.Name + ": " + prop.Value + " (" + typeName + ")");
            }
#endif
        }

        // Converts a given datetime in DMTF format to System.DateTime object.
        internal System.DateTime ToDateTime(string dmtfDate)
        {
            System.DateTime initializer = System.DateTime.MinValue;
            int year = initializer.Year;
            int month = initializer.Month;
            int day = initializer.Day;
            int hour = initializer.Hour;
            int minute = initializer.Minute;
            int second = initializer.Second;
            long ticks = 0;
            string dmtf = dmtfDate;
            System.DateTime datetime = System.DateTime.MinValue;
            string tempString = string.Empty;
            if (String.IsNullOrEmpty(dmtf))
            {
                return DateTime.MinValue;
            }
            else if ((dmtf.Length != 25))
            {
                throw new System.ArgumentOutOfRangeException();
            }
            try
            {
                tempString = dmtf.Substring(0, 4);
                if (("****" != tempString))
                {
                    year = int.Parse(tempString);
                }
                tempString = dmtf.Substring(4, 2);
                if (("**" != tempString))
                {
                    month = int.Parse(tempString);
                }
                tempString = dmtf.Substring(6, 2);
                if (("**" != tempString))
                {
                    day = int.Parse(tempString);
                }
                tempString = dmtf.Substring(8, 2);
                if (("**" != tempString))
                {
                    hour = int.Parse(tempString);
                }
                tempString = dmtf.Substring(10, 2);
                if (("**" != tempString))
                {
                    minute = int.Parse(tempString);
                }
                tempString = dmtf.Substring(12, 2);
                if (("**" != tempString))
                {
                    second = int.Parse(tempString);
                }
                tempString = dmtf.Substring(15, 6);
                if (("******" != tempString))
                {
                    ticks = (long.Parse(tempString) * ((long)((System.TimeSpan.TicksPerMillisecond / 1000))));
                }
                if (((((((((year < 0)
                            || (month < 0))
                            || (day < 0))
                            || (hour < 0))
                            || (minute < 0))
                            || (minute < 0))
                            || (second < 0))
                            || (ticks < 0)))
                {
                    throw new System.ArgumentOutOfRangeException();
                }
            }
            catch (System.Exception e)
            {
                throw new System.ArgumentOutOfRangeException(null, e.Message);
            }

            if (year == 0
                && month == 0
                && day == 0
                && hour == 0
                && minute == 0
                && second == 0
                && ticks == 0)
                return DateTime.MinValue;

            datetime = new System.DateTime(year, month, day, hour, minute, second, 0);
            datetime = datetime.AddTicks(ticks);
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(datetime);
            int UTCOffset = 0;
            int OffsetToBeAdjusted = 0;
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            tempString = dmtf.Substring(22, 3);
            if ((tempString != "******"))
            {
                tempString = dmtf.Substring(21, 4);
                try
                {
                    UTCOffset = int.Parse(tempString);
                }
                catch (System.Exception e)
                {
                    throw new System.ArgumentOutOfRangeException(null, e.Message);
                }
                OffsetToBeAdjusted = ((int)((OffsetMins - UTCOffset)));
                datetime = datetime.AddMinutes(((double)(OffsetToBeAdjusted)));
            }
            return datetime;
        }

        // Converts a given System.DateTime object to DMTF datetime format.
        internal string ToDmtfDateTime(System.DateTime date)
        {
            string utcString = string.Empty;
            System.TimeSpan tickOffset = System.TimeZone.CurrentTimeZone.GetUtcOffset(date);
            long OffsetMins = ((long)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute)));
            if ((System.Math.Abs(OffsetMins) > 999))
            {
                date = date.ToUniversalTime();
                utcString = "+000";
            }
            else
            {
                if ((tickOffset.Ticks >= 0))
                {
                    utcString = string.Concat("+", ((System.Int64)((tickOffset.Ticks / System.TimeSpan.TicksPerMinute))).ToString().PadLeft(3, '0'));
                }
                else
                {
                    string strTemp = ((System.Int64)(OffsetMins)).ToString();
                    utcString = string.Concat("-", strTemp.Substring(1, (strTemp.Length - 1)).PadLeft(3, '0'));
                }
            }
            string dmtfDateTime = ((System.Int32)(date.Year)).ToString().PadLeft(4, '0');
            dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Month)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Day)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Hour)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Minute)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ((System.Int32)(date.Second)).ToString().PadLeft(2, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, ".");
            System.DateTime dtTemp = new System.DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, 0);
            long microsec = ((long)((((date.Ticks - dtTemp.Ticks)
                        * 1000)
                        / System.TimeSpan.TicksPerMillisecond)));
            string strMicrosec = ((System.Int64)(microsec)).ToString();
            if ((strMicrosec.Length > 6))
            {
                strMicrosec = strMicrosec.Substring(0, 6);
            }
            dmtfDateTime = string.Concat(dmtfDateTime, strMicrosec.PadLeft(6, '0'));
            dmtfDateTime = string.Concat(dmtfDateTime, utcString);
            return dmtfDateTime;
        }

        public ManagementScope GetScope()
        {
            if (scope != null)
                return scope;

            // create new scope
            if (String.IsNullOrEmpty(computerName))
            {
                // local
                scope = new ManagementScope(nameSpace);
            }
            else
            {
                // remote
                ConnectionOptions options = new ConnectionOptions();

                string path = String.Format(@"\\{0}\{1}", computerName, nameSpace);
                scope = new ManagementScope(path, options);
            }

            // connect
            scope.Connect();
            return scope;
        }
    }
}
