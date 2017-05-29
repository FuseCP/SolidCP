<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HyperV2012R2_Create.ascx.cs" Inherits="SolidCP.Portal.ProviderControls.HyperV2012R2_Create" %>
<%@ Register TagPrefix="scp" TagName="Generation" Src="../UserControls/Generation.ascx" %>
<%@ Register TagPrefix="scp" TagName="DynamicMemory" Src="../UserControls/DynamicMemory.ascx" %>

<scp:Generation runat="server" ID="GenerationSetting"></scp:Generation>
<scp:DynamicMemory runat="server" ID="DynamicMemorySetting"></scp:DynamicMemory>
