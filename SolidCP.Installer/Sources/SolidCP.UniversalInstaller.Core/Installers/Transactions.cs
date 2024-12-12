using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace SolidCP.UniversalInstaller
{

	public class Transaction
	{
		protected Installer Installer { get; private set; }
		protected ExceptionDispatchInfo Error { get => Installer.Error; set => Installer.Error = value; }
		protected IEnumerable<Action> Undos { get => Installer.Undos; set => Installer.Undos = value; }

		public Transaction(Installer installer) => Installer = installer;

		public void WithRollback(params IEnumerable<Action> actions)
		{
			Undos = actions = actions.Concat(Undos);
			if (Error != null)
			{
				foreach (var action in actions)
				{
					try
					{
						action?.Invoke();
					}
					catch { }
				}
				Undos = Enumerable.Empty<Action>();
				Error.Throw();
			}
		}
	}
	public partial class Installer
	{
		public ExceptionDispatchInfo Error { get; set; } = null;
		public IEnumerable<Action> Undos { get; set; } = Enumerable.Empty<Action>();
		public Transaction Transaction(params IEnumerable<Action> actions)
		{
			foreach (var action in actions)
			{
				try
				{
					action?.Invoke();
				}
				catch (Exception ex)
				{
					Log.WriteError(ex.Message, ex);
					Error = ExceptionDispatchInfo.Capture(ex);
					break;
				}
			}

			return new Transaction(this);
		}

	}
}
