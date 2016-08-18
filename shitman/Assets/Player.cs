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

    public float staminaRegen = 50;
    public float maxStamina = 100;
    public float jumpWindow = 0.1F;

    public Color staminaBarColor;
    public Color staminaBarBackgroundColor;

    public Color coolDownbarColor;
    public float coolDownBarScale = 100;

    float sideJumpForce;

    bool facingRight = true;
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

        if (foot.touch)
            stamina += staminaRegen * Time.deltaTime;
        if (stamina > maxStamina)
            stamina = maxStamina;

        coolDown -= Time.deltaTime;
        if (coolDown < 0)
            coolDown = 0;

        float control = foot.touch ? 1F : 0.2F;
        bool moving = false;

        if (Input.GetKey("space"))
            spacePressTime += Time.deltaTime;
        else
            spacePressTime = 0;

        if (inJumpWindow() && foot.touch && spendStamina(jumpCost, jumpCoolDown, false))
            body.AddForce(new Vector2(0, jumpForce));

        if(inJumpWindow() && (left.touch || right.touch) && spendStamina(jumpCost, jumpCoolDown, false))
        {
            face(left.touch);
            body.AddForce(new Vector2(jumpForce * Mathf.Cos(wallJumpAngle) * (left.touch ? 1 : -1), jumpForce * Mathf.Sin(wallJumpAngle)));
        }

        if (Input.GetKey("d")) {
            body.AddForce(new Vector2(1000 * control * Time.deltaTime, 0));
            if(foot.touch) face(true);
            moving = true;
        }

        if (Input.GetKey("a"))
        {
            body.AddForce(new Vector2(-walkingForce * control * Time.deltaTime, 0));
            if(foot.touch) face(false);
            moving = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject proj = (GameObject)Instantiate(projectile, transform.position, transform.rotation);
            if (!facingRight)
                proj.transform.Rotate(transform.forward * 180);
            proj.transform.Translate(transform.right * 0.28F);
        }
            

        if (foot.touch && !moving)
            body.AddForce(new Vector2(- body.velocity.x * Time.deltaTime * 100, 0));

        walkTime += Mathf.Abs(body.velocity.x) * Time.deltaTime;

        sprite.sprite = foot.touch ? walkFrames[Mathf.FloorToInt(walkTime * animSpeed) % 4] : walkFrames[0];
	}

    bool spendStamina(float amount, float coolDown, bool ignoreCooldown)
    {
        Debug.Log(coolDown);
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

    void OnGUI()
    {
        box(new Rect(0, 0, maxStamina, 20), staminaBarBackgroundColor);
        box(new Rect(0, 0, stamina, 20), staminaBarColor);
        box(new Rect(0, 20, coolDown * coolDownBarScale, 20), coolDownbarColor);

        //float coolDownBarLength = coolDown * coolDownBarScale;
        //box(new Rect(Screen.width - coolDownBarLength, 0, coolDownBarLength, 20), coolDownbarColor);
    }

    void box(Rect rect, Color color)
    {
        px.SetPixel(0, 0, color);
        px.Apply();

        GUI.DrawTexture(rect, px, ScaleMode.StretchToFill, true, 10);
    }
}
