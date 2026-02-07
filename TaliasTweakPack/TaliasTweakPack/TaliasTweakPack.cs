using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using UnityEngine.SocialPlatforms;

namespace TaliasTweakPack
{
    public class TaliasTweakPack : Mod
    {
        public override string ID => "TaliasTweakPack"; // Your (unique) mod ID 
        public override string Name => "Talia's Tweak Pack"; // Your mod name
        public override string Author => "TaliaKuznetsova"; // Name of the Author (your name
        public override string Version => "1.8"; // Version
        public override string Description => "Misc tweaks and fixes to stuff that annoyed me"; // Short description of your mod 
        public override Game SupportedGames => Game.MyWinterCar;

        public SettingsCheckBox RegisterFix, AdvertSpawn, EntranceLightEnb, FirePlaceTempCap, GarageLightOverride, BiggerTv, PantryLight, InteriorLightTweak,PskTempSignEnb, PorchLightEnb, GifuExhaustEnb,SplashZone;
        public SphereCollider SepticRegisterCol, FleetariRegCol;
        public GameObject EntranceLightObj, EntranceSwitchObj, AdvertSpawner, GarageOverrideSwitchObj, TvContainer, OldTv, NewTv, TvScreen, VhsScreen, PantrySwitchObj, PantryLightObj, HouseLod, PantryLightVis, PskTempSign, PorchLightVis, PorchSwitchObj, PorchLightObj;
        public GameObject HouseApplinces, HouseSwitches, PorchLightTrigger, GifuExhaustHeatZone, Gifu, GifuPoopTrigger;
        public FsmFloat HouseKWH;
        public Material PorchLightOff, PorchLightOn;


        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.ModSettings, Mod_Settings);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.PostLoad, Mod_PostLoad);
        }

        private void Mod_Settings()
        {
            Settings.AddHeader("Save MUST be reloaded after making changes");
            Settings.AddHeader("Bug Fixes");
            RegisterFix = Settings.AddCheckBox("RegisterFix", "Fix Registers?", true, ApplySettings);
            Settings.AddHeader("Features");
            AdvertSpawn = Settings.AddCheckBox("AdvertSpawnChange", "Make the advertisement pile spawn at the parents house?", true, ApplySettings);
            EntranceLightEnb = Settings.AddCheckBox("EntranceLightEnb", "Adds a light to the parents house entrance hallway.", true, ApplySettings);
            PantryLight = Settings.AddCheckBox("PantryLight", "Adds a light to kitchen pantry/closet.", true, ApplySettings);
            GarageLightOverride = Settings.AddCheckBox("GarageLightOverride", "Add a manual override switch to the garage light?", true, ApplySettings);
            BiggerTv = Settings.AddCheckBox("BiggerTv", "Swap parents TV for a bigger one to save eyes?", true, ApplySettings);
            PskTempSignEnb = Settings.AddCheckBox("PskTempSignEnb", "Add temperature display to PSK sin?", true, ApplySettings);
            PorchLightEnb = Settings.AddCheckBox("PorchLightEnb", "Add a light to the parents house porch", true, ApplySettings);
            GifuExhaustEnb = Settings.AddCheckBox("GifuExhaustEnb", "Get warm standing near Gifu exhaust?", true, ApplySettings);
            SplashZone = Settings.AddCheckBox("SplashZone", "Add consequenes to walking in the septic truck stream?", true, ApplySettings);
            Settings.AddHeader("Tweaks");
            FirePlaceTempCap = Settings.AddCheckBox("FirePlaceTempCap", "Cap fireplace to prevent maxing out heat and sweat instantly?", true, ApplySettings);
            InteriorLightTweak = Settings.AddCheckBox("InteriorLightTweak", "Boost interior lights on cars?", true, ApplySettings);

        }

        private void ApplySettings()
        {

        }

        public TaliasTweakPack MainTweakMod;

        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;

        }

        private void Mod_OnLoad()
        {
            AssetBundle ab = LoadAssets.LoadBundle(this, "taliatweakpack");

            HouseLod = GameObject.Find("YARD").transform.Find("LOD").gameObject;

            HouseKWH = FsmVariables.GlobalVariables.GetFsmFloat("PlayerCurrentVehicle");

            HouseApplinces = GameObject.Find("YARD").transform.Find("Building/Dynamics/HouseElectricity/ElectricAppliances").gameObject;
            HouseSwitches = GameObject.Find("YARD").transform.Find("Building/Dynamics/LightSwitches").gameObject;

            if (EntranceLightEnb.GetValue())
            {
                EntranceSwitchObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("switch.prefab"));
                EntranceLightObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("LightEntrance1.prefab"));
                EntranceSwitchObj.name = ("switch_entrance");

                EntranceLightObj.transform.parent = HouseApplinces.transform;
                EntranceSwitchObj.transform.parent = HouseSwitches.transform;

                EntranceSwitchObj.transform.localPosition = new Vector3(1.767601f, 4.725902f, 1.419999f);
                EntranceSwitchObj.transform.localEulerAngles = new Vector3(90f, -9.576709E-06f, 0f);

                EntranceLightObj.transform.localPosition = new Vector3(1.779371f, 3.977836f, 2.392485f);
                EntranceLightObj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

                EntranceSwitchObj.AddComponent<EntranceLightBehavior>().SetupModClass(this);
                EntranceLightObj.SetActive(false);

            }

            if (AdvertSpawn.GetValue())
            {
                AdvertSpawner = GameObject.Find("JOBS").transform.Find("ADs/AdvertSpawn").gameObject;

                AdvertSpawner.transform.localPosition = new Vector3(1270.51f, -0.2388216f, -1073.096f);
                AdvertSpawner.transform.localEulerAngles = new Vector3(270f, 90f, 0f);
            }

            if (RegisterFix.GetValue())
            {

                SepticRegisterCol = GameObject.Find("WATERFACILITY").transform.Find("LOD/Desk/cash_register 1/cash_register_screen/CashRegisterLogic").GetComponent<SphereCollider>();
                FleetariRegCol = GameObject.Find("REPAIRSHOP").transform.Find("LOD/Store/CashRegister/FleetariCashRegister/cash_register/cash_register_screen/CashRegisterLogic").GetComponent<SphereCollider>();


                SepticRegisterCol.radius = 0.25f;
                FleetariRegCol.radius = 0.25f;
            }

            if (FirePlaceTempCap.GetValue())
            {
                GameObject.Find("YARD/Building/LIVINGROOM/Fireplace/HeatSourceFireplaceHouse").AddComponent<GarageLightOverrideBehavior>().SetupModClass(this);
            }

            if (GarageLightOverride.GetValue())
            {
                GarageOverrideSwitchObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("switch.prefab"));

                GarageOverrideSwitchObj.transform.parent = HouseSwitches.transform;
                GarageOverrideSwitchObj.name = ("switch_garage_override");

                GarageOverrideSwitchObj.transform.localPosition = new Vector3(4.669703f, 1.014f, 1.418f);
                GarageOverrideSwitchObj.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                GarageOverrideSwitchObj.transform.localScale = new Vector3(1f, 1f, 1f);

                GarageOverrideSwitchObj.AddComponent<GarageLightOverrideBehavior>().SetupModClass(this);
            }

            if (BiggerTv.GetValue())
            {

                TvContainer = GameObject.Find("YARD").transform.Find("Building/LIVINGROOM/TV").gameObject;
                OldTv = GameObject.Find("YARD").transform.Find("Building/LIVINGROOM/TV/mesh").gameObject;
                TvScreen = GameObject.Find("YARD").transform.Find("Building/LIVINGROOM/TV/TVScreen").gameObject;


                OldTv.SetActive(false);

                NewTv = GameObject.Instantiate(ab.LoadAsset<GameObject>("NewTvObj.prefab"));

                NewTv.transform.SetParent(TvContainer.transform, false);

                NewTv.transform.SetParent(TvContainer.transform, false);
                NewTv.transform.localPosition = new Vector3(-0.163f, 0.041f, -0.046f);
                NewTv.transform.localEulerAngles = new Vector3(-1.866933E-05f, 180f, 270f);


                TvContainer.transform.localPosition = new Vector3(-1.987495f, -1.94508f, -0.1121542f);
                TvContainer.transform.localEulerAngles = new Vector3(1.846991E-05f, -9.035568E-06f, 317.3911f);
                TvContainer.transform.localScale = new Vector3(1f, 2.15f, 1.6f);

                TvScreen.transform.localPosition = new Vector3(0.01f, 0f, 0.02f);
                TvScreen.transform.localEulerAngles = new Vector3(-3.415095E-06f, 3.415094E-06f, 1.024528E-05f);
                TvScreen.transform.localScale = new Vector3(1f, 1.002f, 1f);
            }

            if (PantryLight.GetValue())
            {
                PantrySwitchObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("switch.prefab"));
                PantryLightObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("LightEntrance1.prefab"));
                PantryLightVis = GameObject.Instantiate(ab.LoadAsset<GameObject>("HouseCeilingLight.prefab"));
                PantrySwitchObj.name = ("switch_pantry");
                PantryLightObj.name = ("light_pantry");
                PantryLightVis.name = ("PantryLightMesh");

                PantryLightObj.transform.parent = HouseApplinces.transform;
                PantrySwitchObj.transform.parent = HouseSwitches.transform;
                PantryLightVis.transform.parent = HouseLod.transform;


                PantrySwitchObj.transform.localPosition = new Vector3(2.430586f, 0.9683727f, 1.279999f);
                PantrySwitchObj.transform.localEulerAngles = new Vector3(-0.0003176038f, 270f, 270f);

                PantryLightObj.transform.localPosition = new Vector3(3.230625f, 0.362542f, 2.325636f);
                PantryLightObj.transform.localEulerAngles = new Vector3(270f, 180f, 0f);

                PantryLightVis.transform.localPosition = new Vector3(-13.56262f, 1.986179f, 6.411217f);
                PantryLightVis.transform.localEulerAngles = new Vector3(0f, 14f, 0f);




                PantrySwitchObj.AddComponent<PantryLightBehavior>().SetupModClass(this);
                PantryLightObj.SetActive(false);
            }

            if (InteriorLightTweak.GetValue())
            {
                GameObject.Find("JOBS").transform.Find("TAXIJOB/MACHTWAGEN/LOD/InteriorLight/Light").gameObject.GetComponent<Light>().range = 1.8f;
                GameObject.Find("BACHGLOTZ(1905kg)").transform.Find("LOD/InteriorLight/Light").gameObject.GetComponent<Light>().range = 2.5f;
                GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/InteriorLight/Light").gameObject.GetComponent<Light>().range = 1.8f;
                GameObject.Find("GIFU(750/450psi)").transform.Find("LOD/InteriorLight/Light").gameObject.GetComponent<Light>().range = 2.5f;
                GameObject.Find("SORBET(190-200psi)").transform.Find("LOD/InteriorLight/Light").gameObject.GetComponent<Light>().range = 2.5f;
            }

            if (PskTempSignEnb.GetValue())
            {
                PskTempSign = GameObject.Instantiate(ab.LoadAsset<GameObject>("PskTempDisplay.prefab"));
                PskTempSign.transform.parent = GameObject.Find("PERAPORTTI/FuelPylon").transform;

                PskTempSign.transform.localPosition = new Vector3(-0.395f, 0.001f, 1.46f);
                PskTempSign.transform.localEulerAngles = new Vector3(90f, 0f, 0f);



                PskTempSign.AddComponent<PskTempSignBehavior>().SetupModClass(this);//
            }

            if (PorchLightEnb.GetValue())
            {
                PorchSwitchObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("switch.prefab"));
                PorchLightObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("SpotLightOne.prefab"));
                PorchLightVis = GameObject.Instantiate(ab.LoadAsset<GameObject>("MotionLight.prefab"));
                PorchLightTrigger = GameObject.Instantiate(ab.LoadAsset<GameObject>("PorchLightTrigger.prefab"));

                PorchSwitchObj.name = ("switch_porch");
                PorchLightObj.name = ("light_porch");
                PorchLightVis.name = ("PorchLightMesh");

                PorchLightObj.transform.parent = HouseApplinces.transform;
                PorchSwitchObj.transform.parent = HouseSwitches.transform;
                PorchLightVis.transform.parent = HouseLod.transform;

                PorchSwitchObj.transform.localPosition = new Vector3(0.9581431f, -2.470926f, 1.279999f);
                PorchSwitchObj.transform.localEulerAngles = new Vector3(0.0003210189f, 90.00004f, 89.99998f);

                PorchLightObj.transform.localPosition = new Vector3(-2.471693f, -1.39828f, 1.78f);
                PorchLightObj.transform.localEulerAngles = new Vector3(84.67155f, 298.9441f, 284.0569f);

                PorchLightTrigger.transform.localPosition = new Vector3(-5.52582f, 0.4560307f, 0.3911738f);
                PorchLightTrigger.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                PorchLightTrigger.transform.localScale = new Vector3(11f, 3f, 9f);

                PorchLightVis.transform.localPosition = new Vector3(-7.877101f, 1.44618f, 4.743125f);
                PorchLightVis.transform.localEulerAngles = new Vector3(0f, 193f, 0f);

                PorchLightOff = GameObject.Instantiate(ab.LoadAsset<Material>("AtlasHomeMatTweakPack.mat"));
                PorchLightOn = GameObject.Instantiate(ab.LoadAsset<Material>("PorchLightLit.mat"));

                PorchLightObj.SetActive(false);

                PorchSwitchObj.AddComponent<PorchSwitchBehavior>().SetupModClass(this);
                PorchLightTrigger.AddComponent<PorchTriggerBehavior>().SetupModClass(this);
            }

            if (GifuExhaustEnb.GetValue())
            {
                GifuExhaustHeatZone = GameObject.Instantiate(ab.LoadAsset<GameObject>("HeatSourceGifuExhaust.prefab"));
                Gifu = GameObject.Find("GIFU(750/450psi)");
                GifuExhaustHeatZone.transform.SetParent(Gifu.transform.Find("LOD"));
                GifuExhaustHeatZone.AddComponent<GifuExhaustHeatBehavior>().SetupModClass(this);
                GifuExhaustHeatZone.transform.localPosition = new Vector3(-1.190144f, 0.1203288f, -0.2579448f);
                GifuExhaustHeatZone.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                GifuExhaustHeatZone.transform.localScale = new Vector3(0.4999999f, 1f, 1.5f);
            }

            if (SplashZone.GetValue())
            {
                Gifu = GameObject.Find("GIFU(750/450psi)");
                GifuPoopTrigger = GameObject.Instantiate(ab.LoadAsset<GameObject>("HeatSourceGifuExhaust.prefab"));
                GifuPoopTrigger.name = "SplashZoneTrigger";
                GifuPoopTrigger.transform.SetParent(Gifu.transform.Find("LOD"));

                GifuPoopTrigger.transform.localPosition = new Vector3(-0.09006908f, 0.6110867f, -5.554031f);
                GifuPoopTrigger.transform.localEulerAngles = new Vector3(0f, 90.00001f, 0f);
                GifuPoopTrigger.transform.localScale = new Vector3(2f, 1.5f, 1.6f);

                GifuPoopTrigger.AddComponent<SplashZoneBehavior>().SetupModClass(this);
            }

            ab.Unload(false);
        }

        private void Mod_PostLoad()
        {
            if (BiggerTv.GetValue())
            {
                if (ModLoader.IsModPresent("VHSPlayer"))
                {
                    VhsScreen = GameObject.Find("tv_table(Clone)").transform.Find("VHSScreen").gameObject;
                    VhsScreen.transform.localPosition = new Vector3(0.004f, 0.647f, -0.0618f);
                    VhsScreen.transform.localEulerAngles = new Vector3(270f, 90f, 0f);
                    VhsScreen.transform.localScale = new Vector3(1f, 2.15f, 1.6f);
                }
            }


        }
        private void Mod_Update()
        {
            

        }
    }
}
