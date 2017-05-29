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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using SolidCP.Server.Utils;

namespace SolidCP.Providers.DNS
{
	/// <summary>This class is a generic helper hosting the PowerShell runtime.</summary>
	/// <remarks>It's probably a good idea to move to some utility module.</remarks>
	public class PowerShellHelper: IDisposable
	{
		private static InitialSessionState s_session = null;

		static PowerShellHelper()
		{
			s_session = InitialSessionState.CreateDefault();
			// s_session.ImportPSModule( new string[] { "FileServerResourceManager" } );
		}

		public PowerShellHelper()
		{
			Log.WriteStart( "PowerShellHelper::ctor" );

			Runspace rs = RunspaceFactory.CreateRunspace( s_session );
			rs.Open();
			// rs.SessionStateProxy.SetVariable( "ConfirmPreference", "none" );

			this.runSpace = rs;
			Log.WriteEnd( "PowerShellHelper::ctor" );
		}

		public void Dispose()
		{
			try
			{
				if( this.runSpace == null )
					return;
				if( this.runSpace.RunspaceStateInfo.State == RunspaceState.Opened )
					this.runSpace.Close();
				this.runSpace = null;
			}
			catch( Exception ex )
			{
				Log.WriteError( "Runspace error", ex );
			}
		}

		public Runspace runSpace { get; private set; }

		public Collection<PSObject> RunPipeline( params Command[] pipelineCommands )
		{
			Log.WriteStart( "ExecuteShellCommand" );
			List<object> errorList = new List<object>();

			Collection<PSObject> results = null;
			using( Pipeline pipeLine = runSpace.CreatePipeline() )
			{
				// Add the command
				foreach( var cmd in pipelineCommands )
					pipeLine.Commands.Add( cmd );

				// Execute the pipeline and save the objects returned.
				results = pipeLine.Invoke();

				// Only non-terminating errors are delivered here.
				// Terminating errors raise exceptions instead.
				if( null != pipeLine.Error && pipeLine.Error.Count > 0 )
				{
					foreach( object item in pipeLine.Error.ReadToEnd() )
					{
						errorList.Add( item );
						string errorMessage = string.Format( "Invoke error: {0}", item );
						Log.WriteWarning( errorMessage );
					}
				}
			}
			// errors = errorList.ToArray();
			Log.WriteEnd( "ExecuteShellCommand" );
			return results;
		}
	}
}
