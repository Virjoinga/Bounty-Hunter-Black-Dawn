using System.IO;

public class Message
{
	private short _class;

	private short _size;

	private byte[] _content;

	public byte[] Content
	{
		get
		{
			return _content;
		}
		set
		{
			_content = value;
		}
	}

	public short Size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
		}
	}

	public short Class
	{
		get
		{
			return _class;
		}
		set
		{
			_class = value;
		}
	}

	public Message()
	{
	}

	public Message(byte @class, byte[] content)
	{
		_class = @class;
		_size = (byte)content.Length;
		_content = content;
	}

	public byte[] ToBytes()
	{
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(_class);
			binaryWriter.Write(_size);
			if (_size > 0)
			{
				binaryWriter.Write(_content);
			}
			byte[] result = memoryStream.ToArray();
			binaryWriter.Close();
			return result;
		}
	}

	public static Message FromBytes(byte[] Buffer)
	{
		Message message = new Message();
		using (MemoryStream input = new MemoryStream(Buffer))
		{
			BinaryReader binaryReader = new BinaryReader(input);
			message._class = binaryReader.ReadInt16();
			message._size = binaryReader.ReadInt16();
			if (message._size > 0)
			{
				message._content = binaryReader.ReadBytes(message._size);
			}
			binaryReader.Close();
			return message;
		}
	}
}
