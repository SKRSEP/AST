using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AST
{
    public partial class Form1 : Form
    {
        static string path = @"Data Source=SKRIPKO\SQLEXPRESS;Initial Catalog=Employees;Integrated Security=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adpt;
        DataTable dt;

        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(path);
        }

        //Upload DB when loading WinForms
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbSalary.Items.Add("Ascending");
            cmbSalary.Items.Add("Descending");

            display();
        }

        //Add
        private void button2_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtAge.Text == "" || txtSalary.Text == "")
            {
                MessageBox.Show("Please fill in the blanks");
            }

            else
            {
                try
                {
                    con.Open();
                        cmd = new SqlCommand("insert into Employees (Name, Age, Salary) values ('" + txtName.Text + "','" + txtAge.Text + "','" + txtSalary.Text + "') ", con);
                        cmd.ExecuteNonQuery();
                    con.Close();

                    clear();
                    display();

                    MessageBox.Show("Your record has been saved in the Database");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
            public void clear()
            {
                txtName.Text = "";
                txtAge.Text = "";
                txtSalary.Text = "";
            }

            public void display()
            {
                dt = new DataTable();
                con.Open();
                    adpt = new SqlDataAdapter("select * from Employees", con);
                    adpt.Fill(dt);
                    dataGridView1.DataSource = dt;
                con.Close();
            }

        //Update (dataGridView1 to DataBase)
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
                int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);
                cmd = new SqlCommand("update Employees set Name='" + dataGridView1.CurrentRow.Cells["Name"].Value.ToString() + "', Age='" + Convert.ToInt32(dataGridView1.CurrentRow.Cells["Age"].Value) + "' , Salary = '" + Convert.ToInt32(dataGridView1.CurrentRow.Cells["Salary"].Value) + "' where Id ='" + ID + "' ", con);
                cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Database has been updated");
        }

        //Delete
        private void button3_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

            con.Open();
                cmd = new SqlCommand("delete from Employees where Id ='" + ID + "' ", con);
                cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Your record has been deleted from Database");
            display();
        }

        //Filter
        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();
                string query = "select * from Employees where Age like '%" + txtSearch.Text + "%' ";
                adptMethod(query);
            con.Close();

        }

        //Sort
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortType = cmbSalary.SelectedItem.ToString();
            string sortOrder = (sortType == "Ascending") ? "ASC" : "DESC";

            con.Open();
                string query = $"select * from Employees ORDER BY Salary {sortOrder}";
                adptMethod(query);
            con.Close();
        }

        //Метод для работы с SqlDataAdapter - применяем в Filter и Sort
        public void adptMethod(string query)
        {
            adpt = new SqlDataAdapter(query, con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
