using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Sensor foot;
    Rigidbody2D body;
    SpriteRenderer sprite;

    public Sprite[] walkFrames;
    float walkTime = 0;

    void Start () {
        foot = transform.Find("foot").gameObject.GetComponent<Sensor>();
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
	}

    float animSpeed = 3;
    float walkingForce = 1000;
    float jumpForce = 500;
	void Update () {

        float control = foot.touch ? 1F : 0.15F;
        bool moving = false;

        if (Input.GetKeyDown("space") && foot.touch)
            body.AddForce(new Vector2(0, jumpForce));

        if (Input.GetKey("d")) {
            body.AddForce(new Vector2(1000 * control * Time.deltaTime, 0));
            face(true);
            moving = true;
        }

        if (Input.GetKey("a"))
        {
            body.AddForce(new Vector2(-walkingForce * control * Time.deltaTime, 0));
            face(false);
            moving = true;
        }

        if (foot.touch && !moving)
            body.AddForce(new Vector2(- body.velocity.x * Time.deltaTime * 100, 0));

        walkTime += Mathf.Abs(body.velocity.x) * Time.deltaTime;

        sprite.sprite = foot.touch ? walkFrames[Mathf.FloorToInt(walkTime * animSpeed) % 4] : walkFrames[0];
	}

    bool facingRight;
    void face(bool right)
    {
        facingRight = right;
        sprite.flipX = !right;
    }
}
