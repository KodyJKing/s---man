using UnityEngine;
using System.Collections.Generic;

class NeuralCharacter : Character
{

    FFN brain;

    float energy;
    float memory;
    NeuralCharacter parent;

    static int eyeCount = 16;
    static int motionCount = 2;
    static int randomCount = 2;
    static int memCount = 1;

    static int population = 0;

    new void Start()
    {
        base.Start();

        GLCanvas.subscribers.Add(gameObject);
        population++;

        energy = 10;
        //brain = new FFN(eyeCount + motionCount + randomCount + memCount, 8, 4, 2 + memCount);
        brain = new FFN(motionCount + eyeCount * 2, 8, 4, 2);
        if (parent != null)
        {
            brain.setGenome(parent.brain.getGenome());
            brain.mutationRadius = 5;
            brain.mutationRate = 0.5F;
            brain.mutate();
        } else
        {
            brain.mutationRadius = 10;
            brain.mutationRate = 1F;
            brain.mutate();
        }
    }

    new void Update()
    {
        base.Update();

        List<float> input = new List<float>();
        //input.Add(body.velocity.x);
        //input.Add(body.velocity.y);
        input.Add(body.position.x);
        input.Add(body.position.y);
        //input.Add(Mathf.Sin(Time.time * 2 * Mathf.PI) * 100);
        //input.Add(Random.value * 20);
        addVision(input);
        //input.Add(memory);

        float[] response = brain.response(input.ToArray());
        if (response[0] > 10)
            walk(true);
        if (response[0] < 10)
            walk(false);
        if (response[1] > 20)
            tryJump();

        //memory = Mathf.Min(response[2], 100);

        energy -= Time.deltaTime;

        if (body.position.y < -5 || energy < 0)
            onDeath();
    }

    void addVision(List<float> input)
    {
        float angle = 0;
        float da = Mathf.PI / (eyeCount - 1);
        for(int i = 0; i < eyeCount; i++)
        {
            Vector2 look = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 origin = body.position + Vector2.up;
            RaycastHit2D hit = Physics2D.Raycast(origin, look);
            bool isHit = hit.collider != null;
            bool isFood = isHit && hit.collider.gameObject.tag == "Food";
            input.Add(hit.collider == null ? 1000 : hit.distance);
            input.Add(isFood ? 1000 : 0);
            Debug.DrawLine(origin, origin + look * 2, isFood ? Color.green : Color.white);
            angle += da;
        }
    }

    void reproduce()
    {
        GameObject child = Instantiate(gameObject);
        child.transform.position += Vector3.up * 2 + Vector3.right * 4 * (Random.value - 0.5F);
        child.SendMessage("setParent", this);
    }

    protected override void onDeath()
    {
        population--;
        if (population == 0)
            Common.restartCurrentScene();
        DestroyImmediate(gameObject);
    }

    public void setParent(NeuralCharacter parent)
    {
        this.parent = parent;
    }

    public void eat()
    {
        this.energy += 20;
        reproduce();
        reproduce();
        reproduce();
    }

    public void onCanvas()
    {
        GLCanvas.rect(body.position.x - energy * 0.1F, body.position.y + 1.1F, energy * 0.2F, 0.2F, Color.yellow, 1);
    }
}
