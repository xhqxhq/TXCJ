
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

using System.Windows.Forms;
using Utils;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        //导入信息
        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Excel文件(*.xlsx)|*.xlsx;*.xls";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = dlg.FileName;

                //弹出提示框
                int index = dataGridView1.CurrentRow.Index;
                string projectName = dataGridView1.Rows[index].Cells[0].Value.ToString();
                //确认要导入到项目xxx中吗?
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确定要把excel表导入到 " + projectName + " 吗?", "系统提示", messButton);

                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {
                    //从excel中读到datatable
                    DataTable dataTable = FileUtils.GetExcelDatatable(filePath, "info");
                    //从datatable中写入access
                    int count = new AccessHelper(Environment.CurrentDirectory + "\\project\\"+projectName+"\\dbf\\photoSystem.accdb").InsertDataFormDatataleToInfo(dataTable);
                    MessageBox.Show("已成功将"+count + "条数据导入到"+projectName+"中!");

                    //更新lists集合
                    Utils.CommonUtils.loadDataToLists();
                }
                
            }

        }

        //新增项目
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
           
        }

        //删除项目
        private void button2_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;
            string projectName = dataGridView1.Rows[index].Cells[0].Value.ToString();
            //MessageBox.Show("确认要删除"+projectName+"吗？");
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确定要删除 "+projectName+" 吗?", "系统警示", messButton);

            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                //删除文件夹
                string path = FileUtils.ProjectPath+ projectName;
                Directory.Delete(path,true);
                //重新加载lists
                CommonUtils.loadDataToLists();
            }


        }

        //合并数据导出
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount==0)
            {
                MessageBox.Show("没有可以导出的数据！");
            }
            else
            {
                Form4 form4 = new Form4();
                
                form4.ShowDialog();
            }
            
        }

        //加载当前项目
        private void button5_Click(object sender, EventArgs e)
        {
            //获取datagridview当前选中的行的项目名
            int index = dataGridView1.CurrentRow.Index;
            string projectName = dataGridView1.Rows[index].Cells[0].Value.ToString();

            //MessageBox.Show("确认加载"+projectName+"吗?");
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确认加载" + projectName + "吗 ?", "系统警示", messButton);

            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                Form3 form3 = new Form3();
                form3.projectName = projectName;
                form3.ShowDialog();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //加载数据
            Utils.CommonUtils.loadDataToLists(); 
            //设置数据源
            dataGridView1.DataSource = Utils.FileUtils.lists;

            //datagridview格式设置
            //不显示出dataGridView1的最后一行空白   
            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.Columns[0].HeaderCell.Value = "项目名称";
            dataGridView1.Columns[1].HeaderCell.Value = "起始编号";
            dataGridView1.Columns[2].HeaderCell.Value = "导入人数";
            dataGridView1.Columns[3].HeaderCell.Value = "身份证照数";
            dataGridView1.Columns[4].HeaderCell.Value = "照片数量";

            //列宽自适应
            CommonUtils.AutoSizeColumn(dataGridView1);
            //调整第一列至合适宽度
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        
    }
}
