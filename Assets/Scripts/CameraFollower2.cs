using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower2 : MonoBehaviour {

    public Transform player;

    /* Camera offset keeps track of where the camera should be positioned
     * in relative to the player. In this script, assumes player faces
     * towards positive z-axis. */
    private Vector3 cameraOffset = new Vector3(0, 5, -10);
    private float angleToPlayer = 0.0f;
    private float sensitivity = 2.5f;

    private void Start() {
        this.transform.position = player.position + cameraOffset;
        this.transform.LookAt(player.position);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            angleToPlayer -= sensitivity;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            angleToPlayer += sensitivity;
        }
    }

    /* Updates the camera's offset with respect to player */
    private void LateUpdate() {
        Quaternion rotation = Quaternion.Euler(0, angleToPlayer, 0);
        this.transform.position = player.position + rotation * cameraOffset;
        this.transform.LookAt(player.position);
    }
}
