using System;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions;

[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=12044.0")]
[Tooltip("Return the index of an item from a PlayMaker Array List Proxy component. Can search within a range")]
[ActionCategory("ArrayMaker/ArrayList")]
public class ArrayListIndexOfByString : ArrayListActions
{
	[ActionSection("Set up")]
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	public FsmOwnerDefault gameObject;

	[UIHint(UIHint.FsmString)]
	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
	public FsmString reference;

	[UIHint(UIHint.FsmInt)]
	[Tooltip("Optional start index to search from: set to 0 to ignore")]
	public FsmInt startIndex;

	[Tooltip("Optional amount of elements to search within: set to 0 to ignore")]
	[UIHint(UIHint.FsmInt)]
	public FsmInt count;

	[RequiredField]
	[Tooltip("The variable to get the index of.")]
	[ActionSection("Data")]
	public FsmString name;

	[ActionSection("Result")]
	[Tooltip("The index of the item described below")]
	[UIHint(UIHint.Variable)]
	public FsmInt indexOf;

	[UIHint(UIHint.FsmEvent)]
	[Tooltip("Optional Event sent if this arrayList contains that element ( described below)")]
	public FsmEvent itemFound;

	[UIHint(UIHint.FsmEvent)]
	[Tooltip("Optional Event sent if this arrayList does not contains that element ( described below)")]
	public FsmEvent itemNotFound;

	[Tooltip("Optional Event to trigger if the action fails ( likely an out of range exception when using wrong values for index and/or count)")]
	[UIHint(UIHint.FsmEvent)]
	public FsmEvent failureEvent;

	private char[] m_chars;

	private int m_charsCount;

	private int m_charsCapacity;

	private List<char> m_replacement;

	private bool m_isStringGenerated;

	private string m_stringGenerated = string.Empty;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		startIndex = null;
		count = null;
		itemFound = null;
		itemNotFound = null;
		name = null;
	}

	public override void OnEnter()
	{
		if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			DoArrayListIndexOf();
		}
		Finish();
	}

	public void DoArrayListIndexOf()
	{
		if (!isProxyValid())
		{
			return;
		}
		int num = 0;
		int num2 = -1;
		int num3 = proxy.arrayList.Count;
		m_chars = new char[m_charsCapacity = 32];
		for (num = startIndex.Value; num < num3; num++)
		{
			object obj = null;
			try
			{
				obj = proxy.arrayList[num];
			}
			catch (Exception)
			{
				Debug.LogWarning("<b>[ArrayListIndexOfByString]</b><color=#FF9900ff>  Please review!</color>", Owner);
				Fsm.Event(failureEvent);
				return;
			}
			Clear();
			string text = obj.ToString();
			text = text.Remove(text.Length - 25);
			if (name.Value == text)
			{
				indexOf.Value = num;
				Fsm.Event(itemFound);
				Finish();
			}
		}
		if (num == num3 - 1)
		{
			indexOf.Value = -1;
			Fsm.Event(itemNotFound);
			Finish();
		}
	}

	public void Clear()
	{
		m_charsCount = 0;
		m_isStringGenerated = false;
	}

	public bool IsEmpty()
	{
		return (!m_isStringGenerated) ? (m_charsCount == 0) : (m_stringGenerated == null);
	}

	public override string ToString()
	{
		if (!m_isStringGenerated)
		{
			m_stringGenerated = new string(m_chars, 0, m_charsCount);
			m_isStringGenerated = true;
		}
		return m_stringGenerated;
	}

	private void ReallocateIFN(int nbCharsToAdd)
	{
		if (m_charsCount + nbCharsToAdd > m_charsCapacity)
		{
			m_charsCapacity = Math.Max(m_charsCapacity + nbCharsToAdd, m_charsCapacity * 2);
			char[] array = new char[m_charsCapacity];
			m_chars.CopyTo(array, 0);
			m_chars = array;
		}
	}
}
