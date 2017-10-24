/*
 * This script contains the movement controls of a player,
 * applies behaviour of power ups and updates the in-game UI 
 * (e.g. number of power ups, detect if game ended and show
 * winner on screen)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float thrust;
    public float maxVelocity = 150f;
    public new Camera camera;
    public bool isPlayer1 = false;
    public bool speed = false;
    public Transform sparksParticleEffect;
    public PlaneController plane;

    /* UI elements */
    public GameObject[] SpeedyIndicator;
    public Text fps;
    public Text winState;
    public Text endGameDescription;
    public Text score;

    /* Stores value used to detect at which y value a player is indicated as dead (fall off plane) */
    private int deathThreshold = -10;

    /* Stores current number of power ups */
    private int powerUpCount = 0; 
    private int speedyCount = 0; 

    private Color color;
    private Rigidbody rb;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule particleEmitter;
    private int maxParticleRate = 200;
    private bool big = false;
    private bool hiding = false;

    private GameObject ring;

    /* Use this for initialization */
    void Start() {
        rb = this.GetComponent<Rigidbody>();
        color = this.GetComponent<Renderer>().material.color;
        ring = this.transform.GetChild(0).gameObject;

        sparksParticleEffect = Instantiate(sparksParticleEffect, transform.position, transform.rotation);
        particleSystem = sparksParticleEffect.GetComponent<ParticleSystem>();
        particleEmitter = particleSystem.emission;
        particleEmitter.enabled = true;
        
        /* Hide all the power up icons first */
        foreach (GameObject powerUp in SpeedyIndicator) {
            powerUp.SetActive(false);
        }
        fps.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();
        winState.text = "";
        endGameDescription.text = "";
        score.text = GameState.getScoreText(this.gameObject.name);
    }

    /* Controls movement and checks if game ended */
    void Update() {
        CheckIfDead();
        fps.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();

        this.maxVelocity = 150f;
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0;

        /* Allow movement controls only when game has started */
        if (!GameState.gameEnded) {
            CleanScreenText();

            if (isPlayer1) {
                if (Input.GetKey(KeyCode.W)) {
                    rb.AddForce(cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.S)) {
                    rb.AddForce(-cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.A)) {
                    rb.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D)) {
                    rb.transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);
                }
                if (Input.GetKeyDown(KeyCode.Space) && speedyCount > 0) {
                    BoostPowerUp(cameraForward);
                }
            } else {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    rb.AddForce(cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    rb.AddForce(-cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    rb.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.RightArrow)) {
                    rb.transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);
                }
                if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && speedyCount > 0) {
                    BoostPowerUp(cameraForward);
                }
            }
        } else {
            ShowEndGameInformation();
            score.text = GameState.getScoreText(this.gameObject.name);

            /* Check for options after game ended. P to play again and H to go home/main screen. */
            if (Input.GetKey(KeyCode.P)) {
                GameState.gameEnded = false;
            }
            if (Input.GetKey(KeyCode.H)) {
                SceneManager.LoadScene("Start");
            }
        }
    }

    /* Set limit to player's maximum speed */
    void FixedUpdate() {
        if (rb.velocity.magnitude > maxVelocity) {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    /* Create sparks upon collision */
    void OnCollisionEnter(Collision col) {
        sparksParticleEffect.transform.position = col.contacts[0].point;

        /* Generate number of particles according to impact of collision */
        if (col.gameObject.tag == "Player") {
            float particleRate = maxParticleRate * col.relativeVelocity.magnitude/(2 * maxVelocity);
            particleEmitter.rateOverTime = (int) particleRate;
            particleSystem.Play();
        }
    }

    /* Restarts the game if player falls off */
    private void CheckIfDead() {
        if (transform.position.y < deathThreshold) {
            SceneManager.LoadScene("Game");
            GameState.gameEnded = true;
            GameState.losingPlayer = this.gameObject.name;
            GameState.updateScores();
        }
    }

    /* Remove texts from the UI */
    public void CleanScreenText() {
        winState.text = "";
        endGameDescription.text = "";
    }

    /* Show the texts describing the game state after a player lost */
    private void ShowEndGameInformation() {
        winState.text = GameState.getWinText(this.gameObject.name);
        endGameDescription.text = GameState.endGameText;
    }

    /* Called by power up prefab when player collides with it */
    public void IncreaseSpeedyCount() {
        if(speedyCount < 3) {
            SpeedyIndicator[speedyCount].SetActive(true);
            speedyCount++;
        }
    }

    /* Applies effect of Speed Boost power up, adds a large force forward */
    private void BoostPowerUp(Vector3 cameraForward) {
        speedyCount--;
        SpeedyIndicator[speedyCount].SetActive(false);
        this.maxVelocity *= 10f;
        rb.AddForce(cameraForward * thrust * 50);
    }

    /* Applies effect of Condense power up, increases mass of player */
    public void MassUp() {
        big = true;
        this.GetComponent<Rigidbody>().mass *= 30;
        this.thrust *= 30;
        this.maxVelocity = this.maxVelocity * 30;
        this.GetComponent<Renderer>().material.color = Color.grey;
        Invoke("MassDown", 10);
        plane.hider = true;
    }

    /* Resets player to original state (before MassUp()) */
    public void MassDown() {
        big = false;
        this.GetComponent<Rigidbody>().mass /= 30;
        this.thrust /= 30;
        this.maxVelocity = this.maxVelocity / 30;
        this.GetComponent<Renderer>().material.color = color;
        plane.hider = false;
    }

    /* Applies effect of Invisibility power up, makes sphere invisible to opponent, and 
     * slightly transparent for the player (to indicate effect of power up) */
    public void Hide() {
        /* Makes player invisible to opponent with Invisibility layer */
        this.hiding = true;
        gameObject.layer = LayerMask.NameToLayer("Invisibility");
        ring.layer = LayerMask.NameToLayer("Invisibility");
        camera.cullingMask |= 1 << LayerMask.NameToLayer("Invisibility");

        /* Sets the sphere to be slightly transparent to the player by modifying 
         * the opacity of the material's shader */
        Color transparentPlayerColor = this.GetComponent<Renderer>().material.color;
        Color transparentRingColor = ring.GetComponent<Renderer>().material.color;
        transparentPlayerColor.a = 0.5f;
        transparentRingColor.a = 0.5f;
        this.GetComponent<Renderer>().material.color = transparentPlayerColor;
        ring.GetComponent<Renderer>().material.color = transparentRingColor;
        this.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        ring.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
        Invoke("Unhide", 10);
    }

    /* Resets player to original state (before Hide()) */
    public void Unhide() {
        this.hiding = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        ring.layer = LayerMask.NameToLayer("Default");
        camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Invisibility"));

        Color transparentPlayerColor = this.GetComponent<Renderer>().material.color;
        Color transparentRingColor = ring.GetComponent<Renderer>().material.color;
        transparentPlayerColor.a = 1f;
        transparentRingColor.a = 1f;
        this.GetComponent<Renderer>().material.color = transparentPlayerColor;
        ring.GetComponent<Renderer>().material.color = transparentRingColor;
        this.GetComponent<Renderer>().material.shader = Shader.Find("Lit/PlanesLitShader");
        ring.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
    }

    public bool GetBig() {
        return this.big;
    }

    public bool GetHiding() {
        return this.hiding;
    }

    public Color GetColor() {
        return this.color;
    }
}
