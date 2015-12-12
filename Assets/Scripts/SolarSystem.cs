using UnityEngine;
using System.Collections.Generic;

public class SolarSystem : MonoBehaviour
{

	private Rigidbody2D[] bodies = new Rigidbody2D[] { };
	private List<Rigidbody2D> tempBodies = new List<Rigidbody2D>();

	void FixedUpdate () {
		for(int i = 0; i < bodies.Length; i++)
		{
			for (int j = i+1; j < bodies.Length; j++)
			{
				Vector2 force = PhysicsFunctions.GravityForce(bodies[i], bodies[j]);
				bodies[i].AddForce(-force);
				bodies[j].AddForce(force);
			}
			for (int k = 0; k < tempBodies.Count; k++)
			{
				Vector2 force = PhysicsFunctions.GravityForce(bodies[i], tempBodies[k]);
				tempBodies[k].AddForce(force);
			}
        }
	}

	#region data getters

	public Vector2 centerOfMass { get
		{
			Vector2 center = new Vector2(0f, 0f);
			float mass = totalMass;
			for (int i = 0; i < bodies.Length; i++)
			{
				center += (bodies[i].mass / mass) * bodies[i].position;
			}
			return center;
		} }

	public float totalMass { get; protected set; }

	#endregion

	#region registers
	public void registerConstantBody(Rigidbody2D rigid)
	{
		Rigidbody2D[] newbod = new Rigidbody2D[bodies.Length + 1];
		int i;
        for (i = 0; i<bodies.Length; i++)
		{
			newbod[i] = bodies[i];
		}
		newbod[i] = rigid;
		bodies = newbod;

		totalMass += rigid.mass;
	}

	public void registerTempBody(Rigidbody2D bod)
	{
		tempBodies.Add(bod);
	}

	public void unregisterTempBody(Rigidbody2D bod)
	{
		tempBodies.Remove(bod);
	}
	#endregion
}
