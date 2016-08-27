using UnityEngine;
using System.Collections;

public class Player : Character {

    GUIStyle guiStyle;

    Texture2D px;

    public GameObject projectile;

    public float wallJumpAngle = 45;
    public float jumpWindow = 0.1F;
    public float bulletTimeCost = 60;

    public Color staminaBarColor;
    public Color staminaBarBackgroundColor;

    public Color coolDownbarColor;
    public float coolDownBarScale = 100;

    bool alreadyWallJumped = false;
    float spacePressTime = 0;

    new void Start () {
        base.Start();

        px = new Texture2D(1, 1);
        px.SetPixel(0, 0, Color.white);

        guiStyle = new GUIStyle();
        guiStyle.normal.background = px;

        wallJumpAngle *= Mathf.PI / 180;
    }

	new void Update () {
        base.Update();

        if (Input.GetKey(KeyCode.Space))
            spacePressTime += Time.deltaTime;
        else
            spacePressTime = 0;

        if (inJumpWindow())
            tryJump();

        if (!left.touch && !right.touch) alreadyWallJumped = false;
        if(inJumpWindow() && (left.touch || right.touch) && spendStamina(jumpCost, jumpCoolDown, false))
        {
            body.velocity = Vector2.zero;
            face(left.touch);
            body.AddForce(new Vector2(jumpForce * Mathf.Cos(wallJumpAngle) * (left.touch ? 1 : -1), jumpForce * Mathf.Sin(wallJumpAngle)));
            alreadyWallJumped = true;
        }

        if (Input.GetKey(KeyCode.D))
            walk(true);

        if (Input.GetKey(KeyCode.A))
            walk(false);

        if (Input.GetKeyDown("left"))
        {
            face(false);
            shoot();
        }
        if (Input.GetKeyDown("right"))
        {
            face(true);
            shoot();
        }

        if (Input.GetMouseButton(1) && spendStamina(Time.deltaTime * (bulletTimeCost + currStaminaRegen), 0, true))
            Time.timeScale = 0.25F;
        else
            Time.timeScale = 1;

        if(Input.GetKey(KeyCode.LeftControl) && (left.touch || right.touch) && !alreadyWallJumped && spendStamina(Time.deltaTime * 50, 0, true))
            body.AddForce(-body.velocity * 30);
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
            proj.transform.Translate(transform.right * 0.1F);
        }
        //body.AddForce(Vector2.left * (facingRight ? 100 : -100));
    }

    GameObject spawnBullet(float angle)
    {
        GameObject proj = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
        proj.transform.Rotate(transform.forward * angle);
        return proj;
    }

    void OnGUI()
    {
        fillRect(new Rect(0, 0, maxStamina, 20), staminaBarBackgroundColor);
        fillRect(new Rect(0, 0, stamina, 20), staminaBarColor);
        fillRect(new Rect(0, 20, coolDown * coolDownBarScale, 20), coolDownbarColor);
    }

    void fillRect(Rect rect, Color color)
    {
        px.SetPixel(0, 0, color);
        px.Apply();

        GUI.DrawTexture(rect, px, ScaleMode.StretchToFill, true, 10);
    }
}
