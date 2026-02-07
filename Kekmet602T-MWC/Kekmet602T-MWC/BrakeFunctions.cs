using MSCLoader;
using UnityEngine;

namespace Kekmet602T_MWC
{
    public class BrakeFunctions : MonoBehaviour
    {

        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {



        }

        // Update is called once per frame
        void Update()
        {

            if (MainKekmetMod.KekmetSeatCheck.Value == "Kekmet")
            {

                if (MainKekmetMod.BrakeLeft.GetKeybind()) // When the player hits their split brake left button
                {
                    {
                        MainKekmetMod.RL_Wheel.brake = 1f * MainKekmetMod.BrakeForceModifier;
                        MainKekmetMod.BrakeNewL.transform.localPosition = new Vector3(0.01f, 0.01f, 0.05f);
                        MainKekmetMod.BrakeNewL.transform.localEulerAngles = new Vector3(0, 25f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
                    }

                }
                else
                {
                    MainKekmetMod.BrakeNewL.transform.localPosition = new Vector3(-0.01f, 0.01f, 0.05f);
                    MainKekmetMod.BrakeNewL.transform.localEulerAngles = new Vector3(0, 0f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
                }

                if (MainKekmetMod.BrakeRight.GetKeybind()) // When the player hits their split brake right button
                {
                    {
                        MainKekmetMod.RR_Wheel.brake = 1f * MainKekmetMod.BrakeForceModifier;
                        MainKekmetMod.BrakeNewR.transform.localPosition = new Vector3(0.02f, 0.01f, 0.05f);
                        MainKekmetMod.BrakeNewR.transform.localEulerAngles = new Vector3(0, 25f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
                    }
                }
                else
                {
                    MainKekmetMod.BrakeNewR.transform.localPosition = new Vector3(0f, 0.01f, 0.05f);
                    MainKekmetMod.BrakeNewR.transform.localEulerAngles = new Vector3(0, 0f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
                }

            }

        }
    }
}