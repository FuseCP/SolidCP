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
    public class VirtualHardDiskInfo
    {
        public VirtualHardDiskInfo()
        {

        }

        public VirtualHardDiskInfo(long fileSize, bool inSavedState, bool inUse, long maxInternalSize, string parentPath, VirtualHardDiskType diskType,
            bool supportPersistentReservations, ulong maximumIOPS, ulong minimumIOPS, ControllerType vhdControllerType, int controllerNumber, int controllerLocation,
            string name, string path, VirtualHardDiskFormat diskFormat, bool attached, uint blockSizeBytes)
        {
            FileSize = fileSize;
            InSavedState = inSavedState;
            InUse = inUse;
            MaxInternalSize = maxInternalSize;
            ParentPath = parentPath;
            DiskType = diskType;
            SupportPersistentReservations = supportPersistentReservations;
            MaximumIOPS = maximumIOPS;
            MinimumIOPS = minimumIOPS;
            VHDControllerType = vhdControllerType;
            ControllerNumber = controllerNumber;
            ControllerLocation = controllerLocation;
            Name = name;
            Path = path;
            DiskFormat = diskFormat;
            Attached = attached;
            BlockSizeBytes = blockSizeBytes;
        }

        public VirtualHardDiskInfo Clone()
        {
            return new VirtualHardDiskInfo(FileSize, InSavedState, InUse, MaxInternalSize, ParentPath, DiskType, SupportPersistentReservations, MaximumIOPS,
                MinimumIOPS, VHDControllerType, ControllerNumber, ControllerLocation, Name, Path, DiskFormat, Attached, BlockSizeBytes);
        }

        public long FileSize { get; set; }
        public bool InSavedState { get; set; }
        public bool InUse { get; set; }
        public long MaxInternalSize { get; set; }
        public string ParentPath { get; set; }
        public VirtualHardDiskType DiskType { get; set; }
        public bool SupportPersistentReservations { get; set; }
        public ulong MaximumIOPS { get; set; }
        public ulong MinimumIOPS { get; set; }
        public ControllerType VHDControllerType { get; set; }
        public int ControllerNumber { get; set; }
        public int ControllerLocation { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public VirtualHardDiskFormat DiskFormat { get; set; }
        public bool Attached { get; set; }
        public uint BlockSizeBytes { get; set; }
    }
}
