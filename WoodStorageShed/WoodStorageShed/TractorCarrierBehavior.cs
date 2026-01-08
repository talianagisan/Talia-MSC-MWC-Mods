using MSCLoader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WoodStorageShed
{
    public class TractorCarrierBehavior : MonoBehaviour
    {

        public int WoodCountTractor;

        AudioSource WoodSoundTake;
        AudioSource WoodSoundAdd;

        public GameObject WoodCarrierTractor;
        public GameObject WoodCarrierTractorCollider;
        public GameObject LogPiecesTractor;
        public GameObject FirewoodPrefab;
        public GameObject WoodSoundTakeObj;
        public GameObject WoodSoundAddObj;
        public GameObject WoodSpawnerPositioner;

        public Rigidbody CarrierRigidbody;

        public List<GameObject> FirewoodPiecesTractor = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            WoodCarrierTractor = GameObject.Find("TractorWoodCarrier(Clone)");
            LogPiecesTractor = GameObject.Find("TractorWoodCarrier(Clone)/LogBits");
            WoodCarrierTractorCollider = GameObject.Find("TractorWoodCarrier(Clone)/Colliders/MainBodyColl");
            WoodSpawnerPositioner = GameObject.Find("TractorWoodCarrier(Clone)/WoodSpawnPos");

            CarrierRigidbody = WoodCarrierTractor.GetComponent<Rigidbody>();

            WoodSoundTakeObj = GameObject.Find("TractorWoodCarrier(Clone)/SoundSources/WoodTake");
            WoodSoundAddObj = GameObject.Find("TractorWoodCarrier(Clone)/SoundSources/WoodAdd");

            WoodSoundTake = WoodSoundTakeObj.GetComponent<AudioSource>();
            WoodSoundAdd = WoodSoundAddObj.GetComponent<AudioSource>();

            FirewoodPrefab = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(x => x.name == "firewood" && x.transform.root == x.transform && x.activeSelf && !x.activeInHierarchy);

            foreach (Transform child in LogPiecesTractor.transform) FirewoodPiecesTractor.Add(child.gameObject);

        }
        
        // Update is called once per frame
        void Update()
        {
            if (UnifiedRaycast.GetHit(WoodCarrierTractorCollider.GetComponent<Collider>()))
            {

                if (WoodCountTractor > 0)
                {
                    PlayMakerGlobals.Instance.Variables.FindFsmBool("GUIuse").Value = true;
                    if (cInput.GetButtonDown("Use"))
                    {
                        WoodCountTractor -= 1;
                        WoodSoundTake.Play();
                        Instantiate(FirewoodPrefab, (WoodSpawnerPositioner.transform.position), Quaternion.identity);

                    }
                }
            }

            CarrierRigidbody.mass = 90 + (WoodCountTractor*2);

            for (var i = 0; i < FirewoodPiecesTractor.Count; ++i)
            {
                FirewoodPiecesTractor[i].SetActive(WoodCountTractor > i);
            }
        }

        private void OnTriggerEnter(Collider TractorCarrier)
        {
            if (TractorCarrier.gameObject.name == "firewood(Clone)" && WoodCountTractor < 47)
            {
                Destroy(TractorCarrier.gameObject);
                WoodCountTractor += 1;
                WoodSoundAdd.Play();
            }
        }
    }
}