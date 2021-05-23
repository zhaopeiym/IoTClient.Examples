using IoTClient.Clients.PLC;
using IoTClient.Common.Enums;
using IoTClient.Common.Helpers;
using IoTClient.Enums;
using IoTClient.Tool.Common;
using IoTServer.Common;
using IoTServer.Servers.PLC;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Talk.Linq.Extensions;

namespace IoTClient.Tool
{
    public partial class SiemensControl : UserControl
    {
        SiemensClient client;
        SiemensServer server;
        private SiemensVersion siemensVersion;
        public SiemensControl(SiemensVersion siemensVersion)
        {
            this.siemensVersion = siemensVersion;
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            Size = new Size(880, 450);
            groupBox2.Location = new Point(13, 5);
            groupBox2.Size = new Size(855, 50);
            groupBox1.Location = new Point(13, 55);
            groupBox1.Size = new Size(855, 50);
            groupBox3.Location = new Point(13, 105);
            groupBox3.Size = new Size(855, 50);
            txt_content.Location = new Point(13, 160);

            lab_address.Location = new Point(9, 22);
            txt_address.Location = new Point(39, 18);
            txt_address.Size = new Size(88, 21);
            but_read.Location = new Point(132, 17);

            but_brokenline.Location = new Point(209, 17);

            lab_value.Location = new Point(297, 22);
            txt_value.Location = new Point(319, 18);
            txt_value.Size = new Size(74, 21);
            but_write.Location = new Point(398, 17);

            txt_dataPackage.Location = new Point(490, 18);
            txt_dataPackage.Size = new Size(186, 21);
            but_sendData.Location = new Point(680, 17);

            chb_show_package.Location = new Point(776, 20);

            but_read.Enabled = false;
            but_brokenline.Enabled = false;
            but_write.Enabled = false;
            but_close_server.Enabled = false;
            but_close.Enabled = false;
            but_sendData.Enabled = false;

            toolTip1.SetToolTip(but_open, "点击打开连接");
            toolTip1.SetToolTip(txt_address, "支持批量读取，如V2634、V2638、V2642");
            txt_content.Text = @"小技巧:
1、读取地址支持批量读取，如V2634、V2638、V2642。
2、关于PLC地址：VB263、VW263、VD263中的B、W、D分别表示byte、word、doubleword数据类型，分别对应C#中的byte、ushort(UInt16)、uint(UInt32)类型。此工具直接传入地址（如:V263）即可。";

            var config = ConnectionConfig.GetConfig();

            switch (siemensVersion)
            {
                case SiemensVersion.S7_200:
                    if (!string.IsNullOrWhiteSpace(config.S7200_IP)) txt_ip.Text = config.S7200_IP;
                    if (!string.IsNullOrWhiteSpace(config.S7200_Port)) txt_port.Text = config.S7200_Port;
                    if (!string.IsNullOrWhiteSpace(config.S7200_Address)) txt_address.Text = config.S7200_Address;
                    if (!string.IsNullOrWhiteSpace(config.S7200_Value)) txt_value.Text = config.S7200_Value;
                    if (!string.IsNullOrWhiteSpace(config.S7200_Slot)) txt_slot.Text = config.S7200_Slot;
                    if (!string.IsNullOrWhiteSpace(config.S7200_Rack)) txt_rack.Text = config.S7200_Rack;
                    chb_show_package.Checked = config.S7200_ShowPackage;
                    switch (config.S7200_Datatype)
                    {
                        case "rd_bit": rd_bit.Checked = true; break;
                        case "rd_short": rd_short.Checked = true; break;
                        case "rd_ushort": rd_ushort.Checked = true; break;
                        case "rd_int": rd_int.Checked = true; break;
                        case "rd_uint": rd_uint.Checked = true; break;
                        case "rd_long": rd_long.Checked = true; break;
                        case "rd_ulong": rd_ulong.Checked = true; break;
                        case "rd_float": rd_float.Checked = true; break;
                        case "rd_double": rd_double.Checked = true; break;
                    };
                    break;
                case SiemensVersion.S7_200Smart:
                    if (!string.IsNullOrWhiteSpace(config.S7200Smart_IP)) txt_ip.Text = config.S7200Smart_IP;
                    if (!string.IsNullOrWhiteSpace(config.S7200Smart_Port)) txt_port.Text = config.S7200Smart_Port;
                    if (!string.IsNullOrWhiteSpace(config.S7200Smart_Address)) txt_address.Text = config.S7200Smart_Address;
                    if (!string.IsNullOrWhiteSpace(config.S7200Smart_Value)) txt_value.Text = config.S7200Smart_Value;
                    if (!string.IsNullOrWhiteSpace(config.S7200Smart_Slot)) txt_slot.Text = config.S7200Smart_Slot;
                    if (!string.IsNullOrWhiteSpace(config.S7200Smart_Rack)) txt_rack.Text = config.S7200Smart_Rack;
                    chb_show_package.Checked = config.S7200Smart_ShowPackage;
                    switch (config.S7200Smart_Datatype)
                    {
                        case "rd_bit": rd_bit.Checked = true; break;
                        case "rd_short": rd_short.Checked = true; break;
                        case "rd_ushort": rd_ushort.Checked = true; break;
                        case "rd_int": rd_int.Checked = true; break;
                        case "rd_uint": rd_uint.Checked = true; break;
                        case "rd_long": rd_long.Checked = true; break;
                        case "rd_ulong": rd_ulong.Checked = true; break;
                        case "rd_float": rd_float.Checked = true; break;
                        case "rd_double": rd_double.Checked = true; break;
                    };
                    break;
                case SiemensVersion.S7_300:
                    if (!string.IsNullOrWhiteSpace(config.S7300_IP)) txt_ip.Text = config.S7300_IP;
                    if (!string.IsNullOrWhiteSpace(config.S7300_Port)) txt_port.Text = config.S7300_Port;
                    if (!string.IsNullOrWhiteSpace(config.S7300_Address)) txt_address.Text = config.S7300_Address;
                    if (!string.IsNullOrWhiteSpace(config.S7300_Value)) txt_value.Text = config.S7300_Value;
                    if (!string.IsNullOrWhiteSpace(config.S7300_Slot)) txt_slot.Text = config.S7300_Slot;
                    if (!string.IsNullOrWhiteSpace(config.S7300_Rack)) txt_rack.Text = config.S7300_Rack;
                    chb_show_package.Checked = config.S7300_ShowPackage;
                    switch (config.S7300_Datatype)
                    {
                        case "rd_bit": rd_bit.Checked = true; break;
                        case "rd_short": rd_short.Checked = true; break;
                        case "rd_ushort": rd_ushort.Checked = true; break;
                        case "rd_int": rd_int.Checked = true; break;
                        case "rd_uint": rd_uint.Checked = true; break;
                        case "rd_long": rd_long.Checked = true; break;
                        case "rd_ulong": rd_ulong.Checked = true; break;
                        case "rd_float": rd_float.Checked = true; break;
                        case "rd_double": rd_double.Checked = true; break;
                    };
                    break;
                case SiemensVersion.S7_400:
                    if (!string.IsNullOrWhiteSpace(config.S7400_IP)) txt_ip.Text = config.S7400_IP;
                    if (!string.IsNullOrWhiteSpace(config.S7400_Port)) txt_port.Text = config.S7400_Port;
                    if (!string.IsNullOrWhiteSpace(config.S7400_Address)) txt_address.Text = config.S7400_Address;
                    if (!string.IsNullOrWhiteSpace(config.S7400_Value)) txt_value.Text = config.S7400_Value;
                    if (!string.IsNullOrWhiteSpace(config.S7400_Slot)) txt_slot.Text = config.S7400_Slot;
                    if (!string.IsNullOrWhiteSpace(config.S7400_Rack)) txt_rack.Text = config.S7400_Rack;
                    chb_show_package.Checked = config.S7400_ShowPackage;
                    switch (config.S7400_Datatype)
                    {
                        case "rd_bit": rd_bit.Checked = true; break;
                        case "rd_short": rd_short.Checked = true; break;
                        case "rd_ushort": rd_ushort.Checked = true; break;
                        case "rd_int": rd_int.Checked = true; break;
                        case "rd_uint": rd_uint.Checked = true; break;
                        case "rd_long": rd_long.Checked = true; break;
                        case "rd_ulong": rd_ulong.Checked = true; break;
                        case "rd_float": rd_float.Checked = true; break;
                        case "rd_double": rd_double.Checked = true; break;
                    };
                    break;
                case SiemensVersion.S7_1200:
                    if (!string.IsNullOrWhiteSpace(config.S71200_IP)) txt_ip.Text = config.S71200_IP;
                    if (!string.IsNullOrWhiteSpace(config.S71200_Port)) txt_port.Text = config.S71200_Port;
                    if (!string.IsNullOrWhiteSpace(config.S71200_Address)) txt_address.Text = config.S71200_Address;
                    if (!string.IsNullOrWhiteSpace(config.S71200_Value)) txt_value.Text = config.S71200_Value;
                    if (!string.IsNullOrWhiteSpace(config.S71200_Slot)) txt_slot.Text = config.S71200_Slot;
                    if (!string.IsNullOrWhiteSpace(config.S71200_Rack)) txt_rack.Text = config.S71200_Rack;
                    chb_show_package.Checked = config.S71200_ShowPackage;
                    switch (config.S71200_Datatype)
                    {
                        case "rd_bit": rd_bit.Checked = true; break;
                        case "rd_short": rd_short.Checked = true; break;
                        case "rd_ushort": rd_ushort.Checked = true; break;
                        case "rd_int": rd_int.Checked = true; break;
                        case "rd_uint": rd_uint.Checked = true; break;
                        case "rd_long": rd_long.Checked = true; break;
                        case "rd_ulong": rd_ulong.Checked = true; break;
                        case "rd_float": rd_float.Checked = true; break;
                        case "rd_double": rd_double.Checked = true; break;
                    };
                    break;
                case SiemensVersion.S7_1500:
                    if (!string.IsNullOrWhiteSpace(config.S71500_IP)) txt_ip.Text = config.S71500_IP;
                    if (!string.IsNullOrWhiteSpace(config.S71500_Port)) txt_port.Text = config.S71500_Port;
                    if (!string.IsNullOrWhiteSpace(config.S71500_Address)) txt_address.Text = config.S71500_Address;
                    if (!string.IsNullOrWhiteSpace(config.S71500_Value)) txt_value.Text = config.S71500_Value;
                    if (!string.IsNullOrWhiteSpace(config.S71500_Slot)) txt_slot.Text = config.S71500_Slot;
                    if (!string.IsNullOrWhiteSpace(config.S71500_Rack)) txt_rack.Text = config.S71500_Rack;
                    chb_show_package.Checked = config.S71500_ShowPackage;
                    switch (config.S71500_Datatype)
                    {
                        case "rd_bit": rd_bit.Checked = true; break;
                        case "rd_short": rd_short.Checked = true; break;
                        case "rd_ushort": rd_ushort.Checked = true; break;
                        case "rd_int": rd_int.Checked = true; break;
                        case "rd_uint": rd_uint.Checked = true; break;
                        case "rd_long": rd_long.Checked = true; break;
                        case "rd_ulong": rd_ulong.Checked = true; break;
                        case "rd_float": rd_float.Checked = true; break;
                        case "rd_double": rd_double.Checked = true; break;
                    };
                    break;
            }
        }

        private void but_server_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_content.Text.Contains("小技巧")) txt_content.Text = string.Empty;
                server?.Stop();
                server = new SiemensServer(int.Parse(txt_port.Text.Trim()));
                server.Start();
                but_server.Enabled = false;
                but_close_server.Enabled = true;
                AppendText($"开启仿真模拟服务");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void but_close_server_Click(object sender, EventArgs e)
        {
            server?.Stop();
            but_server.Enabled = true;
            but_close_server.Enabled = false;
            AppendText($"关闭仿真模拟服务");
        }

        private void but_open_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    but_open.Text = "连接中...";
                    if (txt_content.Text.Contains("小技巧")) txt_content.Text = string.Empty;
                    client?.Close();
                    var slot = byte.Parse(txt_slot.Text?.Trim());
                    var rack = byte.Parse(txt_rack.Text?.Trim());
                    client = new SiemensClient(siemensVersion, txt_ip.Text?.Trim(), int.Parse(txt_port.Text.Trim()), slot, rack);
                    client.WarningLog = (msg, ex) =>
                    {
                        //MessageBox.Show(ex.Message);
                    };
                    var result = client.Open();

                    if (!result.IsSucceed)
                    {
                        MessageBox.Show($"连接失败：{result.Err}");
                        if (chb_show_package.Checked || (ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            AppendText($"[请求报文]{result.Requst}");
                            if (result.Response.IsAny())
                                AppendText($"[响应报文]{result.Response}");
                            if (result.Requst2.IsAny())
                                AppendText($"[请求报文]{result.Requst2}");
                            if (result.Response2.IsAny())
                                AppendText($"[响应报文]{result.Response2}\r\n");
                        }
                    }
                    else
                    {
                        but_read.Enabled = true;
                        but_brokenline.Enabled = true;
                        but_write.Enabled = true;
                        but_open.Enabled = false;
                        but_close.Enabled = true;
                        but_sendData.Enabled = true;
                        AppendText($"连接成功\t\t\t\t耗时：{result.TimeConsuming}ms");
                        if (chb_show_package.Checked || (ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            AppendText($"[请求报文]{result.Requst}");
                            AppendText($"[响应报文]{result.Response}");
                            AppendText($"[请求报文]{result.Requst2}");
                            AppendText($"[响应报文]{result.Response2}\r\n");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    but_open.Text = "连接";
                }
            });
        }

        private void but_close_Click(object sender, EventArgs e)
        {
            client?.Close();
            but_open.Enabled = true;
            but_close.Enabled = false;
            AppendText($"连接关闭");
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_read_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_address.Text))
                {
                    MessageBox.Show("请输入地址");
                    return;
                }
                dynamic result = null;

                var addressAndReadNumber = txt_address.Text.Split(',', '、', '，');

                //批量读取
                if (addressAndReadNumber.Length >= 2)
                {
                    DataTypeEnum datatype = DataTypeEnum.None;
                    if (rd_byte.Checked) datatype = DataTypeEnum.Byte;
                    else if (rd_bit.Checked) datatype = DataTypeEnum.Bool;
                    else if (rd_short.Checked) datatype = DataTypeEnum.Int16;
                    else if (rd_ushort.Checked) datatype = DataTypeEnum.UInt16;
                    else if (rd_int.Checked) datatype = DataTypeEnum.Int32;
                    else if (rd_uint.Checked) datatype = DataTypeEnum.UInt32;
                    else if (rd_long.Checked) datatype = DataTypeEnum.Int64;
                    else if (rd_ulong.Checked) datatype = DataTypeEnum.UInt64;
                    else if (rd_float.Checked) datatype = DataTypeEnum.Float;
                    else if (rd_double.Checked) datatype = DataTypeEnum.Double;

                    Dictionary<string, DataTypeEnum> addresses = new Dictionary<string, DataTypeEnum>();
                    foreach (var item in addressAndReadNumber)
                    {
                        addresses.Add(item, datatype);
                    }

                    result = client.BatchRead(addresses);

                    if (result.IsSucceed)
                    {
                        AppendEmptyText();
                        foreach (var item in result.Value)
                        {
                            AppendText($"[读取 {item.Key} 成功]：{item.Value}\t\t耗时：{result.TimeConsuming}ms");
                        }
                    }
                    else
                        AppendText($"[读取 {txt_address.Text?.Trim()} 失败]：{result.Err}\t\t耗时：{result.TimeConsuming}ms");
                }
                //单个读取
                else
                {
                    if (rd_byte.Checked)
                    {
                        result = client.ReadByte(txt_address.Text);
                    }
                    else if (rd_bit.Checked)
                    {
                        result = client.ReadBoolean(txt_address.Text);
                    }
                    else if (rd_short.Checked)
                    {
                        result = client.ReadInt16(txt_address.Text);
                    }
                    else if (rd_ushort.Checked)
                    {
                        result = client.ReadUInt16(txt_address.Text);
                    }
                    else if (rd_int.Checked)
                    {
                        result = client.ReadInt32(txt_address.Text);
                    }
                    else if (rd_uint.Checked)
                    {
                        result = client.ReadUInt32(txt_address.Text);
                    }
                    else if (rd_long.Checked)
                    {
                        result = client.ReadInt64(txt_address.Text);
                    }
                    else if (rd_ulong.Checked)
                    {
                        result = client.ReadUInt64(txt_address.Text);
                    }
                    else if (rd_float.Checked)
                    {
                        result = client.ReadFloat(txt_address.Text);
                    }
                    else if (rd_double.Checked)
                    {
                        result = client.ReadDouble(txt_address.Text);
                    }
                    if (result.IsSucceed)
                        AppendText($"[读取 {txt_address.Text?.Trim()} 成功]：{result.Value}\t\t耗时：{result.TimeConsuming}ms");
                    else
                        AppendText($"[读取 {txt_address.Text?.Trim()} 失败]：{result.Err}\t\t耗时：{result.TimeConsuming}ms");
                }

                if (chb_show_package.Checked || (ModifierKeys & Keys.Control) == Keys.Control)
                {
                    AppendText($"[请求报文]{result.Requst}");
                    AppendText($"[响应报文]{result.Response}\r\n");
                }

                var config = ConnectionConfig.GetConfig();
                switch (siemensVersion)
                {
                    case SiemensVersion.S7_200:
                        config.S7200_IP = txt_ip.Text;
                        config.S7200_Port = txt_port.Text;
                        config.S7200_Address = txt_address.Text;
                        config.S7200_Value = txt_value.Text;
                        config.S7200_Slot = txt_slot.Text;
                        config.S7200_Rack = txt_rack.Text;
                        config.S7200_ShowPackage = chb_show_package.Checked;
                        config.S7200_Datatype = string.Empty;
                        if (rd_bit.Checked) config.S7200_Datatype = "rd_bit";
                        else if (rd_short.Checked) config.S7200_Datatype = "rd_short";
                        else if (rd_ushort.Checked) config.S7200_Datatype = "rd_ushort";
                        else if (rd_int.Checked) config.S7200_Datatype = "rd_int";
                        else if (rd_uint.Checked) config.S7200_Datatype = "rd_uint";
                        else if (rd_long.Checked) config.S7200_Datatype = "rd_long";
                        else if (rd_ulong.Checked) config.S7200_Datatype = "rd_ulong";
                        else if (rd_float.Checked) config.S7200_Datatype = "rd_float";
                        else if (rd_double.Checked) config.S7200_Datatype = "rd_double";
                        break;
                    case SiemensVersion.S7_200Smart:
                        config.S7200Smart_IP = txt_ip.Text;
                        config.S7200Smart_Port = txt_port.Text;
                        config.S7200Smart_Address = txt_address.Text;
                        config.S7200Smart_Value = txt_value.Text;
                        config.S7200Smart_Slot = txt_slot.Text;
                        config.S7200Smart_Rack = txt_rack.Text;
                        config.S7200Smart_ShowPackage = chb_show_package.Checked;
                        config.S7200Smart_Datatype = string.Empty;
                        if (rd_bit.Checked) config.S7200Smart_Datatype = "rd_bit";
                        else if (rd_short.Checked) config.S7200Smart_Datatype = "rd_short";
                        else if (rd_ushort.Checked) config.S7200Smart_Datatype = "rd_ushort";
                        else if (rd_int.Checked) config.S7200Smart_Datatype = "rd_int";
                        else if (rd_uint.Checked) config.S7200Smart_Datatype = "rd_uint";
                        else if (rd_long.Checked) config.S7200Smart_Datatype = "rd_long";
                        else if (rd_ulong.Checked) config.S7200Smart_Datatype = "rd_ulong";
                        else if (rd_float.Checked) config.S7200Smart_Datatype = "rd_float";
                        else if (rd_double.Checked) config.S7200Smart_Datatype = "rd_double";
                        break;
                    case SiemensVersion.S7_300:
                        config.S7300_IP = txt_ip.Text;
                        config.S7300_Port = txt_port.Text;
                        config.S7300_Address = txt_address.Text;
                        config.S7300_Value = txt_value.Text;
                        config.S7300_Slot = txt_slot.Text;
                        config.S7300_Rack = txt_rack.Text;
                        config.S7300_ShowPackage = chb_show_package.Checked;
                        config.S7300_Datatype = string.Empty;
                        if (rd_bit.Checked) config.S7300_Datatype = "rd_bit";
                        else if (rd_short.Checked) config.S7300_Datatype = "rd_short";
                        else if (rd_ushort.Checked) config.S7300_Datatype = "rd_ushort";
                        else if (rd_int.Checked) config.S7300_Datatype = "rd_int";
                        else if (rd_uint.Checked) config.S7300_Datatype = "rd_uint";
                        else if (rd_long.Checked) config.S7300_Datatype = "rd_long";
                        else if (rd_ulong.Checked) config.S7300_Datatype = "rd_ulong";
                        else if (rd_float.Checked) config.S7300_Datatype = "rd_float";
                        else if (rd_double.Checked) config.S7300_Datatype = "rd_double";
                        break;
                    case SiemensVersion.S7_400:
                        config.S7400_IP = txt_ip.Text;
                        config.S7400_Port = txt_port.Text;
                        config.S7400_Address = txt_address.Text;
                        config.S7400_Value = txt_value.Text;
                        config.S7400_Slot = txt_slot.Text;
                        config.S7400_Rack = txt_rack.Text;
                        config.S7400_ShowPackage = chb_show_package.Checked;
                        config.S7400_Datatype = string.Empty;
                        if (rd_bit.Checked) config.S7400_Datatype = "rd_bit";
                        else if (rd_short.Checked) config.S7400_Datatype = "rd_short";
                        else if (rd_ushort.Checked) config.S7400_Datatype = "rd_ushort";
                        else if (rd_int.Checked) config.S7400_Datatype = "rd_int";
                        else if (rd_uint.Checked) config.S7400_Datatype = "rd_uint";
                        else if (rd_long.Checked) config.S7400_Datatype = "rd_long";
                        else if (rd_ulong.Checked) config.S7400_Datatype = "rd_ulong";
                        else if (rd_float.Checked) config.S7400_Datatype = "rd_float";
                        else if (rd_double.Checked) config.S7400_Datatype = "rd_double";
                        break;
                    case SiemensVersion.S7_1200:
                        config.S71200_IP = txt_ip.Text;
                        config.S71200_Port = txt_port.Text;
                        config.S71200_Address = txt_address.Text;
                        config.S71200_Value = txt_value.Text;
                        config.S71200_Slot = txt_slot.Text;
                        config.S71200_Rack = txt_rack.Text;
                        config.S71200_ShowPackage = chb_show_package.Checked;
                        config.S71200_Datatype = string.Empty;
                        if (rd_bit.Checked) config.S71200_Datatype = "rd_bit";
                        else if (rd_short.Checked) config.S71200_Datatype = "rd_short";
                        else if (rd_ushort.Checked) config.S71200_Datatype = "rd_ushort";
                        else if (rd_int.Checked) config.S71200_Datatype = "rd_int";
                        else if (rd_uint.Checked) config.S71200_Datatype = "rd_uint";
                        else if (rd_long.Checked) config.S71200_Datatype = "rd_long";
                        else if (rd_ulong.Checked) config.S71200_Datatype = "rd_ulong";
                        else if (rd_float.Checked) config.S71200_Datatype = "rd_float";
                        else if (rd_double.Checked) config.S71200_Datatype = "rd_double";
                        break;
                    case SiemensVersion.S7_1500:
                        config.S71500_IP = txt_ip.Text;
                        config.S71500_Port = txt_port.Text;
                        config.S71500_Address = txt_address.Text;
                        config.S71500_Value = txt_value.Text;
                        config.S71500_Slot = txt_slot.Text;
                        config.S71500_Rack = txt_rack.Text;
                        config.S71500_ShowPackage = chb_show_package.Checked;
                        config.S71500_Datatype = string.Empty;
                        if (rd_bit.Checked) config.S71500_Datatype = "rd_bit";
                        else if (rd_short.Checked) config.S71500_Datatype = "rd_short";
                        else if (rd_ushort.Checked) config.S71500_Datatype = "rd_ushort";
                        else if (rd_int.Checked) config.S71500_Datatype = "rd_int";
                        else if (rd_uint.Checked) config.S71500_Datatype = "rd_uint";
                        else if (rd_long.Checked) config.S71500_Datatype = "rd_long";
                        else if (rd_ulong.Checked) config.S71500_Datatype = "rd_ulong";
                        else if (rd_float.Checked) config.S71500_Datatype = "rd_float";
                        else if (rd_double.Checked) config.S71500_Datatype = "rd_double";
                        break;
                }
                config.SaveConfig();
            }
            catch (Exception ex)
            {
                //client?.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private async void but_brokenline_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                var constant = new BrokenLineChart(txt_address.Text);
                constant.Show();
                while (!constant.IsDisposed)
                {
                    await Task.Delay(800);

                    dynamic result = null;
                    if (rd_byte.Checked)
                    {
                        result = client.ReadByte(txt_address.Text);
                    }
                    else if (rd_bit.Checked)
                    {
                        result = client.ReadBoolean(txt_address.Text);
                    }
                    else if (rd_short.Checked)
                    {
                        result = client.ReadInt16(txt_address.Text);
                    }
                    else if (rd_ushort.Checked)
                    {
                        result = client.ReadUInt16(txt_address.Text);
                    }
                    else if (rd_int.Checked)
                    {
                        result = client.ReadInt32(txt_address.Text);
                    }
                    else if (rd_uint.Checked)
                    {
                        result = client.ReadUInt32(txt_address.Text);
                    }
                    else if (rd_long.Checked)
                    {
                        result = client.ReadInt64(txt_address.Text);
                    }
                    else if (rd_ulong.Checked)
                    {
                        result = client.ReadUInt64(txt_address.Text);
                    }
                    else if (rd_float.Checked)
                    {
                        result = client.ReadFloat(txt_address.Text);
                    }
                    else if (rd_double.Checked)
                    {
                        result = client.ReadDouble(txt_address.Text);
                    }
                    if (result.IsSucceed)
                    {
                        constant.AddData(result.Value);
                    }
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        ///  写入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_write_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_address.Text))
            {
                MessageBox.Show("请输入地址");
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_value.Text))
            {
                MessageBox.Show("请输入值");
                return;
            }

            try
            {
                var address = txt_address.Text.Split('-')[0];
                dynamic result = null;
                if (rd_bit.Checked)
                {
                    if (!bool.TryParse(txt_value.Text?.Trim(), out bool bit))
                    {
                        if (txt_value.Text?.Trim() == "0")
                            bit = false;
                        else if (txt_value.Text?.Trim() == "1")
                            bit = true;
                        else
                        {
                            MessageBox.Show("请输入 True 或 False");
                            return;
                        }
                    }
                    result = client.Write(address, bit);
                }
                else if (rd_byte.Checked)
                {
                    result = client.Write(address, byte.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_short.Checked)
                {
                    result = client.Write(address, short.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_ushort.Checked)
                {
                    result = client.Write(address, ushort.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_int.Checked)
                {
                    result = client.Write(address, int.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_uint.Checked)
                {
                    result = client.Write(address, uint.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_long.Checked)
                {
                    result = client.Write(address, long.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_ulong.Checked)
                {
                    result = client.Write(address, ulong.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_float.Checked)
                {
                    result = client.Write(address, float.Parse(txt_value.Text?.Trim()));
                }
                else if (rd_double.Checked)
                {
                    result = client.Write(address, double.Parse(txt_value.Text?.Trim()));
                }


                if (result.IsSucceed)
                    AppendText($"[写入 {address?.Trim()} 成功]：{txt_value.Text?.Trim()} OK\t\t耗时：{result.TimeConsuming}ms");
                else
                    AppendText($"[写入 {address?.Trim()} 失败]：{result.Err}\t\t耗时：{result.TimeConsuming}ms");
                if (chb_show_package.Checked || (ModifierKeys & Keys.Control) == Keys.Control)
                {
                    AppendText($"[请求报文]{result.Requst}");
                    AppendText($"[响应报文]{result.Response}\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataPersist.Clear();
            AppendText($"数据清空成功\r\n");
        }

        private void but_sendData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_dataPackage.Text))
                {
                    MessageBox.Show("请输入要发送的报文");
                    return;
                }
                var dataPackageString = txt_dataPackage.Text.Replace(" ", "");
                if (dataPackageString.Length % 2 != 0)
                {
                    MessageBox.Show("请输入正确的的报文");
                    return;
                }

                var dataPackage = DataConvert.StringToByteArray(txt_dataPackage.Text?.Trim(), false);
                var msg = client.SendPackage(dataPackage).Value;
                AppendText($"[请求报文]{string.Join(" ", dataPackage.Select(t => t.ToString("X2")))}");
                AppendText($"[响应报文]{string.Join(" ", msg.Select(t => t.ToString("X2")))}\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                client.Close();
                client.Open();
            }
        }

        private void AppendText(string content)
        {
            txt_content.Invoke((Action)(() =>
            {
                txt_content.AppendText($"[{DateTime.Now.ToLongTimeString()}]{content}\r\n");
            }));
        }

        private void AppendEmptyText()
        {
            txt_content.Invoke((Action)(() =>
            {
                txt_content.AppendText($"\r\n");
            }));
        }

        private void SiemensControl_ControlRemoved(object sender, ControlEventArgs e)
        {

        }
    }
}
