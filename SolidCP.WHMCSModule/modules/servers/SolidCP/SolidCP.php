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

// SolidCP server module core files
require_once(ROOTDIR. '/modules/addons/solidcp_module/lib/database.php');
require_once(ROOTDIR. '/modules/addons/solidcp_module/lib/settings.php');
require_once(ROOTDIR. '/modules/addons/solidcp_module/lib/enterpriseserver.php');
require_once(ROOTDIR. '/modules/addons/solidcp_module/lib/solidcp_functions.php');

/**
 * SolidCP WHMCS Server Module
 *
 * @author SolidCP
 * @link https://solidcp.com/
 * @access public
 * @name SolidCP
 * @version 1.1.3
 * @package WHMCS
 */

/**
 * Returns the SolidCP package / server configuration options
 *
 * @access public
 * @return array
 */
function SolidCP_ConfigOptions()
{
	return array('Package Name' => array( 'Type' => 'text', 'Size' => 25, 'Description' => 'Package Name'),
				 'Web Space Quota' => array( 'Type' => 'text', 'Size' => 5, 'Description' => 'MB'),
				 'Bandwidth Limit' => array( 'Type' => 'text', 'Size' => 5, 'Description' => 'MB'),
				 'SolidCP Plan ID' => array( 'Type' => 'text', 'Size' => 4, 'Description' => 'SolidCP hosting plan id'),
				 'Parent Space ID' => array( 'Type' => 'text', 'Size' => 3, 'Description' => '* SpaceID that all accounts are created under', 'Default' => 1),
				 'Different Potal URL' => array( 'Type' => 'yesno', 'Description' => 'Tick if portal address is different to server address'),
				 'Portal URL' => array( 'Type' => 'text', 'Size' => 25, 'Description' => 'Portal URL, with http(s)://, no trailing slash'  ),
				 'Send Account Summary Email' => array( 'Type' => 'yesno', 'Description' => 'Tick to send the "Account Summary" letter' ),
				 'Send Hosting Space Summary Email' => array( 'Type' => 'yesno', 'Description' => 'Tick to send the "Hosting Space Summary" letter'),
				 'Create Mail Account' => array( 'Type' => 'yesno', 'Description' => 'Tick to create mail account' ),
				 'Create FTP Account' => array( 'Type' => 'yesno', 'Description' => 'Tick to create FTP account' ),
                 'Create Temporary Domain' => array( 'Type' => 'yesno', 'Description' => 'Tick to create a temporary domain' ),
				 'Send HTML Email' => array( 'Type' => 'yesno', 'Description' => 'Tick enable HTML email from SolidCP' ),
				 'Create Website' => array( 'Type' => 'yesno', 'Description' => 'Tick to create Website' ),
				 'Count Bandwidth / Diskspace' => array( 'Type' => 'yesno', 'Description' => 'Tick to update diskpace / bandwidth in WHMCS'),
				 'Default Pointer' => array( 'Type' => 'text', 'Size' => 25, 'Description' => 'The default pointer (hostname) to use when creating a Website' ),
				 'Create DNS Zone' => array( 'Type' => 'yesno', 'Description' => 'Tick to create domain DNS zone'));  
}

/**
 * Metadata for server page
 */
function SolidCP_MetaData()
{
    return array(
        'DisplayName' => 'SolidCP',
        'DefaultNonSSLPort' => '9002',
        'DefaultSSLPort' => '9002',
    );
}

/**
 * Test connection with the given server parameters.
 * 
 * @param array $params common module parameters
 * @return array
 */
function SolidCP_TestConnection(array $params)
{
    try {
	$serverUsername = $params['serverusername'];
	$serverPassword = $params['serverpassword'];
	$serverPort = $params['serverport'];
	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
	$serverSecure = $params['serversecure'];
        $scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
        $result = $scp->getUserByUsername($serverUsername);
        if ($result){
            $success = true;
            $errorMsg = '';
        }
        else{
            $success = false;
            $errorMsg = SolidCP_EnterpriseServer::getFriendlyError($result);
        }
    } catch (Exception $e) {
        // Record the error in WHMCS's module log.
        logModuleCall(
            'SolidCP',
            __FUNCTION__,
            $params,
            $e->getMessage(),
            $e->getTraceAsString()
        );
        $success = false;
        $errorMsg = $e->getMessage();
    }
    return array(
        'success' => $success,
        'error' => $errorMsg,
    );
}

/**
 * Creates the SolidCP user account and package
 * 
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return string
 */
function SolidCP_CreateAccount($params)
{
        $solidcp_settings = new solidcp_settings;
        $solidcp_settings->getSettings();
        if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return "The first configuration of the SolidCP Module need to be performed. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";
        if($solidcp_settings->settings['NeedMigration'] == 1) return "A migration from an old to this new module need to be performed first. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";
        
	// WHMCS server parameters & package parameters
	$serverUsername = $params['serverusername'];
	$serverPassword = $params['serverpassword'];
	$serverPort = $params['serverport'];
	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
	$serverSecure = $params['serversecure'];
	$username = $params['username'];
	$password = $params['password'];
	$domain = $params['domain'];
	$packageType = $params['type'];
	$clientsDetails = $params['clientsdetails'];
	$userId = $clientsDetails['userid'];
	$serviceid = $params['serviceid'];
		
	// SolidCP API parameters
	$planId = $params['configoption4'];
	$parentPackageId = $params['configoption5'];
	$roleId = (($packageType == 'reselleraccount') ? 2 : 3);
	$htmlMail = ($params['configoption13'] == 'on');
	$sendAccountLetter = ($params['configoption8'] == 'on');
	$sendPackageLetter = ($params['configoption9'] == 'on');
	$createMailAccount = ($params['configoption10'] == 'on');
	$createTempDomain = ($params['configoption12'] == 'on');
	$createFtpAccount = ($params['configoption11'] == 'on');
	$createWebsite = ($params['configoption14'] == 'on');
	$websitePointerName = $params['configoption16'];
	$createZoneRecord = ($params['configoption17'] == 'on');
	
	try
	{
	    // Create the SolidCP Enterprise Server Client object instance
	    $scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
	    
	    // Create the user's new account using the CreateUserWizard method
	    $userParams = array('parentPackageId' => $parentPackageId,
	    					'username' => $username,
	    					'password' => $password,
	    					'roleId' => $roleId,
	    					'firstName' => $clientsDetails['firstname'],
	    					'lastname' => $clientsDetails['lastname'],
	    					'email' => $clientsDetails['email'],
	    					'secondaryEmail' => '',
	    					'htmlMail' => $htmlMail,
	    					'sendAccountLetter' => $sendAccountLetter,
	    					'createPackage' => TRUE,
	    					'planId' => $planId,
	    					'sendPackageLetter' => $sendPackageLetter,
	    					'domainName' => $domain,
	    					'tempDomain' => $createTempDomain,
	    					'createWebSite' => $createWebsite,
	    					'createFtpAccount' => $createFtpAccount,
	    					'ftpAccountName' => $username,
	    					'createMailAccount' => $createMailAccount,
	    					'hostName' => $websitePointerName,
	    					'createZoneRecord' => $createZoneRecord);
	    
	    // Execute the CreateUserWizard method
	    $result = $scp->createUserWizard($userParams);
	    if ($result < 0)
	    {
	    	// Something went wrong
	    	throw new Exception('Fault: ' . SolidCP_EnterpriseServer::getFriendlyError($result), $result);
	    }
	    // Log the module call
	    SolidCP_logModuleCall(__FUNCTION__, $params, $result);
	    
	    // Get the newly created user's details from SolidCP so we can update the account details completely
	    $user = $scp->getUserByUsername($username);
	    
	    // Update the user's account details using the previous details + WHMCS's details (address, city, state etc.)
	    $userParams = array('RoleId' => $roleId,
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
				    		'Username' => $username,
				    		'Password' => $password,
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
				    		'HtmlMail' => $htmlMail,
				    		'CompanyName' => $clientsDetails['companyname'],
				    		'EcommerceEnabled' => ($roleId == 2),
				    		'SubscriberNumber' => '',
							'MfaMode' => $user['MfaMode']);
	    
	    // Execute the UpdateUserDetails method
	    $scp->updateUserDetails($userParams);
            
            // Create configurable options as addons.
            if($params['configoptions'] && $solidcp_settings->settings['ConfigurableOptionsActive'] == 1){
                // Get the associated SolidCP addon id to the configurable option
                $configurableoptions = solidcp_database::getServiceConfigurableOptions($serviceid);
                if(is_array($configurableoptions) && $configurableoptions['status']=='error'){
                    throw new Exception($configurableoptions['description']);
                }

                if(count($configurableoptions)>0){
                    $package = $scp->getUserPackages($user['UserId']);
                    $packageId = $package['PackageId'];
                  
                    foreach ($configurableoptions as $addon){
                        $addonPlanId = $addon->scp_id;
                        $addonIsIpAddress = $addon->is_ipaddress;
                        $addonqty = $addon->qty;
                        $addonOptType = $addon->optiontype;
                        // If Optiontype is quantity or yes/no then 0 is allowed. Otherwise quantity will be 1.
                        if($addonqty == 0 && ($addonOptType == 3 || $addonOptType == 4)) continue;
                        elseif($addonOptType != 3 && $addonOptType != 4) $addonqty = 1;
                        // Add the Addon Plan to the customer's SolidCP package / hosting space
                        $results = $scp->addPackageAddonById($packageId, $addonPlanId, $addonqty);

                        // Check the results to verify that the addon has been successfully allocated
                        if ($results['Result'] > 0)
                        {
                            // If this addon is an IP address addon - attempt to randomly allocate an IP address to the customer's hosting space
                            if ($addonIsIpAddress == 1)
                            {
                                $scp->allocatePackageIPAddresses($packageId);
                            }

                            // Add log entry to client log
                            logactivity("SolidCP Addon - Account {$username} addon successfully completed - Addon ID: {$addonId}", $userId);
                        }
                        else
                        {
                            // Add log entry to client log
                            throw new Exception("Unknown", $results['Result']);
                        }
                    }
                }
            }
            
            // Notify success
	    return 'success';
	}
	catch (Exception $e)
	{
		// Error message to log / return
		$errorMessage = "CreateAccount Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";
		
		// Log to WHMCS
		logactivity($errorMessage, $userId);
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());
		
		// Notify failure - Houston we have a problem!
		return $errorMessage;
	}
}

/**
 * Terminates the SolidCP user account and package
 *
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return string
 */
function SolidCP_TerminateAccount($params)
{
        $solidcp_settings = new solidcp_settings;
        $solidcp_settings->getSettings();
        if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return "The first configuration of the SolidCP Module need to be performed. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";
        if($solidcp_settings->settings['NeedMigration'] == 1) return "A migration from an old to this new module need to be performed first. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";

	// WHMCS server parameters & package parameters
	$serverUsername = $params['serverusername'];
	$serverPassword = $params['serverpassword'];
	$serverPort = $params['serverport'];
	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
	$serverSecure = $params['serversecure'];
	$username = $params['username'];
	$clientsDetails = $params['clientsdetails'];
	$userId = $clientsDetails['userid'];
	$serviceid = $params['serviceid'];
	$domain = $params['domain'];
	
	try
	{
		// Create the SolidCP Enterprise Server Client object instance
		$scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
		
		// Get the user's details from SolidCP - We need the userid
		$user = $scp->getUserByUsername($username);
		if (empty($user))
		{
			throw new Exception("User {$username} does not exist - Cannot terminate account for unknown user");
		}
		
		// Attempt the delete the users account
		$result = $scp->deleteUser($user['UserId']);
		if ($result < 0)
		{
			// Something went wrong
			throw new Exception('Fault: ' . SolidCP_EnterpriseServer::getFriendlyError($result), $result);
		}
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $result);

		// Notify success
		return 'success';
	}
	catch (Exception $e)
	{
		// Error message to log / return
		$errorMessage = "TerminateAccount Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";
		
		// Log to WHMCS
		logactivity($errorMessage, $userId);
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());
		
		// Notify failure - Houston we have a problem!
		return $errorMessage;
	}
}

/**
 * Suspends the SolidCP user account and package
 *
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return string
 */
function SolidCP_SuspendAccount($params)
{
        $solidcp_settings = new solidcp_settings;
        $solidcp_settings->getSettings();
        if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return "The first configuration of the SolidCP Module need to be performed. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";
        if($solidcp_settings->settings['NeedMigration'] == 1) return "A migration from an old to this new module need to be performed first. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";

	// WHMCS server parameters & package parameters
	$serverUsername = $params['serverusername'];
	$serverPassword = $params['serverpassword'];
	$serverPort = $params['serverport'];
	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
	$serverSecure = $params['serversecure'];
	$username = $params['username'];
	$clientsDetails = $params['clientsdetails'];
	$userId = $clientsDetails['userid'];
	$serviceid = $params['serviceid'];
	
	try
	{
		// Create the SolidCP Enterprise Server Client object instance
		$scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
		
		// Get the user's details from SolidCP - We need the userid
		$user = $scp->getUserByUsername($username);
		if (empty($user))
		{
			throw new Exception("User {$username} does not exist - Cannot suspend account for unknown user");
		}
		
		// Change the user's account and package account status
		$result = $scp->changeUserStatus($user['UserId'], SolidCP_EnterpriseServer::USERSTATUS_SUSPENDED);
		if ($result < 0)
		{
			// Something went wrong
			throw new Exception('Fault: ' . SolidCP_EnterpriseServer::getFriendlyError($result), $result);
		}
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $result);
		
		// Notify success
		return 'success';
	}
	catch (Exception $e)
	{
		// Error message to log / return
		$errorMessage = "SuspendAccount Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";
		
		// Log to WHMCS
		logactivity($errorMessage, $userId);
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());
		
		// Notify failure - Houston we have a problem!
		return $errorMessage;
	}
}

/**
 * Unsuspends the SolidCP user account and package
 *
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return string
 */
function SolidCP_UnsuspendAccount($params)
{
        $solidcp_settings = new solidcp_settings;
        $solidcp_settings->getSettings();
        if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return "The first configuration of the SolidCP Module need to be performed. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";
        if($solidcp_settings->settings['NeedMigration'] == 1) return "A migration from an old to this new module need to be performed first. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";

	// WHMCS server parameters & package parameters
	$serverUsername = $params['serverusername'];
	$serverPassword = $params['serverpassword'];
	$serverPort = $params['serverport'];
	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
	$serverSecure = $params['serversecure'];
	$username = $params['username'];
	$clientsDetails = $params['clientsdetails'];
	$userId = $clientsDetails['userid'];
	$serviceid = $params['serviceid'];
	
	try
	{
		// Create the SolidCP Enterprise Server Client object instance
		$scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
	
		// Get the user's details from SolidCP - We need the userid
		$user = $scp->getUserByUsername($username);
		if (empty($user))
		{
			throw new Exception("User {$username} does not exist - Cannot unsuspend account for unknown user");
		}
	
		// Change the user's account and package account status
		$result = $scp->changeUserStatus($user['UserId'], SolidCP_EnterpriseServer::USERSTATUS_ACTIVE);
		if ($result < 0)
		{
			// Something went wrong
			throw new Exception('Fault: ' . SolidCP_EnterpriseServer::getFriendlyError($result), $result);
		}
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $result);
	
		// Notify success
		return 'success';
	}
	catch (Exception $e)
	{
		// Error message to log / return
		$errorMessage = "UnsuspendAccount Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";
	
		// Log to WHMCS
		logactivity($errorMessage, $userId);
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());
	
		// Notify failure - Houston we have a problem!
		return $errorMessage;
	}
}

/**
 * Changes the SolidCP user account password
 *
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return string
 */
function SolidCP_ChangePassword($params)
{
        $solidcp_settings = new solidcp_settings;
        $solidcp_settings->getSettings();
        if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return "The first configuration of the SolidCP Module need to be performed. Please contact your hosting partner!";
        if($solidcp_settings->settings['NeedMigration'] == 1) return "A migration from an old to this new module need to be performed first. Please contact your hosting partner!";

	// WHMCS server parameters & package parameters
	$serverUsername = $params['serverusername'];
	$serverPassword = $params['serverpassword'];
	$serverPort = $params['serverport'];
	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
	$serverSecure = $params['serversecure'];
	$username = $params['username'];
	$password = $params['password'];
	$clientsDetails = $params['clientsdetails'];
	$userId = $clientsDetails['userid'];
	$serviceid = $params['serviceid'];
	
	try
	{
		// Create the SolidCP Enterprise Server Client object instance
		$scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
	
		// Get the user's details from SolidCP - We need the userid
		$user = $scp->getUserByUsername($username);
		if (empty($user))
		{
			throw new Exception("User {$username} does not exist - Cannot change account password for unknown user");
		}
	
		// Change the user's account password
		$result = $scp->changeUserPassword($user['UserId'], $password);
		if ($result < 0)
		{
			// Something went wrong
			throw new Exception('Fault: ' . SolidCP_EnterpriseServer::getFriendlyError($result), $result);
		}
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $result);
                
                $command = "updateclientproduct";
                $values["serviceid"] = $serviceid;
                $values["servicepassword"] = $password;
                $result = localAPI($command, $values, $solidcp_settings->settings['WhmcsAdmin']);
                if ($result['result']!="success") echo "An Error Occurred: ".$result['result'];
		// Notify success
		return 'success';
	}
	catch (Exception $e)
	{
		// Error message to log / return
		$errorMessage = "ChangePassword Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";
	
		// Log to WHMCS
		logactivity($errorMessage, $userId);
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());
	
		// Notify failure - Houston we have a problem!
		return $errorMessage;
	}
}

/**
 * Changes the SolidCP user hosting package
 *
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return string
 */
function SolidCP_ChangePackage($params)
{
        $solidcp_settings = new solidcp_settings;
        $solidcp_settings->getSettings();
        if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return "The first configuration of the SolidCP Module need to be performed. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";
        if($solidcp_settings->settings['NeedMigration'] == 1) return "A migration from an old to this new module need to be performed first. Please go to 'Addons' -> 'SolidCP Module' in the Admin Panel!";

        // WHMCS server parameters & package parameters
	$serverUsername = $params['serverusername'];
	$serverPassword = $params['serverpassword'];
	$serverPort = $params['serverport'];
	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
	$serverSecure = $params['serversecure'];
	$username = $params['username'];
	$clientsDetails = $params['clientsdetails'];
	$userId = $clientsDetails['userid'];
	$serviceid = $params['serviceid'];
	$domain = $params['domain'];
	
	// SolidCP API parameters
	$planId = $params['configoption4'];
	$packageName = $params['configoption1'];
	
	try
	{
		// Create the SolidCP Enterprise Server Client object instance
		$scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
	
		// Get the user's details from SolidCP - We need the userid
		$user = $scp->getUserByUsername($username);
		if (empty($user))
		{
			throw new Exception("User {$username} does not exist - Cannot change package for unknown user");
		}

                // Get the user's package details from SolidCP - We need the PackageId
		$package = $scp->getUserPackages($user['UserId']);
                
                // Delete all existing Addons.
                $addons_xml = $scp->getPackageAddons($package['PackageId']);
                $xml    = str_replace(array("diffgr:","msdata:"),'', $addons_xml["any"]);
                $xml    = "<package>".$xml."</package>";
                $data   = simplexml_load_string($xml);
                $result = $data->xpath('//PackageAddonID');
                $i=0;
                while(list(,$node) = each($result)) {
                    $addons[$i] = (string) $node;
                    $i++;
                }
                foreach($addons as $value){
                    $result = $scp->deletePackageAddon($value);
                }

                // Create configurable options as addons.
                if($params['configoptions'] && $solidcp_settings->settings['ConfigurableOptionsActive'] == 1){
                    // Get the associated SolidCP addon id to the configurable option
                    $configurableoptions = solidcp_database::getServiceConfigurableOptions($serviceid);
                    if(is_array($configurableoptions) && $configurableoptions['status']=='error'){
                        throw new Exception($configurableoptions['description']);
                    }

                    if(count($configurableoptions)>0){
                        $package = $scp->getUserPackages($user['UserId']);
                        $packageId = $package['PackageId'];
                        foreach ($configurableoptions as $addon){
                            // WHMCS stores all (also previous) config options in the database, even
                            // if the package was changed and the new package doesn't include the same
                            // options. This is needed to avoid "old" addons to be added to Hosting Package.
                            $is_still_active = false;
                            foreach($params['configoptions'] as $key => $value){
                                if($key == $addon->optionname){ 
                                    $is_still_active = true;
                                }
                            }
                            // end of error handling on product change.
                            if($is_still_active){
                                $addonPlanId = $addon->scp_id;
                                $addonIsIpAddress = $addon->is_ipaddress;
                                if($addon->qty < $addon->qtyminimum) $addonqty = $addon->qtyminimum; // if < minimum qty
                                else $addonqty = $addon->qty; // from DB
                                $addonOptType = $addon->optiontype;
                                // If Optiontype is quantity or yes/no then 0 is allowed. Otherwise quantity will be 1.
                                if($addonqty == 0 && ($addonOptType == 3 || $addonOptType == 4)) continue;
                                elseif($addonOptType != 3 && $addonOptType != 4) $addonqty = 1;
                                // Add the Addon Plan to the customer's SolidCP package / hosting space
                                $results = $scp->addPackageAddonById($packageId, $addonPlanId, $addonqty);

                                // Check the results to verify that the addon has been successfully allocated
                                if ($results['Result'] > 0)
                                {
                                    // If this addon is an IP address addon - attempt to randomly allocate an IP address to the customer's hosting space
                                    if ($addonIsIpAddress == 1)
                                    {
                                        $scp->allocatePackageIPAddresses($packageId);
                                    }

                                    // Add log entry to client log
                                    logactivity("SolidCP Addon - Account {$username} addon successfully completed - Addon ID: {$addonId}", $userId);
                                }
                                else
                                {
                                    // Add log entry to client log
                                    throw new Exception("Unknown", $results['Result']);
                                }
                            }
                        }
                    }
                }

                // Create WHMCS addons as addons.
                if($solidcp_settings->settings['AddonsActive'] == 1){
                    $addons = solidcp_database::getServiceAddons($serviceid);
                    if(is_array($addons) && $addons['status']=='error'){
                        throw new Exception($addons['description']);
                    }
                    if(count($addons) > 0){
                        $package = $scp->getUserPackages($user['UserId']);
                        $packageId = $package['PackageId'];
                        foreach ($addons as $addon){
                            $addonPlanId = $addon->scp_id;
                            $addonIsIpAddress = $addon->is_ipaddress;
                            $results = $scp->addPackageAddonById($packageId, $addonPlanId);

                            // Check the results to verify that the addon has been successfully allocated
                            if ($results['Result'] > 0)
                            {
                                // If this addon is an IP address addon - attempt to randomly allocate an IP address to the customer's hosting space
                                if ($addonIsIpAddress == 1)
                                {
                                    $scp->allocatePackageIPAddresses($packageId);
                                }

                                // Add log entry to client log
                                logactivity("SolidCP Addon - Account {$username} addon successfully completed - Addon ID: {$addonId}", $userId);
                            }
                            else
                            {
                                // Add log entry to client log
                                throw new Exception("Unknown", $results['Result']);
                            }
                            
                        }
                    }
                }
                
                // Update the user's SolidCP package
		$result = $scp->updatePackageLiteral($package['PackageId'], $package['StatusId'], $planId, $package['PurchaseDate'], $packageName, $package['PackageComments']);
		if ($result < 0)
		{
			// Something went wrong
			throw new Exception('Fault: ' . SolidCP_EnterpriseServer::getFriendlyError($result), $result);
		}

		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $result);

		// Notify success
		return 'success';
	}
	catch (Exception $e)
	{
		// Error message to log / return
		$errorMessage = "ChangePackage Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";
	
		// Log to WHMCS
		logactivity($errorMessage, $userId);
		
		// Log the module call
		SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());
	
		// Notify failure - Houston we have a problem!
		return $errorMessage;
	}
}

/**
 * Updates the WHMCS service's usage details from SolidCP
 * 
 * @param aray $params WHMCS parameters
 * @throws Exception
 */
function SolidCP_UsageUpdate($params)
{
    $solidcp_settings = new solidcp_settings;
    $solidcp_settings->getSettings();
    if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return false;
    if($solidcp_settings->settings['NeedMigration'] == 1) return false;

    // WHMCS server parameters & package parameters
    $serverUsername = $params['serverusername'];
    $serverPassword = $params['serverpassword'];
    $serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
    $serverPort = $params['serverport'];
    $serverSecure = $params['serversecure'];
    $serverid = $params['serverid'];
    $userId = 0;
    $serviceid = 0;

    try
    {

        // Query for SolidCP user accounts assigned to this server
        // Only services that have packages that have "Tick to update diskpace / bandwidth in WHMCS" enabled
        $services = solidcp_database::getUsageUpdateServices($serverid);
        if(is_array($services) && $services['status']=='error'){
            throw new Exception($services['description']);
        }
        if(count($services) > 0){
            foreach($services as $service){
                // Start processing the users usage
                $username = $service->username;
                $userId = $service->userid;
                $serviceid = $service->serviceid;

                // Diskspace and Bandwidth limits for this package
                $disklimit = $service->configoption2;
                $bwidthlimit = $service->configoption3;

                // Create the SolidCP Enterprise Server Client object instance
                $scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);

                // Get the user's details from SolidCP - We need the userid
                $user = $scp->getUserByUsername($username);
                if (empty($user))
                {
                        throw new Exception("User {$username} does not exist - Cannot calculate usage for unknown user");
                }

                // Get the user's package details from SolidCP - We need the PackageId
                $package = $scp->getUserPackages($user['UserId']);

                // Gather the bandwidth / diskspace usage stats
				// WHMCS bills for overages on calendar month so date range should reflect calendar month
                $bwidthusage = SolidCP_CalculateUsage(
					$scp->getPackageBandwidthUsage(
						$package['PackageId'], 
						date('Y-m-01'), 
						date('Y-m-t')), 
					SolidCP_EnterpriseServer::USAGE_BANDWIDTH);
                
				$diskusage = SolidCP_CalculateUsage($scp->getPackageDiskspaceUsage($package['PackageId']), SolidCP_EnterpriseServer::USAGE_DISKSPACE);

                // Update WHMCS's service details
                solidcp_database::setUsage($serviceid, $diskusage, $disklimit, $bwidthusage, $bwidthlimit);
                // Log the module call
                SolidCP_logModuleCall(__FUNCTION__, $params, $package);
            }
        }
    }
    catch (Exception $e)
    {
            // Error message to log / return
            $errorMessage = "UsageUpdate Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";

            // Log the module call
            SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());

            // Log to WHMCS
            logactivity($errorMessage, $userId);
    }

}

/**
 * Returns the SolidCP one-click login link
 *
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return string
 */
function SolidCP_LoginLink($params)
{
    $solidcp_settings = new solidcp_settings;
    $solidcp_settings->getSettings();
    if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return false;
    if($solidcp_settings->settings['NeedMigration'] == 1) return false;

    // WHMCS does not return the full hosting account details, we will query for what we need
    $service = solidcp_database::getService($params['serviceid']);
        
    // Display the link only if the account is Active or Suspended
    if (in_array($service->domainstatus, array('Active', 'Suspended')))
    {
        // WHMCS server parameters & package parameters
    	$serverUsername = $params['serverusername'];
    	$serverPassword = $params['serverpassword'];
    	$serverPort = $params['serverport'];
    	$serverHost = (empty($params['serverhostname']) ? $params['serverip'] : $params['serverhostname']);
    	$serverSecure = $params['serversecure'];
    	$username = $params['username'];
    	$serviceid = $params['serviceid'];
    	$clientsDetails = $params['clientsdetails'];
    	$userId = $clientsDetails['userid'];
    	
    	try
    	{
    		// Create the SolidCP Enterprise Server Client object instance
    		$scp = new SolidCP_EnterpriseServer($serverUsername, $serverPassword, $serverHost, $serverPort, $serverSecure);
    	
    		// Get the user's details from SolidCP - We need the userid
    		$user = $scp->getUserByUsername($username);
    		if (empty($user))
    		{
    			throw new Exception("User {$username} does not exist - Cannot display account link for unknown user");
    		}
    		
    		// Load the client area language file
    		$LANG = SolidCP_LoadClientLanguage();
    		
    		// Print the link
    		echo "<a href=\"{$params['configoption7']}/Default.aspx?pid=Home&UserID={$user['UserId']}\" target=\"_blank\" title=\"{$LANG['SolidCP_adminarea_gotoSolidCPaccount']}\">{$LANG['SolidCP_adminarea_gotoSolidCPaccount']}</a><br />";
    		
    		// Log the module call
    		SolidCP_logModuleCall(__FUNCTION__, $params, $user);
    	}
    	catch (Exception $e)
    	{
    		// Error message to log / return
    		$errorMessage = "LoginLink Fault: (Code: {$e->getCode()}, Message: {$e->getMessage()}, Service ID: {$serviceid})";
    	
    		// Log to WHMCS
    		logactivity($errorMessage, $userId);
    		
    		// Log the module call
    		SolidCP_logModuleCall(__FUNCTION__, $params, $e->getMessage());
    	}
    }
}

/**
 * Client Area module output for the customer's "My Services" service output
 * 
 * @access public
 * @param array $params WHMCS parameters
 * @throws Exception
 * @return array
 */
function SolidCP_ClientArea($params)
{
    $solidcp_settings = new solidcp_settings;
    $solidcp_settings->getSettings();
    if($solidcp_settings->settings['NeedFirstConfiguration'] == 1) return false;
    if($solidcp_settings->settings['NeedMigration'] == 1) return false;

    // WHMCS server parameters & package parameters
    $username = $params['username'];
    $password = $params['password'];
    
    // Load the client area language file
    SolidCP_LoadClientLanguage();
    
    // Return template information
    return array('templatefile' => '/templates/clientarea.tpl', 'vars' => array('SolidCP_url' => $params['configoption7'], 'username' => $username, 'password' => $password));
}

/**
 * Logs all module calls to the WHMCS module debug logger
 *
 * @access public
 * @param string $function
 * @param mixed $params
 * @param mixed $response
 */
function SolidCP_logModuleCall($function, $params, $response)
{
    // Get the module name
    $callerData = explode('_', $function);
    $module = $callerData[0];
    $action = $callerData[1];

    // Replacement variables
    $replacementVars = array('');

    // Log the call with WHMCS
    logModuleCall($module, $action, $params, $response, '', $replacementVars);
}

/**
* Open an URL, needed for Mailcleaner API calls
* 
* @access public
* @param str $url
* @return  string
*/
function SolidCP_callExtApiUrl($url) {
    $mc_call = curl_init();
    $timeout = 30;
    curl_setopt($mc_call, CURLOPT_URL, $url);
    curl_setopt($mc_call, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($mc_call, CURLOPT_CONNECTTIMEOUT, $timeout);
    $data = curl_exec($mc_call);
    curl_close($mc_call);
    return $data;
}
