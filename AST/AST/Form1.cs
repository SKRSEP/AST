using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AST
{
    public partial class Form1 : Form
    {
        //string path = @"Data Source=SKRIPKO\SQLEXPRESS;Initial Catalog=Employees;Integrated Security=True";
        string path = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Employees.mdf;Integrated Security=True";

        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adpt;
        DataTable dt;
        SqlDataReader dr;

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

            con.Open();
            adptMethod("select * from Employees");
            con.Close();
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

                    cmd = new SqlCommand("insert into Employees (Name, Age, Salary) values (@Name, @Age, @Salary)", con);

                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text);

                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("select * from Employees", con);

                    dr = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dr);
                    dataGridView1.DataSource = dt;

                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Update (dataGridView1 to DataBase)
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

            cmd = new SqlCommand("update Employees set Name = @Name, Age = @Age, Salary = @Salary where Id = @ID", con);

            cmd.Parameters.AddWithValue("@Name", dataGridView1.CurrentRow.Cells["Name"].Value.ToString());
            cmd.Parameters.AddWithValue("@Age", Convert.ToInt32(dataGridView1.CurrentRow.Cells["Age"].Value));
            cmd.Parameters.AddWithValue("@Salary", Convert.ToInt32(dataGridView1.CurrentRow.Cells["Salary"].Value));
            cmd.Parameters.AddWithValue("@ID", ID);

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
            adptMethod("select * from Employees");
            con.Close();

            //MessageBox.Show("Your record has been deleted from Database");
        }

        //Filter
        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();
            adptMethod("select * from Employees where Age like '%" + txtSearch.Text + "%' ");
            con.Close();
        }

        //Sort
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sortType = cmbSalary.SelectedItem.ToString();
            string sortOrder = (sortType == "Ascending") ? "ASC" : "DESC";

            con.Open();
            adptMethod($"select * from Employees ORDER BY Salary {sortOrder}");
            con.Close();
        }

        //Метод для заполнения dataGridView
        public void adptMethod(string query)
        {
            adpt = new SqlDataAdapter(query, con);
            dt = new DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
