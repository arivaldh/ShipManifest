﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConnectedLivingSpace;
using ShipManifest.InternalObjects;
using ShipManifest.Modules;
using UnityEngine;

namespace ShipManifest.Windows.Tabs.Control
{
  internal static class TabHatch
  {
    internal static string ToolTip = "";
    internal static bool ToolTipActive;
    internal static bool ShowToolTips = true;
    private const float guiRuleWidth = 350;
    private const float guiToggleWidth = 325;

    internal static void Display(Vector2 displayViewerPosition)
    {
      //float scrollX = WindowControl.Position.x + 20;
      //float scrollY = WindowControl.Position.y + 50 - displayViewerPosition.y;
      float scrollX = 20;
      float scrollY = displayViewerPosition.y;

      // Reset Tooltip active flag...
      ToolTipActive = false;
      SMHighlighter.IsMouseOver = false;

      GUILayout.BeginVertical();
      GUI.enabled = true;
      //GUILayout.Label("Hatch Control Center ", SMStyle.LabelTabHeader);
      GUILayout.Label(SmUtils.Localize("#smloc_control_hatch_000"), SMStyle.LabelTabHeader);
      GUILayout.Label("____________________________________________________________________________________________",
        SMStyle.LabelStyleHardRule, GUILayout.Height(10), GUILayout.Width(guiRuleWidth));
      string step = "start";
      try
      {
        // Display all hatches
        // ReSharper disable once ForCanBeConvertedToForeach
        for (int x = 0; x < SMAddon.SmVessel.Hatches.Count; x++)
        {
          ModHatch iHatch = SMAddon.SmVessel.Hatches[x];
          bool isEnabled = true;
          bool open = false;

          // get hatch state
          if (!iHatch.IsDocked)
            isEnabled = false;
          if (iHatch.HatchOpen)
            open = true;

          step = "gui enable";
          GUI.enabled = isEnabled;
          bool newOpen = GUILayout.Toggle(open, $"{iHatch.HatchStatus} - {iHatch.Title}", GUILayout.Width(guiToggleWidth));
          step = "button toggle check";
          if (!open && newOpen)
          {
            iHatch.OpenHatch(true);
          }
          else if (open && !newOpen)
          {
            iHatch.CloseHatch(true);
          }
          Rect rect = GUILayoutUtility.GetLastRect();
          if (Event.current.type == EventType.Repaint && rect.Contains(Event.current.mousePosition))
            SMHighlighter.SetMouseOverData(rect, scrollY, scrollX, WindowControl.TabBox.height, iHatch.ClsPart.Part, Event.current.mousePosition);
        }
        // Display MouseOverHighlighting, if any
        SMHighlighter.MouseOverHighlight();
      }
      catch (Exception ex)
      {
        SmUtils.LogMessage(
          $" in Hatches Tab at step {step}.  Error:  {ex.Message} \r\n\r\n{ex.StackTrace}",
          SmUtils.LogType.Error, true);
      }
      GUI.enabled = true;
      GUILayout.EndVertical();
    }

    internal static void OpenAllHatches()
    {
      // TODO: for realism, add a closing/opening sound
      // ReSharper disable once SuspiciousTypeConversion.Global
      List<IModuleDockingHatch>.Enumerator iModules = SMAddon.SmVessel.Hatches.Select(iHatch => (IModuleDockingHatch) iHatch.HatchModule)
        .Where(iModule => iModule.IsDocked).ToList().GetEnumerator();
      while (iModules.MoveNext())
      {
        if (iModules.Current == null) continue;
        iModules.Current.HatchEvents["CloseHatch"].active = true;
        iModules.Current.HatchEvents["OpenHatch"].active = false;
        iModules.Current.HatchOpen = true;
      }
      iModules.Dispose();
      SMAddon.FireEventTriggers();
    }

    internal static void CloseAllHatches()
    {
      // TODO: for realism, add a closing/opening sound
      // ReSharper disable once SuspiciousTypeConversion.Global
      List<IModuleDockingHatch>.Enumerator iModules = SMAddon.SmVessel.Hatches.Select(iHatch => (IModuleDockingHatch)iHatch.HatchModule)
        .Where(iModule => iModule.IsDocked).ToList().GetEnumerator();
      while (iModules.MoveNext())
      {
        if (iModules.Current == null) continue;
        iModules.Current.HatchEvents["CloseHatch"].active = false;
        iModules.Current.HatchEvents["OpenHatch"].active = true;
        iModules.Current.HatchOpen = false;
      }
      iModules.Dispose();
      SMAddon.FireEventTriggers();
    }
  }
}