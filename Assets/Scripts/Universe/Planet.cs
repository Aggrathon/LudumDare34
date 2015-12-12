using UnityEngine;
using System.Collections;

public class Planet : GravityObject
{

	public enum ProductionType
	{
		Population,
		Food,
		Products
	}

	public static float PRODUCTION_INTERVAL = 1.5f;

	[Header("Planet")]
	public bool antiClockWiseOrbit = false;

	[Header("Production")]
	public ProductionType productionType;
	public int productionAmount = 5;
	public float productionCapMultiplier = 2.0f;

	[Header("Current")]
	public int population = 0;
	public int food = 0;
	public int products = 0;

	[Header("Demand")]
	public int populationDemand = 100;
	public int foodDemand = 100;
	public int productDemand = 100;
	public float demandScaling = 1.5f;

	public void Start()
	{
		SolarSystem solarSystem = transform.parent.GetComponent<SolarSystem>();
		Vector2 center = solarSystem.centerOfMass;
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

		bool rot = rigidbody.position.y < center.y;
		if (antiClockWiseOrbit) rot = !rot;
        rigidbody.velocity = PhysicsFunctions.OrbitalSpeed(center - rigidbody.position, solarSystem.totalMass, rot);

		StartCoroutine(Produce());
	}

	IEnumerator Produce()
	{
		WaitForSeconds wait = new WaitForSeconds(PRODUCTION_INTERVAL);
        while (true)
		{
			switch(productionType)
			{
				case ProductionType.Food:
					food += productionAmount;
					break;
				case ProductionType.Population:
					population += productionAmount;
					break;
				case ProductionType.Products:
					products += productionAmount;
					break;
			}
			yield return wait;
		}
	}
}
