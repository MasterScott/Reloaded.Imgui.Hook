﻿using System;
using System.Diagnostics;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Imgui.Hook;
using Reloaded.Mod.Interfaces;
using Reloaded.Mod.Interfaces.Internal;
using DearImguiSharp;

namespace Reloaded.ImGui.TestMod
{
    public class Program : IMod
    {
        /// <summary>
        /// Your mod if from ModConfig.json, used during initialization.
        /// </summary>
        private const string MyModId = "Reloaded.ImGui.TestMod";

        /// <summary>
        /// Used for writing text to the console window.
        /// </summary>
        private ILogger _logger;

        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private IModLoader _modLoader;

        /// <summary>
        /// An interface to Reloaded's the function hooks/detours library.
        /// See: https://github.com/Reloaded-Project/Reloaded.Hooks
        ///      for documentation and samples. 
        /// </summary>
        private IReloadedHooks _hooks;

        /// <summary>
        /// Contains the ImGui hook.
        /// </summary>
        private ImguiHook _imguiHook;

        /// <summary>
        /// Entry point for your mod.
        /// </summary>
        public async void Start(IModLoaderV1 loader)
        {
            _modLoader = (IModLoader)loader;
            _logger = (ILogger)_modLoader.GetLogger();
            _modLoader.GetController<IReloadedHooks>().TryGetTarget(out _hooks);

            /* Your mod code starts here. */
            SDK.Init(_hooks);
            _imguiHook = await ImguiHook.Create(RenderTestWindow);
        }

        private void RenderTestWindow()
        {
            bool open = true;
            DearImguiSharp.ImGui.ShowDemoWindow(ref open);
        }

        /* Mod loader actions. */
        public void Suspend() => _imguiHook.Disable();
        public void Resume() => _imguiHook.Enable();
        public void Unload() => Suspend();

        /*  If CanSuspend == false, suspend and resume button are disabled in Launcher and Suspend()/Resume() will never be called.
            If CanUnload == false, unload button is disabled in Launcher and Unload() will never be called.
        */
        public bool CanUnload() => true;
        public bool CanSuspend() => true;

        /* Automatically called by the mod loader when the mod is about to be unloaded. */
        public Action Disposing { get; }

        /* This is a dummy for R2R (ReadyToRun) deployment.
           For more details see: https://github.com/Reloaded-Project/Reloaded-II/blob/master/Docs/ReadyToRun.md
        */
        public static void Main() { }
    }
}
