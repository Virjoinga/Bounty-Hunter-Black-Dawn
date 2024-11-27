using System;
using System.IO;
using System.Threading;
using UnityEngine;

public class Receiver
{
	private BinaryReader br;

	private bool isRunning;

	private Thread t;

	private NetworkManager networkMgr;

	public bool isExit;

	private DateTime lastTime = DateTime.Now;

	private int packetNum;

	private int byteNum;

	public Receiver(NetworkManager networkMgr)
	{
		this.networkMgr = networkMgr;
	}

	public void Init(BinaryReader br)
	{
		isRunning = true;
		isExit = false;
		this.br = br;
		t = new Thread(Run);
		t.Start();
	}

	public void Run()
	{
		while (isRunning)
		{
			try
			{
				if (br != null)
				{
					short responseID = BytesBuffer.ReverseShort(br.ReadInt16());
					short num = BytesBuffer.ReverseShort(br.ReadInt16());
					byte[] array = null;
					if (num > 0)
					{
						array = new byte[num];
						DateTime now = DateTime.Now;
						int i = br.Read(array, 0, num);
						DateTime now2 = DateTime.Now;
						TimeSpan timeSpan = now2 - now;
						int num2;
						for (; i != num; i += num2)
						{
							Debug.Log("error...");
							num2 = br.Read(array, i, num - i);
						}
					}
					Response response = Response.CreateResponse(responseID);
					response.responseID = responseID;
					response.ReadData(array);
					networkMgr.getReceivedPacketCache().AddPacket(response);
					updatePacket(num);
				}
				else
				{
					Thread.Sleep(16);
				}
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message + "\r\n" + ex.StackTrace);
				networkMgr.exceptionMessage += ex.Message;
				networkMgr.IsDisplayErrorBox = true;
				break;
			}
		}
		networkMgr.IsDisconnected = true;
		isExit = true;
	}

	private void updatePacket(short length)
	{
		packetNum++;
		byteNum = byteNum + 2 + length;
		TimeSpan timeSpan = DateTime.Now - lastTime;
		if ((float)timeSpan.TotalSeconds > 1f)
		{
			GameApp.GetInstance().AverageReceivePacket = (float)packetNum / (float)timeSpan.TotalSeconds;
			GameApp.GetInstance().AverageReceiveByte = (float)byteNum / (float)timeSpan.TotalSeconds;
			packetNum = 0;
			byteNum = 0;
			lastTime = DateTime.Now;
		}
	}

	public void StopThread()
	{
		isRunning = false;
		if (t != null)
		{
			t.Interrupt();
		}
	}

	public void Close()
	{
		try
		{
			if (br != null)
			{
				br.Close();
				br = null;
			}
		}
		catch (IOException ex)
		{
			networkMgr.exceptionMessage = ex.Message;
		}
	}
}
