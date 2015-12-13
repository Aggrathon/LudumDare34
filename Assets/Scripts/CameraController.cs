using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	new private Camera camera;
	private Quaternion origRotation;

	public float zoomMultiplier = 10f;
	public bool rotateCamera = false;
	public Transform player;

	void Awake()
	{
		camera = GetComponent<Camera>();
		origRotation = transform.rotation;
		if (player == null)
			player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update () {
		camera.orthographicSize *= 1-(Input.GetAxis("Scroll") * Time.deltaTime * zoomMultiplier);
		if(rotateCamera)
		{
			transform.rotation = player.rotation;
		}
		transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
	}

	public void SetCameraRotating(bool value)
	{
		if (value)
			transform.rotation = player.rotation;
		else
			transform.rotation = origRotation;
		rotateCamera = value;
	}
}
