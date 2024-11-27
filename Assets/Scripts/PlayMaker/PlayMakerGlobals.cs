using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

public class PlayMakerGlobals : ScriptableObject
{
	private static PlayMakerGlobals instance;

	[SerializeField]
	private FsmVariables variables = new FsmVariables();

	[SerializeField]
	private List<string> events = new List<string>();

	public static PlayMakerGlobals Instance
	{
		get
		{
			if (instance == null)
			{
				Object @object = Resources.Load("PlayMakerGlobals", typeof(PlayMakerGlobals));
				if (@object != null)
				{
					if (Application.isPlaying || Application.isLoadingLevel)
					{
						PlayMakerGlobals playMakerGlobals = (PlayMakerGlobals)@object;
						instance = ScriptableObject.CreateInstance<PlayMakerGlobals>();
						instance.Variables = new FsmVariables(playMakerGlobals.variables);
						instance.Events = new List<string>(playMakerGlobals.Events);
					}
					else
					{
						instance = @object as PlayMakerGlobals;
					}
				}
				else
				{
					instance = ScriptableObject.CreateInstance<PlayMakerGlobals>();
				}
			}
			return instance;
		}
	}

	public FsmVariables Variables
	{
		get
		{
			return variables;
		}
		set
		{
			variables = value;
		}
	}

	public List<string> Events
	{
		get
		{
			return events;
		}
		set
		{
			events = value;
		}
	}
}
