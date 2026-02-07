using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;

namespace TaliasTweakPack
{
    public class PskTempSignBehavior : MonoBehaviour
    {

        // Use this for initialization
        public TextMesh TempTextOne, TempTextTwo, TempTextThree;
        public FsmFloat AmbientTemp;

        private TaliasTweakPack MainTweakMod;
        public void SetupModClass(TaliasTweakPack mainMod)
        {
            MainTweakMod = mainMod;
        }
        void Start()
        {
            TempTextOne = MainTweakMod.PskTempSign.transform.Find("TempTexts/TextOne").GetComponent<TextMesh>();
            TempTextTwo = MainTweakMod.PskTempSign.transform.Find("TempTexts/TextTwo").GetComponent<TextMesh>();
            TempTextThree = MainTweakMod.PskTempSign.transform.Find("TempTexts/TextThree").GetComponent<TextMesh>();

            AmbientTemp = FsmVariables.GlobalVariables.GetFsmFloat("AmbientTemperature");

        }

        // Update is called once per frame.
        void Update()
        {


            TempTextOne.text = TempTextTwo.text = TempTextThree.text = AmbientTemp.Value.ToString("#.00");
        }
    }
}