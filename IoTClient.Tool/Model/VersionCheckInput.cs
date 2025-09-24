namespace IoTClient.Tool.Model
{
    public class VersionCheckInput
    {
        ///// <summary>
        ///// 当前版本
        ///// </summary>
        //public float CurrentVersion { get; set; } = 1.20f;

        ///// <summary>
        ///// 忽略版本
        ///// </summary>
        //public float? IgnoreVersion { get; set; }

        public int AppVersion { get; set; } = 1101;
        public string AppCode { get; set; } = "iotclient";
        public bool? IsRelease { get; set; }
    }
}
