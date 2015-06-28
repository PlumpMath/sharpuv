﻿using System;
using System.Net;

namespace SharpUV
{
	internal abstract class UvCallback<TArgs> where TArgs : UvArgs
	{
        private readonly object _parent;
		protected Action<TArgs> _callback;

		protected UvCallback(object sender, Action<TArgs> callback)
		{
			_callback = callback;
            _parent = sender;
		}

		protected abstract TArgs CreateArgs(int code);

	    internal Action<TArgs> Callback
	    {
            get { return _callback; }
	    }

		public void Invoke(int code, Action<TArgs> callback, EventHandler<TArgs> handler)
		{
			this.Invoke(this.CreateArgs(code), callback, handler);
		}

		protected void Invoke(TArgs args, Action<TArgs> callback, EventHandler<TArgs> handler)
		{
			if (callback != null)
				callback.Invoke(args);

			if (_callback != null)
				_callback.Invoke(args);

			if (handler != null)
				handler(_parent, args);
		}
	}

	internal class UvCallback : UvCallback<UvArgs>
	{
		internal UvCallback(object sender, Action<UvArgs> callback)
			: base(sender, callback)
		{
		}

		protected override UvArgs CreateArgs (int code)
		{
			return new UvArgs(code);
		}
	}

	internal class UvDataCallback : UvCallback<UvDataArgs>
	{
		private byte[] _data;

		internal UvDataCallback(object sender, Action<UvDataArgs> callback, byte[] data = null)
			: base(sender, callback)
		{
			_data = data;
		}

		protected override UvDataArgs CreateArgs (int code)
		{
			return new UvDataArgs(code, _data);
		}

        public void Invoke(UvDataArgs args, Action<UvDataArgs> callback, EventHandler<UvDataArgs> handler)
        {
			base.Invoke(args, callback, handler);
        }
	}
}
