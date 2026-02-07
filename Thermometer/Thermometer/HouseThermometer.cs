using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections;

namespace Thermometer
{
    public class HouseThermometer : MonoBehaviour
    {
        public GameObject HouseNoRain, HouseThermometerObj;
        public FsmFloat HouseTemp;
        public TextMesh HouseThermometerText;
        public bool Pause = false;

        // Use this for initialization
        void Start()
        {
            HouseThermometerObj = GameObject.Find("House Thermometer");

            HouseNoRain = GameObject.Find("YARD/Building/NoRainHouse1");


            HouseTemp = HouseNoRain.GetPlayMaker("Data").FsmVariables.GetFsmFloat("Temp");

            HouseThermometerText = HouseThermometerObj.GetComponentInChildren<TextMesh>();
        }

        // Update is called once per frame..
        void Update()
        {

            if (HouseTemp.Value > 27f)
            {
                HouseThermometerText.text = "H I";
            }
            else if (HouseTemp.Value < 8f & HouseTemp.Value >= 5f)
            {
                HouseThermometerText.text = "L O";
                StopAllCoroutines();
                Pause = false;
            }
            else if (HouseTemp.Value < 5f)
            {
                if (Pause == false)
                {
                    StartCoroutine(BlinkHouse());
                }
            }
            else if(HouseTemp.Value >= 8f & HouseTemp.Value <= 27f)
            {
                HouseThermometerText.text = HouseTemp.Value.ToString("#.00");
            }
        }

        public IEnumerator BlinkHouse()
        {

            Pause = true;
            while (HouseTemp.Value < 5f)
            {
                HouseThermometerText.text = "FRZ";
                yield return new WaitForSeconds(1.5f);
                HouseThermometerText.text = "L O";
                yield return new WaitForSeconds(1.5f);
            }
           
        }
    }
}