﻿using Microsoft.SharePoint.Client;

namespace FluentlySharepoint.Extensions
{
	public static class Folder
	{
		public static CSOMOperation CreateFolder(this CSOMOperation operation, string remotePath, bool overwrite = true)
		{
			var list = operation.LastList;
			var resourceFolderPath = ResourcePath.FromDecodedUrl(list.RootFolder.Name + "/" + remotePath); // DRY...

			var folder = list.RootFolder.Folders.AddUsingPath(resourceFolderPath, new FolderCollectionAddParameters { Overwrite = overwrite });

			folder.Context.Load(folder);

			return operation;
		}

		public static CSOMOperation DeleteFolder(this CSOMOperation operation, string remotePath)
		{
			var list = operation.LastList;
			var resourceFolderPath = ResourcePath.FromDecodedUrl(list.RootFolder.Name + "/" + remotePath); // DRY...

			list.RootFolder.Folders.GetByPath(resourceFolderPath).DeleteObject();

			return operation;
		}
	}
}