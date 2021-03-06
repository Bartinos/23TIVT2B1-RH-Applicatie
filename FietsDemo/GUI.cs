﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FietsDemo
{

    public class GUI
    {

       private MainForm form;
       private BluetoothBike program;

        public GUI(BluetoothBike program)
        {
            this.program = program;
        }

       public void run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            this.form = new MainForm(this);

            Application.Run(form);
        }

        public MainForm getForm()
        {
            return this.form;
        }

        public void stopSimulator()
        {
            this.program.stopSimulator();
        }

        public void startSimulator()
        {
            this.program.startSimulator();
        }

        public void setResistance(int resistance)
        {
            this.form.setResistance(resistance);
        }

        public void addTextMessage(string message)
        {
            this.form.addMessage(message);
        }

   
    }
}
