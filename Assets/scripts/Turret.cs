using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {
    public float angle;
    public GameObject target;
    public float shotInterval;
    private float time;
    //Start is called once
    void Start()
    {
        shotInterval = 2;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 myPosition = this.transform.position;
        Vector3 targetPosition = target.transform.position;
        Vector3 direction = (targetPosition - myPosition);
        angle = 180 - Vector3.Angle(new Vector3(1, 0, 0), direction);
        if (targetPosition.y > myPosition.y)
            angle *= -1;

        this.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
    }
}
