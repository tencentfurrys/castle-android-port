using System.Collections.Generic;
using System.Reflection;
using HutongGames.PlayMaker;
using UnityEngine;

public class PlayMakerAnimatorStateSynchronization : MonoBehaviour
{
	public int LayerIndex;

	public PlayMakerFSM Fsm;

	public bool EveryFrame = true;

	public bool debug;

	private Animator animator;

	private int lastState;

	private int lastTransition;

	private Dictionary<int, FsmState> fsmStateLUT;

	private void Start()
	{
		animator = GetComponent<Animator>();
		if (!(Fsm != null))
		{
			return;
		}
		string layerName = animator.GetLayerName(LayerIndex);
		fsmStateLUT = new Dictionary<int, FsmState>();
		FsmState[] states = Fsm.Fsm.States;
		foreach (FsmState fsmState in states)
		{
			string text = fsmState.Name;
			RegisterHash(fsmState.Name, fsmState);
			if (!text.StartsWith(layerName + "."))
			{
				RegisterHash(layerName + "." + fsmState.Name, fsmState);
			}
		}
	}

	private void RegisterHash(string key, FsmState state)
	{
		int num = Animator.StringToHash(key);
		fsmStateLUT.Add(num, state);
		if (debug)
		{
			Debug.Log("registered " + key + " ->" + num);
		}
	}

	private void Update()
	{
		if (EveryFrame)
		{
			Synchronize();
		}
	}

	public void Synchronize()
	{
		if (animator == null || Fsm == null)
		{
			return;
		}
		bool flag = false;
		if (animator.IsInTransition(LayerIndex))
		{
			int nameHash = animator.GetAnimatorTransitionInfo(LayerIndex).nameHash;
			int userNameHash = animator.GetAnimatorTransitionInfo(LayerIndex).userNameHash;
			if (lastTransition != nameHash)
			{
				if (debug)
				{
					Debug.Log("is in transition");
				}
				if (fsmStateLUT.ContainsKey(userNameHash))
				{
					FsmState fsmState = fsmStateLUT[userNameHash];
					if (Fsm.Fsm.ActiveState != fsmState)
					{
						SwitchState(Fsm.Fsm, fsmState);
						flag = true;
					}
				}
				if (!flag && fsmStateLUT.ContainsKey(nameHash))
				{
					FsmState fsmState2 = fsmStateLUT[nameHash];
					if (Fsm.Fsm.ActiveState != fsmState2)
					{
						SwitchState(Fsm.Fsm, fsmState2);
						flag = true;
					}
				}
				if (!flag && debug)
				{
					Debug.LogWarning("Fsm is missing animator transition name or username for hash:" + nameHash);
				}
				lastTransition = nameHash;
			}
		}
		if (flag)
		{
			return;
		}
		int fullPathHash = animator.GetCurrentAnimatorStateInfo(LayerIndex).fullPathHash;
		if (lastState == fullPathHash)
		{
			return;
		}
		if (debug)
		{
			Debug.Log("Net state switch");
		}
		if (fsmStateLUT.ContainsKey(fullPathHash))
		{
			FsmState fsmState3 = fsmStateLUT[fullPathHash];
			if (Fsm.Fsm.ActiveState != fsmState3)
			{
				SwitchState(Fsm.Fsm, fsmState3);
			}
		}
		else if (debug)
		{
			Debug.LogWarning("Fsm is missing animator state hash:" + fullPathHash);
		}
		lastState = fullPathHash;
	}

	private void SwitchState(Fsm fsm, FsmState state)
	{
		fsm.GetType().GetMethod("SwitchState", BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(fsm, new object[1] { state });
	}
}
