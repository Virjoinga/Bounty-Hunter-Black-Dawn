using System.Collections.Generic;
using UnityEngine;

public class UploadExploreItemBlockRequest : Request
{
	private byte mBlockID;

	private Dictionary<byte, ExploreItemStatesInfo> mblockItemDictionary = new Dictionary<byte, ExploreItemStatesInfo>();

	public UploadExploreItemBlockRequest(byte ID)
	{
		requestID = 165;
		mBlockID = ID;
		Dictionary<byte, ExploreItemStatesInfo> mExplorableStateDictionary = GameApp.GetInstance().GetGameWorld().GetExplorItemBlock(mBlockID)
			.mExplorableStateDictionary;
		foreach (KeyValuePair<byte, ExploreItemStatesInfo> item in mExplorableStateDictionary)
		{
			Debug.Log("Upload---Key: " + item.Key + " ItemPair: " + item.Value);
			ExploreItemStatesInfo value = new ExploreItemStatesInfo
			{
				mQuestID = item.Value.mQuestID,
				mState = item.Value.mState
			};
			mblockItemDictionary.Add(item.Key, value);
		}
	}

	public override byte[] GetBody()
	{
		BytesBuffer bytesBuffer = new BytesBuffer(2 + 4 * mblockItemDictionary.Count);
		bytesBuffer.AddByte(mBlockID);
		bytesBuffer.AddByte((byte)mblockItemDictionary.Count);
		foreach (KeyValuePair<byte, ExploreItemStatesInfo> item in mblockItemDictionary)
		{
			bytesBuffer.AddByte(item.Key);
			bytesBuffer.AddShort(item.Value.mQuestID);
			bytesBuffer.AddByte((byte)item.Value.mState);
		}
		return bytesBuffer.GetBytes();
	}
}
