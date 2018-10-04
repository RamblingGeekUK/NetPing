using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using NetPing.Common;

namespace NetPing.AddIns
{
	class SystemInformationMenuItem : IContextMenuItemProvider
	{
		public void Execute(IPAddress ipAddress)
		{
			try
			{
				Process.Start("cmd.exe", "/k systeminfo /s " + ipAddress);
			}
			catch (Exception e)
			{
				MessageBox.Show("An error occurred while retrieving system information:\n\n" + e.Message, "System information", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public string Text
		{
			get { return "System Information"; }
		}
	}
}
