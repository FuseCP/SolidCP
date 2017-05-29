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
 * SolidCP database model
 * 
 * @author SolidCP
 * @link https://solidcp.com/
 * @access public
 * @name SolidCP
 * @version 1.1.2
 * @package WHMCS
 * @final
 */

require_once (ROOTDIR. '/modules/addons/solidcp_module/lib/var_definition.php');

use Illuminate\Database\Capsule\Manager as Capsule;

Class solidcp_database{

    /**
     * Creates the settings table in the WHMCS database
     * @return array 
     */
    public static function createSettingsTable(){
        try{
            if(!Capsule::schema()->hasTable(SOLIDCP_SETTINGS_TABLE)){
                Capsule::schema()->create(SOLIDCP_SETTINGS_TABLE,
                    function ($table){
                        $table->engine='InnoDB';
                        $table->text('setting');
                        $table->text('value')->nullable();
                        $table->timestamps();
                    }
                );

                Capsule::statement('ALTER TABLE '.SOLIDCP_SETTINGS_TABLE.' ADD INDEX setting(setting(32));');

                Capsule::table(SOLIDCP_SETTINGS_TABLE)->insert([
                    ['setting' => 'NeedFirstConfiguration', 'value' => '1', 'created_at' => date('Y-m-d H:i:s')],
                    ['setting' => 'NeedMigration', 'value' => '0', 'created_at' => date('Y-m-d H:i:s')],
                    ['setting' => 'DeleteTablesOnDeactivate', 'value' => '0', 'created_at' => date('Y-m-d H:i:s')],
                    ['setting' => 'AddonsActive', 'value' => '0', 'created_at' => date('Y-m-d H:i:s')],
                    ['setting' => 'ConfigurableOptionsActive', 'value' => '0', 'created_at' => date('Y-m-d H:i:s')],
                    ['setting' => 'SyncActive', 'value' => '1', 'created_at' => date('Y-m-d H:i:s')],
                    ['setting' => 'WhmcsAdmin', 'value' => '', 'created_at' => date('Y-m-d H:i:s')]
                ]);

                return array('status' => 'success', 'description' => "SolidCP settings table successfully created.");
            }
            else{
                return array('status' => 'success', 'description' => "SolidCP settings table already exists.");
            }
        }
        catch (Exception $e){
            Capsule::schema()->dropIfExists(SOLIDCP_SETTINGS_TABLE);
            return array('status' => 'error', 'description' => "Couldn't create settings table in database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }
    
        /**
     * Creates the Addons table in the WHMCS database
     * @return array 
     */
    public static function createAddonsTable(){
        try{
            if(!Capsule::schema()->hasTable(SOLIDCP_ADDONS_TABLE)){
                Capsule::schema()->create(SOLIDCP_ADDONS_TABLE,
                    function ($table){
                        $table->engine='InnoDB';
                        $table->integer('whmcs_id')->primary();
                        $table->integer('scp_id');
                        $table->boolean('is_ipaddress')->default(0);
                        $table->text('ipadress_type')->nullable();
                        $table->timestamps();
                    }
                );

                return array('status' => 'success', 'description' => "SolidCP Addons table successfully created.");
            }
            else{
                return array('status' => 'success', 'description' => "SolidCP Addons table already exists.");
            }
        }
        catch (Exception $e){
            Capsule::schema()->dropIfExists(SOLIDCP_ADDONS_TABLE);
            return array('status' => 'error', 'description' => "Couldn't create Addons table in database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }
    
    /**
     * Creates the configurable options table in the WHMCS database
     * @return array 
     */
    public static function createConfigurableOptionsTable(){
        try{
            if(!Capsule::schema()->hasTable(SOLIDCP_CONFIGURABLE_OPTIONS_TABLE)){
                Capsule::schema()->create(SOLIDCP_CONFIGURABLE_OPTIONS_TABLE,
                    function ($table){
                        $table->engine='InnoDB';
                        $table->integer('whmcs_id')->primary();
                        $table->integer('scp_id');
                        $table->boolean('is_ipaddress')->default(0);
                        $table->text('ipadress_type')->nullable();
                        $table->timestamps();
                    }
                );

                return array('status' => 'success', 'description' => "SolidCP configurable options table successfully created.");
            }
            else{
                return array('status' => 'success', 'description' => "SolidCP configurable options table already exists.");
            }
        }
        catch (Exception $e){
            Capsule::schema()->dropIfExists(SOLIDCP_CONFIGURABLE_OPTIONS_TABLE);
            return array('status' => 'error', 'description' => "Couldn't create configrable options table in database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }
    
    /**
     * Delete the settings table from the WHMCS database
     * @return array 
     */
    public static function deleteSettingsTable(){
        try{
            Capsule::schema()->dropIfExists(SOLIDCP_SETTINGS_TABLE);
            return array('status' => 'success', 'description' => "SolidCP settings table successfully deleted.");
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't delete settings table from database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }
    
    /**
     * Delete the Addons table from the WHMCS database
     * @return array 
     */
    public static function deleteAddonsTable(){
        try{
            Capsule::schema()->dropIfExists(SOLIDCP_ADDONS_TABLE);
            return array('status' => 'success', 'description' => "SolidCP Addons table successfully deleted.");
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't delete Addons table from database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }
    
    /**
     * Delete the configurable options table from the WHMCS database
     * @return array 
     */
    public static function deleteConfigurableOptionsTable(){
        try{
            Capsule::schema()->dropIfExists(SOLIDCP_CONFIGURABLE_OPTIONS_TABLE);
            return array('status' => 'success', 'description' => "SolidCP configurable options table successfully deleted.");
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't delete configrable options table from database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Get an Addon with a specified ID from the WHMCS SolidCP Addons table
     * @return StdClass object 
     */
    public static function getSCPAddon($addonid){
        try{
            $addon = Capsule::table(SOLIDCP_ADDONS_TABLE)->where('whmcs_id', $addonid)->first();
            return $addon;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get the addon from addons table from database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Get hosting with a specified ID from the WHMCS table
     * @return StdClass object 
     */
    public static function getService($serviceid){
        try{
            $service = Capsule::table('tblhosting')->where('id', $serviceid)->first();
            return $service;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get the service from database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }
    
    /**
     * Get an Admin language from a specified Admin ID from the WHMCS database
     * @return StdClass object 
     */
    public static function getAdminLanguage($adminid){
        try{
            $admin = Capsule::table('tbladmins')->where('id', $adminid)->first();
            return $admin;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get the admin from WHMCS database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Get all SolidCP accounts of a user with a specified user ID from the WHMCS database
     * @return StdClass object 
     * 
     */
    public static function getUserSCPAccounts($userid){
        try{
            $scpaccounts = Capsule::select("
                SELECT 
                    h.username AS username,
                    s.ipaddress AS serverip,
                    s.hostname AS serverhostname,
                    s.secure AS serversecure,
                    s.username AS serverusername,
                    s.password AS serverpassword,
                    s.port AS serverport,
                    h.id AS serviceid
                FROM
                    `tblhosting` AS h,
                    `tblservers` AS s,
                    `tblproducts` AS p
                WHERE
                    h.userid = ".$userid."
                    AND h.packageid = p.id
                    AND h.server = s.id
                    AND s.type = 'SolidCP'
                    AND h.domainstatus IN ('Active', 'Suspended')
            ");
            return $scpaccounts;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get SolidCP accounts for Userid {$userid} from WHMCS database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Get the server account of a user for which an addon should be activated.
     * Need service ID and addon ID (after it's already saved to the WHMCS database).
     * @return StdClass object 
     * 
     */
    public static function getAddonActivationSCPAccount($serviceid, $addonid){
        try{
            $scpaccounts = Capsule::select("
                SELECT
                    h.username AS username,
                    s.ipaddress AS serverip,
                    s.hostname AS serverhostname,
                    s.secure AS serversecure,
                    s.username AS serverusername,
                    s.password AS serverpassword,
                    h.id AS serviceid
                FROM
                    `tblhosting` AS h,
                    `tblservers` AS s,
                    `tblproducts` AS p,
                    `".SOLIDCP_ADDONS_TABLE."` AS w
                WHERE
                    h.packageid = p.id
                    AND w.whmcs_id = ".$addonid."
                    AND h.id = ".$serviceid."
                    AND h.server = s.id
                    AND s.type = 'SolidCP'
            ");
            if(count($scpaccounts)>0) return $scpaccounts[0];
            else return $scpaccounts;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get SolidCP accounts for Userid {$userid} from WHMCS database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Get the configurable options for a service specified with a service id.
     * @return StdClass object 
     * 
     */
    public static function getServiceConfigurableOptions($serviceid){
        try{
            $configurableoptions = Capsule::select("
                SELECT
                    c.scp_id as scp_id,
                    o.qty as qty,
                    c.is_ipaddress as is_ipaddress,
                    co.optiontype as optiontype,
                    co.optionname as optionname,
                    co.qtyminimum as qtyminimum
                FROM
                    tblhostingconfigoptions as o
                INNER JOIN
                    solidcp_configurable as c on o.optionid = c.whmcs_id
                LEFT JOIN
                    tblproductconfigoptions as co on o.configid = co.id
                WHERE 
                    o.relid = ".$serviceid."
            ");
            return $configurableoptions;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get configurable options for ServiceID {$serviceid} from WHMCS database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Get the addons for a service specified with a service id.
     * @return StdClass object 
     * 
     */
    public static function getServiceAddons($serviceid){
        try{
            $addons = Capsule::select("
                SELECT
                    s.scp_id as scp_id,
                    s.is_ipaddress as is_ipaddress
                FROM
                    tblhostingaddons AS a,
                    ".SOLIDCP_ADDONS_TABLE." as s
                WHERE 
                    a.addonid = s.whmcs_id
                    AND a.hostingid = ".$serviceid."
            ");
            return $addons;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get addons for ServiceID {$serviceid} from WHMCS database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Get the services on a server that have packages that have "Tick to update diskpace / bandwidth in WHMCS" enabled.
     * Need a server ID.
     * @return StdClass object 
     * 
     */
    public static function getUsageUpdateServices($serverid){
        try{
            $services = Capsule::select("
                SELECT
                    h.id AS serviceid,
                    h.userid AS userid,
                    h.username AS username,
                    h.regdate AS regdate,
                    p.configoption2 AS configoption2,
                    p.configoption3 AS configoption3
                FROM
                    `tblhosting` AS h,
                    `tblproducts` AS p
                WHERE
                    h.server = ".$serverid."
                    AND h.packageid = p.id
                    AND p.configoption15 = 'on'
                    AND h.domainstatus IN ('Active', 'Suspended')
            ");
            return $services;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't get services for ServerID {$serverid} from WHMCS database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

    /**
     * Set bandwith and disk usage for a WHMCS service
     * @return null 
     */
    public static function setUsage($serviceid, $diskusage, $disklimit, $bwidthUsage, $bwidthLimit){
        try{
            Capsule::table('tblhosting')
                ->where('id', $serviceid)
                ->update(['diskusage' => $diskusage,
                    'disklimit' => $disklimit,
                    'bwusage' => $bwidthUsage,
                    'bwlimit' => $bwidthLimit,
                    'lastupdate' => Capsule::raw('now()')])
                ;
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't set the usage in WHMCS database: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
    }

}