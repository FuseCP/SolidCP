using System.Collections.Generic;
using System.Web.UI;
using SolidCP.Providers.Virtualization;

namespace SolidCP.Portal.Code.Helpers
{

    public static class VirtualMachineSettingExtensions
    {
        public static void BindSettingsControls(this Control page, VirtualMachine vm)
        {
            page.GetSettingsControls().ForEach(s => s.BindItem(vm));
        }

        public static void SaveSettingsControls(this Control page, ref VirtualMachine vm)
        {
            foreach (var s in page.GetSettingsControls()) s.SaveItem(ref vm);
        }

        public static List<IVirtualMachineSettingsControl> GetSettingsControls(this Control parent)
        {
            var result = new List<IVirtualMachineSettingsControl>();
            foreach (Control control in parent.Controls)
            {
                if (control is IVirtualMachineSettingsControl)
                    result.Add((IVirtualMachineSettingsControl)control);

                if (control.HasControls())
                    result.AddRange(control.GetSettingsControls());
            }
            return result;
        }
    }
}