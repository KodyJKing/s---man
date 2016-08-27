using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    Sensor foot;
    Sensor left;
    Sensor right;

    Rigidbody2D body;
    SpriteRenderer sprite;

    public Sprite[] walkFrames;
    public float animSpeed = 3;

    float walkTime;

    // Use this for initialization
    void Start () {
        foot = transform.Find("foot").gameObject.GetComponent<Sensor>();
        left = transform.Find("left_side").gameObject.GetComponent<Sensor>();
        right = transform.Find("right_side").gameObject.GetComponent<Sensor>();

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        walkTime += Mathf.Abs(body.velocity.x) * Time.deltaTime;

        if (Mathf.Abs(body.velocity.x) < 0.01)
            walkTime = 0;

        sprite.sprite = foot.touch ? walkFrames[Mathf.FloorToInt(walkTime * animSpeed) % walkFrames.Length] : walkFrames[0];
    }
}
