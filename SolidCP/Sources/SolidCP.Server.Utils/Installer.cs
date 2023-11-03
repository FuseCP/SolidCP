using System;
using System.Linq;
using System.Diagnostics;
using SolidCP.Server.Utils;

namespace SolidCP.Utils
{
    public abstract class Installer
    {
        public virtual Shell Shell { get; set; }
        public abstract Shell Install(string apps)
        public abstract void AddSources(string sources);

    }

}