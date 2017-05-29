<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SolidCP.WebPortal.DefaultPage" %>

<!DOCTYPE html>
<html>
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=9">
    <title>Solid CP</title>
    <link runat="server" rel="stylesheet" href="~/Styles/Import.css" type="text/css" id="AdaptersInvariantImportCSS" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    	<!-- Fav and touch icons -->
	<link rel="apple-touch-icon" sizes="57x57" href="/apple-touch-icon-57x57.png">
    <link rel="apple-touch-icon" sizes="60x60" href="/apple-touch-icon-60x60.png">
    <link rel="apple-touch-icon" sizes="72x72" href="/apple-touch-icon-72x72.png">
    <link rel="apple-touch-icon" sizes="76x76" href="/apple-touch-icon-76x76.png">
    <link rel="apple-touch-icon" sizes="114x114" href="/apple-touch-icon-114x114.png">
    <link rel="apple-touch-icon" sizes="120x120" href="/apple-touch-icon-120x120.png">
    <link rel="apple-touch-icon" sizes="144x144" href="/apple-touch-icon-144x144.png">
    <link rel="apple-touch-icon" sizes="152x152" href="/apple-touch-icon-152x152.png">
    <link rel="apple-touch-icon" sizes="180x180" href="/apple-touch-icon-180x180.png">
    <link rel="icon" type="image/png" href="/favicon-32x32.png" sizes="32x32">
    <link rel="icon" type="image/png" href="/android-chrome-192x192.png" sizes="192x192">
    <link rel="icon" type="image/png" href="/favicon-96x96.png" sizes="96x96">
    <link rel="icon" type="image/png" href="/favicon-16x16.png" sizes="16x16">
    <link rel="manifest" href="/manifest.json">
    <link rel="mask-icon" href="/safari-pinned-tab.svg" color="#5bbad5">
    <meta name="msapplication-TileColor" content="#da532c">
    <meta name="msapplication-TileImage" content="/mstile-144x144.png">
    <meta name="theme-color" content="#ffffff">
<!--[if lt IE 7]>
    <link runat="server" rel="stylesheet" href="~/Styles/BrowserSpecific/IEMenu6.css" type="text/css" id="IEMenu6CSS" />
<![endif]--> 
<!--[if gt IE 6]>
    <link runat="server" rel="stylesheet" href="~/Styles/BrowserSpecific/IEMenu7.css" type="text/css" id="IEMenu7CSS" />
<![endif]--> 
</head>
<body class="fixed-top-active dashboard">
    <form id="form1" runat="server"  autocomplete="off">
        <asp:PlaceHolder ID="skinPlaceHolder" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
