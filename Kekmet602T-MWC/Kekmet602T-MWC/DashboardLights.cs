using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections;

namespace Kekmet602T_MWC
{
    public class DashboardLights : MonoBehaviour
    {

        public FsmBool AccPower, PtoEnaged, HandbrakeOn;
        public MeshRenderer PtoLight, HandbrakeLight, TurnSignalLight, HiBeamLight, OilPresLight;
        public AudioSource TurnSignalSoundOne, TurnSignalSoundTwo;
        public bool TurnSignalRunning;
        public GameObject FrTurnSignal, LeftTurnSignal, RightTurnSignal, MarkerLights, BrakeLights;

        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        private MultiFunctionStickBehavior MultiFunctionStickBehaviorScript;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }


        void Start()
        {
            AccPower = MainKekmetMod.Kekmet.transform.Find("Simulation/STARTERxKekmet").gameObject.GetPlayMaker("Starter").FsmVariables.GetFsmBool("ACC");

            PtoEnaged = MainKekmetMod.KekmetLOD.transform.Find("Dashboard/PTO").gameObject.GetPlayMaker("Use").FsmVariables.GetFsmBool("PTO");
            HandbrakeOn = MainKekmetMod.Kekmet.transform.Find("Dashboard/ParkingBrake").gameObject.GetPlayMaker("Use").FsmVariables.GetFsmBool("Brake");

            TurnSignalSoundOne = MainKekmetMod.NewBody.transform.Find("TurnSignalAudioOne").gameObject.GetComponent<AudioSource>();
            TurnSignalSoundTwo = MainKekmetMod.NewBody.transform.Find("TurnSignalAudioTwo").gameObject.GetComponent<AudioSource>();

            PtoLight = MainKekmetMod.NewBody.transform.Find("IndicatorLights/PtoLightON").GetComponent<MeshRenderer>();
            HandbrakeLight = MainKekmetMod.NewBody.transform.Find("IndicatorLights/HandbrakeLightON").GetComponent<MeshRenderer>();

            TurnSignalLight = MainKekmetMod.NewBody.transform.Find("IndicatorLights/TurnSignalIndicatorON").GetComponent<MeshRenderer>();

            HiBeamLight = MainKekmetMod.NewBody.transform.Find("IndicatorLights/HeadLightIndicatorON").GetComponent<MeshRenderer>();
            OilPresLight = MainKekmetMod.NewBody.transform.Find("IndicatorLights/OilLightON").GetComponent<MeshRenderer>();

            MultiFunctionStickBehaviorScript = MainKekmetMod.NewBody.GetComponent<MultiFunctionStickBehavior>();

            MarkerLights = MainKekmetMod.NewBody.transform.Find("Lights/MarkerLights").gameObject;

            LeftTurnSignal = MainKekmetMod.NewBody.transform.Find("Lights/TurnSignalsActiveLeft").gameObject;

            RightTurnSignal = MainKekmetMod.NewBody.transform.Find("Lights/TurnSignalsActiveRight").gameObject;

            BrakeLights = MainKekmetMod.NewBody.transform.Find("Lights/BrakeLightsGlow").gameObject;




            PtoLight.enabled = false; HandbrakeLight.enabled = false; TurnSignalLight.enabled = false; HiBeamLight.enabled = false; OilPresLight.enabled = false; LeftTurnSignal.SetActive(false); RightTurnSignal.SetActive(false); MarkerLights.SetActive(false);
            BrakeLights.SetActive(false);

            OnEnable();
        }

        void OnEnable()
        {
            StopAllCoroutines();
            TurnSignalLight.enabled = false;
            TurnSignalSoundOne.Stop();
            TurnSignalSoundTwo.Stop();
            TurnSignalRunning = false;
            LeftTurnSignal.SetActive(false);
            RightTurnSignal.SetActive(false);
        }

        // Update is called once per frame..
        void Update()
        {

            if (AccPower.Value)
            {
                if (PtoEnaged.Value)
                {
                    PtoLight.enabled = true;
                }
                else
                {
                    PtoLight.enabled = false;
                }

                if (HandbrakeOn.Value)
                {
                    HandbrakeLight.enabled = true;
                }
                else
                {
                    HandbrakeLight.enabled = false;
                }

                if (MainKekmetMod.TractorDriveTrain.rpm <= 450f)
                {
                    OilPresLight.enabled = true;
                }
                else
                {
                    OilPresLight.enabled = false;
                }


                if (MultiFunctionStickBehaviorScript.LightMode == 2f)
                {
                    HiBeamLight.enabled = true;
                }
                else
                {
                    HiBeamLight.enabled = false;
                }

                if (MainKekmetMod.KekmetSeatCheck.Value == "Kekmet")
                {
                    if (cInput.GetAxisRaw("Brake") >= 0.1f)
                    {
                        BrakeLights.SetActive(true);
                    }
                    else
                    {
                        BrakeLights.SetActive(false);
                    }
                }
            }
            else
            {
                PtoLight.enabled = false; HandbrakeLight.enabled = false; TurnSignalLight.enabled = false; HiBeamLight.enabled = false; OilPresLight.enabled = false; BrakeLights.SetActive(false); MarkerLights.SetActive(false);

            }

            if (MultiFunctionStickBehaviorScript.LightMode >= 1f)
            {
                MarkerLights.SetActive(true);
            }
            else
            {
                MarkerLights.SetActive(false);

            }
        }

            public IEnumerator TurnSignalBehavior()
        {

            while (MultiFunctionStickBehaviorScript.LeftTurnToggle || MultiFunctionStickBehaviorScript.RightTurnToggle)
            {
                TurnSignalRunning = true;
                if (AccPower.Value)
                {
                    TurnSignalSoundTwo.Stop();
                    TurnSignalSoundOne.Play();
                    TurnSignalLight.enabled = true;

                    if (MultiFunctionStickBehaviorScript.LeftTurnToggle)
                    {
                        LeftTurnSignal.SetActive(true);
                    }
                    else
                    {
                        LeftTurnSignal.SetActive(false);
                    }

                    if (MultiFunctionStickBehaviorScript.RightTurnToggle)
                    {
                        RightTurnSignal.SetActive(true);
                    }
                    else
                    {
                        RightTurnSignal.SetActive(false);
                    }

                }
                else
                {
                    TurnSignalSoundOne.Stop();
                    TurnSignalSoundTwo.Stop();
                    TurnSignalLight.enabled = false;
                    LeftTurnSignal.SetActive(false);
                    RightTurnSignal.SetActive(false);
                }

                yield return new WaitForSeconds(0.4f);
                if (AccPower.Value)
                {
                    TurnSignalSoundOne.Stop();
                    TurnSignalSoundTwo.Play();
                    TurnSignalLight.enabled = false;
                    LeftTurnSignal.SetActive(false);
                    RightTurnSignal.SetActive(false);
                }
                else
                {
                    TurnSignalSoundOne.Stop();
                    TurnSignalSoundTwo.Stop();
                    TurnSignalLight.enabled = false;
                    LeftTurnSignal.SetActive(false);
                    RightTurnSignal.SetActive(false);
                }

                yield return new WaitForSeconds(0.4f);
            }
            TurnSignalLight.enabled = false;
            TurnSignalSoundOne.Stop();
            TurnSignalSoundTwo.Stop();
            TurnSignalRunning = false;
            LeftTurnSignal.SetActive(false);
            RightTurnSignal.SetActive(false);
            yield break;
        }
    }
}