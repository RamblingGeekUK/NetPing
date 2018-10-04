using System.Net;

namespace NetPing.Common
{
	/// <summary>
	/// Represents an item that provides a context-menu action for a host.
	/// </summary>
	public interface IContextMenuItemProvider
	{
		void	Execute(IPAddress ipAddress);
		string	Text { get; }
	}
}
