using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace IoTClient.Tool
{
    public partial class UpdateLog : Form
    {
        public UpdateLog(bool hasNew)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            var logs = new List<string>();
            logs.Add("日志记录");
            logs.Add("\r\n版本：[0.4.0]");
            logs.Add($"时间：2020-09-11");
            logs.Add("内容");
            logs.Add("1.西门子PLC批量读写，大幅提高读写性能");
            logs.Add("2.手动检查更新");
            logs.Add("3.显示历史更新日志");

            logs.Add("\r\n版本：[0.4.1]");
            logs.Add($"时间：2021-01-19");
            logs.Add("内容");
            logs.Add("1.西门子PLC批量写，服务端模拟的实现");
            logs.Add("2.西门子PLC批量写Byte类型bug修复");
            logs.Add("3.ModbusTcp批量读取");
            logs.Add("4.ModbusTcp线程安全读取");

            logs.Add("\r\n版本：[0.4.2]");
            logs.Add($"时间：2021-03-10");
            logs.Add("内容");
            logs.Add("1.修复相关bug");

            logs.Add("\r\n版本：[0.4.4]");
            logs.Add($"时间：2021-03-21");
            logs.Add("内容");
            logs.Add("1.Modbus 大小端设置");

            logs.Add("\r\n版本：[0.4.5]");
            logs.Add($"时间：2021-04-14");
            logs.Add("内容");
            logs.Add("1.三菱MC_Qna-3E帧客户端实现");
            logs.Add("2.三菱MC_A-1E帧客户端实现");
            logs.Add("3.三菱MC_Qna-3E帧模拟服务端实现");
            logs.Add("4.三菱MC_A-1E帧模拟服务端实现");

            logs.Add("\r\n版本：[0.4.6]");
            logs.Add($"时间：2021-04-25");
            logs.Add("内容");
            logs.Add("1.欧姆龙客户端实现");
            logs.Add("2.欧姆龙模拟服务端实现");

            logs.Add("\r\n版本：[0.4.7]");
            logs.Add($"时间：2021-05-04");
            logs.Add("内容");
            logs.Add("1.罗克韦尔AB Plc客户端实现");
            logs.Add("2.罗克韦尔AB Plc模拟服务端实现");
            logs.Add("3.界面参数保存");

            logs.Add("\r\n版本：[0.4.8]");
            logs.Add($"时间：2021-05-06");
            logs.Add("内容");
            logs.Add("1.折线图显示");

            logs.Add("\r\n版本：[0.4.9]");
            logs.Add($"时间：2021-05-23");
            logs.Add("内容");
            logs.Add("1.西门子插槽和机架号的配置");

            logs.Add("\r\n版本：[0.5.0]");
            logs.Add($"时间：2021-07-07");
            logs.Add("内容");
            logs.Add("1.西门子读写结果验证，友好提示plc中不存在点位");
            logs.Add("2.AllenBradley、OmronFinsTcp发送报文");
            logs.Add("3.ModbusRtu低波特率读写异常修复");
            logs.Add("4.OmronFins读写");
            logs.Add("5.IoTClient可域名连接");

            logs.Add("\r\n版本：[1.0.3]");
            logs.Add($"时间：2022-07-08");
            logs.Add("内容");
            logs.Add("1.Socket连接操作设置");
            logs.Add("2.BACnet切换网卡扫描");
            logs.Add("3.BACnet导出Excel扫描结果");

            logs.Add("\r\n版本：[1.0.8]");
            logs.Add($"时间：2022-07-22");
            logs.Add("内容");
            logs.Add("1.BACnet释放优先级的值");

            logs.Add("\r\n版本：[1.1.0]");
            logs.Add($"时间：2022-10-14");
            logs.Add("内容");
            logs.Add("1.Modbus按位读取如：1.11");
            logs.Add("2.Modbus异常码提示");

            logs.Add("\r\n版本：[1.1.01]");
            logs.Add($"时间：2025-09-24");
            logs.Add("内容");
            logs.Add("1.西门子PLC字符串读写");

            logs.Add("\r\n版本：[1.1.02]");
            logs.Add($"时间：2025-10-21");
            logs.Add("内容");
            logs.Add("1.MQTT配置持久储存");
            logs.Add("2.MQTT历史订阅可视");

            logs.Add("\r\n版本：[1.1.03]");
            logs.Add($"时间：2025-11-24");
            logs.Add("内容");
            logs.Add("1.BACnet可选中设备ip扫描");

            logs.Add("\r\n版本：[1.1.04]");
            logs.Add($"时间：2025-12-18");
            logs.Add("内容");
            logs.Add("1.fix - BACnet扫描后不能读写");

            textBox1.Text = string.Join("\r\n", logs);
            if (hasNew)
            {
                button1.Enabled = true;
                button1.Text = "自动更新";
            }
            else
            {
                button1.Enabled = false;
                button1.Text = "已是最新版本";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("https://github.com/zhaopeiym/IoTClient.Examples/releases");
            }
            catch (Exception) { }
        }
    }
}
