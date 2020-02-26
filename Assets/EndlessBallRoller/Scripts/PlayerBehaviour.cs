using UnityEngine;
using UnityEngine.UI; // Text
using System.Collections;
using System;
using Random=UnityEngine.Random;

namespace EndlessBallRoller {

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerBehaviour : MonoBehaviour {

        public static event Action<float, bool> RestartGameEvent;
        private Rigidbody rb;

        //[HideInInspector]
        public float dodgeSpeed = 5;
        private float rollSpeed = 10;
        public int health = 100;
        public GameObject protectedImage;
        public enum MobileHorizMovement {
            Accelerometer,
            ScreenTouch
        }
        public Text speedText;
        public Text healthText;

        public Text infoText;
        public Text endInfoText;
        public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;
        public float swipeMove = 2f;
        public float minSwipeDistance = 2f;
        private Vector2 touchStart;
        bool isStuck = false;
        private bool isProtected = false;
        public AudioClip changeBall;
        AudioSource audioSource;
        Image image;
        private float thrust = 0.5f;
        public float waitTime = 2.0f;

        bool isDefaultChanged = true;
        bool isGlassChanged = true;
        bool isRubberChanged = true;
        bool isWoodenChanged = true;
        bool isPlasticChanged = true;
        bool isConcreteChanged = true;
        bool isAluminiumChanged = true;
        bool isCopperChanged = true;
        bool isIronChanged = true;
        bool isTitanChanged = true;
        bool isSilverChanged = true;
        bool isGoldenChanged = true;
        bool isDiamondChanged = true;

        private Material glass;
        private Material wooden;
        private Material rubber;
        private Material plastic;
        private Material concrete;
        private Material aluminium;
        private Material iron;
        private Material copper;
        private Material titan;
        private Material silver;
        private Material golden;
        private Material platinum;

        private int glassLevel = 1000;
        private int woodenLevel = 5000;
        private int rubberLevel = 10000;
        private int plasticLevel = 15000;
        private int concreteLevel = 20000;
        private int aluminiumLevel = 25000;
        private int ironLevel = 30000;
        private int copperLevel = 35000;
        private int titanLevel = 40000;
        private int silverLevel = 45000;
        private int goldenLevel = 50000;
        private int diamondLevel = 55000;

        private string ballType;
        private int playerVelocity;

        public string BallType {
            get {
                return ballType;
            }
        }
        public int PlayerVelocity {
            get {
                return playerVelocity;
            }
        }

        void Start() {
            // Get access to our Rigidbody component 
            rb = GetComponent<Rigidbody>();

            Score = 0;
            speedText.text = ((int)rollSpeed).ToString();
            protectedImage.SetActive(false);
            image = protectedImage.GetComponent<Image>();
            audioSource = GetComponent<AudioSource>();
            coroutine = BlinkCor();
            infoText.text = ballType = "Default Ball";
            endInfoText.text = "Ups, something wrong!!!";

            glass = Resources.Load<Material>("BallMaterials/BallGlass") as Material;
            wooden = Resources.Load<Material>("BallMaterials/BallWood") as Material;
            rubber = Resources.Load<Material>("BallMaterials/BallRubber") as Material;
            plastic = Resources.Load<Material>("BallMaterials/BallPlastic") as Material;
            concrete = Resources.Load<Material>("BallMaterials/BallConcrete") as Material;
            aluminium = Resources.Load<Material>("BallMaterials/BallAlum") as Material;
            iron = Resources.Load<Material>("BallMaterials/BallIron") as Material;
            copper = Resources.Load<Material>("BallMaterials/BallCopper") as Material;
            titan = Resources.Load<Material>("BallMaterials/BallTitan") as Material;
            silver = Resources.Load<Material>("BallMaterials/BallSilver") as Material;
            golden = Resources.Load<Material>("BallMaterials/BallGold") as Material;
            platinum = Resources.Load<Material>("BallMaterials/BallPlatinum") as Material;
        }

        protected virtual void OnRestartGameEvent(bool isReset) {
            if (RestartGameEvent != null) {
                RestartGameEvent(waitTime, isReset);
            }
        }

        void Update() {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f)) {
                if (hit.collider.gameObject.CompareTag("Tile")) {
                    isGrounded = true;
                } else {
                    isGrounded = false;
                }
            }

            if (PauseScreenBehaviour.paused) {
                return;
            }
            healthText.text = health.ToString();
            //Score += Time.deltaTime;

            // Movement in the x axis 
            float horizontalSpeed = 0;

            if (Input.GetKey(KeyCode.DownArrow)) {
                if ((int)rollSpeed >= 0 && isStuck == true) {
                    rb.AddForce(0, 0, -thrust, ForceMode.Impulse);
                }
            } else if (Input.GetKey(KeyCode.UpArrow)) {
                if ((int)rollSpeed <= 100) {
                    rollSpeed += Time.deltaTime * 10;
                }

            } else {
                rollSpeed = 10.0f;
            }


            /*if(Input.GetKeyDown(KeyCode.UpArrow)){
                if(isGrounded){
                    //

                }
                 if((int)rollSpeed < 10){
                    rollSpeed += Time.deltaTime * 10;
                    speedText.text = ((int)rollSpeed).ToString();
                }
            }else if(Input.GetKey(KeyCode.DownArrow)){
                if((int)rollSpeed > 0){
                    rollSpeed -= Time.deltaTime * 10;
                    speedText.text = ((int)rollSpeed).ToString();
                }
            }else{
                //rollSpeed = 5.0f;
            } */
            playerVelocity = (int)rb.velocity.z;
            speedText.text = (playerVelocity).ToString();

            //Check if we are running either in the Unity editor or in a standalone build. 
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

            // Check if we're moving to the side 
            horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

            // If the mouse is held down (or the screen is tapped  
            // on Mobile) 
            if (Input.GetMouseButton(0)) {
                horizontalSpeed = CalculateMovement(Input.mousePosition);
            }

            //Check if we are running on a mobile device 
#elif UNITY_IOS || UNITY_ANDROID

        if(horizMovement == MobileHorizMovement.Accelerometer){
            // Move player based on direction of the accelerometer
            horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        }

        // Check if Input has registered more than zero touches   
        if (Input.touchCount > 0){
            // Store the first touch detected.        
            Touch touch = Input.touches[0];

            if(horizMovement == MobileHorizMovement.ScreenTouch){
                horizontalSpeed = CalculateMovement(touch.position);  
            }

            SwipeTeleport(touch);

            TouchObjects(touch);
        }

#endif

            var movementForce = new Vector3(horizontalSpeed, 0, rollSpeed);

            // Time.deltaTime is the amount of time since the  
            // last frame (approx. 1/60 seconds) 
            //movementForce *= (Time.deltaTime * 60);

            // Apply our auto-moving and movement forces 
            //rb.AddForce(movementForce);
            rb.velocity = movementForce;
        }

        public void SetDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                OnDead();
            }
        }

        bool isGrounded = false;
        void OnCollisionEnter(Collision col) {
            if (col.gameObject.CompareTag("Obstacle")) {
                isStuck = true;
            }
        }

        void OnCollisionExit(Collision col) {
            if (col.gameObject.CompareTag("Obstacle")) {
                isStuck = false;
            }
        }

        private void SwipeTeleport(Touch touch) {
            if (touch.phase == TouchPhase.Began) {
                touchStart = touch.position;
            } else if (touch.phase == TouchPhase.Ended) {
                Vector2 touchEnd = touch.position;

                float x = touchEnd.x - touchStart.x;

                if (Mathf.Abs(x) < minSwipeDistance) {
                    return;
                }

                Vector3 moveDirection;

                if (x < 0) {
                    moveDirection = Vector3.left;
                } else {
                    // Otherwise we're on the right 
                    moveDirection = Vector3.right;
                }

                RaycastHit hit;

                // Only move if we wouldn't hit something 
                if (!rb.SweepTest(moveDirection, out hit, swipeMove)) {
                    // Move the player 
                    rb.MovePosition(rb.position + (moveDirection * swipeMove));
                }
            }
        }

        /// <summary> 
        /// Will figure out where to move the player horizontally 
        /// </summary> 
        /// <param name="pixelPos">The position the player has  
        ///                         touched/clicked on</param> 
        /// <returns>The direction to move in the x axis</returns> 
        float CalculateMovement(Vector3 pixelPos) {
            // Converts to a 0 to 1 scale 
            var worldPos = Camera.main.ScreenToViewportPoint(pixelPos);

            float xMove = 0;

            // If we press the right side of the screen 
            if (worldPos.x < 0.5f) {
                xMove = -1;
            } else {
                // Otherwise we're on the left 
                xMove = 1;
            }

            // replace horizontalSpeed with our own value 
            return xMove * dodgeSpeed;
        }


        /// <summary> 
        /// Will determine if we are touching a game object and if so  
        /// call events for it 
        /// </summary> 
        /// <param name="touch">Our touch event</param> 
        private static void TouchObjects(Touch touch) {
            // Convert the position into a ray 
            Ray touchRay = Camera.main.ScreenPointToRay(touch.position);

            RaycastHit hit;

            // Are we touching an object with a collider? 
            if (Physics.Raycast(touchRay, out hit)) {
                // Call the PlayerTouch function if it exists on a  
                // component attached to this object 
                hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
            }
        }

        private int score = 0;

        public Text scoreText;

        public int Score {
            get { return score; }
            set {
                score = value;

                // Update the text to display the whole number portion  
                // of the score 
                scoreText.text = string.Format("{0:0}", score);
            }
        }

        public bool GetProtected() {
            return isProtected;
        }

        public void SetProtected() {
            if (PlayerVelocity > 50) {
                if (!isProtected) {
                    StartCoroutine(SetProtectedCor());
                }
            }
        }
        private IEnumerator coroutine;
        IEnumerator SetProtectedCor() {
            float protectedTime = Random.Range(10, 15);
            isProtected = true;
            protectedImage.SetActive(true);
            yield return new WaitForSeconds(protectedTime);
            yield return StartCoroutine(BlinkCor());
            //yield return new WaitForSeconds(2.0f);
            //StopCoroutine(coroutine);
            //StopAllCoroutines();
            //yield return new WaitForSeconds(1.0f);
            protectedImage.SetActive(false);
            isProtected = false;
        }

        IEnumerator BlinkCor() {
            int counter = 0;
            int blinks = 6;
            while (counter < blinks) {
                switch (image.color.a.ToString()) {
                    case "0":
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                        yield return new WaitForSeconds(0.1f);
                        break;
                    case "1":
                        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                        yield return new WaitForSeconds(0.1f);
                        break;
                }
                counter++;
            }
            if (counter == 5) {
                yield return null;
            }
        }

        public void OnDead() {
            int noHealth = 0;
            healthText.text = noHealth.ToString();

            if (health <= 0) {

                //Analytics.CustomEvent("Game Over", eventData);

                // Call the function ResetGame after waitTime has passed 
                //Invoke("ResetGame", waitTime);
                RestartGameEvent(waitTime, true);
                gameObject.SetActive(false);
            }
        }


        public void UpdateScore(int scoreAmount) {
            if (scoreAmount > 0 && PlayerVelocity > 0) {
                scoreAmount *= PlayerVelocity;
                Score += scoreAmount;
            } else {
                Score += scoreAmount;
            }

            if (Score > glassLevel && Score < woodenLevel) {//Glass
                if (isDefaultChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = iron;
                    infoText.text = ballType = "Glass Ball";
                    endInfoText.text = "Really bad!";
                    ChangeBallSound();
                    isDefaultChanged = false;
                }
            } else if (Score > woodenLevel && Score < rubberLevel) {//Wooden
                if (isGlassChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = wooden;
                    infoText.text = ballType = "Wooden Ball";
                    endInfoText.text = "Still bad!";
                    ChangeBallSound();
                    isGlassChanged = false;
                }
            } else if (Score > rubberLevel && Score < plasticLevel) {//Rubber
                if (isRubberChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = rubber;
                    infoText.text = ballType = "Rubber Ball";
                    endInfoText.text = "Oh, still not good!";
                    ChangeBallSound();
                    isRubberChanged = false;
                }
            } else if (Score > plasticLevel && Score < concreteLevel) {//Plastic
                if (isPlasticChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = plastic;
                    infoText.text = ballType = "Plastic Ball";
                    endInfoText.text = "On the right way!";
                    ChangeBallSound();
                    isPlasticChanged = false;
                }
            } else if (Score > concreteLevel && Score < aluminiumLevel) {//Concrete
                if (isConcreteChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = concrete;
                    infoText.text = ballType = "Concrete Ball";
                    endInfoText.text = "That's it!";
                    ChangeBallSound();
                    isConcreteChanged = false;
                }
            } else if (Score > aluminiumLevel && Score < ironLevel) {//Aluminium
                if (isAluminiumChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = aluminium;
                    infoText.text = ballType = "Aluminium Ball";
                    endInfoText.text = "You are almost good!";
                    ChangeBallSound();
                    isAluminiumChanged = false;
                }
            } else if (Score > ironLevel && Score < copperLevel) {//Iron
                if (isIronChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = iron;
                    infoText.text = ballType = "Iron Ball";
                    endInfoText.text = "You are quite good!";
                    ChangeBallSound();
                    isIronChanged = false;
                }
            } else if (Score > copperLevel && Score < titanLevel) {//Copper
                if (isCopperChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = copper;
                    infoText.text = ballType = "Copper Ball";
                    endInfoText.text = "You are good!";
                    ChangeBallSound();
                    isCopperChanged = false;
                }
            } else if (Score > titanLevel && Score < silverLevel) {//Titan
                if (isTitanChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = titan;
                    infoText.text = ballType = "Titan Ball";
                    endInfoText.text = "You are really good!";
                    ChangeBallSound();
                    isTitanChanged = false;
                }
            } else if (Score > silverLevel && Score < goldenLevel) {//Silver
                if (isSilverChanged) {
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = silver;
                    infoText.text = ballType = "Silver Ball";
                    endInfoText.text = "You almost the best!";
                    ChangeBallSound();
                    isSilverChanged = false;
                }
            } else if (Score > goldenLevel && Score < diamondLevel) {//Golden
                if (isGoldenChanged) {
                    ChangeBallSound();
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = golden;
                    infoText.text = ballType = "Golden Ball";
                    endInfoText.text = "You are the best!";
                    isGoldenChanged = false;
                }
            } else if (Score > diamondLevel) {//Diamond
                if (isDiamondChanged) {
                    ChangeBallSound();
                    Renderer ren = gameObject.GetComponent<Renderer>();
                    ren.material = platinum;
                    infoText.text = ballType = "Platinum Ball";
                    endInfoText.text = "You are the winner!!!";
                    isDiamondChanged = false;
                    RestartGameEvent(waitTime, false);
                    gameObject.SetActive(false);
                }
            }

        }

        void ChangeBallSound() {
            audioSource.PlayOneShot(changeBall, 1.0f);
        }

        public void UpdateHealth() {
            if (!isProtected) {
                if (health <= 100) {
                    health += PlayerVelocity;
                    if (health > 100) {
                        health = 100;
                    }
                }
            }
        }
    }
}