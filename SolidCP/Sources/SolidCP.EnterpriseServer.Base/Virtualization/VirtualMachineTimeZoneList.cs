using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidCP.EnterpriseServer.Base.Virtualization
{
    public class VirtualMachineTimeZoneList
    {
        private VirtualMachineTimeZoneList(string index, string displayName, string standardName)
        {
            Id = index;
            DisplayName = displayName;
            StandardName = standardName;
        }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string StandardName { get; set; }

        public static VirtualMachineTimeZoneList GetVirtualMachineTimeZoneById(string id)
        {
            TimeZoneInfo t = TimeZoneInfo.FindSystemTimeZoneById(id);
            return new VirtualMachineTimeZoneList(t.Id, t.DisplayName, t.StandardName);
        }

        public static bool IsTimeZoneExist(string id)
        {
            bool exist = false;
            try
            {
                TimeZoneInfo.FindSystemTimeZoneById(id);
                exist = true;
            }
            catch { }
            return exist;
        }

        public static Dictionary<string, string> GetDictionary()
        {
            return TimeZoneInfo.GetSystemTimeZones().Select(x => new { x.Id, x.DisplayName }).ToDictionary(y => y.Id, y => y.DisplayName);
        }

        public static List<KeyValuePair<string, string>> GetList()
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(TimeZoneInfo.GetSystemTimeZones().Select(x => new { x.Id, x.DisplayName }).ToDictionary(y => y.Id, y => y.DisplayName));
            list.Insert(0, new KeyValuePair<string, string>("", "Do not use this Time Zones"));
            return list;
        }
        //public static readonly List<VirtualMachineTimeZoneList> List = new List<VirtualMachineTimeZoneList>()
        //    {
        //        new VirtualMachineTimeZoneList(000, "Dateline Standard Time", "(GMT-12:00) International Date Line West"),
        //        new VirtualMachineTimeZoneList(001, "Samoa Standard Time", "(GMT-11:00) Midway Island, Samoa"),
        //        new VirtualMachineTimeZoneList(002, "Hawaiian Standard Time", "(GMT-10:00) Hawaii"),
        //        new VirtualMachineTimeZoneList(003, "Alaskan Standard Time", "(GMT-09:00) Alaska"),
        //        new VirtualMachineTimeZoneList(004, "Pacific Standard Time", "(GMT-08:00) Pacific Time (US and Canada); Tijuana"),
        //        new VirtualMachineTimeZoneList(010, "Mountain Standard Time", "(GMT-07:00) Mountain Time (US and Canada)"),
        //        new VirtualMachineTimeZoneList(013, "Mexico Standard Time 2", "(GMT-07:00) Chihuahua, La Paz, Mazatlan"),
        //        new VirtualMachineTimeZoneList(015, "U.S. Mountain Standard Time", "(GMT-07:00) Arizona"),
        //        new VirtualMachineTimeZoneList(020, "Central Standard Time", "(GMT-06:00) Central Time (US and Canada"),
        //        new VirtualMachineTimeZoneList(025, "Canada Central Standard Time", "(GMT-06:00) Saskatchewan"),
        //        new VirtualMachineTimeZoneList(030, "Mexico Standard Time", "(GMT-06:00) Guadalajara, Mexico City, Monterrey"),
        //        new VirtualMachineTimeZoneList(033, "Central America Standard Time", "(GMT-06:00) Central America"),
        //        new VirtualMachineTimeZoneList(035, "Eastern Standard Time", "(GMT-05:00) Eastern Time (US and Canada)"),
        //        new VirtualMachineTimeZoneList(040, "U.S. Eastern Standard Time", "(GMT-05:00) Indiana (East)"),
        //        new VirtualMachineTimeZoneList(045, "S.A. Pacific Standard Time", "(GMT-05:00) Bogota, Lima, Quito"),
        //        new VirtualMachineTimeZoneList(050, "Atlantic Standard Time", "(GMT-04:00) Atlantic Time (Canada)"),
        //        new VirtualMachineTimeZoneList(055, "S.A. Western Standard Time", "(GMT-04:00) Caracas, La Paz"),
        //        new VirtualMachineTimeZoneList(056, "Pacific S.A. Standard Time", "(GMT-04:00) Santiago"),
        //        new VirtualMachineTimeZoneList(060, "Newfoundland and Labrador Standard Time", "(GMT-03:30) Newfoundland and Labrador"),
        //        new VirtualMachineTimeZoneList(065, "E. South America Standard Time", "(GMT-03:00) Brasilia"),
        //        new VirtualMachineTimeZoneList(070, "S.A. Eastern Standard Time", "(GMT-03:00) Buenos Aires, Georgetown"),
        //        new VirtualMachineTimeZoneList(073, "Greenland Standard Time", "(GMT-03:00) Greenland"),
        //        new VirtualMachineTimeZoneList(075, "Mid-Atlantic Standard Time", "(GMT-02:00) Mid-Atlantic"),
        //        new VirtualMachineTimeZoneList(080, "Azores Standard Time", "(GMT-01:00) Azores"),
        //        new VirtualMachineTimeZoneList(083, "Cape Verde Standard Time", "(GMT-01:00) Cape Verde Islands"),
        //        new VirtualMachineTimeZoneList(085, "GMT Standard Time", "(GMT) Greenwich Mean Time: Dublin, Edinburgh, Lisbon, London"),
        //        new VirtualMachineTimeZoneList(090, "Greenwich Standard Time", "(GMT) Casablanca, Monrovia"),
        //        new VirtualMachineTimeZoneList(095, "Central Europe Standard Time", "(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague"),
        //        new VirtualMachineTimeZoneList(100, "Central European Standard Time", "(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb"),
        //        new VirtualMachineTimeZoneList(105, "Romance Standard Time", "(GMT+01:00) Brussels, Copenhagen, Madrid, Paris"),
        //        new VirtualMachineTimeZoneList(110, "W. Europe Standard Time", "(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna"),
        //        new VirtualMachineTimeZoneList(113, "W. Central Africa Standard Time", "(GMT+01:00) West Central Africa"),
        //        new VirtualMachineTimeZoneList(115, "E. Europe Standard Time", "(GMT+02:00) Bucharest"),
        //        new VirtualMachineTimeZoneList(120, "Egypt Standard Time", "(GMT+02:00) Cairo"),
        //        new VirtualMachineTimeZoneList(125, "FLE Standard Time", "(GMT+02:00) Helsinki, Kiev, Riga, Sofia, Tallinn, Vilnius"),
        //        new VirtualMachineTimeZoneList(130, "GTB Standard Time", "(GMT+02:00) Athens, Istanbul, Minsk"),
        //        new VirtualMachineTimeZoneList(135, "Israel Standard Time", "(GMT+02:00) Jerusalem"),
        //        new VirtualMachineTimeZoneList(140, "South Africa Standard Time", "(GMT+02:00) Harare, Pretoria"),
        //        new VirtualMachineTimeZoneList(145, "Russian Standard Time", "(GMT+03:00) Moscow, St. Petersburg, Volgograd"),
        //        new VirtualMachineTimeZoneList(150, "Arab Standard Time", "(GMT+03:00) Kuwait, Riyadh"),
        //        new VirtualMachineTimeZoneList(155, "E. Africa Standard Time", "(GMT+03:00) Nairobi"),
        //        new VirtualMachineTimeZoneList(158, "Arabic Standard Time", "(GMT+03:00) Baghdad"),
        //        new VirtualMachineTimeZoneList(160, "Iran Standard Time", "(GMT+03:30) Tehran"),
        //        new VirtualMachineTimeZoneList(165, "Arabian Standard Time", "(GMT+04:00) Abu Dhabi, Muscat"),
        //        new VirtualMachineTimeZoneList(170, "Caucasus Standard Time", "(GMT+04:00) Baku, Tbilisi, Yerevan"),
        //        new VirtualMachineTimeZoneList(175, "Transitional Islamic State of Afghanistan Standard Time", "(GMT+04:30) Kabul"),
        //        new VirtualMachineTimeZoneList(180, "Ekaterinburg Standard Time", "(GMT+05:00) Ekaterinburg"),
        //        new VirtualMachineTimeZoneList(185, "West Asia Standard Time", "(GMT+05:00) Islamabad, Karachi, Tashkent"),
        //        new VirtualMachineTimeZoneList(190, "India Standard Time", "(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi"),
        //        new VirtualMachineTimeZoneList(193, "Nepal Standard Time", "(GMT+05:45) Kathmandu"),
        //        new VirtualMachineTimeZoneList(195, "Central Asia Standard Time", "(GMT+06:00) Astana, Dhaka"),
        //        new VirtualMachineTimeZoneList(200, "Sri Lanka Standard Time", "(GMT+06:00) Sri Jayawardenepura"),
        //        new VirtualMachineTimeZoneList(201, "N. Central Asia Standard Time", "(GMT+06:00) Almaty, Novosibirsk"),
        //        new VirtualMachineTimeZoneList(203, "Myanmar Standard Time", "(GMT+06:30) Yangon Rangoon"),
        //        new VirtualMachineTimeZoneList(205, "S.E. Asia Standard Time", "(GMT+07:00) Bangkok, Hanoi, Jakarta"),
        //        new VirtualMachineTimeZoneList(207, "North Asia Standard Time", "(GMT+07:00) Krasnoyarsk"),
        //        new VirtualMachineTimeZoneList(210, "China Standard Time", "(GMT+08:00) Beijing, Chongqing, Hong Kong SAR, Urumqi"),
        //        new VirtualMachineTimeZoneList(215, "Singapore Standard Time", "(GMT+08:00) Kuala Lumpur, Singapore"),
        //        new VirtualMachineTimeZoneList(220, "Taipei Standard Time", "(GMT+08:00) Taipei"),
        //        new VirtualMachineTimeZoneList(225, "W. Australia Standard Time", "(GMT+08:00) Perth"),
        //        new VirtualMachineTimeZoneList(227, "North Asia East Standard Time", "(GMT+08:00) Irkutsk, Ulaanbaatar"),
        //        new VirtualMachineTimeZoneList(230, "Korea Standard Time", "(GMT+09:00) Seoul"),
        //        new VirtualMachineTimeZoneList(235, "Tokyo Standard Time", "(GMT+09:00) Osaka, Sapporo, Tokyo"),
        //        new VirtualMachineTimeZoneList(240, "Yakutsk Standard Time", "(GMT+09:00) Yakutsk"),
        //        new VirtualMachineTimeZoneList(245, "A.U.S. Central Standard Time", "(GMT+09:30) Darwin"),
        //        new VirtualMachineTimeZoneList(250, "Cen. Australia Standard Time", "(GMT+09:30) Adelaide"),
        //        new VirtualMachineTimeZoneList(255, "A.U.S. Eastern Standard Time", "(GMT+10:00) Canberra, Melbourne, Sydney"),
        //        new VirtualMachineTimeZoneList(260, "E. Australia Standard Time", "(GMT+10:00) Brisbane"),
        //        new VirtualMachineTimeZoneList(265, "Tasmania Standard Time", "(GMT+10:00) Hobart"),
        //        new VirtualMachineTimeZoneList(270, "Vladivostok Standard Time", "(GMT+10:00) Vladivostok"),
        //        new VirtualMachineTimeZoneList(275, "West Pacific Standard Time", "(GMT+10:00) Guam, Port Moresby"),
        //        new VirtualMachineTimeZoneList(280, "Central Pacific Standard Time", "(GMT+11:00) Magadan, Solomon Islands, New Caledonia"),
        //        new VirtualMachineTimeZoneList(285, "Fiji Islands Standard Time", "(GMT+12:00) Fiji Islands, Kamchatka, Marshall Islands"),
        //        new VirtualMachineTimeZoneList(290, "New Zealand Standard Time", "(GMT+12:00) Auckland, Wellington"),
        //        new VirtualMachineTimeZoneList(300, "Tonga Standard Time", "(GMT+13:00) Nuku'alofa")
        //    };
    }
}
