using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// directive to include MySQL
using MySql.Data.MySqlClient;
using System.IO;

/*
 * Name: Israel Torres
 * Class: Visual Frameworks - Online (MDV1830-O)
 * Term: C201905 01
 * Code Exercise: Synthesis
 * Number: Final
 */

namespace IsraelTorres_Final
{
    public partial class Form1 : Form
    {
        // connection string variable
        MySqlConnection conn = new MySqlConnection();

        // an instance variable for the DataTable
        DataTable theData = new DataTable();

        // create the MySQL command
        MySqlCommand cmd;

        MySqlDataAdapter adr;

        public Data data
        {
            get
            {
                Data d = new Data();
                d.Title = textBoxTitle.Text;
                d.Genre = textBoxGenre.Text;
                d.Year = numericUpDownYear.Value;
                d.Price = numericUpDownPrice.Value;

                return d;
            }
            set
            {
                textBoxTitle.Text = value.Title;
                textBoxGenre.Text = value.Genre;
                numericUpDownYear.Value = value.Year;
                numericUpDownPrice.Value = value.Price;
            }
        }

        public Form1()
        {
            InitializeComponent();

            string connetionString = BuildConnectionString("exampleDatabase", "dbsAdmin", "password");

            Connect(connetionString);

            // Invoke the method to retrieve the data
            RetrieveData();
        }

        private bool RetrieveData()
        {
            // create the SQL statement
            string sql = "SELECT dvdId, DVD_Title, Price, Year, Genre FROM dvd LIMIT 25";

            // Create the DataAdapter
            adr = new MySqlDataAdapter(sql, conn);

            // set the type for the SELECT command to text
            adr.SelectCommand.CommandType = CommandType.Text;

            // fill the DataTable with the recordset returned by the DataAdapter
            adr.Fill(theData);

            // load the data from database into ListView
            for (int i = 0; i < theData.Select().Length; i++)
            {
                Data d = new Data();

                d.DVDID = Convert.ToInt32(theData.Rows[i]["dvdId"]);
                d.Title = theData.Rows[i]["DVD_Title"].ToString();
                d.Genre = theData.Rows[i]["Genre"].ToString();
                d.Year = Convert.ToDecimal(theData.Rows[i]["Year"].ToString());
                d.Price = Convert.ToDecimal(theData.Rows[i]["Price"].ToString());

                ListViewItem lvi = new ListViewItem();

                lvi.Text = d.ToString();
                lvi.ImageIndex = 0;
                lvi.Tag = d;
                lvi.Name = d.DVDID.ToString();

                listView1.Items.Add(lvi);
            }

            return true;
        }

        private void Connect(string myConnectionString)
        {
            try
            {
                conn.ConnectionString = myConnectionString;
                conn.Open();
                //MessageBox.Show("Connected!");
            }
            catch (MySqlException e)
            {

                // message strig variable
                string msg = "";

                // check what exception was received
                switch (e.Number)
                {
                    case 0:
                        msg = e.ToString();
                        break;
                    case 1042:
                        msg = "Can't resolve host address.\n" + myConnectionString;
                        break;
                    case 1045:
                        msg = "invalid username/password";
                        break;
                    default:
                        // generic message if the others don't cover it
                        msg = e.ToString() + "\n" + myConnectionString;
                        break;
                }
                MessageBox.Show(msg);
            }
        }

        private string BuildConnectionString(string database, string uid, string pwd)
        {
            string serverIP = "";

            try
            {
                // open the text file using stream reader
                using (StreamReader sr = new StreamReader("C:\\VFW\\connect.txt"))
                {
                    // reader the server IP data
                    serverIP = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            // return the connection string
            return "server=" + serverIP + ";uid=" + uid + ";pwd=" + pwd + ";database=" + database + " ;port=8889";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // assign the small icons view to smallIconsToolStripMenuItem_Click
            listView1.View = View.SmallIcon;

            smallIconsToolStripMenuItem.Checked = true;
            largeIconsToolStripMenuItem.Checked = false;
        }

        private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // assign the small icons view to largeIconsToolStripMenuItem_Click
            listView1.View = View.LargeIcon;

            largeIconsToolStripMenuItem.Checked = true;
            smallIconsToolStripMenuItem.Checked = false;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // set the data object with the selected item tag to populate the user input controls
            data = listView1.SelectedItems[0].Tag as Data;
        }

        // create a custom EventArgs class for the ModifyObject method
        public class ModifyObjectEventArgs : EventArgs
        {
            ListViewItem ObjectToModify;

            public ListViewItem ObjectToModify1
            {
                get
                {
                    return ObjectToModify;
                }

                set
                {
                    ObjectToModify = value;
                }
            }

            public ModifyObjectEventArgs(ListViewItem lvi)
            {
                ObjectToModify = lvi;
            }
        }

        // create the ModifyObject to update the ListView
        public void ModifyObject(object sender, ModifyObjectEventArgs e)
        {
            Data d = e.ObjectToModify1.Tag as Data;
            d.Title = textBoxTitle.Text;
            d.Genre = textBoxGenre.Text;
            d.Year = numericUpDownYear.Value;
            d.Price = numericUpDownPrice.Value;

            e.ObjectToModify1.Text = d.ToString();
            e.ObjectToModify1.ImageIndex = 0;
            e.ObjectToModify1.Tag = d;
        }

        // create the UpdateDat method to update the database
        private bool UpdateData()
        {
            Data d = new Data();
            d.Title = textBoxTitle.Text;
            d.Genre = textBoxGenre.Text;
            d.Year = numericUpDownYear.Value;
            d.Price = numericUpDownPrice.Value;
            d.DVDID = Convert.ToInt32(listView1.SelectedItems[0].Name);

            // Create the UpdateCommand.
            cmd = new MySqlCommand("UPDATE dvd SET DVD_Title = '" + d.Title + "', Price = '" + d.Price + "', Year = '" + d.Year + "', Genre = '" + d.Genre + "'WHERE dvdid ='" + d.DVDID + "'LIMIT 1;", conn);

            // Create a data reader
            MySqlDataReader MyReader;

            // execute the query
            MyReader = cmd.ExecuteReader();

            // close the data reader
            MyReader.Close();

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                // invoke the ModifyObject method
                ModifyObject(this, new ModifyObjectEventArgs(listView1.SelectedItems[0]));

                try
                {
                    // invoke the update method
                    UpdateData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
