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

        public bool HiTrac; // Find a way so says in the hud "Gear + Range + (Hi-Trac/Road)" depending on mode To help the player understand its enabled better. 

        public Drivetrain TractorDriveTrain;

        public AxisCarController KekmetController;

        public Axles FrontAxle;

        public Wheel FL_Wheel, FR_Wheel, RL_Wheel, RR_Wheel;

        public SettingsKeybind HiTracSelector, BrakeLeft, BrakeRight;

        public SettingsCheckBox HiTracDefault;

        public GameObject Kekmet;
        public GameObject OldBody;

        public GameObject FL_Wheel_OBJ, FR_Wheel_OBJ, RL_Wheel_OBJ, RR_Wheel_OBJ;

        public GameObject AirFilter;
        public GameObject NewBody;

        public GameObject BrakePivotExisting;
        public GameObject OldBrakePedal;

        public GameObject OldLights;
        public GameObject NewLights;
        public GameObject LightSwitch;

        public GameObject Light1Obj, Light2Obj, Light3Obj, Light4Obj, Light5Obj, Light6Obj;

        public GameObject BrakeNewL, BrakeNewR;

        public GameObject Rim, Tire;

        public GameObject FRRim, FLRim, FRWheel, FLWheel;

        public GameObject FRRimVis, FLRimVis, FRWheelVis, FLWheelVis;

        public GameObject TurboSoundObj, ButtonSoundObj;

        public GameObject RangeShiftSource;

        public GameObject RangeLever;


        public AudioSource RangeShiftSound, TractorTurboSound, ButtonSound;

        public float SoundBoost;

        public float BrakeForceModifier;

        public SettingsSlider BrakeForceSlider;

        public GameObject RearLights, FrontLights, SideLights;

        public GameObject SideLightTrigger, FrontLightTrigger;

        public GameObject SideLightSwitch, FrontLightSwitch;

        public bool Pause;
        public bool FrontLightsToggled;
        public bool SideLightsToggled;

        public GameObject QuadForLights;

        public Light Light1, Light2, Light3, Light4, Light5, Light6;

        public Flare Lightflare;

        public GameObject LeftLightExisting;


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
            HiTracSelector = Keybind.Add("Kekmet602HiTracToggleKey", "Hi-Trac toggle keybind", KeyCode.Keypad5); // Set keybind for hi-trac toggle. 

            BrakeLeft = Keybind.Add("BrakeLeftKey", "Press to brake just the left wheel", KeyCode.A, KeyCode.LeftAlt); // set keybind for left rear wheel brake. 
            BrakeRight = Keybind.Add("BrakeRightKey", "Press to brake just the right wheel", KeyCode.D, KeyCode.LeftAlt); // Set keybind for right rear wheel brake. 

            HiTracDefault = Settings.AddCheckBox("HiTracDefaultSetting", "Is HiTrac enabled by default?", false); // Does the player want hi-trac enabled by default?

            BrakeForceSlider = Settings.AddSlider("BrakeForceSlider", "Sets the brake force of the split brakes. 1 = 100%.", 0f, 1f, 1f, ApplySettings); // Player can set desired brake force.

        }

        private void ApplySettings()
        {
            BrakeForceModifier = BrakeForceSlider.GetValue(); // Needed to update it without reload? Idk i was given this, im not sure. 
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

            AssetBundle ab = LoadAssets.LoadBundle(this, "kekmet602t-mwc"); // Loads asset bundle, i've used AB since it was first added fun fact

            Kekmet = GameObject.Find("KEKMET(350-400psi)"); // tells it what the kekmet itself is

            OldBody = GameObject.Find("KEKMET(350-400psi)/valmet_body"); //tells it what the old body is for the kekmet
            OldBody.SetActive(false); // disables old body, needs replacing for texture. 

            NewBody = GameObject.Instantiate(ab.LoadAsset<GameObject>("602_body.prefab")); // instantiates new body, needed for new texture and future additions
            NewBody.transform.SetParent(Kekmet.transform, false); // parents body to tractor
            NewBody.transform.localPosition = new Vector3(0f, 0.3656657f, 0f);
            NewBody.transform.localEulerAngles = new Vector3(0, -90f, 0); //Sets position of new body to account for importing weirdnes


            BrakePivotExisting = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/Dashboard/Brake/Pivot").gameObject; // Finds brake pedal pivot made by topless. 
            OldBrakePedal = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/Dashboard/Brake/Pivot/tractor_pedal_brake").gameObject; // Finds old brake pedal

            OldBrakePedal.SetActive(false); // Disables old brake pedal to add new model.

            BrakeNewL = GameObject.Instantiate(ab.LoadAsset<GameObject>("Tractor_Pedal_Brake_Left.prefab")); //instantiate new left brake pedal
            BrakeNewL.transform.SetParent(BrakePivotExisting.transform, false); // Parents to existing pivot
            BrakeNewL.transform.localPosition = new Vector3(-0.01f, 0.01f, 0.05f);
            BrakeNewL.transform.localEulerAngles = new Vector3(0, 0f, 0); // sets left brake pedal position

            BrakeNewR = GameObject.Instantiate(ab.LoadAsset<GameObject>("Tractor_Pedal_Brake_Right.prefab")); //instantiate new right brake pedal
            BrakeNewR.transform.SetParent(BrakePivotExisting.transform, false); // Parents to existing pivot
            BrakeNewR.transform.localPosition = new Vector3(0f, 0.01f, 0.05f);
            BrakeNewR.transform.localEulerAngles = new Vector3(0, 0f, 0); // sets right brake pedal position


            TurboSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("TurboSoundObj.prefab")); // Instiate the obj for the turbo sound
            TurboSoundObj.transform.SetParent(Kekmet.transform, false); // parents to tractor
            TurboSoundObj.SetActive(false); //Active by default as its a constant playing loop

            TractorTurboSound = TurboSoundObj.GetComponent<AudioSource>(); // gets the audio source component from the sound object.


            KekmetController = Kekmet.GetComponentInChildren<AxisCarController>(); // Stores axis car controller

            TractorDriveTrain = Kekmet.GetComponent<Drivetrain>(); // Stores the drivetrain component for power and etc edits

            FrontAxle = Kekmet.GetComponent<Axles>(); // Stores front axle, needed to engage AWD if bugs can be worked out. 

            HiTrac = HiTracDefault.GetValue(); // Sets hi-trac state to enabled on load if the player set it in settings

            KekmetRange = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/Dashboard/Range").gameObject.GetPlayMaker("Use").FsmVariables.GetFsmBool("Range"); // Figures out what range tractor is in


            FL_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFL"); //gets the game object of the front wheels
            FR_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFR");

            FL_Wheel = FL_Wheel_OBJ.GetComponent<Wheel>(); // Gets the actual physical wheel component of the front wheels wheel
            FR_Wheel = FR_Wheel_OBJ.GetComponent<Wheel>();

            RL_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/wheelRL");
            RR_Wheel_OBJ = GameObject.Find("KEKMET(350-400psi)/wheelRR");

            RL_Wheel = RL_Wheel_OBJ.GetComponent<Wheel>();// Gets the actual physical wheel component of the Rear wheels
            RR_Wheel = RR_Wheel_OBJ.GetComponent<Wheel>(); 

            RangeLever = GameObject.Instantiate(ab.LoadAsset<GameObject>("HiTracLever.prefab")); // Instantiate the hi-trac lever

            AirFilter = GameObject.Instantiate(ab.LoadAsset<GameObject>("AirFilter.prefab")); // Istantiate air filter
            AirFilter.transform.SetParent(Kekmet.transform, false); // parents to tractor
            AirFilter.transform.localPosition = new Vector3(-0.07f, 1.19f, 0.5f);
            AirFilter.transform.localEulerAngles = new Vector3(3.5f, 0, 0); // sets position

            
            



            Tire = GameObject.Instantiate(ab.LoadAsset<GameObject>("FrontTireNew.prefab")); // instaniate new Wheel
            Mesh FrontWheel = Tire.GetComponentInChildren<MeshFilter>().mesh; // Sets mesh name to mesh of this obj, why did i make it declared here instead of up top?


            Rim = GameObject.Instantiate(ab.LoadAsset<GameObject>("FrontRimNew.prefab")); // instaniate new rim
            Mesh FrontRim = Rim.GetComponentInChildren<MeshFilter>().mesh; // Sets mesh name to mesh of this obj, Probally leftover from old kekmet 602 years ago

            Tire.SetActive(false); //Disables object, we only need its mesh
            Rim.SetActive(false); //Disables object, we only need its mesh

            FRRim = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFR/tire/valmet_wheel_front"); // gets the front rim for the right
            FLRim = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFL/tire/valmet_wheel_front"); // gets the front rim for the left

            FRWheel = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFR/tire/valmet_tire_front"); // gets the front tire for the right
            FLWheel = GameObject.Find("KEKMET(350-400psi)/FrontAxle/wheelFL/tire/valmet_tire_front"); // Gets the front tire for the left


            FRRim.GetComponent<MeshFilter>().mesh = FrontRim; // sets mesh of obj to new mesh
            FLRim.GetComponent<MeshFilter>().mesh = FrontRim; // sets mesh of obj to new mesh
            FRWheel.GetComponent<MeshFilter>().mesh = FrontWheel; // sets mesh of obj to new mesh
            FLWheel.GetComponent<MeshFilter>().mesh = FrontWheel; // sets mesh of obj to new mesh


            FRWheel.transform.localScale = new Vector3(0.7f, 1f, 0.7f); // scales visual tire
            FLWheel.transform.localScale = new Vector3(0.7f, 1f, 0.7f); //scales visual tire
            FRRim.transform.localScale = new Vector3(0.7f, 1f, 0.7f); // scales visual rim
            FLRim.transform.localScale = new Vector3(0.7f, 1f, 0.7f); // Scales visual rim

            FRWheel.transform.localEulerAngles = new Vector3(0, 180f, 90f); //Rotates tire so its facing forward, kinda funny having it backwards so one tread is going opposite. might re-add. 

            FL_Wheel.radius = 0.55f; // Inccreases radius of the actual tire that effects handling
            FR_Wheel.radius = 0.55f;// Inccreases radius of the actual tire that effects handling

            FL_Wheel.mass = 64f; // Increases front wheel weight due to bigger size, total guess
            FR_Wheel.mass = 64f; // Increases front wheel weight due to bigger size, total guess

            TractorDriveTrain.maxTorqueRPM = 1450; // Max Torque rpm
            TractorDriveTrain.maxPowerRPM = 2300; // max HP rpm
            TractorDriveTrain.maxPower = 82.16f; // Max hp
            TractorDriveTrain.maxTorque = 289.58f; //Max torque in 

            RangeShiftSource = GameObject.Instantiate(ab.LoadAsset<GameObject>("RangeShiftSound.prefab")); // Instantite the obj containing the range shift sound
            RangeShiftSource.transform.SetParent(Kekmet.transform, false); // parrent sound obj to tractor
            RangeShiftSound = RangeShiftSource.GetComponent<AudioSource>(); //gets audio source part from obj

            ButtonSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("ButtonSwitchSound.prefab")); //instantiate the obj containing the button sound
            ButtonSoundObj.transform.SetParent(Kekmet.transform, false); // parents the button sound object to the tractor
            ButtonSound = ButtonSoundObj.GetComponent<AudioSource>(); //Gets the audiosource part from the obj


            RangeLever.transform.SetParent(Kekmet.transform, false);


            OldLights = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/accessories").gameObject; // Finds the old lights and switches for the tractor
            OldLights.SetActive(false); // Disables it because it will be replaced


            NewLights = GameObject.Instantiate(ab.LoadAsset<GameObject>("Kekmet602Lights.prefab")); // Instantiates the new lights and switch for the tractor
            NewLights.transform.SetParent(Kekmet.transform, false); // Parent to tractor
            NewLights.transform.localPosition = new Vector3(0f, 0.3660001f, -0.002999991f); // Sets position of new lights
            NewLights.transform.localEulerAngles = new Vector3(0f, 270f, 0f); // Rotation adjustment of new light. 

            RearLights = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/WorkLights").gameObject; // Finds existing rear lights
            RearLights.transform.localPosition = new Vector3(0f, -0.3699999f, 3.34f); // Moves rear lights up to account for new model

            SideLightTrigger = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLightTrigger"); // Find the trigger for the side lights
            FrontLightTrigger = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLightTrigger"); // Find the trigger for the front lights

            SideLights = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights"); // Side lights
            FrontLights = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLights"); // Front lights

            SideLightSwitch = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/Switches/SideLightSwitch"); // Side light visual switch object
            FrontLightSwitch = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/Switches/FrontLightSwitch"); // Front light visual switch object

            SideLights.SetActive(false); //Disables sidelights by default/off
            FrontLights.SetActive(false); //Disables frontlights by default/off

            LeftLightExisting = GameObject.Find("KEKMET(350-400psi)").transform.Find("LOD/WorkLights/WorkLightLeft").gameObject; //gets default left rear work light
            Lightflare = LeftLightExisting.GetComponent<Light>().flare; // grabs the flare from left rear work light and stores it.

            Light1Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLights/RightLight");
            Light2Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/FrontLights/LeftLight");
            Light3Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/LeftRearLight");
            Light4Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/LeftFrontLight");
            Light5Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/RightFrontLight");
            Light6Obj = GameObject.Find("KEKMET(350-400psi)/Kekmet602Lights(Clone)/SideLights/RightRearLight"); // Gets all the objects that that have the light for below to work

            Light1 = Light1Obj.GetComponent<Light>();
            Light2 = Light2Obj.GetComponent<Light>();
            Light3 = Light3Obj.GetComponent<Light>(); //Gets all the light components 
            Light4 = Light4Obj.GetComponent<Light>();
            Light5 = Light5Obj.GetComponent<Light>();
            Light6 = Light6Obj.GetComponent<Light>();


            Light1.flare = Lightflare;
            Light2.flare = Lightflare;
            Light3.flare = Lightflare;
            Light4.flare = Lightflare;
            Light5.flare = Lightflare;
            Light6.flare = Lightflare; // Sets all the flares, this is horrible

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
            if (TractorDriveTrain.rpm > 800)
            {

                TurboSoundObj.SetActive(true);
                TractorTurboSound.pitch = ((TractorDriveTrain.rpm * 0.000572f) + 0.108f);
                TractorTurboSound.volume = ((TractorDriveTrain.rpm * 0.00009627f) + SoundBoost); // Sets pitch and volume based on RPM and a preset modifer
            }
            else
            {
                TurboSoundObj.SetActive(false); // Disables sound below a certain RPM.
            }

            if (TractorDriveTrain.rpm > 1900)
            {
                SoundBoost = ((TractorDriveTrain.rpm - 1800) * -0.00056f) + 0.038f; // Lowers Sound volume exponentially past 1900 rpm to save ears when you're cruising at high RPM.
            }
            else
            {
                SoundBoost = 0.038f; // Default volume boost below EarSaver(TM) RPM
            }

            //TractorDriveTrain.SetTransmission(Drivetrain.Transmissions.AWD);
            //FrontAxle.frontAxle.powered = true;
            //FL_Wheel.isPowered = true;
            //FR_Wheel.isPowered = true;
            // Tractor pulses rpm as if front axle is back feeding, only gets a fraction of rear wheel speed. Center diff support? Can't find it. 


            if (UnifiedRaycast.GetHit(SideLightTrigger.GetComponent<Collider>())) //Gets the collider named SideLightTrigger
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true; // Enables hand interact icon
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "SIDE LIGHTS"; // Tells the player they're looking at the switch for the side lights
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Left click
                {
                    SideLightsToggled = !SideLightsToggled; //Toggles side light state
                    ButtonSound.Play();
                }
            }

            if (UnifiedRaycast.GetHit(FrontLightTrigger.GetComponent<Collider>()))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true; // Enables hand interact icon
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "FRONT LIGHTS"; // Tells the player they're looking at the switch for the front lights
                if (Input.GetKeyDown(KeyCode.Mouse0)) //Left click
                {
                    FrontLightsToggled = !FrontLightsToggled; // Toggles front light state
                    ButtonSound.Play();
                }
            }


            if (SideLightsToggled) 
            {
                SideLights.SetActive(true);
                SideLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.513999f, -0.285f);
                SideLightSwitch.transform.localEulerAngles = new Vector3(279.9999f, 267.7023f, 180f); // Sets switch position to On and enables the light obj, Could be done by togling active obj vs position adjustment
            }
            else
            {
                SideLights.SetActive(false);
                SideLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.522f, -0.285f);
                SideLightSwitch.transform.localEulerAngles = new Vector3(290f, 87.70235f, -4.493277E-05f); // Sets switch position to Off and disables the light obj, Same potential different way.
            }


            if (FrontLightsToggled)
            {
                FrontLights.SetActive(true);
                FrontLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.513999f, -0.1929f);
                FrontLightSwitch.transform.localEulerAngles = new Vector3(0f, 0f, 105f); // Sets switch position and enables front lights
            }
            else
            {
                FrontLights.SetActive(false);
                FrontLightSwitch.transform.localPosition = new Vector3(-0.1666f, 1.522f, -0.1929f);
                FrontLightSwitch.transform.localEulerAngles = new Vector3(0f, 0f, 70.00001f); // sets switch position and disables front lights
            }



            if (BrakeLeft.GetKeybind()) // When the player hits their split brake left button
                {
                    if (cInput.GetButtonDown("Brake") == false) // If the brakes are not active, i dont think I need this? I was worried about it doubling brake force.
                    {
                        RL_Wheel.brake = 1f * BrakeForceModifier;
                        BrakeNewL.transform.localPosition = new Vector3(0.01f, 0.01f, 0.05f);
                        BrakeNewL.transform.localEulerAngles = new Vector3(0, 25f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
                    }

                }
                else
                {
                    BrakeNewL.transform.localPosition = new Vector3(-0.01f, 0.01f, 0.05f);
                    BrakeNewL.transform.localEulerAngles = new Vector3(0, 0f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
            }

                if (BrakeRight.GetKeybind()) // When the player hits their split brake right button
            {
                    if (cInput.GetButtonDown("Brake") == false)
                    {
                        RR_Wheel.brake = 1f * BrakeForceModifier;
                        BrakeNewR.transform.localPosition = new Vector3(0.02f, 0.01f, 0.05f);
                        BrakeNewR.transform.localEulerAngles = new Vector3(0, 25f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
                }
                }
                else
                {
                    BrakeNewR.transform.localPosition = new Vector3(0f, 0.01f, 0.05f);
                    BrakeNewR.transform.localEulerAngles = new Vector3(0, 0f, 0); // Sets brake pedal position directly to avoid touching the pivot set by topless that controls both
            }



                if (HiTracSelector.GetKeybindDown()) // When the player hits their hits the hi-trac toggle bind
                {
                    HiTrac = !HiTrac; // toggle hi-trac state
                    RangeShiftSound.Play(); // Shifter go clunk sound hue hue
                }

                 if (HiTrac)
                 {
                     RangeLever.transform.localPosition = new Vector3(0.4399f, 0.88f, -0.73f);
                     RangeLever.transform.localEulerAngles = new Vector3(270, 280, 0); // Sets lever postion for On
                 }
                 if (!HiTrac)
                 {
                     RangeLever.transform.localPosition = new Vector3(0.4399f, 0.88f, -0.8f);
                     RangeLever.transform.localEulerAngles = new Vector3(270, 280, 0); // Sets lever position for Off
                 }

                 // Hi-Trac = about a 20% gear reduction. 
                if (HiTrac && !KekmetRange.Value) // If hi-trac enabled and tractor is in Low Range. 
                {
                    TractorDriveTrain.finalDriveRatio = 15.48f;
                }

                if (HiTrac == false && KekmetRange.Value) // If Hi-trac disabled and Tractor is in high range. 
                {
                    TractorDriveTrain.finalDriveRatio = 4.68f;
                }

                if (HiTrac == false && !KekmetRange.Value) // If Hi-trac disabled and Tractor is in low range. 
                {
                    TractorDriveTrain.finalDriveRatio = 12.39f;
                }

                if (HiTrac && KekmetRange.Value) // If Hi-trac Enabled and Tractor is in high range.
            {
                    TractorDriveTrain.finalDriveRatio = 5.85f;
                }           
        }
    }
}
