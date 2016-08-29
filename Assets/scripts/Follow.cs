using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

    public Transform target;
    public float rate = 0.8F;

	// Use this for initialization
	void Start () {
        Vector3 tpos = target.position;
        Vector3 pos = transform.position;
        transform.Translate(new Vector3(tpos.x - pos.x, tpos.y - pos.y,0));
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tpos = target.position;
        Vector3 pos = transform.position;
        transform.Translate(new Vector3(tpos.x - pos.x, tpos.y - pos.y, 0) * rate);
    }
}
