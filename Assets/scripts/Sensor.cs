using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {

    public bool touch = false;
    public bool isOld = false;
    public GameObject contact = null;
    public string tagFilter = "";

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Active!");
        if (!passesFilter(other))
            return;
        touch = true;
        contact = other.gameObject;
        isOld = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Active!");
        if (!passesFilter(other))
            return;
        touch = true;
        contact = other.gameObject;
        //isOld = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("Inactive!");
        if (!passesFilter(other))
            return;
        touch = false;
        isOld = false;
    }

    bool passesFilter(Collider2D other)
    {
        return tagFilter == "" || other.tag == tagFilter;
    }
}
