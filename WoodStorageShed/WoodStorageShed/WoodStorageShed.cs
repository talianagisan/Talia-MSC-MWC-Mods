using MSCLoader;
using UnityEngine;
using static CarDynamics;
using static UnityEngine.UI.GridLayoutGroup;

namespace WoodStorageShed
{
    public class WoodStorageShed : Mod
    {
        public override string ID => "WoodStorageShed"; // Your (unique) mod ID 
        public override string Name => "Wood Storage Shed"; // Your mod name
        public override string Author => "TaliaKuznetsova"; // Name of the Author (your name)
        public override string Version => "1.0"; // Version
        public override string Description => "Adds a wood storage shed to the side of the parents house, storing wood to save your electric bills."; // Short description of your mod

        public override Game SupportedGames => Game.MyWinterCar;

        GameObject WoodRack;
        GameObject TractorWoodMover;
        int NewGameWoodShed;
        AudioSource WoodSound;
        public WoodShedBehavior WoodShedStuff;
        public TractorCarrierBehavior TractorCarrierStuff;


        public override void ModSetup()
        {
            SetupFunction(Setup.OnNewGame, Mod_OnNewGame);
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.ModSettings, Mod_Settings);
        }

        private void Mod_Settings()
        {
            // All settings should be created here. 
            // DO NOT put anything that isn't settings or keybinds in here!
        }

        private void Mod_OnNewGame()
        {
            NewGameWoodShed = Random.Range(1, 48);            
        }
        private void Mod_OnLoad()
        {
            AssetBundle ab = LoadAssets.LoadBundle(this, "woodrack");

            WoodRack = GameObject.Instantiate(ab.LoadAsset<GameObject>("WoodRack.prefab"));
            WoodRack.transform.localPosition = new Vector3(-10.6f, -0.7700001f, -1.66f);
            WoodRack.transform.localEulerAngles = new Vector3(0f, 90.00001f, 0f);


            WoodRack.AddComponent<WoodShedBehavior>();

            WoodSound = WoodRack.GetComponent<AudioSource>();

            WoodShedStuff = WoodRack.GetComponent<WoodShedBehavior>();


            WoodShedStuff.WoodCountShed = SaveLoad.ReadValue<int>(this, "LogAmountShed");


            TractorWoodMover = GameObject.Instantiate(ab.LoadAsset<GameObject>("TractorWoodCarrier.prefab"));

            TractorWoodMover.AddComponent<TractorCarrierBehavior>();


            TractorCarrierStuff = TractorWoodMover.GetComponent<TractorCarrierBehavior>();


            TractorCarrierStuff.WoodCountTractor = SaveLoad.ReadValue<int>(this, "LogAmountTractor");


            TractorWoodMover.transform.localPosition = SaveLoad.ReadValue<Vector3>(this, "TractorCarrierPos");
            TractorWoodMover.transform.localEulerAngles = SaveLoad.ReadValue<Vector3>(this, "TractorCarrierRot");

            if(TractorWoodMover.transform.localPosition == Vector3.zero)
            {
                TractorWoodMover.transform.localPosition = new Vector3(19f, -0.38f, -49.2f);
                TractorWoodMover.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            }


            if (NewGameWoodShed != 0)
            {

                TractorWoodMover.transform.localPosition = new Vector3(19f, -0.38f, -49.2f);
                TractorWoodMover.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                WoodShedStuff.WoodCountShed = NewGameWoodShed;
                TractorCarrierStuff.WoodCountTractor = 0;
                
            }

            ab.Unload(false);
        }

        private void Mod_OnSave()
        {

            SaveLoad.WriteValue(this, "LogAmountShed", (WoodShedStuff.WoodCountShed));
            SaveLoad.WriteValue(this, "LogAmountTractor", (TractorCarrierStuff.WoodCountTractor));
            SaveLoad.WriteValue(this, "TractorCarrierPos", (TractorWoodMover.transform.localPosition));
            SaveLoad.WriteValue(this, "TractorCarrierRot", (TractorWoodMover.transform.localEulerAngles));
        }

        private void Mod_Update()
        {
            // Update is called once per frame
        }
    }
}
