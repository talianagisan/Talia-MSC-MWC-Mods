using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using ModsShop;
using System;

namespace TSW1000
{
    public class TSW1000 : Mod
    {
        public override string ID => "TSW1000"; // Your (unique) mod ID 
        public override string Name => "TSW-1000"; // Your mod name
        public override string Author => "TaliaKuznetsova"; // Name of the Author (your name)
        public override string Version => "1.1"; // Version
        public override string Description => "Adds a watch that tells you time and temperature"; // Short description of your mod 
        public override Game SupportedGames => Game.MyWinterCar;

        public override void ModSetup()
        {
            SetupFunction(Setup.OnNewGame, Mod_OnNewGame);
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.ModSettings, Mod_Settings);
            SetupFunction(Setup.Update, Mod_Update);
        }

        private void Mod_Settings()
        {
            AlternateSkinSetting = Settings.AddSlider("AlternateSkinSetting", "Selects alternate skins", 1, 4, 1, ApplySettings);
        }

        private void ApplySettings()
        {
            if (AlternateSkinSetting.GetValue() == 1)
            {
                WatchRenderer.material = WatchMatOne;
            }
            else if (AlternateSkinSetting.GetValue() == 2)
            {
                WatchRenderer.material = WatchMatTwo;
            }
            else if (AlternateSkinSetting.GetValue() == 3)
            {
                WatchRenderer.material = WatchMatThree;
            }
            else if (AlternateSkinSetting.GetValue() == 4)
            {
                WatchRenderer.material = WatchMatFour;
            }
        }

        private void Mod_OnNewGame()
        {
            WatchPurchased = false;
            SaveLoad.WriteValue(this, "WatchPurchased", (WatchPurchased));
        }

        public TSW1000 MainWatchMod;
        public GameObject TswWatch, OldWatch, WatchDisplay;
        public TextMesh DayText, TimeTempText, TempSelectText;
        public FsmFloat AmbientTemp, BodyTemp, ClockMin;
        public FsmInt ClockHour, ClockDay;
        public bool WatchPurchased;
        public AudioSource WatchBeep;
        public SettingsSliderInt AlternateSkinSetting;
        public Material WatchMatOne, WatchMatTwo, WatchMatThree, WatchMatFour;
        public MeshRenderer WatchRenderer, BoxRenderer;

        public string[] Days =
        {
        "ERR","MON","TUE","WEN","THR","FRI","SAT","SUN"
        
        };




        public void SetupModClass(TSW1000 mainMod)
        {
            MainWatchMod = mainMod;

        }
        private void Mod_OnLoad()
        {
            WatchPurchased = false;
            WatchPurchased = SaveLoad.ReadValue<bool>(this, "WatchPurchased");

            AssetBundle ab = LoadAssets.LoadBundle(this, "tsw-1000");
            TswWatch = GameObject.Instantiate(ab.LoadAsset<GameObject>("Tsw1000.prefab"));
            TswWatch.SetActive(false);
            WatchRenderer = TswWatch.transform.Find("Mesh").GetComponent<MeshRenderer>();


            OldWatch = GameObject.Find("PLAYER").transform.Find("Pivot/AnimPivot/Camera/FPSCamera/FPSCamera/Watch/Animate/BreathAnim/WristwatchHand").gameObject;

            WatchMatOne = GameObject.Instantiate(ab.LoadAsset<Material>("WatchMat1.mat"));
            WatchMatTwo = GameObject.Instantiate(ab.LoadAsset<Material>("WatchMat2.mat"));
            WatchMatThree = GameObject.Instantiate(ab.LoadAsset<Material>("WatchMat3.mat"));
            WatchMatFour = GameObject.Instantiate(ab.LoadAsset<Material>("WatchMat4.mat"));




            TswWatch.transform.SetParent(OldWatch.transform, false);

            DayText = TswWatch.transform.Find("Texts/DayText").gameObject.GetComponent<TextMesh>();
            TempSelectText = TswWatch.transform.Find("Texts/TempSelectText").gameObject.GetComponent<TextMesh>(); ;
            TimeTempText = TswWatch.transform.Find("Texts/Time-Temp-Text").gameObject.GetComponent<TextMesh>();

            TswWatch.transform.localEulerAngles = new Vector3(270f, 180f, 0f);

            AmbientTemp = GameObject.Find("PLAYER/BodyTemp").GetPlayMaker("Calculations").GetVariable<FsmFloat>("Temperature");
            BodyTemp = PlayMakerGlobals.Instance.Variables.GetFsmFloat("PlayerTemp"); PlayMakerGlobals.Instance.Variables.GetFsmFloat("ClockMinutes");

            ClockMin = PlayMakerGlobals.Instance.Variables.GetFsmFloat("ClockMinutes");
            ClockHour = PlayMakerGlobals.Instance.Variables.GetFsmInt("GlobalHour");
            ClockDay = PlayMakerGlobals.Instance.Variables.GetFsmInt("GlobalDay");

            WatchBeep = TswWatch.transform.Find("WatchBeep").GetComponent<AudioSource>();

            if (WatchPurchased)
            {
                OldWatch.transform.Find("table").gameObject.SetActive(false); OldWatch.transform.Find("glass").gameObject.SetActive(false); OldWatch.transform.Find("Clock").gameObject.SetActive(false);
                TswWatch.SetActive(true); WatchPurchased = true;
            }
            else
            {

                WatchDisplay = GameObject.Instantiate(ab.LoadAsset<GameObject>("WatchDisplayBox.prefab"));
                BoxRenderer = TswWatch.transform.Find("Mesh").GetComponent<MeshRenderer>();

                ModsShop.Shop shop = ModsShop.ModsShop.GetShopReference();

                ModsShop.ItemDetails WatchShopItem = shop.CreateShopItem(this, "TswWatchPurchaseable1", "Cashino TSW-1000", 1250f, false, AfterPurchased, TswWatch, ModsShop.SpawnMethod.Custom, true);

                shop.AddDisplayItem(WatchShopItem, WatchDisplay, ModsShop.SpawnMethod.Instantiate, new Vector3(0, 180f, 0f));
            }


            ab.Unload(false);

            if (AlternateSkinSetting.GetValue() == 1)
            {
                WatchRenderer.material = WatchMatOne;
            }
            else if (AlternateSkinSetting.GetValue() == 2)
            {
                WatchRenderer.material = WatchMatTwo;
            }
            else if (AlternateSkinSetting.GetValue() == 3)
            {
                WatchRenderer.material = WatchMatThree;
            }
            else if (AlternateSkinSetting.GetValue() == 4)
            {
                WatchRenderer.material = WatchMatFour;
            }

        }
        private void Mod_OnSave()
        {
            SaveLoad.WriteValue(this, "WatchPurchased", (WatchPurchased));
        }

        private void AfterPurchased(ModsShop.Checkout WatchShopItem)
        {
            OldWatch.transform.Find("table").gameObject.SetActive(false); OldWatch.transform.Find("glass").gameObject.SetActive(false); OldWatch.transform.Find("Clock").gameObject.SetActive(false);
            TswWatch.SetActive(true); WatchPurchased = true;
        }

        private void Mod_Update()
        {
            if (cInput.GetButton("Watch"))
            {
                DayText.text = Days[ClockDay.Value];

                if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))
                {
                    WatchBeep.Play();
                }


                if (Input.GetKey(KeyCode.Mouse0))
                {
                    TempSelectText.text = "AMB";
                    TimeTempText.text = AmbientTemp.Value.ToString("#.0");

                }
                else if (Input.GetKey(KeyCode.Mouse1))
                {
                    TempSelectText.text = "BOD";
                    TimeTempText.text = ((BodyTemp.Value * 0.0222f)+35).ToString("#.0");
                }
                else
                {
                    TempSelectText.text = "TIM";                  
                    TimeTempText.text = ClockHour.Value.ToString() + ":" + ClockMin.Value.ToString("00");
                }
            }
        }
    }
}
