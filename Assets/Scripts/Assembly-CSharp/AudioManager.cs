using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public bool bMuteSound;

	public bool bMuteMusic;

	public float soundVolume = 1f;

	public float musicVolume = 1f;

	protected Dictionary<string, List<AudioObject>> m_soundClips = new Dictionary<string, List<AudioObject>>();

	protected Dictionary<string, List<GameObject>> m_musicClips = new Dictionary<string, List<GameObject>>();

	private void Awake()
	{
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		bMuteSound = !GameApp.GetInstance().GetGlobalState().GetPlaySound();
		bMuteMusic = !GameApp.GetInstance().GetGlobalState().GetPlayMusic();
		soundVolume = GameApp.GetInstance().GetGlobalState().GetSoundVolume();
		musicVolume = GameApp.GetInstance().GetGlobalState().GetMusicVolume();
		NGUITools.soundVolume = soundVolume;
		if (bMuteSound)
		{
			NGUITools.soundVolume = 0f;
		}
		m_soundClips.Clear();
		m_musicClips.Clear();
	}

	public static AudioManager GetInstance()
	{
		return Camera.main.GetComponent<AudioManager>();
	}

	public AudioClip LoadMusic(string name)
	{
		return Resources.Load(name, typeof(AudioClip)) as AudioClip;
	}

	public AudioClip LoadSound(string name)
	{
		return Resources.Load(name, typeof(AudioClip)) as AudioClip;
	}

	protected void AddSound(string name, AudioObject obj)
	{
		if (m_soundClips.ContainsKey(name))
		{
			if (m_soundClips[name] == null)
			{
				m_soundClips[name] = new List<AudioObject>();
				m_soundClips[name].Add(obj);
			}
			else
			{
				m_soundClips[name].Add(obj);
			}
		}
		else
		{
			List<AudioObject> list = new List<AudioObject>();
			list.Add(obj);
			m_soundClips.Add(name, list);
		}
	}

	protected void AddMusic(string name, GameObject obj)
	{
		if (m_musicClips.ContainsKey(name))
		{
			if (m_musicClips[name] == null)
			{
				m_musicClips[name] = new List<GameObject>();
				m_musicClips[name].Add(obj);
			}
			else
			{
				m_musicClips[name].Add(obj);
			}
		}
		else
		{
			List<GameObject> list = new List<GameObject>();
			list.Add(obj);
			m_musicClips.Add(name, list);
		}
	}

	private void OnDestroy()
	{
		StopAllMusic();
		StopAllSound();
	}

	public void PlayMusic(string name)
	{
		AudioClip audioClip = LoadMusic(name);
		Debug.Log("clip: " + audioClip);
		GameObject gameObject = new GameObject("AudioMusic::" + audioClip.name);
		gameObject.transform.parent = base.transform;
		gameObject.transform.position = base.transform.position;
		AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioSource.clip = audioClip;
		audioSource.loop = true;
		audioSource.volume = musicVolume;
		audioSource.playOnAwake = false;
		audioSource.mute = bMuteMusic;
		audioSource.Play();
		AddMusic(name, gameObject);
	}

	public bool IsPlayingMusic(string name)
	{
		if (m_musicClips.ContainsKey(name) && m_musicClips[name].Count > 0)
		{
			return true;
		}
		return false;
	}

	public void StopAllMusic()
	{
		foreach (KeyValuePair<string, List<GameObject>> musicClip in m_musicClips)
		{
			foreach (GameObject item in musicClip.Value)
			{
				if (item != null)
				{
					Object.Destroy(item);
				}
			}
		}
		m_musicClips.Clear();
	}

	public void StopAllSound()
	{
		foreach (KeyValuePair<string, List<AudioObject>> soundClip in m_soundClips)
		{
			foreach (AudioObject item in soundClip.Value)
			{
				if (item.clipObject != null)
				{
					Object.Destroy(item.clipObject);
				}
			}
		}
		m_soundClips.Clear();
	}

	public void SetSoundMute(bool bMute)
	{
		bMuteSound = bMute;
		foreach (KeyValuePair<string, List<AudioObject>> soundClip in m_soundClips)
		{
			foreach (AudioObject item in soundClip.Value)
			{
				if (item.clipObject != null)
				{
					AudioSource audioSource = item.clipObject.GetComponent(typeof(AudioSource)) as AudioSource;
					audioSource.mute = bMute;
				}
			}
		}
	}

	public void SetMusicMute(bool bMute)
	{
		bMuteMusic = bMute;
		foreach (KeyValuePair<string, List<GameObject>> musicClip in m_musicClips)
		{
			foreach (GameObject item in musicClip.Value)
			{
				if (item != null)
				{
					AudioSource audioSource = item.GetComponent(typeof(AudioSource)) as AudioSource;
					audioSource.mute = bMute;
				}
			}
		}
	}

	public void SetSoundVolume(float volume)
	{
		soundVolume = volume;
		foreach (KeyValuePair<string, List<AudioObject>> soundClip in m_soundClips)
		{
			foreach (AudioObject item in soundClip.Value)
			{
				if (item.clipObject != null && item.clipObject != null)
				{
					AudioSource audioSource = item.clipObject.GetComponent(typeof(AudioSource)) as AudioSource;
					audioSource.volume = volume;
				}
			}
		}
	}

	public void SetMusicVolume(float volume)
	{
		musicVolume = volume;
		foreach (KeyValuePair<string, List<GameObject>> musicClip in m_musicClips)
		{
			foreach (GameObject item in musicClip.Value)
			{
				if (item != null)
				{
					AudioSource audioSource = item.GetComponent(typeof(AudioSource)) as AudioSource;
					audioSource.volume = volume;
				}
			}
		}
	}

	public void PlaySoundAt(string name, Vector3 pos)
	{
		PlaySoundAt(name, null, pos);
	}

	public void PlaySoundAt(string name, Vector3 pos, AudioRolloffMode mode, float maxDistance)
	{
		PlaySoundAt(name, null, pos, mode, maxDistance);
	}

	public void PlaySound(string name)
	{
		PlaySound(name, null);
	}

	public void PlaySoundLoop(string name)
	{
		PlaySoundLoop(name, null);
	}

	public void PlaySoundSingle(string name)
	{
		if (!IsPlayingSound(name))
		{
			PlaySound(name);
		}
	}

	public void PlaySoundSingleLoop(string name)
	{
		if (!IsPlayingSound(name))
		{
			PlaySoundLoop(name);
		}
	}

	public bool IsPlayingSound(string name)
	{
		if (m_soundClips.ContainsKey(name) && m_soundClips[name].Count > 0)
		{
			for (int i = 0; i < m_soundClips[name].Count; i++)
			{
				if (m_soundClips[name][i].clipObject != null)
				{
					return true;
				}
			}
			m_soundClips[name].Clear();
			return false;
		}
		return false;
	}

	public void PlaySoundSingleAt(string name, Vector3 pos)
	{
		if (!IsPlayingSound(name))
		{
			PlaySoundAt(name, pos);
		}
	}

	public void PlaySoundSingleAt(string name, Vector3 pos, AudioRolloffMode mode, float maxDistance)
	{
		if (!IsPlayingSound(name))
		{
			PlaySoundAt(name, pos, mode, maxDistance);
		}
	}

	public void StopSound(string name)
	{
		if (!m_soundClips.ContainsKey(name))
		{
			return;
		}
		foreach (AudioObject item in m_soundClips[name])
		{
			if (item.clipObject != null)
			{
				Object.Destroy(item.clipObject);
			}
		}
		m_soundClips[name].Clear();
		m_soundClips.Remove(name);
	}

	public void PlaySoundAt(string name, GameObject owner, Vector3 pos)
	{
		if (soundVolume != 0f && !bMuteSound)
		{
			AudioClip audioClip = LoadSound(name);
			GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
			AudioObject obj = default(AudioObject);
			obj.clipObject = gameObject;
			obj.owner = owner;
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = pos;
			AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			audioSource.clip = audioClip;
			audioSource.volume = soundVolume;
			audioSource.maxDistance = 300f;
			audioSource.Play();
			audioSource.rolloffMode = AudioRolloffMode.Linear;
			audioSource.playOnAwake = false;
			audioSource.mute = bMuteSound;
			AddSound(name, obj);
			Object.Destroy(gameObject, audioClip.length);
		}
	}

	public void PlaySoundAt(string name, GameObject owner, Vector3 pos, AudioRolloffMode mode, float maxDistance)
	{
		if (soundVolume != 0f && !bMuteSound)
		{
			AudioClip audioClip = LoadSound(name);
			GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
			AudioObject obj = default(AudioObject);
			obj.clipObject = gameObject;
			obj.owner = owner;
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = pos;
			AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			audioSource.clip = audioClip;
			audioSource.volume = soundVolume;
			audioSource.maxDistance = maxDistance;
			audioSource.Play();
			audioSource.rolloffMode = mode;
			audioSource.playOnAwake = false;
			audioSource.mute = bMuteSound;
			AddSound(name, obj);
			Object.Destroy(gameObject, audioClip.length);
		}
	}

	public void PlaySound(string name, GameObject owner)
	{
		if (soundVolume != 0f && !bMuteSound)
		{
			AudioClip audioClip = LoadSound(name);
			GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
			AudioObject obj = default(AudioObject);
			obj.clipObject = gameObject;
			obj.owner = owner;
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = base.transform.position;
			AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			audioSource.clip = audioClip;
			audioSource.volume = soundVolume;
			audioSource.Play();
			audioSource.playOnAwake = false;
			audioSource.mute = bMuteSound;
			AddSound(name, obj);
			Object.Destroy(gameObject, audioClip.length);
		}
	}

	public void PlaySoundLoop(string name, GameObject owner)
	{
		if (soundVolume != 0f && !bMuteSound)
		{
			AudioClip audioClip = LoadSound(name);
			GameObject gameObject = new GameObject("AudioSound::" + audioClip.name);
			AudioObject obj = default(AudioObject);
			obj.clipObject = gameObject;
			obj.owner = owner;
			gameObject.transform.parent = base.transform;
			gameObject.transform.position = base.transform.position;
			AudioSource audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			audioSource.clip = audioClip;
			audioSource.volume = soundVolume;
			audioSource.loop = true;
			audioSource.Play();
			audioSource.playOnAwake = false;
			audioSource.mute = bMuteSound;
			AddSound(name, obj);
			Object.Destroy(gameObject, audioClip.length);
		}
	}

	public void PlaySoundSingle(string name, GameObject owner)
	{
		if (!IsPlayingSound(name, owner))
		{
			PlaySound(name, owner);
		}
	}

	public void PlaySoundSingleLoop(string name, GameObject owner)
	{
		if (!IsPlayingSound(name, owner))
		{
			PlaySoundLoop(name, owner);
		}
	}

	public bool IsPlayingSound(string name, GameObject owner)
	{
		if (m_soundClips.ContainsKey(name) && m_soundClips[name].Count > 0)
		{
			bool flag = true;
			for (int i = 0; i < m_soundClips[name].Count; i++)
			{
				if (m_soundClips[name][i].clipObject != null)
				{
					flag = false;
					if (m_soundClips[name][i].owner == owner)
					{
						return true;
					}
				}
			}
			if (flag)
			{
				m_soundClips[name].Clear();
			}
			return false;
		}
		return false;
	}

	public void PlaySoundSingleAt(string name, GameObject owner, Vector3 pos)
	{
		if (!IsPlayingSound(name, owner))
		{
			PlaySoundAt(name, owner, pos);
		}
	}

	public void PlaySoundSingleAt(string name, GameObject owner, Vector3 pos, AudioRolloffMode mode, float maxDistance)
	{
		if (!IsPlayingSound(name, owner))
		{
			PlaySoundAt(name, owner, pos, mode, maxDistance);
		}
	}

	public void StopSound(string name, GameObject owner)
	{
		if (!m_soundClips.ContainsKey(name))
		{
			return;
		}
		List<AudioObject> list = new List<AudioObject>();
		foreach (AudioObject item in m_soundClips[name])
		{
			if (item.owner == owner)
			{
				list.Add(item);
				if (item.clipObject != null)
				{
					Object.Destroy(item.clipObject);
				}
			}
		}
		foreach (AudioObject item2 in list)
		{
			if (m_soundClips[name].Contains(item2))
			{
				m_soundClips[name].Remove(item2);
			}
		}
		if (m_soundClips[name].Count == 0)
		{
			m_soundClips.Remove(name);
		}
	}
}
