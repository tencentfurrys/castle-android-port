using UnityEngine;

public class Force_Fixedupdate : MonoBehaviour
{
	public Rigidbody rb;

	public PlayMakerFSM fsm;

	public float power;

	private float horizontalMove;

	private void Start()
	{
	}

	private void Update()
	{
		horizontalMove = (Input.GetAxisRaw("Horizontal") + CastleTouch.Axis("Horizontal")) * power;
	}

	private void FixedUpdate()
	{
		rb.velocity = Vector3.right * horizontalMove;
	}
}
