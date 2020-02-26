using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessBallRoller {

    public class TileController : MonoBehaviour {

        public GameObject flowerPotParent;
        public GameObject coinParent;
        public GameObject shieldParent;
        public GameObject trashBinParent;
        public GameObject healthVolumes;

        public int tileNumber;

        public int TileNumber {
            get {
                return tileNumber;
            }
            set {
                tileNumber = value;
            }
        }

        void Start() {
            healthVolumes.transform.localPosition = new Vector3(Random.Range(-2, 2), 0, Random.Range(-15, 15));
        }

        public void SetTileVolume() {
            healthVolumes.SetActive(true);
        }

        public void UnSetTileVolume() {
            healthVolumes.SetActive(false);
        }

    }
}
