using System;
using System.IO;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Saves a Screenshot to the users MyPictures folder. TIP: Can be useful for automated testing and debugging.")]
	public class TakeScreenshot : FsmStateAction
	{
		[RequiredField]
		public FsmString filename;

		public bool autoNumber;

		private int screenshotCount;

		public override void Reset()
		{
			filename = string.Empty;
			autoNumber = false;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(filename.Value))
			{
				return;
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/";
			string path = text + filename.Value + ".png";
			if (autoNumber)
			{
				while (File.Exists(path))
				{
					screenshotCount++;
					path = text + filename.Value + screenshotCount + ".png";
				}
			}
			ScreenCapture.CaptureScreenshot(path);
			Finish();
		}
	}
}
