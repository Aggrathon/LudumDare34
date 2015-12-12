using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

	new private Camera camera;
	private Quaternion rotation;

	public float zoomMultiplier = 10f;
	public bool doNotRotate = false;

	void Awake()
	{
		camera = GetComponent<Camera>();
		rotation = transform.rotation;
	}

	void Update () {
		camera.orthographicSize *= 1-(Input.GetAxis("Scroll") * Time.deltaTime * zoomMultiplier);
		if(doNotRotate)
		{
			transform.rotation = rotation;
		}
	}

	public void SetCameraRotating(bool value)
	{
		if (value)
			transform.rotation = transform.parent.rotation;
		doNotRotate = !value;
	}
}
