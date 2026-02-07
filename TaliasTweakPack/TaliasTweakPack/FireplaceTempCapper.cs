using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace TaliasTweakPack
{
    public class FireplaceTempCapper : MonoBehaviour
    {

        public FsmFloat FirePlaceTemp;
        // Use this for initialization
        void Start()
        {
            FirePlaceTemp = GameObject.Find("YARD/Building/LIVINGROOM/Fireplace/HeatSourceFireplaceHouse").GetPlayMaker("Data").FsmVariables.GetFsmFloat("Temperature");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
                if (FirePlaceTemp.Value > 45f)
                {
                    FirePlaceTemp.Value = 45f;
                }       
        }
    }
}