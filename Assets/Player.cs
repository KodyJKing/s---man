using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    Sensor foot;
    Sensor left;
    Sensor right;

    Rigidbody2D body;
    SpriteRenderer sprite;
    GUIStyle guiStyle;

    Texture2D px;

    public GameObject projectile;

    public Sprite[] walkFrames;
    public float animSpeed = 3;
    public float walkingForce = 1000;

    public float jumpForce = 500;
    public float jumpCost = 20;
    public float jumpCoolDown = 0.25F;
    public float wallJumpAngle = 45;
    public float airControl = 0.2F;
    public float staminaRegen = 50;
    public float airStaminaRegen = 20;
    public float maxStamina = 100;
    public float jumpWindow = 0.1F;
    public float bulletTimeCost = 60;
    public float maxWalkSpeed = 10;

    public Color staminaBarColor;
    public Color staminaBarBackgroundColor;

    public Color coolDownbarColor;
    public float coolDownBarScale = 100;

    float sideJumpForce;

    bool facingRight = true;
    bool alreadyWallJumped = false;
    float walkTime;
    float stamina;
    float coolDown;
    float spacePressTime = 0;

    void Start () {
        px = new Texture2D(1, 1);
        px.SetPixel(0, 0, Color.white);

        guiStyle = new GUIStyle();
        guiStyle.normal.background = px;

        sideJumpForce = Mathf.Sqrt(2) / 2 * jumpForce;
        wallJumpAngle *= Mathf.PI / 180;

        foot = transform.Find("foot").gameObject.GetComponent<Sensor>();
        left = transform.Find("left_side").gameObject.GetComponent<Sensor>();
        right = transform.Find("right_side").gameObject.GetComponent<Sensor>();

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
	}

	void Update () {

        float currStaminaRegen = foot.touch ? staminaRegen : airStaminaRegen;
        stamina += currStaminaRegen * Time.deltaTime;
        if (stamina > maxStamina)
            stamina = maxStamina;

        coolDown -= Time.deltaTime;
        if (coolDown < 0)
            coolDown = 0;

        float control = foot.touch ? 1F : airControl;

        if (Input.GetKey(KeyCode.Space))
            spacePressTime += Time.deltaTime;
        else
            spacePressTime = 0;

        if (inJumpWindow() && foot.touch && spendStamina(jumpCost, jumpCoolDown, false))
            body.AddForce(new Vector2(0, jumpForce));

        if (!left.touch && !right.touch) alreadyWallJumped = false;
        if(inJumpWindow() && (left.touch || right.touch) && spendStamina(jumpCost, jumpCoolDown, false))
        {
            body.velocity = Vector2.zero;
            face(left.touch);
            body.AddForce(new Vector2(jumpForce * Mathf.Cos(wallJumpAngle) * (left.touch ? 1 : -1), jumpForce * Mathf.Sin(wallJumpAngle)));
            alreadyWallJumped = true;
        }

        if (Input.GetKey(KeyCode.D)) {
            body.AddForce(new Vector2(walkingForce * control * Time.deltaTime, 0));
            if(foot.touch) face(true);
            if (body.velocity.x > maxWalkSpeed)
                body.velocity = new Vector2(maxWalkSpeed, body.velocity.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            body.AddForce(new Vector2(-walkingForce * control * Time.deltaTime, 0));
            if (foot.touch) face(false);
            if (body.velocity.x < -maxWalkSpeed)
                body.velocity = new Vector2(-maxWalkSpeed, body.velocity.y);
        }

        if (Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < 5; i++)
            {
                float randomAngle = Random.Range(-10, 10);
                GameObject proj = spawnBullet((!facingRight ? 180 : 0) + randomAngle);
                proj.transform.Translate(transform.right * 0.28F);
            }
            body.AddForce(Vector2.left * (facingRight ? 100 : -100));
        }

        if (Input.GetMouseButton(1) && spendStamina(Time.deltaTime * (bulletTimeCost + currStaminaRegen), 0, true))
            Time.timeScale = 0.25F;
        else
            Time.timeScale = 1;

        if(Input.GetKey(KeyCode.LeftControl) && (left.touch || right.touch) && !alreadyWallJumped && spendStamina(Time.deltaTime * 50, 0, true))
            body.AddForce(-body.velocity * 30);

        walkTime += Mathf.Abs(body.velocity.x) * Time.deltaTime;

        if (Mathf.Abs(body.velocity.x) < 0.01)
            walkTime = 0;

        sprite.sprite = foot.touch ? walkFrames[Mathf.FloorToInt(walkTime * animSpeed) % 4] : walkFrames[0];
	}

    bool spendStamina(float amount, float coolDown, bool ignoreCooldown)
    {
        if ((ignoreCooldown || this.coolDown <= 0) && stamina >= amount)
        {
            stamina -= amount;
            this.coolDown = coolDown;
            return true;
        }
        return false;
    }

    bool inJumpWindow()
    {
        return spacePressTime > 0 && spacePressTime <= jumpWindow;
    }

    void face(bool right)
    {
        facingRight = right;
        sprite.flipX = !right;
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
