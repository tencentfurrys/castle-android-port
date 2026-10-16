using UnityEngine;

public class gravityController : MonoBehaviour
{
	public Vector3 localGravity;

	private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
	}

	private void FixedUpdate()
	{
		setLocalGravity();
	}

	private void setLocalGravity()
	{
		rb.AddForce(localGravity, ForceMode.Acceleration);
	}
}
