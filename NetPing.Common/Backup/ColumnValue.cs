namespace NetPing.Common
{
	/// <summary>
	/// Represents a column value.
	/// </summary>
	public class ColumnValue
	{
		private string	_text;
		private string	_sortKey;
		private int		_columnIndex;

		public ColumnValue(int index, string text)
			: this(index, text, text)
		{}

		public ColumnValue(int index, string text, string sortKey)
		{
			_columnIndex = index;
			_text = text;
			_sortKey = sortKey;
		}

		/// <summary>
		/// The text of the column.
		/// </summary>
		public string Text
		{
			get { return _text; }
		}

		/// <summary>
		/// The string used for comparing the value to another in the same column. Used for sorting.
		/// </summary>
		public string SortKey
		{
			get { return _sortKey; }
		}

		/// <summary>
		/// The index of the value.
		/// </summary>
		public int ColumnIndex
		{
			get { return _columnIndex; }
		}
	}
}
