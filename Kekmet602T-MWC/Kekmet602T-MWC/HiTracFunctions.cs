using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace Kekmet602T_MWC
{
    public class HiTracFunctions : MonoBehaviour
    {

        public FsmBool KekmetRange;
        public bool HiTrac;
        public Collider RangeLeverTrigger;


        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        private SmellyGearIndicatorSystem GearIndicatorInternal;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {


            KekmetRange = MainKekmetMod.KekmetLOD.transform.Find("Dashboard/Range").gameObject.GetPlayMaker("Use").FsmVariables.GetFsmBool("Range");

            RangeLeverTrigger = MainKekmetMod.RangeLever.GetComponent<Collider>();

            GearIndicatorInternal = MainKekmetMod.NewBody.GetComponent<SmellyGearIndicatorSystem>();

        }

        // Update is called once per frame
        void Update()
        {
            if (UnifiedRaycast.GetHit(RangeLeverTrigger))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Hi-Trac Selector";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!HiTrac)
                    {
                        HiTrac = true;
                        MainKekmetMod.RangeLever.transform.localPosition = new Vector3(0.15f, 1.130001f, -0.11f);
                        MainKekmetMod.RangeLever.transform.localEulerAngles = new Vector3(270f, 270f, 0f);
                        GearIndicatorInternal.HiTracString = "EXTRA-LOW";

                    }
                    else
                    {
                        HiTrac = false;
                        MainKekmetMod.RangeLever.transform.localPosition = new Vector3(0.15f, 1.130001f, -0.14f);
                        MainKekmetMod.RangeLever.transform.localEulerAngles = new Vector3(285f, 180f, 90.00008f);
                        GearIndicatorInternal.HiTracString = "HIGH";

                    }
                    MainKekmetMod.RangeShiftSound.Play();
                }
            }



            if (MainKekmetMod.HiTracSelector.GetKeybindDown()) // When the player hits their hits the hi-trac toggle bind
            {
                if (MainKekmetMod.KekmetSeatCheck.Value == "Kekmet")
                {
                    if (!HiTrac)
                    {
                        HiTrac = true;
                        MainKekmetMod.RangeLever.transform.localPosition = new Vector3(0.15f, 1.130001f, -0.11f);
                        MainKekmetMod.RangeLever.transform.localEulerAngles = new Vector3(270f, 270f, 0f);
                        GearIndicatorInternal.HiTracString = "EXTRA-LOW";

                    }
                    else
                    {
                        HiTrac = false;
                        MainKekmetMod.RangeLever.transform.localPosition = new Vector3(0.15f, 1.130001f, -0.14f);
                        MainKekmetMod.RangeLever.transform.localEulerAngles = new Vector3(285f, 180f, 90.00008f);
                        GearIndicatorInternal.HiTracString = "HIGH";

                    }
                    MainKekmetMod.RangeShiftSound.Play();
                }
            }



            // Hi-Trac 
            if (HiTrac) // If hi-trac enabled and tractor is in Low Range. 
            {
                if (!KekmetRange.Value)
                {
                    MainKekmetMod.TractorDriveTrain.finalDriveRatio = 24.731f;
                }
                else
                {
                    MainKekmetMod.TractorDriveTrain.finalDriveRatio = 12.365f;
                }

            }

            if (HiTrac == false) // If Hi-trac disabled and Tractor is in high range. 
            {
                if (KekmetRange.Value)
                {
                    MainKekmetMod.TractorDriveTrain.finalDriveRatio = 3.646f;
                }
                else
                {
                    MainKekmetMod.TractorDriveTrain.finalDriveRatio = 7.292f;
                }
                
            }
        }
    }
}