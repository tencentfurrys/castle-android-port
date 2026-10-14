using System;

namespace HutongGames.PlayMaker.Actions;

[Tooltip("Loads into a PlayMaker Array List Proxy component the tags of an EasySave file")]
[ActionCategory("Easy Save 2")]
public class ArrayListEasyGetTags : ArrayListActions
{
	[RequiredField]
	[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
	[CheckForComponent(typeof(PlayMakerArrayListProxy))]
	[ActionSection("Set up")]
	public FsmOwnerDefault gameObject;

	[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
	[UIHint(UIHint.FsmString)]
	public FsmString reference;

	[RequiredField]
	[Tooltip("The name or absolute path of the file where our data will be stored. If the file doesn't exist, it will be created.")]
	[ActionSection("Easy Save Set Up")]
	public FsmString filename = "defaultFile.txt";

	[Tooltip("Whether the data is encrypted or not. If set to true, you must set an encryption password.")]
	public FsmBool encrypt = ES2GlobalSettings.defaultEncrypt;

	[Tooltip("The password to use for encryption if it is enabled.")]
	public FsmString encryptionPassword = ES2GlobalSettings.defaultEncryptionPassword;

	[ActionSection("Result")]
	[Tooltip("The number of tags found on that file.")]
	[UIHint(UIHint.Variable)]
	public FsmInt tagCount;

	[Tooltip("The event to send if accessing tag was successful.")]
	public FsmEvent isDoneEvent;

	[Tooltip("The event to send if no tag were found on the file.")]
	public FsmEvent noTagsEvent;

	[Tooltip("The event to send if there was an error.")]
	public FsmEvent isErrorEvent;

	[UIHint(UIHint.Variable)]
	[Tooltip("Where any errors thrown will be stored. Set this to a variable, or leave it blank.")]
	public FsmString errorMessage = string.Empty;

	public override void Reset()
	{
		gameObject = null;
		reference = null;
		filename.Value = "defaultFile.txt";
		encrypt.Value = ES2GlobalSettings.defaultEncrypt;
		encryptionPassword.Value = ES2GlobalSettings.defaultEncryptionPassword;
		tagCount = null;
		isDoneEvent = null;
		noTagsEvent = null;
		isErrorEvent = null;
		errorMessage = new FsmString
		{
			UseVariable = true
		};
	}

	public override void OnEnter()
	{
		if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
		{
			GetTags();
		}
		Finish();
	}

	public void GetTags()
	{
		if (isProxyValid())
		{
			ES2Settings eS2Settings = new ES2Settings();
			eS2Settings.encrypt = encrypt.Value;
			eS2Settings.encryptionPassword = encryptionPassword.Value;
			proxy.arrayList.Clear();
			string[] array = new string[0];
			try
			{
				array = ES2.GetTags(filename.Value, eS2Settings);
			}
			catch (Exception ex)
			{
				errorMessage.Value = ex.Message;
				Fsm.Event(isErrorEvent);
			}
			string[] array2 = array;
			foreach (string value in array2)
			{
				proxy.arrayList.Add(value);
			}
			tagCount.Value = array.Length;
			if (array.Length == 0)
			{
				Fsm.Event(noTagsEvent);
			}
			Fsm.Event(isDoneEvent);
		}
	}
}
