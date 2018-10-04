using System.Net;
using System.Windows.Forms;

namespace NetPing.Common
{
	/// <summary>
	/// A base class that may be used by column providers.
	/// </summary>
	public abstract class ColumnProviderBase : IColumnProvider
	{
		private int		_columnIndex;
		private string	_headerText;
		private int		_defaultWidth;

		protected ColumnProviderBase(string headerText)
			: this(headerText, 100)
		{}

		protected ColumnProviderBase(string headerText, int defaultWidth)
		{
			_headerText = headerText;
			_defaultWidth = defaultWidth;
		}

		public abstract ColumnValue Execute(IPAddress ipAddress);

		public virtual string HeaderText
		{
			get { return _headerText; }
		}

		public virtual void Initialize(int columnIndex)
		{
			_columnIndex = columnIndex;
		}

		public virtual int DefaultWidth
		{
			get { return _defaultWidth; }
		}

		public virtual HorizontalAlignment Alignment
		{
			get { return HorizontalAlignment.Left; }
		}

		public int ColumnIndex
		{
			get { return _columnIndex; }
		}
	}
}
