using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using RestSharp;
using System.Threading;
using SolidCP.Providers.Virtualization.Proxmox;
using SolidCP.Providers.HostedSolution;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SolidCP.Providers.Virtualization
{
	public class ApiClient
	{
		private string baseUrl;
		private bool validateCertificate;
		//private string node;
		private ApiTicket apiTicket;

		private const string TaskOk = "TASK OK";
		private const string RequestRootElement = "data";

		//public ApiClient(ProxmoxServer server, string node)
		public ApiClient(ProxmoxServer server)
		{
			this.baseUrl = "https://" + server.Ip + ":" + server.Port + "/api2/json";
			this.validateCertificate = server.ValidateCertificate;
			//this.node = node;
			//HostedSolutionLog.DebugInfo("APIClient: {0}", this.baseUrl);
		}

		RestClient GetRestClient(int timeout = 0)
		{
			var options = new RestClientOptions(baseUrl)
			{
				RemoteCertificateValidationCallback = validateCertificate ? null :
				(sender, certificate, chain, sslPolicyErrors) =>
					true
			};
			if (timeout == -1) options.MaxTimeout = int.MaxValue;
			else if (timeout != 0) options.MaxTimeout = timeout;
			return new RestClient(options);
		}
		public RestResponse Login(User user)
		{
			//HostedSolutionLog.DebugInfo("Login"); 
			var restClient = GetRestClient();
			//HostedSolutionLog.DebugInfo("Login - baseUrl: {0}", baseUrl);
			var request = new RestRequest("access/ticket", Method.Post);
			//request.AddHeader("content-type", "application/json");
			request.RequestFormat = DataFormat.Json;
			request.AddParameter("username", user.Username);
			request.AddParameter("password", user.Password);
			request.AddParameter("realm", user.Realm);
			//request.RootElement = RequestRootElement;
			//HostedSolutionLog.DebugInfo("Login - request: {0}", request.ToString());
			var response = restClient.Execute(request);
			if (response == null || response.Content == null || response.StatusCode == HttpStatusCode.ServiceUnavailable) throw new Exception($"Proxmox Server API Service at {baseUrl} unavaliable.\n{response.ErrorMessage}\n{response.ErrorException}");
			//HostedSolutionLog.DebugInfo("Login - response Content: {0}", response.Content.ToString());
			JObject json = JObject.Parse(response.Content);

			var data = json["data"];
			ApiTicket apiTicketdata = new ApiTicket();

			apiTicketdata.ticket = (string)data["ticket"];
			apiTicketdata.username = (string)data["username"];
			apiTicketdata.CSRFPreventionToken = (string)data["CSRFPreventionToken"];
			apiTicket = apiTicketdata;
			//HostedSolutionLog.DebugInfo("Login - apiTicket: {0}", apiTicket.ticket);
			return response;
		}


		public dynamic Status(string vmId)
		{
			var client = GetRestClient();
			var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/status/current", nodeid(vmId).node, nodeid(vmId).id));
			var response = client.Execute<VMStatusInfo>(request);
			//HostedSolutionLog.DebugInfo("Status - response Content: {0}", response.Content.ToString());
			dynamic json = JObject.Parse(response.Content);
			return json;
		}

		public RestResponse<Upid> Start(string vmId)
		{
			var client = GetRestClient();
			var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/start", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> Stop(string vmId)
		{
			var client = GetRestClient();
			var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/stop", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> Shutdown(string vmId)
		{
			var client = GetRestClient();
			var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/shutdown", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> Reset(string vmId)
		{
			var client = GetRestClient();
			var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/reset", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> Suspend(string vmId)
		{
			var client = GetRestClient();
			var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/suspend", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> Resume(string vmId)
		{
			var client = GetRestClient();
			var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/status/resume", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> ChangeName(string vmId, string newhostname)
		{
			var client = GetRestClient();
			var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id), Method.Post);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = "root";
			request.AddParameter("name", newhostname);
			return client.Execute<Upid>(request);
		}


		public RestResponse<NodeStatus> NodeStatus(string node)
		{
			var client = GetRestClient();
			var request = PrepareGetRequest(string.Format("nodes/{0}/status", node));
			return client.Execute<NodeStatus>(request);
		}


		public RestResponse<VMConfig> VMConfig(string vmId)
		{
			var client = GetRestClient();
			var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<VMConfig>(request);
		}

		public RestResponse<Upid> Delete(string vmId)
		{
			var client = GetRestClient();
			var request = PrepareDeleteRequest(string.Format("nodes/{0}/qemu/{1}", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> Unlink(string vmId, string device)
		{
			var client = GetRestClient();
			var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/unlink", nodeid(vmId).node, nodeid(vmId).id), Method.Put);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = "root";
			request.AddParameter("idlist", device);
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> CreateSnapshot(string vmId, string name, string description)
		{
			var client = GetRestClient();
			var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/snapshot", nodeid(vmId).node, nodeid(vmId).id), Method.Post);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = "root";
			request.AddParameter("snapname", name);
			request.AddParameter("description", description);
			return client.Execute<Upid>(request);
		}

		public RestResponse<ListProxmoxSnapshots> ListSnapshots(string vmId)
		{
			var client = GetRestClient();
			var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/snapshot", nodeid(vmId).node, nodeid(vmId).id));
			return client.Execute<ListProxmoxSnapshots>(request);
		}

		public RestResponse<SnapshotConfig> GetSnapshot(string vmId, string snapshotid)
		{
			var client = GetRestClient();
			var request = PrepareGetRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}/config", nodeid(vmId).node, nodeid(vmId).id, snapshotid));
			return client.Execute<SnapshotConfig>(request);
		}

		public RestResponse<Upid> RenameSnapshot(string vmId, string snapshotid, string description)
		{
			var client = GetRestClient();
			var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}/config", nodeid(vmId).node, nodeid(vmId).id, snapshotid), Method.Put);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = "root";
			request.AddParameter("description", description);
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> DeleteSnapshot(string vmId, string snapshotid)
		{
			var client = GetRestClient();
			var request = PrepareDeleteRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}", nodeid(vmId).node, nodeid(vmId).id, snapshotid));
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> rollback(string vmId, string snapshotid)
		{
			var client = GetRestClient();
			var request = PreparePostRequest(string.Format("nodes/{0}/qemu/{1}/snapshot/{2}/rollback", nodeid(vmId).node, nodeid(vmId).id, snapshotid));
			return client.Execute<Upid>(request);
		}


		public RestResponse ListISOs(string vmId, string storage)
		{
			var client = GetRestClient();
			var request = PrepareGetRequest(string.Format("nodes/{0}/storage/{1}/content", nodeid(vmId).node, storage));
			return client.Execute(request);
		}


		public RestResponse<ProxmoxTaskStatus> TaskStatus(string upid)
		{
			var client = GetRestClient();
			var request = PrepareGetRequest(string.Format("nodes/{0}/tasks/{1}/status", upid2node(upid), upid));
			return client.Execute<ProxmoxTaskStatus>(request);
		}

		public RestResponse<List<TaskLog>> TaskLog(string upid)
		{
			var client = GetRestClient();
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
		public RestResponse<List<TaskStatus>> TaskStatusList()
		{
			 var client = GetRestClient();
			 var request = PrepareGetRequest(string.Format("nodes/{0}/tasks/", node));
			 return client.Execute<List<TaskStatus>>(request);
		}
		*/

		public RestResponse ClusterResources()
		{
			var client = GetRestClient();
			var request = PrepareGetRequest("cluster/resources");
			return client.Execute(request);
		}

		public RestResponse ClusterVMList()
		{
			//HostedSolutionLog.LogStart("ClusterVMList");
			var client = GetRestClient();
			var request = PrepareGetRequest("cluster/resources?type=vm");
			//HostedSolutionLog.LogEnd("ClusterVMList");
			return client.Execute(request);
		}

		public RestResponse<Upid> UpdateConfig(String vmId, UpdateConfiguration template)
		{
			var client = GetRestClient();
			var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id), Method.Post);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = "root";
			request.AddParameter("cores", template.cores);
			request.AddParameter("memory", template.memory);
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> UpdateDVD(String vmId, UpdateDVD template)
		{
			var client = GetRestClient();
			var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/config", nodeid(vmId).node, nodeid(vmId).id), Method.Post);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = "root";
			request.AddParameter("ide2", template.ide2);
			return client.Execute<Upid>(request);
		}

		public RestResponse<Upid> ResizeDisk(string vmId, string disk, string size)
		{
			var client = GetRestClient();
			var request = new RestRequest(string.Format("nodes/{0}/qemu/{1}/resize", nodeid(vmId).node, nodeid(vmId).id), Method.Put);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = "root";
			request.AddParameter("disk", disk);
			request.AddParameter("size", size);
			return client.Execute<Upid>(request);
		}


		private RestRequest PreparePostRequest(string resource)
		{
			var request = new RestRequest(resource, Method.Post);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = rootElement;
			return request;
		}

		private RestRequest PrepareGetRequest(string resource)
		{
			//HostedSolutionLog.DebugInfo("PrepareGetRequest");
			var request = new RestRequest(resource, Method.Get);
			//HostedSolutionLog.DebugInfo("PrepareGetRequest Request: {0}", request.ToString());
			request.RequestFormat = DataFormat.Json;
			//HostedSolutionLog.DebugInfo("PrepareGetRequest - PVEAuthCookie: {0)", apiTicket.ticket); 
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = rootElement;
			//HostedSolutionLog.DebugInfo("PrepareGetRequest Request2: {0}", request);
			return request;
		}

		private RestRequest PrepareDeleteRequest(string resource)
		{
			var request = new RestRequest(resource, Method.Delete);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader("CSRFPreventionToken", apiTicket.CSRFPreventionToken);
			request.AddCookie("PVEAuthCookie", apiTicket.ticket, new Uri(baseUrl).AbsolutePath, new Uri(baseUrl).Host);
			//request.RootElement = rootElement;
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

			JToken jsonResponse = JToken.Parse(RestResponse.Content);
			JArray jsonResponsearray = (JArray)jsonResponse["data"];

			foreach (JObject resources in jsonResponsearray)
			{
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
