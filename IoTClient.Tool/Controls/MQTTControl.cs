using AntdUI;
using IoTClient.Tool.Common;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using Talk.Extensions;
using Talk.Linq.Extensions;

namespace IoTClient.Tool.Controls
{
    public partial class MQTTControl : UserControl
    {
        public MQTTControl()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Size = new Size(880, 450);
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            but_Subscribe.Enabled = false;
            but_Publish.Enabled = false;
            but_Stop.Enabled = false;
            txt_ClientID.Text = Guid.NewGuid().ToString();
            checkBox1_CheckedChanged(null, null);

            var config = ConnectionConfig.GetConfig();
            if (!string.IsNullOrWhiteSpace(config.Mqtt_Address)) txt_Address.Text = config.Mqtt_Address;
            if (!string.IsNullOrWhiteSpace(config.Mqtt_Port)) txt_Port.Text = config.Mqtt_Port;
            if (!string.IsNullOrWhiteSpace(config.Mqtt_ClientID)) txt_ClientID.Text = config.Mqtt_ClientID;
            if (!string.IsNullOrWhiteSpace(config.Mqtt_UserName)) txt_UserName.Text = config.Mqtt_UserName;
            if (!string.IsNullOrWhiteSpace(config.Mqtt_Password)) txt_Password.Text = config.Mqtt_Password;
            if (!string.IsNullOrWhiteSpace(config.Mqtt_ca_file)) txt_ca_file.Text = config.Mqtt_ca_file;
            if (!string.IsNullOrWhiteSpace(config.Mqtt_pfx_file)) txt_pfx_file.Text = config.Mqtt_pfx_file;
            if (config.Mqtt_Topics.IsAny())
            {
                foreach (var topic in config.Mqtt_Topics)
                {
                    var tag = new Tag();
                    tag.Text = topic;
                    tag.AutoSize = true;
                    tag.MinimumSize = new Size(18, 18);
                    tag.CloseIcon = true;
                    tag.CloseChanged += Tag_CloseChangedAsync;
                    flowLayoutPanel1.Controls.Add(tag);
                };
            }
        }

        private IManagedMqttClient mqttClient;
        private async void but_start_ClickAsync(object sender, EventArgs even)
        {
            try
            {
                but_Stop_Click(null, null);
                var factory = new MqttFactory();
                mqttClient = factory.CreateManagedMqttClient();
                var mqttClientOptions = new MqttClientOptionsBuilder()
                                 .WithClientId(txt_ClientID.Text?.Trim())
                                 //.WithTcpServer(txt_Address.Text?.Trim(), int.Parse(txt_Port.Text?.Trim()))
                                 .WithCredentials(txt_UserName.Text, txt_Password.Text);

                if (checkBox1.Checked)
                {
                    if (!File.Exists(txt_ca_file.Text))
                    {
                        MessageBox.Show($"没有找到文件:{txt_ca_file.Text}");
                        return;
                    }
                    if (!File.Exists(txt_pfx_file.Text))
                    {
                        MessageBox.Show($"没有找到文件:{txt_pfx_file.Text}");
                        return;
                    }
                    var caCert = X509Certificate.CreateFromCertFile(txt_ca_file.Text);
                    var clientCert = new X509Certificate2(txt_pfx_file.Text);
                    mqttClientOptions = mqttClientOptions.WithTls(new MqttClientOptionsBuilderTlsParameters()
                    {
                        UseTls = true,
                        SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
                        CertificateValidationHandler = (o) =>
                        {
                            return true;
                        },
                        Certificates = new List<X509Certificate>(){
                                    caCert, clientCert
                                 }
                    });
                }

                if (comboBox1.SelectedIndex == 0)
                {
                    mqttClientOptions = mqttClientOptions.WithTcpServer(txt_Address.Text?.Trim(), int.Parse(txt_Port.Text?.Trim()));
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    mqttClientOptions = mqttClientOptions.WithWebSocketServer($"{txt_Address.Text?.Trim()}:{txt_Port.Text?.Trim()}/mqtt").WithTls();
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    mqttClientOptions = mqttClientOptions.WithWebSocketServer($"{txt_Address.Text?.Trim()}:{txt_Port.Text?.Trim()}/mqtt");
                }

                var options = new ManagedMqttClientOptionsBuilder()
                            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                            .WithClientOptions(mqttClientOptions.Build())
                            .Build();

                await mqttClient.StartAsync(options);

                mqttClient.UseDisconnectedHandler(e =>
                {
                    WriteLine_1("### 服务器断开连接 ###");
                });


                mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    WriteLine_1($"### 收到消息 {DateTime.Now.yyyMMddHHmmss()} ###");
                    WriteLine_1($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                    WriteLine_1($"+ Retain = {e.ApplicationMessage.Retain}");
                    WriteLine_1($"+ Topic = {e.ApplicationMessage.Topic}");
                    try
                    {
                        WriteLine_1($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    }
                    catch { }
                    WriteLine_1();
                });

                mqttClient.UseConnectedHandler(e =>
                {
                    WriteLine_1("### 连接到服务 ###");

                    but_Start.Enabled = false;
                    but_Subscribe.Enabled = true;
                    but_Publish.Enabled = true;
                    but_Stop.Enabled = true;
                });

                var config = ConnectionConfig.GetConfig();
                config.Mqtt_Address = txt_Address.Text;
                config.Mqtt_Port = txt_Port.Text;
                config.Mqtt_ClientID = txt_ClientID.Text;
                config.Mqtt_UserName = txt_UserName.Text;
                config.Mqtt_Password = txt_Password.Text;
                config.Mqtt_ca_file = txt_ca_file.Text;
                config.Mqtt_pfx_file = txt_pfx_file.Text;
                config.SaveConfig();
                if (config.Mqtt_Topics.IsAny())
                {
                    foreach (var topic in config.Mqtt_Topics)
                    {
                        await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());
                    };
                }
            }
            catch (Exception ex)
            {
                WriteLine_1($"err：{ex.Message}");
            }
        }

        private async void but_Stop_Click(object sender, EventArgs e)
        {
            if (mqttClient != null)
            {
                if (mqttClient.IsStarted)
                    await mqttClient.StopAsync();
                mqttClient.Dispose();
            }
            but_Subscribe.Enabled = false;
            but_Publish.Enabled = false;
            but_Stop.Enabled = false;
            but_Start.Enabled = true;
        }

        private void WriteLine_1(string msg = "")
        {
            //txt_msg.AppendText($"{msg} \r\n");
            //设置存储最大存储
            int maxLength = 200000;
            if (txt_msg.Text.Length >= maxLength)
            {
                txt_msg.Text = txt_msg.Text.Substring(txt_msg.Text.Length - maxLength / 2);
            }
            txt_msg.AppendText($"{msg.Replace("},", "},\r\n")} \r\n");
        }

        private void WriteLine_2(string msg = "")
        {
            textBox2.AppendText($"{msg}\r\n");
        }

        private async void but_Subscribe_ClickAsync(object sender, EventArgs e)
        {
            // Subscribe to a topic
            var topic = txt_subscribe_topic.Text?.Trim();
            if (string.IsNullOrWhiteSpace(topic))
            {
                WriteLine_1("### 请输入Topic ###");
                return;
            }
            if (mqttClient == null || !mqttClient.IsStarted || !mqttClient.IsConnected)
            {
                WriteLine_1("### 请先启动MQTT ###");
                return;
            }
            var config = ConnectionConfig.GetConfig();
            if (config.Mqtt_Topics.Any(t => t == topic))
            {
                WriteLine_1($"### 已订阅过Topic[ {topic} ] ###");
                return;
            }

            await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());

            WriteLine_1($"### 订阅Topic[ {topic} ] ###");

            var tag = new Tag();
            tag.Text = topic;
            tag.AutoSize = true;
            tag.MinimumSize = new Size(18, 18);
            tag.CloseIcon = true;
            tag.CloseChanged += Tag_CloseChangedAsync;
            flowLayoutPanel1.Controls.Add(tag);
            config.Mqtt_Topics.Add(topic);
            config.SaveConfig();
        }

        private bool Tag_CloseChangedAsync(object sender, EventArgs e)
        {
            var obj = sender as Tag;
            var topic = obj.Text;
            if (!topic.IsNullOrWhiteSpace())
                mqttClient.UnsubscribeAsync(topic).Wait();
            var config = ConnectionConfig.GetConfig();
            config.Mqtt_Topics.Remove(topic);
            config.SaveConfig();
            WriteLine_1($"### 取消订阅Topic[ {topic} ] ###");
            return true;
        }

        private async void but_Publish_Click(object sender, EventArgs e)
        {
            var topic = txt_publish_topic.Text?.Trim();
            var payload = txt_publish_payload.Text?.Trim();
            if (string.IsNullOrWhiteSpace(topic))
            {
                WriteLine_1("### 请输入Topic ###");
                return;
            }
            var result = await mqttClient.PublishAsync(topic, payload);
            WriteLine_2($"topic:{topic} payload:{payload} {result.ReasonCode}");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txt_Port.Text = checkBox1.Checked ? "8883" : "1883";
            txt_pfx_file.Enabled = checkBox1.Checked;
            txt_ca_file.Enabled = checkBox1.Checked;
        }

        private void txt_ca_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*crt*)|*.crt*"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txt_ca_file.Text = fileDialog.FileName;//返回文件的完整路径   
                txt_ca_file.Select(txt_ca_file.Text.Length, 1);
            }
        }

        private void txt_pfx_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*pfx*)|*.pfx*"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txt_pfx_file.Text = fileDialog.FileName;//返回文件的完整路径           
                txt_pfx_file.Select(txt_pfx_file.Text.Length, 1);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBox1.Enabled = comboBox1.SelectedIndex == 0;
            if (!checkBox1.Enabled) checkBox1.Checked = false;
        }

        private void txt_subscribe_topic_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                but_Subscribe_ClickAsync(sender, e);
            }
        }
    }
}
