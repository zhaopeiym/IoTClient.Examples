using Talk.NPOI;

namespace IoTClient.Tool.Model
{
    public class BacnetPropertyInfo
    {
        [Alias("设备地址")]
        public string IpAddress { get; set; }

        [Alias("地址")]
        public string Address { get; set; }

        [Alias("数据类型")]
        public string DataType { get; set; }

        [Alias("值")]
        public string Value { get; set; }

        //[Alias("读写")]//暂不导出
        public string ReadWrite { get; set; }

        [Alias("点名")]
        public string PropName { get; set; }

        [Alias("描述")]
        public string Describe { get; set; }

        [Alias("ObjectType")]
        public string ObjectType { get; set; }
    }
}
