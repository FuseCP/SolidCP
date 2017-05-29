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
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolidCP.EnterpriseServer {

	public struct IPAddress : IComparable {
		public Int128 Address;
		public bool V6 { get; private set; }
		public bool V4 { get { return !V6 || Null; } }
		public bool IsSubnet { get; private set; }
		public bool IsMask { get; private set; }
		public int Cidr { get; private set; }
		public bool Null { get; private set; }
		public IPAddress LastSubnetIP { get { return new IPAddress { Address = (Address | (~((Int128)0) >> (V4 ? Cidr + 64 : Cidr))), Cidr = V4 ? 32 : 128, IsSubnet = false, Null = false, V6 = V6 }; } }
		public IPAddress FirstSubnetIP { get { return new IPAddress { Address = (Address & ~(~((Int128)0) >> (V4 ? Cidr + 64 : Cidr))) + 1, Cidr = V4 ? 32 : 128, IsSubnet = false, Null = false, V6 = V6 }; } }
		public Int128 Mask { get { return IsSubnet ? Int128.MinValue >> (Cidr-1) : Address; } }

		const int c = 256*256;
		
		public static IPAddress Parse(string ip)
        {
			IPAddress adr = default(IPAddress);
			adr.V6 = false;

            if (String.IsNullOrEmpty(ip)) {
				adr.Address = 0; adr.Null = true; adr.Cidr = 32; adr.IsSubnet = false;
				return adr;
			}

			if (ip.Contains('/')) {
				var tokens = ip.Split('/');
				ip = tokens[0];
				adr.IsSubnet = true;
				adr.Cidr = Utils.ParseInt(tokens[1], -1);
			}

			if (string.IsNullOrWhiteSpace(ip)) {
				adr.IsMask = true; adr.V6 = true;
				adr.Address = adr.Mask;
			} else {

				var ipadr = System.Net.IPAddress.Parse(ip);

				if (adr.V6 = ipadr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) {
					byte[] bytes = ipadr.GetAddressBytes();
					Int128 a = 0;
					for (int i = 0; i < 16; i++) {
						a = a * 256 + bytes[i];
					}
					adr.Address = a;
				} else {
					string[] parts = ip.Split('.');
					adr.Address = (Int128)(Int32.Parse(parts[3]) +
						(Int32.Parse(parts[2]) << 8) +
						(Int32.Parse(parts[1]) << 16) +
						(Int32.Parse(parts[0]) << 24));
				}
			}
			if (adr.V4 && (adr.Cidr > 32 || 0 > adr.Cidr)) throw new ArgumentOutOfRangeException("Cidr must not be greater than 32 for IPv4 Addresses.");
			if (adr.V6 && (adr.Cidr > 128 || 0 > adr.Cidr)) throw new ArgumentOutOfRangeException("Cidr must not be greater than 128 for IPv6 Addresses.");
			return adr;
        }

        public override string ToString()
        {
            if (Null)
                return "";
			var s = new System.Text.StringBuilder();
            if (!V6)
            {
                var ipl = Address;
                if (IsMask)
                {
                    int digits = 32 - Cidr;
                    ipl = (Int128.MaxValue << 1) | 0x1; // remove left sign bit
                    ipl = ipl << digits;
                }
                s.Append(String.Format("{0}.{1}.{2}.{3}", (ipl >> 24) & 0xFFL, (ipl >> 16) & 0xFFL, (ipl >> 8) & 0xFFL, (ipl & 0xFFL)));
            }
            else if (!IsMask)
            {
				
				var vals = new List<int>();
				int i;
				Int128 a = Address;
				for (i = 0; i < 8; i++) {
					vals.Add((int)(a % c));
					a = a / c;
				}

				int index = -1, n = 0, m = 0;
				for (i = 7; i >= 0; i--) {
					if (vals[i] == 0) {
						n++;
						if (n > m) {
							index = i;
							m = n;
						}
					}
				}
				index += m-1;

				i = 7;
				while (i >= 0) {
					if (i == index) {
						if (m == 8) s.Append("::");
						else s.Append(":");
						i -= m;
					}
					if (i >= 0) {
						if (i < 7) s.Append(":");
						s.Append(vals[i].ToString("x"));
					}
					i--;
				}
			}
			if (IsSubnet && !(IsMask && V4)) {
				s.Append('/'); s.Append(Cidr.ToString());
			}
			return s.ToString();
		}

		public string ToV4MaskString() {
			V6 = false;
			IsMask = true;
			return ToString();
		}

		public static bool operator ==(IPAddress a, IPAddress b) { return a.Address == b.Address && a.Null == b.Null && (a.Null || !(a.IsSubnet && b.IsSubnet || a.IsMask && b.IsMask) || a.Cidr == b.Cidr); }
		public static bool operator ==(IPAddress a, long b) { return a.Address == b; }
		public static bool operator !=(IPAddress a, IPAddress b) { return !(a == b); }
		public static bool operator !=(IPAddress a, long b) { return a.Address != b; }
		public static bool operator <(IPAddress a, IPAddress b) { return a.Address < b.Address; }
		public static bool operator >(IPAddress a, IPAddress b) { return a.Address > b.Address; }
		public static bool operator <=(IPAddress a, IPAddress b) { return a.Address <= b.Address; }
		public static bool operator >=(IPAddress a, IPAddress b) { return a.Address >= b.Address; }

        public override bool Equals(object obj)
        {
            if (obj is IPAddress)
            {
                var b = (IPAddress)obj;
                return this.Address == b.Address && this.Null == b.Null && (this.Null || !(this.IsSubnet && b.IsSubnet || this.IsMask && b.IsMask) || this.Cidr == b.Cidr);
            }
            else if (obj is long)
            {
                var b = (long)obj;
                return this.Address == b;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.Address.GetHashCode();
        }

		/*
		public static IPAddress operator +(IPAddress a, IPAddress b) {
			if (a.IsSubnet || b.IsSubnet || a.V6 != b.V6) throw new ArgumentException("Arithmetic with subnets or mixed v4 & v6 addresses not supported.");
			return new IPAddress { Address = a.Address + b.Address, Null = a.Null && b.Null, Cidr = 0, V6 = a.V6 };
		}*/

		public static Int128 operator -(IPAddress a, IPAddress b) {
			if (a.IsSubnet || b.IsSubnet || a.V6 != b.V6) throw new ArgumentException("Arithmetic with subnets or mixed v4 & v6 addresses not supported.");
			return a.Address - b.Address;
		}
		public static IPAddress operator +(IPAddress a, Int128 b) {
			return new IPAddress { Address = a.Address + b, Null = a.Null, Cidr = a.V4 ? 32 : 128, V6 = a.V6 };
		}
		public static IPAddress operator -(IPAddress a, Int128 b) {
			return new IPAddress { Address = a.Address - b, Null = a.Null, Cidr = a.V4 ? 32 : 128, V6 = a.V6 };
		}
		public static IPAddress operator |(IPAddress a, IPAddress b) {
			if (a.V6 != b.V6) throw new ArgumentException("Arithmetic with mixed v4 & v6 addresses not supported.");
			return new IPAddress { Address = a.Address | b.Address, Cidr = a.V4 ? 32 : 128, Null = false, V6 = a.V6, IsSubnet = false };
		}
		public static IPAddress operator &(IPAddress a, IPAddress b) {
			if (a.V6 != b.V6) throw new ArgumentException("Arithmetic with mixed v4 & v6 addresses not supported.");
			return new IPAddress { Address = a.Address & b.Address, Cidr = a.V4 ? 32 : 128, Null = false, V6 = a.V6, IsSubnet = false };
		}
		public static IPAddress operator ~(IPAddress a) {
			if (a.Null) return new IPAddress { Address = 0, Null = true, Cidr = a.V4 ? 32 : 128, V6 = true, IsSubnet = false };
			return new IPAddress { Address = ~a.Address, Cidr = a.Cidr , Null = false, V6 = a.V6, IsSubnet = false };
		}

		public static implicit operator IPAddress(NullIPAddress a) { return new IPAddress { Null = true, Address = 0, Cidr = -1 }; }

        public int CompareTo(object obj)
        {
            var a = this.Address;
            var b = ((IPAddress)obj).Address;

            if (a < b)
                return 1;
            else if (a > b)
                return -1;
            else
                return 0;
        }
    }

	public class NullIPAddress { }

}
