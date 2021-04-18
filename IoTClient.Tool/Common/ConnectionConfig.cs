using Newtonsoft.Json;
using System.IO;

namespace IoTClient.Tool.Common
{
    public class ConnectionConfig
    {
        public string ModBusTcp_IP;
        public string ModBusTcp_Port;
        public string ModBusTcp_Address;
        public string ModBusTcp_Value;
        public bool ModBusTcp_ShowPackage;

        public string ModBusRtu_IP;
        public string ModBusRtu_Port;
        public string ModBusRtu_Address;
        public string ModBusRtu_Value;
        public bool ModBusRtu_ShowPackage;

        public string ModBusAscii_IP;
        public string ModBusAscii_Port;
        public string ModBusAscii_Address;
        public string ModBusAscii_Value;
        public bool ModBusAscii_ShowPackage;

        public string S7200_IP;
        public string S7200_Port;
        public string S7200_Address;
        public string S7200_Value;
        public bool S7200_ShowPackage;

        public string S7200Smart_IP;
        public string S7200Smart_Port;
        public string S7200Smart_Address;
        public string S7200Smart_Value;
        public bool S7200Smart_ShowPackage;

        public string S7300_IP;
        public string S7300_Port;
        public string S7300_Address;
        public string S7300_Value;
        public bool S7300_ShowPackage;

        public string S7400_IP;
        public string S7400_Port;
        public string S7400_Address;
        public string S7400_Value;
        public bool S7400_ShowPackage;

        public string S71200_IP;
        public string S71200_Port;
        public string S71200_Address;
        public string S71200_Value;
        public bool S71200_ShowPackage;

        public string S71500_IP;
        public string S71500_Port;
        public string S71500_Address;
        public string S71500_Value;
        public bool S71500_ShowPackage;

        public string MitsubishiQna3E_IP;
        public string MitsubishiQna3E_Port;
        public string MitsubishiQna3E_Address;
        public string MitsubishiQna3E_Value;
        public bool MitsubishiQna3E_ShowPackage;

        public string MitsubishiA1E_IP;
        public string MitsubishiA1E_Port;
        public string MitsubishiA1E_Address;
        public string MitsubishiA1E_Value;
        public bool MitsubishiA1E_ShowPackage;

        public string OmronFins_IP;
        public string OmronFins_Port;
        public string OmronFins_Address;
        public string OmronFins_Value;
        public bool OmronFins_ShowPackage;

        public static ConnectionConfig GetConfig()
        {
            var dataString = string.Empty;
            var path = @"C:\IoTClient";
            var filePath = path + @"\ConnectionConfig.Data";
            if (File.Exists(filePath))
                dataString = File.ReadAllText(filePath);
            else
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.SetAttributes(path, FileAttributes.Hidden);
            }
            return JsonConvert.DeserializeObject<ConnectionConfig>(dataString) ?? new ConnectionConfig();
        }

        public void SaveConfig()
        {
            var dataString = JsonConvert.SerializeObject(this);
            var path = @"C:\IoTClient";
            var filePath = path + @"\ConnectionConfig.Data";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    sw.Write(dataString);
                }
            }
        }
    }
}