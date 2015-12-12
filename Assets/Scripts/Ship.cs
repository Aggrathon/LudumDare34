using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{

	public enum MoveState
	{
		nothing,
		left,
		right,
		forward
	}

	private MoveState movement = MoveState.nothing;
	private float lastDouble = 0f;
	private bool noButton = true;

	new private Rigidbody2D rigidbody;

	[Header("Movement")]
	public float acceleration = 300f;
	public float turnRate = 300f;

	[Header("Abilities")]
	public float doubleClickTime = 0.4f;
	public float missileCooldown = 5.0f;
	public float missileSpeed = 30f;
	public float shieldCooldown = 5.0f;
	public float shieldDuration = 2.0f;

	private float missileTime = 0f;
	private float shieldTime = 0f;

	[Header("Stats")]
	public int maxCargo = 100;
	public int maxHealth = 3;
	public int currentHealth = 3;
	public int currentPopulation = 0;
	public int currentFood = 3;
	public int currentProducts = 3;

	public int currentCargo { get { return currentPopulation + currentFood + currentProducts; } }

	[Header("GameObjects")]
	public ShipUI shipStats;
	public SolarSystem solarSystem;
	public Shield shield;
	public Trade trade;

	[Header("Targetting")]
	public float targetLength = 800f;
	public float targetAngle = 60f;
	public Transform target;
	public Transform targetMarker;

	private float raycastStart;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		if (shield == null)
			shield = GetComponentInChildren<Shield>();
	}

	void Start()
	{
		SwitchSystem(solarSystem);
		if (target == null)
			targetMarker.gameObject.SetActive(false);

		shipStats.setCargo((float)currentCargo / (float)maxCargo);

		raycastStart = GetComponent<BoxCollider2D>().size.y * 0.5f * transform.lossyScale.y + 0.5f;
	}

	void Update()
	{
		if (missileTime > 0f)
		{
			missileTime -= Time.deltaTime;
			shipStats.setMissile(missileTime < 0f ? 1f : 1f - missileTime / missileCooldown);
		}
		if (shieldTime > 0f)
		{
			shieldTime -= Time.deltaTime;
			shipStats.setShield(shieldTime < 0f ? 1f : 1f - shieldTime / shieldCooldown);
		}

		bool left = Input.GetButton("Left");
		bool right = Input.GetButton("Right");
		if (left && right)
		{
			if (noButton)
			{
				if (Time.time - lastDouble < doubleClickTime)
				{
					if (target != null) //target
					{
						if (missileTime <= 0)
						{
							missileTime = missileCooldown;
							shipStats.setMissile(0f);
							//Deploy Missile
						}
					}
					else
					{
						if (shieldTime <= 0)
						{
							shieldTime = shieldCooldown;
							shipStats.setShield(0f);
							shield.EnableShield(shieldDuration);
						}
					}
					lastDouble = 0f;
				}
				else
				{
					noButton = false;
					lastDouble = Time.time;
				}
			}
			movement = MoveState.forward;
		}
		else if (left)
		{
			movement = MoveState.left;
		}
		else if (right)
		{
			movement = MoveState.right;
		}
		else
		{
			noButton = true;
		}

		RaycastHit2D ray = Physics2D.Raycast(transform.position + transform.up * raycastStart, transform.up, targetLength);
		if (ray.collider != null && ray.collider.tag == "Ship")
		{
			target = ray.collider.transform;
			targetMarker.gameObject.SetActive(true);
			targetMarker.position = target.position;
		}
		else if (target != null)
		{
			float angle = Vector2.Angle(transform.forward, target.transform.position - transform.position);
			if (angle < targetAngle && angle > -targetAngle)
			{
				targetMarker.position = target.position;
			}
			else
			{
				target = null;
				targetMarker.gameObject.SetActive(false);
			}
		}
	}

	void FixedUpdate()
	{
		switch (movement)
		{
			case MoveState.forward:
				rigidbody.AddRelativeForce(new Vector2(0f, Time.fixedDeltaTime * acceleration));
				break;
			case MoveState.left:
				rigidbody.AddTorque(Time.fixedDeltaTime * turnRate);
				break;
			case MoveState.right:
				rigidbody.AddTorque(-Time.fixedDeltaTime * turnRate);
				break;
		}
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Missile" || collision.gameObject.tag == "Ship")
		{
			collision.gameObject.SetActive(false);
			currentHealth--;
			if (currentHealth == 0)
			{
				//LOSE
			}
			shipStats.setHealth((float)currentHealth/(float)maxHealth);
		}
		else if (collision.gameObject.tag == "Sun")
		{
			//Lose
		}
		else
		{
			Planet planet = collision.gameObject.GetComponent<Planet>();
			if(planet != null)
				trade.OpenTrade(this, planet);
		}
	}

	void SwitchSystem(SolarSystem ss)
	{
		if (solarSystem != null)
			solarSystem.unregisterTempBody(rigidbody);
		ss.registerTempBody(rigidbody);
		solarSystem = ss;
		rigidbody.velocity = PhysicsFunctions.OrbitalSpeed(
			solarSystem.centerOfMass - rigidbody.position,
			solarSystem.totalMass, false);
	}
}
