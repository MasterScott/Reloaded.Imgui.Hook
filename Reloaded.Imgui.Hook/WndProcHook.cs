﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using Reloaded.Imgui.Hook.Misc;
using CallingConventions = Reloaded.Hooks.Definitions.X86.CallingConventions;

namespace Reloaded.Imgui.Hook
{
    /// <summary>
    /// Hooks the <see cref="WndProc"/> function of a given window.
    /// </summary>
    public class WndProcHook
    {
        /// <summary>
        /// The function that gets called when hooked.
        /// </summary>
        public WndProc HookFunction { get; private set; }

        /// <summary>
        /// Window handle of hooked window.
        /// </summary>
        public IntPtr WindowHandle { get; private set; }
        
        /// <summary>
        /// The hook created for the WndProc function.
        /// Can be used to call the original WndProc.
        /// </summary>
        public IHook<WndProc> Hook { get; private set; }

        /// <summary>
        /// Creates a hook for the WindowProc function.
        /// </summary>
        /// <param name="hWnd">Handle of the window to hook.</param>
        /// <param name="wndProcHandler">Handles the WndProc function.</param>
        public WndProcHook(IntPtr hWnd, WndProc wndProcHandler)
        {
            WindowHandle = hWnd;
            var windowProc = Native.GetWindowLong(hWnd, Native.GWL.GWL_WNDPROC);
            Misc.Debug.WriteLine($"WindowProc: {(long)windowProc:X}");
            SetupHook(wndProcHandler, windowProc);
        }

        /// <summary>
        /// Initializes the hook class.
        /// </summary>
        private void SetupHook(WndProc proc, IntPtr address)
        {
            HookFunction = proc;
            Hook = SDK.Hooks.CreateHook<WndProc>(HookFunction, (long) address).Activate();
        }

        public void Disable() => Hook.Disable();
        public void Enable()  => Hook.Enable();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [Function(Reloaded.Hooks.Definitions.X64.CallingConventions.Microsoft)]
        [Reloaded.Hooks.Definitions.X86.Function(CallingConventions.Stdcall)]
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
