using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GILighting : MonoBehaviour {
    public new Renderer renderer;
    // Use this for initialization
    void Start () {
        renderer = GetComponent<Renderer>();

    }
	
	// Update is called once per frame
	void Update () {
        float intensity = 5.0f;
        Color baseColor = new Color(0, 244, 255, 255);
        Color final = baseColor * Mathf.LinearToGammaSpace(intensity);
        renderer.material.SetColor("_EmissionColor", final);
        renderer.UpdateGIMaterials();
        
    }
}
