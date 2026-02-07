using MSCLoader;
using UnityEngine;

namespace Kekmet602T_MWC
{
    public class EngineSounds : MonoBehaviour
    {

        public AudioSource HighSound, MediumSound, LowSound, IdleSound, ThrottleOffSound, ThrottleOnSound, TractorTurboSound;

        // Use this for initialization
        private Kekmet602T_MWC MainKekmetMod;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }
        void Start()
        {
            

            HighSound = MainKekmetMod.NewTractorSounds.transform.Find("High").gameObject.GetComponent<AudioSource>();
            MediumSound = MainKekmetMod.NewTractorSounds.transform.Find("Med").gameObject.GetComponent<AudioSource>();
            LowSound = MainKekmetMod.NewTractorSounds.transform.Find("Low").gameObject.GetComponent<AudioSource>();
            IdleSound = MainKekmetMod.NewTractorSounds.transform.Find("Idle").gameObject.GetComponent<AudioSource>();
            ThrottleOffSound = MainKekmetMod.NewTractorSounds.transform.Find("ThrottleOff").gameObject.GetComponent<AudioSource>();
            ThrottleOnSound = MainKekmetMod.NewTractorSounds.transform.Find("ThrottleOn").gameObject.GetComponent<AudioSource>();
            TractorTurboSound = MainKekmetMod.NewTractorSounds.transform.Find("TurboSound").gameObject.GetComponent<AudioSource>();

        }

        // Update is called once per frame.
        void Update()
        {
            if (MainKekmetMod.TractorDriveTrain.rpm < 200)
            {
                IdleSound.volume = 0f;
                TractorTurboSound.volume = 0f;

            }

            if (MainKekmetMod.TractorDriveTrain.rpm < 550)
            {
                LowSound.volume = 0f;

            }

            if (MainKekmetMod.TractorDriveTrain.rpm < 700)
            {
                MediumSound.volume = 0f;

            }

            if (MainKekmetMod.TractorDriveTrain.rpm < 1150)
            {
                HighSound.volume = 0f;

            }



            if (MainKekmetMod.TractorDriveTrain.rpm >= 200 & MainKekmetMod.TractorDriveTrain.rpm < 800)
            {
                IdleSound.volume = 1 - (MainKekmetMod.TractorDriveTrain.rpm * 0.00056666666f);
                IdleSound.pitch = MainKekmetMod.TractorDriveTrain.rpm * 0.0023f;
            }

            if (MainKekmetMod.TractorDriveTrain.rpm >= 800)
            {
                IdleSound.volume = 1 - (MainKekmetMod.TractorDriveTrain.rpm * 0.00056666666f);
            }



            if (MainKekmetMod.TractorDriveTrain.rpm >= 700 & MainKekmetMod.TractorDriveTrain.rpm < 1700)
            {
                MediumSound.volume = MainKekmetMod.TractorDriveTrain.rpm * 0.00041823529f;
                MediumSound.pitch = MainKekmetMod.TractorDriveTrain.rpm * 0.000667f;
            }

            if (MainKekmetMod.TractorDriveTrain.rpm >= 1700)
            {
                MediumSound.volume = 1 - MainKekmetMod.TractorDriveTrain.rpm * 0.00032f;
            }


            if (MainKekmetMod.TractorDriveTrain.rpm >= 1150)
            {
                HighSound.volume = MainKekmetMod.TractorDriveTrain.rpm * 0.00033949047f;
                HighSound.pitch = MainKekmetMod.TractorDriveTrain.rpm * 0.00047619047f;
            }


            if (MainKekmetMod.TractorDriveTrain.rpm > 200 & MainKekmetMod.TurboNoiseToggled)
            {
                TractorTurboSound.pitch = ((MainKekmetMod.TractorDriveTrain.rpm * 0.00025f) + 0.5f);
                TractorTurboSound.volume = ((MainKekmetMod.TractorDriveTrain.rpm * 0.00001830434f)); // Sets pitch and volume based on RPM and a preset modifer
            }
        }
    }
}