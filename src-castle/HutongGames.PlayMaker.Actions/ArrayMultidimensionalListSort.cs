using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[ActionCategory("ArrayMaker/ArrayList")]
[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=11918.0")]
[Tooltip("Multidimensional sorts (2d sort) of two array of same length from the sequence of elements in a PlayMaker ArrayList Proxy component. Includes a shuffle option")]
public class ArrayMultidimensionalListSort : ArrayListActions
{
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	[ActionSection("Set up")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
	public FsmString keyArrayName;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
	public FsmString itemsArrayName;

	[ActionSection("Sort Option")]
	[Tooltip("Shuffle and can be used with 'useRange' to optional start Index for the shuffling.")]
	public FsmBool shuffle;

	public FsmBool useRange;

	[Tooltip("The starting index of the range to sort.")]
	public FsmInt startIndex;

	[Title("Range")]
	[Tooltip("The number of elements in the range to sort (the ending index of the range to sort).")]
	public FsmInt length;

	[Tooltip("When On - sort by item and not by key")]
	[Title("Sort by item")]
	public FsmBool flipOn;

	[UIHint(UIHint.FsmEvent)]
	[ActionSection("Event")]
	public FsmEvent doneEvent;

	[Tooltip("The event if error")]
	[UIHint(UIHint.FsmEvent)]
	public FsmEvent failureEvent;

	private int l1;

	private int l2;

	private int lengthTemp;

	private object element;

	public override void Reset()
	{
		gameObject = null;
		keyArrayName = null;
		itemsArrayName = null;
		startIndex = 0;
		length = 0;
		useRange = false;
		flipOn = false;
		failureEvent = null;
		shuffle = false;
		doneEvent = null;
	}

	public override void OnEnter()
	{
		if (shuffle.Value)
		{
			DoArrayListShuffle();
			Fsm.Event(doneEvent);
			Finish();
		}
		else
		{
			DoArrayListSort();
		}
		Fsm.Event(doneEvent);
		Finish();
	}

	public void DoArrayListSort()
	{
		setProxyKey();
		if (!isProxyValid())
		{
			Fsm.Event(failureEvent);
			return;
		}
		l1 = proxy.arrayList.Count;
		if (useRange.Value)
		{
			int num = Mathf.Min(proxy.arrayList.Count - 1, startIndex.Value + length.Value);
			if (num > l1)
			{
			}
			Debug.LogWarning("<color=#6B8E23ff>Your range is above array max length. Please review!</color>", Owner);
			Fsm.Event(failureEvent);
			return;
		}
		object[] array = new object[l1];
		for (int i = 0; i < l1; i++)
		{
			element = null;
			try
			{
				element = proxy.arrayList[i];
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
				string fullFsmLabel = Fsm.GetFullFsmLabel(Fsm);
				string activeStateName = Fsm.ActiveStateName;
				Debug.Log("Fsm Path= " + fullFsmLabel + " : " + activeStateName);
				Fsm.Event(failureEvent);
				return;
			}
			array[i] = element;
		}
		setProxyItem();
		if (!isProxyValid())
		{
			Fsm.Event(failureEvent);
			return;
		}
		l2 = proxy.arrayList.Count;
		object[] array2 = new object[l2];
		if (l1 != l2)
		{
			Debug.LogWarning("<color=#6B8E23ff>The arrays are NOT equal length. They must be the same length. Please review!</color>", Owner);
			Fsm.Event(failureEvent);
			Finish();
		}
		for (int j = 0; j < l2; j++)
		{
			element = null;
			try
			{
				element = proxy.arrayList[j];
			}
			catch (Exception ex2)
			{
				Debug.Log(ex2.Message);
				string fullFsmLabel2 = Fsm.GetFullFsmLabel(Fsm);
				string activeStateName2 = Fsm.ActiveStateName;
				Debug.Log("Fsm Path= " + fullFsmLabel2 + " : " + activeStateName2);
				Fsm.Event(failureEvent);
				return;
			}
			array2[j] = element;
		}
		if (!flipOn.Value & !useRange.Value)
		{
			Array.Sort(array, array2);
		}
		if (flipOn.Value & !useRange.Value)
		{
			Array.Sort(array2, array);
		}
		if (!flipOn.Value & useRange.Value)
		{
			Array.Sort(array, array2, startIndex.Value, length.Value);
		}
		if (flipOn.Value & useRange.Value)
		{
			Array.Sort(array2, array, startIndex.Value, length.Value);
		}
		lengthTemp = l1;
		setProxyKey();
		proxy.arrayList.Clear();
		for (int k = 0; k < lengthTemp; k++)
		{
			proxy.arrayList.Add(array[k]);
		}
		lengthTemp = l2;
		setProxyItem();
		proxy.arrayList.Clear();
		for (int l = 0; l < lengthTemp; l++)
		{
			proxy.arrayList.Add(array2[l]);
		}
	}

	public void setProxyKey()
	{
		SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), keyArrayName.Value);
	}

	public void setProxyItem()
	{
		SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), itemsArrayName.Value);
	}

	public void DoArrayListShuffle()
	{
		setProxyItem();
		int num = 0;
		int num2 = proxy.arrayList.Count - 1;
		if (startIndex.Value > 0)
		{
			num = Mathf.Min(startIndex.Value, num2);
		}
		if (length.Value > 0)
		{
			num2 = Mathf.Min(proxy.arrayList.Count - 1, num + length.Value);
		}
		for (int num3 = num2; num3 > num; num3--)
		{
			setProxyItem();
			int index = UnityEngine.Random.Range(num, num3 + 1);
			object value = proxy.arrayList[num3];
			proxy.arrayList[num3] = proxy.arrayList[index];
			proxy.arrayList[index] = value;
			setProxyKey();
			value = proxy.arrayList[num3];
			proxy.arrayList[num3] = proxy.arrayList[index];
			proxy.arrayList[index] = value;
		}
		Fsm.Event(doneEvent);
	}
}
