﻿using KerbalKonstructs.Core;
using KerbalKonstructs.API;
using KerbalKonstructs.Utilities;
using System;
using System.Collections.Generic;
using LibNoise.Unity.Operator;
using UnityEngine;
using System.Linq;
using System.IO;
using Upgradeables;
using UpgradeLevel = Upgradeables.UpgradeableObject.UpgradeLevel;

namespace KerbalKonstructs.UI
{
    class KSCManager : KKWindow
    {
        Rect KSCmanagerRect = new Rect(150, 50, 410, 300);

        public Texture tTexture = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/kscadminister", false);
        public Texture tBanner = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/banner", false);

        public Texture tAdmin = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/kscadminister", false);
        public Texture tTracking = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/ksctrack", false);
        public Texture tRND = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/ksclabs", false);
        public Texture tLaunch = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/kscpad", false);
        public Texture tRunway = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/kscway", false);
        public Texture tFacVAB = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/kscassembly", false);
        public Texture tFacSPH = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/kschangar", false);
        public Texture tAstro = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/kscquarters", false);
        public Texture tControl = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/ksccontrol", false);
        public Texture tTick = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/settingstick", false);
        public Texture tCross = GameDatabase.Instance.GetTexture("KerbalKonstructs/Assets/settingscross", false);

        GUIStyle Yellowtext;
        GUIStyle TextAreaNoBorder;
        GUIStyle KKWindow;
        GUIStyle BoxNoBorder;
        GUIStyle SmallButton;
        GUIStyle LabelWhite;
        GUIStyle KKWindowTitle;
        GUIStyle LabelInfo;
        GUIStyle DeadButton;
        GUIStyle DeadButtonRed;
        GUIStyle LabelTip;

        public String sSelectedfacility = "None";
        public String sFacilityUseRange = "";


        public bool bDetermined = false;

        public static float fFacLvl = 0f;
        public static float fmaxLvl = 0f;

        public double dUpdater = 0;

        public override void Draw()
        {
            drawKSCManager();
        }

        public override void Close()
        {
            KerbalKonstructs.GUI_Settings.Close();
            base.Close();
        }

        public void drawKSCManager()
        {
            KKWindow = new GUIStyle(GUI.skin.window);
            KKWindow.padding = new RectOffset(3, 3, 5, 5);

            KSCmanagerRect = GUI.Window(0xC00B1E2, KSCmanagerRect, drawKSCmanagerWindow, "", KKWindow);
        }


        public void drawKSCmanagerWindow(int WindowID)
        {
            DeadButton = new GUIStyle(GUI.skin.button);
            DeadButton.normal.background = null;
            DeadButton.hover.background = null;
            DeadButton.active.background = null;
            DeadButton.focused.background = null;
            DeadButton.normal.textColor = Color.white;
            DeadButton.hover.textColor = Color.white;
            DeadButton.active.textColor = Color.white;
            DeadButton.focused.textColor = Color.white;
            DeadButton.fontSize = 14;
            DeadButton.fontStyle = FontStyle.Bold;

            DeadButtonRed = new GUIStyle(GUI.skin.button);
            DeadButtonRed.normal.background = null;
            DeadButtonRed.hover.background = null;
            DeadButtonRed.active.background = null;
            DeadButtonRed.focused.background = null;
            DeadButtonRed.normal.textColor = Color.red;
            DeadButtonRed.hover.textColor = Color.yellow;
            DeadButtonRed.active.textColor = Color.red;
            DeadButtonRed.focused.textColor = Color.red;
            DeadButtonRed.fontSize = 12;
            DeadButtonRed.fontStyle = FontStyle.Bold;

            Yellowtext = new GUIStyle(GUI.skin.box);
            Yellowtext.normal.textColor = Color.yellow;
            Yellowtext.normal.background = null;

            TextAreaNoBorder = new GUIStyle(GUI.skin.textArea);
            TextAreaNoBorder.normal.background = null;
            TextAreaNoBorder.normal.textColor = Color.white;
            TextAreaNoBorder.fontSize = 12;
            TextAreaNoBorder.padding.left = 1;
            TextAreaNoBorder.padding.right = 1;
            TextAreaNoBorder.padding.top = 4;

            BoxNoBorder = new GUIStyle(GUI.skin.box);
            BoxNoBorder.normal.background = null;
            BoxNoBorder.normal.textColor = Color.white;

            LabelWhite = new GUIStyle(GUI.skin.label);
            LabelWhite.normal.background = null;
            LabelWhite.normal.textColor = Color.white;
            LabelWhite.fontSize = 12;
            LabelWhite.padding.left = 1;
            LabelWhite.padding.right = 1;
            LabelWhite.padding.top = 4;

            LabelTip = new GUIStyle(GUI.skin.label);
            LabelTip.normal.background = null;
            LabelTip.normal.textColor = Color.yellow;
            LabelTip.fontSize = 12;
            LabelTip.padding.left = 1;
            LabelTip.padding.right = 1;
            LabelTip.padding.top = 4;

            LabelInfo = new GUIStyle(GUI.skin.label);
            LabelInfo.normal.background = null;
            LabelInfo.normal.textColor = Color.white;
            LabelInfo.fontSize = 13;
            LabelInfo.fontStyle = FontStyle.Bold;
            LabelInfo.padding.left = 3;
            LabelInfo.padding.top = 0;
            LabelInfo.padding.bottom = 0;

            KKWindowTitle = new GUIStyle(GUI.skin.box);
            KKWindowTitle.normal.background = null;
            KKWindowTitle.normal.textColor = Color.white;
            KKWindowTitle.fontSize = 14;
            KKWindowTitle.fontStyle = FontStyle.Bold;

            SmallButton = new GUIStyle(GUI.skin.button);
            SmallButton.normal.textColor = Color.red;
            SmallButton.hover.textColor = Color.white;
            SmallButton.padding.top = 1;
            SmallButton.padding.left = 1;
            SmallButton.padding.right = 1;
            SmallButton.padding.bottom = 4;
            SmallButton.normal.background = null;
            SmallButton.hover.background = null;
            SmallButton.fontSize = 12;

            GUILayout.Box(tBanner, BoxNoBorder);
            GUILayout.Space(5);




            GUILayout.Space(3);
            GUILayout.Box("Only used @KSC to access Mod Settings");
            GUILayout.Label("Use this button in other GameScenes like in-flight, Trackingstation or on the map", LabelInfo);

            GUILayout.FlexibleSpace();
            GUILayout.Label("TIP: To use the KK editor, hit Ctrl+K when inflight, preferably when landed near the base you want to edit or the location you want to make a new base.", LabelTip);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Mod Settings"))
            {
                KerbalKonstructs.GUI_Settings.Toggle();
            }

            GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        }
    }
}
