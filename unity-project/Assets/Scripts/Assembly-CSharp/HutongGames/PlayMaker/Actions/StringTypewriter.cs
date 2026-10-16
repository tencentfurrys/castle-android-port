using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Automatically types into a string.")]
	[ActionCategory(ActionCategory.String)]
	public class StringTypewriter : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.TextArea)]
		[Tooltip("The string with the entire message to type out.")]
		public FsmString baseString;

		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The target result string (can be the same as the base string).")]
		public FsmString resultString;

		[Tooltip("The time between letters appearing.")]
		public FsmFloat pause;

		[Tooltip("When punctuation is encountered then pause is multiplied by this.\n(period, exclamation, question, comma, semicolon, colon and ellipsis).\nIt also handles repeating characters and pauses only one time at their end.")]
		public FsmFloat punctuationMultiplier;

		public FsmFloat Now_Pause_Time;

		[Tooltip("True is realtime: continues typing while game is paused. False will subject time variable to the game's timeScale.")]
		public FsmBool realtime;

		public FsmBool Pause_Skip;

		public FsmBool Done;

		[Tooltip("Support <color><b><i><size> use. \n LIMITATION: Cannot stack formats together yet! eg <b><i>Text</i></b> won't work. \n Pause: <p=0.9> for a 0.9 second pause (mid sentance pause). \n Speed: <s=1.5> changes Pause to 1.5 (time between characters).")]
		public bool richText;

		[Tooltip("Send this event when finished typing.")]
		public FsmEvent finishEvent;

		[UIHint(UIHint.Description)]
		public string d1 = "     Optional Sounds Section:";

		[Tooltip("Check this to play sounds while typing.")]
		public bool useSounds;

		[Tooltip("Check this to not play a sound when it is a spacebar character.")]
		public bool noSoundOnSpaces;

		[ObjectType(typeof(AudioClip))]
		[Tooltip("The sound to play for each letter typed.")]
		public FsmObject typingSound;

		[Tooltip("The GameObject with an AudioSource component.")]
		public FsmOwnerDefault audioSourceObj;

		private float p;

		private float startTime;

		private float timer;

		private char[] punctuation = new char[6] { '.', '!', '?', ',', ';', ':' };

		private string message = string.Empty;

		private int index;

		private char lastChar;

		private char nextChar;

		private string block;

		private string suffix;

		private bool fBold;

		private bool fItal;

		private bool fSize;

		private bool fColor;

		private float forcedPause;

		private AudioSource audioSource;

		private AudioClip sound;

		public override void Reset()
		{
			baseString = null;
			resultString = null;
			pause = 0.05f;
			punctuationMultiplier = 10f;
			realtime = false;
			richText = true;
			finishEvent = null;
			Pause_Skip = false;
			useSounds = false;
			noSoundOnSpaces = true;
			typingSound = null;
			audioSourceObj = null;
		}

		public override void OnEnter()
		{
			if (useSounds)
			{
				GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(audioSourceObj);
				if (ownerDefaultTarget != null)
				{
					audioSource = ownerDefaultTarget.GetComponent<AudioSource>();
					if (audioSource == null)
					{
						Debug.LogError("String Typewriter Action reports: The <color=#ffa500ff>AudioSource component</color> was not found! Does the target object have an Audio Source component?");
						useSounds = false;
					}
					sound = typingSound.Value as AudioClip;
					if (sound == null)
					{
						Debug.LogError("String Typewriter Action reports: The <color=#ffa500ff>AudioClip</color> was not found!");
						useSounds = false;
					}
				}
				else
				{
					Debug.LogError("String Typewriter Action reports: The <color=#ffa500ff>target Game Object</color> for the audio source was not found!");
					useSounds = false;
				}
			}
			fItal = false;
			fSize = false;
			fColor = false;
			fBold = false;
			block = string.Empty;
			suffix = string.Empty;
			index = 0;
			message = baseString.Value;
			resultString.Value = string.Empty;
			startTime = Time.realtimeSinceStartup;
		}

		public override void OnUpdate()
		{
			Done.Value = false;
			if (message == resultString.Value)
			{
				DoFinish();
				return;
			}
			p = pause.Value;
			nextChar = message[index];
			int num = Array.IndexOf(punctuation, lastChar);
			int num2 = Array.IndexOf(punctuation, nextChar);
			bool flag = num != -1;
			bool flag2 = num2 != -1;
			if (flag && !flag2)
			{
				pause.Value = p * punctuationMultiplier.Value;
			}
			if (richText && message[index] == '<')
			{
				DoRichText();
			}
			if (Pause_Skip.Value)
			{
				forcedPause = 0f;
				pause.Value = 0f;
				Pause_Skip.Value = false;
			}
			Now_Pause_Time.Value = forcedPause;
			if (realtime.Value && Time.realtimeSinceStartup - startTime >= ((forcedPause == 0f) ? pause.Value : forcedPause))
			{
				DoTyping();
			}
			if (!realtime.Value)
			{
				timer += Time.deltaTime;
				if (timer >= ((forcedPause == 0f) ? pause.Value : forcedPause))
				{
					DoTyping();
				}
			}
			pause.Value = p;
		}

		public void DoTyping()
		{
			forcedPause = 0f;
			if (useSounds)
			{
				if (noSoundOnSpaces && message[index] != ' ')
				{
					audioSource.PlayOneShot(sound);
				}
				else
				{
					audioSource.PlayOneShot(sound);
				}
			}
			if (richText)
			{
				suffix = ((!fBold) ? string.Empty : "</b>") + ((!fItal) ? string.Empty : "</i>") + ((!fSize) ? string.Empty : "</size>") + ((!fColor) ? string.Empty : "</color>");
				resultString.Value = message.Substring(0, index) + message[index] + suffix;
			}
			if (!richText)
			{
				resultString.Value += message[index];
			}
			lastChar = message[index];
			index++;
			timer = 0f;
			startTime = Time.realtimeSinceStartup;
		}

		public void DoRichText()
		{
			forcedPause = 0f;
			block = string.Empty;
			int num = index;
			while (index < message.Length)
			{
				block += message[index];
				index++;
				if (message[index] == '>')
				{
					block += message[index];
					index++;
					break;
				}
			}
			block = block.ToLower();
			if (block.Contains("/"))
			{
				if (block.Contains("</c"))
				{
					fColor = false;
					return;
				}
				if (block.Contains("</s"))
				{
					fSize = false;
					return;
				}
				if (block.Contains("</i"))
				{
					fItal = false;
					return;
				}
				if (block.Contains("</b"))
				{
					fBold = false;
					return;
				}
			}
			if (block.Contains("/"))
			{
				return;
			}
			if (block.Contains("<color"))
			{
				fColor = true;
				return;
			}
			if (block.Contains("<size"))
			{
				fSize = true;
				return;
			}
			if (block.Contains("<i"))
			{
				fItal = true;
				return;
			}
			if (block.Contains("<b"))
			{
				fBold = true;
				return;
			}
			if (block.Contains("<s="))
			{
				int num2 = 3;
				string text = string.Empty;
				while (num2 < block.Length)
				{
					text += block[num2];
					num2++;
					if (block[num2] == '>')
					{
						p = float.Parse(text);
						pause.Value = p;
						string text2 = message.Substring(0, num);
						string text3 = message.Substring(num + block.Length, message.Length - text2.Length - block.Length);
						index = num;
						message = text2 + text3;
						break;
					}
				}
			}
			if (!block.Contains("<p="))
			{
				return;
			}
			int num3 = 3;
			string text4 = string.Empty;
			while (num3 < block.Length)
			{
				text4 += block[num3];
				num3++;
				if (block[num3] == '>')
				{
					float num4 = float.Parse(text4);
					forcedPause += num4;
					string text5 = message.Substring(0, num);
					string text6 = message.Substring(num + block.Length, message.Length - text5.Length - block.Length);
					index = num;
					message = text5 + text6;
					break;
				}
			}
		}

		public void DoFinish()
		{
			Finish();
			Done.Value = true;
			if (finishEvent != null)
			{
				base.Fsm.Event(finishEvent);
			}
		}

		public override void OnExit()
		{
			resultString.Value = message;
		}
	}
}
