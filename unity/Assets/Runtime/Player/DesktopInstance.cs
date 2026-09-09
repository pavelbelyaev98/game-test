using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace SomethingDownThere
{
    // The named kernel object's lifetime reserves one player per Windows session.
    // No thread owns the mutex; closing the last handle (including a process crash)
    // releases the reservation without a stale file or abandoned-lock recovery.
    public sealed class DesktopInstance : IDisposable
    {
        private readonly Mutex reservation;
        private static DesktopInstance current;
        public static bool IsDuplicate { get; private set; }

        private DesktopInstance(Mutex reservation) => this.reservation = reservation;

        public static DesktopInstance TryAcquire(string name)
        {
            var reservation = new Mutex(false, name, out bool created);
            if (created) return new DesktopInstance(reservation);
            reservation.Dispose();
            return null;
        }

        public void Dispose() => reservation.Dispose();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            current = TryAcquire(@"Local\SomethingDownThere.Player");
            if (current != null)
            {
                Application.quitting += Release;
                return;
            }
            IsDuplicate = true;
            FocusExistingGame();
            Debug.Log("Duplicate launch: returning to the running game.");
            // Use Unity's orderly native shutdown; immediate managed exit can
            // stall its worker threads. Scene save ownership also checks this flag.
            Application.Quit(0);
#endif
        }

        private static void Release()
        {
            current?.Dispose();
            current = null;
        }

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        private static void FocusExistingGame()
        {
            uint ownProcess = GetCurrentProcessId();
            EnumWindows((window, _) =>
            {
                GetWindowThreadProcessId(window, out uint process);
                if (process == ownProcess) return true;
                var text = new StringBuilder(256);
                GetClassName(window, text, text.Capacity);
                if (text.ToString() != "UnityWndClass") return true;
                GetWindowText(window, text, text.Capacity);
                if (text.ToString() != Application.productName) return true;
                // Restoring a minimized game and requesting focus are best effort;
                // Windows foreground rules must not become another player error.
                if (IsIconic(window)) ShowWindowAsync(window, 9);
                else if (!IsWindowVisible(window)) ShowWindowAsync(window, 5);
                SetForegroundWindow(window);
                return false;
            }, IntPtr.Zero);
        }

        private delegate bool WindowVisitor(IntPtr window, IntPtr state);
        [DllImport("kernel32.dll")] private static extern uint GetCurrentProcessId();
        [DllImport("user32.dll")] private static extern bool EnumWindows(WindowVisitor visitor, IntPtr state);
        [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr window, out uint process);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern int GetClassName(IntPtr window, StringBuilder text, int length);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)] private static extern int GetWindowText(IntPtr window, StringBuilder text, int length);
        [DllImport("user32.dll")] private static extern bool ShowWindowAsync(IntPtr window, int command);
        [DllImport("user32.dll")] private static extern bool IsIconic(IntPtr window);
        [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr window);
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr window);
#endif
    }
}
