using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // abc def ghi jkl
                if (serialPortArduino.IsOpen)
                { }
                else
                {
                    serialPortArduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = comboBoxBaudrate.SelectedIndex;
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;
                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();

                    Oefening5.Enabled = true;
                }
            }
            catch (Exception exeption)
            {
                labelStatus.Text = " error " + exeption.Message;
            }

        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDigital2.Checked)
            {
                serialPortArduino.WriteLine("set d2 high");
            }
            else
            {
                serialPortArduino.WriteLine("set d2 low");
            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDigital3.Checked)
            {
                serialPortArduino.WriteLine("set d3 high");
            }
            else
            {
                serialPortArduino.WriteLine("set d3 low");
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDigital4.Checked)
            {
                serialPortArduino.WriteLine("set d4 high");

            }
            else
            {
                serialPortArduino.WriteLine("set d4 low");
            }
        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            serialPortArduino.WriteLine("set pwm9 " + trackBarPWM9.Value);
        }

        private void trackBarPWM10_Scroll(object sender, EventArgs e)
        {
            serialPortArduino.WriteLine("set pwm10 " + trackBarPWM10.Value);
        }

        private void trackBarPWM11_Scroll(object sender, EventArgs e)
        {
            serialPortArduino.WriteLine("set pwm11 " + trackBarPWM11.Value);
        }

        private void Oefening5_Tick(object sender, EventArgs e)
        {
            serialPortArduino.Close();
            serialPortArduino.Open();
            serialPortArduino.WriteLine("get a0");
            string antwoord = serialPortArduino.ReadLine();
            antwoord = antwoord.Remove(0, 4);
            antwoord = antwoord.Remove(antwoord.Length - 1, 1);
            double tekst = Math.Round( (Convert.ToDouble(antwoord) / 25.575) + 5.0,1);
            labelGewensteTemp.Text = tekst.ToString("F1") +"°C";

            serialPortArduino.WriteLine("get a1");
            string antwoord2 = serialPortArduino.ReadLine();
            antwoord2 = antwoord2.Remove(0, 4);
            antwoord2 = antwoord2.Remove(antwoord2.Length - 1, 1);
            double tekst2 = Math.Round((Convert.ToDouble(antwoord2) / 2.046) + 5.0, 1);

            labelHuidigeTemp.Text = tekst2.ToString("F1") + "°C";

            if (tekst2 < tekst)
            {
                serialPortArduino.WriteLine("set d2 high");
            }
            else
            {
                serialPortArduino.WriteLine("set d2 low");
            }
        }
    }
}

