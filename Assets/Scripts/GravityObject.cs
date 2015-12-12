using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityObject : MonoBehaviour {

	protected void Start()
	{
		transform.parent.GetComponent<SolarSystem>().registerConstantBody(GetComponent<Rigidbody2D>());
	}
}
