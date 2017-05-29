using System;
using Microsoft.Deployment.WindowsInstaller;

namespace SolidCP.WIXInstaller.Common.Util
{
    internal interface IListCtrl
    {        
        ulong Count { get; }
        string Id { get; }
        void AddItem(Record Item);
    }

    internal abstract class ListCtrlBase : IListCtrl
    {
        private Session m_Ctx;
        private string m_CtrlType;
        private string m_CtrlId;        
        private View m_View;
        private ulong m_Count;

        public ListCtrlBase(Session session, string CtrlType, string CtrlId)
        {
            m_Ctx = session;
            m_CtrlType = CtrlType;
            m_CtrlId = CtrlId;
            m_View = null;
            m_Count = 0;
            Initialize();
        }

        ~ListCtrlBase()
        {
            if (m_View != null)
                m_View.Close();
        }

        public virtual ulong Count { get { return m_Count; } }

        public virtual string Id { get { return m_CtrlId; } }

        public virtual void AddItem(Record Item)
        {
            m_View.Execute(Item);
            ++m_Count;
        }

        private void Initialize()
        {
            m_Ctx.Database.Execute(string.Format("DELETE FROM `{0}` WHERE `Property`='{1}'", m_CtrlType, m_CtrlId)); 
            m_View = m_Ctx.Database.OpenView(m_Ctx.Database.Tables[m_CtrlType].SqlInsertString + " TEMPORARY");
        }        
    }

    class ListViewCtrl : ListCtrlBase
    {
        public ListViewCtrl(Session session, string WiXListID) : base(session, "ListView", WiXListID)
        {
            
        } 
       
        public void AddItem(bool Checked, string Value)
        {
            AddItem(new Record(new object[] { Id, Count, Value, Value, Checked ? "passmark" : "failmark" }));
        }
    }

    class ComboBoxCtrl : ListCtrlBase
    {
        public ComboBoxCtrl(Session session, string WiXComboID): base(session, "ComboBox", WiXComboID)
        {

        }

        public void AddItem(string Value)
        {
            AddItem(new Record(new object[] { Id, Count, Value, Value }));
        }
    }
}
