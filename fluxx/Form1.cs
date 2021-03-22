using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace fluxx
{
    public partial class Form1 : Form
    {
        string strConn = ConfigurationManager.ConnectionStrings["fluxx.Properties.Settings.BaoShan1880CoilYardConnectionString"].ToString();
        DataTable dt = null;
        DataTable dthistory = null;
        List<string> Status = new List<string>() { "Canceled", "Successful", "Rejected" };


        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<List<string>> result = new List<List<string>>();
            List<string> temp = new List<string>();
            dataGridView.Rows.Clear();
            //if(dt_xx != null)
            //{
            //    dt_xx.Rows.Clear();
            //    dataGridView.DataSource = dt_xx;
            //}
            
            //设置数据表格为只读
            //dataGridView.ReadOnly = true;
            //不允许添加行
            dataGridView.AllowUserToAddRows = false;
            using (SqlConnection conn = new SqlConnection(strConn)) //定义连接对象并实例化（实例化时需要提供连接字符串作为参数）
            {
                if (conn.State == ConnectionState.Closed) //判断数据库连接是否已经打开，如果没打开则打开数据库连接
                {
                    conn.Open(); //打开数据库连接
                }

                string historysql = "SELECT ProductBId,ListBId,ExecutionStatus FROM ProductOperationHistory";

                string sql = "SELECT OPERATION.ProductBId, OPERATIONLIST.BId FROM ProductOperation AS OPERATION INNER JOIN ProductOperationList AS OPERATIONLIST ON OPERATION.ListId = OPERATIONLIST.Id";
                if (TextBoxProduct.TextLength != 0)
                {
                    sql += " AND OPERATION.ProductBId LIKE '%" + TextBoxProduct.Text + "%'";
                    historysql += " WHERE ProductBId LIKE '%" + TextBoxProduct.Text + "%'";
                }

                using (SqlCommand cmd = new SqlCommand(historysql, conn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dthistory = new DataTable();
                        dthistory.Load(dr);
                    }
                }

                using (SqlCommand cmd1 = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader dr = cmd1.ExecuteReader())
                    {
                        dt = new DataTable();
                        dt.Load(dr);
                    }
                }
            }
            if (dt != null && dt.Rows[0][0] != null)
            {
                temp.Add(dt.Rows[0][0].ToString());
                for (int i = 0; i < dt.Rows.Count; i++) //行
                {

                    if (i > 0 && dt.Rows[i][0].ToString() != dt.Rows[i - 1][0].ToString())
                    {
                        if (dthistory != null)
                        {

                        }
                        result.Add(temp);
                        temp = new List<string>
                        {
                            dt.Rows[i][0].ToString()
                        };
                    }
                    temp.Add(dt.Rows[i][1].ToString());
                }
                result.Add(temp);
                //dataGridView.Rows.Add()
            }
            //result.Find
            if (result.Any())
            {
                var tempvalue = "";
                for(int i = 0;i<result.Count;i++)
                {
                    int index = dataGridView.Rows.Add();
                    for (int j = 0; j < result[i].Count; j++)
                    {
                        
                        dataGridView.Rows[index].Cells[0].Value = result[i][0];
                        tempvalue = result[i][j];
                        if(Status.Contains(tempvalue))
                        {
                            var pervieus = result[i][j - 1];

                        }
                        else
                        {
                            switch (tempvalue)
                            {
                                case "Boxing":
                                    dataGridView.Rows[index].Cells[1].Style.BackColor = Color.FromArgb(86, 156, 214);
                                    break;
                                case "Cutting":
                                    dataGridView.Rows[index].Cells[2].Style.BackColor = Color.FromArgb(86, 156, 214);
                                    break;
                                case "Packaging":
                                    dataGridView.Rows[index].Cells[3].Style.BackColor = Color.FromArgb(86, 156, 214);
                                    break;
                                case "Repair":
                                    dataGridView.Rows[index].Cells[4].Style.BackColor = Color.FromArgb(86, 156, 214);
                                    break;
                                case "Sampling":
                                case "SamplingOnPackaging":
                                    dataGridView.Rows[index].Cells[5].Style.BackColor = Color.FromArgb(86, 156, 214);
                                    break;
                                case "Weighing":
                                    dataGridView.Rows[index].Cells[6].Style.BackColor = Color.FromArgb(86, 156, 214);
                                    break;

                            }
                        }

                        //dataGridView.Rows[index].Cells[1].Value = true;
                    }
                }
                
            }
        }
    }
}
