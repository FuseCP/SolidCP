<?php if (!defined('WHMCS')) exit('ACCESS DENIED');
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

/**
 * SolidCP WHMCS SolidCP / WHMCS Hooks
 *
 * @author SolidCP
 * @link https://solidcp.com/
 * @access public
 * @name SolidCP
 * @version 1.1.2
 * @package WHMCS
 */

require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/enterpriseserver.php');
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/var_definition.php');
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/database.php');
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/settings.php');

/**
 * Handles updating SolidCP account details when a client or administrator updates a client's details
 * 
 * @access public
 * @param array $params WHMCS parameters
 * @throws Exception
 */
function solidcp_module_ClientEdit($params)
{
    $solidcp_settings = new solidcp_settings;
    $solidcp_settings->getSettings();
    if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return false;
    if($solidcp_settings->settings['NeedMigration'] == 1) return false;
    if($solidcp_settings->settings['SyncActive'] != 1) return false;

    // WHMCS server parameters & package parameters
    $userid = $params['userid'];
    $serviceid = 0;
    
    // Query for the users SolidCP accounts - If they do not have any, just ignore the request
    $scpaccounts = solidcp_database::getUserSCPAccounts($userid);
    if(is_array($scpaccounts) && $scpaccounts['status']=='error'){
        throw new Exception($scpaccounts['description']);
    }
    if(!empty($scpaccounts)){
        foreach($scpaccounts as $scpaccount){
            // Start updating the users account details
            $serviceid = $scpaccount->serviceid;
            $username = $scpaccount->username;
            $serverUsername = $scpaccount->serverusername;
            $serverPassword = decrypt($scpaccount->serverpassword);
            $serverPort = empty($scpaccount->serverport) ? '9002' : $scpaccount->serverport;
            $serverHost = empty($scpaccount->serverhostname) ? $scpaccount->serverip : $scpaccount->serverhostname;
            $serverSecure = $scpaccount->serversecure == 'on' ? TRUE : FALSE;
            $clientsDetails = $params;

            try
            {
                // Create the SolidCP Enterprise Server Client object instance
                $scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);

                // Get the user's details from SolidCP - We need the username
                $user = $scp->getUserByUsername($username);
                if (empty($user))
                {
                    throw new Exception("User {$username} does not exist - Cannot update account details for unknown user");
                }

                // Update the user's account details using the previous details + WHMCS's details (address, city, state etc.)
                $userParams = array('RoleId' => $user['RoleId'],
                                'Role' => $user['Role'],
                                'StatusId' => $user['StatusId'],
                                'Status' => $user['Status'],
                                'LoginStatusId' => $user['LoginStatusId'],
                                'LoginStatus' => $user['LoginStatus'],
                                'FailedLogins' => $user['FailedLogins'],
                                'UserId' => $user['UserId'],
                                'OwnerId' => $user['OwnerId'],
                                'IsPeer' => $user['IsPeer'],
                                'Created' => $user['Created'],
                                'Changed' => $user['Changed'],
                                'IsDemo' => $user['IsDemo'],
                                'Comments' => $user['Comments'],
                                'LastName' => $clientsDetails['lastname'],
                                'Username' => $user['Username'],
                                'Password' => $user['Password'],
                                'FirstName' => $clientsDetails['firstname'],
                                'Email' => $clientsDetails['email'],
                                'PrimaryPhone' => $clientsDetails['phonenumber'],
                                'Zip' => $clientsDetails['postcode'],
                                'InstantMessenger' => '',
                                'Fax' => '',
                                'SecondaryPhone' => '',
                                'SecondaryEmail' => '',
                                'Country' => $clientsDetails['country'],
                                'Address' => $clientsDetails['address1'],
                                'City' => $clientsDetails['city'],
                                'State' => $clientsDetails['state'],
                                'HtmlMail' => $user['HtmlMail'],
                                'CompanyName' => $clientsDetails['companyname'],
                                'EcommerceEnabled' => $user['EcommerceEnabled'],
                                'SubscriberNumber' => '');

                // Execute the UpdateUserDetails method
                $scp->updateUserDetails($userParams);

                // Add log entry to client log
                logactivity("SolidCP Sync - Account {$username} contact details updated successfully", $userid);
            }
            catch (Exception $e)
            {
                // Error message to log / return
                $errorMessage = "solidcp_module_ClientEdit Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";

                // Log to WHMCS
                logactivity($errorMessage, $userid);
            }
        }
    }
}

/**
 * Handles activating and adding client addons to SolidCP
 * 
 * @access public
 * @param array $params WHMCS parameters
 * @throws Exception
 */
function solidcp_module_AddonActivation($params)
{
    $solidcp_settings = new solidcp_settings;
    $solidcp_settings->getSettings();
    if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return false;
    if($solidcp_settings->settings['NeedMigration'] == 1) return false;
    if($solidcp_settings->settings['AddonsActive'] != 1) return false;

    // WHMCS server parameters & package parameters
    $userid = $params['userid'];
    $serviceid = $params['serviceid'];
    $addonid = $params['addonid'];

    try
    {
        $scpaccount = solidcp_database::getAddonActivationSCPAccount($serviceid, $addonid);
        if(is_array($scpaccount) && $scpaccount['status']=='error'){
            throw new Exception($scpaccount['description']);
        }

        if (!empty($scpaccount)){
            // Start processing the users addon
            $username = $scpaccount->username;
            $serverUsername = $scpaccount->serverusername;
            $serverPassword = decrypt($scpaccount->serverpassword);
            $serverPort = empty($scpaccount->serverport) ? '9002' : $scpaccount->serverport;
            $serverHost = empty($scpaccount->serverhostname) ? $scpaccount->serverip : $scpaccount->serverhostname;
            $serverSecure = $scpaccount->serversecure == 'on' ? TRUE : FALSE;
        
            // Create the SolidCP Enterprise Server Client object instance
            $scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
        
            // Get the user's details from SolidCP - We need the userid
            $user = $scp->getUserByUsername($username);
            if (empty($user))
            {
                throw new Exception("User {$username} does not exist - Cannot allocate addon for unknown user");
            }
            
            // Get the user's package details from SolidCP - We need the PackageId
            $package = $scp->getUserPackages($user['UserId']);
            $packageId = $package['PackageId'];
            
            // Get the associated SolidCP addon id
            $addon = solidcp_database::getSCPAddon($addonid);
            if(is_array($addon) && $addon['status']=='error'){
                throw new Exception($addon['description']);
            }
            elseif(empty($addon)){
                throw new Exception("WHMCS AddonID {$addonid} doesn't exists in".SOLIDCP_ADDONS_TABLE);
            }
            
            // Add the Addon Plan to the customer's SolidCP package / hosting space
            $results = $scp->addPackageAddonById($packageId, $addon->scp_id);
            
            // Check the results to verify that the addon has been successfully allocated
            if ($results['Result'] > 0)
            {
                // If this addon is an IP address addon - attempt to randomly allocate an IP address to the customer's hosting space
                if ($addon->is_ipaddress == 1)
                {
                    $scp->allocatePackageIPAddresses($packageId);
                }
                
                // Add log entry to client log
                logactivity("SolidCP Addon - Account {$username} addon successfully completed - Addon ID: {$addonid}", $userid);
            }
            else
            {
                // Add log entry to client log
                throw new Exception("Unknown", $results['Result']);
            }
        }
    }
    catch (Exception $e)
    {
        // Error message to log / return
        $errorMessage = "solidcp_addons_AddonActivation Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";

        // Log to WHMCS
        logactivity($errorMessage, $userid);
    }
}

/**
 * Handles deleting addons to SolidCP from Admin area
 * 
 * @access public
 * @param array $params WHMCS parameters
 * @throws Exception
 */
/*  FIX ME!!!!!!
 * This code is executed, before the values are saved to the WHMCS DB. Therefore "modulechangepackage" doesn't work.
 * 
function solidcp_module_AddonDeleted($params)
{
    $solidcp_settings = new solidcp_settings;
    $solidcp_settings->getSettings();
    if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return false;
    if($solidcp_settings->settings['NeedMigration'] == 1) return false;
    if($solidcp_settings->settings['AddonsActive'] != 1) return false;

    $id = $params['id'];
    $result = full_query("SELECT h.id AS serviceid FROM `tblhostingaddons` AS a, `tblhosting` AS h, `tblservers` AS s, `tblproducts` AS p WHERE a.id = {$id} AND h.id = a.hostingid AND h.packageid = p.id AND h.server = s.id AND s.type = 'SolidCP' AND h.domainstatus IN ('Active', 'Suspended')");
    if (mysql_num_rows($result) > 0)
    {
        $adminuser = $solidcp_settings->settings['WhmcsAdmin'];
        $row = mysql_fetch_assoc($result);
        $values["serviceid"]  = $row['serviceid'];
        $command = "modulechangepackage";
        $results = localAPI($command,$values,$adminuser);
        if($results['result'] == 'success') logactivity("SolidCP Addon - Addon ID: {$id} successfully deleted.");
        elseif($results['result'] == 'error') logactivity("SolidCP Addon - Addon ID: {$id} couldn't be deleted. Error: {$results['message']}");
    }
}*/

/* Update Client Contact Details - SolidCP */
add_hook('ClientEdit', 1, 'solidcp_module_ClientEdit');
/* Addon Activation/Deleting - SolidCP */
add_hook('AddonActivation', 1, 'solidcp_module_AddonActivation');
add_hook('AddonAdd', 1, 'solidcp_module_AddonActivation');
//add_hook('AddonDeleted', 1, 'solidcp_module_AddonDeleted');
