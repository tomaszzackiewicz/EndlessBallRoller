using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessBallRoller {

    public class ShieldBehaviour : MonoBehaviour {

        private GameObject player;
        public AnimationCurve myCurve;
        public GameObject fx;
        public AudioClip impact;
        AudioSource audioSource;

        void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        void Update() {
            transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
            transform.Rotate(20 * Time.deltaTime, 20 * Time.deltaTime, 20 * Time.deltaTime);
        }

        void OnTriggerEnter(Collider col) {
            var playerBehaviour = col.gameObject.GetComponent<PlayerBehaviour>();

            if (playerBehaviour != null) {

                GameObject psObject = Instantiate(fx, transform.position, Quaternion.identity) as GameObject;
                ParticleSystem ps = psObject.GetComponentInChildren<ParticleSystem>();
                ps.Play();
                playerBehaviour.SetProtected();
                Destroy(psObject, 1.0f);

                player = col.gameObject;
                playerBehaviour.UpdateScore(1);
                audioSource.PlayOneShot(impact, 1.0f);
                GameObject.Destroy(gameObject, 0.1f);
            }
        }
    }
}
