<####################################################################################################
SolidSCP - web.config Fix Script

v1.0    1st September 2016:    First release of the SolidCP web.config Fix Script
v1.1    2nd September 2016:    2nd release - Added Portal to the script so the new features are added

Written By Marc Banyard for the SolidCP Project (c) 2016 SolidCP

The script needs to be run from the server that holds your Enterprise Server
as the script will query the database to get the servers that form part of your
SolidCP setup and apply the fix to each one in turn.

Copyright (c) 2016, SolidCP
SolidCP is distributed under the Creative Commons Share-alike license

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

- Redistributions of source code must  retain  the  above copyright notice, this
  list of conditions and the following disclaimer.

- Redistributions in binary form  must  reproduce the  above  copyright  notice,
  this list of conditions  and  the  following  disclaimer in  the documentation
  and/or other materials provided with the distribution.

- Neither the name of  SolidCP  nor the names of its contributors may be used to
  endorse or  promote  products  derived  from  this  software  without specific
  prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

All Code provided as is and used at your own risk.
####################################################################################################>
#Requires -RunAsAdministrator

####################################################################################################################################################################################
function CheckXMLnode ($dFilePath, $dXMLNode, $dXMLname, $dXMLvalue) # Function to check if a node exists in an XML file with specific values
{ # Usage - CheckXMLnode $dFilePath "//SecurityClasses" "SecurityClass" "name-Custom"
  # Usage - CheckXMLnode $dFilePath "//SecurityClass" "Name" "Custom"
	[xml]$xml = Get-Content -Path "$dFilePath"
	if ([string]::IsNullOrEmpty($dXMLvalue)) {
		if ($xml.selectNodes("$dXMLNode").$dXMLname) { $true } else { $false }
	}else{
		if ($xml.selectNodes("$dXMLNode").$dXMLname -contains "$dXMLvalue") { $true } else { $false }
	}
}


####################################################################################################################################################################################
function ModifyXML([String] $dFilePath, $dAction, [String] $dNodePath, $dElement, $dValue) # Function to Add, Comment  or UnComment XML nodes in an XML File
{
	[xml]$xml = Get-Content -Path "$dFilePath"
	if ($dAction -eq "Add") {
		$Child = $xml.CreateElement("$dElement")
		if ($dValue -is [System.Array]) {
			if ($dValue[0] -is [System.Array]) { # The Attribute are in a Multi Dimensional Array
				$dCheckNode = "$dNodePath/$dElement"
				if ( !(CheckXMLnode "$dFilePath" "$dCheckNode" $dValue[0][0] $dValue[0][1]) ) {
					foreach($value in $dValue) {
						$Child.SetAttribute($value[0],$value[1])
						$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
					}
				}
			}else{ # The Attributes are in a Single Level Array
				$dCheckNode = "$dNodePath/$dElement"
				if ( !(CheckXMLnode "$dFilePath" "$dCheckNode" $dValue[0] $dValue[1]) ) {
					$Child.SetAttribute($dValue[0],$dValue[1])
					$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
				}
			}
		}
		elseif ($dValue -isnot [System.Array]) { # Simple Element with a Value
			if ( !(CheckXMLnode "$dFilePath" "$dNodePath" "$dElement" "$dValue") ) {
				$Child.InnerText = "$dValue"
				$xml.selectNodes("$dNodePath").AppendChild($Child) | Out-Null
			}
		}
		$xml.Save("$dFilePath") | Out-Null
	}elseif ($dAction -eq "Update") {
		$dNode = $xml.SelectNodes($dNodePath)
		foreach ($node in $dNode) {
			if ($node -ne $null) {
				if ( $node.NodeType -eq "Element") { $node.InnerXml = $dElement }
				else { $node.Value = $dElement }
			}
		}
		$xml.Save("$dFilePath") | Out-Null
	}elseif ($dAction -eq "Get") {
		$dNode = $xml.SelectNodes($dNodePath)
		if ($dNode -ne $null) {
			if ( $dNode.NodeType -eq "Element") { $dNode.InnerXml }
			else { $dNode.Value }
		}
	}elseif ($dAction -eq "Delete") {
		if ( ($xml.selectNodes("$dNodePath")).ParentNode.Name ) {
			($xml | select-xml -xpath ("//" + ($xml.selectNodes("$dNodePath")).ParentNode.Name) | ForEach-Object {$_.node.removechild((select-xml -xpath $dNodePath  -xml $xml).node)}) | Out-Null
			$xml.Save("$dFilePath");
		}
	}elseif ($dAction -eq "Comment") {
		$xml.SelectNodes("$dNodePath") | ForEach-Object {
			$Comment = $xml.CreateComment($_.OuterXml);
			$_.ParentNode.ReplaceChild($Comment, $_) | Out-Null
		}
		$xml.Save("$dFilePath");
	}elseif ($dAction -eq "UnComment") {
		$xml.SelectNodes("//comment()") | ForEach-Object {     
			($_.InnerText | convertto-xml).SelectNodes("/descendant::*[contains(text(), '$dNodePath')]") | ForEach-Object { 
				$UnComment = $_;
				(Get-Content "$dFilePath") | ForEach-Object { $_.Replace("<!--" + $UnComment.InnerText + "-->", $UnComment.InnerText) } | Set-Content "$dFilePath"
			}
		}
	}
}

####################################################################################################################################################################################
Function dPressAnyKeyToExit()                               # Function to press any key to exit
{
	if ($psISE) { # Check if running Powershell ISE
		Add-Type -AssemblyName System.Windows.Forms
		[System.Windows.Forms.MessageBox]::Show("Press any key to exit")
		exit
	}else{
		Write-Host "`n`tPress any key to exit..." -ForegroundColor Yellow
		$x = $host.ui.RawUI.ReadKey("NoEcho,IncludeKeyDown")
		exit
	}
}


####################################################################################################################################################################################
Import-Module WebAdministration
$SCP_EntSvr_Dir     = ((Get-ChildItem IIS:\Sites | Where-Object {$_.Name -match "SolidCP Enterprise Server|WebsitePanel Enterprise Server|DotNetPanel Enterprise Server"}).physicalPath) # SolidCP Enterprise Server Files Location
$SCP_Portal_Dir     = ((Get-ChildItem IIS:\Sites | Where-Object {$_.Name -match "SolidCP Portal|WebsitePanel Portal|DotNetPanel Portal"}).physicalPath)                                  # SolidCP Portal Files Location
$SCP_Database_Name  = ( (([xml](Get-Content "$SCP_EntSvr_Dir\Web.config")).configuration.connectionStrings.add.connectionString) | `
                        Select-String '((Initial\sCatalog)|((Database)))\s*=(?<ic>[a-z\s0-9]+?);' | `
                        ForEach-Object  {$_.Matches} | `
                        ForEach-Object {$_.Groups["ic"].Value} ) # Get the SolidCP Database Name from the Enterprise Server Connection String in the web.config file
$SCP_Database_Servr = ( (([xml](Get-Content "$SCP_EntSvr_Dir\Web.config")).configuration.connectionStrings.add.connectionString) | `
                        Select-String 'server=(?<ic>[^;]+?);' | `
                        ForEach-Object  {$_.Matches} | `
                        ForEach-Object {$_.Groups["ic"].Value} ) # Get the SolidCP Database Server from the Enterprise Server Connection String in the web.config file

# Update the Enterprise Server web.config file and remove the Windows Authentication that some people had mistakenly set
ModifyXML "$SCP_EntSvr_Dir\web.config" "Delete" "//configuration/system.webServer"
Write-Host "`t Enterprise Server - web.config Upgraded" -ForegroundColor Green

# Set the correct values in the "bin\SolidCP.SchedulerService.exe.config" file so they match the ones in the "web.config" file for the Connection String and the CryptoKey
ModifyXML "C:\SolidCP\Enterprise Server\bin\SolidCP.SchedulerService.exe.config" "Update" "//configuration/connectionStrings/add[@name='EnterpriseServer']/@connectionString" (ModifyXML "C:\SolidCP\Enterprise Server\web.config" "Get" "//configuration/connectionStrings/add[@name='EnterpriseServer']/@connectionString")
ModifyXML "C:\SolidCP\Enterprise Server\bin\SolidCP.SchedulerService.exe.config" "Update" "//configuration/appSettings/add[@key='SolidCP.CryptoKey']/@value"                  (ModifyXML "C:\SolidCP\Enterprise Server\web.config" "Get" "//configuration/appSettings/add[@key='SolidCP.CryptoKey']/@value")
Write-Host "`t Enterprise Server - Schedular Config File Upgraded" -ForegroundColor Green

# Update the Portal web.config file and the additional items if they are missing
# Update the web.config to change the "xmlns" to "xmlns-temp" otherwise we have issues when parsing the XML file
(Get-Content "$SCP_Portal_Dir\web.config") -replace " xmlns=`"", " xmlns-temp=`"" | Set-Content "$SCP_Portal_Dir\web.config"
# Update the web.config file to make sure it is up to date with the new Mailcleaner (Ignore SSL Check) Settings
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" 'system.net'
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.net" "settings"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.net/settings" "servicePointManager" @( ("checkCertificateName","false"), ("checkCertificateRevocationList","false") )
# Update the web.config file to make sure it is up to date with the new Settings for v1.1.0 of SolidCP
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" "configSections"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections" "sectionGroup" @("name","jsEngineSwitcher")
(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <sectionGroup name=`"jsEngineSwitcher`" />", "    <sectionGroup name=`"jsEngineSwitcher`">`n    </sectionGroup>" | Set-Content "$SCP_Portal_Dir\web.config"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='jsEngineSwitcher']" "section" @( ("name","core"), ("type","JavaScriptEngineSwitcher.Core.Configuration.CoreConfiguration, JavaScriptEngineSwitcher.Core") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='jsEngineSwitcher']" "section" @( ("name","msie"), ("type","JavaScriptEngineSwitcher.Msie.Configuration.MsieConfiguration, JavaScriptEngineSwitcher.Msie") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections" "sectionGroup" @("name","bundleTransformer")
(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <sectionGroup name=`"bundleTransformer`" />", "    <sectionGroup name=`"bundleTransformer`">`n    </sectionGroup>" | Set-Content "$SCP_Portal_Dir\web.config"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='bundleTransformer']" "section" @( ("name","core"), ("type","BundleTransformer.Core.Configuration.CoreSettings, BundleTransformer.Core") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/configSections/sectionGroup[@name='bundleTransformer']" "section" @( ("name","less"), ("type","BundleTransformer.Less.Configuration.LessSettings, BundleTransformer.Less") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.web/pages[@theme='Default'][@validateRequest='false'][@controlRenderingCompatibilityVersion='3.5'][@clientIDMode='AutoID']/controls" "add" @( ("tagPrefix","CPCC"), ("namespace","CPCC"), ("assembly","CPCC") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/system.webServer/handlers" "add" @( ("name","LessAssetHandler"), ("path","`*.less"), ("verb","GET"), ("type","BundleTransformer.Less.HttpHandlers.LessAssetHandler, BundleTransformer.Less"), ("resourceType","File"), ("preCondition","") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" "jsEngineSwitcher" @("xmlns-temp","http`:`/`/tempuri.org`/JavaScriptEngineSwitcher.Configuration.xsd")
(Get-Content "$SCP_Portal_Dir\web.config") -replace "  <jsEngineSwitcher xmlns-temp=`"http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd`" />", "  <jsEngineSwitcher xmlns-temp=`"http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd`">`n  </jsEngineSwitcher>" | Set-Content "$SCP_Portal_Dir\web.config"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/jsEngineSwitcher[@xmlns-temp='http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd']" "core"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/jsEngineSwitcher[@xmlns-temp='http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd']/core" "engines"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/jsEngineSwitcher[@xmlns-temp='http://tempuri.org/JavaScriptEngineSwitcher.Configuration.xsd']/core/engines" "add" @( ("name","MsieJsEngine"), ("type","JavaScriptEngineSwitcher.Msie.MsieJsEngine, JavaScriptEngineSwitcher.Msie") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" "bundleTransformer" @("xmlns-temp","http://tempuri.org/BundleTransformer.Configuration.xsd")
(Get-Content "$SCP_Portal_Dir\web.config") -replace "  <bundleTransformer xmlns-temp=`"http://tempuri.org/BundleTransformer.Configuration.xsd`" />", "  <bundleTransformer xmlns-temp=`"http://tempuri.org/BundleTransformer.Configuration.xsd`">`n  </bundleTransformer>" | Set-Content "$SCP_Portal_Dir\web.config"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']" "core"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core" "css"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "translators"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/translators" "add" @( ("name","NullTranslator"), ("type","BundleTransformer.Core.Translators.NullTranslator, BundleTransformer.Core"), ("enabled","false") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/translators" "add" @( ("name","LessTranslator"), ("type","BundleTransformer.Less.Translators.LessTranslator, BundleTransformer.Less") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "postProcessors"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/postProcessors" "add" @( ("name","UrlRewritingCssPostProcessor"), ("type","BundleTransformer.Core.PostProcessors.UrlRewritingCssPostProcessor, BundleTransformer.Core"), ("useInDebugMode","false") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "minifiers"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/minifiers" "add" @( ("name","NullMinifier"), ("type","BundleTransformer.Core.Minifiers.NullMinifier, BundleTransformer.Core") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css" "fileExtensions"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/fileExtensions" "add" @( ("fileExtension",".css"), ("assetTypeCode","Css") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/css/fileExtensions" "add" @( ("fileExtension",".less"), ("assetTypeCode","Less") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core" "js"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js" "translators"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js/translators" "add" @( ("name","NullTranslator"), ("type","BundleTransformer.Core.Translators.NullTranslator, BundleTransformer.Core"), ("enabled","false") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js" "minifiers"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js/minifiers" "add" @( ("name","NullMinifier"), ("type","BundleTransformer.Core.Minifiers.NullMinifier, BundleTransformer.Core") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js" "fileExtensions"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/core/js/fileExtensions" "add" @( ("fileExtension",".js"), ("assetTypeCode","JavaScript") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']" "less" @( ("useNativeMinification","true"), ("ieCompat","true"), ("strictMath","false"), ("strictUnits","false"), ("dumpLineNumbers","None"), ("javascriptEnabled","true") )
(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <less useNativeMinification=`"true`" ieCompat=`"true`" strictMath=`"false`" strictUnits=`"false`" dumpLineNumbers=`"None`" javascriptEnabled=`"true`" />", "    <less useNativeMinification=`"true`" ieCompat=`"true`" strictMath=`"false`" strictUnits=`"false`" dumpLineNumbers=`"None`" javascriptEnabled=`"true`">`n    </less>" | Set-Content "$SCP_Portal_Dir\web.config"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/bundleTransformer[@xmlns-temp='http://tempuri.org/BundleTransformer.Configuration.xsd']/less[@useNativeMinification='true'][@ieCompat='true'][@strictMath='false'][@strictUnits='false'][@dumpLineNumbers='None'][@javascriptEnabled='true']" "jsEngine" @("name","MsieJsEngine")
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration" "runtime"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime" "assemblyBinding" @("xmlns-temp","urn:schemas-microsoft-com:asm.v1")
(Get-Content "$SCP_Portal_Dir\web.config") -replace "    <assemblyBinding xmlns-temp=`"urn:schemas-microsoft-com:asm.v1`" />", "    <assemblyBinding xmlns-temp=`"urn:schemas-microsoft-com:asm.v1`">`n    </assemblyBinding>" | Set-Content "$SCP_Portal_Dir\web.config"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']" "dependentAssembly"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "assemblyIdentity" @( ("name","Newtonsoft.Json"), ("publicKeyToken","30ad4fe6b2a6aeed"), ("culture","neutral") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "bindingRedirect" @( ("oldVersion","0.0.0.0-9.0.0.0"), ("newVersion","9.0.0.0") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']" "dependentAssembly"
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "assemblyIdentity" @( ("name","WebGrease"), ("publicKeyToken","31bf3856ad364e35"), ("culture","neutral") )
ModifyXML "$SCP_Portal_Dir\web.config" "Add" "//configuration/runtime/assemblyBinding[@xmlns-temp='urn:schemas-microsoft-com:asm.v1']/dependentAssembly" "bindingRedirect" @( ("oldVersion","0.0.0.0-1.5.2.14234"), ("newVersion","1.5.2.14234") )
# Update the web.config to change the "xmlns-temp" back to "xmlns" now we have finished parsing the XML file
(Get-Content "$SCP_Portal_Dir\web.config") -replace " xmlns-temp=`"", " xmlns=`"" | Set-Content "$SCP_Portal_Dir\web.config"

# Update each of the Server web.config file and remove the Windows Authentication from them that some people had mistakenly set
push-location ; ($SCP_UNC_Test = Invoke-SQLCmd -query "SELECT [ServerName], [ServerUrl] FROM [$SCP_Database_Name].[dbo].[Servers] WHERE [VirtualServer]='0'" -Server $SCP_Database_Servr) | Out-Null ; Pop-Location
for ($i = 0; $i -lt ($SCP_UNC_Test.count); $i++) { # Loop through each server in the $SQPServerQuery variable
	# Define the Variables to be used per Server
	$SCP_UNCsvr_Name = $SCP_UNC_Test.ServerName[$i]
	$SCP_Server_Root = ($SCP_UNC_Test.ServerUrl[$i] -replace "http://|https://|:9003", "")
	# Loop through the hard drive on the Remote Server to find the SolidCP Server directory
	foreach ($RemoteServer in (Get-ChildItem (Get-ChildItem -Path "\\$SCP_Server_Root\c$\" -Include ("WebsitePanel", "SolidCP", "DotNetPanel")).FullName)) {
		If ($RemoteServer.name -eq "Server") {$SCP_UNCsvr_Dir = $RemoteServer.FullName}
	}
	if (Test-Path "$SCP_UNCsvr_Dir") {
		ModifyXML "$SCP_UNCsvr_Dir\web.config" "Delete" "//configuration/system.webServer"
		Write-Host "`t $SCP_UNCsvr_Name - web.config Upgraded" -ForegroundColor Green
	}else{
		Write-Host "`t Unable to connect to $SCP_UNCsvr_Name - Check Firewall Settings" -ForegroundColor Yellow
	}
}
dPressAnyKeyToExit


####################################################################################################################################################################################
