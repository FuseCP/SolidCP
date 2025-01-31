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

using System;
using System.ServiceProcess;
using System.Threading;
using SolidCP.EnterpriseServer;

namespace SolidCP.SchedulerService;


#if NETCOREAPP
public class ServiceBase {
    protected virtual void OnStart(string[] args) { }

    public static void Run(ServiceBase[] services) {
        foreach (var service in services) service.OnStart(Environment.GetCommandLineArgs());
    }    
}
#endif

public partial class SchedulerService : ServiceBase
{
    private Timer _Timer;
    private static object _isRuninng;
    #region Construcor

    public SchedulerService()
    {
        _isRuninng = new object();

        InitializeComponent();

        _Timer = new Timer(Process, null, 5000, 5000);
    }

    #endregion

    #region Methods

    protected override void OnStart(string[] args)
    {
    }

    protected static void Process(object callback)
    {
        //check running service
        if (!Monitor.TryEnter(_isRuninng))
            return;

        try
        {
            using (var scheduler = new Scheduler())
            {
                scheduler.Start();
            }
        }
        finally
        {
            Monitor.Exit(_isRuninng);
        }

    }

    #endregion
}
