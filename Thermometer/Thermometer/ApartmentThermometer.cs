using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections;

namespace Thermometer
{
    public class ApartmentThermometer : MonoBehaviour
    {
        public GameObject ApartmentNoRain, ApartmentThermometerObj;
        public FsmFloat ApartmentTemp;
        public TextMesh ApartmentThermometerText;
        public bool Pause = false;
        // Use this for initialization
        void Start()
        {
            ApartmentThermometerObj = GameObject.Find("Apartment Thermometer");

            ApartmentNoRain = GameObject.Find("HOMENEW/NoRainApartment");

            ApartmentThermometerText = ApartmentThermometerObj.GetComponentInChildren<TextMesh>();

            ApartmentTemp = ApartmentNoRain.GetPlayMaker("Data").FsmVariables.GetFsmFloat("Temp");

        }

        // Update is called once per frame.
        void Update()
        {
            if (ApartmentTemp.Value > 27f)
            {
                ApartmentThermometerText.text = "H I";
            }
            else if (ApartmentTemp.Value < 8f & ApartmentTemp.Value >= 5f)
            {
                ApartmentThermometerText.text = "L O";
                StopAllCoroutines();
                Pause = false;
            }
            else if (ApartmentTemp.Value < 5f)
            {
                if (Pause == false)
                {
                    StartCoroutine(BlinkHouse());
                }
            }
            else if (ApartmentTemp.Value >= 8f & ApartmentTemp.Value <= 27f)
            {
                ApartmentThermometerText.text = ApartmentTemp.Value.ToString("#.00");
            }
        }

        public IEnumerator BlinkHouse()
        {

            Pause = true;
            while (ApartmentTemp.Value < 5f)
            {
                ApartmentThermometerText.text = "FRZ";
                yield return new WaitForSeconds(1.5f);
                ApartmentThermometerText.text = "L O";
                yield return new WaitForSeconds(1.5f);
            }

        }

    }
}