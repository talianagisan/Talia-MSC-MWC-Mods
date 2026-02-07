using MSCLoader;
using UnityEngine;

namespace Kekmet602T_MWC
{
    public class SideLightFunctions : MonoBehaviour
    {

        public bool SideLightsToggled;
        public Collider SideLightTrigCol;
        public GameObject SideLightSwitch, SideLights, Light3Obj, Light4Obj, Light5Obj, Light6Obj;
        public Light Light3, Light4, Light5, Light6;

        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {

            SideLightTrigCol = MainKekmetMod.NewLights.transform.Find("SideLightTrigger").GetComponent<Collider>(); // Find the trigger for the side lights
            SideLightSwitch = MainKekmetMod.NewLights.transform.Find("Switches/SideLightSwitch").gameObject;

            SideLights = MainKekmetMod.NewLights.transform.Find("SideLights").gameObject;
            SideLights.SetActive(false);

            Light3Obj = SideLights.transform.Find("LeftRearLight").gameObject;
            Light4Obj = SideLights.transform.Find("LeftFrontLight").gameObject;
            Light5Obj = SideLights.transform.Find("RightFrontLight").gameObject;
            Light6Obj = SideLights.transform.Find("RightRearLight").gameObject;

            Light3 = Light3Obj.GetComponent<Light>(); //Gets all the light components 
            Light4 = Light4Obj.GetComponent<Light>();
            Light5 = Light5Obj.GetComponent<Light>();
            Light6 = Light6Obj.GetComponent<Light>();

            Light3.flare = MainKekmetMod.Lightflare;
            Light4.flare = MainKekmetMod.Lightflare;
            Light5.flare = MainKekmetMod.Lightflare;
            Light6.flare = MainKekmetMod.Lightflare;

        }

        // Update is called once per frame
        void Update()
        {

            if (UnifiedRaycast.GetHit(SideLightTrigCol)) //Gets the collider named SideLightTrigger
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true; // Enables hand interact icon
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "SIDE LIGHTS"; // Tells the player they're looking at the switch for the side lights
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Left click
                {
                    if (!SideLightsToggled)
                    {
                        SideLights.SetActive(true);
                        SideLightsToggled = true;
                        SideLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.513999f, -0.285f);
                        SideLightSwitch.transform.localEulerAngles = new Vector3(279.9999f, 267.7023f, 180f); // Sets switch position to On and enables the light obj, Could be done by togling active obj vs position adjustment
                    }
                    else
                    {
                        SideLights.SetActive(false);
                        SideLightsToggled = false;
                        SideLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.522f, -0.285f);
                        SideLightSwitch.transform.localEulerAngles = new Vector3(290f, 87.70235f, -4.493277E-05f); // Sets switch position to Off and disables the light obj, Same potential different way.
                    }
                    MainKekmetMod.ButtonSound.Play();

                }
            }
        }
    }
}