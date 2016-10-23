using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    bool victory = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("You wun!");
        victory = true;
    }

    void OnGUI()
    {
        if(victory)
            GUI.Label(new Rect(Screen.width / 2, Screen.height * 3.0F / 4, 200, 20), "YOU WON!");
    }
}
