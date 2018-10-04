using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NetPing.Common;

namespace NetPing.AddIns
{
	public class UptimeColumn : ColumnProviderBase
	{
		public UptimeColumn() 
			: base("Uptime", 200)
		{}

		public override ColumnValue Execute(IPAddress ipAddress)
		{
			ManagementScope scope = new ManagementScope(string.Format("\\\\{0}\\root\\cimv2", ipAddress));      
			ObjectQuery oq = new ObjectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem");
			ManagementObjectSearcher query = new ManagementObjectSearcher(scope, oq);

			ColumnValue columnValue = new ColumnValue(ColumnIndex, "");

			try
			{
				//MessageBox.Show(Application.user);
				ManagementObjectCollection managementObjects = query.Get();
				
				foreach (ManagementObject mo in managementObjects)
				{
					Dictionary<string, object> propValues = new Dictionary<string, object>();

					foreach (PropertyData propData in mo.Properties)
					{
						propValues.Add(propData.Name, propData.Value);
					}

					string lastBootUpTime = (string)propValues["LastBootUpTime"];

					DateTime bootTime = GetLocalTimeFromWmiDate(lastBootUpTime);
					TimeSpan uptime = DateTime.Now - bootTime;

					columnValue = new ColumnValue(ColumnIndex, GetEnglishTimeSpan(uptime), lastBootUpTime);
				}
			}
			catch (Exception e)
			{
				columnValue = new ColumnValue(ColumnIndex, e.Message);
			}


			return columnValue;
		}

		private static DateTime GetLocalTimeFromWmiDate(string lastBootUpTime)
		{
			int utcMinuteOffset = int.Parse(lastBootUpTime.Substring(lastBootUpTime.IndexOf('-') + 1));
			bool subtractMinutes = lastBootUpTime[21] == '+';

			// Remove the fractions of a second.
			lastBootUpTime = lastBootUpTime.Substring(0, lastBootUpTime.IndexOf('.'));

			DateTime bootDateTime = DateTime.ParseExact(lastBootUpTime, "yyyyMMddHHmmss", null);

			if (subtractMinutes)
			{
				utcMinuteOffset = -utcMinuteOffset;
			}

			bootDateTime = bootDateTime.AddMinutes(utcMinuteOffset);

			return bootDateTime.ToLocalTime();
		}

	
		private static string GetEnglishTimeSpan(TimeSpan span)
		{
			StringBuilder sb = new StringBuilder();

			if (span.Days > 0)
			{
				sb.Append(GetTimePart("Day", span.Days) + ", ");
			}
			if (span.Hours > 0)
			{
				sb.Append(GetTimePart("Hour", span.Hours) + ", ");
			}
			if (span.Minutes > 0)
			{
				sb.Append(GetTimePart("Minute", span.Minutes) + ", ");
			}

			sb.Append(GetTimePart("Second", span.Seconds));

			return sb.ToString();
		}

		private static string GetTimePart(string partName, int value)
		{
			return string.Format("{0} {1}{2}", value, partName, value == 1 ? "" : "s");
		}
	}
}
