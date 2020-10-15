﻿using DoctorApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DoctorServer
{
    public partial class DoctorForm : Form
    {

        public string selectedBike { get; set; }
        public int selectedIndex { get; set; }
        private DoctorClient doctorClient;
        private Dictionary<string, string> usernameAndResistance;
        private Dictionary<string, bool> usernameAndSession;
        private Dictionary<string, List<String>> usernameAndMessages;
       

        public DoctorForm(DoctorClient doctorClient)
        {
            this.usernameAndMessages = new Dictionary<string, List<String>>();
            this.usernameAndSession = new Dictionary<string, bool>();
            this.usernameAndResistance = new Dictionary<string, string>();
            this.doctorClient = doctorClient;
            this.selectedIndex = -1;
            InitializeComponent();
            this.resistanceTextbox.MouseWheel += new MouseEventHandler(changeResistance);

        }

        public void run()
        {
            Application.Run(this);
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }


        public void setSpeed(string speed)
        {

            SpeedValue.Invoke((MethodInvoker)(() =>
            {
                this.SpeedValue.Text = speed;
            }));
        }

        public void setHeartrate(string heartrate)
        {
            HeartrateValue.Invoke((MethodInvoker)(() =>
            {
                this.HeartrateValue.Text = heartrate;
            }));
        }

        public void setDT(string DT)
        {
            DistanceTraveledValue.Invoke((MethodInvoker)(() =>
            {
                this.DistanceTraveledValue.Text = DT;
            }));
        }

        public void setAP(string AP)
        {
            APLabel.Invoke((MethodInvoker)(() =>
            {
                this.APLabel.Text = AP;
            }));

        }

        public void setElapsedTime(string elapsedTime)
        {
            if (!elapsedTime.Equals(""))
            {
                TimeSpan time = TimeSpan.FromSeconds(double.Parse(elapsedTime));
                ElapsedTimeValue.Invoke((MethodInvoker)(() =>
                {
                    this.ElapsedTimeValue.Text = time.ToString(@"hh\:mm\:ss");
                }));
            }
            else
            {
                ElapsedTimeValue.Invoke((MethodInvoker)(() =>
                {
                    this.ElapsedTimeValue.Text = elapsedTime;
                }));
            }
        }

        public void addBike(string username)
        {
            try
            {
                BikeListBox.Invoke((MethodInvoker)(() =>
                {
                    if (!this.usernameAndResistance.ContainsKey(username))
                    {
                        BikeListBox.Items.Add(username);
                        this.usernameAndResistance.Add(username, 0+"");
                        this.usernameAndSession.Add(username, false);
                        this.usernameAndMessages.Add(username, new List<string>());
                    }

                }));
            }catch(Exception e)
            {
                if (!this.usernameAndResistance.ContainsKey(username))
                {
                    BikeListBox.Items.Add(username);
                    this.usernameAndResistance.Add(username, 0 + "");
                    this.usernameAndSession.Add(username, false);
                    this.usernameAndMessages.Add(username, new List<string>());

                }
            }
            
            
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        public void changeResistance(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                int i = Int32.Parse(resistanceTextbox.Text);
                i++;
                if (i > 100)
                {
                    i = 100;
                }
                resistanceTextbox.Text = i + "";

                if (this.selectedIndex != -1)
                {
                    this.doctorClient.sendResistance(i + "", this.selectedBike);
                    this.usernameAndResistance[selectedBike] = i+"";
                }
            }
            else if (e.Delta < 0)
            {
                int i = Int32.Parse(resistanceTextbox.Text);
                i--;
                if (i < 0)
                {
                    i = 0;
                }
                resistanceTextbox.Text = i + "";

                if (this.selectedIndex != -1)
                {
                    this.doctorClient.sendResistance(i+"", this.selectedBike);
                    this.usernameAndResistance[selectedBike] = i+"";

                }


            }

        }

        private void minResistance_Click_1(object sender, EventArgs e)
        {
            int i = Int32.Parse(resistanceTextbox.Text);
            i -= 5;
            if (i < 0)
            {
                i = 0;
            }
            resistanceTextbox.Text = i + "";

            if (this.selectedIndex != -1)
            {
                this.doctorClient.sendResistance(i + "", this.selectedBike);
                this.usernameAndResistance[selectedBike] = i+"";

            }

        }

        private void plusResistance_Click_1(object sender, EventArgs e)
        {
            int i = Int32.Parse(resistanceTextbox.Text);
            i += 5;
            if (i > 100)
            {
                i = 100;
            }
            resistanceTextbox.Text = i + "";

            if (this.selectedIndex != -1)
            {

                this.doctorClient.sendResistance(i + "", this.selectedBike);
                this.usernameAndResistance[selectedBike] = i+"";
            }
        }

        public void setResistance(string resistance, string username)
        {
            this.usernameAndResistance[username] = resistance;
            if (this.selectedBike == username)
            {
                resistanceTextbox.Invoke((MethodInvoker)(() =>
                {
                    this.resistanceTextbox.Text = resistance;
                }));
            }
        }


        private void privSendButton_Click(object sender, EventArgs e)
        {
            if (this.selectedIndex != -1 && PrivateChatBox.Text != "") 
            {
                PrivateChat.Items.Add(PrivateChatBox.Text);
                this.doctorClient.sendPrivMessage(PrivateChatBox.Text,selectedBike);
                this.usernameAndMessages[this.selectedBike].Add(PrivateChatBox.Text);
                PrivateChatBox.Text = "";


            }
        }

        private void globalSendButton_Click(object sender, EventArgs e)
        {
            if (GlobalChatBox.Text != "")
            {
                GlobalChat.Items.Add(GlobalChatBox.Text);
                this.doctorClient.sendGlobalChatMessage(GlobalChatBox.Text);
                GlobalChatBox.Text = "";

            }
        }

        private void BikeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.selectedBike = (string)BikeListBox.SelectedItem;
                doctorClient.selectedUsername = (string)BikeListBox.SelectedItem;

                this.selectedIndex = BikeListBox.SelectedIndex;

                this.resistanceTextbox.Text = usernameAndResistance[selectedBike] + "";

                if (this.usernameAndSession[this.selectedBike] == true)
                {
                    SessionButton.Text = "Stop session";
                }
                else
                {
                    SessionButton.Text = "Start session";
                }

                setChat();

                setAllToEmpty();
            }
            catch(Exception f) {
                Console.WriteLine(f.Message);
            }


        }

        public void setChat()
        {
            PrivateChat.Items.Clear();
            List<string> messages = this.usernameAndMessages[selectedBike];
            foreach(string message in messages)
            {
                PrivateChat.Items.Add(message);
            }
        }

        public void addMessage(string username, string receivedMessage)
        {
            string message = "Patient: " + receivedMessage;
            this.usernameAndMessages[username].Add(message);
            if(this.selectedBike == username)
            {
                PrivateChat.Items.Add(message);
            }
         
        }



        private void setAllToEmpty()
        {
            setAP("");
            setHeartrate("");
            setDT("");
            setElapsedTime("");
            setSpeed("");
           
        }


        private void startSessionButton_Click(object sender, EventArgs e)
        {

            if (this.selectedIndex != -1)
            {
                if(this.usernameAndSession[this.selectedBike] == false)
                {
                    SessionButton.Text = "Stop session";
                    this.usernameAndSession[this.selectedBike] = true;
                }else
                {
                    SessionButton.Text = "Start session";
                    this.usernameAndSession[this.selectedBike] = false;
                }

            }
        }
        private void PrivKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            privSendButton_Click(sender, null);
        }
        private void GlobalKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                globalSendButton_Click(sender, null);
        }

        private void EmergencyStopButton_Click(object sender, EventArgs e)
        {
            if (this.selectedIndex != -1)
            {

                this.doctorClient.sendResistance("0", this.selectedBike);
                this.usernameAndResistance[selectedBike] = "0";
                resistanceTextbox.Text = "0";

            }
        }

        private void ExitButton_Click_1(object sender, EventArgs e)
        {
            this.doctorClient.disconnect();
            Application.Exit();
        }
    }
}
