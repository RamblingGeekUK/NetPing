using System.Collections.Generic;
using System.Management;
using System.Net;
using NetPing.Common;

namespace NetPing.AddIns
{
	public class OperatingSystemColumn : ColumnProviderBase
	{
		public OperatingSystemColumn() 
			: base("Operating System", 250)
		{}

		public override ColumnValue Execute(IPAddress ipAddress)
		{
			ManagementScope scope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", ipAddress));      
			ObjectQuery oq = new ObjectQuery("SELECT Caption, BuildNumber, CSDVersion FROM Win32_OperatingSystem");
			ManagementObjectSearcher query = new ManagementObjectSearcher(scope, oq);

			ColumnValue columnValue = new ColumnValue(ColumnIndex, "");

			try
			{
				ManagementObjectCollection managementObjects = query.Get();
				
				foreach (ManagementObject mo in managementObjects)
				{
					Dictionary<string, object> propValues = new Dictionary<string, object>();

					foreach (PropertyData propData in mo.Properties)
					{
						propValues.Add(propData.Name, propData.Value);
					}

					string version = (string)propValues["Caption"];
					string build = (string)propValues["BuildNumber"];

					if (propValues["CSDVersion"] != null)
					{
						version += " (" + propValues["CSDVersion"] + ")";
					}

					columnValue = new ColumnValue(ColumnIndex, version, build);
				}
			}
			catch
			{
				columnValue = new ColumnValue(ColumnIndex, "");
			}


			return columnValue;
		}
	}
}
