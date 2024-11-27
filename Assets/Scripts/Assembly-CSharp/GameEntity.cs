using UnityEngine;

public class GameEntity
{
	protected GameObject entityObject;

	protected Transform entityTransform;

	protected GameObject animationObject;

	protected Animation animation;

	protected GameScene gameScene;

	protected GameWorld gameWorld;

	public string Name { get; set; }

	public bool IsSamplingAnimation { get; set; }

	public GameObject GetObject()
	{
		return entityObject;
	}

	public Transform GetTransform()
	{
		return entityTransform;
	}

	public Vector3 GetPosition()
	{
		return entityTransform.position;
	}

	public void SetPosition(Vector3 pos)
	{
		entityTransform.position = pos;
	}

	public GameObject GetAnimationObject()
	{
		return animationObject;
	}

	public virtual void SetObject(GameObject obj)
	{
		entityObject = obj;
		entityTransform = obj.transform;
		animationObject = obj;
	}

	public void Show()
	{
		entityObject.SetActive(true);
	}

	public void Hide()
	{
		entityObject.SetActive(false);
	}

	public virtual void PlayAnimation(string name, WrapMode mode)
	{
		if (animationObject.GetComponent<Animation>()[name] == null)
		{
			Debug.Log("missed animation..." + name);
		}
		else if (!animationObject.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever || AnimationPlayed(name, 1f))
		{
			animationObject.GetComponent<Animation>()[name].wrapMode = mode;
			animationObject.GetComponent<Animation>().CrossFade(name, 0.2f);
		}
	}

	public virtual void PlayAnimation(string name, WrapMode mode, float speed)
	{
		if (animationObject.GetComponent<Animation>()[name] == null)
		{
			Debug.Log("missed animation..." + name);
		}
		else if (!animationObject.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever)
		{
			animationObject.GetComponent<Animation>()[name].wrapMode = mode;
			animationObject.GetComponent<Animation>()[name].speed = speed;
			animationObject.GetComponent<Animation>().CrossFade(name);
		}
	}

	public virtual void PlayAnimationWithoutBlend(string name, WrapMode mode)
	{
		if (animationObject.GetComponent<Animation>()[name] == null)
		{
			Debug.Log("missed animation..." + name);
		}
		else if (!animationObject.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever || AnimationPlayed(name, 1f))
		{
			animationObject.GetComponent<Animation>()[name].wrapMode = mode;
			animationObject.GetComponent<Animation>().Play(name);
		}
	}

	public virtual void PlayAnimationAllLayers(string name, WrapMode mode)
	{
		if (animationObject.GetComponent<Animation>()[name] == null)
		{
			Debug.Log("missed animation..." + name);
		}
		else if (!animationObject.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever)
		{
			animationObject.GetComponent<Animation>()[name].wrapMode = mode;
			animationObject.GetComponent<Animation>().CrossFade(name, 0.1f, PlayMode.StopAll);
		}
	}

	public bool IsPlayingAnimation()
	{
		return animation.isPlaying;
	}

	public virtual bool IsPlayingAnimation(string name)
	{
		return animationObject.GetComponent<Animation>().IsPlaying(name);
	}

	public bool AnimationPlayed(string name, float percent)
	{
		if (animationObject.GetComponent<Animation>()[name] == null)
		{
			return false;
		}
		if (animationObject.GetComponent<Animation>()[name].speed >= 0f)
		{
			if (animationObject.GetComponent<Animation>()[name].time >= animationObject.GetComponent<Animation>()[name].clip.length * percent)
			{
				return true;
			}
			return false;
		}
		if (animationObject.GetComponent<Animation>()[name].time <= animationObject.GetComponent<Animation>()[name].clip.length * (1f - percent))
		{
			return true;
		}
		return false;
	}

	public bool AnimationPlayedLoop(string name, float percent)
	{
		int num = (int)(animationObject.GetComponent<Animation>()[name].time / animationObject.GetComponent<Animation>()[name].clip.length);
		float num2 = animationObject.GetComponent<Animation>()[name].time - animationObject.GetComponent<Animation>()[name].clip.length * (float)num;
		if (animationObject.GetComponent<Animation>()[name] == null)
		{
			return false;
		}
		if (animationObject.GetComponent<Animation>()[name].speed >= 0f)
		{
			if (num2 >= animationObject.GetComponent<Animation>()[name].clip.length * percent)
			{
				return true;
			}
			return false;
		}
		if (num2 <= animationObject.GetComponent<Animation>()[name].clip.length * (1f - percent))
		{
			return true;
		}
		return false;
	}

	public void StopAnimation()
	{
		animationObject.GetComponent<Animation>().Stop();
	}

	public virtual void StopAnimation(string name)
	{
		animationObject.GetComponent<Animation>().Stop(name);
	}

	public void SampleAnimation()
	{
		IsSamplingAnimation = true;
		if (animationObject.GetComponent<Animation>().isPlaying)
		{
			Debug.Log("still playing animation...");
		}
		else
		{
			Debug.Log("sample..");
		}
	}
}
