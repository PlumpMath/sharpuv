﻿using NUnit.Framework;
using System;
using System.Text;

using SharpUV;

namespace SharpUV.NUnit
{
	[TestFixture]
	public class FileTests
	{
		private static bool done = false;

		[Test]
		public void Open()
		{
			var handle = new WriteFileHandle();
			var path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "test.txt");

			handle.Open(path, FileAccessMode.WriteOnly, FileOpenMode.Create | FileOpenMode.BinaryMode, FilePermissions.S_IRUSR | FilePermissions.S_IWUSR);

			while (!done) {
				Loop.Default.RunOnce();
			}

			Assert.AreEqual(System.IO.File.ReadAllText(path), "test");
		}

		internal class WriteFileHandle : FileHandle
		{
			protected override void OnOpen(UvArgs args)
			{
				args.Throw ();
				this.Write(Encoding.UTF8.GetBytes("test"));
			}

			protected override void OnWrite(UvArgs args)
			{
				this.Close();
			}

			protected override void OnClose ()
			{
				done = true;
			}
		}
	}
}
