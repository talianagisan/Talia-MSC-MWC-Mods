using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace TaliasTweakPack
{
    public class SplashZoneBehavior : MonoBehaviour
    {

        public FsmBool Spilling;
        public FsmFloat Dirtiness, Stress;
        public GameObject Player;

        // Use this for initialization
        private TaliasTweakPack MainTweakMod;
        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;
        }
        public
        void Start()
        {
            Spilling = MainTweakMod.Gifu.transform.Find("ShitTank").GetPlayMaker("SpillPump").GetVariable<FsmBool>("SpillPump");
            Dirtiness = FsmVariables.GlobalVariables.GetFsmFloat("PlayerDirtiness");
            Stress = FsmVariables.GlobalVariables.GetFsmFloat("PlayerStress");
            Player = GameObject.Find("PLAYER");
 
        }

       
        // Update is called once per frame.
        void Update()
        {

        }

        public void OnTriggerStay(Collider PoorBastard)
        {
            if (Spilling.Value)
            {
                if (PoorBastard.gameObject.name == "PLAYER")
                {
                    Stress.Value += 0.08f;
                    Dirtiness.Value = 150f;
                    MasterAudio.PlaySound3DAndForget("Shit", Player.transform, false, 1f, null, 0f, "shit01");
                   
                }
            }
        }

    }
}