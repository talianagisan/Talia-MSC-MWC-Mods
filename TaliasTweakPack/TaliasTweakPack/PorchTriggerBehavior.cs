using MSCLoader;
using UnityEngine;

namespace TaliasTweakPack
{
    public class PorchTriggerBehavior : MonoBehaviour
    {

        // Use this for initialization
        public PorchSwitchBehavior PorchSwitchScript;
        public MeshRenderer PorchLightMat;

        private TaliasTweakPack MainTweakMod;

        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;
        }
        void Start()
        {
            PorchSwitchScript = MainTweakMod.PorchSwitchObj.GetComponent<PorchSwitchBehavior>();
            PorchLightMat = MainTweakMod.PorchLightVis.transform.Find("default").GetComponent<MeshRenderer>();
        }
        
        void OnTriggerStay(Collider ObjectMoving)
        {
            if (ObjectMoving.name == "PLAYER" && !PorchSwitchScript.lightOn)
            {
                MainTweakMod.PorchLightObj.SetActive(true);
                if(MainTweakMod.HouseApplinces.activeSelf == true)
                {
                    PorchLightMat.material = MainTweakMod.PorchLightOn;
                }
                
            }
        }

        void OnTriggerExit(Collider ObjectMoving)
        {
            if (!PorchSwitchScript.lightOn)
            {
                MainTweakMod.PorchLightObj.SetActive(false);
                PorchLightMat.material = MainTweakMod.PorchLightOff;
            }
        }
    }
}