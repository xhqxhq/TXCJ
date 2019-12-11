
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

using System.Windows.Forms;
using Utils;

using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            // 使用验证：1、使用时间；2、网卡MAC地址；
            if (ValidateEncrypt())
            {
                InitializeComponent();
            }
            
        }

        /// <summary>
        /// 验证：1、是否过期；2、是否本机；
        /// </summary>
        /// <returns></returns>
        private bool ValidateEncrypt()
        {
            bool result = false;

            // 获取网络时间
            DateTime chinaTime = GetNetDateTime(); 
            // 判断时间是否正确
            if (chinaTime.Equals(DateTime.MinValue))
            {
                MessageBox.Show("获取网络时间错误！");
                return false;
            }

            //将解密文件中的使用时间与本机时间进行对比
            try
            {
                //读取解密文件
                StreamReader sr = new StreamReader(System.Windows.Forms.Application.StartupPath + "\\Encryp.txt", false);
                string str = DESDecrypt(sr.ReadLine(), "EncrypEn");
                sr.Close();

                //将解密文件中的使用时间与本机时间进行对比
                if (!string.IsNullOrEmpty(str)) //非空
                {
                    string[] lstStr = str.Split('|');   //开始时间|结束时间
                    if (lstStr.Length == 2)
                    {
                        // 非空
                        if (string.IsNullOrEmpty(lstStr[0]) || string.IsNullOrEmpty(lstStr[1]))
                        {
                            MessageBox.Show("授权文件错误！请联系软件供应商！");
                            return false;
                        }

                        DateTime beginTime = Convert.ToDateTime(lstStr[0]);
                        DateTime endTime = Convert.ToDateTime(lstStr[1]);
                        if (beginTime <= chinaTime && chinaTime <= endTime) //在使用时间内
                        {
                            // 根据网卡MAC地址判断本机是否能使用
                            // 获取网卡MAC地址


                            // 判断本机是否能使用
                            //if ()
                            {
                                result = true;
                            }
                            //else
                            //{
                            //    return false;
                            //}
                        }
                        else if (chinaTime < beginTime)
                        {
                            MessageBox.Show("授权开始时间为：" + lstStr[0]);
                        }
                        else
                        {
                            MessageBox.Show("授权已过期！到期时间为：" + lstStr[1]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("授权文件错误！请联系软件供应商！");
                    }
                }
                else //为空则可能被用户删除
                {
                    MessageBox.Show("授权文件错误！请联系软件供应商！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return result;

        }

        /// <summary>
        /// DES 解密(数据加密标准，速度较快，适用于加密大量数据的场合)
        /// </summary>
        /// <param name="DecryptString">待解密的密文</param>
        /// <param name="DecryptKey">解密的密钥</param>
        /// <returns>returns</returns>
        private string DESDecrypt(string DecryptString, string DecryptKey)
        {
            if (string.IsNullOrEmpty(DecryptString)) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
            if (DecryptKey.Length != 8) { throw (new Exception("密钥必须为8位")); }
            byte[] m_btIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            string m_strDecrypt = "";
            DESCryptoServiceProvider m_DESProvider = new DESCryptoServiceProvider();
            try
            {
                byte[] m_btDecryptString = Convert.FromBase64String(DecryptString);
                MemoryStream m_stream = new MemoryStream();
                CryptoStream m_cstream = new CryptoStream(m_stream, m_DESProvider.CreateDecryptor(Encoding.Default.GetBytes(DecryptKey), m_btIV), CryptoStreamMode.Write);
                m_cstream.Write(m_btDecryptString, 0, m_btDecryptString.Length);
                m_cstream.FlushFinalBlock();
                m_strDecrypt = Encoding.Default.GetString(m_stream.ToArray());
                m_stream.Close(); m_stream.Dispose();
                m_cstream.Close(); m_cstream.Dispose();
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_DESProvider.Clear(); }
            return m_strDecrypt;
        }

        /// <summary>
        /// 获取网络日期时间 
        /// </summary>
        /// <returns></returns>
        public DateTime GetNetDateTime()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            DateTime dateTime = new DateTime();
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                {
                    if (h == "Date") {
                        dateTime = Convert.ToDateTime(headerCollection[h]); // {2019/12/10 17:13:17}
                    }
                }
                return dateTime;
            }
            catch (Exception) {
                return dateTime;
            }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
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
