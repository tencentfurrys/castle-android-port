using System;
using UnityEngine;

public class LastIndexOfWord : MonoBehaviour
{
	public int LastIndexNum;

	public string stringVariable;

	public string containsString;

	public static void Main()
	{
		string text = "fox";
		string text2 = "dog";
		string text3 = string.Format("The {0} jumped over the {1}.", text, text2);
		Console.WriteLine("The original string is:{0}{1}{0}", Environment.NewLine, text3);
		Console.Write("Enter an adjective (or group of adjectives) to describe the {0}: ==> ", text);
		string text4 = Console.ReadLine();
		Console.Write("Enter an adjective (or group of adjectives) to describe the {0}: ==> ", text2);
		string text5 = Console.ReadLine();
		text4 = text4.Trim() + " ";
		text5 = text5.Trim() + " ";
		text3 = text3.Insert(text3.IndexOf(text), text4);
		text3 = text3.Insert(text3.IndexOf(text2), text5);
		Console.WriteLine("{0}The final string is:{0}{1}", Environment.NewLine, text3);
	}
}
