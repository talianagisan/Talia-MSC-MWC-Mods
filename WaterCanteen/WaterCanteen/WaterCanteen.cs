using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;



namespace WaterCanteen
{
    public class WaterCanteen : Mod
    {
        public override string ID => "WaterCanteen";
        public override string Name => "WaterCanteen";
        public override string Author => "Talia Kuznetsova";
        public override string Version => "2.0";

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => true;

        public GameObject Canteen;
        public GameObject WaterTrig;



        public override void OnLoad()
        {
        
            AssetBundle ab = LoadAssets.LoadBundle(this, "watercanteen.unity3d"); //Specifies what the asset bundle is
            Canteen = ab.LoadAsset("Canteen.prefab") as GameObject;
            Canteen = GameObject.Instantiate(Canteen);
            Canteen.name = "Water Canteen(itemx)";
            Canteen.layer = LayerMask.NameToLayer("Parts");
            Canteen.tag = "PART";        

            
            WaterTrig.layer = LayerMask.NameToLayer("TriggerOnly");
			WaterTrig.tag = "Trigger";
            ab.Unload(false);
            
        }

 

        // Update is called once per frame
        public override void Update()
        {
        }  
    }
}
