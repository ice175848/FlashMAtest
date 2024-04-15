using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading; //thread要用
using Timer = System.Timers.Timer;

namespace 珠心算閃算出題機
{
    public partial class Form1 : Form
    {
        private delegate void DelShowMessage(string sMessage);
        private void AddMessage(string sMessage)
        {
            if (this.InvokeRequired) // 若非同執行緒
            {
                DelShowMessage del = new DelShowMessage(AddMessage); //利用委派執行
                this.Invoke(del, sMessage);
            }
            else // 同執行緒
            {

                this.label4.Text = sMessage + Environment.NewLine;
                label5.Text = times_check.ToString();
                if (times_check == times || mode == 4 || mode == 5)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
            }
        }
        public void slow(object sender = null, EventArgs e = null)
        {
            timer.Stop();
            AddMessage("");
            //Thread.Sleep(100);
            timer1.Stop();
            timer1.Enabled = false;
            timer.Start();
        }
        public Form1()
        {
            InitializeComponent();

            rnd = new Random();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        int min_value;
        int max_value;
        int mode = 1;
        int times;
        int times_check = 0;
        int cycle=0;
        int hard;
        int ans;
        int random;
        int time_i;
        int again_i = 0;
        Random rnd;
        int[] memory;

        Timer timer = new Timer();
        //Timer pause = new Timer();
        public void button1_Click(object sender, EventArgs ee)
        {
            time_i = 0;
            ans = 0;
            button1.Enabled = false;
            button2.Enabled = false;
            label4.Text = "";
            cycle = int.Parse(numericUpDown1.Text);//週期
            times = int.Parse(numericUpDown2.Text);
            times_check = 0;
            hard = int.Parse(numericUpDown3.Text);
            memory = new int[times];
            max_value = Convert.ToInt32(Math.Pow(10, hard));
            if (radioButton1.Checked == true)
            {
                mode = 1;
            }
            if (radioButton2.Checked == true)
            {
                mode = 2;
            }
            if (radioButton3.Checked == true)
            {
                mode = 3;
            }
            if (radioButton4.Checked == true)
            {
                mode = 4;
            }
            if (radioButton5.Checked == true)
            {
                mode = 5;
            }
            if(mode == 1||mode == 2||mode == 3)
            {
                timer = new Timer();
                timer.Enabled = true;
                timer.Interval = cycle;  // 執行區隔時間
                timer.Start();
                timer.Elapsed += new ElapsedEventHandler(test);
            }
            if(mode ==4||mode == 5)
            {
                test();
            }
        }
        public void test(object source=null,ElapsedEventArgs e=null)
        {
            if (times_check != times)
            {
                times_check++;
                
                switch (mode)
                {
                    case 1://只有加
                        {
                            //pause.Start();
                            timer1.Tick += new EventHandler(slow);
                            timer1.Start();
                            int random = rnd.Next(1, max_value);
                            memory[time_i] = random;
                            time_i++;
                            ans += random;
                            AddMessage(random.ToString());
                            //label4.Text = random.ToString();
                        }
                        break;
                    case 2://加減混合
                        {
                            int random = rnd.Next(0-max_value, max_value);
                            
                            while (ans+random<0)
                            {
                                random = rnd.Next(0 - max_value, max_value);
                            }
                            memory[time_i] = random;
                            time_i++;
                            ans += random;
                            AddMessage(random.ToString());
                        }
                        break;
                    case 3:
                        {
                            int random = rnd.Next(0 - max_value, max_value);
                            memory[time_i] = random;
                            time_i++;
                            ans += random;
                            AddMessage(random.ToString());
                        }
                        break;
                    case 4:
                        {
                            int questionX = rnd.Next(Convert.ToInt32(Math.Pow(10, times-1)), Convert.ToInt32(Math.Pow(10, times)));
                            int questionY = rnd.Next(Convert.ToInt32(Math.Pow(10, hard-1)), Convert.ToInt32(Math.Pow(10, hard)));
                            ans = questionX * questionY;
                            string str = ($"{questionX}×{questionY}=?");
                            AddMessage(str);
                        }
                        break;
                    case 5:
                        {
                            int ansX = rnd.Next(Convert.ToInt32(Math.Pow(10, times - hard)), Convert.ToInt32(Math.Pow(10, times - hard + 1)));
                            int questionY = rnd.Next(Convert.ToInt32(Math.Pow(10, hard - 1)), Convert.ToInt32(Math.Pow(10, hard)));
                            while(ansX * questionY>Math.Pow(10,times))
                            {
                                ansX = rnd.Next(Convert.ToInt32(Math.Pow(10, times - hard)), Convert.ToInt32(Math.Pow(10, times - hard + 1)));
                            }
                            int questionX = ansX * questionY;
                            string str = ($"{questionX}÷{questionY}=?");
                            ans = ansX;
                            AddMessage(str);
                        }
                        break;
                }
            }
            else
            {
                timer.Enabled = false;
                timer.Stop();
                //pause.Enabled = false;
                //pause.Stop();
            }
        }
        public void again(object source = null, ElapsedEventArgs e = null)
        {
            if (again_i < times)
            {
                AddMessage(memory[again_i].ToString());
                again_i++;
            }
            else
            {
                again_i = 0;
                timer.Enabled = false;
                timer.Stop();
                //pause.Stop();
            }
        }
        private void domainUpDown3_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label5.Text = ans.ToString();
        }

        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                numericUpDown1.Enabled = false;
            }
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                numericUpDown1.Enabled = false;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label5.Text = null;
            for (int i = 0; i < times; i++)
                label5.Text += "\n"+memory[i];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cycle = int.Parse(numericUpDown1.Text);//週期

            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = cycle;  // 執行區隔時間
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(again);

        }
    }
}
