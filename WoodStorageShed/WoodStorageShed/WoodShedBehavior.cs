using MSCLoader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodStorageShed
{
    public class WoodShedBehavior : MonoBehaviour
    {
        public int WoodCountShed;
        AudioSource WoodSound;
        public GameObject WoodRack;
        public GameObject LogPiecesShed;
        public GameObject FirewoodPrefab;
        public List<GameObject> FirewoodPiecesShed = new List<GameObject>();


        // Use this for initialization
        void Start()
        {
            WoodRack = GameObject.Find("WoodRack(Clone)");
            LogPiecesShed = GameObject.Find("WoodRack(Clone)/LogPieces");

            WoodSound = WoodRack.GetComponent<AudioSource>();

            FirewoodPrefab = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "firewood" && x.transform.root == x.transform && x.activeSelf && !x.activeInHierarchy);

            foreach (Transform child in LogPiecesShed.transform) FirewoodPiecesShed.Add(child.gameObject);


        }

        // Update is called once per frame
        void Update()
        {
            if (UnifiedRaycast.GetHit(WoodRack.GetComponent<Collider>()))
            {

                if (WoodCountShed > 0)
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                    if (cInput.GetButtonDown("Use"))
                    {
                        WoodCountShed -= 1;
                        WoodSound.Play();
                        Instantiate(FirewoodPrefab, new Vector3(-9.6f, -0.5f, -1.6f), Quaternion.identity); ;
                    }
                }
            }

            for (var i = 0; i < FirewoodPiecesShed.Count; ++i)
            {
                FirewoodPiecesShed[i].SetActive(WoodCountShed > i);
            }
        }

        private void OnTriggerEnter(Collider WoodShed)
        {
            if (WoodShed.gameObject.name == "firewood(Clone)" && WoodCountShed < 80)
            {
                Destroy(WoodShed.gameObject);
                WoodCountShed += 1;
                WoodSound.Play();
            }
        }
    }
}