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
using Utils;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {

        
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            HashSet<string> set = new HashSet<string>();
            //初始化combobox
            string[] fileNameList = Directory.GetFileSystemEntries(FileUtils.ProjectPath, "*_*_*_*");
            //取出学校名字
            foreach (string name in fileNameList)
            {
                string[] s = name.Split('_');
                set.Add(s[1]);
            }
            //设置下拉框的数据源
            List<string> list = new List<string>(set);
            comboBox1.DataSource = list;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


            try
            {
                //获取学校名字
                string schoolName = comboBox1.SelectedItem.ToString();
                //MessageBox.Show("");
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确认要合并导出 " + schoolName + " 的所有数据吗？", "系统警示", messButton);

                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {
                    System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                    dialog.Description = "请选择保存路径";

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string filePath = dialog.SelectedPath;
                        if (string.IsNullOrEmpty(filePath))
                        {
                            MessageBox.Show(this, "文件夹路径不能为空", "提示");
                            return;
                        }
                        else
                        {
                            //具体文件保存位置
                            //C:\\Users\\yuan\\Desktop汇总信息导出2019 / 11 / 10 16:57:48.xlsx
                            //对字符串进行处理
                            string dealPath = "\\"+schoolName+"汇总信息导出" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".xls";
                            string path = filePath + dealPath;
                            //项目数
                            string[] names = Directory.GetFileSystemEntries(Utils.FileUtils.ProjectPath, "*_" + schoolName + "_*_*");
                            //把所有的info表汇总到SummaryInfo表
                            foreach (string name in names)
                            {
                                DataTable dataTable = new AccessHelper(name + "\\dbf\\photoSystem.accdb").GetDataTableFromDB("select * from info");
                                new AccessHelper(Application.StartupPath + "\\template\\base\\SummaryInfoDatabase\\photoSystem.accdb").InsertDataFormDatataleToSummaryInfo(dataTable);
                            }
                            //把SummaryInfo导入到excel表
                            new AccessHelper(Application.StartupPath + "\\template\\base\\SummaryInfoDatabase\\photoSystem.accdb").accessToExcel(path, "SummaryInfo");
                            //清空SummaryInfo表
                            new AccessHelper(Application.StartupPath + "\\template\\base\\SummaryInfoDatabase\\photoSystem.accdb").ExcuteSql("delete * from SummaryInfo where 1=1;");
                            MessageBox.Show("数据导出完毕!");
                            this.Close();
                        }

                    }

                }


            }
            catch (Exception exce)
            {
                MessageBox.Show(exce.Message);
            }
            }
        }
    }

