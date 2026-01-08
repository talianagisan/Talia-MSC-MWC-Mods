using MSCLoader;
using UnityEngine;
using HutongGames.PlayMaker;
using System.IO;
using System.Text;
using System;
using Random = UnityEngine.Random;

namespace Hayosiko4x4
{
    public class Hayosiko4x4 : Mod
    {
        public override string ID => "Hayosiko4x4"; //Your mod ID (unique)
        public override string Name => "Hayosiko4x4"; //You mod name
        public override string Author => "TaliaKuznetsova"; //Your Username
        public override string Version => "2.3"; //Version
        public override string Description => "Adds realistic 4x4 system to the Hayosiko"; //Short description of your mod

        public enum RANGE
        {
            H2,
            L4,
            H4
        };

        public RANGE range;
        bool Pause;
        public GameObject Van;
        Drivetrain drivetrain;

        GameObject RangeLever;
        GameObject VisualLever;
        GameObject ShiftArmVisual;

        GameObject ShiftTrigger;
        GameObject ShiftFixTrigger;

        GameObject ShiftSoundObj;
        GameObject GrindSoundObj;
        GameObject HSShiftSoundObj;
        GameObject FixSoundObj;

        string RaycastHit;
        string ShiftArm = "4x4 Shift Arm Linkage";
        string ShiftLever = "4x4 Selector";
        string BrokeText1 = "Something feels disconnected...";
        string BrokeText2 = "Still broken...";
        string BrokeText3 = "Something feels loose underneath...";
        string BrokeText4 = "Still broken idiot.";
        string BrokeText5 = "Now you're just playing with it.";
        string BrokeText6 = "Oh wait something, Wait... Nevermind still broken dickhead.";
        string BrokeText7 = "READ THE MOD DESCRIPTION";
        string Gear;

        AudioSource ShiftSound;
        AudioSource GrindSound;
        AudioSource HSShiftSound;
        AudioSource FixSound;

        bool ShiftArmFixed = false;
        float BrokenAttempts = 0;

        FsmFloat FuelUsage;

        PlayMakerFSM SWEAR;

        
        public override void ModSetup()
        {
            SetupFunction(Setup.OnNewGame, Mod_OnNewGame);
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.Update, Mod_Update);
            SetupFunction(Setup.OnSave, Mod_OnSave);

        }

        public override void ModSettings()
        {
            // All settings should be created here. 
            // DO NOT put anything else here that settings or keybinds
        }
        private void Mod_OnNewGame()
        {
            if (File.Exists(Path.Combine(ModLoader.GetModAssetsFolder(this), "Save.txt")))
            {
                File.Delete(Path.Combine(ModLoader.GetModAssetsFolder(this), "Save.txt"));
            }
        }

        private void Mod_OnLoad()
        {

            if (File.Exists(Path.Combine(ModLoader.GetModAssetsFolder(this), "Save.txt")))
            {
                string mySavedBool = File.ReadAllText(Path.Combine(ModLoader.GetModAssetsFolder(this), "Save.txt"));
                ShiftArmFixed = Convert.ToBoolean(mySavedBool);
            }
            else
            {
                ShiftArmFixed = false;
            }

            AssetBundle ab = LoadAssets.LoadBundle(this, "hayosiko4x4");

            //foreach (string a in ab.GetAllAssetNames())
            //{
            //ModConsole.Warning(a);
            //ModConsole.Print("You can ignore this. This is for development.");
            //}

            Van = GameObject.Find("HAYOSIKO(1500kg, 250)");//Specifies what van is
            drivetrain = Van.GetComponent<Drivetrain>();

            FuelUsage = Van.transform.Find("LOD/Simulation/FuelUsage").GetComponent<PlayMakerFSM>().FsmVariables.FindFsmFloat("Consumption");


            RangeLever = GameObject.Instantiate(ab.LoadAsset<GameObject>("GearLever.prefab"));
            RangeLever.transform.SetParent(Van.transform, false);
            RangeLever.transform.localPosition = new Vector3(-0.05f, 0.342f, 1.557f);
            RangeLever.transform.localEulerAngles = new Vector3(270f, 180f, 0);

            VisualLever = GameObject.Find("HAYOSIKO(1500kg, 250)/GearLever(Clone)/lever");
            VisualLever.transform.localEulerAngles = new Vector3(-13f, 3f, 22.15f);

            ShiftArmVisual = GameObject.Instantiate(ab.LoadAsset<GameObject>("GearLinkage.prefab"));
            ShiftArmVisual.transform.SetParent(Van.transform, false);

            if (!ShiftArmFixed)
            {
                ShiftArmVisual.transform.localPosition = new Vector3(-0.17f, -0.01f, 0.58f);
                ShiftArmVisual.transform.localEulerAngles = new Vector3(257f, 0f, 0f);
            }
            else
            {
                ShiftArmVisual.transform.localPosition = new Vector3(-0.17f, -0.01f, 0.58f);
                ShiftArmVisual.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
            }


            ShiftTrigger = GameObject.Instantiate(ab.LoadAsset<GameObject>("4x4ShiftTrigger.prefab"));
            ShiftTrigger.transform.SetParent(Van.transform, false);


            ShiftTrigger.transform.localPosition = new Vector3(-0.06f, 0.469f, 1.45f);
            ShiftTrigger.transform.localScale = new Vector3(0.09f, 0.32f, 0.25f);

            ShiftFixTrigger = GameObject.Instantiate(ab.LoadAsset<GameObject>("4x4ShiftFixTrigger.prefab"));
            ShiftFixTrigger.transform.SetParent(Van.transform, false);

            ShiftFixTrigger.transform.localPosition = new Vector3(-0.11f, 0.01f, 0.4f);
            ShiftFixTrigger.transform.localScale = new Vector3(0.15f, 0.25f, 0.5f);


            ShiftSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("4x4ShiftSound.prefab"));
            ShiftSoundObj.transform.SetParent(Van.transform, false);

            FixSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("4x4ArmRepairSound.prefab"));
            FixSoundObj.transform.SetParent(Van.transform, false);

            GrindSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("4x4Grind.prefab"));
            GrindSoundObj.transform.SetParent(Van.transform, false);


            HSShiftSoundObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("4x4FastShift.prefab"));
            HSShiftSoundObj.transform.SetParent(Van.transform, false);


            ShiftSound = ShiftSoundObj.GetComponent<AudioSource>();
            GrindSound = GrindSoundObj.GetComponent<AudioSource>();
            HSShiftSound = HSShiftSoundObj.GetComponent<AudioSource>();
            FixSound = FixSoundObj.GetComponent<AudioSource>();

            ab.Unload(false);
            range = RANGE.H2;

            SWEAR = GameObject.Find("PLAYER/Pivot/AnimPivot/Camera/FPSCamera/SpeakDatabase").GetComponent<PlayMakerFSM>();



        }

        private void Mod_OnSave()
        {
            File.WriteAllText(Path.Combine(ModLoader.GetModAssetsFolder(this), "Save.txt"), ShiftArmFixed.ToString());
        }

        private void Mod_Update()
        {

            RaycastHit[] hits = UnifiedRaycast.GetRaycastHits();
            for (int i = 0; i < hits.Length; ++i)
            {
                RaycastHit = hits[i].collider.transform.gameObject.name;
            }

            if (RaycastHit == "4x4ShiftFixTrigger(Clone)")
            {
                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = ShiftArm;
               

                if (cInput.GetButtonDown("Use") && !ShiftArmFixed)
                {
                    ShiftArmFixed = true;
                    ShiftArmVisual.transform.localPosition = new Vector3(-0.17f, -0.01f, 0.58f);
                    ShiftArmVisual.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
                    if (FixSound.isPlaying)
                        FixSound.Stop();
                    FixSound.Play();
                }
            }


            if (RaycastHit == "4x4ShiftTrigger(Clone)")
            {

                PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = ShiftLever + "(" + Gear + ")";

                if (ShiftArmFixed)
                {

                    if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        Pause = false;
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (drivetrain.wheelTireVelo <= 3 && drivetrain.wheelTireVelo > 1 && range == RANGE.H2)
                        {
                            if (HSShiftSound.isPlaying)
                                HSShiftSound.Stop();
                            HSShiftSound.Play();
                            range = RANGE.H4;
                            Pause = true;
                        }
                        else if (drivetrain.wheelTireVelo <= 1 && range == RANGE.H2)
                        {
                            if (ShiftSound.isPlaying)
                                ShiftSound.Stop();
                            ShiftSound.Play();
                            range = RANGE.H4;
                            Pause = true;
                        }
                        else if (drivetrain.wheelTireVelo <= 1 && range == RANGE.H4 && Pause != true)
                        {

                            if (ShiftSound.isPlaying)
                                ShiftSound.Stop();
                            ShiftSound.Play();
                            range = RANGE.L4;
                        }
                        else if (drivetrain.wheelTireVelo > 3)
                        {

                            if (GrindSound.isPlaying)
                                GrindSound.Stop();
                            GrindSound.Play();
                        }
                        else if (drivetrain.wheelTireVelo > 1 && drivetrain.wheelTireVelo < 3 && range == RANGE.H4 && !Pause)
                        {

                            if (GrindSound.isPlaying)
                                GrindSound.Stop();
                            GrindSound.Play();

                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        if (drivetrain.wheelTireVelo <= 1 && range == RANGE.L4)
                        {
                            if (ShiftSound.isPlaying)
                                ShiftSound.Stop();
                            ShiftSound.Play();
                            range = RANGE.H4;
                            Pause = true;
                        }
                        else if (drivetrain.wheelTireVelo > 1 && range == RANGE.H4 && Pause != true)
                        {
                            if (HSShiftSound.isPlaying)
                                HSShiftSound.Stop();
                            HSShiftSound.Play();
                            range = RANGE.H2;
                        }
                        else if (drivetrain.wheelTireVelo <= 1 && range == RANGE.H4 && Pause != true)
                        {
                            if (ShiftSound.isPlaying)
                                ShiftSound.Stop();
                            ShiftSound.Play();
                            range = RANGE.H2;
                        }
                        else if (this.Van.GetComponent<Drivetrain>().wheelTireVelo < 3 && range == RANGE.L4)
                        {
                            if (GrindSound.isPlaying)
                                GrindSound.Stop();
                            GrindSound.Play();
                            
                        }
                        else if (this.Van.GetComponent<Drivetrain>().wheelTireVelo > 3 && range == RANGE.L4)
                        {
                            if (GrindSound.isPlaying)
                                GrindSound.Stop();
                            GrindSound.Play();
                            
                        }
                    }




                    if (range == RANGE.H2 && drivetrain.wheelTireVelo > 1)
                    {
                        FuelUsage.Value = 322000f;
                    }
                    else if (range == RANGE.H4 && drivetrain.wheelTireVelo > 1)
                    {
                        FuelUsage.Value = 316000;
                    }
                    else if (range == RANGE.L4 && drivetrain.wheelTireVelo > 1)
                    {
                        FuelUsage.Value = 316000;
                    }
                    else
                    {
                        FuelUsage.Value = 350000f;
                    }




                    switch (range)
                    {

                        case RANGE.H2:
                            drivetrain.SetTransmission(Drivetrain.Transmissions.RWD); //Set's it to RWD
                            VisualLever.transform.localEulerAngles = new Vector3(-13f, 3f, 22.15f);
                            Gear = "H2";
                            break;

                        case RANGE.H4:
                            drivetrain.finalDriveRatio = 4.286f;//default gear ratio
                            drivetrain.SetTransmission(Drivetrain.Transmissions.AWD);//Set's it to 4x4/AWD
                            VisualLever.transform.localEulerAngles = new Vector3(-8f, 3f, 22.15f);
                            Gear = "H4";
                            break;

                        case RANGE.L4:
                            drivetrain.finalDriveRatio = 9.77208f; //2.28:1 reduction in gear ratio
                            drivetrain.SetTransmission(Drivetrain.Transmissions.AWD);//Set's it to 4x4/AWD                   
                            VisualLever.transform.localEulerAngles = new Vector3(2f, -3f, 22.15f);
                            Gear = "L4";
                            break;
                    }
                } else if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0))
                {
                    if (BrokenAttempts == 0)
                    {
                        PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = BrokeText1;
                    }
                    else
                    {

                    }

                    
                    SWEAR.SendEvent("SWEARING");
                }
                else
                {
                    Gear = "H2";
                }
            }            
        }
    }
}
