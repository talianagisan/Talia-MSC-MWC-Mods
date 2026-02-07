using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using System.Collections;

namespace Kekmet602T_MWC
{
    public class HeatFunctions : MonoBehaviour
    {

        public bool BlindDown, OverheatPause, FanPause, IgnitionSoundPause, ThermostartToggle;
        public Collider BlindTrigCol, FanTrigCol, IgnitionCol;
        public AudioSource BlindSound, FanSound,IgnitionSound;
        public FsmFloat CoolingModifier, HeatGeneration, EngineTemp, StartTime, AmbientTemp, CabinTemp, FuelLevel;
        public PlayMakerFSM HandThrottle;
        public float FanSpeed, MaxTemp, CabinTempGenDebug, AmbientTempDebug, IntakeHeatLevel, MaxTempIntake;
        public GameObject FanSwitchObj, FanSoundObj, FlameHeaterSound, KeyPivot;
        public int FanMode;
        private FsmBool AccPowerInt;
        private PlayMakerFSM KeyFSM, StarterFSM;

        public float[] FanSpeeds =
        {
            0f,0.5f,1f,0.5f

        };


        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {
            MainKekmetMod.MainBodyAnimation.Play("BlindUp");

            BlindTrigCol = MainKekmetMod.NewBody.transform.Find("RadiatorBlind").GetComponent<Collider>();

            BlindSound = MainKekmetMod.NewBody.transform.Find("RadiatorBlind").GetComponent<AudioSource>();

            CoolingModifier = MainKekmetMod.Kekmet.transform.Find("Simulation/Cooling").gameObject.GetPlayMaker("Cooling").FsmVariables.GetFsmFloat("CoolingModifier");
            HeatGeneration = MainKekmetMod.Kekmet.transform.Find("Simulation/Cooling").gameObject.GetPlayMaker("Cooling").FsmVariables.GetFsmFloat("HeatGeneration");
            EngineTemp = MainKekmetMod.Kekmet.transform.Find("Simulation/Cooling").gameObject.GetPlayMaker("Cooling").FsmVariables.GetFsmFloat("EngineTemp");

            StartTime = MainKekmetMod.Kekmet.transform.Find("Simulation/STARTERxKekmet").gameObject.GetPlayMaker("Starter").FsmVariables.GetFsmFloat("StartTime");

            HandThrottle = MainKekmetMod.KekmetLOD.transform.Find("Dashboard/Throttle").gameObject.GetPlayMaker("Throttle");

            MainKekmetMod.Kekmet.transform.Find("Simulation/Cooling").gameObject.GetPlayMaker("Cooling").GetState("Reset").Actions[1].Enabled = false;
            MainKekmetMod.Kekmet.transform.Find("Simulation/Cooling").gameObject.GetPlayMaker("Cooling").GetState("Reset").Actions[3].Enabled = false;

            MainKekmetMod.Kekmet.transform.Find("Simulation/IGNITIONxKekmet").gameObject.GetPlayMaker("Use").GetState("Motor OFF").Actions[8].Enabled = false;

            AmbientTemp = FsmVariables.GlobalVariables.GetFsmFloat("AmbientTemperature");

            CabinTemp = MainKekmetMod.Kekmet.transform.Find("HeatSourceKekmet").gameObject.GetPlayMaker("Data").FsmVariables.GetFsmFloat("Temperature");

            FanTrigCol = MainKekmetMod.NewBody.transform.Find("HeaterTrig").gameObject.GetComponent<Collider>();
            FanSoundObj = MainKekmetMod.NewBody.transform.Find("BlowerAudio").gameObject;
            FanSound = MainKekmetMod.NewBody.transform.Find("BlowerAudio").gameObject.GetComponent<AudioSource>();
            FanSwitchObj = MainKekmetMod.NewBody.transform.Find("HeatFanSwitch").gameObject;

            AccPowerInt = MainKekmetMod.Kekmet.transform.Find("Simulation/STARTERxKekmet").gameObject.GetPlayMaker("Starter").FsmVariables.GetFsmBool("ACC");
            StarterFSM = MainKekmetMod.Kekmet.transform.Find("Simulation/STARTERxKekmet").gameObject.GetPlayMaker("Starter");

            KeyFSM = MainKekmetMod.Kekmet.transform.Find("Simulation/IGNITIONxKekmet").gameObject.GetPlayMaker("Use");


            MainKekmetMod.KekmetLOD.transform.Find("Dashboard/ButtonWipers").localPosition = new Vector3(0.1626f, -0.492f, 1.7775f);
            MainKekmetMod.KekmetLOD.transform.Find("Dashboard/ButtonWipers").localEulerAngles = new Vector3(0f, 0f, 0f);
            MainKekmetMod.KekmetLOD.transform.Find("Dashboard/ButtonWipers").localScale = new Vector3(0.8000002f, 0.8000002f, 1f);

            IgnitionCol = MainKekmetMod.Kekmet.transform.Find("Simulation/IGNITIONxKekmet").gameObject.GetComponent<SphereCollider>();

            FuelLevel = MainKekmetMod.Kekmet.transform.Find("FuelTankKekmet").gameObject.GetPlayMaker("Data").FsmVariables.GetFsmFloat("FuelLevel");

            IgnitionSound = MainKekmetMod.NewBody.transform.Find("IgnitionKeySound").gameObject.GetComponent<AudioSource>();

            FlameHeaterSound = MainKekmetMod.NewBody.transform.Find("FlameHeaterSound").gameObject;

            KeyPivot = MainKekmetMod.KekmetLOD.transform.Find("Dashboard/KnobIgnition/Pivot").gameObject;

        }

        // Update is called once per frame..
        void Update()
        {

            if (UnifiedRaycast.GetHit(IgnitionCol))
            {
                if (ThermostartToggle)
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                    PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Ignition";
                }


                if (Input.GetKeyDown(KeyCode.Mouse1) && AccPowerInt.Value == false)
                {
                    if (ThermostartToggle)
                    {
                        IgnitionSound.Play();
                        FlameHeaterSound.SetActive(false);
                        ThermostartToggle = false;
                        KeyPivot.transform.localEulerAngles = new Vector3(0, 0, 0);
                        KeyFSM.enabled = true;
                    }
                    else
                    {
                        KeyFSM.enabled = false;
                        IgnitionSound.Play();
                        FlameHeaterSound.SetActive(true);
                        ThermostartToggle = true;
                        KeyPivot.transform.localEulerAngles = new Vector3(0, 30f, 0);

                    }
                }

            }

            if (ThermostartToggle)
            {
                if (FuelLevel.Value > 2f)
                {
                    IntakeHeatLevel = Mathf.Clamp(IntakeHeatLevel += (70f / 6000f) + (AmbientTemp.Value / 12000f), AmbientTemp.Value, MaxTempIntake);
                    FuelLevel.Value += -0.0001f;

                }
            }

            if (MainKekmetMod.TractorDriveTrain.rpm < 300f)
            {
                IntakeHeatLevel = Mathf.Clamp(IntakeHeatLevel += (AmbientTemp.Value / 12000f), AmbientTemp.Value, MaxTempIntake);
            }
            

            if (EngineTemp.Value >= 95f)
            {
                StartTime.Value = 240f;

            }
            else if(EngineTemp.Value > 20f && EngineTemp.Value < 95f)
            {
                IntakeHeatLevel = MaxTempIntake;

            }
            else
            {
                if (IntakeHeatLevel < 0)
                {
                    StartTime.Value = 8 - IntakeHeatLevel;
                }
                else
                {
                    StartTime.Value = 8 - (IntakeHeatLevel * 0.34722222222f);
                }
            }




            MaxTempIntake = 21 + AmbientTemp.Value * 0.42f;


            if (UnifiedRaycast.GetHit(BlindTrigCol))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                if (!BlindDown)
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Cover Radiator";
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        BlindDown = true;
                        MainKekmetMod.MainBodyAnimation.Play("BlindDown");
                        BlindSound.Play();

                        CoolingModifier.Value = 1f;
                        HeatGeneration.Value = 1750f;
                    }
                }
                else
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Uncover Radiator";
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        MainKekmetMod.MainBodyAnimation.Play("BlindUp");
                        BlindDown = false;
                        BlindSound.Play();

                        CoolingModifier.Value = 20000f;
                        HeatGeneration.Value = 20000f;
                    }
                }
            }

            if (UnifiedRaycast.GetHit(FanTrigCol))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Blower";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    FanPause = false;
                    if (FanMode == 0 && !FanPause)
                    {
                        FanSwitchObj.transform.localEulerAngles = new Vector3(0, 0, 0f);
                        FanMode = 1;
                        FanSound.volume = 0.5f;
                        FanPause = true;
                    }

                    if (FanMode == 1 && !FanPause)
                    {
                        FanSwitchObj.transform.localEulerAngles = new Vector3(0, 0, -9f);
                        FanMode = 2;
                        FanSound.volume = 1f;
                        FanPause = true;
                    }

                    if (FanMode == 2 && !FanPause)
                    {
                        FanSwitchObj.transform.localEulerAngles = new Vector3(0, 0, 9f);
                        FanMode = 0;
                        FanSound.volume = 0f;
                        FanPause = true;
                    }

                    /*if (FanMode == 3 && !FanPause)
                    {
                        FanSwitchObj.transform.localEulerAngles = new Vector3(0, 0, 9f);
                        FanMode = 0;
                        FanSound.volume = 0.5f;
                        FanPause = true;
                    }*/
                    MainKekmetMod.ButtonSound.Play();
                }
            }



            if (AccPowerInt.Value)
            {
                if (FanMode >= 1f)
                {
                    FanSoundObj.SetActive(true);
                    FanSpeed = FanSpeeds[FanMode];
                }
            }
            else
            {
                FanSoundObj.SetActive(false);
                FanSpeed = 0f;
            }



            if (EngineTemp.Value > 130f & OverheatPause == false)
            {
                StartCoroutine(OverheatShutDown());

                OverheatPause = true;
            }

            if (OverheatPause == true)
            {
                MainKekmetMod.TractorDriveTrain.minRPM = 0f;
            }
            

            if(EngineTemp.Value < 0)
            {
                MaxTemp = EngineTemp.Value;
            }
            else
            {
                MaxTemp = EngineTemp.Value / 3.57142857143f;
            }

            CabinTemp.Value = Mathf.Clamp(CabinTemp.Value += ((EngineTemp.Value*FanSpeed)/12000f)+(AmbientTemp.Value/ 12000f), AmbientTemp.Value, MaxTemp);
            //CabinTempGenDebug = ((EngineTemp.Value * FanSpeed) / 12000f) + (AmbientTemp.Value / 12000f);
            //AmbientTempDebug = AmbientTemp.Value;

        }

        public IEnumerator OverheatShutDown()
        {

            while (EngineTemp.Value > 90f)
            {
                OverheatPause = true;

                MainKekmetMod.TractorDriveTrain.maxPower = 0.1f;
                MainKekmetMod.TractorDriveTrain.maxTorque = 0.1f;
                HandThrottle.enabled = false;
                StartTime = Mathf.Infinity;
                yield return null;
            }


            yield return new WaitForSeconds(5f);

            MainKekmetMod.TractorDriveTrain.maxPower = 82.16f;
            MainKekmetMod.TractorDriveTrain.maxTorque = 289.58f;
            OverheatPause = false;
            HandThrottle.enabled = true;
        }
    }
}