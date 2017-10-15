using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public Transform player;

    /* Camera offset keeps track of where the camera should be positioned
     * in relative to the player */
    private Vector3 cameraOffset;
    private float angleToPlayer = 0.0f;
    private float sensitivity = 2.5f;
    private Player playerObject;

    private void Start() {
        playerObject = (Player)player.GetComponent<Player>();
        if(playerObject.isPlayer1) {
            cameraOffset = new Vector3(0, 5, 10);
        } else {
            cameraOffset = new Vector3(0, 5, -10);
        }
        this.transform.position = player.position + cameraOffset;
        this.transform.LookAt(player.position);
    }

    private void Update() {
        if (!GameState.gameEnded) {
            if (playerObject.isPlayer1) {
                if (Input.GetKey(KeyCode.A)) {
                    angleToPlayer -= sensitivity;
                }
                if (Input.GetKey(KeyCode.D)) {
                    angleToPlayer += sensitivity;
                }
            }
            else {
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    angleToPlayer -= sensitivity;
                }
                if (Input.GetKey(KeyCode.RightArrow)) {
                    angleToPlayer += sensitivity;
                }
            }
        }
    }

    /* Updates the camera's offset with respect to player */
    private void LateUpdate() {
        Quaternion rotation = Quaternion.Euler(0, angleToPlayer, 0);
        this.transform.position = player.position + rotation * cameraOffset;
        this.transform.LookAt(player.position);
    }

}
