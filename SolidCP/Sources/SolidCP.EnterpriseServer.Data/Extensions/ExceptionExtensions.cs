using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
#if NetCore
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity.Validation;
#endif

namespace SolidCP.EnterpriseServer.Data.Extensions
{
	public static class ExceptionExtensions
	{
		public static string ValidationErrorMessage(this Exception ex)
		{
#if NETFRAMEWORK
			if (ex is DbEntityValidationException evex)
			{
				return $"Validation Errors: {Environment.NewLine}" +
					string.Join(Environment.NewLine,
						evex.EntityValidationErrors
							.Select(err => string.Join(Environment.NewLine, err.ValidationErrors
								.Select(err2 => $"{err2.PropertyName} - {err2.ErrorMessage}"))));
			}
#endif
			return null;
		}
	}
}
