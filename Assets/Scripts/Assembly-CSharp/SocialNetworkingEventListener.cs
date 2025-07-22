using System;
using System.Collections;
using UnityEngine;

public class SocialNetworkingEventListener : MonoBehaviour
{
	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void twitterLogin()
	{
		Debug.Log("Successfully logged in to Twitter");
		string status = UIConstant.HEY + UIConstant.FULL_VERSION_URL;
		if (!Lobby.GetInstance().IsPostingScoreToSocialNetwork)
		{
			ScreenCapture.CaptureScreenshot("tempscreens.png");
			string pathToImage = Application.persistentDataPath + "/tempscreens.png";
			TwitterBinding.postStatusUpdate(status, pathToImage);
		}
		else
		{
			ScreenCapture.CaptureScreenshot("tempscreens.png");
			string pathToImage2 = Application.persistentDataPath + "/tempscreens.png";
			TwitterBinding.postStatusUpdate(status, pathToImage2);
		}
	}

	private void twitterLoginFailed(string error)
	{
		Debug.Log("Twitter login failed: " + error);
	}

	private void twitterPost()
	{
		if (Lobby.GetInstance().IsPostingScoreToSocialNetwork)
		{
		}
		Debug.Log("Successfully posted to Twitter");
	}

	private void twitterPostFailed(string error)
	{
		Debug.Log("Twitter post failed: " + error);
	}

	private void twitterHomeTimelineFailed(string error)
	{
		Debug.Log("Twitter HomeTimeline failed: " + error);
	}

	private void twitterHomeTimelineReceived(ArrayList result)
	{
		Debug.Log("received home timeline with tweet count: " + result.Count);
	}

	private void twitterRequestDidFailEvent(string error)
	{
		Debug.Log("twitterRequestDidFailEvent: " + error);
	}

	private void twitterRequestDidFinishEvent(object result)
	{
		if (result != null)
		{
			Debug.Log("twitterRequestDidFinishEvent: " + result.GetType().ToString());
		}
		else
		{
			Debug.Log("twitterRequestDidFinishEvent with no data");
		}
	}

	private void facebookLogin()
	{
		Debug.Log("Successfully logged in to Facebook");
		if (Lobby.GetInstance().IsPostingScoreToSocialNetwork)
		{
		}
	}

	private void facebookLoginFailed(string error)
	{
		Debug.Log("Facebook login failed: " + error);
	}

	private void facebookDidLogoutEvent()
	{
		Debug.Log("facebookDidLogoutEvent");
	}

	private void facebookDidExtendTokenEvent(DateTime newExpiry)
	{
		Debug.Log("facebookDidExtendTokenEvent: " + newExpiry);
	}

	private void facebookSessionInvalidatedEvent()
	{
		Debug.Log("facebookSessionInvalidatedEvent");
	}

	private void facebookReceivedUsername(string username)
	{
		Debug.Log("Facebook logged in users name: " + username);
	}

	private void facebookUsernameRequestFailed(string error)
	{
		Debug.Log("Facebook failed to receive username: " + error);
	}

	private void facebookPost()
	{
		if (Lobby.GetInstance().IsPostingScoreToSocialNetwork)
		{
		}
		Debug.Log("Successfully posted to Facebook");
	}

	private void facebookPostFailed(string error)
	{
		Debug.Log("Facebook post failed: " + error);
	}

	private void facebookReceivedFriends(ArrayList result)
	{
		Debug.Log("received total friends: " + result.Count);
	}

	private void facebookFriendRequestFailed(string error)
	{
		Debug.Log("FfacebookFriendRequestFailed: " + error);
	}

	private void facebokDialogCompleted()
	{
		Debug.Log("facebokDialogCompleted");
	}

	private void facebookDialogCompletedWithUrl(string url)
	{
		Debug.Log("facebookDialogCompletedWithUrl: " + url);
	}

	private void facebookDialogDidntComplete()
	{
		Debug.Log("facebookDialogDidntComplete");
	}

	private void facebookDialogFailed(string error)
	{
		Debug.Log("facebookDialogFailed: " + error);
	}

	private void facebookReceivedCustomRequest(object obj)
	{
		Debug.Log("facebookReceivedCustomRequest");
	}

	private void facebookCustomRequestFailed(string error)
	{
		Debug.Log("facebookCustomRequestFailed failed: " + error);
	}
}
