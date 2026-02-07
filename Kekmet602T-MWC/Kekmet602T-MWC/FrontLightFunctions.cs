using MSCLoader;
using UnityEngine;

namespace Kekmet602T_MWC
{
    public class FrontLightFunctions : MonoBehaviour
    {
        public GameObject FrontLights, FrontLightSwitch, Light1Obj, Light2Obj;
        public Collider FrontLightTrigCol;
        public Light Light1, Light2;
        public bool FrontLightsToggled;
        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {
            FrontLightTrigCol = MainKekmetMod.NewLights.transform.Find("FrontLightTrigger").gameObject.GetComponent<Collider>(); // Find the trigger for the front lights


            FrontLights = MainKekmetMod.NewLights.transform.Find("FrontLights").gameObject; // Front lights

            FrontLightSwitch = MainKekmetMod.NewLights.transform.Find("Switches/FrontLightSwitch").gameObject; // Front light visual switch object

            FrontLights.SetActive(false); //Disables frontlights by default/off

            Light1Obj = FrontLights.transform.Find("RightLight").gameObject;
            Light2Obj = FrontLights.transform.Find("LeftLight").gameObject;

            Light1 = Light1Obj.GetComponent<Light>();
            Light2 = Light2Obj.GetComponent<Light>();

            Light1.flare = MainKekmetMod.Lightflare;
            Light2.flare = MainKekmetMod.Lightflare;
        }

        // Update is called once per frame
        void Update()
        {

            if (UnifiedRaycast.GetHit(FrontLightTrigCol))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true; // Enables hand interact icon
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "FRONT LIGHTS"; // Tells the player they're looking at the switch for the front lights
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Left click
                {
                    if (!FrontLightsToggled)
                    {
                        FrontLights.SetActive(true);
                        FrontLightsToggled = true;
                        FrontLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.513999f, -0.1929f);
                        FrontLightSwitch.transform.localEulerAngles = new Vector3(0f, 0f, 105f); // Sets switch position and enables front lights
                    }
                    else
                    {
                        FrontLights.SetActive(false);
                        FrontLightsToggled = false;
                        FrontLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.522f, -0.1929f);
                        FrontLightSwitch.transform.localEulerAngles = new Vector3(0f, 0f, 70.00001f); // sets switch position and disables front lights
                    }
                    MainKekmetMod.ButtonSound.Play();

                }
            }
        }
    }
}