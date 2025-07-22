using System.Collections.Generic;

public class IAPShop
{
	private static IAPShop instance;

	private Dictionary<IAPName, IAPItem> iapItemList = new Dictionary<IAPName, IAPItem>();

	private Dictionary<ExchangeName, ExchangeItem> exchangeItemList = new Dictionary<ExchangeName, ExchangeItem>();

	private IAPShop()
	{
		CreateIAPShopData();
		CreateExchangeShopData();
	}

	public static IAPShop GetInstance()
	{
		if (instance == null)
		{
			instance = new IAPShop();
		}
		return instance;
	}

	private void CreateIAPShopData()
	{
		IAPItem iAPItem = new IAPItem();
		iAPItem.ID = "com.ifreyr.bo01.210m1d99";
		iAPItem.Desc = "Get 210 Mithrils";
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.MM:
			iAPItem.Mithril = 120;
			break;
		case AndroidConstant.Version.KindleCn:
			iAPItem.ID = "com.ifreyr.bo01cn.210m1d99";
			iAPItem.Mithril = 240;
			break;
		default:
			iAPItem.Mithril = 240;
			break;
		}
		AddIAPItem(IAPName.C199M240, iAPItem);
		IAPItem iAPItem2 = new IAPItem();
		iAPItem2.ID = "com.ifreyr.bo01.550m4d99";
		iAPItem2.Desc = "Get 550 Mithrils";
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.MM:
			iAPItem2.Mithril = 330;
			break;
		case AndroidConstant.Version.KindleCn:
			iAPItem2.ID = "com.ifreyr.bo01cn.550m4d99";
			iAPItem2.Mithril = 660;
			break;
		default:
			iAPItem2.Mithril = 660;
			break;
		}
		AddIAPItem(IAPName.C499M660, iAPItem2);
		IAPItem iAPItem3 = new IAPItem();
		iAPItem3.ID = "com.ifreyr.bo01.1200m9d99";
		iAPItem3.Desc = "Get 1200 Mithrils";
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.MM:
			iAPItem3.Mithril = 730;
			break;
		case AndroidConstant.Version.KindleCn:
			iAPItem3.ID = "com.ifreyr.bo01cn.1200m9d99";
			iAPItem3.Mithril = 1450;
			break;
		default:
			iAPItem3.Mithril = 1450;
			break;
		}
		AddIAPItem(IAPName.C999M1456, iAPItem3);
		IAPItem iAPItem4 = new IAPItem();
		iAPItem4.ID = "com.ifreyr.bo01.2600m19d99";
		iAPItem4.Desc = "Get 2600 Mithrils";
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.MM:
			iAPItem4.Mithril = 1240;
			break;
		case AndroidConstant.Version.KindleCn:
			iAPItem4.ID = "com.ifreyr.bo01cn.2600m19d99";
			iAPItem4.Mithril = 3300;
			break;
		default:
			iAPItem4.Mithril = 3300;
			break;
		}
		AddIAPItem(IAPName.C1999M3300, iAPItem4);
		IAPItem iAPItem5 = new IAPItem();
		iAPItem5.ID = "com.ifreyr.bo01.5800m39d99";
		iAPItem5.Desc = "Get 5800 Mithrils";
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.MM:
			iAPItem5.Mithril = 1930;
			break;
		case AndroidConstant.Version.KindleCn:
			iAPItem5.ID = "com.ifreyr.bo01cn.5800m39d99";
			iAPItem5.Mithril = 7700;
			break;
		default:
			iAPItem5.Mithril = 7700;
			break;
		}
		AddIAPItem(IAPName.C3999M7700, iAPItem5);
		IAPItem iAPItem6 = new IAPItem();
		iAPItem6.ID = "com.ifreyr.bo01.15000m99d99";
		iAPItem6.Desc = "Get 15000 Mithrils";
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.MM:
			iAPItem6.Mithril = 3750;
			break;
		case AndroidConstant.Version.KindleCn:
			iAPItem6.ID = "com.ifreyr.bo01cn.15000m99d99";
			iAPItem6.Mithril = 22500;
			break;
		default:
			iAPItem6.Mithril = 22500;
			break;
		}
		AddIAPItem(IAPName.C9999M22500, iAPItem6);
		IAPItem iAPItem7 = new IAPItem();
		iAPItem7.ID = "com.ifreyr.bo01.smgrp0d99";
		AndroidConstant.Version version = AndroidConstant.version;
		if (version == AndroidConstant.Version.KindleCn)
		{
			iAPItem7.ID = "com.ifreyr.bo01cn.smgrp0d99";
		}
		iAPItem7.Desc = "Get 15000 Mithrils";
		iAPItem7.Mithril = 0;
		AddIAPItem(IAPName.C99NewbieGift1, iAPItem7);
		IAPItem iAPItem8 = new IAPItem();
		iAPItem8.ID = "com.ifreyr.bo01.sniperrp1d99";
		version = AndroidConstant.version;
		if (version == AndroidConstant.Version.KindleCn)
		{
			iAPItem8.ID = "com.ifreyr.bo01cn.sniperrp1d99";
		}
		iAPItem8.Desc = "Get 15000 Mithrils";
		iAPItem8.Mithril = 0;
		AddIAPItem(IAPName.C199NewbieGift2, iAPItem8);
		IAPItem iAPItem9 = new IAPItem();
		iAPItem9.ID = "com.ifreyr.bo01.2xp2d99";
		version = AndroidConstant.version;
		if (version == AndroidConstant.Version.KindleCn)
		{
			iAPItem9.ID = "com.ifreyr.bo01cn.2xp2d99";
		}
		iAPItem9.Desc = "Get 15000 Mithrils";
		iAPItem9.Mithril = 0;
		AddIAPItem(IAPName.C199DoubleExp, iAPItem9);
		IAPItem iAPItem10 = new IAPItem();
		iAPItem10.ID = "com.ifreyr.bo01.iammo4d99";
		version = AndroidConstant.version;
		if (version == AndroidConstant.Version.KindleCn)
		{
			iAPItem10.ID = "com.ifreyr.bo01cn.iammo4d99";
		}
		iAPItem10.Desc = "Get 15000 Mithrils";
		iAPItem10.Mithril = 0;
		AddIAPItem(IAPName.C499InfiniteSMGBullet, iAPItem10);
		IAPItem iAPItem11 = new IAPItem();
		iAPItem11.ID = "com.ifreyr.bo01.irpgammo4d99";
		version = AndroidConstant.version;
		if (version == AndroidConstant.Version.KindleCn)
		{
			iAPItem11.ID = "com.ifreyr.bo01cn.irpgammo4d99";
		}
		iAPItem11.Desc = "Get 15000 Mithrils";
		iAPItem11.Mithril = 0;
		AddIAPItem(IAPName.C499InfiniteRPGBullet, iAPItem11);
	}

	private void CreateExchangeShopData()
	{
		ExchangeItem exchangeItem = new ExchangeItem();
		exchangeItem.MithrilCost = string.Empty + 100;
		exchangeItem.CashRatio = 12;
		AddExchangeItem(ExchangeName.M10G1K, exchangeItem);
		ExchangeItem exchangeItem2 = new ExchangeItem();
		exchangeItem2.MithrilCost = string.Empty + 500;
		exchangeItem2.CashRatio = 15;
		AddExchangeItem(ExchangeName.M50G10K, exchangeItem2);
		ExchangeItem exchangeItem3 = new ExchangeItem();
		exchangeItem3.MithrilCost = string.Empty + 1000;
		exchangeItem3.CashRatio = 16;
		AddExchangeItem(ExchangeName.M100G30K, exchangeItem3);
		ExchangeItem exchangeItem4 = new ExchangeItem();
		exchangeItem4.MithrilCost = string.Empty + 5000;
		exchangeItem4.CashRatio = 17;
		AddExchangeItem(ExchangeName.M1KG320K, exchangeItem4);
		ExchangeItem exchangeItem5 = new ExchangeItem();
		exchangeItem5.MithrilCost = string.Empty + 10000;
		exchangeItem5.CashRatio = 18;
		AddExchangeItem(ExchangeName.M5KG1700K, exchangeItem5);
		ExchangeItem exchangeItem6 = new ExchangeItem();
		exchangeItem6.MithrilCost = string.Empty + 100000;
		exchangeItem6.CashRatio = 20;
		AddExchangeItem(ExchangeName.M10KG3800K, exchangeItem6);
	}

	public void AddIAPItem(IAPName name, IAPItem item)
	{
		iapItemList.Add(name, item);
	}

	public void AddExchangeItem(ExchangeName name, ExchangeItem item)
	{
		exchangeItemList.Add(name, item);
	}

	public Dictionary<IAPName, IAPItem> GetIAPList()
	{
		return iapItemList;
	}

	public Dictionary<ExchangeName, ExchangeItem> GetExchangeList()
	{
		return exchangeItemList;
	}

	public IAPItem GetIAPItem(IAPName iapName)
	{
		if (iapItemList.ContainsKey(iapName))
		{
			return iapItemList[iapName];
		}
		return null;
	}
}
