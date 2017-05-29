<####################################################################################################
SolidSCP - Clean IIS Logs

v1.0    8th August 2016:    First release of the SolidCP Clean IIS Logs Script

Written By Marc Banyard for the SolidCP Project (c) 2016 SolidCP

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

$dDaysToKeep  = "7" # Number of days to keep log files for

# Editable features are above this line
####################################################################################################
#
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
#                                                                                                  #
# DO NOT EDIT ANYTHING BELOW THIS LINE                                                             #
#                                                                                                  #
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
####################################################################################################
$host.UI.RawUI.BufferSize  = New-Object -TypeName System.Management.Automation.Host.Size -ArgumentList (120, 50)
$host.UI.RawUI.WindowSize  = New-Object -TypeName System.Management.Automation.Host.Size -ArgumentList (120, 50)
$Host.UI.RawUI.WindowTitle = "$([Environment]::UserName): --  SolidCP - Clean IIS Logs  --"

####################################################################################################################################################################################
# find the IIS Logs directory for each hosting space in C:\HostingSpaces, Then delete all of the IIS log files older than the days specified above
foreach ( $dDirectory in ((Get-ChildItem -Directory -Path "C:\HostingSpaces").FullName) ) {
    foreach ( $dSubDirectory in ((Get-ChildItem -Directory -Path "$dDirectory").FullName) ) {
        if (Test-Path "$dSubDirectory\logs") {
            foreach ( $dLogDir in ((Get-ChildItem -Directory -Path "$dSubDirectory\logs").FullName) ) {
                foreach ($dLogFile in (Get-ChildItem -Path $dLogDir)) {
                    if ( ($dLogFile.FullName -like '*.log') -and ($dLogFile.LastWriteTime -lt (Get-Date).AddDays(-$dDaysToKeep)) ) {
                        Write-Host ("Deleting - " + $dLogFile.FullName)
                        (Remove-Item -Path $dLogFile.FullName -Force) | Out-Null
                    }
                }
            }
        }
    }
}

####################################################################################################################################################################################
# find the IIS Logs directory for the default sites in C:\inetpub, Then delete all of the IIS log files older than the days specified above
foreach ($dDirectory in ((Get-ChildItem -Directory -Path "C:\inetpub\logs\LogFiles").FullName) ) {
    foreach ($dLogFile in (Get-ChildItem -Path $dDirectory)) {
        if ( ($dLogFile.FullName -like '*.log') -and ($dLogFile.LastWriteTime -lt (Get-Date).AddDays(-$dDaysToKeep)) ) {
            Write-Host ("Deleting - " + $dLogFile.FullName)
            (Remove-Item -Path $dLogFile.FullName -Force) | Out-Null
        }
    }
}

