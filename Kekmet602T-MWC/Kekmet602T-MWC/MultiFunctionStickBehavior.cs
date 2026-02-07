using MSCLoader;
using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

namespace Kekmet602T_MWC
{
    public class MultiFunctionStickBehavior : MonoBehaviour
    {
        public GameObject HiBeams, LoBeams, StickPivot;
        public FsmBool AccPowered;
        public Collider LightTrigger, RightTurnTrig, LeftTurnTrigger;
        public bool RightTurnToggle, LeftTurnToggle, HeadlightDelay;
        public AudioSource StickSound, HornSound;
        public float LightMode;


        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        private DashboardLights DashboardLightsScript;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {

            AccPowered = MainKekmetMod.Kekmet.transform.Find("Simulation/STARTERxKekmet").gameObject.GetPlayMaker("Starter").FsmVariables.GetFsmBool("ACC");

            MainKekmetMod.KekmetLOD.transform.Find("Dashboard/ButtonLightModes").gameObject.SetActive(false);

            StickPivot = MainKekmetMod.KekmetLOD.transform.Find("Dashboard/KnobLights/Pivot").gameObject;

            LightTrigger = MainKekmetMod.NewBody.transform.Find("MultiFunctionStickTriggers/LightTrigger").gameObject.GetComponent<Collider>();
            RightTurnTrig = MainKekmetMod.NewBody.transform.Find("MultiFunctionStickTriggers/RightTurnTrig").gameObject.GetComponent<Collider>();
            LeftTurnTrigger = MainKekmetMod.NewBody.transform.Find("MultiFunctionStickTriggers/LeftTurnTrig").gameObject.GetComponent<Collider>();


            StickSound = MainKekmetMod.NewBody.transform.Find("StickSound").gameObject.GetComponent<AudioSource>();
            HornSound = MainKekmetMod.NewBody.transform.Find("HornAudio").gameObject.GetComponent<AudioSource>();
            LightMode = 0f;

            HiBeams = MainKekmetMod.KekmetLOD.transform.Find("Electrics/PowerOn/BeamsLong").gameObject;
            LoBeams = MainKekmetMod.KekmetLOD.transform.Find("Electrics/PowerOn/BeamsShort").gameObject;

            MainKekmetMod.KekmetLOD.transform.Find("Electrics").gameObject.GetPlayMaker("Status").enabled = false;
            MainKekmetMod.KekmetLOD.transform.Find("Electrics/PowerOn").gameObject.SetActive(true);

            DashboardLightsScript = MainKekmetMod.NewBody.GetComponent<DashboardLights>();
            OnEnable();
        }

        void OnEnable()
        {
            StickPivot.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
            LeftTurnToggle = false; RightTurnToggle = false;
        }

        // Update is called once per frame.
        void Update()
        {
            if (UnifiedRaycast.GetHit(LightTrigger))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Multi Function Stick (Headlights)";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    HeadlightDelay = false;
                    if (LightMode == 0f && HeadlightDelay == false)
                    {
                        LoBeams.SetActive(true);
                        LightMode = 1f;
                        HeadlightDelay = true;

                    }

                    if (LightMode == 1f && HeadlightDelay == false)
                    {
                        LoBeams.SetActive(false);
                        HiBeams.SetActive(true);
                        LightMode = 2f;
                        HeadlightDelay = true;
                    }

                    if (LightMode == 2f && HeadlightDelay == false)
                    {
                        HiBeams.SetActive(false);
                        LightMode = 0f;
                        HeadlightDelay = true;
                    }
                    StickSound.Play();
                    StartCoroutine(LightToggleAnim());
                }



                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    HornSound.Play();
                }

            }


            if (MainKekmetMod.KekmetSeatCheck.Value == "Kekmet")
            {

                if (cInput.GetButtonDown("IndicatorLeft"))
                {
                    if (!LeftTurnToggle)
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(15f, 0f, 0f);
                        LeftTurnToggle = true;
                        RightTurnToggle = false;

                        if (DashboardLightsScript.TurnSignalRunning == false)
                        {
                            StartCoroutine(DashboardLightsScript.TurnSignalBehavior());
                        }

                    }
                    else
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                        LeftTurnToggle = false;

                    }
                    StickSound.Play();
                }


                if (cInput.GetButtonDown("IndicatorRight"))
                {
                    if (!RightTurnToggle)
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(345f, 0f, 0f);
                        RightTurnToggle = true;
                        LeftTurnToggle = false;

                        if (DashboardLightsScript.TurnSignalRunning == false)
                        {
                            StartCoroutine(DashboardLightsScript.TurnSignalBehavior());
                        }


                    }
                    else
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                        RightTurnToggle = false;
                    }
                    StickSound.Play();
                }


                if (cInput.GetButtonDown("LightsDrive"))
                {
                    HeadlightDelay = false;
                    if (LightMode == 0f && HeadlightDelay == false)
                    {
                        LoBeams.SetActive(true);
                        LightMode = 1f;
                        HeadlightDelay = true;

                    }

                    if (LightMode == 1f && HeadlightDelay == false)
                    {
                        LoBeams.SetActive(false);
                        HiBeams.SetActive(false);
                        LightMode = 0f;
                        HeadlightDelay = true;
                    }


                    if (LightMode == 2f && HeadlightDelay == false)
                    {
                        LoBeams.SetActive(false);
                        HiBeams.SetActive(false);
                        LightMode = 0f;
                        HeadlightDelay = true;
                    }
                    StickSound.Play();
                    StartCoroutine(LightToggleAnim());
                }

                if (cInput.GetButtonDown("HiBeamToggle"))
                {
                    HeadlightDelay = false;
                    if (LightMode == 0f && HeadlightDelay == false)
                    {
                        HiBeams.SetActive(true);
                        LightMode = 2f;
                        HeadlightDelay = true;

                    }

                    if (LightMode == 1f && HeadlightDelay == false)
                    {
                        LoBeams.SetActive(false);
                        HiBeams.SetActive(true);
                        LightMode = 2f;
                        HeadlightDelay = true;
                    }

                    if (LightMode == 2f && HeadlightDelay == false)
                    {
                        HiBeams.SetActive(false);
                        LoBeams.SetActive(true);
                        LightMode = 1f;
                        HeadlightDelay = true;
                    }
                    StickSound.Play();
                    StartCoroutine(LightToggleAnim());
                }

            }

            if (UnifiedRaycast.GetHit(RightTurnTrig))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Multi Function Stick (Right Turnsignal)";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!RightTurnToggle)
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(345f, 0f, 0f);
                        RightTurnToggle = true;
                        LeftTurnToggle = false;

                        if(DashboardLightsScript.TurnSignalRunning == false)
                        {
                            StartCoroutine(DashboardLightsScript.TurnSignalBehavior());
                        }

                        
                    }
                    else
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                        RightTurnToggle = false;
                    }
                    StickSound.Play();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    HornSound.Play();
                }

            }

            if (UnifiedRaycast.GetHit(LeftTurnTrigger))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Multi Function Stick (Left Turnsignal)";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!LeftTurnToggle)
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(15f, 0f, 0f);
                        LeftTurnToggle = true;
                        RightTurnToggle = false;

                        if (DashboardLightsScript.TurnSignalRunning == false)
                        {
                            StartCoroutine(DashboardLightsScript.TurnSignalBehavior());
                        }

                    }
                    else
                    {
                        StickPivot.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                        LeftTurnToggle = false;
                    }
                    StickSound.Play();
                }

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    HornSound.Play();
                }               
            }



        }

        public IEnumerator LightToggleAnim()
        {
            StickPivot.transform.Rotate(0, 350, 0);
            yield return new WaitForSeconds(0.25f);
            StickPivot.transform.Rotate(0, -350f, 0);
            yield return new WaitForSeconds(0.25f);
        }
    }
}