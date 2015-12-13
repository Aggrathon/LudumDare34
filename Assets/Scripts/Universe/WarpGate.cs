using UnityEngine;

public class WarpGate : MonoBehaviour
{
	private bool tempDisable = false;

	public WarpGate target;
	public Warper warper;
	public bool antiClockWiseOrbit = false;


	public void OnTriggerEnter2D(Collider2D collision)
	{
		if(tempDisable)
		{
			tempDisable = false;
		}
		else if (collision.gameObject.tag == "Player")
		{
			warper.Warp(collision.gameObject.GetComponent<Ship>(), target, transform.position);
		}
	}

	public void WarpComplete()
	{
		tempDisable = true;
	}

	public void PrepareEnemies()
	{

	}

	public void OnDrawGizmos()
	{
		if(target != null)
		{
			Gizmos.color = new Color(0.6f, 0.7f, 1f, 0.5f);
			Gizmos.DrawLine(transform.position, target.transform.position);
		}
	}
}
