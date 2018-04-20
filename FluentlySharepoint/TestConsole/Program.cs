﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KeenMate.FluentlySharePoint;
using KeenMate.FluentlySharePoint.Extensions;
using KeenMate.FluentlySharePoint.Interfaces;
using Microsoft.SharePoint.Client;
using TestConsole.Loggers;

namespace TestConsole
{
	class Program
	{
		public const string UserName = "trent.reznor@keenmate.com";
		public const string Password = "Discipline042008";
		public const string SiteUrl =
				"https://keenmate.sharepoint.com/sites/demo/fluently-sharepoint/";

		public static ILogger logger = new ConsoleLogger();

		static void Main(string[] args)
		{
			MeasuredOperation(CreateAndExecute);

			MeasuredOperation(CreateExecuteAndReuse);

			MeasuredOperation(ReuseExistingContext);
		}

		private static void MeasuredOperation(Action operation)
		{
			var stopwatch = Stopwatch.StartNew();

			operation();

			stopwatch.Stop();

			logger.Trace($"Operation finished in {stopwatch.ElapsedMilliseconds}");
		}

		private static void CreateAndExecute()
		{
			var op = SiteUrl
				.Create(logger)
				.SetOnlineCredentials(UserName, Password) // Available also with SecureString parameter
				.Execute();

			logger.Trace($"Default timeout: {op.Context.RequestTimeout}");
		}

		private static void CreateExecuteAndReuse()
		{
			logger.Info("Create, execute and reuse example");

			var op = SiteUrl
				.Create(logger)
				.SetOnlineCredentials(UserName, Password) // Available also with SecureString parameter
				.Execute();
			
			var listTitle = "Documents";

			op.LoadList(listTitle, (context, list) =>
			{
				context.Load(list, l => l.ItemCount);
			});

			var items = op.GetItems();

			logger.Info($"Total items of list {listTitle} with list.ItemCount: {op.LastList.ItemCount} = Items count loaded with GetItems: {items.Count}");
		}

		private static void ReuseExistingContext()
		{
			logger.Info("Reuse existing context example");

			ClientContext context = new ClientContext(SiteUrl);
			context.Credentials = new SharePointOnlineCredentials(UserName, Password.ToSecureString());

			var listTitle = "Documents";

			var items = context
				.Create()
				.LoadList(listTitle)
				.GetItems();

			logger.Info($"Total items of list {listTitle} with list.ItemCount: {items.Count}");

		}
	}
}
