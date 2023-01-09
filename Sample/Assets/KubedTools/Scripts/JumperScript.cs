using UnityEngine;
using System.Collections;

public class JumperScript : MonoBehaviour {

	public Vector3 dir;

    [Range(1,100)]
    public float force;

	void OnTriggerEnter(Collider other)
	{
		PlayerScript ps = other.gameObject.transform.root.gameObject.GetComponent<PlayerScript>();

		ps.JumperPush( (ps.transform.TransformVector(Vector3.forward)), force );
	}	

	protected int power = 10;
}
