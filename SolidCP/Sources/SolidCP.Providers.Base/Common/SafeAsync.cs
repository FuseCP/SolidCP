using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidCP.Providers
{
	internal readonly ref struct SafeAsync: IDisposable
	{
		private readonly SynchronizationContext oldSynchronizationContext;

		public SafeAsync()
		{
			oldSynchronizationContext = SynchronizationContext.Current;
			SynchronizationContext.SetSynchronizationContext(null);
		}

		public void Dispose()
		{
			SynchronizationContext.SetSynchronizationContext(oldSynchronizationContext);
		}
	}
}
