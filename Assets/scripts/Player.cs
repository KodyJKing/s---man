using UnityEngine;
using System.Collections;
using Assets.scripts;

public class Player : Character {

    GUIStyle guiStyle;

    Texture2D px;

    public GameObject projectile;

    public float wallJumpAngle = 45;
    public float jumpWindow = 0.1F;
    public float bulletTimeCost = 60;

    public float dashCost = 40;
    public float dashCooldown = 0.4F;

    public Color staminaBarColor;
    public Color staminaBarBackgroundColor;
    public Color healhBarColor;
    public Color healthBarBackgroundColor;

    public Color coolDownbarColor;
    public float coolDownBarScale = 100;

    bool alreadyWallJumped = false;
    float spacePressTime = 0;
    float fallTime = 0;
    float wallJumpTime = 99;
    float dashTime = 99;

    public Sprite[] fallFrames;
    public Sprite[] wallJumpFrames;
    public Sprite wallFrame;
    public Sprite dashFrame;

    public Vector3 spawnPoint;

    new void Start() {
        base.Start();

        px = new Texture2D(1, 1);
        px.SetPixel(0, 0, Color.white);

        guiStyle = new GUIStyle();
        guiStyle.normal.background = px;

        wallJumpAngle *= Mathf.PI / 180;
    }

    new void Update() {
        base.Update();

        if (Input.GetKey(KeyCode.Space))
            spacePressTime += Time.deltaTime;
        else
            spacePressTime = 0;

        if (foot.touch || body.velocity.y > 0)
            fallTime = 0;
        else
            fallTime += Time.deltaTime;
       
        if (inJumpWindow())
        {
            if(tryJump())
                spacePressTime += jumpWindow;
        }

        wallJumpTime += Time.deltaTime;
        if (foot.touch) wallJumpTime = 99;
        if (!left.touch && !right.touch) alreadyWallJumped = false;
        if (inJumpWindow() && (left.touch || right.touch) && spendStamina(jumpCost, jumpCoolDown, false))
        {
            body.velocity = Vector2.zero;
            face(left.touch);
            body.AddForce(new Vector2(jumpForce * Mathf.Cos(wallJumpAngle) * (left.touch ? 1 : -1), jumpForce * Mathf.Sin(wallJumpAngle)));
            alreadyWallJumped = true;
            wallJumpTime = 0;
            spacePressTime += jumpWindow;
        }

        if (Input.GetKey(KeyCode.D))
            walk(true);

        if (Input.GetKey(KeyCode.A))
            walk(false);

        dashTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            face(false);
            dash();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            face(true);
            dash();
        }

        if (Input.GetKey(KeyCode.RightShift) && spendStamina(Time.deltaTime * (bulletTimeCost + currStaminaRegen), 0, true))
            Time.timeScale = 0.5F;
        else
            Time.timeScale = 1;

        if (Input.GetKey(KeyCode.LeftShift) && (left.touch ^ right.touch) && !alreadyWallJumped && spendStamina(Time.deltaTime * 50, 0, true))
        {
            if (right.touch)
                body.AddForce(Vector2.right * Time.deltaTime * 1000);
            else
                body.AddForce(Vector2.left * Time.deltaTime * 1000);
            body.AddForce(-body.velocity * 30);
            fallTime = 0;
        }

        if ((left.touch ^ right.touch) && !foot.touch)
            face(right.touch);

        if (Input.GetKey(KeyCode.Q))
            stamina = maxStamina;

        if (fallTime > 3)
            onDeath();

        if (Input.GetKeyDown(KeyCode.P))
            health -= 50;
    }

    bool inJumpWindow()
    {
        return spacePressTime > 0 && spacePressTime <= jumpWindow;
    }

    void shoot()
    {
        for (int i = 0; i < 1; i++)
        {
            float randomAngle = Random.Range(-0, 0);
            GameObject proj = spawnBullet((!facingRight ? 180 : 0) + randomAngle);
            proj.transform.Translate(transform.right * 0.15F);
        }
        //body.AddForce(Vector2.left * (facingRight ? 100 : -100));
    }

    void dash()
    {
        if (!spendStamina(dashCost, dashCooldown, false))
            return;

        dashTime = 0;
        body.velocity *= 0.25F;
        body.AddForce((facingRight ? Vector2.right : Vector2.left) * 300);
    }

    GameObject spawnBullet(float angle)
    {
        GameObject proj = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        proj.transform.Rotate(transform.forward * angle);
        return proj;
    }

    void OnGUI()
    {
        fillRect(new Rect(0, 0, maxHealth, 20), healthBarBackgroundColor);
        fillRect(new Rect(0, 0, health, 20), healhBarColor);
        fillRect(new Rect(0, 20, maxStamina, 20), staminaBarBackgroundColor);
        fillRect(new Rect(0, 20, stamina, 20), staminaBarColor);
        fillRect(new Rect(0, 40, coolDown * coolDownBarScale, 20), coolDownbarColor);
    }

    void fillRect(Rect rect, Color color)
    {
        px.SetPixel(0, 0, color);
        px.Apply();

        GUI.DrawTexture(rect, px, ScaleMode.StretchToFill, true, 10);
    }

    override protected void setFrame()
    {
        if (wallJumpTime < 0.5F)
            setWallJumpFrame();
        else if ((left.touch || right.touch) && !foot.touch)
            sprite.sprite = wallFrame;
        else if (dashTime < 0.5F)
            sprite.sprite = dashFrame;
        else if (fallTime > 0)
            setFallFrame();
        else
            setWalkFrame();
    }

    void setFallFrame()
    {
        int frame = Mathf.Min(Mathf.FloorToInt(fallTime * 15), fallFrames.Length - 1);
        sprite.sprite = fallFrames[frame];
    }

    void setWallJumpFrame()
    {
        int frame = Mathf.Min(Mathf.FloorToInt(wallJumpTime * 20), wallJumpFrames.Length - 1);
        sprite.sprite = wallJumpFrames[frame];
    }

    void findObstacle()
    {
        Navigation.arcTrace(body.position, body.velocity, body.gravityScale * Physics2D.gravity, 1, 10, "platform");
    }

    protected override void onDeath()
    {
        alreadyWallJumped = false;
        fallTime = 0;
        body.position = spawnPoint;
        health = maxHealth;
        stamina = maxStamina;
        body.velocity = new Vector2(0, 0);
    }
}
