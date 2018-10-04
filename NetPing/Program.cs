using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using NetPing.Common;

namespace NetPing
{
	static class Program
	{
		public static readonly List<IColumnProvider> ColumnProviders = new List<IColumnProvider>();
		public static readonly List<IContextMenuItemProvider> ContextMenuItemProviders = new List<IContextMenuItemProvider>();
		
		[STAThread]
		static void Main()
		{
			Application.ThreadException += delegate(object sender, ThreadExceptionEventArgs e)
			{
				MessageBox.Show("The following exception occurred:\n\n" + e.Exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			};
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}