using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;

namespace SolidCP.Portal {
    public partial class AutoUpdateServers : SolidCPModuleBase {
        DataSet dsServers = null;
        string downloadLink = "";

        protected void Page_Load(object sender, EventArgs e) {
            try {
                BindVersions();
                BindServers();
            } catch(Exception ex) {
                ProcessException(ex);
                this.DisableControls = true;
                return;
            }
        }

        private void BindServers() {
            dsServers = ES.Services.Servers.GetRawServers();
            if(!IsPostBack) {
                dlServers.DataSource = dsServers;
                dlServers.DataBind();
            }
            tblEmptyList.Visible = (dlServers.Items.Count == 0);
        }

        private void BindVersions() {
            XmlDocument doc = new XmlDocument();
            doc.Load("http://autoupdate.solidcp.com/version.xml");
            downloadLink = doc.GetElementsByTagName("downloadURL")[0].InnerText;
            XmlNodeList versions = doc.SelectNodes("root/versions/version");
            ddlSelectVersion.Items.Clear();
            foreach(XmlNode n in versions) {
                ddlSelectVersion.Items.Add(
                    new ListItem {
                        Value = n.SelectSingleNode("file").InnerText,
                        Text = n.SelectSingleNode("version-nr").InnerText
                    }
                );
            }
        }

        public int GetServiceIDFromDataView(int serverId) {
            DataView v = new DataView(dsServers.Tables[1], "ServerID=" + serverId.ToString(), "", DataViewRowState.CurrentRows);
            foreach(DataRow r in v.Table.Columns["ServerID"].Table.Rows) {
                if((int)r.ItemArray[1] == serverId) {
                    return (int)r.ItemArray[0];
                }
            }
            return 0;
        }

        public string getServerName(int serverId) {
            return ES.Services.Servers.GetServerById(serverId).ServerName;
        }

        protected void btnUpdateServers_Click(object sender, EventArgs e) {
            int[][] servers = new int[dlServers.Items.Count][];
            Dictionary<int, string> result = new Dictionary<int, string>();
            int i = 0; int s = 0;
            foreach(DataListItem item in dlServers.Items) {
                CheckBox chkServer = ((CheckBox)(item.FindControl("chkServer")));
                if(chkServer.Checked) {
                    int serverID = int.Parse(chkServer.Attributes["Value"]);
                    int serviceID = GetServiceIDFromDataView(serverID);

                    if(serviceID > 0) { 
                        int[] ServerService;

                        ServerService = new int[] {
                            serverID, serviceID
                        };

                        servers[i++] = ServerService;
                    } else {
                        result.Add(serverID, "No services");
                    }
                }
                s++;
            }

            string[] response = ES.Services.Servers.AutoUpdateServer(servers, downloadLink, ddlSelectVersion.SelectedValue);
            foreach(string x in response) {
                string[] parts = x.Split('-');
                result.Add(int.Parse(parts[0]), parts[1]);
            }


            if(result.Count == 0) {
                ShowSuccessMessage("SERVERS_UPDATED");
            } else {
                if(result.Count == dlServers.Items.Count) {
                    ShowErrorMessage("SERVERS_UPDATED");
                } else {
                    ShowWarningMessage("SERVERS_UPDATED");
                }
                lstFailed.DataSource = result;
                lstFailed.DataBind();
                failedList.Visible = true;
            }

        }
    }
}