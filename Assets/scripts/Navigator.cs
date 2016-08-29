using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Navigator : Character {

    public GameObject platA;

    // Use this for initialization
    new void Start () {
        base.Start();
    }

    bool dir = true;
    bool jumped = false;

	// Update is called once per frame
	new void Update () {
        base.Update();

        if (foot.touch && !jumped)
        {
            walk(dir);
        }
        if (foot.touch && canMakeJump() && !jumped)
        {
            tryJump();
            jumped = true;
        }
    }

    public override void tryJump()
    {
        if (foot.touch && spendStamina(jumpCost, jumpCoolDown, false))
        {
            body.velocity += new Vector2(0, jumpForce);
        }
    }

    bool canMakeJump()
    {
        Vector2 jumpVel = body.velocity + Vector2.up * jumpForce;
        RaycastHit2D[] hits = Navigation.arcTrace(body.position + Vector2.down * 0.2F, jumpVel, body.gravityScale * Physics2D.gravity, 10, 100, "platform");
        for(int i = 0; i < 100; i++)
        {
            if (hits[i] && hits[i].transform.gameObject != gameObject)
                return Mathf.Abs(hits[i].normal.x) < 0.2F && hits[i].transform.gameObject == platA;
        }
        return false;
    }
}
