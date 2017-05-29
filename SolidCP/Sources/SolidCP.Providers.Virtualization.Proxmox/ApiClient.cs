using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using RestSharp;
using System.Threading;
using SolidCP.Providers.Virtualization.Proxmox;

namespace SolidCP.Providers.Virtualization
{
    public class ApiClient
    {
        private string baseUrl;
        //private string node;
        private ApiTicket apiTicket;

        private const string TaskOk = "TASK OK";
        private const string RequestRootElement = "data";

        //public ApiClient(ProxmoxServer server, string node)
        public ApiClient(ProxmoxServer server)
        {
            this.baseUrl = "https://" + server.Ip + ":" + server.Port + "/api2/json/";
            //this.node = node;
        }

        public IRestResponse<ApiTicket> Login(User user)
        {
            var restClient = new RestClient(baseUrl);
            var request = new RestRequest("access/ticket", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("username", user.Username);
            request.AddParameter("password", user.Password);
            request.AddParameter("realm", user.Realm);
            request.RootElement = RequestRootElement;
            var response = restClient.Execute<ApiTicket>(request);
            apiTicket = response.Data;
            return response;
        }


        public IRestResponse<VMStatusInfo> Status(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/status/current", nodeid(vmId).node, nodeid(vmId).id));
            return client.Execute<VMStatusInfo>(request);
        }

        public IRestResponse<Upid> Start(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/start", nodeid(vmId).node, nodeid(vmId).id), "");
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> Stop(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/stop", nodeid(vmId).node, nodeid(vmId).id), "");
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> Shutdown(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/shutdown", nodeid(vmId).node, nodeid(vmId).id), "");
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> Reset(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/reset", nodeid(vmId).node, nodeid(vmId).id), "");
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> Suspend(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/suspend", nodeid(vmId).node, nodeid(vmId).id), "");
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> Resume(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/resume", nodeid(vmId).node, nodeid(vmId).id), "");
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> ChangeName(string vmId, string newhostname)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = "root";
            request.AddParameter("name", newhostname);
            return client.Execute<Upid>(request);
        }


        public IRestResponse<NodeStatus> NodeStatus(string node)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/status", node));
            return client.Execute<NodeStatus>(request);
        }


        public IRestResponse<VMConfig> VMConfig(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id));
            return client.Execute<VMConfig>(request);
        }

        public IRestResponse<Upid> Delete(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareDeleteRequest(string.Format("nodes/{0}/qemu/{1}", nodeid(vmId).node, nodeid(vmId).id), "");
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> Unlink(string vmId, string device)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/unlink", nodeid(vmId).node, nodeid(vmId).id), Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = "root";
            request.AddParameter("idlist", device);
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> CreateSnapshot(string vmId, string name, string description)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/snapshot", nodeid(vmId).node, nodeid(vmId).id), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = "root";
            request.AddParameter("snapname", name);
            request.AddParameter("description", description);
            return client.Execute<Upid>(request);
        }

        public IRestResponse<ListProxmoxSnapshots> ListSnapshots(string vmId)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/snapshot", nodeid(vmId).node, nodeid(vmId).id));
            return client.Execute<ListProxmoxSnapshots>(request);
        }
        
        public IRestResponse<SnapshotConfig> GetSnapshot(string vmId, string snapshotid)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}/config", nodeid(vmId).node, nodeid(vmId).id, snapshotid));
            return client.Execute<SnapshotConfig>(request);
        }

        public IRestResponse<Upid> RenameSnapshot(string vmId, string snapshotid, string description)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}/config", nodeid(vmId).node, nodeid(vmId).id, snapshotid), Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = "root";
            request.AddParameter("description", description);
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> DeleteSnapshot(string vmId, string snapshotid)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareDeleteRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}", nodeid(vmId).node, nodeid(vmId).id, snapshotid));
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> rollback(string vmId, string snapshotid)
        {
            var client = new RestClient(baseUrl);
            var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}/rollback", nodeid(vmId).node, nodeid(vmId).id, snapshotid), "");
            return client.Execute<Upid>(request);
        }


        public IRestResponse ListISOs(string vmId, string storage)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/storage/{1}/content", nodeid(vmId).node, storage));
            return client.Execute(request);
        }


        public IRestResponse<ProxmoxTaskStatus> TaskStatus(string upid)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/tasks/{1}/status", upid2node(upid), upid));
            return client.Execute<ProxmoxTaskStatus>(request);
        }

        public IRestResponse<List<TaskLog>> TaskLog(string upid)
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/tasks/{1}/log", upid2node(upid), upid));
            return client.Execute<List<TaskLog>>(request);
        }

        public bool TaskHasFinished(string upid, int seconds = 30)
        {
            var oneSecond = 1000;
            for (var i = 0; i < seconds * oneSecond;)
            {
                var logs = TaskLog(upid).Data;
                foreach (var log in logs)
                {
                    if (log.t == TaskOk)
                    {
                        return true;
                    }
                }
                i += oneSecond;
                Thread.Sleep(oneSecond);
            }
            return false;
        }

        /*
        public IRestResponse<List<TaskStatus>> TaskStatusList()
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest(string.Format("nodes/{0}/tasks/", node));
            return client.Execute<List<TaskStatus>>(request);
        }
        */

        public IRestResponse ClusterResources()
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest("cluster/resources");
            return client.Execute(request);
        }

        public IRestResponse ClusterVMList()
        {
            var client = new RestClient(baseUrl);
            var request = PrepareGetRequest("cluster/resources?type=vm");
            return client.Execute(request);
        }

        public IRestResponse<Upid> UpdateConfig(String vmId, UpdateConfiguration template)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = "root";
            request.AddParameter("cores", template.cores);
            request.AddParameter("memory", template.memory);
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> UpdateDVD(String vmId, UpdateDVD template)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id), Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = "root";
            request.AddParameter("ide2", template.ide2);
            return client.Execute<Upid>(request);
        }

        public IRestResponse<Upid> ResizeDisk(string vmId, string disk, string size)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/resize", nodeid(vmId).node, nodeid(vmId).id), Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = "root";
            request.AddParameter("disk", disk);
            request.AddParameter("size", size);
            return client.Execute<Upid>(request);
        }


        private RestRequest PreparePostRequest(string resource, string rootElement = RequestRootElement)
        {
            var request = new RestRequest(resource, Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = rootElement;
            return request;
        }

        private RestRequest PrepareGetRequest(string resource, string rootElement = RequestRootElement)
        {
            var request = new RestRequest(resource, Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = rootElement;
            return request;
        }

        private RestRequest PrepareDeleteRequest(string resource, string rootElement = RequestRootElement)
        {
            var request = new RestRequest(resource, Method.DELETE);
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
            request.AddCookie("PVEAuthCookie", apiTicket.ticket);
            request.RootElement = rootElement;
            return request;
        }

        private string upid2node(string upid)
        {
            string node = upid.Split(':')[1];
            return node;
        }

        private ApiVM nodeid(string vmId)
        {
            ApiVM apivm = new ApiVM();
            apivm.node = vmId.Split(':')[0];
            apivm.id = vmId.Split(':')[1];

            var RestResponse = ClusterVMList();
            JsonObject jsonResponse = (JsonObject)SimpleJson.DeserializeObject(RestResponse.Content);
            JsonArray jsonResponsearray = (JsonArray)SimpleJson.DeserializeObject(jsonResponse["data"].ToString());

            foreach (Object vmObject in jsonResponsearray)
            {
                JsonObject resources = (JsonObject)SimpleJson.DeserializeObject(vmObject.ToString());
                try
                {
                    if (resources["type"].ToString().Equals("qemu") && resources["vmid"].ToString().Equals(apivm.id))
                    {
                        apivm.node = resources["node"].ToString();
                        return apivm;
                    }
                }
                #pragma warning disable 0168
                catch (Exception ex)
                #pragma warning restore 0168
                {
                    apivm.node = vmId.Split(':')[0];
                }

            }
            return apivm;
        }

    }

}
