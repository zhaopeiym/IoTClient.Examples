namespace IoTClient.Tool.Model
{
    public class VersionCheckInput
    {
        public int AppVersion { get; set; } = 1103;
        public string AppCode { get; set; } = "iotclient";
        public bool? IsRelease { get; set; }
    }
}
