using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace Thermometer
{
    public class Thermometer : Mod
    {
        public override string ID => "Thermometer"; // Your (unique) mod ID 
        public override string Name => "Thermometer"; // Your mod name
        public override string Author => "TaliaKuznetsova"; // Name of the Author (your name)
        public override string Version => "1.1.1"; // Version
        public override string Description => ""; // Short description of your mod 
        public override Game SupportedGames => Game.MyWinterCar;

        GameObject ApartmentThermometer, HouseThermometer;



        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.Update, Mod_Update);
        }

        private void Mod_Settings()
        {
            // All settings should be created here. 
            // DO NOT put anything that isn't settings or keybinds in here!
        }

        private void Mod_OnLoad()
        {
            // Called once, when mod is loading after game is fully loaded.
            AssetBundle ab = LoadAssets.LoadBundle(this, "thermometer");

            ApartmentThermometer = GameObject.Instantiate(ab.LoadAsset<GameObject>("Thermometer.prefab"));
            HouseThermometer = GameObject.Instantiate(ab.LoadAsset<GameObject>("Thermometer.prefab"));

            ApartmentThermometer.name = "Apartment Thermometer";
            HouseThermometer.name = "House Thermometer";

            ApartmentThermometer.AddComponent<ApartmentThermometer>();
            HouseThermometer.AddComponent<HouseThermometer>();

            HouseThermometer.transform.localPosition = new Vector3(-1.823585f, 0.5861815f, 8.061175f);
            HouseThermometer.transform.localEulerAngles = new Vector3(0f, 15f, 0f);


            ApartmentThermometer.transform.localPosition = new Vector3(-1280.885f, 0.9700118f, 1079.785f);
            ApartmentThermometer.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

            ab.Unload(false);
        }
        private void Mod_Update()
        {
        }
        
    }
}
