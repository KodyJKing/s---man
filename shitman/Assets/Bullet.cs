using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    Rigidbody2D body;

    public float speed;
    public float maxAge = 10;

    float age = 0;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        body.velocity = transform.right * speed;
    }
	
	// Update is called once per frame
	void Update () {
        age += Time.deltaTime;
        if (age >= maxAge)
            DestroyImmediate(gameObject);
	}
}
