using System.Net;
using System.Windows.Forms;

namespace NetPing.Common
{
	/// <summary>
	/// Represents an item that provides a column and its values in the NetPing list view.
	/// </summary>
	public interface IColumnProvider
	{
		void				Initialize(int columnIndex);
		ColumnValue			Execute(IPAddress ipAddress);
		string				HeaderText { get; }
		int					DefaultWidth { get; }
		HorizontalAlignment	Alignment { get; }
		int					ColumnIndex { get; }
	}
}
