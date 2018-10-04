using System.Net;
using NetPing.Common;

namespace NetPing.AddIns
{
	class ComputerNameColumn : ColumnProviderBase
	{
		public ComputerNameColumn() 
			: base("Name", 125)
		{}

		public override ColumnValue Execute(IPAddress ipAddress)
		{
			IPHostEntry hostEntry;

			try
			{
				hostEntry = Dns.GetHostEntry(ipAddress);
			}
			catch 
			{
				hostEntry = null;
			}

			string hostName;

			if (hostEntry == null)
			{
				hostName = "";
			}
			else
			{
				hostName = hostEntry.HostName;
			}

			return new ColumnValue(ColumnIndex, hostName);
		}
	}
}
