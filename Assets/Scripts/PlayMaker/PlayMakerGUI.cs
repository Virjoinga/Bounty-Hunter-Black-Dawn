using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("PlayMaker/PlayMakerGUI")]
public class PlayMakerGUI : MonoBehaviour
{
	private const float MaxLabelWidth = 200f;

	private static readonly List<PlayMakerFSM> fsmList = new List<PlayMakerFSM>();

	public static Fsm SelectedFSM;

	private static readonly GUIContent labelContent = new GUIContent();

	public bool previewOnGUI = true;

	public bool enableGUILayout = true;

	public bool drawStateLabels = true;

	public bool GUITextureStateLabels = true;

	public bool GUITextStateLabels = true;

	public bool filterLabelsWithDistance;

	public float maxLabelDistance = 10f;

	public bool controlMouseCursor = true;

	private static readonly List<PlayMakerFSM> SortedFsmList = new List<PlayMakerFSM>();

	private static GameObject labelGameObject;

	private static float fsmLabelIndex;

	private static PlayMakerGUI instance;

	private static GUISkin guiSkin;

	private static Color guiColor = Color.white;

	private static Color guiBackgroundColor = Color.white;

	private static Color guiContentColor = Color.white;

	private static Matrix4x4 guiMatrix = Matrix4x4.identity;

	private static GUIStyle labelStyle;

	private static Texture2D labelBackground;

	public static bool EnableStateLabels
	{
		get
		{
			if (instance == null)
			{
				instance = (PlayMakerGUI)Object.FindObjectOfType(typeof(PlayMakerGUI));
			}
			if (instance != null)
			{
				return instance.drawStateLabels;
			}
			return false;
		}
		set
		{
			if (instance == null)
			{
				instance = (PlayMakerGUI)Object.FindObjectOfType(typeof(PlayMakerGUI));
			}
			if (instance != null)
			{
				instance.drawStateLabels = value;
			}
		}
	}

	public static PlayMakerGUI Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (PlayMakerGUI)Object.FindObjectOfType(typeof(PlayMakerGUI));
				if (instance == null)
				{
					GameObject gameObject = new GameObject("PlayMakerGUI");
					instance = gameObject.AddComponent<PlayMakerGUI>();
				}
			}
			return instance;
		}
	}

	public static bool Enabled
	{
		get
		{
			if (instance != null)
			{
				return instance.enabled;
			}
			return false;
		}
	}

	public static GUISkin GUISkin
	{
		get
		{
			return guiSkin;
		}
		set
		{
			guiSkin = value;
		}
	}

	public static Color GUIColor
	{
		get
		{
			return guiColor;
		}
		set
		{
			guiColor = value;
		}
	}

	public static Color GUIBackgroundColor
	{
		get
		{
			return guiBackgroundColor;
		}
		set
		{
			guiBackgroundColor = value;
		}
	}

	public static Color GUIContentColor
	{
		get
		{
			return guiContentColor;
		}
		set
		{
			guiContentColor = value;
		}
	}

	public static Matrix4x4 GUIMatrix
	{
		get
		{
			return guiMatrix;
		}
		set
		{
			guiMatrix = value;
		}
	}

	public static Texture MouseCursor { get; set; }

	public static bool LockCursor { get; set; }

	public static bool HideCursor { get; set; }

	private static void InitLabelStyle()
	{
		if (labelBackground != null)
		{
			Object.Destroy(labelBackground);
		}
		labelBackground = new Texture2D(1, 1);
		labelBackground.SetPixel(0, 0, Color.white);
		labelBackground.Apply();
		GUIStyle gUIStyle = new GUIStyle();
		gUIStyle.normal.background = labelBackground;
		gUIStyle.normal.textColor = Color.white;
		gUIStyle.fontSize = 10;
		gUIStyle.alignment = TextAnchor.MiddleLeft;
		gUIStyle.padding = new RectOffset(4, 4, 1, 1);
		labelStyle = gUIStyle;
	}

	private void DrawStateLabels()
	{
		SortedFsmList.Clear();
		foreach (PlayMakerFSM fsm in PlayMakerFSM.FsmList)
		{
			if (fsm.gameObject.active)
			{
				SortedFsmList.Add(fsm);
			}
		}
		SortedFsmList.Sort((PlayMakerFSM x, PlayMakerFSM y) => string.Compare(x.gameObject.name, y.gameObject.name));
		labelGameObject = null;
		foreach (PlayMakerFSM sortedFsm in SortedFsmList)
		{
			if (sortedFsm.Fsm.ShowStateLabel)
			{
				DrawStateLabel(sortedFsm);
			}
		}
	}

	private void DrawStateLabel(PlayMakerFSM fsm)
	{
		if (labelStyle == null)
		{
			InitLabelStyle();
			if (labelStyle == null)
			{
				return;
			}
		}
		if (Camera.main == null || fsm.gameObject == Camera.main)
		{
			return;
		}
		if (fsm.gameObject == labelGameObject)
		{
			fsmLabelIndex += 1f;
		}
		else
		{
			fsmLabelIndex = 0f;
			labelGameObject = fsm.gameObject;
		}
		string text = GenerateStateLabel(fsm);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		Vector2 vector = default(Vector2);
		labelContent.text = text;
		Vector2 vector2 = labelStyle.CalcSize(labelContent);
		vector2.x = Mathf.Clamp(vector2.x, 10f, 200f);
		if (GUITextureStateLabels && fsm.gameObject.GetComponent<GUITexture>() != null)
		{
			vector.x = fsm.gameObject.transform.position.x * (float)Screen.width + fsm.gameObject.GetComponent<GUITexture>().pixelInset.x;
			vector.y = fsm.gameObject.transform.position.y * (float)Screen.height + fsm.gameObject.GetComponent<GUITexture>().pixelInset.y;
		}
		else if (GUITextStateLabels && fsm.gameObject.GetComponent<GUIText>() != null)
		{
			vector.x = fsm.gameObject.transform.position.x * (float)Screen.width;
			vector.y = fsm.gameObject.transform.position.y * (float)Screen.height;
		}
		else
		{
			if (filterLabelsWithDistance)
			{
				float num = Vector3.Distance(Camera.main.transform.position, fsm.transform.position);
				if (num > maxLabelDistance)
				{
					return;
				}
			}
			if (Camera.main.transform.InverseTransformPoint(fsm.transform.position).z <= 0f)
			{
				return;
			}
			vector = Camera.main.WorldToScreenPoint(fsm.transform.position);
			vector.x -= vector2.x * 0.5f;
		}
		vector.y = (float)Screen.height - vector.y - fsmLabelIndex * 15f;
		Color backgroundColor = GUI.backgroundColor;
		Color color = GUI.color;
		int num2 = 0;
		if (fsm.Fsm.ActiveState != null)
		{
			num2 = fsm.Fsm.ActiveState.ColorIndex;
		}
		Color color2 = Fsm.StateColors[num2];
		GUI.backgroundColor = new Color(color2.r, color2.g, color2.b, 0.5f);
		GUI.contentColor = Color.white;
		GUI.Label(new Rect(vector.x, vector.y, vector2.x, vector2.y), text, labelStyle);
		GUI.backgroundColor = backgroundColor;
		GUI.color = color;
	}

	private static string GenerateStateLabel(PlayMakerFSM fsm)
	{
		if (fsm.Fsm.ActiveState == null)
		{
			return "[DISABLED]";
		}
		return fsm.Fsm.ActiveState.Name;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.LogWarning("There should only be one PlayMakerGUI per scene!");
		}
	}

	private void OnEnable()
	{
	}

	private void OnGUI()
	{
		base.useGUILayout = enableGUILayout;
		if (GUISkin != null)
		{
			GUI.skin = GUISkin;
		}
		GUI.color = GUIColor;
		GUI.backgroundColor = GUIBackgroundColor;
		GUI.contentColor = GUIContentColor;
		if (previewOnGUI && !Application.isPlaying)
		{
			DoEditGUI();
			return;
		}
		fsmList.Clear();
		fsmList.AddRange(PlayMakerFSM.FsmList);
		foreach (PlayMakerFSM fsm in fsmList)
		{
			if (fsm == null || !fsm.Active || fsm.Fsm.ActiveState == null || fsm.Fsm.HandleOnGUI)
			{
				continue;
			}
			FsmStateAction[] actions = fsm.Fsm.ActiveState.Actions;
			FsmStateAction[] array = actions;
			foreach (FsmStateAction fsmStateAction in array)
			{
				if (fsmStateAction.Active)
				{
					fsmStateAction.OnGUI();
				}
			}
		}
		if (!Application.isPlaying || Event.current.type != EventType.Repaint)
		{
			return;
		}
		Matrix4x4 matrix = GUI.matrix;
		GUI.matrix = Matrix4x4.identity;
		if (MouseCursor != null)
		{
			Rect position = new Rect(Input.mousePosition.x - (float)MouseCursor.width * 0.5f, (float)Screen.height - Input.mousePosition.y - (float)MouseCursor.height * 0.5f, MouseCursor.width, MouseCursor.height);
			GUI.DrawTexture(position, MouseCursor);
		}
		if (drawStateLabels && EnableStateLabels)
		{
			DrawStateLabels();
		}
		GUI.matrix = matrix;
		GUIMatrix = Matrix4x4.identity;
		if (controlMouseCursor)
		{
			if (Screen.lockCursor != LockCursor)
			{
				Screen.lockCursor = LockCursor;
			}
			if (Cursor.visible == HideCursor)
			{
				Cursor.visible = !HideCursor;
			}
		}
	}

	private void OnDisable()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	private static void DoEditGUI()
	{
		if (SelectedFSM == null || SelectedFSM.HandleOnGUI)
		{
			return;
		}
		FsmState editState = SelectedFSM.EditState;
		if (editState == null || !editState.IsInitialized)
		{
			return;
		}
		FsmStateAction[] actions = editState.Actions;
		FsmStateAction[] array = actions;
		foreach (FsmStateAction fsmStateAction in array)
		{
			if (fsmStateAction.Active)
			{
				fsmStateAction.OnGUI();
			}
		}
	}

	public void OnApplicationQuit()
	{
		instance = null;
	}
}
