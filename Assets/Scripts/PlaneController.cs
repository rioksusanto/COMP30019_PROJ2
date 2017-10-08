using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour {

    public PowerUpBehaviour powerUpPrefab;
    private int i = 0;
    private float scale_x;
    private float scale_z;

    // Use this for initialization
    void Start () {
		scale_x = this.transform.localScale.x * 2;
        scale_z = this.transform.localScale.z * 2;
    }
	
	// Update is called once per frame
	void Update () {
        i += 1;

        if ((i % 155) == 44) 
        {
            PowerUpBehaviour p = Instantiate<PowerUpBehaviour>(powerUpPrefab);
            p.transform.position = new Vector3(Random.Range(-scale_x, scale_x), 2.0f, Random.Range(-scale_z, scale_z));
        }
	}
}
