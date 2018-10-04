using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using NetPing.Common;

namespace NetPing.AddIns
{
    class BrowserMenuItem : IContextMenuItemProvider
    {
        public void Execute(IPAddress ipAddress)
        {
            try
            {
                Process.Start("http://" + ipAddress.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred while trying to connect:\n\n" + e.Message, "Cannot connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string Text
        {
            get { return "Browse"; }
        }
    }
}
