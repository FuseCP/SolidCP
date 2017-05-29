using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace SolidCP.Setup.Internal
{
    public static class XmlDocumentMerge
    {
        public class AttrTag
        {
            public AttrTag()
                : this(null)
            {

            }
            public AttrTag(params string[] Attributes)
            {
                this.Attributes = Attributes;
                Priority = Attributes == null ? 0 : (ulong)Attributes.LongLength;
            }
            public ulong Priority { get; private set; }
            public string[] Attributes;
        }
        public class FrozenAttrTag
        {
            public FrozenAttrTag()
                : this(false)
            {

            }
            public FrozenAttrTag(bool Relative)
            {
                IsRelative = Relative;
            }
            public string Path { get; set; }
            public IList<string> Attributes { get; set; }
            public bool IsRelative { get; private set; }
        }
        const string SuccessFormat = "Success: {0}.";
        const string ErrorFormat = "Error: {0}.";
        const string MergeCompleted = "XmlDocumentMerge completed";
        static XmlDocumentMerge()
        {
            KeyAttributes = new List<string>();
            FrozenAttributes = new List<FrozenAttrTag>();
        }
        public static IList<string> KeyAttributes { get; set; }
        public static IList<FrozenAttrTag> FrozenAttributes { get; set; }
        public static string Process(string Src, string Dst, string SaveTo = "")
        {
            var Result = string.Empty;
            if (!File.Exists(Src))
                Result = string.Format(ErrorFormat, string.Format("source document [{0}] does not exists", Src));
            else if (!File.Exists(Dst))
                Result = string.Format(ErrorFormat, string.Format("destination document [{0}] does not exists", Dst));
            else
            {
                try
                {
                    var InStream = new FileStream(Src, FileMode.Open, FileAccess.Read);
                    var OutStream = new FileStream(Dst, FileMode.Open, FileAccess.ReadWrite);
                    Result = Process(InStream,
                                     OutStream,
                                     SaveTo);
                    InStream.Close();
                    OutStream.Flush();
                    OutStream.Close();
                }
                catch (Exception ex)
                {
                    Result = string.Format(ErrorFormat, ex.ToString());
                }
            }
            return Result;
        }
        public static string Process(Stream InSrc, Stream OutDst, string SaveTo = "")
        {
            var Result = string.Format(SuccessFormat, MergeCompleted);
            try
            {
                var SrcDoc = new XmlDocument();
                SrcDoc.Load(InSrc);
                var DstDoc = new XmlDocument();
                DstDoc.Load(OutDst);
                var DstNavi = DstDoc.CreateNavigator();
                var DstIterator = DstNavi.SelectChildren(XPathNodeType.All);
                while (DstIterator.MoveNext())
                    Merge(DstIterator.Current.Clone(), SrcDoc, string.Empty);
                if (string.IsNullOrWhiteSpace(SaveTo))
                {
                    OutDst.SetLength(0);
                    DstDoc.Save(OutDst);
                }
                else
                    DstDoc.Save(SaveTo);
            }
            catch (Exception ex)
            {
                Result = string.Format(ErrorFormat, ex.ToString());
            }
            return Result;
        }
        private static string NodePath(string Parent, string Current)
        {
            var Result = string.Empty;
            if (!string.IsNullOrWhiteSpace(Parent) && !string.IsNullOrWhiteSpace(Current))
                Result = string.Format("{0}/{1}", Parent, Current);
            else if (!string.IsNullOrWhiteSpace(Parent))
                Result = Parent;
            else if (!string.IsNullOrWhiteSpace(Current))
                Result = Current;
            return Result;
        }
        private static string NodeView(XPathNavigator Navi)
        {
            foreach (var Item in GetProcessingChain(KeyAttributes))
            {
                string Result = string.Empty;
                foreach (var Attr in Item.Attributes)
                {
                    var Value = Navi.GetAttribute(Attr, string.Empty);
                    if (string.IsNullOrWhiteSpace(Value))
                    {
                        Result = string.Empty;
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(Result))
                            Result = string.Format("@{0}='{1}'", Attr, Value);
                        else
                            Result = string.Format("{0} and @{1}='{2}'", Result, Attr, Value);
                    }
                }
                if (!string.IsNullOrWhiteSpace(Result))
                    return string.Format("{0}[{1}]", Navi.Name, Result);
            }
            return Navi.Name;
        }
        private static void Merge(XPathNavigator DstNavi, XmlDocument SrcDoc, string Parent)
        {
            var Current = NodePath(Parent, NodeView(DstNavi));
            if (!string.IsNullOrWhiteSpace(Current))
            {
                if (DstNavi.NodeType == XPathNodeType.Element)
                {
                    var SrcElem = SrcDoc.SelectSingleNode(Current);
                    if (SrcElem != null)
                    {
                        var Frozen = GetFrozenAttributes(Current, FrozenAttributes);
                        if (DstNavi.MoveToFirstAttribute())
                        {
                            do
                            {
                                var SrcElemAttr = SrcElem.Attributes[DstNavi.LocalName];
                                if (SrcElemAttr != null && CanProcess(DstNavi.LocalName, Frozen))
                                    DstNavi.SetValue(SrcElemAttr.Value);
                            }
                            while (DstNavi.MoveToNextAttribute());
                            DstNavi.MoveToParent();
                        }
                    }
                }
                else if (DstNavi.NodeType == XPathNodeType.Text)
                {
                    var SrcElem = SrcDoc.SelectSingleNode(Current);
                    if (SrcElem != null)
                        DstNavi.SetValue(SrcElem.InnerText);
                }
                if (DstNavi.MoveToFirstChild())
                {
                    do
                    {
                        Merge(DstNavi, SrcDoc, Current);
                    }
                    while (DstNavi.MoveToNext());
                    DstNavi.MoveToParent();
                }
                else if (DstNavi.NodeType == XPathNodeType.Element)
                {
                    var SrcElem = SrcDoc.SelectSingleNode(Current);
                    if (SrcElem != null && !string.IsNullOrWhiteSpace(SrcElem.InnerXml))
                        foreach (XmlNode Child in SrcElem.ChildNodes)
                            DstNavi.AppendChild(Child.CloneNode(true).CreateNavigator());
                }
            }
        }
        private static IList<AttrTag> GetProcessingChain(IEnumerable<string> Attributes)
        {
            var Delimiter = ";";
            var Chain = new List<AttrTag>();
            foreach (var Attribute in Attributes)
                Chain.Add(new AttrTag(Attribute.Split(new string[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries)));
            Chain.Sort(delegate(AttrTag a, AttrTag b) { return a.Priority == b.Priority ? 0 : a.Priority > b.Priority ? -1 : 1; });
            return Chain;
        }
        private static FrozenAttrTag GetFrozenAttributes(string Path, IEnumerable<FrozenAttrTag> Frozens)
        {
            foreach (var Frozen in Frozens)
                if (Frozen.IsRelative ? Path.IndexOf(Frozen.Path, 1) == -1 : Path.StartsWith(Frozen.Path))
                    return Frozen;
            return null;
        }
        private static bool CanProcess(string Name, FrozenAttrTag Frozen)
        {
            return Frozen == null ? true : !Frozen.Attributes.Contains(Name);
        }
    }
}
