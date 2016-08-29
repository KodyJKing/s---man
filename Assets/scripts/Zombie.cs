using UnityEngine;
using System.Collections;

public class Zombie : Character {

    GameObject player;

    public float seekDist = 1;
    public float maxSeekDist = 10;

    // Use this for initialization
    new void Start () {
        base.Start();

        player = GameObject.Find("player");
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();

        float dist = (player.transform.position - transform.position).magnitude;
        bool seeking = dist > seekDist && dist < maxSeekDist;

        if (player.transform.position.x > transform.position.x)
        {
            face(true);
            if (true) walk(true);
        } else
        {
            face(false);
            if (true) walk(false);
        }

        if (facingRight && right.touch || !facingRight && left.touch)// || Mathf.Abs(body.velocity.x) < 1F && seeking)
            body.AddForce(Vector2.up * 50);

        if(Mathf.Abs(body.velocity.x) < 1F && seeking && Random.Range(0, 1F) < 0.5F)
            tryJump();
    }
}