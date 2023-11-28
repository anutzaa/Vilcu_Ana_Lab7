using System;
using Vilcu_Ana_Lab7.Data;
using System.IO;

namespace Vilcu_Ana_Lab7;

public partial class App : Application
{
	static ShoppingListDatabase database;

	public static ShoppingListDatabase Database
	{
		get
		{
			if (database == null)
			{
				database = new ShoppingListDatabase(Path.Combine(Environment.GetFolderPath(Environment
					.SpecialFolder.LocalApplicationData), "ShoppingList.db3"));
			}
			return database;
		}
	}

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
