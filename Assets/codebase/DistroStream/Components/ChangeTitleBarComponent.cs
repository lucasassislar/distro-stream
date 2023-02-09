using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DistroStream {
    public class ChangeTitleBarComponent : MonoBehaviour {
        //Import the following.
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern System.IntPtr FindWindow(System.String className, System.String windowName);

        private void Awake() {
            Process p = Process.GetCurrentProcess();
            SetWindowText(p.MainWindowHandle, "distro-badges");
        }
    }
}
