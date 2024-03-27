using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Lab3WFDB
{
    public partial class Form1 : Form
    {
        SqlConnection myCon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\relik\Desktop\Lab3WFDB\Lab3WFDB\Database1.mdf;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
            dataGridView2.UserDeletingRow += new DataGridViewRowCancelEventHandler(dataGridView2_UserDeletingRow);
            dataGridView2.CellEndEdit += new DataGridViewCellEventHandler(dataGridView2_CellEndEdit);

        }

        private void dataGridView2_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            int id = (int)dataGridView2.Rows[e.Row.Index].Cells[0].Value;
            int code = (int)dataGridView2.Rows[e.Row.Index].Cells[1].Value;
            String name = (String)dataGridView2.Rows[e.Row.Index].Cells[2].Value;


            myCon.Open();
            SqlCommand cmd = myCon.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Facultati where Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            int rowsAffected = cmd.ExecuteNonQuery();
            myCon.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Record deleted successfully");
            }
            else
            {
                MessageBox.Show("No records deleted");
            }

        }
     

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.facultatiTableAdapter.Fill(this.database1DataSet.Facultati);
            this.universitatiTableAdapter.Fill(this.database1DataSet.Universitati);
            disp_data();
            disp_dataFacultati();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myCon.Open();
            SqlCommand cmd = myCon.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into Universitati values('" +textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "')";
            cmd.ExecuteNonQuery();
            
            myCon.Close();
            disp_data();

            MessageBox.Show("record inserted succesfull");
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            int code;
            if (!int.TryParse(textBox4.Text, out code))
            {
                MessageBox.Show("Invalid input for Code");
                return;
            }

            myCon.Open();
            SqlCommand cmd = myCon.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete from Universitati where Code = @Code";
            cmd.Parameters.AddWithValue("@Code", code);
            int rowsAffected = cmd.ExecuteNonQuery();
            myCon.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Record deleted successfully");
            }
            else
            {
                MessageBox.Show("No records deleted");
            }

            disp_data(); 
        }

        public void disp_data()
        {
            myCon.Open();
            SqlCommand cmd = myCon.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Universitati";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;  

            myCon.Close();
        }

        private void disp_dataFacultati()
        {
            myCon.Open();
            SqlCommand cmd = new SqlCommand("select * from Facultati", myCon);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            myCon.Close();
        }

 
        
        private void button3_Click(object sender, EventArgs e)
        {
            string name = textBox2.Text;
            string newCity = textBox3.Text;

            myCon.Open();
            SqlCommand cmd = myCon.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update Universitati set City = @NewCity where CONVERT(nvarchar(max), NameUniv) = @Name";
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@NewCity", newCity);
            cmd.ExecuteNonQuery();
            myCon.Close();

            disp_data(); 
        }

       

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow editedRow = dataGridView2.Rows[e.RowIndex];

            if (editedRow.Cells[2].Value != null && !string.IsNullOrWhiteSpace(editedRow.Cells[2].Value.ToString()))
            {
                string id = editedRow.Cells[0].Value?.ToString() ?? "Not Set";
                string code = editedRow.Cells[1].Value?.ToString() ?? "Not Set";
                string name = editedRow.Cells[2].Value.ToString();

                myCon.Open();
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Facultati WHERE Id = @id", myCon);
                checkCmd.Parameters.AddWithValue("@id", id);
                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    myCon.Close();
                }
                else
                {
                    SqlCommand insertCmd = myCon.CreateCommand();
                    insertCmd.CommandType = CommandType.Text;
                    insertCmd.CommandText = "insert into Facultati values('" + id + "','" + code + "','" + name + "')";
                    int queryworking = insertCmd.ExecuteNonQuery();

                    myCon.Close();

                    if (queryworking > 0)
                    {
                        MessageBox.Show("Record inserted successfully");
                    }
                    else
                    {
                        MessageBox.Show("No records inserted");
                    }
                }
            }
         
        }

    }
}


