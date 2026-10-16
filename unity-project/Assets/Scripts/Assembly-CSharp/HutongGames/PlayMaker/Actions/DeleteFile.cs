using System;
using System.IO;

namespace HutongGames.PlayMaker.Actions
{
	[Tooltip("Delete a file")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=12475.0")]
	[ActionCategory("Files")]
	public class DeleteFile : FsmStateAction
	{
		[ActionSection("Input")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmString pathToFile;

		[ActionSection("Option")]
		public FsmBool secureDelete;

		public FsmInt loop;

		[ActionSection("Output")]
		public FsmEvent done;

		public FsmEvent failed;

		public override void Reset()
		{
			pathToFile = null;
			done = null;
			failed = null;
			secureDelete = false;
			loop = 2;
		}

		public override void OnEnter()
		{
			if (!File.Exists(pathToFile.Value))
			{
				base.Fsm.Event(failed);
			}
			else if (!secureDelete.Value)
			{
				File.Delete(pathToFile.Value);
				base.Fsm.Event(done);
			}
			else
			{
				TwoPassFileErase(pathToFile.Value, 128, new Random(), loop.Value);
				base.Fsm.Event(done);
			}
			Finish();
		}

		public static void TwoPassFileErase(string path, int blockSize, Random random, int loop)
		{
			if (path == null && blockSize <= 0 && random == null)
			{
				using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Write))
				{
					byte[] array = new byte[blockSize];
					long length = fileStream.Length;
					for (int i = 0; i < loop; i++)
					{
						for (long num = 0L; num < length; num += array.Length)
						{
							random.NextBytes(array);
							fileStream.Write(array, 0, array.Length);
						}
						fileStream.Flush();
						fileStream.Seek(0L, SeekOrigin.Begin);
						Array.Clear(array, 0, array.Length);
						for (long num2 = 0L; num2 < length; num2 += array.Length)
						{
							fileStream.Write(array, 0, array.Length);
						}
						fileStream.Flush();
					}
				}
			}
			string text;
			do
			{
				text = Path.Combine(Path.GetDirectoryName(path), Path.GetRandomFileName());
			}
			while (File.Exists(text));
			File.Copy(path, text);
			File.Delete(path);
			File.Delete(text);
		}
	}
}
