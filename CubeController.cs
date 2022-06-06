using UnityEngine;
using UnityEngine.Events;

public class CubeController : MonoBehaviour
{

	[Header("Settings")]
	public float maxZone = 50.0f;

	[Header("Other")]

	private Vector3 previousMousePosition;
	private Vector3 currentMousePosition;

	private Vector3 direction;
	private float deltaX;
	private float delta;

	public event UnityAction <CubeController> TheStartOfTheCubeIsMade;

	void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			previousMousePosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0))
		{
			currentMousePosition = Input.mousePosition;

			direction = -(previousMousePosition - currentMousePosition);

			delta = (direction.x * maxZone * 2f) / Screen.width * 1.5f;
			deltaX = (deltaX + delta >= maxZone) ? maxZone : (deltaX + delta <= - maxZone) ? -maxZone : deltaX + delta;

			previousMousePosition = currentMousePosition;
		}

		if(Input.GetMouseButtonUp(0))
        {
			transform.GetChild(0).gameObject.SetActive(true);
			GetComponent<Cube>().PushCube(Vector3.forward * 20f);
			CubeFired();
			Destroy(this);
        }

		transform.position = Vector3.Lerp(transform.position, new Vector3(deltaX, transform.position.y, transform.position.z), Time.deltaTime * 20f);
	}

	private void CubeFired()
    {
		TheStartOfTheCubeIsMade?.Invoke(this);
	}
}
