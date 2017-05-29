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
using System.Text;
using System.IO;
using System.Threading;

namespace SolidCP.Setup.Actions
{
	public class ActionProgressEventArgs<T> : EventArgs
	{
		public string StatusMessage { get; set; }
		public T EventData { get; set; }
		public bool Indeterminate { get; set; }
	}

	public class ProgressEventArgs : EventArgs
	{
		public int Value { get; set; }
	}

	public class ActionErrorEventArgs : EventArgs
	{
		public string ErrorMessage { get; set; }
		public Exception OriginalException { get; set; }
	}

	public abstract class Action
	{
		public event EventHandler<ActionProgressEventArgs<int>> ProgressChange;
		public event EventHandler<ActionProgressEventArgs<bool>> PrerequisiteComplete;

		protected object objectLock = new Object();

		public virtual bool Indeterminate
		{
			get
			{
				return true;
			}
		}

		protected void Begin(string message)
		{
			OnInstallProgressChanged(message, 0);
		}

		protected void Finish(string message)
		{
			OnInstallProgressChanged(message, 100);
		}

		protected void OnInstallProgressChanged(string message, int progress)
		{
			if (ProgressChange == null)
				return;
			//
			ProgressChange(this, new ActionProgressEventArgs<int>
			{
				StatusMessage = message,
				EventData = progress,
				Indeterminate = this.Indeterminate
			});
		}

		protected void OnUninstallProgressChanged(string message, int progress)
		{
			if (ProgressChange == null)
				return;
			//
			ProgressChange(this, new ActionProgressEventArgs<int>
			{
				StatusMessage = message,
				EventData = progress,
				Indeterminate = this.Indeterminate
			});
		}

		protected void OnPrerequisiteCompleted(string message, bool result)
		{
			if (PrerequisiteComplete == null)
				return;
			//
			PrerequisiteComplete(this, new ActionProgressEventArgs<bool>
			{
				StatusMessage = message,
				EventData = result
			});
		}
	}

	public interface IActionManager
	{
		/// <summary>
		/// 
		/// </summary>
		event EventHandler<ProgressEventArgs> TotalProgressChanged;
		/// <summary>
		/// 
		/// </summary>
		event EventHandler<ActionProgressEventArgs<int>> ActionProgressChanged;
		/// <summary>
		/// 
		/// </summary>
		event EventHandler<ActionProgressEventArgs<bool>> PrerequisiteComplete;
		/// <summary>
		/// 
		/// </summary>
		event EventHandler<ActionErrorEventArgs> ActionError;
		/// <summary>
		/// Gets current variables available in this session
		/// </summary>
		SetupVariables SessionVariables { get; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		void AddAction(Action action);
		/// <summary>
		/// Triggers manager to run currentScenario specified
		/// </summary>
		void Start();
		/// <summary>
		/// Triggers manager to run prerequisites verification procedure
		/// </summary>
		void VerifyDistributivePrerequisites();
		/// <summary>
		/// Triggers manager to prepare default parameters for the distributive
		/// </summary>
		void PrepareDistributiveDefaults();
		/// <summary>
		/// Initiates rollback procedure from the action specified
		/// </summary>
		/// <param name="lastSuccessActionIndex"></param>
		void Rollback();
	}

	public class BaseActionManager : IActionManager
	{
		private List<Action> currentScenario;
		//
		private int lastSuccessActionIndex = -1;

		private SetupVariables sessionVariables;

		public SetupVariables SessionVariables
		{
			get
			{
				return sessionVariables;
			}
		}

		protected List<Action> CurrentScenario
		{
			get { return currentScenario; }
			set { currentScenario = value; }
		}

		#region Events
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<ProgressEventArgs> TotalProgressChanged;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<ActionProgressEventArgs<int>> ActionProgressChanged;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<ActionProgressEventArgs<bool>> PrerequisiteComplete;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler<ActionErrorEventArgs> ActionError;
		/// <summary>
		/// 
		/// </summary>
		public event EventHandler Initialize;
		#endregion

		protected BaseActionManager(SetupVariables sessionVariables)
		{
			if (sessionVariables == null)
				throw new ArgumentNullException("sessionVariables");
			//
			currentScenario = new List<Action>();
			//
			this.sessionVariables = sessionVariables;
		}

		private void OnInitialize()
		{
			if (Initialize == null)
				return;
			//
			Initialize(this, EventArgs.Empty);
		}
		
		/// <summary>
		/// Adds action into the list of currentScenario to be executed in the current action manager's session and attaches to its ProgressChange event 
		/// to track the action's execution progress.
		/// </summary>
		/// <param name="action">Action to be executed</param>
		/// <exception cref="ArgumentNullException"/>
		public virtual void AddAction(Action action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			currentScenario.Add(action);
		}

		private void UpdateActionProgress(string actionText, int actionValue, bool indeterminateAction)
		{
			OnActionProgressChanged(this, new ActionProgressEventArgs<int>
			{
				StatusMessage = actionText,
				EventData = actionValue,
				Indeterminate = indeterminateAction
			});
		}

		private void UpdateTotalProgress(int value)
		{
			OnTotalProgressChanged(this, new ProgressEventArgs { Value = value });
		}

		// Fire the Event   
		private void OnTotalProgressChanged(object sender, ProgressEventArgs args)
		{
			// Check if there are any Subscribers   
			if (TotalProgressChanged != null)
			{
				// Call the Event   
				TotalProgressChanged(sender, args);
			}
		}

		// Fire the Event   
		private void OnActionProgressChanged(object sender, ActionProgressEventArgs<int> args)
		{
			// Check if there are any Subscribers   
			if (ActionProgressChanged != null)
			{
				// Call the Event   
				ActionProgressChanged(sender, args);
			}
		}

		// Fire the Event   
		private void OnPrerequisiteComplete(object sender, ActionProgressEventArgs<bool> args)
		{
			// Check if there are any Subscribers   
			if (PrerequisiteComplete != null)
			{
				// Call the Event   
				PrerequisiteComplete(sender, args);
			}
		}

		private void OnActionError(Exception ex)
		{
			//
			if (ActionError == null)
				return;
			//
			var args = new ActionErrorEventArgs
			{
				ErrorMessage = "An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@solidcp.com.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"SolidCP Installer home directory.",
				OriginalException = ex,
			};
			//
			ActionError(this, args);
		}

		/// <summary>
		/// Starts action execution.
		/// </summary>
		public virtual void Start()
		{
			var currentActionType = default(Type);

			#region Executing the installation session
			//
			int totalValue = 0;
			for (int i = 0, progress = 1; i < currentScenario.Count; i++, progress++)
			{
				var item = currentScenario[i];
				// Get the next action from the queue
				var action = item as IInstallAction;
				// Take the action's type to log as much information about it as possible
				currentActionType = item.GetType();
				//
				if (action != null)
				{
					item.ProgressChange += new EventHandler<ActionProgressEventArgs<int>>(action_ProgressChanged);

					try
					{
						// Execute an install action
						action.Run(SessionVariables);
					}
					catch (Exception ex)
					{
						//
						if (currentActionType != default(Type))
						{
							Log.WriteError(String.Format("Failed to execute '{0}' type of action.", currentActionType));
						}
						//
						Log.WriteError("Here is the original exception...", ex);
						//
						if (Utils.IsThreadAbortException(ex))
							return;
						// Notify external clients
						OnActionError(ex);
						//
						Rollback();
						//
						return;
					}
					//
					item.ProgressChange -= new EventHandler<ActionProgressEventArgs<int>>(action_ProgressChanged);
					// Calculate overall current progress status
					totalValue = Convert.ToInt32(progress * 100 / currentScenario.Count);
					// Update overall progress status
					UpdateTotalProgress(totalValue);
				}
				//
				lastSuccessActionIndex = i;
			}
			//
			totalValue = 100;
			//
			UpdateTotalProgress(totalValue);
			//
			#endregion
		}

		void action_ProgressChanged(object sender, ActionProgressEventArgs<int> e)
		{
			// Action progress has been changed
			UpdateActionProgress(e.StatusMessage, e.EventData, e.Indeterminate);
		}

		public virtual void Rollback()
		{
			var currentActionType = default(Type);
			//
			Log.WriteStart("Rolling back");
			//
			UpdateActionProgress("Rolling back", 0, true);
			//
			var totalValue = 0;
			//
			UpdateTotalProgress(totalValue);
			//
			for (int i = lastSuccessActionIndex, progress = 1; i >= 0; i--, progress++)
			{
				var action = currentScenario[i] as IUninstallAction;
				//
				if (action != null)
				{
					action.ProgressChange += new EventHandler<ActionProgressEventArgs<int>>(action_ProgressChanged);
					//
					try
					{
						action.Run(sessionVariables);
					}
					catch (Exception ex)
					{
						if (currentActionType != default(Type))
							Log.WriteError(String.Format("Failed to rollback '{0}' action.", currentActionType));
						//
						Log.WriteError("Here is the original exception...", ex);
						//
					}
					//
					action.ProgressChange -= new EventHandler<ActionProgressEventArgs<int>>(action_ProgressChanged);
				}
				//
				totalValue = Convert.ToInt32(progress * 100 / (lastSuccessActionIndex + 1));
				//
				UpdateTotalProgress(totalValue);
			}
			//
			Log.WriteEnd("Rolled back");
		}

		public void VerifyDistributivePrerequisites()
		{
			var currentActionType = default(Type);

			try
			{
				//
				for (int i = 0; i < currentScenario.Count; i++)
				{
					var item = currentScenario[i];
					// Get the next action from the queue
					var action = item as IPrerequisiteAction;
					//
					currentActionType = item.GetType();
					//
					if (action != null)
					{
						//
						action.Complete += new EventHandler<ActionProgressEventArgs<bool>>(action_Complete);
						// Execute an install action
						action.Run(SessionVariables);
						//
						action.Complete -= new EventHandler<ActionProgressEventArgs<bool>>(action_Complete);
					}
				}
			}
			catch (Exception ex)
			{
				//
				if (currentActionType != default(Type))
				{
					Log.WriteError(String.Format("Failed to execute '{0}' type of action.", currentActionType));
				}
				//
				Log.WriteError("Here is the original exception...", ex);
				//
				if (Utils.IsThreadAbortException(ex))
					return;
				// Notify external clients
				OnActionError(ex);
				//
				return;
			}
		}

		void action_Complete(object sender, ActionProgressEventArgs<bool> e)
		{
			OnPrerequisiteComplete(sender, e);
		}

		public void PrepareDistributiveDefaults()
		{
			//
			OnInitialize();
			//
			var currentActionType = default(Type);

			try
			{
				//
				for (int i = 0; i < currentScenario.Count; i++)
				{
					// Get the next action from the queue
					var action = currentScenario[i] as IPrepareDefaultsAction;
					//
					if (action == null)
					{
						continue;
					}
					//
					currentActionType = action.GetType();
					// Execute an install action
					action.Run(SessionVariables);
				}
			}
			catch (Exception ex)
			{
				//
				if (currentActionType != default(Type))
				{
					Log.WriteError(String.Format("Failed to execute '{0}' type of action.", currentActionType));
				}
				//
				Log.WriteError("Here is the original exception...", ex);
				//
				if (Utils.IsThreadAbortException(ex))
					return;
				// Notify external clients
				OnActionError(ex);
				//
				return;
			}
		}
	}
}
