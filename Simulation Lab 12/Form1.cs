using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation_Lab_12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        double k = 0, t = 0;
        int it = 0, flag = 0;
        double[] count = new double[3];
        double[,] pr = new double[3, 3];
        string[] wet = new string[3];

        private void btStart_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            wet[0] = "Ясно";
            wet[1] = "Облачно";
            wet[2] = "Пасмурно";

            pr[0, 1] = 0.1; pr[0, 2] = 0.3;
            pr[1, 0] = 0.3; pr[1, 2] = 0.1;
            pr[2, 0] = 0.1; pr[2, 1] = 0.4;

            for (int i = 0; i < 3; i++)
            {
                pr[i, i] = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (i != j) pr[i, i] -= pr[i, j];
                }
            }

            for (int i = 0; i < 1000; i++)
            {
                double tr = Math.Log(random.NextDouble()) / pr[it, it];
                t += tr;
                double[] p = new double[3];
                for (int j = 0; j < 3; j++)
                {
                    if (it != j) p[j] = -(pr[it, j] / pr[it, it]);
                    else p[j] = 0;
                }

                double ran = random.NextDouble();
                for (int j = 0; j < 3; j++)
                {
                    ran = ran - p[j];
                    if (ran <= 0)
                    {
                        it = j;
                        break;
                    }
                }
            }

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            int inew = 0;
            double tn;

            double tr = Math.Log(random.NextDouble()) / pr[it, it];
            tn = t + tr;
            k += tr;
            double[] p = new double[3];
            for (int j = 0; j < 3; j++)
            {
                if (it != j) p[j] = -(pr[it, j] / pr[it, it]);
                else p[j] = 0;
            }

            double ran = random.NextDouble();
            for (int j = 0; j < 3; j++)
            {
                ran = ran - p[j];
                if (ran <= 0)
                {
                    inew = j;
                    break;
                }
            }

            count[it] += tn - t;
            it = inew;
            t = tn;

            tbDays.Text = Math.Round(k, 3) + " дней";

            if (flag == 1)
            {
                double sum = 0;
                for (int i = 0; i < 3; i++)
                {
                    sum += count[i];
                }
                for (int i = 0; i < 3; i++)
                {
                    chart1.Series[0].Points.AddXY(wet[i], count[i] / sum);
                }
                timer1.Stop();
            }
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            flag = 1;
        }
    }
}
