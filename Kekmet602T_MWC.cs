using HutongGames.PlayMaker;
using MSCLoader;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static CarDynamics;

namespace Kekmet602T_MWC
{
    public class Kekmet602T_MWC : Mod
    {
        public override string ID => "Kekmet602T_MWC"; // Your (unique) mod ID 
        public override string Name => "Kekmet602-T (MWC Edition)"; // Your mod name
        public override string Author => "TaliaKuznetsova"; // Name of the Author (your name)
        public override string Version => "1.0"; // Version
        public override string Description => "Changes the Kekmet into it's bigger 602 Turbocharged version"; // Short description of your mod

        public override Game SupportedGames => Game.MyWinterCar;

        public FsmBool KekmetRange;

        public bool HiTrac;

        public Drivetrain TractorDriveTrain;

        public AxisCarController KekmetController;

        public Axles FrontAxle;

        public Wheel FL_Wheel;
        public Wheel FR_Wheel;
        public Wheel RL_Wheel;
        public Wheel RR_Wheel;

        public SettingsKeybind HiTracSelector, BrakeLeft, BrakeRight;

        public SettingsCheckBox HiTracDefault;

        public GameObject Kekmet;
        public GameObject OldBody;

        public GameObject FL_Wheel_OBJ;
        public GameObject FR_Wheel_OBJ;
        public GameObject RL_Wheel_OBJ;
        public GameObject RR_Wheel_OBJ;

        public GameObject AirFilter;
        public GameObject NewBody;

        public GameObject BrakePivotExisting;
        public GameObject OldBrakePedal;

        public GameObject BrakeNewL;
        public GameObject BrakeNewR;

        public GameObject Rim;
        public GameObject Tire;

        public GameObject FRRim;
        public GameObject FLRim;
        public GameObject FRWheel;
        public GameObject FLWheel;

        public GameObject FRRimVis;
        public GameObject FLRimVis;
        public GameObject FRWheelVis;
        public GameObject FLWheelVis;

        public GameObject TurboSoundObj;

        public GameObject RangeShiftSource;

        public GameObject RangeLever;

        public GameObject OldLights;
        public GameObject NewLights;
        public GameObject LightSwitch;

        public AudioSource RangeShiftSound;

        public AudioSource TractorTurboSound;

        public float SoundBoost;

        public float BrakeForceModifier;

        public SettingsSlider BrakeForceSlider;

        public GameObject RearLights;
        public GameObject FrontLights;
        public GameObject SideLights;

        public GameObject SideLightTrigger;
        public GameObject FrontLightTrigger;

        public GameObject SideLightSwitch;
        public GameObject FrontLightSwitch;

        public bool Pause;
        public bool FrontLightsToggled;
        public bool SideLightsToggled;

        public GameObject QuadForLights;

        public GameObject Light1Obj;
        public GameObject Light2Obj;
        public GameObject Light3Obj;
        public GameObject Light4Obj;
        public GameObject Light5Obj;
        public GameObject Light6Obj;

        public Light Light1;
        public Light Light2;
        public Light Light3;
        public Light Light4;
        public Light Light5;
        public Light Light6;

        public Flare Lightflare;

        public GameObject LeftLightExisting;

        public GameObject ButtonSoundObj;
        public AudioSource ButtonSound;


        public override void ModSetup()
        {
            SetupFunction(Setup.OnNewGame, Mod_OnNewGame);
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.ModSettings, Mod_Settings);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.Update, Mod_PostLoad);
        }

        private void Mod_Settings()
        {
            HiTracSelector = Keybind.Add("Kekmet602HiTracToggleKey", "Hi-Trac toggle keybind", KeyCode.Keypad5);

            BrakeLeft = Keybind.Add("BrakeLeftKey", "Press to brake just the left wheel", KeyCode.A, KeyCode.LeftAlt);
            BrakeRight = Keybind.Add("BrakeRightKey", "Press to brake just the right wheel", KeyCode.D, KeyCode.LeftAlt);

            HiTracDefault = Settings.AddCheckBox("HiTracDefaultSetting", "Is HiTrac enabled by default?", false);

            BrakeForceSlider = Settings.AddSlider("BrakeForceSlider", "Sets the brake force of the split brakes. 1 = 100%.", 0f, 1f, 1f, ApplySettings);

        }

        private void ApplySettings()
        {
            BrakeForceModifier = BrakeForceSlider.GetValue();
        }

        void Mod_SettingsLoaded()
        {
            ApplySettings();
        }





        private void Mod_OnNewGame()
        {
            // Called once, when creating a new game. This is useful for deleting old mod saves
        }
        private void Mod_OnLoad()
        {
            // Called once, when mod is loading after game is fully loaded

            AssetBundle ab = LoadAssets.LoadBundle(this, "kekmet602t-mwc");

            Kekmet = GameObject.Find("KEKMET(350-400psi)");
            OldBody = GameObject.Find("KEKMET(350-400psi)/valmet_body");
            OldBody.SetActive(false);

            BrakePivotExisting = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/Dashboard/Brake/Pivot").gameObject;
            OldBrakePedal = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/Dashboard/Brake/Pivot/tractor_pedal_brake").gameObject;

            TurboSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("TurboSoundObj.prefab"));
            TurboSoundObj.transform.SetParent(Kekmet.transform, false);
            TurboSoundObj.SetActive(false);

            TractorTurboSound = TurboSoundObj.GetComponent<AudioSource>();

            OldBrakePedal.SetActive(false);

            KekmetController = Kekmet.GetComponentInChildren<AxisCarController>();

            TractorDriveTrain = Kekmet.GetComponent<Drivetrain>();

            FrontAxle = Kekmet.GetComponent<Axles>();

            HiTrac = HiTracDefault.GetValue();

            KekmetRange = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/Dashboard/Range").gameObject.GetPlayMaker("Use").FsmVariables.GetFsmBool("Range");

            TractorDriveTrain.SetTransmission(Drivetrain.Transmissions.AWD);

            FL_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFL");
            FR_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFR");

            FL_Wheel = FL_Wheel_OBJ.GetComponent<Wheel>();
            FR_Wheel = FR_Wheel_OBJ.GetComponent<Wheel>();

            RL_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/wheelRL");
            RR_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/wheelRR");

            RL_Wheel = RL_Wheel_OBJ.GetComponent<Wheel>();
            RR_Wheel = RR_Wheel_OBJ.GetComponent<Wheel>();

            RangeLever = GameObject.Instantiate(ab.LoadAsset<GameObject>("HiTracLever.prefab"));

            AirFilter = GameObject.Instantiate(ab.LoadAsset<GameObject>("AirFilter.prefab"));
            AirFilter.transform.SetParent(Kekmet.transform, false);
            AirFilter.transform.localPosition = new Vector3(-0.07f, 1.19f, 0.5f);
            AirFilter.transform.localEulerAngles = new Vector3(3.5f, 0, 0);

            NewBody = GameObject.Instantiate(ab.LoadAsset<GameObject>("602_body.prefab"));
            NewBody.transform.SetParent(Kekmet.transform, false);
            NewBody.transform.localPosition = new Vector3(0f, 0.3656657f, 0f);
            NewBody.transform.localEulerAngles = new Vector3(0, -90f, 0);

            BrakeNewL = GameObject.Instantiate(ab.LoadAsset<GameObject>("Tractor_Pedal_Brake_Left.prefab"));
            BrakeNewL.transform.SetParent(BrakePivotExisting.transform, false);
            BrakeNewL.transform.localPosition = new Vector3(-0.01f, 0.01f, 0.05f);
            BrakeNewL.transform.localEulerAngles = new Vector3(0, 0f, 0);

            BrakeNewR = GameObject.Instantiate(ab.LoadAsset<GameObject>("Tractor_Pedal_Brake_Right.prefab"));
            BrakeNewR.transform.SetParent(BrakePivotExisting.transform, false);
            BrakeNewR.transform.localPosition = new Vector3(0f, 0.01f, 0.05f);
            BrakeNewR.transform.localEulerAngles = new Vector3(0, 0f, 0);



            Tire = GameObject.Instantiate(ab.LoadAsset<GameObject>("FrontTireNew.prefab"));
            Mesh FrontWheel = Tire.GetComponentInChildren<MeshFilter>().mesh;


            Rim = GameObject.Instantiate(ab.LoadAsset<GameObject>("FrontRimNew.prefab"));
            Mesh FrontRim = Rim.GetComponentInChildren<MeshFilter>().mesh;

            Tire.SetActive(false);
            Rim.SetActive(false);

            FRRim = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFR/tire/valmet_wheel_front");
            FLRim = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFL/tire/valmet_wheel_front");

            FRWheel = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFR/tire/valmet_tire_front");
            FLWheel = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFL/tire/valmet_tire_front");


            FRRim.GetComponent<MeshFilter>().mesh = FrontRim;
            FLRim.GetComponent<MeshFilter>().mesh = FrontRim;
            FRWheel.GetComponent<MeshFilter>().mesh = FrontWheel;
            FLWheel.GetComponent<MeshFilter>().mesh = FrontWheel;


            FRWheel.transform.localScale = new Vector3(0.7f, 1f, 0.7f);
            FLWheel.transform.localScale = new Vector3(0.7f, 1f, 0.7f);
            FRRim.transform.localScale = new Vector3(0.7f, 1f, 0.7f);
            FLRim.transform.localScale = new Vector3(0.7f, 1f, 0.7f);

            FRWheel.transform.localEulerAngles = new Vector3(0, 180f, 90f);

            FL_Wheel.radius = 0.55f;
            FR_Wheel.radius = 0.55f;

            FL_Wheel.mass = 64f;
            FR_Wheel.mass = 64f;

            TractorDriveTrain.maxTorqueRPM = 1450;
            TractorDriveTrain.maxPowerRPM = 2300;
            TractorDriveTrain.maxPower = 82.16f;
            TractorDriveTrain.maxTorque = 289.58f;

            RangeShiftSource = GameObject.Instantiate(ab.LoadAsset<GameObject>("RangeShiftSound.prefab"));
            RangeShiftSource.transform.SetParent(Kekmet.transform, false);
            RangeShiftSound = RangeShiftSource.GetComponent<AudioSource>();

            ButtonSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("ButtonSwitchSound.prefab"));
            ButtonSoundObj.transform.SetParent(Kekmet.transform, false);
            ButtonSound = ButtonSoundObj.GetComponent<AudioSource>();



            RangeLever.transform.SetParent(Kekmet.transform, false);

            OldLights = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/accessories").gameObject;
            OldLights.SetActive(false);


            NewLights = GameObject.Instantiate(ab.LoadAsset<GameObject>("Kekmet602Lights.prefab"));
            NewLights.transform.SetParent(Kekmet.transform, false);
            NewLights.transform.localPosition = new Vector3(0f, 0.3660001f, -0.002999991f);
            NewLights.transform.localEulerAngles = new Vector3(0f, 270f, 0f);

            RearLights = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/WorkLights").gameObject;
            RearLights.transform.localPosition = new Vector3(0f, -0.3699999f, 3.34f);

            SideLightTrigger = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLightTrigger");
            FrontLightTrigger = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLightTrigger");

            SideLights = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights");
            FrontLights = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLights");

            SideLightSwitch = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/Switches/SideLightSwitch");
            FrontLightSwitch = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/Switches/FrontLightSwitch");

            SideLights.SetActive(false);
            FrontLights.SetActive(false);

            LeftLightExisting = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/WorkLights/WorkLightLeft").gameObject;
            Lightflare = LeftLightExisting.GetComponent<Light>().flare;

            Light1Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLights/RightLight");
            Light2Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLights/LeftLight");
            Light3Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/LeftRearLight");
            Light4Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/LeftFrontLight");
            Light5Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/RightFrontLight");
            Light6Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/RightRearLight");

            Light1 = Light1Obj.GetComponent<Light>();
            Light2 = Light2Obj.GetComponent<Light>();
            Light3 = Light3Obj.GetComponent<Light>();
            Light4 = Light4Obj.GetComponent<Light>();
            Light5 = Light5Obj.GetComponent<Light>();
            Light6 = Light6Obj.GetComponent<Light>();


            Light1.flare = Lightflare;
            Light2.flare = Lightflare;
            Light3.flare = Lightflare;
            Light4.flare = Lightflare;
            Light5.flare = Lightflare;
            Light6.flare = Lightflare;

            ab.Unload(false);
        }
        private void Mod_PostLoad()
        {

        }
        private void Mod_OnSave()
        {
            // Called once, when save and quit
            // Serialize your save file here.
        }
        private void Mod_Update()
        {
            if (TractorDriveTrain.rpm > 750)
            {

                TurboSoundObj.SetActive(true);
                TractorTurboSound.pitch = ((TractorDriveTrain.rpm * 0.000572f) + 0.108f);
                TractorTurboSound.volume = ((TractorDriveTrain.rpm * 0.00009627f) + SoundBoost);
            }
            else
            {
                TurboSoundObj.SetActive(false);
            }

            if (TractorDriveTrain.rpm > 1900)
            {
                SoundBoost = ((TractorDriveTrain.rpm - 1800) * -0.00056f) + 0.038f;
            }
            else
            {
                SoundBoost = 0.038f;
            }

            //TractorDriveTrain.SetTransmission(Drivetrain.Transmissions.AWD);
            //FrontAxle.frontAxle.powered = true;
            //FL_Wheel.isPowered = true;
            //FR_Wheel.isPowered = true;


            if (UnifiedRaycast.GetHit(SideLightTrigger.GetComponent<Collider>()))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "SIDE LIGHTS";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    SideLightsToggled = !SideLightsToggled;
                    ButtonSound.Play();
                }
            }

            if (UnifiedRaycast.GetHit(FrontLightTrigger.GetComponent<Collider>()))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "FRONT LIGHTS";
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    FrontLightsToggled = !FrontLightsToggled;
                    ButtonSound.Play();
                }
            }


            if (SideLightsToggled)
            {
                SideLights.SetActive(true);
                SideLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.513999f, -0.285f);
                SideLightSwitch.transform.localEulerAngles = new Vector3(279.9999f, 267.7023f, 180f);
            }
            else
            {
                SideLights.SetActive(false);
                SideLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.522f, -0.285f);
                SideLightSwitch.transform.localEulerAngles = new Vector3(290f, 87.70235f, -4.493277E-05f);
            }


            if (FrontLightsToggled)
            {
                FrontLights.SetActive(true);
                FrontLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.513999f, -0.1929f);
                FrontLightSwitch.transform.localEulerAngles = new Vector3(0f, 0f, 105f);
            }
            else
            {
                FrontLights.SetActive(false);
                FrontLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.522f, -0.1929f);
                FrontLightSwitch.transform.localEulerAngles = new Vector3(0f, 0f, 70.00001f);
            }



            if (BrakeLeft.GetKeybind())
                {
                    if (cInput.GetButtonDown("Brake") == false)
                    {
                        RL_Wheel.brake = 1f * BrakeForceModifier;
                        BrakeNewL.transform.localPosition = new Vector3(0.01f, 0.01f, 0.05f);
                        BrakeNewL.transform.localEulerAngles = new Vector3(0, 25f, 0);
                    }

                }
                else
                {
                    BrakeNewL.transform.localPosition = new Vector3(-0.01f, 0.01f, 0.05f);
                    BrakeNewL.transform.localEulerAngles = new Vector3(0, 0f, 0);
                }

                if (BrakeRight.GetKeybind())
                {
                    if (cInput.GetButtonDown("Brake") == false)
                    {
                        RR_Wheel.brake = 1f * BrakeForceModifier;
                        BrakeNewR.transform.localPosition = new Vector3(0.02f, 0.01f, 0.05f);
                        BrakeNewR.transform.localEulerAngles = new Vector3(0, 25f, 0);
                    }
                }
                else
                {
                    BrakeNewR.transform.localPosition = new Vector3(0f, 0.01f, 0.05f);
                    BrakeNewR.transform.localEulerAngles = new Vector3(0, 0f, 0);
                }



                if (HiTracSelector.GetKeybindDown())
                {
                    HiTrac = !HiTrac;
                    RangeShiftSound.Play();
                }

                 if (HiTrac)
                 {
                     RangeLever.transform.localPosition = new Vector3(0.4399f, 0.88f, -0.73f);
                     RangeLever.transform.localEulerAngles = new Vector3(270, 280, 0);
                 }
                 if (!HiTrac)
                 {
                     RangeLever.transform.localPosition = new Vector3(0.4399f, 0.88f, -0.8f);
                     RangeLever.transform.localEulerAngles = new Vector3(270, 280, 0);
                 }

                if (HiTrac && !KekmetRange.Value)
                {
                    TractorDriveTrain.finalDriveRatio = 15.48f;
                }

                if (HiTrac == false && KekmetRange.Value)
                {
                    TractorDriveTrain.finalDriveRatio = 4.68f;
                }

                if (HiTrac == false && !KekmetRange.Value)
                {
                    TractorDriveTrain.finalDriveRatio = 12.39f;
                }

                if (HiTrac && KekmetRange.Value)
                {
                    TractorDriveTrain.finalDriveRatio = 5.85f;
                }           
        }
    }
}
