using MSCLoader;
using UnityEngine;

namespace Kekmet602T_MWC
{
    public class FourWheelDriveFunctions : MonoBehaviour
    {

        
        public bool FourWheelDriveToggle;
        public Collider FourWheelDriveLeverTrigger;

        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        private SmellyGearIndicatorSystem GearIndicatorInternal;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {
            FourWheelDriveToggle = false;
            FourWheelDriveLeverTrigger = MainKekmetMod.FourWheelDriveLever.GetComponent<BoxCollider>();
            GearIndicatorInternal = MainKekmetMod.NewBody.GetComponent<SmellyGearIndicatorSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (UnifiedRaycast.GetHit(FourWheelDriveLeverTrigger))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "4WD Selector";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!FourWheelDriveToggle)
                    {
                        FourWheelDriveToggle = true;

                        MainKekmetMod.TractorDriveTrain.SetTransmission(Drivetrain.Transmissions.AWD);
                        MainKekmetMod.FrontAxle.frontAxle.powered = true;
                        MainKekmetMod.FL_Wheel.isPowered = true;
                        MainKekmetMod.FR_Wheel.isPowered = true;

                        MainKekmetMod.FourWheelDriveLever.transform.localPosition = new Vector3(-0.4399999f, 0.7200001f, -0.5750002f);
                        MainKekmetMod.FourWheelDriveLever.transform.localEulerAngles = new Vector3(270f, 0f, 0f);



                        GearIndicatorInternal.FourWheelDriveString = "4";

                    }
                    else
                    {
                        FourWheelDriveToggle = false;

                        MainKekmetMod.TractorDriveTrain.SetTransmission(Drivetrain.Transmissions.RWD);
                        MainKekmetMod.FrontAxle.frontAxle.powered = false;
                        MainKekmetMod.FL_Wheel.isPowered = false;
                        MainKekmetMod.FR_Wheel.isPowered = false;

                        MainKekmetMod.FourWheelDriveLever.transform.localPosition = new Vector3(-0.4399999f, 0.6600002f, -0.5750002f);
                        MainKekmetMod.FourWheelDriveLever.transform.localEulerAngles = new Vector3(270f, 0f, 0f);

                        GearIndicatorInternal.FourWheelDriveString = "2";

                    }
                    MainKekmetMod.RangeShiftSound.Play();
                }
            }
        }
    }
}