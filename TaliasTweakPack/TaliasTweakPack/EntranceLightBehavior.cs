using MSCLoader;
using UnityEngine;

namespace TaliasTweakPack
{
    public class EntranceLightBehavior : MonoBehaviour
    {

        public GameObject LightSwitchButton;
        public SphereCollider SwitchTrigCol;
        public bool lightOn;
        public AudioSource SwitchSound;

        // Use this for initialization
        private TaliasTweakPack MainTweakMod;
        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;
        }
        void Start()
        {

            LightSwitchButton = MainTweakMod.EntranceSwitchObj.transform.Find("lights_switch/lights_switch_button").gameObject;
            LightSwitchButton.transform.localEulerAngles = new Vector3(12f, 0f, 0f);

            SwitchSound = MainTweakMod.EntranceSwitchObj.GetComponent<AudioSource>();
            SwitchTrigCol = MainTweakMod.EntranceSwitchObj.GetComponent<SphereCollider>();

            
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
                        MainTweakMod.HouseKWH.Value += 0.0028f;
                    }
                    else
                    {
                        LightSwitchButton.transform.localEulerAngles = new Vector3(345f, 0f, 0f);
                        MainTweakMod.EntranceLightObj.SetActive(true);
                        lightOn = true;
                    }
                    SwitchSound.Play();
                }
            }

        }
    }
}