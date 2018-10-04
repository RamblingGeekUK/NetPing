using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;
using NetPing.Common;
using NetPing.Properties;

namespace NetPing
{
	public partial class MainForm : Form
	{
		// The IP addresses are initially displayed in order, so it is effectively already sorted by the address (column 0) in
		// ascending order.
		private int			_lastSortedColumnIndex = 0;				
		private SortOrder	_lastSortOrder = SortOrder.Ascending;

		public MainForm()
		{
			InitializeComponent();

			Icon = Resources.NetPing;

			imageList.Images.Add(Resources.Unavailable);
			imageList.Images.Add(Resources.Available);

			// Set up event handlers to update end address label when start address text boxes are changed.
			startAddress1TextBox.TextChanged += UpdateEndAddressLabel;
			startAddress2TextBox.TextChanged += UpdateEndAddressLabel;
			startAddress3TextBox.TextChanged += UpdateEndAddressLabel;

			// Find user's IP address and populate start textboxes with first three octets.
			foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
			{
				if (address.AddressFamily == AddressFamily.InterNetwork)
				{
					byte[] bytes = address.GetAddressBytes();

					startAddress1TextBox.Text = bytes[0].ToString();
					startAddress2TextBox.Text = bytes[1].ToString();
					startAddress3TextBox.Text = bytes[2].ToString();
					break;
				}
			}

			LoadAddIns();
		}

		private void LoadAddIns()
		{
			string iColumnProviderName = typeof(IColumnProvider).FullName;
			string iContextMenuItemProviderName = typeof(IContextMenuItemProvider).FullName;
			
			DirectoryInfo addInDirInfo = new DirectoryInfo(Path.Combine(Application.StartupPath, "AddIns"));

			if (addInDirInfo.Exists)
			{
				// Get all of the assemblies in the AddIns directory
				foreach (FileInfo fileInfo in addInDirInfo.GetFiles("*.dll"))
				{
					// Load the assembly and iterate through all of the types in it.
					Assembly assembly = Assembly.LoadFrom(fileInfo.FullName);

					foreach (Type type in assembly.GetTypes())
					{
						// If the type implements IColumnProvider, create an instance of it and add the column it represents to the list view.
						if (type.GetInterface(iColumnProviderName) != null)
						{
							IColumnProvider columnProvider = (IColumnProvider)Activator.CreateInstance(type);
							Program.ColumnProviders.Add(columnProvider);

							ColumnHeader columnHeader = listView.Columns.Add(columnProvider.HeaderText, columnProvider.DefaultWidth, columnProvider.Alignment);
							columnProvider.Initialize(columnHeader.Index);
						}

						// If the type implements IContextMenuItemProvider, create an instance of it and add it to the context menu.
						if (type.GetInterface(iContextMenuItemProviderName) != null)
						{
							IContextMenuItemProvider menuItemProvider = (IContextMenuItemProvider)Activator.CreateInstance(type);
							Program.ContextMenuItemProviders.Add(menuItemProvider);

							MenuItemProviderMenuItem contextMenuItem = new MenuItemProviderMenuItem(menuItemProvider);
							contextMenuItem.Click += contextMenuItem_Click;
							contextMenu.Items.Add(contextMenuItem);
						}
					}
				}
			}
		}

		private void contextMenuItem_Click(object sender, EventArgs e)
		{
			if (listView.SelectedItems.Count == 1)
			{
				MenuItemProviderMenuItem menuItem = (MenuItemProviderMenuItem)sender;
				HostListViewItem listViewItem = (HostListViewItem)listView.SelectedItems[0];
				menuItem.MenuItemProvider.Execute(listViewItem.IpAddress);
			}
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			listView.Items.Clear();

			byte[] address = {byte.Parse(startAddress1TextBox.Text), byte.Parse(startAddress2TextBox.Text), byte.Parse(startAddress3TextBox.Text), 0};
			byte max = Math.Max(byte.Parse(startAddress4TextBox.Text), byte.Parse(endAddressTextBox.Text));
			byte min = Math.Min(byte.Parse(startAddress4TextBox.Text), byte.Parse(endAddressTextBox.Text));

			listView.BeginUpdate();

			for (int i = min; i <= max; i++)
			{
				address[3] = (byte)i;

				// Create and add a new list view item to the list view.
				HostListViewItem lvItem = new HostListViewItem(new IPAddress(address));
				listView.Items.Add(lvItem);
				
				// Instruct the list view item to ping, which kicks off the process of filling in the columns.
				lvItem.Ping();
			}

			listView.EndUpdate();
		}

		private void UpdateEndAddressLabel(object sender, EventArgs e)
		{
			endAddressLabel.Text = string.Format("{0}.{1}.{2}.", startAddress1TextBox.Text, startAddress2TextBox.Text, startAddress3TextBox.Text);
		}

		private void listView_MouseUp(object sender, MouseEventArgs e)
		{
			// If the user right-clicks an item, show the context menu.
			if (e.Button == MouseButtons.Right)
			{
				HostListViewItem item = (HostListViewItem)listView.GetItemAt(e.X, e.Y);

				if (item != null && item.Selected)
				{
					contextMenu.Show(listView.PointToScreen(e.Location));
				}
			}
		}

		private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			int index = e.Column;

			// If the user clicked the same column twice in succession, change the sort order.
			if (index == _lastSortedColumnIndex)
			{
				_lastSortOrder = _lastSortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
			}
			else
			{
				_lastSortOrder = SortOrder.Ascending;
			}

			// Sort the list view
			listView.ListViewItemSorter = new HostListViewItemComparer(index, _lastSortOrder);

			_lastSortedColumnIndex = index;
		}
	}
}