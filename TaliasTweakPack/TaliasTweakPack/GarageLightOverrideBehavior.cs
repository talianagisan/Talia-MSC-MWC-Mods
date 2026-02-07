using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace TaliasTweakPack
{
    public class GarageLightOverrideBehavior : MonoBehaviour
    {

        public GameObject LightSwitchButton, LightObj;
        public SphereCollider SwitchTrigCol;
        public bool lightOn;
        public AudioSource SwitchSound;
        public PlayMakerFSM MotionLogic;

        // Use this for initialization
        private TaliasTweakPack MainTweakMod;
        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;
        }
        void Start()
        {
            LightSwitchButton = MainTweakMod.GarageOverrideSwitchObj.transform.Find("lights_switch/lights_switch_button").gameObject;
            LightSwitchButton.transform.localEulerAngles = new Vector3(12f, 0f, 0f);

            SwitchSound = MainTweakMod.GarageOverrideSwitchObj.GetComponent<AudioSource>();
            SwitchTrigCol = MainTweakMod.GarageOverrideSwitchObj.GetComponent<SphereCollider>();

            LightObj = GameObject.Find("YARD").transform.Find("Building/Dynamics/HouseElectricity/ElectricAppliances/AutomaticLight/LightGarage3").gameObject;
            MotionLogic = GameObject.Find("YARD").transform.Find("Building/Dynamics/HouseElectricity/ElectricAppliances/AutomaticLight").gameObject.GetPlayMaker("Logic");
        }

        // Update is called once per frame
        void Update()
        {
            if (UnifiedRaycast.GetHit(SwitchTrigCol))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (lightOn)
                    {
                        LightSwitchButton.transform.localEulerAngles = new Vector3(12f, 0f, 0f);
                        MainTweakMod.EntranceLightObj.SetActive(false);
                        lightOn = false;
                        LightObj.SetActive(false);
                        MotionLogic.enabled = true;
                        MainTweakMod.HouseKWH.Value += 0.005f;
                    }
                    else
                    {
                        LightSwitchButton.transform.localEulerAngles = new Vector3(345f, 0f, 0f);
                        MainTweakMod.EntranceLightObj.SetActive(true);
                        lightOn = true;
                        LightObj.SetActive(true);
                        MotionLogic.enabled = false;
                    }
                    SwitchSound.Play();
                }
            }
        }
    }
}