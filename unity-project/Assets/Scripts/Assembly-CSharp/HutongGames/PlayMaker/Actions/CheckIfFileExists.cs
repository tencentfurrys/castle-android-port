using System.IO;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Files")]
	[Tooltip("Check if a File exists")]
	public class CheckIfFileExists : FsmStateAction
	{
		[Tooltip("The file path")]
		[RequiredField]
		public FsmString filePath;

		[Tooltip("True if the file exists, false if it wasn't found")]
		[UIHint(UIHint.Variable)]
		[ActionSection("Result")]
		public FsmBool fileExists;

		public FsmEvent FileExistsEvent;

		public FsmEvent FileNotFoundEvent;

		public override void Reset()
		{
			filePath = null;
			fileExists = null;
			FileExistsEvent = null;
			FileNotFoundEvent = null;
		}

		public override void OnEnter()
		{
			if (DoesFileExists())
			{
				base.Fsm.Event(FileExistsEvent);
			}
			else
			{
				base.Fsm.Event(FileNotFoundEvent);
			}
			Finish();
		}

		private bool DoesFileExists()
		{
			bool flag = File.Exists(filePath.Value);
			fileExists.Value = flag;
			return flag;
		}
	}
}
