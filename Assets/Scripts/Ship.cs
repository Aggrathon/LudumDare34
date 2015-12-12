using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	[SerializeField]
	private SolarSystem solarSystem;
	new private Rigidbody2D rigidbody;

	[Space(5)]
	public float acceleration = 15f;
	public float turnRate = 10f;
	
	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	void Start () {
		SwitchSystem(solarSystem);
	}
	
	void FixedUpdate () {
        rigidbody.AddRelativeForce(new Vector2(0f, Input.GetAxis("Vertical") * Time.fixedDeltaTime * acceleration));
		rigidbody.AddTorque(-Input.GetAxis("Horizontal") * Time.fixedDeltaTime * turnRate);
	}

	void SwitchSystem(SolarSystem ss)
	{
		if(solarSystem != null)
			solarSystem.unregisterTempBody(rigidbody);
		ss.registerTempBody(rigidbody);
		solarSystem = ss;
		rigidbody.velocity = PhysicsFunctions.OrbitalSpeed(
			solarSystem.centerOfMass - rigidbody.position,
			solarSystem.totalMass, false);
	}
}
