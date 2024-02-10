using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace WPF.Scripts
{
    public class WPFManager
    {
        #region Variables
        // Handles
        // ======================================================================
        // ======================================================================
        private static IntPtr DesktopHandle;
        private static IntPtr WorkerHandle;
        private static IntPtr WPFHandle;


        // System Parameters Info Codes
        // ======================================================================
        // ======================================================================
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;


        // Send Message Timeout Codes
        // ======================================================================
        // ======================================================================
        private static UInt32 SPAWN_WORKERW = 0x052C;
        private static UInt32 CLOSE = 0x0010;
        private static uint Timeout = 1000;
        #endregion Variables



        #region DLLs
        // Find Window
        // ======================================================================
        // ======================================================================
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string ClassName, string WindowName);


        // Get Next Window
        // ======================================================================
        // ======================================================================
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindow")]
        public static extern IntPtr GetNextWindow(IntPtr hWnd, GetWindowType uCmd);


        // Set Parent Window
        // ======================================================================
        // ======================================================================
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        // Send Message Timeout
        // ======================================================================
        // ======================================================================
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);


        // System Parameters Info
        // ======================================================================
        // ======================================================================
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        #endregion DLLs



        #region Enumerations
        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public enum GetWindowType : uint
        {
            /// <summary>
            /// The retrieved handle identifies the window of the same type that is highest in the Z order.
            /// <para/>
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// The retrieved handle identifies the window of the same type that is lowest in the Z order.
            /// <para />
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// The retrieved handle identifies the window below the specified window in the Z order.
            /// <para />
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// The retrieved handle identifies the window above the specified window in the Z order.
            /// <para />
            /// If the specified window is a topmost window, the handle identifies a topmost window.
            /// If the specified window is a top-level window, the handle identifies a top-level window.
            /// If the specified window is a child window, the handle identifies a sibling window.
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// The retrieved handle identifies the specified window's owner window, if any.
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// The retrieved handle identifies the child window at the top of the Z order,
            /// if the specified window is a parent window; otherwise, the retrieved handle is NULL.
            /// The function examines only child windows of the specified window. It does not examine descendant windows.
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// The retrieved handle identifies the enabled popup window owned by the specified window (the
            /// search uses the first such window found using GW_HWNDNEXT); otherwise, if there are no enabled
            /// popup windows, the retrieved handle is that of the specified window.
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }
        #endregion Enumerations



        #region Methods
        // Parent Window
        // ======================================================================
        // ======================================================================
        public static void ParentWindow()
        {
            //Get Program Manager Handle
            DesktopHandle = FindWindow("Progman", "Program Manager");

            //Send Message To Program Manager To Spawn wWorker Window
            SendMessageTimeout(DesktopHandle, SPAWN_WORKERW, new IntPtr(0), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, Timeout, out IntPtr result);

            //Get Created wWorker Window
            WorkerHandle = GetNextWindow(DesktopHandle, GetWindowType.GW_HWNDPREV);

            //Get Main Window Handle
            WPFHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;

            //Set Main Window Parent's To wWorker Window
            SetParent(WPFHandle, WorkerHandle);
        }


        // Refresh Desktop
        // ======================================================================
        // ======================================================================
        public static void RefreshDesktop()
        {
            //Refresh Desktop to Remove Window
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, null, SPIF_UPDATEINIFILE);
        }
        #endregion Methods
    }
}