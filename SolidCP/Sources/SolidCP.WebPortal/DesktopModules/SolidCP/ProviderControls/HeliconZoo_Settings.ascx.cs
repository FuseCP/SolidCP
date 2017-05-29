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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using SolidCP.Portal;
using SolidCP.Providers.HeliconZoo;
using SolidCP.Server;


public partial class HeliconZoo_Settings : SolidCPControlBase, IHostingServiceProviderSettings
{
    private class EnvBoxPair
    {

        public TextBox Name;
        public TextBox Value;
        public EnvBoxPair(TextBox name, TextBox value)
        {
            Name = name;
            Value = value;
        }
    }

    private EnvBoxPair[] _envBoxsPair;

    protected void Page_Load(object sender, EventArgs e)
    {

        _envBoxsPair = new EnvBoxPair[]
        {
            new EnvBoxPair(EnvKey1, EnvValue1),
            new EnvBoxPair(EnvKey2, EnvValue2),
            new EnvBoxPair(EnvKey3, EnvValue3),
            new EnvBoxPair(EnvKey4, EnvValue4),
            new EnvBoxPair(EnvKey5, EnvValue5),
            new EnvBoxPair(EnvKey6, EnvValue6),
            new EnvBoxPair(EnvKey7, EnvValue7),
            new EnvBoxPair(EnvKey8, EnvValue8),
        };

        if (!IsPostBack)
        {
            BindHostingPackages();

            EngineTransport.Items.Clear();
            EngineTransport.Items.AddRange(
                new ListItem[]
                {
                    new ListItem("Named pipe", "namedpipe"), 
                    new ListItem("TCP", "tcp"), 
                }
            );
            EngineProtocol.Items.Clear();
            EngineProtocol.Items.AddRange(
                new ListItem[]
                {
                    new ListItem("FastCGI", "fastcgi"), 
                    new ListItem("HTTP", "http"), 
                }
            );

            BindEngines();
        }
    }

    private void BindHostingPackages()
    {

        WPIProduct[] products = null;
        try
        {
            products = GetHostingPackages();
        }
        catch(Exception e)
        {
            HostingPackagesGrid.Visible = false;
            HostingPackagesInstallButtonsPanel.Visible = false;
            HostingPackagesErrorsPanel.Visible = true;
            if (e.InnerException != null)
            {
                e = e.InnerException;
            }
            HostingPackagesLoadingError.Text = e.Message;
        }
        HostingPackagesGrid.DataSource = products;
        HostingPackagesGrid.DataBind();
    }


    private void BindEngines()
    {
        WPIProduct zooModule =  ES.Services.Servers.GetWPIProductById(PanelRequest.ServerId, "HeliconZooModule");
        if (!zooModule.IsInstalled || zooModule.IsUpgrade)
        {
            HostModule.ShowWarningMessage("Zoo Module is not installed or out-of-date. To proceed press 'Add' or 'Update' next to Helicon Zoo Module below, then press 'Install'.");
        }

        // get all engines from IIS
        HeliconZooEngine[] engineList = ES.Services.HeliconZoo.GetEngines(PanelRequest.ServiceId);

        if (null != engineList && engineList.Length > 0)
        {
            // convert list to dict
            Dictionary<string, HeliconZooEngine> enginesDict = new Dictionary<string, HeliconZooEngine>();
            foreach (HeliconZooEngine engine in engineList)
            {
                enginesDict[engine.name] = engine;
            }

            // save engines in view state
            ViewState["HeliconZooEngines"] = enginesDict;

            // bind to grid
            EngineGrid.DataSource = engineList;
            EngineGrid.DataBind();

            // bind 'Enable quotas' checkbox
            bool enabled = ES.Services.HeliconZoo.IsEnginesEnabled(PanelRequest.ServiceId);
            QuotasEnabled.Checked = !enabled;

            WebCosoleEnabled.Checked = ES.Services.HeliconZoo.IsWebCosoleEnabled(PanelRequest.ServiceId);
        }
        else
        {
            EnginesPanel.Visible = false;
        }
    }

    private void RebindEngines()
    {
        Dictionary<string, HeliconZooEngine> engines = GetEngines();
        EngineGrid.DataSource = engines.Values;
        EngineGrid.DataBind();
    }

    public void BindSettings(StringDictionary settings)
    {
    }

    public void SaveSettings(StringDictionary settings)
    {
        // save engines
        ES.Services.HeliconZoo.SetEngines(PanelRequest.ServiceId, new List<HeliconZooEngine>(GetEngines().Values).ToArray());

        // save switcher
        ES.Services.HeliconZoo.SwithEnginesEnabled(PanelRequest.ServiceId, !QuotasEnabled.Checked);

        ES.Services.HeliconZoo.SetWebCosoleEnabled(PanelRequest.ServiceId, WebCosoleEnabled.Checked);
    }

    protected void ClearEngineForm()
    {
        EngineName.Text = string.Empty;
        EngineFriendlyName.Text = string.Empty;
        EngineFullPath.Text = string.Empty;
        EngineArguments.Text = string.Empty;
        EngineProtocol.SelectedIndex = 0;
        EngineTransport.SelectedIndex = 0;

        foreach (EnvBoxPair envBoxPair in _envBoxsPair)
        {
            envBoxPair.Name.Text = string.Empty;
            envBoxPair.Value.Text = string.Empty;
        }
    }

    protected void ShowEngineForm()
    {
        EngineForm.Visible = true;
        EngineFormButtons.Visible = true;
    }

    protected void HideEngineForm()
    {
        EngineForm.Visible = false;
        EngineFormButtons.Visible = false;
    }

    protected void ButtonAddEngine_Click(object sender, EventArgs e)
    {
        ClearEngineForm();
        ShowEngineForm();
    }

    protected void ButtonSaveEngine_Click(object sender, EventArgs e)
    {
        HeliconZooEngine engine = EngineFromForm();
        HeliconZooEngine savedEngine = FindEngineByName(engine.name);
        Dictionary<string, HeliconZooEngine> engines = GetEngines();

        // new user engine or update existing
        engines[engine.name] = engine;

        ClearEngineForm();
        HideEngineForm();

        // rebind grid
        RebindEngines();
    }

    public static long ParseLong(string s, long deflt)
    {
        long result;
        if (!long.TryParse(s, out result))
        {
            result = deflt;
        }

        return result;
    }

    private HeliconZooEngine EngineFromForm()
    {
        HeliconZooEngine engine = new HeliconZooEngine()
        {
            name = EngineName.Text.Trim(),
            displayName = EngineFriendlyName.Text.Trim(),
            arguments = EngineArguments.Text.Trim(),
            fullPath = EngineFullPath.Text.Trim(),
            transport = EngineTransport.SelectedValue,
            protocol = EngineProtocol.SelectedValue,
            portLower = ParseLong(EnginePortLower.Text, -1),
            portUpper = ParseLong(EnginePortUpper.Text, -1),
            minInstances = ParseLong(EngineMinInstances.Text, -1),
            maxInstances = ParseLong(EngineMaxInstances.Text, -1),
            timeLimit = ParseLong(EngineTimeLimit.Text, -1),
            gracefulShutdownTimeout = ParseLong(EngineGracefulShutdownTimeout.Text, -1),
            memoryLimit = ParseLong(EngineMemoryLimit.Text, -1),
            isUserEngine = true
        };

        // envs
        List<HeliconZooEnv> tempEnvList = new List<HeliconZooEnv>();
        for (int i = 0; i < _envBoxsPair.Length; i++)
        {
            EnvBoxPair pair = _envBoxsPair[i];
            if (!string.IsNullOrEmpty(pair.Name.Text.Trim()) && !string.IsNullOrEmpty(pair.Value.Text.Trim()))
            {
                tempEnvList.Add(new HeliconZooEnv(){Name = pair.Name.Text.Trim(), Value = pair.Value.Text.Trim()});
            }
        }

        engine.environmentVariables = tempEnvList.ToArray();

        return engine;
    }

    protected void ButtonCancelEngineForm_Click(object sender, EventArgs e)
    {
        ClearEngineForm();
        HideEngineForm();
    }

    protected void EngineGrid_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EngineEdit")
        {
            HeliconZooEngine engine = FindEngineByName((string)e.CommandArgument);
            if (null != engine)
            {
                BindEngineForm(engine);
                ShowEngineForm();
            }
        }
        if (e.CommandName == "EngineDisable")
        {
            HeliconZooEngine engine = FindEngineByName((string)e.CommandArgument);
            if (null != engine)
            {
                engine.disabled = !engine.disabled;
                RebindEngines();
            }
        }
        if (e.CommandName == "EngineDelete")
        {
            HeliconZooEngine engine = FindEngineByName((string)e.CommandArgument);
            if (null != engine)
            {
                Dictionary<string, HeliconZooEngine> engines = GetEngines();
                engines.Remove(engine.name);
                RebindEngines();
            }
        }
    }

    public static string ToStringClearDeafult(long l)
    {
        if (-1 == l)
        {
            return string.Empty;
        }

        return l.ToString(CultureInfo.InvariantCulture);
    }

    private void BindEngineForm(HeliconZooEngine engine)
    {
        EngineName.Text = engine.name;
        EngineFriendlyName.Text = engine.displayName;
        EngineFullPath.Text = engine.fullPath;
        EngineArguments.Text = engine.arguments;
        EngineTransport.Text = engine.transport.ToLower();
        EngineProtocol.Text = engine.protocol.ToLower();
        EnginePortLower.Text = ToStringClearDeafult(engine.portLower);
        EnginePortUpper.Text = ToStringClearDeafult(engine.portUpper);
        EngineMinInstances.Text = ToStringClearDeafult(engine.minInstances);
        EngineMaxInstances.Text = ToStringClearDeafult(engine.maxInstances);
        EngineTimeLimit.Text = ToStringClearDeafult(engine.timeLimit);
        EngineGracefulShutdownTimeout.Text = ToStringClearDeafult(engine.gracefulShutdownTimeout);
        EngineMemoryLimit.Text = ToStringClearDeafult(engine.memoryLimit);


        for (int i = 0; i < engine.environmentVariables.Length && i < _envBoxsPair.Length; i++)
        {
            HeliconZooEnv env = engine.environmentVariables[i];
            _envBoxsPair[i].Name.Text = env.Name;
            _envBoxsPair[i].Value.Text = env.Value;
        }

    }

    private Dictionary<string, HeliconZooEngine> GetEngines()
    {
        return ViewState["HeliconZooEngines"] as Dictionary<string, HeliconZooEngine>;
    }

    private HeliconZooEngine FindEngineByName(string engineName)
    {
        Dictionary<string, HeliconZooEngine> engines = GetEngines();
        if (null != engines)
        {
            if (engines.ContainsKey(engineName))
            {
                return engines[engineName];
            }
        }

        return null;
    }

    protected void HostingPackagesGrid_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        ArrayList wpiProductsForInstall = GetProductsToInstallList();

        int productIndex = int.Parse((string)e.CommandArgument);
        WPIProduct wpiProduct = GetHostingPackages()[productIndex];

        if (null != wpiProduct)
        {
            if (e.CommandName == "WpiAdd")
            {
                wpiProductsForInstall = GetProductsToInstallList();
                wpiProductsForInstall.Add(wpiProduct.ProductId);
                SetProductsToInstallList(wpiProductsForInstall);

                ((Button)e.CommandSource).Text = AddUpgradeRemoveText(wpiProduct); ;
                ((Button)e.CommandSource).CommandName = "WpiRemove";
            }

            if (e.CommandName == "WpiRemove")
            {
                wpiProductsForInstall = GetProductsToInstallList();
                wpiProductsForInstall.Remove(wpiProduct.ProductId);
                SetProductsToInstallList(wpiProductsForInstall);

                ((Button)e.CommandSource).Text = AddUpgradeRemoveText(wpiProduct);
                ((Button)e.CommandSource).CommandName = "WpiAdd";
            }

            btnInstall.Enabled = wpiProductsForInstall.Count > 0;
        }
    }

    private ArrayList GetProductsToInstallList()
    {
        if (ViewState["wpiProductsForInstall"] != null)
        {
            return (ArrayList)ViewState["wpiProductsForInstall"];
        }
        return new ArrayList();
    }

    private void SetProductsToInstallList(ArrayList wpiProductsForInstall)
    {
        ViewState["wpiProductsForInstall"] = wpiProductsForInstall;
    }

    private WPIProduct[] GetHostingPackages()
    {
        if (ViewState["HeliconZooHostingPackages"] == null)
        {
            ViewState["HeliconZooHostingPackages"] = RequestHostingPackages();
        }

        return (WPIProduct[])ViewState["HeliconZooHostingPackages"];
    }

    private static WPIProduct[] RequestHostingPackages()
    {
        List<WPIProduct> result = new List<WPIProduct>();
        result.Add(ES.Services.Servers.GetWPIProductById(PanelRequest.ServerId, "HeliconZooModule"));
        result.AddRange(ES.Services.Servers.GetWPIProducts(PanelRequest.ServerId, null, "ZooPackage"));
        



        return result.ToArray();

    }

    protected string AddUpgradeRemoveText(WPIProduct wpiProduct)
    {
        if (GetProductsToInstallList().Contains(wpiProduct.ProductId))
        {
            return "- cancel";
        }
        else
        {
            return wpiProduct.IsUpgrade ? "+ upgrade" : "+ add";
        }
    }

    protected void btnInstall_Click(object sender, EventArgs e)
    {
        ArrayList wpiProductsForInstall = GetProductsToInstallList();

        List<string> qsParts = new List<string>();

        qsParts.Add("pid=Servers");
        qsParts.Add("ctl=edit_platforminstaller");
        qsParts.Add("mid=" + Request.QueryString["mid"]);
        qsParts.Add("ServerID=" + Request.QueryString["ServerID"]);
        qsParts.Add("WPIProduct=" + string.Join(",", wpiProductsForInstall.ToArray()));
        qsParts.Add("ReturnUrl=" + Server.UrlEncode(Request.RawUrl));

        string installUrl = "Default.aspx?" + String.Join("&", qsParts.ToArray());

        Response.Redirect(installUrl);

    }
}
