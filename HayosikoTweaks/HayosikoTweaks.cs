using MSCLoader;
using UnityEngine;

namespace HayosikoTweaks
{
    public class HayosikoTweaks : Mod
    {
        public override string ID => "HayosikoTweaks"; //Your mod ID (unique)
        public override string Name => "HayosikoTweaks"; //You mod name
        public override string Author => "TaliaKuznetsova"; //Your Username
        public override string Version => "1.0B"; //Version
        public override string Description => ""; //Short description of your mod

        public Drivetrain VanDriveTrain;
        public GameObject Van;
        public SoundController VanSoundController;
        public Rigidbody VanRigidBody;

        public Wheel FrWheel;
        public Wheel FlWheel;
        public Wheel RrWheel;
        public Wheel RlWheel;

        public GameObject FrWheelOBJ;
        public GameObject FlWheelOBJ;
        public GameObject RrWheelOBJ;
        public GameObject RlWheelOBJ;

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.Update, Mod_Update);
        }

        public override void ModSettings()
        {

        }

        private void Mod_OnLoad()
        {
            Van = GameObject.Find("HAYOSIKO(1500kg, 250)");

            VanDriveTrain = Van.GetComponent<Drivetrain>();
            VanSoundController = Van.GetComponent<SoundController>();
            VanRigidBody = Van.GetComponent<Rigidbody>();

            FrWheelOBJ = GameObject.Find("HAYOSIKO(1500kg, 250)/wheelFR");
            FlWheelOBJ = GameObject.Find("HAYOSIKO(1500kg, 250)/wheelFL");
            RrWheelOBJ = GameObject.Find("HAYOSIKO(1500kg, 250)/wheelRR");
            RlWheelOBJ = GameObject.Find("HAYOSIKO(1500kg, 250)/wheelRL");

            FrWheel = FrWheelOBJ.GetComponent<Wheel>();
            FlWheel = FlWheelOBJ.GetComponent<Wheel>();
            RlWheel = RlWheelOBJ.GetComponent<Wheel>();
            RrWheel = RrWheelOBJ.GetComponent<Wheel>();

            VanDriveTrain.maxTorqueRPM = 2400;
            VanDriveTrain.maxPowerRPM = 4200;

            VanDriveTrain.maxPower = 75f;
            VanDriveTrain.maxTorque = 145f;

            VanDriveTrain.minRPM = 750f;
            VanDriveTrain.maxRPM = 4800f;

            VanSoundController.engineThrottlePitchFactor = 1.8f;
            VanSoundController.engineNoThrottlePitchFactor = 1.7f;

            VanRigidBody.mass = 1365f;

            FrWheel.suspensionRate = 32000f;
            FlWheel.suspensionRate = 30000f;
            RlWheel.suspensionRate = 35000f;
            RrWheel.suspensionRate = 43000f;

            FrWheel.bumpRate = 750f;
            FlWheel.bumpRate = 600f;
            RlWheel.bumpRate = 1800f;
            RrWheel.bumpRate = 2300f;

            FrWheel.reboundRate = 750f;
            FlWheel.reboundRate = 600f;
            RlWheel.reboundRate = 1800f;
            RrWheel.reboundRate = 2300f;

        }
        private void Mod_Update()
        {
           
        }
    }
}
