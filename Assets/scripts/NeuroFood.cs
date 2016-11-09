using UnityEngine;
using System.Collections;

public class NeuroFood : MonoBehaviour {

    public Vector2 lowerBound, upperBound;
    public float coolDown = 5;

    Renderer r;
    Collider2D c;
    float coolDownLeft;

    // Use this for initialization
    void Start () {
        r = GetComponent<Renderer>();
        c = GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        coolDownLeft -= Time.deltaTime;
        if(coolDownLeft < 0)
        {
            r.enabled = true;
            c.enabled = true;
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.gameObject.name);
        other.gameObject.SendMessage("eat");
        coolDownLeft = coolDown;
        r.enabled = false;
        c.enabled = false;
        transform.position = lowerBound + Random.value * (upperBound.x - lowerBound.x) * Vector2.right + Random.value * (upperBound.y - lowerBound.y) * Vector2.up;
    }
}
