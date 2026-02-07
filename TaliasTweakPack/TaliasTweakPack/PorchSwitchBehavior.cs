using MSCLoader;
using UnityEngine;

namespace TaliasTweakPack
{
    public class PorchSwitchBehavior : MonoBehaviour
    {
        public GameObject LightSwitchButton;
        public SphereCollider SwitchTrigCol;
        public bool lightOn;
        public AudioSource SwitchSound;
        public MeshRenderer PorchLightMat;

        // Use this for initialization
        private TaliasTweakPack MainTweakMod;
        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;
        }
        void Start()
        {
            LightSwitchButton = MainTweakMod.PorchSwitchObj.transform.Find("lights_switch/lights_switch_button").gameObject;
            LightSwitchButton.transform.localEulerAngles = new Vector3(12f, 0f, 0f);

            SwitchSound = MainTweakMod.PorchSwitchObj.GetComponent<AudioSource>();
            SwitchTrigCol = MainTweakMod.PorchSwitchObj.GetComponent<SphereCollider>();
            PorchLightMat = MainTweakMod.PorchLightVis.transform.Find("default").GetComponent<MeshRenderer>();
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
                        MainTweakMod.PorchLightObj.SetActive(false);
                        lightOn = false;
                        MainTweakMod.HouseKWH.Value += 0.005f;

                        PorchLightMat.material = MainTweakMod.PorchLightOff;
                    }
                    else
                    {
                        LightSwitchButton.transform.localEulerAngles = new Vector3(345f, 0f, 0f);
                        MainTweakMod.PorchLightObj.SetActive(true);
                        lightOn = true;
                        if (MainTweakMod.HouseApplinces.activeSelf == true)
                        {
                            PorchLightMat.material = MainTweakMod.PorchLightOn;
                        }
                    }
                    SwitchSound.Play();
                }
            }
        }
    }
}