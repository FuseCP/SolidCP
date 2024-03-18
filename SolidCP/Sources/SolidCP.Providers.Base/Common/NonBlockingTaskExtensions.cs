using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace System.Threading.Tasks
{
	public class TaskNotCompletedException: Exception {
		public TaskNotCompletedException():base() { }
		public TaskNotCompletedException(string message) : base(message) { }
		public TaskNotCompletedException(string message, Exception innerException): base(message, innerException) { }
	}
	public static class NonBlockingTaskExtensions
	{
		public static void SafeWait(this Task task)
		{

			if (SynchronizationContext.Current.GetType().Name == "AspNetSynchronizationContext")
			{
				while (!task.IsCompleted) Thread.Sleep(1);
				task.Wait(1);
			}
			else task.Wait(); // For WCF we don't need SafeWait
		}

		public static void SafeWait(this Task task, int milliseconds)
		{
			if (SynchronizationContext.Current.GetType().Name == "AspNetSynchronizationContext")
			{
				var stop = DateTime.Now.AddMilliseconds(milliseconds);
				while (!task.IsCompleted && DateTime.Now < stop) Thread.Sleep(1);
				task.Wait(1);
			}
			else task.Wait(milliseconds); // For WCF we don't need SafeWait
		}

		public static void SafeWait(this Task task, TimeSpan timeout)
		{
			if (SynchronizationContext.Current.GetType().Name == "AspNetSynchronizationContext")
			{
				var stop = DateTime.Now + timeout;
				while (!task.IsCompleted && DateTime.Now < stop) Thread.Sleep(1);
				task.Wait(1);
			}
			else task.Wait(timeout);
		}

		public static void SafeWait(this Task task, CancellationToken token)
		{
			if (SynchronizationContext.Current.GetType().Name == "AspNetSynchronizationContext")
			{
				while (!task.IsCompleted && !token.IsCancellationRequested) Thread.Sleep(1);
				task.Wait(1, token);
			}
			else task.Wait(token);
		}

		public static void SafeWait(this Task task, int milliseconds, CancellationToken token)
		{
			if (SynchronizationContext.Current.GetType().Name == "AspNetSynchronizationContext")
			{
				var stop = DateTime.Now.AddMilliseconds(milliseconds);
				while (!task.IsCompleted && !token.IsCancellationRequested && DateTime.Now < stop) Thread.Sleep(1);
				task.Wait(1, token);
			}
			else task.Wait(milliseconds, token);
		}

		public static T SafeResult<T>(this Task<T> task)
		{
			task.SafeWait();
			return task.Result;
		}
		public static T SafeResult<T>(this Task<T> task, int milliseconds)
		{
			task.SafeWait(milliseconds);
			if (!task.IsCompleted) throw new TaskNotCompletedException("Task not completed.");
			return task.Result;
		}
		public static T SafeResult<T>(this Task<T> task, TimeSpan timeout)
		{
			task.SafeWait(timeout);
			if (!task.IsCompleted) throw new TaskNotCompletedException("Task not completed.");
			return task.Result;
		}
		public static T SafeResult<T>(this Task<T> task, CancellationToken token)
		{
			task.SafeWait(token);
			if (!task.IsCompleted) throw new TaskNotCompletedException("Task not completed.");
			return task.Result;
		}
		public static T SafeResult<T>(this Task<T> task, int milliseconds, CancellationToken token)
		{
			task.SafeWait(milliseconds, token);
			if (!task.IsCompleted) throw new TaskNotCompletedException("Task not completed.");
			return task.Result;
		}

	}
}
