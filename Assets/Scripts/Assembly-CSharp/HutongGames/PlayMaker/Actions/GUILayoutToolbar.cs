using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUILayout)]
	[Tooltip("GUILayout Toolbar. NOTE: Arrays must be the same length as NumButtons or empty.")]
	public class GUILayoutToolbar : GUILayoutAction
	{
		public FsmInt numButtons;

		[UIHint(UIHint.Variable)]
		public FsmInt selectedButton;

		public FsmEvent[] buttonEventsArray;

		public FsmTexture[] imagesArray;

		public FsmString[] textsArray;

		public FsmString[] tooltipsArray;

		public FsmString style;

		private GUIContent[] contents;

		public GUIContent[] Contents
		{
			get
			{
				if (contents == null)
				{
					contents = new GUIContent[numButtons.Value];
					for (int i = 0; i < numButtons.Value; i++)
					{
						contents[i] = new GUIContent();
					}
					for (int j = 0; j < imagesArray.Length; j++)
					{
						contents[j].image = imagesArray[j].Value;
					}
					for (int k = 0; k < textsArray.Length; k++)
					{
						contents[k].text = textsArray[k].Value;
					}
					for (int l = 0; l < tooltipsArray.Length; l++)
					{
						contents[l].tooltip = tooltipsArray[l].Value;
					}
				}
				return contents;
			}
		}

		public override void Reset()
		{
			base.Reset();
			numButtons = 0;
			selectedButton = null;
			buttonEventsArray = new FsmEvent[0];
			imagesArray = new FsmTexture[0];
			tooltipsArray = new FsmString[0];
			style = "Button";
		}

		public override void OnEnter()
		{
			string text = ErrorCheck();
			if (!string.IsNullOrEmpty(text))
			{
				LogError(text);
				Finish();
			}
		}

		public override void OnGUI()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			selectedButton.Value = GUILayout.Toolbar(selectedButton.Value, Contents, style.Value, base.LayoutOptions);
			if (GUI.changed)
			{
				if (selectedButton.Value < buttonEventsArray.Length)
				{
					base.Fsm.Event(buttonEventsArray[selectedButton.Value]);
					GUIUtility.ExitGUI();
				}
			}
			else
			{
				GUI.changed = changed;
			}
		}

		public override string ErrorCheck()
		{
			string text = string.Empty;
			if (imagesArray.Length > 0 && imagesArray.Length != numButtons.Value)
			{
				text += "Images array doesn't match NumButtons.\n";
			}
			if (textsArray.Length > 0 && textsArray.Length != numButtons.Value)
			{
				text += "Texts array doesn't match NumButtons.\n";
			}
			if (tooltipsArray.Length > 0 && tooltipsArray.Length != numButtons.Value)
			{
				text += "Tooltips array doesn't match NumButtons.\n";
			}
			return text;
		}
	}
}
