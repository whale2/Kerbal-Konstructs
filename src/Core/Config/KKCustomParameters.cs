﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KerbalKonstructs;
using System.Collections;
using System.Reflection;
using KerbalKonstructs.Addons;

namespace KerbalKonstructs.Core
{
    public class KKCustomParameters0 : GameParameters.CustomParameterNode
    {
        private static KKCustomParameters0 _instance;


        public static KKCustomParameters0 instance
        {
            get
            {
                if (_instance == null)
                {
                    if (HighLogic.CurrentGame != null)
                    {
                        _instance = HighLogic.CurrentGame.Parameters.CustomParams<KKCustomParameters0>();
                    }
                }
                return _instance;
            }
        }

        public override string Title { get { return "Gameplay Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kerbal Konstructs"; } }
        public override string DisplaySection { get { return "Kerbal Konstructs"; } }
        public override int SectionOrder { get { return 0; } }
        public override bool HasPresets { get { return false; } }


        // GamePlay settings
        [GameParameters.CustomStringParameterUI("Gameplay settings", title = "GamePlay Settings", lines = 1)]
        public string blank0 = "";
        [GameParameters.CustomParameterUI("Enable RemoteTech GroundStation", toolTip = "Kerbal Konstricts will place RemoteTech ground antennas to any open GroundStation", autoPersistance = true)]
        public bool enableRT = true;
        [GameParameters.CustomParameterUI("Enable CommNet GroundStations", toolTip = "Kerbal Konstricts will place CommNet ground antennas to any open GroundStation", autoPersistance = true)]
        public bool enableCommNet = true;

        // difficulty setting
        [GameParameters.CustomStringParameterUI("", title = "", lines = 1)]
        public string blank01 = "";
        [GameParameters.CustomStringParameterUI("", title = "Difficulty Settings", lines = 1)]
        public string blank1 = "";
        [GameParameters.CustomParameterUI("Open bases only when landed", toolTip = "Enable this if you don't want to use the trackingstation to open new bases.\n " +
                                            "With this enabled you need to land at a base to open it", autoPersistance = true)]
        public bool disableRemoteBaseOpening = false;

        [GameParameters.CustomFloatParameterUI("max facility range", toolTip = "Until which distance should a facility be usable", minValue = 50, maxValue = 8000 , stepCount = 50,  autoPersistance = true)]
        public float facilityUseRange = 300;

        [GameParameters.CustomParameterUI("Disable Remote Recoovery", toolTip = "Disable the usage of open bases for the calculation of the recovery value", autoPersistance = true)]
        public bool disableRemoteRecovery = false;
        [GameParameters.CustomFloatParameterUI("Default recovery factor" , toolTip = "How good is KK base at recovering a vessel, this might be overwritten the bases configuration" , minValue = 0 , maxValue = 100 , autoPersistance = true)]
        public float defaultRecoveryFactor = 50;
        [GameParameters.CustomFloatParameterUI("Default recovery range", toolTip = "until which distance should a base be able to recover a vessel, this might be overwritten the bases configuration",  minValue = 0, maxValue = 500000, stepCount = 100, autoPersistance = true)]
        public float defaultEffectiveRange = 100000;

        // remove this
        [GameParameters.CustomStringParameterUI("", title = "", lines = 1)]
        public string blank03 = "";
        [GameParameters.CustomStringParameterUI("", title = "Map Options", lines = 1)]
        public string blank3 = "";
        [GameParameters.CustomParameterUI("Show Icons only with LS selector", toolTip = "Show only the icons on the map, when the KK selector is opened", autoPersistance = true)]
        public bool toggleIconsWithBB = false;


        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (member.Name == "enableRT") //This Field must always be Interactible.
            {
                if (RemoteTechAddon.isInstalled)
                {
                    return true;
                } else
                {
                    enableRT = false;
                    return false;
                }
            }
            else
                return true;
        }

    }

    public class KKCustomParameters1 : GameParameters.CustomParameterNode
    {
        private static KKCustomParameters1 _instance;

        public enum NewInstancePath
        {
            INTERNAL,
            EXTERNAL,
        }

        public static KKCustomParameters1 instance
        {
            get
            {
                if (_instance == null)
                {
                    if (HighLogic.CurrentGame != null)
                    {
                        _instance = HighLogic.CurrentGame.Parameters.CustomParams<KKCustomParameters1>();
                    }
                }
                return _instance;
            }
        }

        public override string Title { get { return "Editor Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kerbal Konstructs"; } }
        public override string DisplaySection { get { return "Kerbal Konstructs"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return false; } }



        [GameParameters.CustomStringParameterUI("", title = "Editor Settings", lines = 1)]
        public string blank1 = "";
        // editor settings
        [GameParameters.CustomFloatParameterUI("Maximum editor local range", minValue = 5000, maxValue = 200000, stepCount = 100, autoPersistance = true)]
        public float maxEditorVisRange = 100000;
        [GameParameters.CustomParameterUI("Spawn preview models", toolTip = "just leave this to true", autoPersistance = true)]
        public bool spawnPreviewModels = true;

        [GameParameters.CustomParameterUI("Directory for new Instances", toolTip = "Path under GameData where newly placed static configs should be saved", autoPersistance = true )]
        public NewInstancePath newInstanceEnum = NewInstancePath.INTERNAL;
        [GameParameters.CustomStringParameterUI("Directory for new Instances",lines = 2, autoPersistance = true )]
        public string newInstancePath = "KerbalKonstructs/NewInstances";


        [GameParameters.CustomStringParameterUI("", title = "", lines = 1)]
        public string blank02 = "";
        [GameParameters.CustomStringParameterUI("", title = "Debug Settings", lines = 1)]
        public string blank2 = "";
        [GameParameters.CustomParameterUI("Debug Mode", toolTip = "enable this to get a lot of debug messages.", autoPersistance = true)]
        public bool DebugMode = false;


        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (newInstanceEnum == NewInstancePath.INTERNAL)
            {
                newInstancePath = "KerbalKonstructs/NewInstances";
            } else
            {
                newInstancePath = "NewInstances";
            }

            return true;
        }


    }


    public class KKCustomParameters2 : GameParameters.CustomParameterNode
    {
        private static KKCustomParameters2 _instance;



        public static KKCustomParameters2 instance
        {
            get
            {
                if (_instance == null)
                {
                    if (HighLogic.CurrentGame != null)
                    {
                        _instance = HighLogic.CurrentGame.Parameters.CustomParams<KKCustomParameters2>();
                    }
                }
                return _instance;
            }
        }

        public override string Title { get { return "Cheats"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kerbal Konstructs"; } }
        public override string DisplaySection { get { return "Kerbal Konstructs"; } }
        public override int SectionOrder { get { return 2; } }
        public override bool HasPresets { get { return false; } }




        [GameParameters.CustomParameterUI("Launch from any Site", toolTip = "With this set to true you could launch a plane from the SHP on a rocket launchpad. or vice versa", autoPersistance = true)]
        public bool launchFromAnySite = false;
        [GameParameters.CustomParameterUI("Open everything", toolTip = "Use every base and facility without paying money", autoPersistance = true, gameMode = GameParameters.GameMode.CAREER)]
        public bool disableCareerStrategyLayer = false;





    }

}
