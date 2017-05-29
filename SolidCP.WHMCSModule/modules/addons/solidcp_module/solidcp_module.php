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
 * SolidCP Enterprise Server Client
 * 
 * @author SolidCP
 * @link https://solidcp.com/
 * @access public
 * @name SolidCP
 * @version 1.1.2
 * @package WHMCS
 * @final
 */
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/addonautomation.php');
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/configurableoptions.php');
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/database.php');
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/migration.php');
require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/settings.php');

/**
 * solidcp_module_config
 *
 * @access public
 * @return array
 */
function solidcp_module_config()
{
    return array('name' => 'SolidCP Module',
                 'description' => 'SolidCP Module for automating product configurable options, addons and sync to SolidCP',
                 'version' => '1.1.2',
                 'author' => '<a href="https://solidcp.com/" target="_blank">SolidCP</a>',
                 'language' => 'english');
}

/**
 * solidcp_module_activate
 *
 * @access public
 * @return array
 */
function solidcp_module_activate()
{
    // Create the SolidCP Module settings table
    $e = solidcp_database::createSettingsTable();
    if($e['status']!='success') return $e;

    // Create the SolidCP Addon table
    $e = solidcp_database::createAddonsTable();
    if($e['status']!='success') return $e;
    
    // Create the SolidCP Configurable Options table
    $e = solidcp_database::createConfigurableOptionsTable();
    if($e['status']!='success') return $e;
    
    return array('status' => 'success', 'description' => 'The module has been activated successfully');
}

/**
 * solidcp_module_deactivate
 *
 * @access public
 * @return array
 */
function solidcp_module_deactivate()
{
    $solidcp_settings = new solidcp_settings;
    $solidcp_settings->getSettings();
    if($solidcp_settings->settings['DeleteTablesOnDeactivate'] == 1){

        // Delete the SolidCP Module settings table
        $e = solidcp_database::deleteSettingsTable();
        if($e['status']!='success') return $e;

        // Delete the SolidCP Addon table
        $e = solidcp_database::deleteAddonsTable();
        if($e['status']!='success') return $e;

        // Delete the SolidCP Configurable Options table
        $e = solidcp_database::deleteConfigurableOptionsTable();
        if($e['status']!='success') return $e;
        
        return array('status' => 'success', 'description' => 'The module has been deactivated and the tables have been deleted successfully');
    }

    return array('status' => 'success', 'description' => 'The module has been deactivated successfully. Tables were NOT deleted.');
}

/**
 * solidcp_moduleupgrade
 *
 * @param $vars array
 * @access public
 * @return array
 */
function solidcp_module_upgrade($vars)
{
    // Module versions
    $version = $vars['version'];

}

/**
 * Displays the SolidCP configurable module output
 *
 * @access public
 * @return mixed
 */
function solidcp_module_output($params)
{   
    define('DS', DIRECTORY_SEPARATOR);
    
    global $aInt, $templates_compiledir;
    $template = NULL;
    $scp_smarty = new Smarty;
    $scp_smarty->caching = false;
    $scp_smarty->compile_dir = $templates_compiledir; 
    $scp_smarty->assign('LANG',$params['_lang']);
    $scp_smarty->assign('params',$params);

    if($_POST['ajax']==1 && $_POST['module']=="solidcp_module" && $_POST['action']=="migration"){
        $aInt->adminTemplate = ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS."ajax";
        $result = startMigration($_POST['command'],$_POST['value1'],$_POST['value2'],$_POST['option']);
        $html_ids = getHTMLids($_POST['command'],$_POST['value1'],$_POST['value2']);
        $scp_smarty->assign('html_ids',$html_ids);
        $template = "ajax".DS."migration.tpl";
    }
    elseif($_POST['ajax']==1 && $_POST['module']=="solidcp_module" && $_POST['action']=="save_settings"){
        $aInt->adminTemplate = ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS."ajax";
        $solidcp_settings = new solidcp_settings;
        $solidcp_settings->getSettings();
        $result = $solidcp_settings->setSettings($_POST);
        $template = "ajax".DS."savesettings.tpl";
    }
    else{
        checkMigration();
        $solidcp_settings = new solidcp_settings;
        $result = $solidcp_settings->getSettings();
        $scp_smarty->assign('admins',$solidcp_settings->admins);
        $scp_smarty->assign('settings',$solidcp_settings->settings);
        if($solidcp_settings->settings['ConfigurableOptionsActive'] == 1){
            $solidcp_configurable = new solidcp_configurableoptions();
        }
        if($solidcp_settings->settings['AddonsActive'] == 1){
            $solidcp_addon = new solidcp_addonautomation();
        }
        if($solidcp_settings->settings['NeedMigration']){
                $scp_smarty->assign('migrationsteps',migrationSteps());
        }
        if($_POST['ajax']==1 && $_POST['module']=="solidcp_module" && $_POST['action']=="load") {
            $aInt->adminTemplate = ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS."ajax";
            $template = "admin_".str_replace("#", "", strip_tags ($_POST['area'])).".tpl";
        }
        elseif($_POST['ajax']==1 && $_POST['module']=="solidcp_module" && $_POST['action']=="add_configurable") {
            $aInt->adminTemplate = ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS."ajax";
            $result = $solidcp_configurable->setConfigurableOption($_POST);
            $template = "admin_configurable.tpl";
        }
        elseif($_POST['ajax']==1 && $_POST['module']=="solidcp_module" && $_POST['action']=="delete_configurable") {
            $aInt->adminTemplate = ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS."ajax";
            $result = $solidcp_configurable->deleteConfigurableOption($_POST['id']);
            $template = "admin_configurable.tpl";
        }
        elseif($_POST['ajax']==1 && $_POST['module']=="solidcp_module" && $_POST['action']=="add_addon") {
            $aInt->adminTemplate = ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS."ajax";
            $result = $solidcp_addon->setAddonAutomation($_POST);
            $template = "admin_addon.tpl";
        }
        elseif($_POST['ajax']==1 && $_POST['module']=="solidcp_module" && $_POST['action']=="delete_addon") {
            $aInt->adminTemplate = ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS."ajax";
            $result = $solidcp_addon->deleteAddonAutomation($_POST['id']);
            $template = "admin_addon.tpl";
        }
        else $template = "admin.tpl";
        if($solidcp_settings->settings['ConfigurableOptionsActive'] == 1){
            $result = $solidcp_configurable->getConfigurableOptions();
            $scp_smarty->assign('configurableoptions',$solidcp_configurable->configurableoptions);
        }
        if($solidcp_settings->settings['AddonsActive'] == 1){
            $result = $solidcp_addon->getAddonAutomation();
            $scp_smarty->assign('addonautomation',$solidcp_addon->addonautomation);
        }

    }
    $scp_smarty->assign('result',$result);
    $scp_smarty->display(ROOTDIR.DS."modules".DS."addons".DS."solidcp_module".DS."templates".DS.$template);
}