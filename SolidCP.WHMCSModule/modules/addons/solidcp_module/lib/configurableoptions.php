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
 * SolidCP configurable options class
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

Class solidcp_configurableoptions{
    
    public $configurableoptions = NULL;
    
    public function getConfigurableOptions(){
        try{
            $this->configurableoptions = Capsule::select("select concat(g.name, ' -> ', o.optionname, ' -> ', os.optionname) as name, c.whmcs_id as whmcs_id, c.scp_id as scp_id, os.hidden as hidden from ".SOLIDCP_CONFIGURABLE_OPTIONS_TABLE." as c
left join tblproductconfigoptionssub as os on c.whmcs_id=os.id
left join tblproductconfigoptions as o on os.configid=o.id
left join tblproductconfiggroups as g on o.gid=g.id
order by g.name,  o.`order`, os.sortorder");
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't read the SolidCP configurable options: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
        return array('status' => 'success', 'description' => "SolidCP configurable options were read successfully.");
    }
    
    public function setConfigurableOption($new){
        if($new['is_ipaddress']=="on") $new['is_ipaddress'] = 1;
        else $new['is_ipaddress'] = 0;
        try{
            Capsule::table(SOLIDCP_CONFIGURABLE_OPTIONS_TABLE)->insert(
                    ['whmcs_id' => $new['whmcs_id'], 'scp_id' => $new['scp_id'],'is_ipaddress' => $new['is_ipaddress'], 'created_at' => date('Y-m-d H:i:s')]
                    );
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't write the SolidCP configurable options: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
        return array('status' => 'success', 'description' => "SolidCP configurable option was written successfully.");
    }
    
    public function deleteConfigurableOption($whmcs_id){
        $whmcs_id = strip_tags($whmcs_id);
        try{
            Capsule::table(SOLIDCP_CONFIGURABLE_OPTIONS_TABLE)
                    ->where('whmcs_id', $whmcs_id)
                    ->delete();
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't delete the SolidCP configurable option: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
        return array('status' => 'success', 'description' => "SolidCP configurable option was deleted successfully.");
    }

}