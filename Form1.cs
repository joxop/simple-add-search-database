using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Project2
{
    public partial class Form1 : Form
    {
        SqlTransaction mytxn;
        SqlConnection myconn = new SqlConnection();
      
        public Form1()
        {
            InitializeComponent();
            string path = @"C:\Users\Jay\source\repos\PVFC\PVFC.mdf";
            myconn.ConnectionString = " Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True;Connect Timeout=30";
            myconn.Open();
            refreshdata();
            NewMethod("","");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //  Establish a connection
            


            //  Make a SQLCommand object

            NewMethod(textBox1.Text,textBox2.Text);
        }

        private void NewMethod(string Id,string Cust)
        {
            //myconn.Open();
            DataTable mytable = new DataTable();
            SqlCommand mycommand = new SqlCommand();
            //  mycommand.CommandText = " Select * from Customer_T where CustomerState = '" + textBox1.Text + "'";
            mycommand.CommandText = " Select * from Order_T where (OrderID =  @Id OR ''=@id) and CustomerID like  @Cust ";
          //  mycommand.CommandText = " Select * from Order_T with (READUNCOMMITTED) ";
            mycommand.Parameters.Add("@Id", SqlDbType.NChar, 20);
           
            mycommand.Parameters["@Id"].Value = Id;
            mycommand.Parameters.Add("@Cust", SqlDbType.NVarChar, 50);
            mycommand.Parameters["@Cust"].Value = "%" + Cust + "%";
            mycommand.Connection = myconn;

            //   Create an Adapter  (messenger carrying our request)

            SqlDataAdapter myadapter = new SqlDataAdapter();
            myadapter.SelectCommand = mycommand;

            //   Fill an internal table

            myadapter.Fill(mytable);

            //   Bind the table to the GUI object
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = mytable;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewMethod(textBox1.Text, textBox2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex==0)
            {
                MessageBox.Show("Select Customer From List");
                return;
            }
            else if (txtdate.Text=="")
            {
                MessageBox.Show("Select Enter Valid Date");
                return;
            }
            else if (txtorder.Text == "")
            {
                MessageBox.Show("Select Enter Order ID");
                return;
            }
            
            
            DataTable dataTable = new DataTable();
            SqlCommand mycommand = new SqlCommand();
            //  mycommand.CommandText = " Select * from Customer_T where CustomerState = '" + textBox1.Text + "'";
            mycommand.CommandText = " Select * from Order_T where (OrderID =  @Id OR ''=@id) and CustomerID = @Cust ";
            //  mycommand.CommandText = " Select * from Order_T with (READUNCOMMITTED) ";
            mycommand.Parameters.Add("@Id", SqlDbType.NChar, 20);

            mycommand.Parameters["@Id"].Value = txtorder.Text;
            mycommand.Parameters.Add("@Cust", SqlDbType.NVarChar, 50);
            mycommand.Parameters["@Cust"].Value =  comboBox1.SelectedValue ;
            mycommand.Connection = myconn;

            //   Create an Adapter  (messenger carrying our request)

            SqlDataAdapter myadapter = new SqlDataAdapter();
            myadapter.SelectCommand = mycommand;
            myadapter.Fill(dataTable);
            if(dataTable.Rows.Count>0)
            {
                MessageBox.Show("duplicate");
            }
            else
            {
                mycommand = new SqlCommand(" insert into  Order_T(OrderID,OrderDate,CustomerID)Values(@Id,@MyDate,@Cust)", myconn);
               
                //  mycommand.CommandText = " Select * from Order_T with (READUNCOMMITTED) ";
                mycommand.Parameters.Add("@Id", SqlDbType.NChar, 20);

                mycommand.Parameters["@Id"].Value = txtorder.Text;
                mycommand.Parameters.Add("@MyDate", SqlDbType.Date, 20);
                mycommand.Parameters["@MyDate"].Value = txtdate.Text;
                mycommand.Parameters.Add("@Cust", SqlDbType.NVarChar, 50);
                mycommand.Parameters["@Cust"].Value =  comboBox1.SelectedValue;
                
                mycommand.ExecuteNonQuery();
                NewMethod(textBox1.Text,textBox2.Text);
                MessageBox.Show("Inserted Successfully");
            }
           
        }
        public void refreshdata()
        {
            DataRow dr;


            SqlCommand cmd = new SqlCommand("select * from [Customer_T]", myconn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            dr = dt.NewRow();
            dr.ItemArray = new object[] { 0, "--Select Customer--" };
            dt.Rows.InsertAt(dr, 0);

            comboBox1.ValueMember = "CustomerID";

            comboBox1.DisplayMember = "CustomerName";
            comboBox1.DataSource = dt;

           // myconn.Close();
        }
    }
}
