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
 * SolidCP WHMCS Server Module Client Area Language
 *
 * @author SolidCP
 * @link https://solidcp.com/
 * @access public
 * @name SolidCP
 * @version 1.1.2
 * @package WHMCS
 */
$_ADDONLANG['SolidCP_migration'] = 'Migration';
$_ADDONLANG['SolidCP_settings'] = 'Settings';
$_ADDONLANG['SolidCP_addon_automation'] = 'Addon Automation';
$_ADDONLANG['SolidCP_configurable_options'] = 'Configurable Options';
$_ADDONLANG['SolidCP_sync_automation'] = 'Sync Automation';
$_ADDONLANG['SolidCP_close'] = 'Close';
$_ADDONLANG['SolidCP_yes'] = 'Yes';
$_ADDONLANG['SolidCP_no'] = 'No';
$_ADDONLANG['SolidCP_action'] = 'Action';
$_ADDONLANG['SolidCP_checkagain'] = 'Check again!';
$_ADDONLANG['SolidCP_addonsnotactive'] = 'Addon automation is not active. Please activate it on the "Setting" page first.';
$_ADDONLANG['SolidCP_configurablenotactive'] = 'Configurable options automation is not active. Please activate it on the "Setting" page first.';
$_ADDONLANG['SolidCP_syncnotactive'] = 'Sync automation is not active. Please activate it on the "Setting" page first.';
$_ADDONLANG['SolidCP_sync_nosetting'] = 'Sync automation has no settings right now';

$_ADDONLANG['SolidCP_migration_needed'] = 'A migration is needed, before you can continue to use this module!';
$_ADDONLANG['SolidCP_migrateTable_text'] = 'Copy database table content from "%s" to "%s".';
$_ADDONLANG['SolidCP_migrateDbValues_text'] = 'Value "%s" from table "%s" need to be migrated due to structure changes.';
$_ADDONLANG['SolidCP_deactivateModules_text'] = 'The module "%s" needs to be manually deactivated after the previous steps are done!';
$_ADDONLANG['SolidCP_deleteFiles_text'] = 'Delete file "%s" manually via FTP after all previous steps are done!';
$_ADDONLANG['SolidCP_confirmdelete'] = 'Confirm deletion';
$_ADDONLANG['SolidCP_confirmdelete_long'] = 'Are you sure, that you want to delete this table? All values inside this table will be deleted. This action is irreversible after your confirmation!';
$_ADDONLANG['SolidCP_copytable'] = 'Copy old table "%s" to new "%s"';
$_ADDONLANG['SolidCP_deletetable'] = 'Delete old "%s" table (not recommended)';
$_ADDONLANG['SolidCP_migratedbvalues'] = 'Migrate values in table "%s" to new structure';
$_ADDONLANG['SolidCP_deactivatemodule'] = 'Goto "System" -> "Addon Module"';
$_ADDONLANG['SolidCP_confirmmigrate'] = 'Confirm migration';
$_ADDONLANG['SolidCP_confirmmigrate_long'] = 'Are you sure, that you want to migrate this database table? There is no way back to use the old module anymore. Please create a backup of this table before strarting the migration. This action is irreversible after your confirmation!';
$_ADDONLANG['SolidCP_migrationrunning'] = 'Migration task is running';
$_ADDONLANG['SolidCP_saverunning'] = 'Save task is running';
$_ADDONLANG['SolidCP_needfirstconfiguration'] = 'This is the first module configuration. Please adjust and save your settings!';
$_ADDONLANG['SolidCP_norecordsfound'] = 'No records found';

$_ADDONLANG['SolidCP_configurableoptionname'] = 'Configurable Option Name';
$_ADDONLANG['SolidCP_addonname'] = 'Addon Name';
$_ADDONLANG['SolidCP_whmcs_id'] = 'WHMCS ID';
$_ADDONLANG['SolidCP_solidcp_id'] = 'SolidCP ID';
$_ADDONLANG['SolidCP_hidden'] = 'Hidden';
$_ADDONLANG['SolidCP_delete'] = 'Delete';
$_ADDONLANG['SolidCP_addassignment'] = 'Add new assignment';
$_ADDONLANG['SolidCP_add_configurable_option'] = 'Add a new configurable option assignment';
$_ADDONLANG['SolidCP_add_addon_option'] = 'Add a new Addon assignment';
$_ADDONLANG['SolidCP_whmcs_id_tooltip'] = 'Fill the WHMCS-ID from the database.';
$_ADDONLANG['SolidCP_solidcp_id_tooltip'] = 'Fill the SolidCP Addon-ID';
$_ADDONLANG['SolidCP_is_ip_address'] = 'Is IP address';
$_ADDONLANG['SolidCP_is_ip_address_tooltip'] = 'Should a new dedicated IP address be assigned in SolidCP?';

$_ADDONLANG['SolidCP_general_settings'] = 'General Settings';
$_ADDONLANG['SolidCP_setting_AddonsActive'] = 'Addon Automation active';
$_ADDONLANG['SolidCP_setting_AddonsActive_tooltip'] = 'Addon provisioning will be automated if this option is checked. Add entries to the Addon Automation tab in order to get it working.';
$_ADDONLANG['SolidCP_setting_ConfigurableOptionsActive'] = 'Configurable Options active';
$_ADDONLANG['SolidCP_setting_ConfigurableOptionsActive_tooltip'] = 'Configurable options provisioning will be automated if this option is checked and a valid license is found. Add entries to the Configurable Options tab in order to get it working.';
$_ADDONLANG['SolidCP_setting_SyncActive'] = 'Sync Automation active';
$_ADDONLANG['SolidCP_setting_SyncActive_tooltip'] = 'Client details will be synced automatically to SolidCP account if changes in WHMCS are made.';
$_ADDONLANG['SolidCP_setting_DeleteTablesOnDeactivate'] = 'Delete DB tables on deactivation';
$_ADDONLANG['SolidCP_setting_DeleteTablesOnDeactivate_tooltip'] = 'Database tables will be deleted, if module deactivation will be performed and this option is checked.';
$_ADDONLANG['SolidCP_setting_WhmcsAdmin'] = 'WHMCS admin for API calls';
$_ADDONLANG['SolidCP_setting_WhmcsAdmin_tooltip'] = 'WHMCS admin user, who will be used for internal API calls from client area scripts (e.g. for changing product password).';

$_ADDONLANG['SolidCP_save_changes'] = 'Save Changes';
$_ADDONLANG['SolidCP_cancel_changes'] = 'Cancel Changes';
$_ADDONLANG['SolidCP_add_new'] = 'Add new';
$_ADDONLANG['SolidCP_cancel'] = 'Cancel';
$_ADDONLANG['SolidCP_saving'] = 'Saving in progress';