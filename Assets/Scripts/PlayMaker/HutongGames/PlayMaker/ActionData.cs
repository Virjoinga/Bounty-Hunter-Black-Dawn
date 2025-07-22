using System;
using System.Collections.Generic;
using System.Reflection;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class ActionData
	{
		private static List<string> assemblies;

		private static readonly Dictionary<string, Type> ActionTypeLookup = new Dictionary<string, Type>();

		public static readonly Dictionary<Type, FieldInfo[]> ActionFieldsLookup = new Dictionary<Type, FieldInfo[]>();

		private static Fsm currentFsm;

		private static FsmState currentState;

		private static FsmStateAction currentAction;

		private static int currentActionIndex;

		private static bool resaveActionData;

		private static readonly List<int> UsedIndices = new List<int>();

		private static readonly List<FieldInfo> InitFields = new List<FieldInfo>();

		[SerializeField]
		private List<string> actionNames = new List<string>();

		[SerializeField]
		private List<string> customNames = new List<string>();

		[SerializeField]
		private List<bool> actionEnabled = new List<bool>();

		[SerializeField]
		private List<bool> actionIsOpen = new List<bool>();

		[SerializeField]
		private List<int> actionStartIndex = new List<int>();

		[SerializeField]
		private List<int> actionHashCodes = new List<int>();

		[SerializeField]
		private List<UnityEngine.Object> unityObjectParams = new List<UnityEngine.Object>();

		[SerializeField]
		private List<FsmGameObject> fsmGameObjectParams = new List<FsmGameObject>();

		[SerializeField]
		private List<FsmOwnerDefault> fsmOwnerDefaultParams = new List<FsmOwnerDefault>();

		[SerializeField]
		private List<FsmAnimationCurve> animationCurveParams = new List<FsmAnimationCurve>();

		[SerializeField]
		private List<FunctionCall> functionCallParams = new List<FunctionCall>();

		[SerializeField]
		private List<FsmEventTarget> fsmEventTargetParams = new List<FsmEventTarget>();

		[SerializeField]
		private List<FsmProperty> fsmPropertyParams = new List<FsmProperty>();

		[SerializeField]
		private List<LayoutOption> layoutOptionParams = new List<LayoutOption>();

		[SerializeField]
		private List<FsmString> fsmStringParams = new List<FsmString>();

		[SerializeField]
		private List<FsmObject> fsmObjectParams = new List<FsmObject>();

		[SerializeField]
		private List<int> arrayParamSizes = new List<int>();

		[SerializeField]
		private List<string> arrayParamTypes = new List<string>();

		[SerializeField]
		private List<byte> byteData = new List<byte>();

		private byte[] byteDataAsArray;

		[SerializeField]
		private List<ParamDataType> paramDataType = new List<ParamDataType>();

		[SerializeField]
		private List<string> paramName = new List<string>();

		[SerializeField]
		private List<int> paramDataPos = new List<int>();

		[SerializeField]
		private List<int> paramByteDataSize = new List<int>();

		private int nextParamIndex;

		public int ActionCount
		{
			get
			{
				return actionNames.Count;
			}
		}

		public ActionData Copy()
		{
			ActionData actionData = new ActionData();
			actionData.actionNames = new List<string>(actionNames);
			actionData.customNames = new List<string>(customNames);
			actionData.actionEnabled = new List<bool>(actionEnabled);
			actionData.actionIsOpen = new List<bool>(actionIsOpen);
			actionData.actionStartIndex = new List<int>(actionStartIndex);
			actionData.actionHashCodes = new List<int>(actionHashCodes);
			actionData.unityObjectParams = new List<UnityEngine.Object>(unityObjectParams);
			actionData.fsmStringParams = CopyFsmStringParams();
			actionData.fsmObjectParams = CopyFsmObjectParams();
			actionData.fsmGameObjectParams = CopyFsmGameObjectParams();
			actionData.fsmOwnerDefaultParams = CopyFsmOwnerDefaultParams();
			actionData.animationCurveParams = CopyAnimationCurveParams();
			actionData.functionCallParams = CopyFunctionCallParams();
			actionData.fsmPropertyParams = CopyFsmPropertyParams();
			actionData.fsmEventTargetParams = CopyFsmEventTargetParams();
			actionData.layoutOptionParams = CopyLayoutOptionParams();
			actionData.byteData = new List<byte>(byteData);
			actionData.paramDataPos = new List<int>(paramDataPos);
			actionData.paramByteDataSize = new List<int>(paramByteDataSize);
			actionData.paramDataType = new List<ParamDataType>(paramDataType);
			actionData.arrayParamSizes = new List<int>(arrayParamSizes);
			actionData.arrayParamTypes = new List<string>(arrayParamTypes);
			actionData.paramName = new List<string>(paramName);
			return actionData;
		}

		private List<FsmString> CopyFsmStringParams()
		{
			List<FsmString> list = new List<FsmString>();
			foreach (FsmString fsmStringParam in fsmStringParams)
			{
				list.Add(new FsmString(fsmStringParam));
			}
			return list;
		}

		private List<FsmObject> CopyFsmObjectParams()
		{
			List<FsmObject> list = new List<FsmObject>();
			foreach (FsmObject fsmObjectParam in fsmObjectParams)
			{
				list.Add(new FsmObject(fsmObjectParam));
			}
			return list;
		}

		private List<FsmGameObject> CopyFsmGameObjectParams()
		{
			List<FsmGameObject> list = new List<FsmGameObject>();
			foreach (FsmGameObject fsmGameObjectParam in fsmGameObjectParams)
			{
				list.Add(new FsmGameObject(fsmGameObjectParam));
			}
			return list;
		}

		private List<FsmOwnerDefault> CopyFsmOwnerDefaultParams()
		{
			List<FsmOwnerDefault> list = new List<FsmOwnerDefault>();
			foreach (FsmOwnerDefault fsmOwnerDefaultParam in fsmOwnerDefaultParams)
			{
				list.Add(new FsmOwnerDefault(fsmOwnerDefaultParam));
			}
			return list;
		}

		private List<FsmAnimationCurve> CopyAnimationCurveParams()
		{
			List<FsmAnimationCurve> list = new List<FsmAnimationCurve>();
			foreach (FsmAnimationCurve animationCurveParam in animationCurveParams)
			{
				FsmAnimationCurve fsmAnimationCurve = new FsmAnimationCurve();
				fsmAnimationCurve.curve.keys = animationCurveParam.curve.keys;
				FsmAnimationCurve item = fsmAnimationCurve;
				list.Add(item);
			}
			return list;
		}

		private List<FunctionCall> CopyFunctionCallParams()
		{
			List<FunctionCall> list = new List<FunctionCall>();
			foreach (FunctionCall functionCallParam in functionCallParams)
			{
				list.Add(new FunctionCall(functionCallParam));
			}
			return list;
		}

		private List<FsmProperty> CopyFsmPropertyParams()
		{
			List<FsmProperty> list = new List<FsmProperty>();
			foreach (FsmProperty fsmPropertyParam in fsmPropertyParams)
			{
				list.Add(new FsmProperty(fsmPropertyParam));
			}
			return list;
		}

		private List<FsmEventTarget> CopyFsmEventTargetParams()
		{
			List<FsmEventTarget> list = new List<FsmEventTarget>();
			foreach (FsmEventTarget fsmEventTargetParam in fsmEventTargetParams)
			{
				list.Add(new FsmEventTarget(fsmEventTargetParam));
			}
			return list;
		}

		private List<LayoutOption> CopyLayoutOptionParams()
		{
			List<LayoutOption> list = new List<LayoutOption>();
			foreach (LayoutOption layoutOptionParam in layoutOptionParams)
			{
				list.Add(new LayoutOption(layoutOptionParam));
			}
			return list;
		}

		private void ClearActionData()
		{
			actionNames.Clear();
			customNames.Clear();
			actionEnabled.Clear();
			actionIsOpen.Clear();
			actionStartIndex.Clear();
			actionHashCodes.Clear();
			unityObjectParams.Clear();
			fsmStringParams.Clear();
			fsmObjectParams.Clear();
			fsmGameObjectParams.Clear();
			fsmOwnerDefaultParams.Clear();
			animationCurveParams.Clear();
			functionCallParams.Clear();
			fsmPropertyParams.Clear();
			fsmEventTargetParams.Clear();
			layoutOptionParams.Clear();
			byteData.Clear();
			paramDataPos.Clear();
			paramByteDataSize.Clear();
			arrayParamSizes.Clear();
			arrayParamTypes.Clear();
			paramDataType.Clear();
			paramName.Clear();
			nextParamIndex = 0;
		}

		public static Type GetActionType(string actionName)
		{
			Type value;
			if (ActionTypeLookup.TryGetValue(actionName, out value))
			{
				return value;
			}
			value = GetGlobalType(actionName);
			if (value == null)
			{
				return null;
			}
			ActionTypeLookup.Add(actionName, value);
			return value;
		}

		private static Type GetGlobalType(string s)
		{
			Type type = Type.GetType(s + ",Assembly-CSharp") ?? Type.GetType(s + ",PlayMaker");
			if (type == null)
			{
				if (assemblies == null)
				{
					assemblies = new List<string>();
					Assembly[] array = AppDomain.CurrentDomain.GetAssemblies();
					Assembly[] array2 = array;
					foreach (Assembly assembly in array2)
					{
						assemblies.Add(assembly.FullName);
					}
				}
				foreach (string assembly2 in assemblies)
				{
					type = Type.GetType(s + "," + assembly2);
					if (type != null)
					{
						break;
					}
				}
			}
			return type;
		}

		public static FieldInfo[] GetFields(Type actionType)
		{
			FieldInfo[] value;
			if (ActionFieldsLookup.TryGetValue(actionType, out value))
			{
				return value;
			}
			value = actionType.GetFields(BindingFlags.Instance | BindingFlags.Public);
			ActionFieldsLookup.Add(actionType, value);
			return value;
		}

		private static int GetActionTypeHashCode(Type actionType)
		{
			string text = "";
			FieldInfo[] fields = GetFields(actionType);
			foreach (FieldInfo fieldInfo in fields)
			{
				text += fieldInfo.FieldType.ToString();
			}
			return text.GetHashCode();
		}

		public FsmStateAction[] LoadActions(FsmState state)
		{
			byteDataAsArray = byteData.ToArray();
			List<FsmStateAction> list = new List<FsmStateAction>();
			resaveActionData = false;
			for (int i = 0; i < actionNames.Count; i++)
			{
				FsmStateAction fsmStateAction = CreateAction(state, i);
				if (fsmStateAction != null)
				{
					list.Add(fsmStateAction);
					fsmStateAction.Init(state);
				}
			}
			if (resaveActionData)
			{
				SaveActions(list.ToArray());
				list = new List<FsmStateAction>(LoadActions(state));
			}
			return list.ToArray();
		}

		public FsmStateAction CreateAction(FsmState state, int actionIndex)
		{
			currentFsm = state.Fsm;
			currentState = state;
			currentActionIndex = actionIndex;
			FsmUtility.CurrentFsm = state.Fsm;
			if (state.Fsm == null)
			{
				Debug.LogError("state.Fsm == null");
			}
			string text = actionNames[actionIndex];
			Type actionType = GetActionType(text);
			if (actionType == null)
			{
				string text2 = TryFixActionName(text);
				actionType = GetActionType(text2);
				if (actionType == null)
				{
					MissingAction missingAction = (MissingAction)Activator.CreateInstance(typeof(MissingAction));
					string text3 = (missingAction.actionName = FsmUtility.StripNamespace(text));
					currentAction = missingAction;
					LogError("Could Not Create Action: " + text3 + " (Maybe the script was removed?)");
					Debug.LogError("Could Not Create Action: " + FsmUtility.GetPath(state) + text3 + " (Maybe the script was removed?)");
					resaveActionData = true;
					return currentAction;
				}
				string text4 = "Action : " + text + " Updated To: " + text2;
				LogInfo(text4);
				Debug.Log(text4);
				text = text2;
				resaveActionData = true;
			}
			FsmStateAction fsmStateAction = (currentAction = (FsmStateAction)Activator.CreateInstance(actionType));
			fsmStateAction.Fsm = currentFsm;
			fsmStateAction.Reset();
			if (Application.isEditor)
			{
				if (paramDataType.Count != paramDataPos.Count || paramName.Count != paramDataPos.Count)
				{
					Debug.LogWarning("Old data format detected! Updating...");
					resaveActionData = true;
				}
				int num = actionHashCodes[actionIndex];
				if (num != GetActionTypeHashCode(actionType))
				{
					resaveActionData = true;
					if (paramDataType.Count != paramDataPos.Count)
					{
						LogError("Action has changed since FSM was saved. Could not recover parameters. Parameters reset to default values.");
						Debug.LogError("Action script has changed since Fsm was saved: " + FsmUtility.GetPath(state) + FsmUtility.StripNamespace(text) + ". Parameters reset to default values...");
						return fsmStateAction;
					}
					try
					{
						fsmStateAction = TryRecoverAction(actionType, fsmStateAction, actionIndex);
					}
					catch
					{
						LogError("Action has changed since FSM was saved. Could not recover parameters. Parameters reset to default values.");
					}
					return fsmStateAction;
				}
			}
			nextParamIndex = actionStartIndex[actionIndex];
			FieldInfo[] fields = GetFields(actionType);
			FieldInfo[] array = fields;
			foreach (FieldInfo fieldInfo in array)
			{
				try
				{
					LoadActionField(fsmStateAction, fieldInfo, nextParamIndex);
				}
				catch (Exception)
				{
					Debug.LogError("Error loading action: " + FsmState.GetFullStateLabel(state) + " : " + text + " : " + fieldInfo.Name);
				}
				nextParamIndex++;
			}
			if (customNames.Count > actionIndex)
			{
				fsmStateAction.Name = customNames[actionIndex];
			}
			if (actionEnabled.Count > actionIndex)
			{
				fsmStateAction.Enabled = actionEnabled[actionIndex];
			}
			fsmStateAction.IsOpen = actionIsOpen.Count <= actionIndex || actionIsOpen[actionIndex];
			fsmStateAction.Awake();
			return fsmStateAction;
		}

		private void LoadActionField(object obj, FieldInfo field, int paramIndex)
		{
			Type fieldType = field.FieldType;
			object value = null;
			if (fieldType == typeof(FsmGameObject))
			{
				value = GetFsmGameObject(paramIndex);
			}
			else if (fieldType == typeof(FsmEvent))
			{
				value = FsmUtility.ByteArrayToFsmEvent(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmFloat))
			{
				value = FsmUtility.ByteArrayToFsmFloat(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmInt))
			{
				value = FsmUtility.ByteArrayToFsmInt(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmBool))
			{
				value = FsmUtility.ByteArrayToFsmBool(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmVector2))
			{
				value = FsmUtility.ByteArrayToFsmVector2(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmVector3))
			{
				value = FsmUtility.ByteArrayToFsmVector3(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmRect))
			{
				value = FsmUtility.ByteArrayToFsmRect(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmQuaternion))
			{
				value = FsmUtility.ByteArrayToFsmQuaternion(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmColor))
			{
				value = FsmUtility.ByteArrayToFsmColor(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType == typeof(FsmObject))
			{
				value = GetFsmObject(paramIndex);
			}
			else if (fieldType == typeof(FsmMaterial))
			{
				value = GetFsmMaterial(paramIndex);
			}
			else if (fieldType == typeof(FsmTexture))
			{
				value = GetFsmTexture(paramIndex);
			}
			else if (fieldType == typeof(FunctionCall))
			{
				value = GetFunctionCall(paramIndex);
			}
			else if (fieldType == typeof(FsmProperty))
			{
				value = GetFsmProperty(paramIndex);
			}
			else if (fieldType == typeof(FsmEventTarget))
			{
				value = GetFsmEventTarget(paramIndex);
			}
			else if (fieldType == typeof(LayoutOption))
			{
				value = GetLayoutOption(paramIndex);
			}
			else if (fieldType == typeof(FsmOwnerDefault))
			{
				value = GetFsmOwnerDefault(paramIndex);
			}
			else if (fieldType == typeof(FsmAnimationCurve))
			{
				value = animationCurveParams[paramDataPos[paramIndex]] ?? new FsmAnimationCurve();
			}
			else if (fieldType == typeof(FsmString))
			{
				value = GetFsmString(paramIndex);
			}
			else if (fieldType == typeof(float))
			{
				value = FsmUtility.BitConverter.ToSingle(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(int))
			{
				value = FsmUtility.BitConverter.ToInt32(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(bool))
			{
				value = FsmUtility.BitConverter.ToBoolean(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(Color))
			{
				value = FsmUtility.ByteArrayToColor(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(Vector2))
			{
				value = FsmUtility.ByteArrayToVector2(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(Vector3))
			{
				value = FsmUtility.ByteArrayToVector3(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(Vector4))
			{
				value = FsmUtility.ByteArrayToVector4(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(Rect))
			{
				value = FsmUtility.ByteArrayToRect(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (fieldType == typeof(string))
			{
				value = FsmUtility.ByteArrayToString(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
			}
			else if (fieldType.IsEnum)
			{
				value = FsmUtility.BitConverter.ToInt32(byteDataAsArray, paramDataPos[paramIndex]);
			}
			else if (typeof(FsmObject).IsAssignableFrom(fieldType))
			{
				FsmObject fsmObject = fsmObjectParams[paramDataPos[paramIndex]];
				if (fsmObject != null)
				{
					field.SetValue(obj, fsmObject);
					return;
				}
			}
			else
			{
				if (!typeof(UnityEngine.Object).IsAssignableFrom(fieldType))
				{
					if (fieldType.IsArray)
					{
						Type globalType = GetGlobalType(arrayParamTypes[paramDataPos[paramIndex]]);
						int num = arrayParamSizes[paramDataPos[paramIndex]];
						Array array = Array.CreateInstance(globalType, num);
						for (int i = 0; i < num; i++)
						{
							nextParamIndex++;
							LoadArrayElement(array, globalType, i, nextParamIndex);
						}
						field.SetValue(obj, array);
					}
					else
					{
						Debug.LogError("ActionData: Missing LoadActionField for type: " + fieldType);
						field.SetValue(obj, null);
					}
					return;
				}
				UnityEngine.Object @object = unityObjectParams[paramDataPos[paramIndex]];
				if (@object != null)
				{
					field.SetValue(obj, @object);
					return;
				}
			}
			field.SetValue(obj, value);
		}

		private void LoadArrayElement(Array field, Type fieldType, int elementIndex, int paramIndex)
		{
			if (elementIndex >= field.GetLength(0) || paramIndex >= paramDataPos.Count)
			{
				return;
			}
			if (fieldType == typeof(FsmGameObject))
			{
				field.SetValue(GetFsmGameObject(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(FunctionCall))
			{
				field.SetValue(GetFunctionCall(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(FsmProperty))
			{
				field.SetValue(GetFsmProperty(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(LayoutOption))
			{
				field.SetValue(GetLayoutOption(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(FsmOwnerDefault))
			{
				field.SetValue(GetFsmOwnerDefault(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(FsmAnimationCurve))
			{
				field.SetValue(animationCurveParams[paramDataPos[paramIndex]] ?? new FsmAnimationCurve(), elementIndex);
			}
			else if (fieldType == typeof(FsmString))
			{
				field.SetValue(GetFsmString(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(FsmObject))
			{
				field.SetValue(GetFsmObject(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(FsmMaterial))
			{
				field.SetValue(GetFsmMaterial(paramIndex), elementIndex);
			}
			else if (fieldType == typeof(FsmTexture))
			{
				field.SetValue(GetFsmTexture(paramIndex), elementIndex);
			}
			else if (fieldType.IsArray)
			{
				Debug.LogError("Nested arrays are not supported!");
			}
			else if (fieldType == typeof(FsmEvent))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmEvent(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmFloat))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmFloat(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmInt))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmInt(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmBool))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmBool(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmVector2))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmVector2(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmVector3))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmVector3(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmRect))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmRect(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmQuaternion))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmQuaternion(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(FsmColor))
			{
				field.SetValue(FsmUtility.ByteArrayToFsmColor(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(float))
			{
				field.SetValue(FsmUtility.BitConverter.ToSingle(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(int))
			{
				field.SetValue(FsmUtility.BitConverter.ToInt32(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(bool))
			{
				field.SetValue(FsmUtility.BitConverter.ToBoolean(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(Color))
			{
				field.SetValue(FsmUtility.ByteArrayToColor(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(Vector2))
			{
				field.SetValue(FsmUtility.ByteArrayToVector2(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(Vector3))
			{
				field.SetValue(FsmUtility.ByteArrayToVector3(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(Vector4))
			{
				field.SetValue(FsmUtility.ByteArrayToVector4(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(Rect))
			{
				field.SetValue(FsmUtility.ByteArrayToRect(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (fieldType == typeof(string))
			{
				field.SetValue(FsmUtility.ByteArrayToString(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]), elementIndex);
			}
			else if (fieldType.IsEnum)
			{
				field.SetValue(FsmUtility.BitConverter.ToInt32(byteDataAsArray, paramDataPos[paramIndex]), elementIndex);
			}
			else if (typeof(FsmObject).IsAssignableFrom(fieldType))
			{
				FsmObject fsmObject = fsmObjectParams[paramDataPos[paramIndex]];
				if (fsmObject != null)
				{
					field.SetValue(fsmObject, elementIndex);
				}
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(fieldType))
			{
				UnityEngine.Object @object = unityObjectParams[paramDataPos[paramIndex]];
				if (@object != null)
				{
					field.SetValue(@object, elementIndex);
				}
			}
			else
			{
				field.SetValue(null, elementIndex);
			}
		}

		private static void LogError(string error)
		{
			if (currentState != null && currentAction != null && currentFsm != null && !(currentFsm.Owner == null))
			{
				PlayMakerFSM fsm = currentFsm.Owner as PlayMakerFSM;
				ActionReport.LogError(fsm, currentState, currentAction, currentActionIndex, error);
			}
		}

		private static void LogInfo(string info)
		{
			if (currentState != null && currentAction != null)
			{
				PlayMakerFSM fsm = currentFsm.Owner as PlayMakerFSM;
				ActionReport.Log(fsm, currentState, currentAction, currentActionIndex, info);
			}
		}

		private FsmGameObject GetFsmGameObject(int paramIndex)
		{
			FsmGameObject fsmGameObject = fsmGameObjectParams[paramDataPos[paramIndex]];
			if (fsmGameObject == null)
			{
				return new FsmGameObject();
			}
			if (string.IsNullOrEmpty(fsmGameObject.Name))
			{
				return fsmGameObject;
			}
			return currentFsm.GetFsmGameObject(fsmGameObject.Name);
		}

		private FunctionCall GetFunctionCall(int paramIndex)
		{
			FunctionCall functionCall = functionCallParams[paramDataPos[paramIndex]];
			if (functionCall == null)
			{
				return new FunctionCall();
			}
			if (!string.IsNullOrEmpty(functionCall.BoolParameter.Name))
			{
				functionCall.BoolParameter = currentFsm.GetFsmBool(functionCall.BoolParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.FloatParameter.Name))
			{
				functionCall.FloatParameter = currentFsm.GetFsmFloat(functionCall.FloatParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.GameObjectParameter.Name))
			{
				functionCall.GameObjectParameter = currentFsm.GetFsmGameObject(functionCall.GameObjectParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.IntParameter.Name))
			{
				functionCall.IntParameter = currentFsm.GetFsmInt(functionCall.IntParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.MaterialParameter.Name))
			{
				functionCall.MaterialParameter = currentFsm.GetFsmMaterial(functionCall.MaterialParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.ObjectParameter.Name))
			{
				functionCall.ObjectParameter = currentFsm.GetFsmObject(functionCall.ObjectParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.QuaternionParameter.Name))
			{
				functionCall.QuaternionParameter = currentFsm.GetFsmQuaternion(functionCall.QuaternionParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.RectParamater.Name))
			{
				functionCall.RectParamater = currentFsm.GetFsmRect(functionCall.RectParamater.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.StringParameter.Name))
			{
				functionCall.StringParameter = currentFsm.GetFsmString(functionCall.StringParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.TextureParameter.Name))
			{
				functionCall.TextureParameter = currentFsm.GetFsmTexture(functionCall.TextureParameter.Name);
			}
			if (!string.IsNullOrEmpty(functionCall.Vector3Parameter.Name))
			{
				functionCall.Vector3Parameter = currentFsm.GetFsmVector3(functionCall.Vector3Parameter.Name);
			}
			return functionCall;
		}

		private FsmProperty GetFsmProperty(int paramIndex)
		{
			FsmProperty fsmProperty = fsmPropertyParams[paramDataPos[paramIndex]];
			if (fsmProperty == null)
			{
				return new FsmProperty();
			}
			if (!string.IsNullOrEmpty(fsmProperty.TargetObject.Name))
			{
				fsmProperty.TargetObject = currentFsm.GetFsmObject(fsmProperty.TargetObject.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.BoolParameter.Name))
			{
				fsmProperty.BoolParameter = currentFsm.GetFsmBool(fsmProperty.BoolParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.FloatParameter.Name))
			{
				fsmProperty.FloatParameter = currentFsm.GetFsmFloat(fsmProperty.FloatParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.GameObjectParameter.Name))
			{
				fsmProperty.GameObjectParameter = currentFsm.GetFsmGameObject(fsmProperty.GameObjectParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.IntParameter.Name))
			{
				fsmProperty.IntParameter = currentFsm.GetFsmInt(fsmProperty.IntParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.MaterialParameter.Name))
			{
				fsmProperty.MaterialParameter = currentFsm.GetFsmMaterial(fsmProperty.MaterialParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.ObjectParameter.Name))
			{
				fsmProperty.ObjectParameter = currentFsm.GetFsmObject(fsmProperty.ObjectParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.QuaternionParameter.Name))
			{
				fsmProperty.QuaternionParameter = currentFsm.GetFsmQuaternion(fsmProperty.QuaternionParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.RectParamater.Name))
			{
				fsmProperty.RectParamater = currentFsm.GetFsmRect(fsmProperty.RectParamater.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.StringParameter.Name))
			{
				fsmProperty.StringParameter = currentFsm.GetFsmString(fsmProperty.StringParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.TextureParameter.Name))
			{
				fsmProperty.TextureParameter = currentFsm.GetFsmTexture(fsmProperty.TextureParameter.Name);
			}
			if (!string.IsNullOrEmpty(fsmProperty.Vector3Parameter.Name))
			{
				fsmProperty.Vector3Parameter = currentFsm.GetFsmVector3(fsmProperty.Vector3Parameter.Name);
			}
			return fsmProperty;
		}

		private FsmEventTarget GetFsmEventTarget(int paramIndex)
		{
			FsmEventTarget fsmEventTarget = fsmEventTargetParams[paramDataPos[paramIndex]];
			if (fsmEventTarget == null)
			{
				return new FsmEventTarget();
			}
			if (!string.IsNullOrEmpty(fsmEventTarget.excludeSelf.Name))
			{
				fsmEventTarget.excludeSelf = currentFsm.GetFsmBool(fsmEventTarget.excludeSelf.Name);
			}
			string name = fsmEventTarget.gameObject.GameObject.Name;
			if (!string.IsNullOrEmpty(name))
			{
				fsmEventTarget.gameObject.GameObject = currentFsm.GetFsmGameObject(name);
			}
			if (!string.IsNullOrEmpty(fsmEventTarget.fsmName.Name))
			{
				fsmEventTarget.fsmName = currentFsm.GetFsmString(fsmEventTarget.fsmName.Name);
			}
			if (!string.IsNullOrEmpty(fsmEventTarget.sendToChildren.Name))
			{
				fsmEventTarget.sendToChildren = currentFsm.GetFsmBool(fsmEventTarget.sendToChildren.Name);
			}
			return fsmEventTarget;
		}

		private LayoutOption GetLayoutOption(int paramIndex)
		{
			LayoutOption layoutOption = layoutOptionParams[paramDataPos[paramIndex]];
			if (layoutOption == null)
			{
				return new LayoutOption();
			}
			if (!string.IsNullOrEmpty(layoutOption.boolParam.Name))
			{
				layoutOption.boolParam = currentFsm.GetFsmBool(layoutOption.boolParam.Name);
			}
			if (!string.IsNullOrEmpty(layoutOption.floatParam.Name))
			{
				layoutOption.floatParam = currentFsm.GetFsmFloat(layoutOption.floatParam.Name);
			}
			return layoutOption;
		}

		private FsmOwnerDefault GetFsmOwnerDefault(int paramIndex)
		{
			FsmOwnerDefault fsmOwnerDefault = fsmOwnerDefaultParams[paramDataPos[paramIndex]];
			if (fsmOwnerDefault == null)
			{
				return new FsmOwnerDefault();
			}
			string name = fsmOwnerDefault.GameObject.Name;
			if (!string.IsNullOrEmpty(name))
			{
				fsmOwnerDefault.GameObject = currentFsm.GetFsmGameObject(name);
			}
			return fsmOwnerDefault;
		}

		private FsmString GetFsmString(int paramIndex)
		{
			FsmString fsmString = fsmStringParams[paramDataPos[paramIndex]];
			if (fsmString == null)
			{
				return new FsmString();
			}
			if (string.IsNullOrEmpty(fsmString.Name))
			{
				return fsmString;
			}
			return currentFsm.GetFsmString(fsmString.Name);
		}

		private FsmObject GetFsmObject(int paramIndex)
		{
			FsmObject fsmObject = fsmObjectParams[paramDataPos[paramIndex]];
			if (fsmObject == null)
			{
				return new FsmObject();
			}
			if (string.IsNullOrEmpty(fsmObject.Name))
			{
				return fsmObject;
			}
			return currentFsm.GetFsmObject(fsmObject.Name);
		}

		private FsmMaterial GetFsmMaterial(int paramIndex)
		{
			FsmObject fsmObject = fsmObjectParams[paramDataPos[paramIndex]];
			if (fsmObject == null)
			{
				return new FsmMaterial();
			}
			if (string.IsNullOrEmpty(fsmObject.Name))
			{
				return new FsmMaterial(fsmObject);
			}
			return currentFsm.GetFsmMaterial(fsmObject.Name);
		}

		private FsmTexture GetFsmTexture(int paramIndex)
		{
			FsmObject fsmObject = fsmObjectParams[paramDataPos[paramIndex]];
			if (fsmObject == null)
			{
				return new FsmTexture();
			}
			if (string.IsNullOrEmpty(fsmObject.Name))
			{
				return new FsmTexture(fsmObject);
			}
			return currentFsm.GetFsmTexture(fsmObject.Name);
		}

		private static string TryFixActionName(string actionName)
		{
			switch (actionName)
			{
			case "HutongGames.PlayMaker.Actions.ClampFloat":
				return "HutongGames.PlayMaker.Actions.FloatClamp";
			case "HutongGames.PlayMaker.Actions.ClampInt":
				return "HutongGames.PlayMaker.Actions.IntClamp";
			case "HutongGames.PlayMaker.Actions.AllTrue":
				return "HutongGames.PlayMaker.Actions.BoolAllTrue";
			case "HutongGames.PlayMaker.Actions.NoneTrue":
				return "HutongGames.PlayMaker.Actions.BoolNoneTrue";
			case "HutongGames.PlayMaker.Actions.AnyTrue":
				return "HutongGames.PlayMaker.Actions.BoolAnyTrue";
			case "HutongGames.PlayMaker.Actions.MouseCursor":
				return "HutongGames.PlayMaker.Actions.SetMouseCursor";
			case "HutongGames.PlayMaker.Actions.HasChildren":
				return "HutongGames.PlayMaker.Actions.GameObjectHasChildren";
			case "HutongGames.PlayMaker.Actions.HasTag":
				return "HutongGames.PlayMaker.Actions.GameObjectCompareTag";
			case "HutongGames.PlayMaker.Actions.IsChildOf":
				return "HutongGames.PlayMaker.Actions.GameObjectIsChildOf";
			case "HutongGames.PlayMaker.Actions.IsGameObjectNull":
				return "HutongGames.PlayMaker.Actions.GameObjectIsNull";
			case "HutongGames.PlayMaker.Actions.IsVisible":
				return "HutongGames.PlayMaker.Actions.GameObjectIsVisible";
			case "HutongGames.PlayMaker.Actions.OnGameObjectChange":
				return "HutongGames.PlayMaker.Actions.GameObjectChanged";
			case "HutongGames.PlayMaker.Actions.TagSwitch":
				return "HutongGames.PlayMaker.Actions.GameObjectTagSwitch";
			case "HutongGames.PlayMaker.Actions.Button":
				return "HutongGames.PlayMaker.Actions.GUIButton";
			case "HutongGames.PlayMaker.Actions.HorizontalSlider":
				return "HutongGames.PlayMaker.Actions.GUIHorizontalSlider";
			case "HutongGames.PlayMaker.Actions.Label":
				return "HutongGames.PlayMaker.Actions.GUILabel";
			case "HutongGames.PlayMaker.Actions.VerticalSlider":
				return "HutongGames.PlayMaker.Actions.GUIVerticalSlider";
			case "HutongGames.PlayMaker.Actions.GetGameObjectName":
				return "HutongGames.PlayMaker.Actions.GetName";
			default:
				return actionName;
			}
		}

		private FsmStateAction TryRecoverAction(Type actionType, FsmStateAction action, int actionIndex)
		{
			UsedIndices.Clear();
			InitFields.Clear();
			int num = actionStartIndex[actionIndex];
			int num2 = ((actionIndex < actionNames.Count - 1) ? actionStartIndex[actionIndex + 1] : paramDataPos.Count);
			if (paramName.Count == paramDataPos.Count)
			{
				for (int i = num; i < num2; i++)
				{
					FieldInfo fieldInfo = FindField(actionType, i);
					if (fieldInfo != null)
					{
						nextParamIndex = i;
						LoadActionField(action, fieldInfo, i);
						UsedIndices.Add(i);
						InitFields.Add(fieldInfo);
					}
				}
				for (int j = num; j < num2; j++)
				{
					if (UsedIndices.Contains(j))
					{
						continue;
					}
					FieldInfo fieldInfo2 = FindField(actionType, paramName[j]);
					if (fieldInfo2 != null)
					{
						nextParamIndex = j;
						if (TryConvertParameter(action, fieldInfo2, j))
						{
							UsedIndices.Add(j);
							InitFields.Add(fieldInfo2);
						}
					}
				}
			}
			FieldInfo[] fields = GetFields(actionType);
			foreach (FieldInfo fieldInfo3 in fields)
			{
				if (!InitFields.Contains(fieldInfo3))
				{
					LogInfo("New parameter: " + fieldInfo3.Name + " (set to default value).");
				}
			}
			return action;
		}

		private FieldInfo FindField(Type actionType, int paramIndex)
		{
			string text = paramName[paramIndex];
			ParamDataType paramDataType = this.paramDataType[paramIndex];
			FieldInfo[] fields = GetFields(actionType);
			foreach (FieldInfo fieldInfo in fields)
			{
				ParamDataType paramDataType2 = GetParamDataType(fieldInfo.FieldType);
				if (!(fieldInfo.Name == text) || paramDataType2 != paramDataType || InitFields.Contains(fieldInfo))
				{
					continue;
				}
				if (paramDataType2 == ParamDataType.Array)
				{
					Type elementType = fieldInfo.GetType().GetElementType();
					if (elementType == null)
					{
						return null;
					}
					if (arrayParamTypes[paramIndex] == elementType.FullName)
					{
						return fieldInfo;
					}
				}
				return fieldInfo;
			}
			return null;
		}

		private static FieldInfo FindField(Type actionType, string name)
		{
			FieldInfo[] fields = GetFields(actionType);
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.Name == name && !InitFields.Contains(fieldInfo))
				{
					return fieldInfo;
				}
			}
			return null;
		}

		private bool TryConvertParameter(FsmStateAction action, FieldInfo field, int paramIndex)
		{
			Type fieldType = field.FieldType;
			ParamDataType paramDataType = this.paramDataType[paramIndex];
			ParamDataType paramDataType2 = GetParamDataType(fieldType);
			if (paramDataType2 != ParamDataType.Array && paramDataType == paramDataType2)
			{
				LoadActionField(action, field, paramIndex);
			}
			if (paramDataType == ParamDataType.String && paramDataType2 == ParamDataType.FsmString)
			{
				LogInfo(field.Name + ": Upgraded from string to FsmString");
				field.SetValue(action, new FsmString
				{
					Value = FsmUtility.ByteArrayToString(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.Integer && paramDataType2 == ParamDataType.FsmInt)
			{
				LogInfo(field.Name + ": Upgraded from int to FsmInt");
				field.SetValue(action, new FsmInt
				{
					Value = FsmUtility.BitConverter.ToInt32(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.Float && paramDataType2 == ParamDataType.FsmFloat)
			{
				LogInfo(field.Name + ": Upgraded from float to FsmFloat");
				field.SetValue(action, new FsmFloat
				{
					Value = FsmUtility.BitConverter.ToSingle(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.Boolean && paramDataType2 == ParamDataType.FsmBool)
			{
				LogInfo(field.Name + ": Upgraded from bool to FsmBool");
				field.SetValue(action, new FsmBool
				{
					Value = FsmUtility.BitConverter.ToBoolean(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.GameObject && paramDataType2 == ParamDataType.FsmGameObject)
			{
				LogInfo(field.Name + ": Upgraded from from GameObject to FsmGameObject");
				field.SetValue(action, new FsmGameObject
				{
					Value = (GameObject)unityObjectParams[paramDataPos[paramIndex]]
				});
			}
			else if (paramDataType == ParamDataType.GameObject && paramDataType2 == ParamDataType.FsmOwnerDefault)
			{
				LogInfo(field.Name + ": Upgraded from GameObject to FsmOwnerDefault");
				FsmOwnerDefault fsmOwnerDefault = new FsmOwnerDefault();
				fsmOwnerDefault.GameObject = new FsmGameObject
				{
					Value = (GameObject)unityObjectParams[paramDataPos[paramIndex]]
				};
				FsmOwnerDefault value = fsmOwnerDefault;
				field.SetValue(action, value);
			}
			else if (paramDataType == ParamDataType.FsmGameObject && paramDataType2 == ParamDataType.FsmOwnerDefault)
			{
				LogInfo(field.Name + ": Converted from FsmGameObject to FsmOwnerDefault");
				FsmOwnerDefault fsmOwnerDefault2 = new FsmOwnerDefault();
				fsmOwnerDefault2.GameObject = fsmGameObjectParams[paramDataPos[paramIndex]];
				fsmOwnerDefault2.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
				FsmOwnerDefault value2 = fsmOwnerDefault2;
				field.SetValue(action, value2);
			}
			else if (paramDataType == ParamDataType.Vector3 && paramDataType2 == ParamDataType.FsmVector3)
			{
				LogInfo(field.Name + ": Upgraded from Vector3 to FsmVector3");
				field.SetValue(action, new FsmVector3
				{
					Value = FsmUtility.ByteArrayToVector3(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.Vector2 && paramDataType2 == ParamDataType.FsmVector2)
			{
				LogInfo(field.Name + ": Upgraded from Vector2 to FsmVector2");
				field.SetValue(action, new FsmVector2
				{
					Value = FsmUtility.ByteArrayToVector2(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.Rect && paramDataType2 == ParamDataType.FsmRect)
			{
				LogInfo(field.Name + ": Upgraded from Rect to FsmRect");
				field.SetValue(action, new FsmRect
				{
					Value = FsmUtility.ByteArrayToRect(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.Quaternion && paramDataType2 == ParamDataType.Quaternion)
			{
				LogInfo(field.Name + ": Upgraded from Quaternion to FsmQuaternion");
				field.SetValue(action, new FsmQuaternion
				{
					Value = FsmUtility.ByteArrayToQuaternion(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType == ParamDataType.Color && paramDataType2 == ParamDataType.FsmColor)
			{
				LogInfo(field.Name + ": Upgraded from Color to FsmColor");
				field.SetValue(action, new FsmColor
				{
					Value = FsmUtility.ByteArrayToColor(byteDataAsArray, paramDataPos[paramIndex])
				});
			}
			else if (paramDataType2 == ParamDataType.FsmMaterial && paramDataType == ParamDataType.ObjectReference)
			{
				LogInfo(field.Name + ": Upgraded from Material to FsmMaterial");
				field.SetValue(action, new FsmMaterial
				{
					Value = (unityObjectParams[paramDataPos[paramIndex]] as Material)
				});
			}
			else if (paramDataType2 == ParamDataType.FsmTexture && paramDataType == ParamDataType.ObjectReference)
			{
				LogInfo(field.Name + ": Upgraded from Texture to FsmTexture");
				field.SetValue(action, new FsmTexture
				{
					Value = (unityObjectParams[paramDataPos[paramIndex]] as Texture)
				});
			}
			else if (paramDataType2 == ParamDataType.FsmObject && paramDataType == ParamDataType.ObjectReference)
			{
				LogInfo(field.Name + ": Upgraded from Object to FsmObject");
				field.SetValue(action, new FsmObject
				{
					Value = unityObjectParams[paramDataPos[paramIndex]]
				});
			}
			else
			{
				if (paramDataType2 != ParamDataType.Array)
				{
					return false;
				}
				Type globalType = GetGlobalType(arrayParamTypes[paramDataPos[paramIndex]]);
				Type elementType = fieldType.GetElementType();
				if (elementType == null)
				{
					LogError("Could not make array: " + field.Name);
					return false;
				}
				int num = arrayParamSizes[paramDataPos[paramIndex]];
				Array array = Array.CreateInstance(elementType, num);
				if (globalType != elementType)
				{
					ParamDataType paramDataType3 = GetParamDataType(globalType);
					ParamDataType paramDataType4 = GetParamDataType(elementType);
					for (int i = 0; i < num; i++)
					{
						nextParamIndex++;
						if (!TryConvertArrayElement(array, paramDataType3, paramDataType4, i, nextParamIndex))
						{
							LogError(string.Concat("Failed to convert Array: ", field.Name, " From: ", paramDataType3, " To: ", paramDataType4));
							return false;
						}
						LogInfo(field.Name + ": Upgraded Array from " + globalType.FullName + " to " + paramDataType4);
					}
				}
				else
				{
					for (int j = 0; j < num; j++)
					{
						nextParamIndex++;
						LoadArrayElement(array, globalType, j, nextParamIndex);
					}
				}
				field.SetValue(action, array);
			}
			return true;
		}

		private bool TryConvertArrayElement(Array field, ParamDataType originalParamType, ParamDataType currentParamType, int elementIndex, int paramIndex)
		{
			if (elementIndex >= field.GetLength(0))
			{
				Debug.LogError("Bad array index: " + elementIndex);
				return false;
			}
			if (paramIndex >= paramDataPos.Count)
			{
				Debug.LogError("Bad param index: " + paramIndex);
				return false;
			}
			object obj = ConvertType(originalParamType, currentParamType, paramIndex);
			if (obj == null)
			{
				return false;
			}
			field.SetValue(obj, elementIndex);
			return true;
		}

		private object ConvertType(ParamDataType originalParamType, ParamDataType currentParamType, int paramIndex)
		{
			if (originalParamType == ParamDataType.String && currentParamType == ParamDataType.FsmString)
			{
				FsmString fsmString = new FsmString();
				fsmString.Value = FsmUtility.ByteArrayToString(byteDataAsArray, paramDataPos[paramIndex], paramByteDataSize[paramIndex]);
				return fsmString;
			}
			if (originalParamType == ParamDataType.Integer && currentParamType == ParamDataType.FsmInt)
			{
				FsmInt fsmInt = new FsmInt();
				fsmInt.Value = FsmUtility.BitConverter.ToInt32(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmInt;
			}
			if (originalParamType == ParamDataType.Float && currentParamType == ParamDataType.FsmFloat)
			{
				FsmFloat fsmFloat = new FsmFloat();
				fsmFloat.Value = FsmUtility.BitConverter.ToSingle(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmFloat;
			}
			if (originalParamType == ParamDataType.Boolean && currentParamType == ParamDataType.FsmBool)
			{
				FsmBool fsmBool = new FsmBool();
				fsmBool.Value = FsmUtility.BitConverter.ToBoolean(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmBool;
			}
			if (originalParamType == ParamDataType.GameObject && currentParamType == ParamDataType.FsmGameObject)
			{
				FsmGameObject fsmGameObject = new FsmGameObject();
				fsmGameObject.Value = (GameObject)unityObjectParams[paramDataPos[paramIndex]];
				return fsmGameObject;
			}
			if (originalParamType == ParamDataType.GameObject && currentParamType == ParamDataType.FsmOwnerDefault)
			{
				FsmOwnerDefault fsmOwnerDefault = new FsmOwnerDefault();
				fsmOwnerDefault.GameObject = new FsmGameObject
				{
					Value = (GameObject)unityObjectParams[paramDataPos[paramIndex]]
				};
				fsmOwnerDefault.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
				return fsmOwnerDefault;
			}
			if (originalParamType == ParamDataType.FsmGameObject && currentParamType == ParamDataType.FsmOwnerDefault)
			{
				FsmOwnerDefault fsmOwnerDefault2 = new FsmOwnerDefault();
				fsmOwnerDefault2.GameObject = fsmGameObjectParams[paramDataPos[paramIndex]];
				fsmOwnerDefault2.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
				return fsmOwnerDefault2;
			}
			if (originalParamType == ParamDataType.Vector2 && currentParamType == ParamDataType.FsmVector2)
			{
				FsmVector2 fsmVector = new FsmVector2();
				fsmVector.Value = FsmUtility.ByteArrayToVector2(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmVector;
			}
			if (originalParamType == ParamDataType.Vector3 && currentParamType == ParamDataType.FsmVector3)
			{
				FsmVector3 fsmVector2 = new FsmVector3();
				fsmVector2.Value = FsmUtility.ByteArrayToVector3(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmVector2;
			}
			if (originalParamType == ParamDataType.Rect && currentParamType == ParamDataType.FsmRect)
			{
				FsmRect fsmRect = new FsmRect();
				fsmRect.Value = FsmUtility.ByteArrayToRect(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmRect;
			}
			if (originalParamType == ParamDataType.Quaternion && currentParamType == ParamDataType.Quaternion)
			{
				FsmQuaternion fsmQuaternion = new FsmQuaternion();
				fsmQuaternion.Value = FsmUtility.ByteArrayToQuaternion(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmQuaternion;
			}
			if (originalParamType == ParamDataType.Color && currentParamType == ParamDataType.FsmColor)
			{
				FsmColor fsmColor = new FsmColor();
				fsmColor.Value = FsmUtility.ByteArrayToColor(byteDataAsArray, paramDataPos[paramIndex]);
				return fsmColor;
			}
			if (currentParamType == ParamDataType.FsmMaterial && originalParamType == ParamDataType.ObjectReference)
			{
				FsmMaterial fsmMaterial = new FsmMaterial();
				fsmMaterial.Value = unityObjectParams[paramDataPos[paramIndex]] as Material;
				return fsmMaterial;
			}
			if (currentParamType == ParamDataType.FsmTexture && originalParamType == ParamDataType.ObjectReference)
			{
				FsmTexture fsmTexture = new FsmTexture();
				fsmTexture.Value = unityObjectParams[paramDataPos[paramIndex]] as Texture;
				return fsmTexture;
			}
			if (currentParamType == ParamDataType.FsmObject && originalParamType == ParamDataType.ObjectReference)
			{
				FsmObject fsmObject = new FsmObject();
				fsmObject.Value = unityObjectParams[paramDataPos[paramIndex]];
				return fsmObject;
			}
			return null;
		}

		public void SaveActions(FsmStateAction[] actions)
		{
			ClearActionData();
			foreach (FsmStateAction action in actions)
			{
				SaveAction(action);
			}
		}

		private void SaveAction(FsmStateAction action)
		{
			if (action != null)
			{
				Type type = action.GetType();
				actionNames.Add(type.ToString());
				customNames.Add(action.Name);
				actionEnabled.Add(action.Enabled);
				actionIsOpen.Add(action.IsOpen);
				actionStartIndex.Add(nextParamIndex);
				actionHashCodes.Add(GetActionTypeHashCode(type));
				FieldInfo[] fields = GetFields(type);
				FieldInfo[] array = fields;
				foreach (FieldInfo fieldInfo in array)
				{
					Type fieldType = fieldInfo.FieldType;
					object value = fieldInfo.GetValue(action);
					paramName.Add(fieldInfo.Name);
					SaveActionField(fieldType, value);
					nextParamIndex++;
				}
			}
		}

		private void SaveActionField(Type fieldType, object obj)
		{
			if (fieldType == typeof(FsmAnimationCurve))
			{
				paramDataType.Add(ParamDataType.FsmAnimationCurve);
				paramDataPos.Add(animationCurveParams.Count);
				paramByteDataSize.Add(0);
				animationCurveParams.Add((FsmAnimationCurve)obj);
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(fieldType))
			{
				paramDataType.Add((fieldType == typeof(GameObject)) ? ParamDataType.GameObject : ParamDataType.ObjectReference);
				paramDataPos.Add(unityObjectParams.Count);
				paramByteDataSize.Add(0);
				unityObjectParams.Add((UnityEngine.Object)obj);
			}
			else if (fieldType == typeof(FunctionCall))
			{
				paramDataType.Add(ParamDataType.FunctionCall);
				paramDataPos.Add(functionCallParams.Count);
				paramByteDataSize.Add(0);
				functionCallParams.Add((FunctionCall)obj);
			}
			else if (fieldType == typeof(FsmProperty))
			{
				paramDataType.Add(ParamDataType.FsmProperty);
				paramDataPos.Add(fsmPropertyParams.Count);
				paramByteDataSize.Add(0);
				fsmPropertyParams.Add((FsmProperty)obj);
			}
			else if (fieldType == typeof(FsmEventTarget))
			{
				paramDataType.Add(ParamDataType.FsmEventTarget);
				paramDataPos.Add(fsmEventTargetParams.Count);
				paramByteDataSize.Add(0);
				fsmEventTargetParams.Add((FsmEventTarget)obj);
			}
			else if (fieldType == typeof(LayoutOption))
			{
				paramDataType.Add(ParamDataType.LayoutOption);
				paramDataPos.Add(layoutOptionParams.Count);
				paramByteDataSize.Add(0);
				layoutOptionParams.Add((LayoutOption)obj);
			}
			else if (fieldType == typeof(FsmGameObject))
			{
				paramDataType.Add(ParamDataType.FsmGameObject);
				paramDataPos.Add(fsmGameObjectParams.Count);
				paramByteDataSize.Add(0);
				fsmGameObjectParams.Add((FsmGameObject)obj);
			}
			else if (fieldType == typeof(FsmOwnerDefault))
			{
				paramDataType.Add(ParamDataType.FsmOwnerDefault);
				paramDataPos.Add(fsmOwnerDefaultParams.Count);
				paramByteDataSize.Add(0);
				fsmOwnerDefaultParams.Add((FsmOwnerDefault)obj);
			}
			else if (fieldType == typeof(FsmString))
			{
				paramDataType.Add(ParamDataType.FsmString);
				paramDataPos.Add(fsmStringParams.Count);
				paramByteDataSize.Add(0);
				fsmStringParams.Add((FsmString)obj);
			}
			else
			{
				if (fieldType.IsArray)
				{
					Type elementType = fieldType.GetElementType();
					if (elementType == null)
					{
						return;
					}
					Array array = ((obj == null) ? Array.CreateInstance(elementType, 0) : ((Array)obj));
					paramDataType.Add(ParamDataType.Array);
					paramDataPos.Add(arrayParamSizes.Count);
					paramByteDataSize.Add(0);
					arrayParamSizes.Add(array.Length);
					arrayParamTypes.Add(elementType.FullName);
					{
						foreach (object item in array)
						{
							nextParamIndex++;
							paramName.Add("");
							SaveActionField(elementType, item);
						}
						return;
					}
				}
				if (fieldType == typeof(float))
				{
					paramDataType.Add(ParamDataType.Float);
					AddByteData(FsmUtility.BitConverter.GetBytes((float)obj));
				}
				else if (fieldType == typeof(int))
				{
					paramDataType.Add(ParamDataType.Integer);
					AddByteData(FsmUtility.BitConverter.GetBytes((int)obj));
				}
				else if (fieldType == typeof(bool))
				{
					paramDataType.Add(ParamDataType.Boolean);
					AddByteData(FsmUtility.BitConverter.GetBytes((bool)obj));
				}
				else if (fieldType == typeof(Color))
				{
					paramDataType.Add(ParamDataType.Color);
					AddByteData(FsmUtility.ColorToByteArray((Color)obj));
				}
				else if (fieldType == typeof(Vector2))
				{
					paramDataType.Add(ParamDataType.Vector2);
					AddByteData(FsmUtility.Vector2ToByteArray((Vector2)obj));
				}
				else if (fieldType == typeof(Vector3))
				{
					paramDataType.Add(ParamDataType.Vector3);
					AddByteData(FsmUtility.Vector3ToByteArray((Vector3)obj));
				}
				else if (fieldType == typeof(Vector4))
				{
					paramDataType.Add(ParamDataType.Vector4);
					AddByteData(FsmUtility.Vector4ToByteArray((Vector4)obj));
				}
				else if (fieldType == typeof(Rect))
				{
					paramDataType.Add(ParamDataType.Rect);
					AddByteData(FsmUtility.RectToByteArray((Rect)obj));
				}
				else if (fieldType == typeof(FsmFloat))
				{
					paramDataType.Add(ParamDataType.FsmFloat);
					AddByteData(FsmUtility.FsmFloatToByteArray((FsmFloat)obj));
				}
				else if (fieldType == typeof(FsmInt))
				{
					paramDataType.Add(ParamDataType.FsmInt);
					AddByteData(FsmUtility.FsmIntToByteArray((FsmInt)obj));
				}
				else if (fieldType == typeof(FsmBool))
				{
					paramDataType.Add(ParamDataType.FsmBool);
					AddByteData(FsmUtility.FsmBoolToByteArray((FsmBool)obj));
				}
				else if (fieldType == typeof(FsmVector2))
				{
					paramDataType.Add(ParamDataType.FsmVector2);
					AddByteData(FsmUtility.FsmVector2ToByteArray((FsmVector2)obj));
				}
				else if (fieldType == typeof(FsmVector3))
				{
					paramDataType.Add(ParamDataType.FsmVector3);
					AddByteData(FsmUtility.FsmVector3ToByteArray((FsmVector3)obj));
				}
				else if (fieldType == typeof(FsmRect))
				{
					paramDataType.Add(ParamDataType.FsmRect);
					AddByteData(FsmUtility.FsmRectToByteArray((FsmRect)obj));
				}
				else if (fieldType == typeof(FsmQuaternion))
				{
					paramDataType.Add(ParamDataType.FsmQuaternion);
					AddByteData(FsmUtility.FsmQuaternionToByteArray((FsmQuaternion)obj));
				}
				else if (fieldType == typeof(FsmColor))
				{
					paramDataType.Add(ParamDataType.FsmColor);
					AddByteData(FsmUtility.FsmColorToByteArray((FsmColor)obj));
				}
				else if (fieldType == typeof(FsmEvent))
				{
					paramDataType.Add(ParamDataType.FsmEvent);
					AddByteData(FsmUtility.FsmEventToByteArray((FsmEvent)obj));
				}
				else if (fieldType == typeof(string))
				{
					paramDataType.Add(ParamDataType.String);
					AddByteData(FsmUtility.StringToByteArray((string)obj));
				}
				else if (fieldType == typeof(FsmObject))
				{
					paramDataType.Add(ParamDataType.FsmObject);
					paramDataPos.Add(fsmObjectParams.Count);
					paramByteDataSize.Add(0);
					fsmObjectParams.Add((FsmObject)obj);
				}
				else if (fieldType == typeof(FsmMaterial))
				{
					paramDataType.Add(ParamDataType.FsmMaterial);
					paramDataPos.Add(fsmObjectParams.Count);
					paramByteDataSize.Add(0);
					fsmObjectParams.Add((FsmObject)obj);
				}
				else if (fieldType == typeof(FsmTexture))
				{
					paramDataType.Add(ParamDataType.FsmTexture);
					paramDataPos.Add(fsmObjectParams.Count);
					paramByteDataSize.Add(0);
					fsmObjectParams.Add((FsmObject)obj);
				}
				else if (fieldType.IsEnum)
				{
					paramDataType.Add(ParamDataType.Enum);
					AddByteData(FsmUtility.BitConverter.GetBytes((int)obj));
				}
				else if (obj != null)
				{
					Debug.LogError("Save Action: Unsupported parameter type: " + fieldType);
					paramDataType.Add(ParamDataType.Unsupported);
					paramDataPos.Add(byteData.Count);
					paramByteDataSize.Add(0);
				}
				else
				{
					paramDataType.Add(ParamDataType.Unsupported);
					paramDataPos.Add(byteData.Count);
					paramByteDataSize.Add(0);
				}
			}
		}

		private void AddByteData(ICollection<byte> bytes)
		{
			paramDataPos.Add(byteData.Count);
			if (bytes != null)
			{
				paramByteDataSize.Add(bytes.Count);
				byteData.AddRange(bytes);
			}
			else
			{
				paramByteDataSize.Add(0);
			}
		}

		private static ParamDataType GetParamDataType(Type type)
		{
			if (type == typeof(int))
			{
				return ParamDataType.Integer;
			}
			if (type == typeof(bool))
			{
				return ParamDataType.Boolean;
			}
			if (type == typeof(float))
			{
				return ParamDataType.Float;
			}
			if (type == typeof(string))
			{
				return ParamDataType.String;
			}
			if (type == typeof(Color))
			{
				return ParamDataType.Color;
			}
			if (type == typeof(LayerMask))
			{
				return ParamDataType.LayerMask;
			}
			if (type == typeof(Vector2))
			{
				return ParamDataType.Vector2;
			}
			if (type == typeof(Vector3))
			{
				return ParamDataType.Vector3;
			}
			if (type == typeof(Vector4))
			{
				return ParamDataType.Vector4;
			}
			if (type == typeof(Quaternion))
			{
				return ParamDataType.Quaternion;
			}
			if (type == typeof(Rect))
			{
				return ParamDataType.Rect;
			}
			if (type == typeof(AnimationCurve))
			{
				return ParamDataType.AnimationCurve;
			}
			if (type == typeof(FsmFloat))
			{
				return ParamDataType.FsmFloat;
			}
			if (type == typeof(FsmInt))
			{
				return ParamDataType.FsmInt;
			}
			if (type == typeof(FsmBool))
			{
				return ParamDataType.FsmBool;
			}
			if (type == typeof(FsmString))
			{
				return ParamDataType.FsmString;
			}
			if (type == typeof(FsmGameObject))
			{
				return ParamDataType.FsmGameObject;
			}
			if (type == typeof(FsmOwnerDefault))
			{
				return ParamDataType.FsmOwnerDefault;
			}
			if (type == typeof(FunctionCall))
			{
				return ParamDataType.FunctionCall;
			}
			if (type == typeof(FsmProperty))
			{
				return ParamDataType.FsmProperty;
			}
			if (type == typeof(FsmAnimationCurve))
			{
				return ParamDataType.FsmAnimationCurve;
			}
			if (type == typeof(FsmEvent))
			{
				return ParamDataType.FsmEvent;
			}
			if (type == typeof(GameObject))
			{
				return ParamDataType.GameObject;
			}
			if (type == typeof(FsmVector2))
			{
				return ParamDataType.FsmVector2;
			}
			if (type == typeof(FsmVector3))
			{
				return ParamDataType.FsmVector3;
			}
			if (type == typeof(FsmRect))
			{
				return ParamDataType.FsmRect;
			}
			if (type == typeof(FsmQuaternion))
			{
				return ParamDataType.FsmQuaternion;
			}
			if (type == typeof(FsmObject))
			{
				return ParamDataType.FsmObject;
			}
			if (type == typeof(FsmMaterial))
			{
				return ParamDataType.FsmMaterial;
			}
			if (type == typeof(FsmTexture))
			{
				return ParamDataType.FsmTexture;
			}
			if (type == typeof(FsmColor))
			{
				return ParamDataType.FsmColor;
			}
			if (type == typeof(LayoutOption))
			{
				return ParamDataType.LayoutOption;
			}
			if (type == typeof(FsmEventTarget))
			{
				return ParamDataType.FsmEventTarget;
			}
			if (type.IsArray)
			{
				return ParamDataType.Array;
			}
			if (type.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				return ParamDataType.ObjectReference;
			}
			if (type.IsEnum)
			{
				return ParamDataType.Enum;
			}
			return ParamDataType.Unsupported;
		}
	}
}
