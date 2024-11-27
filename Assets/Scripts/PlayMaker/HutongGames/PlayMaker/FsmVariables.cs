using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class FsmVariables
	{
		[SerializeField]
		private FsmFloat[] floatVariables = new FsmFloat[0];

		[SerializeField]
		private FsmInt[] intVariables = new FsmInt[0];

		[SerializeField]
		private FsmBool[] boolVariables = new FsmBool[0];

		[SerializeField]
		private FsmString[] stringVariables = new FsmString[0];

		[SerializeField]
		private FsmVector2[] vector2Variables = new FsmVector2[0];

		[SerializeField]
		private FsmVector3[] vector3Variables = new FsmVector3[0];

		[SerializeField]
		private FsmColor[] colorVariables = new FsmColor[0];

		[SerializeField]
		private FsmRect[] rectVariables = new FsmRect[0];

		[SerializeField]
		private FsmQuaternion[] quaternionVariables = new FsmQuaternion[0];

		[SerializeField]
		private FsmGameObject[] gameObjectVariables = new FsmGameObject[0];

		[SerializeField]
		private FsmObject[] objectVariables = new FsmObject[0];

		[SerializeField]
		private FsmMaterial[] materialVariables = new FsmMaterial[0];

		[SerializeField]
		private FsmTexture[] textureVariables = new FsmTexture[0];

		public static PlayMakerGlobals GlobalsComponent
		{
			get
			{
				return PlayMakerGlobals.Instance;
			}
		}

		public static FsmVariables GlobalVariables
		{
			get
			{
				return PlayMakerGlobals.Instance.Variables;
			}
		}

		public static bool GlobalVariablesSynced { get; set; }

		public FsmFloat[] FloatVariables
		{
			get
			{
				return floatVariables;
			}
			set
			{
				floatVariables = value;
			}
		}

		public FsmInt[] IntVariables
		{
			get
			{
				return intVariables;
			}
			set
			{
				intVariables = value;
			}
		}

		public FsmBool[] BoolVariables
		{
			get
			{
				return boolVariables;
			}
			set
			{
				boolVariables = value;
			}
		}

		public FsmString[] StringVariables
		{
			get
			{
				return stringVariables;
			}
			set
			{
				stringVariables = value;
			}
		}

		public FsmVector2[] Vector2Variables
		{
			get
			{
				return vector2Variables;
			}
			set
			{
				vector2Variables = value;
			}
		}

		public FsmVector3[] Vector3Variables
		{
			get
			{
				return vector3Variables;
			}
			set
			{
				vector3Variables = value;
			}
		}

		public FsmRect[] RectVariables
		{
			get
			{
				return rectVariables;
			}
			set
			{
				rectVariables = value;
			}
		}

		public FsmQuaternion[] QuaternionVariables
		{
			get
			{
				return quaternionVariables;
			}
			set
			{
				quaternionVariables = value;
			}
		}

		public FsmColor[] ColorVariables
		{
			get
			{
				return colorVariables;
			}
			set
			{
				colorVariables = value;
			}
		}

		public FsmGameObject[] GameObjectVariables
		{
			get
			{
				return gameObjectVariables;
			}
			set
			{
				gameObjectVariables = value;
			}
		}

		public FsmObject[] ObjectVariables
		{
			get
			{
				return objectVariables;
			}
			set
			{
				objectVariables = value;
			}
		}

		public FsmMaterial[] MaterialVariables
		{
			get
			{
				return materialVariables;
			}
			set
			{
				materialVariables = value;
			}
		}

		public FsmTexture[] TextureVariables
		{
			get
			{
				return textureVariables;
			}
			set
			{
				textureVariables = value;
			}
		}

		public NamedVariable[] GetNames(Type ofType)
		{
			if (ofType == typeof(FsmFloat))
			{
				return floatVariables;
			}
			if (ofType == typeof(FsmInt))
			{
				return intVariables;
			}
			if (ofType == typeof(FsmBool))
			{
				return boolVariables;
			}
			if (ofType == typeof(FsmString))
			{
				return stringVariables;
			}
			if (ofType == typeof(FsmVector2))
			{
				return vector2Variables;
			}
			if (ofType == typeof(FsmVector3))
			{
				return vector3Variables;
			}
			if (ofType == typeof(FsmRect))
			{
				return rectVariables;
			}
			if (ofType == typeof(FsmQuaternion))
			{
				return quaternionVariables;
			}
			if (ofType == typeof(FsmObject))
			{
				return objectVariables;
			}
			if (ofType == typeof(FsmMaterial))
			{
				return materialVariables;
			}
			if (ofType == typeof(FsmTexture))
			{
				return textureVariables;
			}
			if (ofType == typeof(FsmColor))
			{
				return colorVariables;
			}
			if (ofType == typeof(FsmGameObject))
			{
				return gameObjectVariables;
			}
			return new NamedVariable[0];
		}

		public FsmVariables()
		{
		}

		public FsmVariables(FsmVariables source)
		{
			if (source != null)
			{
				floatVariables = new FsmFloat[source.floatVariables.Length];
				for (int i = 0; i < source.floatVariables.Length; i++)
				{
					floatVariables[i] = new FsmFloat(source.floatVariables[i]);
				}
				intVariables = new FsmInt[source.intVariables.Length];
				for (int j = 0; j < source.intVariables.Length; j++)
				{
					intVariables[j] = new FsmInt(source.intVariables[j]);
				}
				boolVariables = new FsmBool[source.boolVariables.Length];
				for (int k = 0; k < source.boolVariables.Length; k++)
				{
					boolVariables[k] = new FsmBool(source.boolVariables[k]);
				}
				gameObjectVariables = new FsmGameObject[source.gameObjectVariables.Length];
				for (int l = 0; l < source.gameObjectVariables.Length; l++)
				{
					gameObjectVariables[l] = new FsmGameObject(source.gameObjectVariables[l]);
				}
				colorVariables = new FsmColor[source.colorVariables.Length];
				for (int m = 0; m < source.colorVariables.Length; m++)
				{
					colorVariables[m] = new FsmColor(source.colorVariables[m]);
				}
				vector2Variables = new FsmVector2[source.vector2Variables.Length];
				for (int n = 0; n < source.vector2Variables.Length; n++)
				{
					vector2Variables[n] = new FsmVector2(source.vector2Variables[n]);
				}
				vector3Variables = new FsmVector3[source.vector3Variables.Length];
				for (int num = 0; num < source.vector3Variables.Length; num++)
				{
					vector3Variables[num] = new FsmVector3(source.vector3Variables[num]);
				}
				rectVariables = new FsmRect[source.rectVariables.Length];
				for (int num2 = 0; num2 < source.rectVariables.Length; num2++)
				{
					rectVariables[num2] = new FsmRect(source.rectVariables[num2]);
				}
				quaternionVariables = new FsmQuaternion[source.quaternionVariables.Length];
				for (int num3 = 0; num3 < source.quaternionVariables.Length; num3++)
				{
					quaternionVariables[num3] = new FsmQuaternion(source.quaternionVariables[num3]);
				}
				objectVariables = new FsmObject[source.objectVariables.Length];
				for (int num4 = 0; num4 < source.objectVariables.Length; num4++)
				{
					objectVariables[num4] = new FsmObject(source.objectVariables[num4]);
				}
				materialVariables = new FsmMaterial[source.materialVariables.Length];
				for (int num5 = 0; num5 < source.materialVariables.Length; num5++)
				{
					materialVariables[num5] = new FsmMaterial(source.materialVariables[num5]);
				}
				textureVariables = new FsmTexture[source.textureVariables.Length];
				for (int num6 = 0; num6 < source.textureVariables.Length; num6++)
				{
					textureVariables[num6] = new FsmTexture(source.textureVariables[num6]);
				}
				stringVariables = new FsmString[source.stringVariables.Length];
				for (int num7 = 0; num7 < source.stringVariables.Length; num7++)
				{
					stringVariables[num7] = new FsmString(source.stringVariables[num7]);
				}
			}
		}

		public void ApplyVariableValues(FsmVariables source)
		{
			for (int i = 0; i < source.floatVariables.Length; i++)
			{
				floatVariables[i].Value = source.floatVariables[i].Value;
			}
			for (int j = 0; j < source.intVariables.Length; j++)
			{
				intVariables[j].Value = source.intVariables[j].Value;
			}
			for (int k = 0; k < source.boolVariables.Length; k++)
			{
				boolVariables[k].Value = source.boolVariables[k].Value;
			}
			for (int l = 0; l < source.gameObjectVariables.Length; l++)
			{
				gameObjectVariables[l].Value = source.gameObjectVariables[l].Value;
			}
			for (int m = 0; m < source.colorVariables.Length; m++)
			{
				colorVariables[m].Value = source.colorVariables[m].Value;
			}
			for (int n = 0; n < source.vector3Variables.Length; n++)
			{
				vector3Variables[n].Value = source.vector3Variables[n].Value;
			}
			for (int num = 0; num < source.rectVariables.Length; num++)
			{
				rectVariables[num].Value = source.rectVariables[num].Value;
			}
			for (int num2 = 0; num2 < source.quaternionVariables.Length; num2++)
			{
				quaternionVariables[num2].Value = source.quaternionVariables[num2].Value;
			}
			for (int num3 = 0; num3 < source.objectVariables.Length; num3++)
			{
				objectVariables[num3].Value = source.objectVariables[num3].Value;
			}
			for (int num4 = 0; num4 < source.materialVariables.Length; num4++)
			{
				materialVariables[num4].Value = source.materialVariables[num4].Value;
			}
			for (int num5 = 0; num5 < source.textureVariables.Length; num5++)
			{
				textureVariables[num5].Value = source.textureVariables[num5].Value;
			}
			for (int num6 = 0; num6 < source.stringVariables.Length; num6++)
			{
				stringVariables[num6].Value = source.stringVariables[num6].Value;
			}
		}

		public FsmFloat GetFsmFloat(string name)
		{
			FsmFloat[] array = floatVariables;
			foreach (FsmFloat fsmFloat in array)
			{
				if (fsmFloat.Name == name)
				{
					return fsmFloat;
				}
			}
			if (GlobalVariables != null)
			{
				FsmFloat[] array2 = GlobalVariables.floatVariables;
				foreach (FsmFloat fsmFloat2 in array2)
				{
					if (fsmFloat2.Name == name)
					{
						return fsmFloat2;
					}
				}
			}
			return new FsmFloat(name);
		}

		public FsmObject GetFsmObject(string name)
		{
			FsmObject[] array = objectVariables;
			foreach (FsmObject fsmObject in array)
			{
				if (fsmObject.Name == name)
				{
					return fsmObject;
				}
			}
			if (GlobalVariables != null)
			{
				FsmObject[] array2 = GlobalVariables.objectVariables;
				foreach (FsmObject fsmObject2 in array2)
				{
					if (fsmObject2.Name == name)
					{
						return fsmObject2;
					}
				}
			}
			return new FsmObject(name);
		}

		public FsmMaterial GetFsmMaterial(string name)
		{
			FsmMaterial[] array = materialVariables;
			foreach (FsmMaterial fsmMaterial in array)
			{
				if (fsmMaterial.Name == name)
				{
					return fsmMaterial;
				}
			}
			if (GlobalVariables != null)
			{
				FsmMaterial[] array2 = GlobalVariables.materialVariables;
				foreach (FsmMaterial fsmMaterial2 in array2)
				{
					if (fsmMaterial2.Name == name)
					{
						return fsmMaterial2;
					}
				}
			}
			return new FsmMaterial(name);
		}

		public FsmTexture GetFsmTexture(string name)
		{
			FsmTexture[] array = textureVariables;
			foreach (FsmTexture fsmTexture in array)
			{
				if (fsmTexture.Name == name)
				{
					return fsmTexture;
				}
			}
			if (GlobalVariables != null)
			{
				FsmTexture[] array2 = GlobalVariables.textureVariables;
				foreach (FsmTexture fsmTexture2 in array2)
				{
					if (fsmTexture2.Name == name)
					{
						return fsmTexture2;
					}
				}
			}
			return new FsmTexture(name);
		}

		public FsmInt GetFsmInt(string name)
		{
			FsmInt[] array = intVariables;
			foreach (FsmInt fsmInt in array)
			{
				if (fsmInt.Name == name)
				{
					return fsmInt;
				}
			}
			if (GlobalVariables != null)
			{
				FsmInt[] array2 = GlobalVariables.intVariables;
				foreach (FsmInt fsmInt2 in array2)
				{
					if (fsmInt2.Name == name)
					{
						return fsmInt2;
					}
				}
			}
			return new FsmInt(name);
		}

		public FsmBool GetFsmBool(string name)
		{
			FsmBool[] array = boolVariables;
			foreach (FsmBool fsmBool in array)
			{
				if (fsmBool.Name == name)
				{
					return fsmBool;
				}
			}
			if (GlobalVariables != null)
			{
				FsmBool[] array2 = GlobalVariables.boolVariables;
				foreach (FsmBool fsmBool2 in array2)
				{
					if (fsmBool2.Name == name)
					{
						return fsmBool2;
					}
				}
			}
			return new FsmBool(name);
		}

		public FsmString GetFsmString(string name)
		{
			FsmString[] array = stringVariables;
			foreach (FsmString fsmString in array)
			{
				if (fsmString.Name == name)
				{
					return fsmString;
				}
			}
			if (GlobalVariables != null)
			{
				FsmString[] array2 = GlobalVariables.stringVariables;
				foreach (FsmString fsmString2 in array2)
				{
					if (fsmString2.Name == name)
					{
						return fsmString2;
					}
				}
			}
			return new FsmString(name);
		}

		public FsmVector2 GetFsmVector2(string name)
		{
			FsmVector2[] array = vector2Variables;
			foreach (FsmVector2 fsmVector in array)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			if (GlobalVariables != null)
			{
				FsmVector2[] array2 = GlobalVariables.vector2Variables;
				foreach (FsmVector2 fsmVector2 in array2)
				{
					if (fsmVector2.Name == name)
					{
						return fsmVector2;
					}
				}
			}
			return new FsmVector2(name);
		}

		public FsmVector3 GetFsmVector3(string name)
		{
			FsmVector3[] array = vector3Variables;
			foreach (FsmVector3 fsmVector in array)
			{
				if (fsmVector.Name == name)
				{
					return fsmVector;
				}
			}
			if (GlobalVariables != null)
			{
				FsmVector3[] array2 = GlobalVariables.vector3Variables;
				foreach (FsmVector3 fsmVector2 in array2)
				{
					if (fsmVector2.Name == name)
					{
						return fsmVector2;
					}
				}
			}
			return new FsmVector3(name);
		}

		public FsmRect GetFsmRect(string name)
		{
			FsmRect[] array = rectVariables;
			foreach (FsmRect fsmRect in array)
			{
				if (fsmRect.Name == name)
				{
					return fsmRect;
				}
			}
			if (GlobalVariables != null)
			{
				FsmRect[] array2 = GlobalVariables.rectVariables;
				foreach (FsmRect fsmRect2 in array2)
				{
					if (fsmRect2.Name == name)
					{
						return fsmRect2;
					}
				}
			}
			return new FsmRect(name);
		}

		public FsmQuaternion GetFsmQuaternion(string name)
		{
			FsmQuaternion[] array = quaternionVariables;
			foreach (FsmQuaternion fsmQuaternion in array)
			{
				if (fsmQuaternion.Name == name)
				{
					return fsmQuaternion;
				}
			}
			if (GlobalVariables != null)
			{
				FsmQuaternion[] array2 = GlobalVariables.quaternionVariables;
				foreach (FsmQuaternion fsmQuaternion2 in array2)
				{
					if (fsmQuaternion2.Name == name)
					{
						return fsmQuaternion2;
					}
				}
			}
			return new FsmQuaternion(name);
		}

		public FsmColor GetFsmColor(string name)
		{
			FsmColor[] array = colorVariables;
			foreach (FsmColor fsmColor in array)
			{
				if (fsmColor.Name == name)
				{
					return fsmColor;
				}
			}
			if (GlobalVariables != null)
			{
				FsmColor[] array2 = GlobalVariables.colorVariables;
				foreach (FsmColor fsmColor2 in array2)
				{
					if (fsmColor2.Name == name)
					{
						return fsmColor2;
					}
				}
			}
			return new FsmColor(name);
		}

		public FsmGameObject GetFsmGameObject(string name)
		{
			FsmGameObject[] array = gameObjectVariables;
			foreach (FsmGameObject fsmGameObject in array)
			{
				if (fsmGameObject.Name == name)
				{
					return fsmGameObject;
				}
			}
			if (GlobalVariables != null)
			{
				FsmGameObject[] array2 = GlobalVariables.gameObjectVariables;
				foreach (FsmGameObject fsmGameObject2 in array2)
				{
					if (fsmGameObject2.Name == name)
					{
						return fsmGameObject2;
					}
				}
			}
			return new FsmGameObject(name);
		}
	}
}
