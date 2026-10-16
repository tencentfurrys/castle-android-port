using HutongGames.PlayMaker;
using UnityEngine;

public class ParticleCollisionPM : MonoBehaviour
{
	public ParticleSystem particleSystem;

	public ParticleCollisionEvent[] collisionEvents;

	private PlayMakerFSM playMakerFsm;

	public FsmVector3 setVector3Data;

	private void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();
		collisionEvents = new ParticleCollisionEvent[16];
		playMakerFsm = base.gameObject.GetComponent<PlayMakerFSM>();
	}

	private void OnParticleCollision(GameObject other)
	{
		int safeCollisionEventSize = particleSystem.GetSafeCollisionEventSize();
		if (collisionEvents.Length < safeCollisionEventSize)
		{
			collisionEvents = new ParticleCollisionEvent[safeCollisionEventSize];
		}
		int num = particleSystem.GetCollisionEvents(other, collisionEvents);
		for (int i = 0; i < num; i++)
		{
			Vector3 intersection = collisionEvents[i].intersection;
			Fsm.EventData.Vector3Data = intersection;
			playMakerFsm.SendEvent("Hit");
		}
	}
}
