using HutongGames.PlayMaker;
using MSCLoader;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static CarDynamics;
using static System.Collections.Specialized.BitVector32;

namespace Kekmet602T_MWC
{
    public class Kekmet602T_MWC : Mod
    {
        public override string ID => "Kekmet602T_MWC"; // Your (unique) mod ID 
        public override string Name => "Kekmet602-T (MWC Edition)"; // Your mod name
        public override string Author => "TaliaKuznetsova"; // Name of the Author (your name)
        public override string Version => "3.4"; // Version
        public override string Description => "Changes the Kekmet into it's bigger 602 Turbocharged version"; // Short description of your mod

        public override Game SupportedGames => Game.MyWinterCar;


        public bool HiTrac, WindowOpen; // Find a way so says in the hud "Gear + Range + (Hi-Trac/Road)" depending on mode To help the player understand its enabled better. 

        public Drivetrain TractorDriveTrain;

        public AxisCarController KekmetController;

        public Axles FrontAxle;

        public Wheel FL_Wheel, FR_Wheel, RL_Wheel, RR_Wheel;

        public SettingsKeybind HiTracSelector, BrakeLeft, BrakeRight;

        public SettingsCheckBox HiTracDefault, TurboSounds;

        public Transform Kekmet, KekmetLOD;
        public GameObject OldBody,OldTractorSoundsObj;

        public GameObject FL_Wheel_OBJ, FR_Wheel_OBJ, RL_Wheel_OBJ, RR_Wheel_OBJ, FrontAxleObj;

        public GameObject AirFilter;
        public GameObject NewBody;

        public GameObject BrakePivotExisting;
        public GameObject OldBrakePedal;

        public GameObject OldLights;
        public GameObject NewLights;
        public GameObject LightSwitch;


        public GameObject BrakeNewL, BrakeNewR;

        public GameObject Rim, Tire;

        public GameObject FRRim, FLRim, FRWheel, FLWheel;

        public GameObject FRRimVis, FLRimVis, FRWheelVis, FLWheelVis;

        public GameObject ButtonSoundObj,NewTractorSounds;

        public GameObject RangeShiftSource;

        public GameObject RangeLever;

        public GameObject RR_Tire, RL_Tire;
        public GameObject RR_Rim, RL_Rim;


        public AudioSource RangeShiftSound, ButtonSound;

        public float BrakeForceModifier;

        public SettingsSlider BrakeForceSlider;

        public SettingsSliderInt AlternateSkinSetting;

        public GameObject RearLights, FrontLights, SideLights;

        public GameObject FrontLightTrigger;
        public Collider FrontLightTrigCol;

        public GameObject FrontLightSwitch;

        public bool Pause;
        public bool FrontLightsToggled;


        public GameObject QuadForLights;

        public Light Light1, Light2, Light3, Light4, Light5, Light6;

        public Flare Lightflare;

        public GameObject LeftLightExisting;

        public Rigidbody TractorWeight;

        public GameObject FrontAxleNewObj,FrontAxleOld;

        public FsmString KekmetSeatCheck;

        public GameObject gear;
        public FsmString guiGear;

        public bool TurboNoiseToggled;

        public GameObject GaugeFaceplates;

        public Transform GuiIndicators;

        public TextMesh GearText, GearTextShadow;

        public GameObject RearGlassNewObj, RearWindowCollider,FourWheelDriveLever;

        public AudioSource OldTractorSounds, TractorTurboSound;

        public Animation MainBodyAnimation;

        public Material LightsOn, LightsOff, NewGaugeTex, NewBodySkin, NewBodySkinTwo, NewBodySkinThree, NewBodySkinFour, NewDoorSkinThreeFour;

        public Renderer DashboardGaugeMat;

        public Mesh NewBodySkinThreeFour, DoorLeftSkinThreeFour, DoorRightSkinThreeFour;



        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.OnSave, Mod_OnSave);
            SetupFunction(Setup.ModSettings, Mod_Settings);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.Update, Mod_PostLoad);
        }

        public Kekmet602T_MWC MainKekmetMod;

        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;

        }



        private void Mod_Settings()
        {
            HiTracSelector = Keybind.Add("Kekmet602HiTracToggleKey", "Hi-Trac toggle keybind", KeyCode.Keypad5); // Set keybind for hi-trac toggle. 

            BrakeLeft = Keybind.Add("BrakeLeftKey", "Press to brake just the left wheel", KeyCode.A, KeyCode.LeftAlt); // set keybind for left rear wheel brake. 
            BrakeRight = Keybind.Add("BrakeRightKey", "Press to brake just the right wheel", KeyCode.D, KeyCode.LeftAlt); // Set keybind for right rear wheel brake. 

            //HiTracDefault = Settings.AddCheckBox("HiTracDefaultSetting", "Is HiTrac enabled by default?", false); // Does the player want hi-trac enabled by default?
            TurboSounds = Settings.AddCheckBox("TurboSounds", "Enable turbo sounds?", true, TurboSoundToggle); // Does the player want turbo sounds?
            AlternateSkinSetting = Settings.AddSlider("AlternateSkinSetting", "Selects alternate skins", 1, 4, 1, ApplySettings); // Does the player want turbo sounds?

            BrakeForceSlider = Settings.AddSlider("BrakeForceSlider", "Sets the brake force of the split brakes. 1 = 100%.", 0f, 1f, 1f, ApplySettings); // Player can set desired brake force.

        }

        private void ApplySettings()
        {
            BrakeForceModifier = BrakeForceSlider.GetValue(); // Needed to update it without reload? Idk i was given this, im not sure. 
            BrakeForceModifier = AlternateSkinSetting.GetValue(); // Needed to update it without reload? Idk i was given this, im not sure. 
            if (AlternateSkinSetting.GetValue() == 1f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkin;
            }
            else if (AlternateSkinSetting.GetValue() == 2f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkinTwo;
            }
            else if (AlternateSkinSetting.GetValue() == 3f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkinThree;
                NewBody.GetComponent<MeshFilter>().mesh = NewBodySkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshFilter>().mesh = DoorRightSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshFilter>().mesh = DoorLeftSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;
            }
            else if (AlternateSkinSetting.GetValue() == 4f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkinFour;
                NewBody.GetComponent<MeshFilter>().mesh = NewBodySkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshFilter>().mesh = DoorRightSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshFilter>().mesh = DoorLeftSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;
            }


        }

        private void TurboSoundToggle()
        {
            TurboNoiseToggled = TurboSounds.GetValue(); // Toggles Turbo noises. 
            TractorTurboSound.pitch = 0f; TractorTurboSound.volume = 0f;
        }

        void Mod_SettingsLoaded()
        {
            ApplySettings();
        }


        private void Mod_OnLoad()
        {
            // Called once, when mod is loading after game is fully loaded

            AssetBundle ab = LoadAssets.LoadBundle(this, "kekmet602t-mwc"); // Loads asset bundle, i've used AB since it was first added fun fact

            RearGlassNewObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("RearWindowGlass.prefab"));
            FourWheelDriveLever = GameObject.Instantiate(ab.LoadAsset<GameObject>("4wd Lever.prefab"));
            NewTractorSounds = GameObject.Instantiate(ab.LoadAsset<GameObject>("NewTractorSounds.prefab"));
            NewLights = GameObject.Instantiate(ab.LoadAsset<GameObject>("Kekmet602Lights.prefab"));
            NewBody = GameObject.Instantiate(ab.LoadAsset<GameObject>("602_body.prefab"));
            BrakeNewL = GameObject.Instantiate(ab.LoadAsset<GameObject>("Tractor_Pedal_Brake_Left.prefab"));
            FrontAxleNewObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("FrontAxle.prefab"));
            RangeLever = GameObject.Instantiate(ab.LoadAsset<GameObject>("HiTracLever.prefab"));
            AirFilter = GameObject.Instantiate(ab.LoadAsset<GameObject>("AirFilter.prefab"));
            Rim = GameObject.Instantiate(ab.LoadAsset<GameObject>("FrontRimNew.prefab"));
            Tire = GameObject.Instantiate(ab.LoadAsset<GameObject>("FrontTireNew.prefab"));
            ButtonSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("ButtonSwitchSound.prefab"));
            RangeShiftSource = GameObject.Instantiate(ab.LoadAsset<GameObject>("RangeShiftSound.prefab"));


            LightsOff = Material.Instantiate(ab.LoadAsset<Material>("LightsOffMat.mat"));
            LightsOn = Material.Instantiate(ab.LoadAsset<Material>("LightsOnMat.mat"));
            NewGaugeTex = Material.Instantiate(ab.LoadAsset<Material>("TractorGaugesNew.mat"));
            NewBodySkin = Material.Instantiate(ab.LoadAsset<Material>("602BodyMat.mat"));
            NewBodySkinTwo = Material.Instantiate(ab.LoadAsset<Material>("602BodyMatTwo.mat"));
            NewBodySkinThree = Material.Instantiate(ab.LoadAsset<Material>("602BodyMatThree.mat"));
            NewBodySkinFour = Material.Instantiate(ab.LoadAsset<Material>("602BodyMatFour.mat"));
            NewDoorSkinThreeFour = Material.Instantiate(ab.LoadAsset<Material>("TigerPrintDoorMat.mat"));

            NewBodySkinThreeFour = Material.Instantiate(ab.LoadAsset<Mesh>("602BodySkinThreeFour.obj"));
            DoorLeftSkinThreeFour = Material.Instantiate(ab.LoadAsset<Mesh>("602DoorSkinThreeFourLeft.obj"));
            DoorRightSkinThreeFour = Material.Instantiate(ab.LoadAsset<Mesh>("602DoorSkinThreeFourRight.obj"));




            Kekmet = GameObject.Find("KEKMET(350-400psi)").transform; // tells it what the kekmet itself is
            KekmetLOD = Kekmet.transform.Find("LOD").transform;

            FrontAxleObj = Kekmet.transform.Find("FrontAxle").gameObject;

            OldBody = Kekmet.Find("valmet_body").gameObject; //tells it what the old body is for the kekmet
            OldBody.SetActive(false); // disables old body, needs replacing for texture. 

             // instantiates new body, needed for new texture and future additions
            NewBody.transform.SetParent(KekmetLOD.transform, false); // parents body to tractor
            NewBody.transform.localPosition = new Vector3(0f, 0.3656657f, 0f);
            NewBody.transform.localEulerAngles = new Vector3(0, -90f, 0); //Sets position of new body to account for importing weirdnes
            
            
            TractorWeight = Kekmet.gameObject.GetComponent<Rigidbody>();
            TractorWeight.mass = 2980;

            BrakePivotExisting = KekmetLOD.Find("Dashboard/Brake/Pivot").gameObject; // Finds brake pedal pivot made by topless. 
            OldBrakePedal = BrakePivotExisting.transform.Find("tractor_pedal_brake").gameObject; //  // Finds old brake pedal

            OldBrakePedal.SetActive(false); // Disables old brake pedal to add new model.

             //instantiate new left brake pedal
            BrakeNewL.transform.SetParent(BrakePivotExisting.transform, false); // Parents to existing pivot
            BrakeNewL.transform.localPosition = new Vector3(-0.01f, 0.01f, 0.05f);
            BrakeNewL.transform.localEulerAngles = new Vector3(0, 0f, 0); // sets left brake pedal position

            BrakeNewR = GameObject.Instantiate(ab.LoadAsset<GameObject>("Tractor_Pedal_Brake_Right.prefab")); //instantiate new right brake pedal
            BrakeNewR.transform.SetParent(BrakePivotExisting.transform, false); // Parents to existing pivot
            BrakeNewR.transform.localPosition = new Vector3(0f, 0.01f, 0.05f);
            BrakeNewR.transform.localEulerAngles = new Vector3(0, 0f, 0); // sets right brake pedal position.

            MainBodyAnimation = NewBody.GetComponent<Animation>();

            Mesh FrontAxleNew = FrontAxleNewObj.GetComponentInChildren<MeshFilter>().mesh;


            FrontAxleOld = Kekmet.transform.Find("FrontAxle/valmet_front_axle").gameObject;
            FrontAxleOld.GetComponentInChildren<MeshFilter>().mesh = FrontAxleNew;

            FrontAxleNewObj.SetActive(false);

            KekmetController = Kekmet.GetComponentInChildren<AxisCarController>(); // Stores axis car controller

            TractorDriveTrain = Kekmet.gameObject.GetComponent<Drivetrain>(); // Stores the drivetrain component for power and etc edits

            FrontAxle = Kekmet.GetComponent<Axles>(); // Stores front axle, needed to engage AWD if bugs can be worked out. 

             // Istantiate air filter
            AirFilter.transform.SetParent(KekmetLOD.transform, false); // parents to tractor
            AirFilter.transform.localPosition = new Vector3(-0.07f, 1.19f, 0.5f);
            AirFilter.transform.localEulerAngles = new Vector3(3.5f, 0, 0); // sets position


            FL_Wheel_OBJ = FrontAxleObj.transform.Find("wheelFL").gameObject; //gets the game object of the front wheels
            FR_Wheel_OBJ = FrontAxleObj.transform.Find("wheelFR").gameObject;

            FL_Wheel = FL_Wheel_OBJ.GetComponent<Wheel>(); // Gets the actual physical wheel component of the front wheels wheel
            FR_Wheel = FR_Wheel_OBJ.GetComponent<Wheel>();

            RL_Wheel_OBJ = Kekmet.transform.Find("wheelRL").gameObject;
            RR_Wheel_OBJ = Kekmet.transform.Find("wheelRR").gameObject;

            RL_Wheel = RL_Wheel_OBJ.GetComponent<Wheel>();// Gets the actual physical wheel component of the Rear wheels
            RR_Wheel = RR_Wheel_OBJ.GetComponent<Wheel>();

            RL_Wheel.width = 0.414f; // Widens to fit wider front. Tires appear equal in pics i've seen.
            RR_Wheel.width = 0.414f;

            RR_Rim = RR_Wheel_OBJ.transform.Find("tire/valmet_wheel_rear").gameObject;
            RR_Tire = RR_Wheel_OBJ.transform.Find("tire/valmet_tire_rear").gameObject;

            RL_Rim = RL_Wheel_OBJ.transform.Find("tire/valmet_wheel_rear").gameObject;
            RL_Tire = RL_Wheel_OBJ.transform.Find("tire/valmet_tire_rear").gameObject;

            RR_Rim.transform.localScale = new Vector3(1f, 1.17f, 1f); // Scales tires to fit better
            RR_Tire.transform.localScale = new Vector3(1f, 1.2f, 1f);

            RL_Rim.transform.localScale = new Vector3(1f, 1.17f, 1f);
            RL_Tire.transform.localScale = new Vector3(1f, 1.2f, 1f);

             // instaniate new Wheel
            Mesh FrontWheel = Tire.GetComponentInChildren<MeshFilter>().mesh; // Sets mesh name to mesh of this obj, why did i make it declared here instead of up top?


             // instaniate new rim
            Mesh FrontRim = Rim.GetComponentInChildren<MeshFilter>().mesh; // Sets mesh name to mesh of this obj, Probally leftover from old kekmet 602 years ago

            Tire.SetActive(false); //Disables object, we only need its mesh
            Rim.SetActive(false); //Disables object, we only need its mesh

            

            FRRim = FR_Wheel.transform.Find("tire/valmet_wheel_front").gameObject; // gets the front rim for the right
            FLRim = FL_Wheel_OBJ.transform.Find("tire/valmet_wheel_front").gameObject; // gets the front rim for the left

            FRWheel = FR_Wheel.transform.Find("tire/valmet_tire_front").gameObject; // gets the front tire for the right
            FLWheel = FL_Wheel_OBJ.transform.Find("tire/valmet_tire_front").gameObject; // Gets the front tire for the left


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

            FL_Wheel.width = 0.2472f; // Widens wheel a tiny bit to account for the wider model. 
            FR_Wheel.width = 0.2472f;

            TractorDriveTrain.maxTorqueRPM = 1450; // Max Torque rpm
            TractorDriveTrain.maxPowerRPM = 2300; // max HP rpm
            TractorDriveTrain.maxPower = 82.16f; // Max hp
            TractorDriveTrain.maxTorque = 289.58f; //Max torque in 





            // Instantite the obj containing the range shift sound
            RangeShiftSource.transform.SetParent(KekmetLOD.transform, false); // parrent sound obj to tractor
            RangeShiftSound = RangeShiftSource.GetComponent<AudioSource>(); //gets audio source part from obj

             //instantiate the obj containing the button sound
            ButtonSoundObj.transform.SetParent(KekmetLOD.transform, false); // parents the button sound object to the tractor
            ButtonSound = ButtonSoundObj.GetComponent<AudioSource>(); //Gets the audiosource part from the obj


            OldLights = KekmetLOD.transform.Find("accessories").gameObject; // Finds the old lights and switches for the tractor
            OldLights.SetActive(false); // Disables it because it will be replaced


             // Instantiates the new lights and switch for the tractor
            NewLights.transform.SetParent(KekmetLOD.transform, false); // Parent to tractor
            NewLights.transform.localPosition = new Vector3(0f, 0.3660001f, -0.002999991f); // Sets position of new lights
            NewLights.transform.localEulerAngles = new Vector3(0f, 270f, 0f); // Rotation adjustment of new light. 

            RearLights = KekmetLOD.transform.Find("WorkLights").gameObject; // Finds existing rear lights
            RearLights.transform.localPosition = new Vector3(0f, -0.3699999f, 3.34f); // Moves rear lights up to account for new model


            LeftLightExisting = RearLights.transform.Find("WorkLightLeft").gameObject; //gets default left rear work light
            Lightflare = LeftLightExisting.GetComponent<Light>().flare; // grabs the flare from left rear work light and stores it.


            DashboardGaugeMat = KekmetLOD.transform.Find("Dashboard/tractor_gauges").gameObject.GetComponent<Renderer>();
            DashboardGaugeMat.material = NewGaugeTex;

            BrakeForceModifier = 1f;

            KekmetSeatCheck = FsmVariables.GlobalVariables.GetFsmString("PlayerCurrentVehicle");

            BrakeForceModifier = BrakeForceSlider.GetValue(); // Needed to update it without reload? Idk i was given this, im not sure.. 
            TurboNoiseToggled = TurboSounds.GetValue(); // Toggles Turbo noises. 

            RangeLever.transform.localPosition = new Vector3(0.15f, 1.130001f, -0.14f);
            RangeLever.transform.localEulerAngles = new Vector3(285f, 180f, 90.00008f);

            OldTractorSoundsObj = KekmetLOD.transform.Find("Sounds/SoundKekmet").gameObject;
            OldTractorSounds = OldTractorSoundsObj.GetComponent<AudioSource>();
            OldTractorSounds.enabled = false;

            NewTractorSounds.transform.SetParent(OldTractorSoundsObj.transform, false);

            FourWheelDriveLever.transform.SetParent(KekmetLOD.transform, false);
            RangeLever.transform.SetParent(KekmetLOD.transform, false);

            FourWheelDriveLever.transform.localPosition = new Vector3(-0.4399999f, 0.6600002f, -0.5750002f);
            FourWheelDriveLever.transform.localEulerAngles = new Vector3(270f, 0f, 0f);

            TractorDriveTrain.SetTransmission(Drivetrain.Transmissions.RWD);
            FrontAxle.frontAxle.powered = false;
            FL_Wheel.isPowered = false;
            FR_Wheel.isPowered = false;

            TractorTurboSound = NewTractorSounds.transform.Find("TurboSound").gameObject.GetComponent<AudioSource>();

            if (AlternateSkinSetting.GetValue() == 1f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkin;
            }
            else if (AlternateSkinSetting.GetValue() == 2f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkinTwo;
            }
            else if (AlternateSkinSetting.GetValue() == 3f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkinThree;
                NewBody.GetComponent<MeshFilter>().mesh = NewBodySkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshFilter>().mesh = DoorRightSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshFilter>().mesh = DoorLeftSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;
            }
            else if (AlternateSkinSetting.GetValue() == 4f)
            {
                NewBody.GetComponent<MeshRenderer>().material = NewBodySkinFour;
                NewBody.GetComponent<MeshFilter>().mesh = NewBodySkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshFilter>().mesh = DoorRightSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorr/door(right)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;

                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshFilter>().mesh = DoorLeftSkinThreeFour;
                GameObject.Find("KEKMET(350-400psi)/DriverDoors/doorl/door(leftx)").GetComponent<MeshRenderer>().material = NewDoorSkinThreeFour;
            }







            NewBody.AddComponent<SmellyGearIndicatorSystem>().SetupModClass(this);
            NewLights.AddComponent<FrontLightFunctions>().SetupModClass(this);
            NewLights.AddComponent<SideLightFunctions>().SetupModClass(this);
            RangeLever.AddComponent<HiTracFunctions>().SetupModClass(this);
            NewTractorSounds.AddComponent<EngineSounds>().SetupModClass(this);
            FourWheelDriveLever.AddComponent<FourWheelDriveFunctions>().SetupModClass(this);
            NewBody.AddComponent<RearWindowFunctions>().SetupModClass(this);
            NewBody.AddComponent<BrakeFunctions>().SetupModClass(this);
            NewBody.AddComponent<HeatFunctions>().SetupModClass(this);
            NewBody.AddComponent<DashboardLights>().SetupModClass(this);
            NewBody.AddComponent<MultiFunctionStickBehavior>().SetupModClass(this);
            

            ab.Unload(false);
        }
        private void Mod_PostLoad()
        {

        }
        private void Mod_OnSave()
        {
            // Called once, when save and quit.
            // Serialize your save file here.
        }
        private void Mod_Update()
        {
   
        }
    }
}
