using D2RMulti;
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

namespace D2RMng
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();

            save.FileName = "d2rmanager.txt";

            save.Filter = "Text File | *.txt";

            if (save.ShowDialog() == DialogResult.OK)

            {

                StreamWriter writer = new StreamWriter(save.OpenFile());


                foreach (D2RInstance instance in D2RInstances) 
                {
                    writer.WriteLine(instance.GetSaveTxt()); ;
                }
                 

                writer.Dispose();

                writer.Close();

            }



        }
        private List<D2RInstance> D2RInstances = new List<D2RInstance>();
        private void button2_Click(object sender, EventArgs e)
        {
            D2RInstance instance = new D2RInstance();
            //   instance.instancePanel.Parent = pnlMain;
            instance.DeleteMe += DoDeletion;
            D2RInstances.Add(instance);
            RedrawInstances();
        }
        private void DoDeletion(object sender, EventArgs e)
        {
            D2RInstance toremove = (D2RInstance)sender;
            toremove.DeleteMe -= DoDeletion;
            D2RInstances.Remove(toremove);
            toremove.instancePanel.Parent = null;
            RedrawInstances();

        }
        public void RedrawInstances()
        {
            int offset = 0;
            foreach (var instance in D2RInstances)
            {
                instance.instancePanel.Parent = pnlMain;
                instance.instancePanel.Left = 10;
                instance.instancePanel.Top = 10 + offset * 60;
                offset++;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        ClearAllInstances();
                        while (reader.Peek() >= 0)
                        {
                            fileContent = reader.ReadLine();
                            D2RInstance instance = new D2RInstance();
                            instance.Init(fileContent);
                            instance.DeleteMe += DoDeletion;
                            D2RInstances.Add(instance);

                        } 
                        RedrawInstances();  

                    }
                }
            }
        }
        private void ClearAllInstances() {
            foreach (D2RInstance instance in D2RInstances)
            {
                instance.DeleteMe -= DoDeletion;
                instance.instancePanel.Parent = null;
            }
            D2RInstances.Clear();
            RedrawInstances();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ClearAllInstances();
            
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            foreach (D2RInstance instance in D2RInstances)

            {
                instance.cbArea.Text = comboBox1.Text;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HandleHandler.FindAndDeleteHandler();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HandleHandler.KillAllD2R();
            foreach (D2RInstance instance in D2RInstances)
            {
                instance.LaunchD2R(sender, e);
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
