using System;
using UnityEngine;

namespace HutongGames.PlayMaker
{
	[Serializable]
	public class NamedVariable : INameable, INamedVariable, IComparable
	{
		[SerializeField]
		private bool useVariable;

		[SerializeField]
		private string name;

		[SerializeField]
		private string tooltip = "";

		[SerializeField]
		private bool showInInspector;

		[SerializeField]
		private bool networkSync;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public string Tooltip
		{
			get
			{
				return tooltip;
			}
			set
			{
				tooltip = value;
			}
		}

		public bool UseVariable
		{
			get
			{
				return useVariable;
			}
			set
			{
				useVariable = value;
			}
		}

		public bool ShowInInspector
		{
			get
			{
				return showInInspector;
			}
			set
			{
				showInInspector = value;
			}
		}

		public bool NetworkSync
		{
			get
			{
				return networkSync;
			}
			set
			{
				networkSync = value;
			}
		}

		public bool IsNone
		{
			get
			{
				if (useVariable)
				{
					return string.IsNullOrEmpty(name);
				}
				return false;
			}
		}

		public NamedVariable()
		{
			name = "";
			tooltip = "";
		}

		public NamedVariable(string name)
		{
			this.name = name;
			if (!string.IsNullOrEmpty(name))
			{
				useVariable = true;
			}
		}

		public NamedVariable(NamedVariable source)
		{
			if (source != null)
			{
				useVariable = source.useVariable;
				name = source.name;
				showInInspector = source.showInInspector;
				tooltip = source.tooltip;
				networkSync = source.networkSync;
			}
		}

		public int CompareTo(object obj)
		{
			NamedVariable namedVariable = obj as NamedVariable;
			if (namedVariable == null)
			{
				return 0;
			}
			return string.CompareOrdinal(name, namedVariable.name);
		}
	}
}
