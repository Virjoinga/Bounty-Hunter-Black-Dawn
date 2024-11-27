using UnityEngine;

internal class RequireExploreItemBlockResponse : Response
{
	private byte mBlockID;

	public override void ReadData(byte[] data)
	{
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		mBlockID = bytesBuffer.ReadByte();
	}

	public override void ProcessLogic()
	{
		Debug.Log("RequireExploreItemBlockResponse: " + mBlockID);
		GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.EXPLORE_ITEM);
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			ExplorItemBlockScript component = gameObject.GetComponent<ExplorItemBlockScript>();
			if (null != component && component.BlockID == mBlockID)
			{
				component.Init();
				component.CreateExplores();
				component.RefreshExplorableStates();
				break;
			}
		}
		UploadExploreItemBlockRequest request = new UploadExploreItemBlockRequest(mBlockID);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}
}
