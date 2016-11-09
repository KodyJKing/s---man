using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Navigator : Character {

    public GameObject platA;

    // Use this for initialization
    float endX, endXV, wTime;
    float time = 6;
    new void Start () {
        base.Start();
        walkModel(time, 100, true, 54F, body.position.x, body.velocity.x, out endX, out endXV);
    }

    bool dir = true;
    bool jumped = false;

	// Update is called once per frame
	new void Update () {
        base.Update();

        //if (foot.touch && !jumped)
        //{
        //    walk(dir);
        //}
        //if (foot.touch && canMakeJump() && !jumped)
        //{
        //    tryJump();
        //    jumped = true;
        //    Debug.Log("Jumped!");
        //}

        //Debug.Log(body.velocity.x);

        walk(true);
        wTime += Time.deltaTime;
        if (wTime > time)
        {
            Debug.Log(body.position.x - endX);
            Time.timeScale = 0;
        }
    }

    public override bool tryJump()
    {
        if (foot.touch && spendStamina(jumpCost, jumpCoolDown, false))
        {
            body.velocity += new Vector2(0, jumpForce);
            return true;
        }
        return false;
    }

    bool canMakeJump()
    {
        Vector2 jumpVel = body.velocity + Vector2.up * jumpForce;
        RaycastHit2D[] hits = Navigation.arcTrace(body.position + Vector2.down, jumpVel, body.gravityScale * Physics2D.gravity, 10, 100, "platform");
        for(int i = 0; i < 100; i++)
        {
            if (hits[i] && hits[i].transform.gameObject != gameObject)
                return Mathf.Abs(hits[i].normal.x) < 0.2F && hits[i].transform.gameObject == platA;
        }
        return false;
    }

    void walkModel(float time, int steps, bool dir, float friction, float x, float xv, out float xo, out float xvo)
    {
        float dt = time / steps;
        for(int i = 0; i < steps; i++)
        {
            float accl = (walkingForce - friction * body.mass * Mathf.Sign(xv)) / body.mass;
            xv += accl * dt;
            if (Mathf.Abs(xv) > maxWalkSpeed)
                xv = maxWalkSpeed * Mathf.Sign(xv);
            x += xv * dt + accl * dt * dt * 0.5F;
        }
        xo = x;
        xvo = xv;
    }
}
