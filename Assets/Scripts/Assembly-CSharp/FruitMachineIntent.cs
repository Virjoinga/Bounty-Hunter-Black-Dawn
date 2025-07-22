using System.Collections.Generic;

public class FruitMachineIntent
{
	private Dictionary<string, object> dic = new Dictionary<string, object>();

	public void Put(string key, object value)
	{
		if (key != null && value != null && !dic.ContainsKey(key))
		{
			dic.Add(key, value);
		}
	}

	public object Get(string key)
	{
		return dic[key];
	}
}
