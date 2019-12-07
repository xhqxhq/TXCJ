using System;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //初始化日期
            date.Text = DateTime.Now.ToLongDateString().ToString();
            
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            string dateText = date.Text;
            string schoolNameText = schoolName.Text;
            string collectorNameText = collectorName.Text;
            string startNumberText = startNumber.Text;

            if ("" == schoolNameText)
                MessageBox.Show("请输入院校名称！");
            else if ("" == collectorNameText)
                MessageBox.Show("请输入采集员名称！"); 
            else if ("" == startNumberText)
                MessageBox.Show("请输入起始编号！"); 
            else
            {
                //进行文件夹的copy,从模板文件夹copy到当前项目
                //文件夹名
                string dirName = dateText + "_" + schoolNameText + "_" + collectorNameText + "_" + startNumberText;
                //判断是否有同名文件夹
                bool exist = Directory.Exists(Utils.FileUtils.ProjectPath + dirName);
                if (!exist)
                {
                    //文件夹copy操作
                    Utils.FileUtils.CopyDir(Utils.FileUtils.templatePath, Utils.FileUtils.ProjectPath + dirName);

                    //将新增数据添加到lists
                    Utils.CommonUtils.loadDataToLists();

                    //善后处理
                    this.Close();
                }
                else {
                    MessageBox.Show("此项目已存在，请重新输入！");
                    schoolName.Text = "";
                    collectorName.Text = "";
                    startNumber.Text = "";
                }
               
            }

        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //IsNumber：指定字符串中位于指定位置的字符是否属于数字类别
            //IsPunctuation：指定字符串中位于指定位置的字符是否属于标点符号类别
            //IsControl：指定字符串中位于指定位置的字符是否属于控制字符类别
            
            char result = e.KeyChar;
            if (char.IsDigit(result) || result == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("请输入数字！");
            }
        }

    }
}
