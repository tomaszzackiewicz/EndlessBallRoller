using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EndlessBallRoller {

    public class HealthBehaviour : MonoBehaviour {

        private GameObject player;
        public AnimationCurve myCurve;
        public GameObject fx;
        public AudioClip impact;
        AudioSource audioSource;

        // Use this for initialization
        void Start() {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {
            transform.position = new Vector3(transform.position.x, myCurve.Evaluate((Time.time % myCurve.length)), transform.position.z);
            transform.Rotate(20 * Time.deltaTime, 20 * Time.deltaTime, 20 * Time.deltaTime);
        }

        void OnTriggerEnter(Collider col) {
            var playerBehaviour = col.gameObject.GetComponent<PlayerBehaviour>();

            // First check if we collided with the player 
            if (playerBehaviour != null) {

                GameObject psObject = Instantiate(fx, transform.position, Quaternion.identity) as GameObject;
                ParticleSystem ps = psObject.GetComponentInChildren<ParticleSystem>();
                ps.Play();
                playerBehaviour.SetProtected();
                Destroy(psObject, 1.0f);

                player = col.gameObject;

                /* var eventData = new Dictionary<string, object>{
                    { "score", playerBehaviour.Score += 1 }
                }; */
                playerBehaviour.UpdateHealth();
                audioSource.PlayOneShot(impact, 1.0f);
                GameObject.Destroy(gameObject, 0.1f);
            }
        }
    }
}
