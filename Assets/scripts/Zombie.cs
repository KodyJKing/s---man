﻿using UnityEngine;
using System.Collections;

public class Zombie : Character {

    GameObject player;

    protected Sensor hitbox;

    public int damage = 50;

    // Use this for initialization
    new void Start () {
        base.Start();

        hitbox = transform.Find("hitbox").gameObject.GetComponent<Sensor>();

        player = GameObject.Find("player");
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();

        bool seeking = !hitbox.touch && hasLineOfSight();

        if (player.transform.position.x > transform.position.x)
        {
            face(true);
            if (seeking) walk(true);
        } else
        {
            face(false);
            if (seeking) walk(false);
        }

        if ((facingRight && right.touch || !facingRight && left.touch) && spendStamina(0.1F * Time.deltaTime, 0, true))
            body.AddForce(Vector2.up * 20);
            
        if(Mathf.Abs(body.velocity.x) < 1F && seeking && Random.Range(0, 1F) < 0.5F)
            tryJump();

        if(hitbox.touch && spendStamina(34, 1, false))
        {
            Vector2 throwDir = (Vector2)(player.transform.position - transform.position).normalized + Vector2.up;
            player.SendMessage("takeDamage", damage);
            player.SendMessage("knockback", throwDir * 55);
        }
    }

    bool hasLineOfSight()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        Vector3 from = transform.position + toPlayer.normalized * 2;
        Vector3 to = player.transform.position;
        Debug.DrawLine(from, to, Color.white, 10, false);
        RaycastHit2D hit = Physics2D.Raycast(from, to - from, toPlayer.magnitude);
        Debug.DrawLine(from, hit.point, Color.red, 10, false);
        return hit.transform != null && hit.transform.gameObject == player;
    }
}