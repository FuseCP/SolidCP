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

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Providers.Virtualization
{
    public enum VirtualMachineState
    {
        /*
        Unknown = 0,
        Running = 2, // start
        Off = 3, // turn off
        Reset = 10, // reset
        Paused = 32768, // pause
        Saved = 32769, // save
        Starting = 32770,
        Snapshotting = 32771,
        Migrating = 32772,
        Saving = 32773,
        Stopping = 32774,
        Deleted = 32775,
        Pausing = 32776
        */
        Snapshotting = 32771,
        Migrating = 32772,
        Deleted = 32775,

        Unknown = 0,
        Other = 1,
        Running = 2,
        Off = 3,
        Stopping = 32774, // new 4
        Saved = 32769, // new 6
        Paused = 32768, // new 9
        Starting = 32770, // new 10
        Reset = 10, // new 11
        Saving = 32773, // new 32773
        Pausing = 32776, // new 32776
        Resuming = 32777,
        FastSaved = 32779,
        FastSaving = 32780,
        RunningCritical = 32781,
        OffCritical = 32782,
        StoppingCritical = 32783,
        SavedCritical = 32784,
        PausedCritical = 32785,
        StartingCritical = 32786,
        ResetCritical = 32787,
        SavingCritical = 32788,
        PausingCritical = 32789,
        ResumingCritical = 32790,
        FastSavedCritical = 32791,
        FastSavingCritical = 32792
    }
}
