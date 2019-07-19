using System;
using System.Collections.Generic;
using System.Text;
using JKang.IpcServiceFramework;
using Newtonsoft.Json;

namespace Alex.GuiDebugger.Common.Services
{
	public class GuiDebuggerIpcMessageSerializer : IIpcMessageSerializer
	{
		public IpcRequest DeserializeRequest(byte[] binary)
		{
			return Deserialize<IpcRequest>(binary);
		}

		public IpcResponse DeserializeResponse(byte[] binary)
		{
			return Deserialize<IpcResponse>(binary);
		}

		public byte[] SerializeRequest(IpcRequest request)
		{
			return Serialize(request);
		}

		public byte[] SerializeResponse(IpcResponse response)
		{
			return Serialize(response);
		}

		private T Deserialize<T>(byte[] binary)
		{
			string json = Encoding.BigEndianUnicode.GetString(binary);
			return JsonConvert.DeserializeObject<T>(json, GuiDebuggerJsonSerializer.Settings);
		}

		private byte[] Serialize(object obj)
		{
			string json = JsonConvert.SerializeObject(obj, GuiDebuggerJsonSerializer.Settings);
			return Encoding.BigEndianUnicode.GetBytes(json);
		}
	}
}
