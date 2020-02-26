using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;

namespace EndlessBallRoller {

    public class ObstacleBehaviour : MonoBehaviour {

        public GameObject fx;
        public int damage = 10;
        public AudioClip impact;
        private AudioSource audioSource;
        private bool isDamage;
        private GameObject player;

        void Start() {
            isDamage = true;
            audioSource = GetComponent<AudioSource>();
        }

        void OnCollisionEnter(Collision collision) {
            var playerBehaviour = collision.gameObject.GetComponent<PlayerBehaviour>();

            if (playerBehaviour != null) {
                GameObject psObject = Instantiate(fx, transform.position, Quaternion.identity) as GameObject;
                ParticleSystem ps = psObject.GetComponentInChildren<ParticleSystem>();
                ps.Play();
                Destroy(psObject, 1.0f);

                if (isDamage && !playerBehaviour.GetProtected()) {
                    playerBehaviour.SetDamage(damage);
                    isDamage = false;
                }
                player = collision.gameObject;
                playerBehaviour.UpdateScore(-damage);
                audioSource.PlayOneShot(impact, 1.0f);
            }
        }

    }
}