﻿#region License
/**
 * Copyright (c) 2012 Luigi Grilli
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */
#endregion

using System;
using System.Net;
using Libuv;

namespace SharpUV
{
	public class TcpClientSocket : TcpSocket
	{
		public TcpClientSocket(Loop loop)
			: base(loop)
		{
			this.Connection = this.Alloc(uv_req_type.UV_CONNECT);
		}

		/// <summary>
		/// Connection handle
		/// </summary>
		/// <remarks>Handle type is <typeparamref name="Libuv.uv_connect_t"/></remarks>
		internal IntPtr Connection { get; set; }

		public void Connect(IPEndPoint endpoint)
		{
			var info = Uvi.uv_ip4_addr(endpoint.Address.ToString(), endpoint.Port);
			CheckError(Uvi.uv_tcp_connect(this.Connection, this.Handle, info, this.OnConnect));
		}

		private void OnConnect(IntPtr connection, int status)
		{
			//var conn = (uv_connect_t)Marshal.PtrToStructure(connection, typeof(uv_connect_t));
		}

		protected void OnConnect()
		{
		}

		protected override void Dispose(bool disposing)
		{
			if(!this.IsDisposed)
			{
				if (this.Connection != IntPtr.Zero)
					this.Connection = this.Free(this.Connection);
			}

			base.Dispose(disposing);
		}
	}

	public class TcpServerSocket : TcpSocket
	{
		public TcpServerSocket(TcpServer server)
			: base(server.Loop)
		{
			this.Server = server;
			this.Accept();
		}

		private void Accept()
		{
			CheckError(Uvi.uv_accept(this.Server.Handle, this.Handle));
		}

		protected TcpServer Server { get; private set; }
	}

	public class TcpSocket : UvStream
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="loop"></param>
		/// <remarks>Handle type is <typeparamref name="Libuv.uv_tcp_t"/> that is a subclass of <typeparamref name="Libuv.uv_stream_t"/></remarks>
		protected TcpSocket(Loop loop)
			: base(loop, uv_handle_type.UV_TCP)
		{
			CheckError(Uvi.uv_tcp_init(this.Loop.Handle, this.Handle));
		}
	}
}