
using UnityEngine;
using HutongGames.PlayMaker;



public class WaterTriggerScript : MonoBehaviour
{
    public float WaterLevel;
    public GameObject Canteen;
    public AudioSource CanteenSoundSource;
    public bool Pause;
    public FsmFloat Thirst;
    string NoThirst = "You are not thirsty";

    // Use this for initialization
    void Start()
    {
        
        Thirst = PlayMakerGlobals.Instance.Variables.FindFsmFloat("PlayerThirst");

        
    }

    // Update is called once per frame
    void Update()
    {

        if (cInput.GetButtonUp("Use"))
        {
            Pause = false;   
        }

        if (cInput.GetButtonDown("Use"))
        {
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 1.0f);
            for (int i = 0; i < hits.Length; ++i)
            {
                if (hits[i].collider.transform.gameObject.name == "Water Canteen(itemx)" && Pause == false && WaterLevel > 0 && Thirst.Value >= 4.5f)
                {
                    if (CanteenSoundSource.isPlaying)
                        CanteenSoundSource.Stop();
                    CanteenSoundSource.Play();
                    WaterLevel -= 1f;
                    Pause = true;
                    Thirst.Value -= 4.5f;
                }else if (Thirst.Value < 4.5f)
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmString("GUIinteraction").Value = NoThirst;
                }
            }
        }       
    }
}
