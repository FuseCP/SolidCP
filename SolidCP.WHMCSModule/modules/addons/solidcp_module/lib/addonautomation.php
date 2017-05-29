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
 * SolidCP addon automation class
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

Class solidcp_addonautomation{
    
    public $addonautomation = NULL;
    
    public function getAddonAutomation(){
        try{
            $this->addonautomation = Capsule::select("SELECT a.name AS 'name', w.whmcs_id AS 'whmcs_id', w.scp_id AS 'scp_id' FROM ".SOLIDCP_ADDONS_TABLE." AS w"
                    . " left join tbladdons as a on w.whmcs_id = a.id"
                    . " order by a.name");
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't read the SolidCP Addons: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
        return array('status' => 'success', 'description' => "SolidCP Addons were read successfully.");
    }
    
    public function setAddonAutomation($new){
        if($new['is_ipaddress']=="on") $new['is_ipaddress'] = 1;
        else $new['is_ipaddress'] = 0;
        try{
            Capsule::table(SOLIDCP_ADDONS_TABLE)->insert(
                    ['whmcs_id' => $new['whmcs_id'], 'scp_id' => $new['scp_id'],'is_ipaddress' => $new['is_ipaddress'], 'created_at' => date('Y-m-d H:i:s')]
                    );
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't write the SolidCP Addon automation: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
        return array('status' => 'success', 'description' => "SolidCP Addon automationwas written successfully.");
    }
    
    public function deleteAddonAutomation($whmcs_id){
        $whmcs_id = strip_tags($whmcs_id);
        try{
            Capsule::table(SOLIDCP_ADDONS_TABLE)
                    ->where('whmcs_id', $whmcs_id)
                    ->delete();
        }
        catch (Exception $e){
            return array('status' => 'error', 'description' => "Couldn't delete the SolidCP Addon automation: (Code: {$e->getCode()}, Message: {$e->getMessage()}");
        }
        return array('status' => 'success', 'description' => "SolidCP Addon automation was deleted successfully.");
    }

}