using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    static int numActive = 0;
    static float oldest = 0;
    public int maxActive = 30;

    Rigidbody2D body;

    public float speed;
    public float maxAge = 10;

    float age = 0;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        body.velocity = transform.right * speed;
        numActive++;
    }
	
	// Update is called once per frame
	void Update () {
        age += Time.deltaTime;
        if (age >= oldest)
            oldest = age;
        if (age >= maxAge || numActive > maxActive)
        {
            DestroyImmediate(gameObject);
            numActive--;
            oldest = 0;
        }
	}
}
