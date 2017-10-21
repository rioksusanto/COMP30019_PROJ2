using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

    public PowerUpBehaviour powerUpPrefab;
    private float i = 0;
    private float scale_x;
    private float scale_z;
    private float scale_y;
    private float spawnTime = 2;
    public bool hider = false;

    public float getScale_x()
    {
        return scale_x;
    }

    public float getScale_z()
    {
        return scale_z;
    }

    public float getScale_y()
    {
        return scale_y;
    }

    // Use this for initialization
    void Start () {
		scale_x = this.transform.localScale.x * 4;
        scale_z = this.transform.localScale.z * 4;
        scale_y = this.transform.localScale.y * 4;
    }
	
	// Update is called once per frame
	void Update () {
        i += Time.deltaTime;

        if (i >= spawnTime && !GameState.gameEnded) 
        {
            PowerUpBehaviour p = Instantiate<PowerUpBehaviour>(powerUpPrefab);
            p.transform.position = new Vector3(Random.Range(-scale_x, scale_x), 2.0f, Random.Range(-scale_z, scale_z));
            i = 0;
        }
	}
}
