using IoTClient.Clients.Modbus;
using IoTClient.Common.Helpers;
using IoTClient.Enums;
using IoTClient.Tool.Common;
using IoTServer.Common;
using IoTServer.Servers.Modbus;
using System;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IoTClient.Tool.Controls
{
    public partial class ModbusAsciiControl : UserControl
    {
        private ModbusAsciiServer server;
        private ModbusAsciiClient client;
        public ModbusAsciiControl()
        {
            InitializeComponent();

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

            che_plcadd.Location = new Point(616, 23);
            button6.Location = new Point(768, 17);

            txt_dataPackage.Location = new Point(490, 18);
            txt_dataPackage.Size = new Size(186, 21);
            but_sendData.Location = new Point(680, 17);

            chb_show_package.Location = new Point(776, 20);
            cmb_EndianFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_EndianFormat.SelectedIndex = 0;

            but_read.Enabled = false;
            but_brokenline.Enabled = false;
            but_write.Enabled = false;
            but_server_close.Enabled = false;
            but_close.Enabled = false;
            but_sendData.Enabled = false;
            UpdatePortNames();
            cb_portNameSend.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_portNameSend_server.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_parity.SelectedIndex = 0;
            cb_parity.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_baudRate.SelectedIndex = 2;

            var config = ConnectionConfig.GetConfig();
            if (!string.IsNullOrWhiteSpace(config.ModBusAscii_Address)) txt_address.Text = config.ModBusAscii_Address;
            if (!string.IsNullOrWhiteSpace(config.ModBusAscii_Value)) txt_value.Text = config.ModBusAscii_Value;
            if (!string.IsNullOrWhiteSpace(config.ModBusAscii_StationNumber)) txt_stationNumber.Text = config.ModBusAscii_StationNumber;
            if (!string.IsNullOrWhiteSpace(config.ModBusAscii_PortName)) cb_portNameSend.SelectedItem = config.ModBusAscii_PortName;
            if (!string.IsNullOrWhiteSpace(config.ModBusAscii_BaudRate)) cb_baudRate.SelectedItem = config.ModBusAscii_BaudRate;
            if (!string.IsNullOrWhiteSpace(config.ModBusAscii_DataBits)) txt_dataBit.Text = config.ModBusAscii_DataBits;
            txt_stopBit.Text = ((int)config.ModBusAscii_StopBits).ToString();
            cb_parity.SelectedIndex = (int)config.ModBusAscii_Parity;
            cmb_EndianFormat.SelectedItem = config.ModBusAscii_EndianFormat.ToString();
            switch (config.ModBusAscii_Datatype)
            {
                case "rd_coil": rd_coil.Checked = true; break;
                case "rd_discrete": rd_discrete.Checked = true; break;
                case "rd_short": rd_short.Checked = true; break;
                case "rd_ushort": rd_ushort.Checked = true; break;
                case "rd_int": rd_int.Checked = true; break;
                case "rd_uint": rd_uint.Checked = true; break;
                case "rd_long": rd_long.Checked = true; break;
                case "rd_ulong": rd_ulong.Checked = true; break;
                case "rd_float": rd_float.Checked = true; break;
                case "rd_double": rd_double.Checked = true; break;
            };
            chb_show_package.Checked = config.ModBusAscii_ShowPackage;
        }

        /// <summary>
        /// 更新串口名
        /// </summary>
        public void UpdatePortNames()
        {
            cb_portNameSend.DataSource = ModbusRtuClient.GetPortNames();
            cb_portNameSend_server.DataSource = ModbusRtuClient.GetPortNames();
        }

        /// <summary>
        /// 开启仿真服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_server_open_Click(object sender, EventArgs e)
        {
            try
            {
                var PortName = cb_portNameSend_server.Text.ToString();
                var BaudRate = int.Parse(cb_baudRate.Text.ToString());
                var DataBits = int.Parse(txt_dataBit.Text.ToString());
                var StopBits = (StopBits)int.Parse(txt_stopBit.Text.ToString());
                var parity = cb_parity.SelectedIndex == 0 ? Parity.None : (cb_parity.SelectedIndex == 1 ? Parity.Odd : Parity.Even);
                server?.Stop();
                server = new ModbusAsciiServer(PortName, BaudRate, DataBits, StopBits, parity);
                server.Start();
                AppendText("开启仿真服务");
                but_server_open.Enabled = false;
                but_server_close.Enabled = true;
                cb_portNameSend_server.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AppendText(string content)
        {
            txt_content.Invoke((Action)(() =>
            {
                txt_content.AppendText($"[{DateTime.Now.ToLongTimeString()}]{content}\r\n");
            }));
        }

        private void but_open_Click(object sender, EventArgs e)
        {
            try
            {
                var PortName = cb_portNameSend.Text.ToString();
                var BaudRate = int.Parse(cb_baudRate.Text.ToString());
                var DataBits = int.Parse(txt_dataBit.Text.ToString());
                var StopBits = (StopBits)int.Parse(txt_stopBit.Text.ToString());
                var parity = cb_parity.SelectedIndex == 0 ? Parity.None : (cb_parity.SelectedIndex == 1 ? Parity.Odd : Parity.Even);
                client?.Close();

                EndianFormat format = EndianFormat.ABCD;
                switch (cmb_EndianFormat.SelectedIndex)
                {
                    case 0:
                        format = EndianFormat.ABCD;
                        break;
                    case 1:
                        format = EndianFormat.BADC;
                        break;
                    case 2:
                        format = EndianFormat.CDAB;
                        break;
                    case 3:
                        format = EndianFormat.DCBA;
                        break;
                }
                var plcadd = che_plcadd.Checked;
                client = new ModbusAsciiClient(PortName, BaudRate, DataBits, StopBits, parity, format: format,plcAddresses: plcadd);
                var result = client.Open();
                if (result.IsSucceed)
                {
                    but_open.Enabled = false;
                    cb_portNameSend.Enabled = false;
                    but_read.Enabled = true;
                    but_brokenline.Enabled = true;
                    but_write.Enabled = true;
                    but_open.Enabled = false;
                    but_close.Enabled = true;
                    but_sendData.Enabled = true;
                    AppendText($"连接成功\t\t\t\t耗时：{result.TimeConsuming}ms");
                    ControlEnabledFalse();
                }
                else
                    AppendText($"连接失败：{result.Err}");

                var config = ConnectionConfig.GetConfig();
                config.ModBusAscii_PortName = PortName;
                config.ModBusAscii_BaudRate = BaudRate.ToString();
                config.ModBusAscii_DataBits = DataBits.ToString();
                config.ModBusAscii_StopBits = StopBits;
                config.ModBusAscii_Parity = parity;
                config.ModBusAscii_Value = txt_value.Text;
                config.ModBusAscii_Address = txt_address.Text;
                config.ModBusAscii_ShowPackage = chb_show_package.Checked;
                config.ModBusAscii_EndianFormat = format;
                config.SaveConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ControlEnabledFalse()
        {
            cmb_EndianFormat.Enabled = false;
            cb_portNameSend.Enabled = false;
            cb_baudRate.Enabled = false;
            txt_dataBit.Enabled = false;
            txt_stopBit.Enabled = false;
            cb_parity.Enabled = false;
            txt_stationNumber.Enabled = false;
        }

        private void ControlEnabledTrue()
        {
            cmb_EndianFormat.Enabled = true;
            cb_portNameSend.Enabled = true;
            cb_baudRate.Enabled = true;
            txt_dataBit.Enabled = true;
            txt_stopBit.Enabled = true;
            cb_parity.Enabled = true;
            txt_stationNumber.Enabled = true;
        }

        private void but_server_close_Click(object sender, EventArgs e)
        {
            server?.Stop();
            AppendText("关闭仿真服务");
            but_server_open.Enabled = true;
            but_server_close.Enabled = false;
            cb_portNameSend_server.Enabled = true;
        }

        private void but_read_Click(object sender, EventArgs e)
        {
            byte.TryParse(txt_stationNumber.Text?.Trim(), out byte stationNumber);
            if (string.IsNullOrWhiteSpace(txt_address.Text))
            {
                MessageBox.Show("请输入地址");
                return;
            }
            try
            {
                if (txt_address.Text.Contains("-"))
                {
                    AppendText($"ModbusAsciiClient 暂不支持批量读取");
                    return;
                }
                dynamic result = null;
                if (rd_coil.Checked)
                {
                    result = client.ReadCoil(txt_address.Text, stationNumber);
                }
                else if (rd_short.Checked)
                {
                    result = client.ReadInt16(txt_address.Text, stationNumber);
                }
                else if (rd_ushort.Checked)
                {
                    result = client.ReadUInt16(txt_address.Text, stationNumber);
                }
                else if (rd_int.Checked)
                {
                    result = client.ReadInt32(txt_address.Text, stationNumber);
                }
                else if (rd_uint.Checked)
                {
                    result = client.ReadUInt32(txt_address.Text, stationNumber);
                }
                else if (rd_long.Checked)
                {
                    result = client.ReadInt64(txt_address.Text, stationNumber);
                }
                else if (rd_ulong.Checked)
                {
                    result = client.ReadUInt64(txt_address.Text, stationNumber);
                }
                else if (rd_float.Checked)
                {
                    result = client.ReadFloat(txt_address.Text, stationNumber);
                }
                else if (rd_double.Checked)
                {
                    result = client.ReadDouble(txt_address.Text, stationNumber);
                }
                else if (rd_discrete.Checked)
                {
                    result = client.ReadDiscrete(txt_address.Text, stationNumber);
                }

                if (result.IsSucceed)
                    AppendText($"[读取 {txt_address.Text?.Trim()} 成功]：{result.Value}\t\t耗时：{result.TimeConsuming}ms");
                else
                    AppendText($"[读取 {txt_address.Text?.Trim()} 失败]：{result.Err}\t\t耗时：{result.TimeConsuming}ms");
                if (chb_show_package.Checked || (ModifierKeys & Keys.Control) == Keys.Control)
                {
                    AppendText($"[请求报文]{result.Requst}");
                    AppendText($"[响应报文]{result.Response}\r\n");
                }

                var config = ConnectionConfig.GetConfig();
                config.ModBusAscii_Value = txt_value.Text;
                config.ModBusAscii_Address = txt_address.Text;
                config.ModBusAscii_StationNumber = txt_stationNumber.Text;
                config.ModBusAscii_ShowPackage = chb_show_package.Checked;
                config.ModBusAscii_Datatype = string.Empty;
                if (rd_coil.Checked) config.ModBusAscii_Datatype = "rd_coil";
                else if (rd_discrete.Checked) config.ModBusAscii_Datatype = "rd_discrete";
                else if (rd_short.Checked) config.ModBusAscii_Datatype = "rd_short";
                else if (rd_ushort.Checked) config.ModBusAscii_Datatype = "rd_ushort";
                else if (rd_int.Checked) config.ModBusAscii_Datatype = "rd_int";
                else if (rd_uint.Checked) config.ModBusAscii_Datatype = "rd_uint";
                else if (rd_long.Checked) config.ModBusAscii_Datatype = "rd_long";
                else if (rd_ulong.Checked) config.ModBusAscii_Datatype = "rd_ulong";
                else if (rd_float.Checked) config.ModBusAscii_Datatype = "rd_float";
                else if (rd_double.Checked) config.ModBusAscii_Datatype = "rd_double";
                config.SaveConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void but_write_Click(object sender, EventArgs e)
        {
            var address = txt_address.Text?.Trim();
            byte.TryParse(txt_stationNumber.Text?.Trim(), out byte stationNumber);
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

                dynamic result = null;
                if (rd_coil.Checked)
                {
                    if (!bool.TryParse(txt_value.Text?.Trim(), out bool coil))
                    {
                        if (txt_value.Text?.Trim() == "0")
                            coil = false;
                        else if (txt_value.Text?.Trim() == "1")
                            coil = true;
                        else
                        {
                            MessageBox.Show("请输入 True 或 False");
                            return;
                        }
                    }
                    result = client.Write(address, coil, stationNumber);
                }
                else if (rd_short.Checked)
                {
                    result = client.Write(address, short.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_ushort.Checked)
                {
                    result = client.Write(address, ushort.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_int.Checked)
                {
                    result = client.Write(address, int.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_uint.Checked)
                {
                    result = client.Write(address, uint.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_long.Checked)
                {
                    result = client.Write(address, long.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_ulong.Checked)
                {
                    result = client.Write(address, ulong.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_float.Checked)
                {
                    result = client.Write(address, float.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_double.Checked)
                {
                    result = client.Write(address, double.Parse(txt_value.Text?.Trim()), stationNumber);
                }
                else if (rd_discrete.Checked)
                {
                    AppendText($"离散类型只读");
                    return;
                }

                if (result.IsSucceed)
                    AppendText($"[写入 {address?.Trim()} 成功]：{txt_value.Text?.Trim()} OK\t\t耗时：{result.TimeConsuming}ms");
                else
                    AppendText($"[写入 {address?.Trim()} 失败]：{result.Err}\t\t耗时：{result.TimeConsuming}ms\r\n");
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
                var msg = client.SendPackageReliable(dataPackage).Value;
                AppendText($"[请求报文]{string.Join(" ", dataPackage.Select(t => t.ToString("X2")))}\r");
                AppendText($"[响应报文]{string.Join(" ", msg.Select(t => t.ToString("X2")))}\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                client.Close();
                client.Open();
            }
        }

        private void but_close_Click(object sender, EventArgs e)
        {
            client?.Close();
            AppendText("关闭连接");
            but_open.Enabled = true;
            but_close.Enabled = false;
            cb_portNameSend.Enabled = true;
            but_sendData.Enabled = false;
            ControlEnabledTrue();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataPersist.Clear();
            AppendText($"数据清空成功\r\n");
        }

        private async void but_brokenline_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                byte.TryParse(txt_stationNumber.Text?.Trim(), out byte stationNumber);

                var constant = new BrokenLineChart(txt_address.Text);
                constant.Show();
                while (!constant.IsDisposed)
                {
                    dynamic result = null;
                    if (rd_coil.Checked)
                        result = client.ReadCoil(txt_address.Text, stationNumber);
                    else if (rd_short.Checked)
                        result = client.ReadInt16(txt_address.Text, stationNumber);
                    else if (rd_ushort.Checked)
                        result = client.ReadUInt16(txt_address.Text, stationNumber);
                    else if (rd_int.Checked)
                        result = client.ReadInt32(txt_address.Text, stationNumber);
                    else if (rd_uint.Checked)
                        result = client.ReadUInt32(txt_address.Text, stationNumber);
                    else if (rd_long.Checked)
                        result = client.ReadInt64(txt_address.Text, stationNumber);
                    else if (rd_ulong.Checked)
                        result = client.ReadUInt64(txt_address.Text, stationNumber);
                    else if (rd_float.Checked)
                        result = client.ReadFloat(txt_address.Text, stationNumber);
                    else if (rd_double.Checked)
                        result = client.ReadDouble(txt_address.Text, stationNumber);
                    else if (rd_discrete.Checked)
                        result = client.ReadDiscrete(txt_address.Text, stationNumber);

                    if (result.IsSucceed)
                        constant.AddData(result.Value);
                    await Task.Delay(800);
                }
            }
            catch (Exception ex)
            {
                AppendText($"[折线图更新失败]：{ex.Message}");
            }
        }

        //private void AppendText(string content)
        //{
        //    txt_content.Invoke((Action)(() =>
        //    {
        //        txt_content.AppendText($"[{DateTime.Now.ToLongTimeString()}]{content}\r\n");
        //    }));
        //}
    }
}
