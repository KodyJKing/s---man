using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {

    public bool touch = false;
    public GameObject contact = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Active!");
        touch = true;
        contact = other.gameObject;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Active!");
        touch = true;
        contact = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Inactive!");
        touch = false;
    }
}
