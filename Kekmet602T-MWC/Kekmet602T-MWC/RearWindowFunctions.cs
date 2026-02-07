using MSCLoader;
using UnityEngine;

namespace Kekmet602T_MWC
{
    public class RearWindowFunctions : MonoBehaviour
    {


        public GameObject RearWindow, RearGlass, WindowOpenSoundObj, WindowCloseSoundObj, RearWindowCollider;
        public AudioSource WindowOpenSound, WindowCloseSound;
        public Collider RearWindowTrigger;
        public Mesh RearGlassNewMesh;
        public bool WindowOpen;

        // Use this for initialization..
        private Kekmet602T_MWC MainKekmetMod;
        public void SetupModClass(Kekmet602T_MWC mainMod)
        {
            MainKekmetMod = mainMod;
        }


        void Start()
        {
            RearWindow = MainKekmetMod.NewBody.transform.Find("Kekmet602RearWindowDoor").gameObject;


            MainKekmetMod.RearGlassNewObj.SetActive(false);
            RearGlassNewMesh = MainKekmetMod.RearGlassNewObj.GetComponent<MeshFilter>().mesh;


            RearGlass = MainKekmetMod.KekmetLOD.transform.Find("Body/glass_rear").gameObject;
            RearGlass.transform.SetParent(RearWindow.transform, false);
            RearGlass.GetComponent<MeshFilter>().mesh = RearGlassNewMesh;

            RearGlass.transform.localPosition = new Vector3(-0.05f, -0.4f, 0f);
            RearGlass.transform.localEulerAngles = new Vector3(0f, 0f, 0f);


            MainKekmetMod.MainBodyAnimation.Play("WindowClose");


            WindowOpenSoundObj = RearWindow.transform.Find("WindowOpenSound").gameObject;
            WindowCloseSoundObj = RearWindow.transform.Find("WindowCloseSound").gameObject;

            WindowOpenSound = WindowOpenSoundObj.GetComponent<AudioSource>();
            WindowCloseSound = WindowCloseSoundObj.GetComponent<AudioSource>();

            RearWindowTrigger = RearWindow.transform.GetComponent<BoxCollider>();

            RearWindowCollider = MainKekmetMod.Kekmet.transform.Find("Colliders/coll7").gameObject;
            RearWindowCollider.transform.SetParent(RearWindow.transform, false);

            RearWindowCollider.transform.localPosition = new Vector3(1.27f, -1.54f, 0.01793455f);
            RearWindowCollider.transform.localEulerAngles = new Vector3(270f, 0f, 0f);



        }

        // Update is called once per frame
        void Update()
        {
            if (UnifiedRaycast.GetHit(RearWindowTrigger))
            {
                PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                {
                    if (!WindowOpen)
                    {
                        PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Open Window";
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            WindowOpen = true;
                            MainKekmetMod.MainBodyAnimation.Play("WindowOpen");
                            WindowOpenSound.Play();
                        }
                    }
                    else
                    {
                        PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = "Close Window";
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            MainKekmetMod.MainBodyAnimation.Play("WindowClose");
                            WindowOpen = false;
                            WindowCloseSound.Play();
                        }
                    }
                }
            }

        }
    }
}