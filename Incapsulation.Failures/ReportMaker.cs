using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    enum FailureType : int
    {
        UnexpectedShutdown,
        ShortNonResponding,
        HardwareFailures,
        ConnectionProblems
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var date = new DateTime(day, month, year);
            var devicesList = new List<Device>();
            var dateList = new List<DateTime>();
            foreach(var item in devices)
                devicesList.Add(new Device((int) item["DeviceId"], item["Name"].ToString()));

            foreach(var item in times)
                dateList.Add(new DateTime((int)item[0], (int)item[1], (int)item[2]));

            return FindDevicesFailedBeforeDate(date, devicesList, dateList);
        }


        public static List<string> FindDevicesFailedBeforeDate(DateTime date, 
            List<Device> devices, List<DateTime> dates)
        {
            var problematicDevices = new HashSet<int>();
            for (int i = 0; i < dates.Count; i++)
                if (i % 2 == 0 && dates[i] < date)
                    problematicDevices.Add(devices[i].Id);

            var result = new List<string>();
            foreach (var device in devices)
                if (problematicDevices.Contains(device.Id))
                    result.Add(device.Name);


            return result;
        }
    }

    public class Device
    {   
        public int Id { get; set; }
        public string Name { get; set; }

        public Device(int id, string name)
        {
            Id = id;
            Name = name;
        }   
    }


    public class DateTime
    {
        public int Year;
        public int Month;
        public int Day;

        public DateTime(int day, int month, int year)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public static bool operator<(DateTime d1, DateTime d2)
        {
            return !(d1.Year > d2.Year || d1.Month > d2.Month || d1.Day > d2.Day);
        }
        public static bool operator >(DateTime d1, DateTime d2)
        {
            return !(d1.Year < d2.Year || d1.Month < d2.Month || d1.Day < d2.Day);
        }
    }
}
