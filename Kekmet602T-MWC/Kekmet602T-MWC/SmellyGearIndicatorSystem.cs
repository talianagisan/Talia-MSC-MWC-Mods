using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace Kekmet602T_MWC
{
    public class SmellyGearIndicatorSystem : MonoBehaviour
    {
        public FsmBool KekmetRange;
        public FsmString guiGear;
        public string GearString, RangeString, HiTracString, FourWheelDriveString;

        string[] gears =
        {
            "R",
            "N",
            "1",
            "2",
            "3",
            "4",
        };

        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        private HiTracFunctions HiTracScript;
        private FourWheelDriveFunctions FourWheelDriveScript;

        



        
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {

            KekmetRange = MainKekmetMod.KekmetLOD.transform.Find("Dashboard/Range").gameObject.GetPlayMaker("Use").FsmVariables.GetFsmBool("Range");

            guiGear = PlayMakerGlobals.Instance.Variables.GetFsmString("GUIgear");


            HiTracScript = MainKekmetMod.RangeLever.GetComponent<HiTracFunctions>();

            FourWheelDriveScript = MainKekmetMod.FourWheelDriveLever.GetComponent<FourWheelDriveFunctions>();

            HiTracString = "HIGH";
            FourWheelDriveString = "2";
            RangeString = "LOW";

            MainKekmetMod.Kekmet.gameObject.GetPlayMaker("GearIndicator").enabled = false;
        }


        // Update is called once per frame
        void Update()
        {


            if(MainKekmetMod.KekmetSeatCheck.Value == "Kekmet")
            {

                RangeString = KekmetRange.Value ? "HIGH" : "LOW";
        

                guiGear.Value = gears[MainKekmetMod.TractorDriveTrain.gear] + " +" + RangeString + " +" + HiTracString + " -" + FourWheelDriveString;
            }

            //gears[MainKekmetMod.TractorDriveTrain.gear] + " +" + RangeString + " +" + HiTracString + " -" + FourWheelDriveString;
        }
    }
}
