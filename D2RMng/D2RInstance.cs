using D2RMulti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace D2RMng
{
    internal class D2RInstance
    {
        public System.Windows.Forms.Panel instancePanel = new Panel();
      
        public ComboBox cbArea = new ComboBox();
        public event EventHandler DeleteMe;

        private Button btnLaunch;
        private Button btnDelete; 
        private TextBox tbName;
        private TextBox tbPath;
        private TextBox tbUser;
        private TextBox tbPW;
        private CheckBox cbFilter;

        private Label ErrorLabel; 


        public D2RInstance()
        {
            CreateGUIObjects();
          
        }

        public string GetSaveTxt()
        {
            string ischecked;
            if (cbFilter.Checked)
                { ischecked = "True"; }
            else
                { ischecked = "False"; }
            return tbName.Text + "," +
                 tbUser.Text + "," +
                  tbPW.Text + "," +
                   tbPath.Text + "," +
                    cbArea.Text + "," + ischecked;
            
        }
        public void Init(string values)
        {
            string[] listvalues = values.Split(
                  new string[] { "," },
                  StringSplitOptions.None
              );
            tbName.Text = listvalues[0];
            tbUser.Text = listvalues[1];
            tbPW.Text = listvalues[2];
            tbPath.Text = listvalues[3];
            cbArea.Text = listvalues[4];
            if (listvalues.Count() >= 6)
            { cbFilter.Checked = listvalues[5] == "True";  } else { cbFilter.Checked = false; } 
        }
        public void LaunchD2R(object sender, EventArgs e)
        {
            HandleHandler.FindAndDeleteHandler();
            string error;
            if (D2RHandler.startInstance(tbPath.Text, tbUser.Text, tbPW.Text, cbArea.Text, cbFilter.Checked, out error))
            {
              
                ErrorLabel.Text = ""; 
            } else
            {
                ErrorLabel.Text = error;
            }
        }

        private void LaunchD2RToken(object sender, EventArgs e)
        {
            HandleHandler.FindAndDeleteHandler();
            string error;
            string testvartoken = "US-6f75b5e286a9958f989a750ca835f5f3-486434754&flowTrackingId=c96f3456-6d11-43ca-a732-207d9fce337c";
            if (D2RHandler.startInstanceToken(tbPath.Text, testvartoken, out error))
            {

                ErrorLabel.Text = "";
            }
            else
            {
                ErrorLabel.Text = error;
            }
        }

        private void DeleteMyself(object sender, EventArgs e)
        {
            DeleteMe?.Invoke(this, EventArgs.Empty);      
        }

        private void CreateGUIObjects()
        {
             
            ErrorLabel = new Label();   
            instancePanel.BackColor = System.Drawing.Color.DarkGray;
            instancePanel.Height = 55;
            instancePanel.Width = 1100;


            btnLaunch = new Button(); 
            btnLaunch.Parent = instancePanel;
            btnLaunch.Left = 5;
            btnLaunch.Top = 5;
            btnLaunch.Height = 45;
            btnLaunch.Text = "Start";
            btnLaunch.Click += new System.EventHandler(LaunchD2R);


            btnDelete = new Button();
            btnDelete.Parent = instancePanel;
            btnDelete.Left = 750 + btnDelete.Width;
            btnDelete.Top = 5;
            btnDelete.Height = 45;
            btnDelete.Text = "Remove";
            btnDelete.Click += new System.EventHandler(DeleteMyself);
              
            Label Namelabel = new Label();
            Label Userlabel = new Label();
            Label PWlabel = new Label();
            Label Pathlabel = new Label();
            Label Arealabel = new Label();
            Label Filterlabel = new Label();
            CheckBox Filter = new CheckBox();

            Namelabel.Text = "Name:";
            Userlabel.Text = "User/Mail:";
            PWlabel.Text = "Password:";
            Pathlabel.Text = "Path:";
            Arealabel.Text = "Area:";
            Filterlabel.Text = "Filter?";

            Namelabel.Parent = instancePanel;
            ErrorLabel.Parent = instancePanel;
            Userlabel.Parent = instancePanel;
            PWlabel.Parent = instancePanel;
            Pathlabel.Parent = instancePanel;
            Arealabel.Parent = instancePanel;
            Filterlabel.Parent = instancePanel; 

            Namelabel.Location = new System.Drawing.Point(105, 0);
            Userlabel.Location = new System.Drawing.Point(225, 0);
            PWlabel.Location = new System.Drawing.Point(355, 0);
            Pathlabel.Location = new System.Drawing.Point(475, 0);
            Arealabel.Location = new System.Drawing.Point(600, 0);
            Filterlabel.Location = new System.Drawing.Point(750, 0);
            ErrorLabel.Location = new System.Drawing.Point(900, 0);
            Pathlabel.Width = 110;

            cbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbArea.FormattingEnabled = true;
            cbArea.Items.AddRange(new object[] {
            "EU",
            "US",
            "KR"});
            cbArea.Parent = instancePanel;
            cbArea.SelectedIndex = 0;
            cbArea.Location = new System.Drawing.Point(600, 25);


            tbName = new TextBox();
            tbName.Parent = instancePanel;
            tbName.Text   = "Name me";
            tbName.Location = new System.Drawing.Point(105, 25);

            

            tbUser = new TextBox();
            tbUser.Parent = instancePanel;
            tbUser.Text = "User";
            tbUser.Location = new System.Drawing.Point(225, 25); 
            
            tbPW = new TextBox();
            tbPW.Parent = instancePanel;
            tbPW.Text = "Password";
            tbPW.Location = new System.Drawing.Point(355, 25);

            tbPath = new TextBox();
            tbPath.Parent = instancePanel;
            tbPath.Text = @"Path:x:\xx\xx\d2r.exe";
            tbPath.Location = new System.Drawing.Point(475, 25);

            cbFilter = new CheckBox();
            cbFilter.Parent = instancePanel;
            cbFilter.Checked = false;
            cbFilter.Location = new System.Drawing.Point(750, 25);

            btnLaunch = new Button();
            btnLaunch.Parent = instancePanel;
            btnLaunch.Left = 900;
            btnLaunch.Top = 5;
            btnLaunch.Height = 45;
            btnLaunch.Text = "Start";
            btnLaunch.Click += new System.EventHandler(LaunchD2RToken);

        }
    }
}
