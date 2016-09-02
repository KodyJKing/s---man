using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    protected Sensor foot;
    protected Sensor walkFoot;
    protected Sensor left;
    protected Sensor right;

    protected Rigidbody2D body;
    protected SpriteRenderer sprite;

    public Sprite[] walkFrames;
    public Sprite idleFrame;

    public float animSpeed = 3;

    protected float walkTime;
    protected bool facingRight;

    public float jumpForce = 500;
    public float jumpCost = 20;
    public float jumpCoolDown = 0.25F;

    public float walkingForce = 750;
    public float maxWalkSpeed = 6.5F;

    public float staminaRegen = 50;
    public float airStaminaRegen = 20;
    public float maxStamina = 100;
    public float maxHealth = 100;
    public float airControl;

    protected float health;
    protected float stamina;
    protected float coolDown;
    protected float currStaminaRegen;
    protected float control;

    // Use this for initialization
    protected void Start () {
        foot = transform.Find("foot").gameObject.GetComponent<Sensor>();
        walkFoot = transform.Find("walkFoot").gameObject.GetComponent<Sensor>();
        left = transform.Find("left_side").gameObject.GetComponent<Sensor>();
        right = transform.Find("right_side").gameObject.GetComponent<Sensor>();

        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        stamina = maxStamina;
        health = maxHealth;
    }

    // Update is called once per frame
    protected void Update () {
        control = walkFoot.touch ? 1F : airControl;
        currStaminaRegen = walkFoot.touch ? staminaRegen : airStaminaRegen;
        stamina += currStaminaRegen * Time.deltaTime;
        if (stamina > maxStamina)
            stamina = maxStamina;

        coolDown -= Time.deltaTime;
        if (coolDown < 0)
            coolDown = 0;

        walkTime += Mathf.Abs(body.velocity.x) * Time.deltaTime;

        if (Mathf.Abs(body.velocity.x) < 0.01)
            walkTime = 0;

        sprite.flipX = !facingRight;
        setFrame();

        if (health <= 0)
            onDeath();
    }

    public void face(bool right)
    {
        sprite.flipX = !facingRight;
        facingRight = right;
    }

    public bool spendStamina(float amount, float coolDown, bool ignoreCooldown)
    {
        if ((ignoreCooldown || this.coolDown <= 0) && stamina >= 0)
        {
            stamina -= amount;
            if (coolDown > 0) this.coolDown = coolDown;
            return true;
        }
        return false;
    }

    public void walk(bool dir)
    {
        face(dir);
        body.AddForce(new Vector2((dir ? 1 : -1) * walkingForce * control * Time.deltaTime, 0));
        if (dir)
        {
            if (body.velocity.x > maxWalkSpeed)
                body.velocity = new Vector2(maxWalkSpeed, body.velocity.y);
        } else
        {
            if (body.velocity.x < -maxWalkSpeed)
                body.velocity = new Vector2(-maxWalkSpeed, body.velocity.y);
        }

    }

    public virtual bool tryJump()
    {
        if(foot.touch && spendStamina(jumpCost, jumpCoolDown, false))
        {
            body.AddForce(new Vector2(0, jumpForce));
            return true;
        }
        return false;
    }
    
    protected virtual void setFrame()
    {
        setWalkFrame();
    }

    protected void setWalkFrame()
    {
        if (Mathf.Abs(body.velocity.x) < 0.1F)
            sprite.sprite = idleFrame;
        else
            sprite.sprite = walkFoot.touch ? walkFrames[Mathf.FloorToInt(walkTime * animSpeed) % walkFrames.Length] : walkFrames[0];
    }

    protected virtual void onDeath()
    {
        DestroyImmediate(gameObject);
    }
}
