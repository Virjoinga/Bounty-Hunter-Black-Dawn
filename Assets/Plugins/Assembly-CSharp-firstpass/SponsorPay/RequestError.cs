namespace SponsorPay
{
	public class RequestError
	{
		public string Type { get; private set; }

		public string Code { get; private set; }

		public string Message { get; private set; }

		public RequestError(string type, string code, string message)
		{
			Type = ((type == null) ? string.Empty : type);
			Code = ((code == null) ? string.Empty : code);
			Message = ((message == null) ? string.Empty : message);
		}

		public override string ToString()
		{
			return string.Format("Error Type: {0}\nError Code: {1}\nError Message: {2}\n", Type, Code, Message);
		}
	}
}
