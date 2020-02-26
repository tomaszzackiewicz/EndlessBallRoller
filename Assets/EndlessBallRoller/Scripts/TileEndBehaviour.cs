using UnityEngine;

namespace EndlessBallRoller {

    public class TileEndBehaviour : MonoBehaviour {
        public float destroyTime = 1.5f;

        void OnTriggerEnter(Collider col) {
            if (col.gameObject.GetComponent<PlayerBehaviour>()) {
                GameObject.FindObjectOfType<GameController>().SpawnNextTile();
                Destroy(transform.parent.gameObject, destroyTime);
            }
        }
    }
}