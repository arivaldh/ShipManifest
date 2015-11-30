﻿using ShipManifest.APIClients;
using UnityEngine;

namespace ShipManifest.Windows.Tabs
{
  internal static class TabConfig
  {
    internal static string TxtSaveInterval = SMSettings.SaveIntervalSec.ToString();

    // GUI tooltip and label support
    private static string _toolTip = "";
    private static Rect _rect;
    private static string _label = "";
    private static GUIContent _guiLabel;

    internal static string ToolTip = "";
    internal static bool ToolTipActive;
    internal static bool ShowToolTips = true;
    private static bool _canShowToolTips = true;

    internal static Rect Position = WindowSettings.Position;

    internal static void Display(Vector2 displayViewerPosition)
    {
      // Reset Tooltip active flag...
      ToolTipActive = false;
      _canShowToolTips = WindowSettings.ShowToolTips && ShowToolTips;

      Position = WindowSettings.Position;
      var scrollX = 20;
      var scrollY = 50;

      GUILayout.Label("Configuraton", SMStyle.LabelTabHeader);
      GUILayout.Label("____________________________________________________________________________________________", SMStyle.LabelStyleHardRule, GUILayout.Height(10), GUILayout.Width(350));

      if (!ToolbarManager.ToolbarAvailable)
      {
        if (SMSettings.EnableBlizzyToolbar)
          SMSettings.EnableBlizzyToolbar = false;
        GUI.enabled = false;
      }
      else
        GUI.enabled = true;

      _label = "Enable Blizzy Toolbar (Replaces Stock Toolbar)";
      _toolTip = "Switches the toolbar Icons over to Blizzy's toolbar, if installed.";
      _toolTip += "\r\nIf Blizzy's toolbar is not installed, option is not selectable.";
      _guiLabel = new GUIContent(_label, _toolTip);
      SMSettings.EnableBlizzyToolbar = GUILayout.Toggle(SMSettings.EnableBlizzyToolbar, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);

      GUI.enabled = true;
      // UnityStyle Mode
      _label = "Enable Unity Style GUI Interface";
      _toolTip = "Changes all window appearances to Unity's Default look (like Mech Jeb).";
      _toolTip += "\r\nWhen Off, all windows look like KSP style windows.";
      _guiLabel = new GUIContent(_label, _toolTip);
      SMSettings.UseUnityStyle = GUILayout.Toggle(SMSettings.UseUnityStyle, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);
      if (SMSettings.UseUnityStyle != SMSettings.PrevUseUnityStyle)
        SMStyle.WindowStyle = null;

      _label = "Enable Debug Window";
      _toolTip = "Turns on or off the SM Debug window.";
      _toolTip += "\r\nAllows viewing log entries / errors generated by SM.";
      _guiLabel = new GUIContent(_label, _toolTip);
      WindowDebugger.ShowWindow = GUILayout.Toggle(WindowDebugger.ShowWindow, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);

      _label = "Enable Verbose Logging";
      _toolTip = "Turns on or off Expanded logging in the Debug Window.";
      _toolTip += "\r\nAids in troubleshooting issues in SM";
      _guiLabel = new GUIContent(_label, _toolTip);
      SMSettings.VerboseLogging = GUILayout.Toggle(SMSettings.VerboseLogging, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);

      _label = "Enable SM Debug Window On Error";
      _toolTip = "When On, Ship Manifest automatically displays the SM Debug window on an error in SM.";
      _toolTip += "\r\nThis is a troubleshooting aid.";
      _guiLabel = new GUIContent(_label, _toolTip);
      SMSettings.AutoDebug = GUILayout.Toggle(SMSettings.AutoDebug, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);

      _label = "Save Error log on Exit";
      _toolTip = "When On, Ship Manifest automatically saves the SM debug log on game exit.";
      _toolTip += "\r\nThis is a troubleshooting aid.";
      _guiLabel = new GUIContent(_label, _toolTip);
      SMSettings.SaveLogOnExit = GUILayout.Toggle(SMSettings.SaveLogOnExit, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);

      // create Limit Error Log Length slider;
      GUILayout.BeginHorizontal();
      _label = "Error Log Length: ";
      _toolTip = "Sets the maximum number of error entries stored in the log.";
      _toolTip += "\r\nAdditional entries will cause first entries to be removed from the log (rolling).";
      _toolTip += "\r\nSetting this value to '0' will allow unlimited entries.";
      _guiLabel = new GUIContent(_label, _toolTip);
      GUILayout.Label(_guiLabel, GUILayout.Width(140));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);
      SMSettings.ErrorLogLength = GUILayout.TextField(SMSettings.ErrorLogLength, GUILayout.Width(40));
      GUILayout.Label("(lines)", GUILayout.Width(50));
      GUILayout.EndHorizontal();

      _label = "Enable Kerbal Renaming";
      _toolTip = "Allows renaming a Kerbal.  The Profession may change when the kerbal is renamed.";
      _guiLabel = new GUIContent(_label, _toolTip);
      SMSettings.EnableKerbalRename = GUILayout.Toggle(SMSettings.EnableKerbalRename, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);

      if (!SMSettings.EnableKerbalRename)
        GUI.enabled = false;
      GUILayout.BeginHorizontal();
      _label = "Rename and Keep Profession (Experimental)";
      _toolTip = "When On, SM will remember the selected profesison when Kerbal is Renamed.";
      _toolTip += "\r\nAdds non printing chars to Kerbal name in your game save.";
      _toolTip += "\r\n(Should be no issue, but use at your own risk.)";
      _guiLabel = new GUIContent(_label, _toolTip);
      GUILayout.Space(20);
      SMSettings.RenameWithProfession = GUILayout.Toggle(SMSettings.RenameWithProfession, _guiLabel, GUILayout.Width(300));
      GUILayout.EndHorizontal();
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);
      GUI.enabled = true;

      _label = "Enable AutoSave Settings";
      _toolTip = "When On, SM automatically saves changes made to settings on a regular interval.";
      _guiLabel = new GUIContent(_label, _toolTip);
      SMSettings.AutoSave = GUILayout.Toggle(SMSettings.AutoSave, _guiLabel, GUILayout.Width(300));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);

      GUILayout.BeginHorizontal();
      _label = "Save Interval: ";
      _toolTip = "Sets the time (in seconds) between automatic saves.";
      _toolTip += "\r\nAutosave Settings must be enabled.";
      _guiLabel = new GUIContent(_label, _toolTip);
      GUILayout.Label(_guiLabel, GUILayout.Width(120));
      _rect = GUILayoutUtility.GetLastRect();
      if (Event.current.type == EventType.Repaint && _canShowToolTips)
        ToolTip = SMToolTips.SetActiveToolTip(_rect, Position, GUI.tooltip, ref ToolTipActive, scrollX, scrollY - displayViewerPosition.y);
      TxtSaveInterval = GUILayout.TextField(TxtSaveInterval, GUILayout.Width(40));
      GUILayout.Label("(sec)", GUILayout.Width(40));
      GUILayout.EndHorizontal();
    }
  }
}
