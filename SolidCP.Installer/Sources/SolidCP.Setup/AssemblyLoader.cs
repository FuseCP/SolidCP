using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SolidCP.Setup
{
	public class AssemblyLoader
	{

		public void ExtractAssemblies()
		{
			var asm = GetType().Assembly;
			var path = Path.GetDirectoryName(asm.Location);
			var metadata = asm.GetManifestResourceStream("costura.metadata");

		}
	}
}
