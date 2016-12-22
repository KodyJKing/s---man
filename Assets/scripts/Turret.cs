using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
    private int turnDirection = 1;
    public int turnSpeed;
    private float aim;
    private float baseX;
    public GameObject target;
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {//obtains targets x position and compares it to it's own
        aim = target.transform.position.x;
        baseX = transform.position.x;

        if (aim > baseX) 
            turnDirection = AbsVal(turnDirection);
        if(aim < baseX)
            turnDirection = AbsVal(turnDirection) * -1;

        //rotates around a certain axis and rotations speed is determined by second parameter
        transform.Rotate(Vector3.forward * turnSpeed * turnDirection);
    }
    //function that returns absolute value
    public int AbsVal(int value)
    {
        if (value > 0)
            return value;
        if (value < 0)
            return value * -1;
        else
            return 0;
    }
}
