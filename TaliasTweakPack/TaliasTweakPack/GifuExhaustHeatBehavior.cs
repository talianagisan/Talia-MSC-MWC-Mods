using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace TaliasTweakPack
{
    public class GifuExhaustHeatBehavior : MonoBehaviour
    {

        // Use this for initialization
        private TaliasTweakPack MainTweakMod;
        public FsmFloat PlayerAmbientTemp, RainTempCheck;
        public GameObject Gifu;
        public Drivetrain GifuDriveTrain;
        public PlayMakerFSM RainTempFsm;
        public bool Pause;

        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;
        }
        void Start()
        {
            PlayerAmbientTemp = GameObject.Find("PLAYER/BodyTemp").GetPlayMaker("Calculations").GetVariable<FsmFloat>("Temperature");
            RainTempCheck = GameObject.Find("PLAYER/Rain").GetPlayMaker("RoofCheck").GetVariable<FsmFloat>("Temperature");
            RainTempFsm = GameObject.Find("PLAYER/Rain").GetPlayMaker("RoofCheck");
            Gifu = GameObject.Find("GIFU(750/450psi)");
            GifuDriveTrain = Gifu.GetComponent<Drivetrain>();
        }

        // Update is called once per frame.
        void Update()
        {

        }

        public void OnTriggerStay(Collider HeatZoneObject)
        {
            if (GifuDriveTrain.rpm > 200f)
            {
                if (HeatZoneObject.gameObject.name == "PLAYER")
                {
                    PlayerAmbientTemp.Value = 21f;
                    RainTempCheck.Value = 21f;
                    RainTempFsm.enabled = false;
                    Pause = true;
                }
            }
        }

        public void OnTriggerExit(Collider HeatZoneObject)
        {
            RainTempFsm.enabled = true;
        }
    }
}