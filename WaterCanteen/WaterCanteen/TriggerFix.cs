using UnityEngine;


public class TriggerFix : MonoBehaviour
{
    public WaterTriggerScript triggerScript;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "WaterTap")
        {
            triggerScript.WaterLevel = 7;
        }
    }
}
