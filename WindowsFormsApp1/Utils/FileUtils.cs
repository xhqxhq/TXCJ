using System;
using System.ComponentModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.Model;
using System.Configuration;
using System.Collections.Generic;

namespace Utils
{
    class FileUtils
    {
        /*定义全局变量*/

        //获取当前工作目录的完全限定路径"D:\\新招生系统\\高校图像采集身份证阅读编号系统\\WindowsFormsApp1\\bin\\Debug"
        public static string templatePath = Application.StartupPath + "\\template\\base\\templateDirectory";

        //定义新生产的项目的路径
        public static string ProjectPath = System.Environment.CurrentDirectory + "\\project\\";

        //lists集合,用于form1页面的datagridview数据源
        public static BindingList<Project> lists = new BindingList<Project>();


        /*定义工具类方法*/


        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="str">文件内容</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            // 检查目标目录是否以目录分割字符结束如果不是则添加
            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
            {
                aimPath += Path.DirectorySeparatorChar;
            }
            // 判断目标目录是否存在如果不存在则新建
            if (!Directory.Exists(aimPath))
            {
                Directory.CreateDirectory(aimPath);
            }
            // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            // 遍历所有的文件和目录
            foreach (string file in fileList)
            {
                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                if (Directory.Exists(file))
                {
                    CopyDir(file, aimPath + Path.GetFileName(file));
                }
                // 否则直接Copy文件
                else
                {
                    System.IO.File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
        }

        /// <summary>
        /// Excel数据导入Datable
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable GetExcelDatatable(string fileUrl, string table)
        {
            //office2007之前 仅支持.xls
            //const string cmdText = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=1';";
            //支持.xls和.xlsx，即包括office2010等版本的   HDR=Yes代表第一行是标题，不是数据；
            const string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

            System.Data.DataTable dt = null;
            //建立连接
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, fileUrl));
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }


                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                //获取Excel的第一个Sheet名称
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString().Trim();

                //查询sheet中的数据
                string strSql = "select * from [" + sheetName + "]";
                OleDbDataAdapter da = new OleDbDataAdapter(strSql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, table);
                dt = ds.Tables[0];

                return dt;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return null;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

        /// <summary>
        /// 获取EXCEL的表 表名字列 
        /// </summary>
        /// <param name="p_ExcelFile">Excel文件</param>
        /// <returns>数据表</returns>
        public static List<string> GetExcelNameToList(string filePath)
        {
            List<string> list = new List<string>();
            if (!File.Exists(filePath))
            { return list; }
            const string cmdText = "Provider=Microsoft.Ace.OleDb.12.0;Data Source={0};Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            //建立连接
            OleDbConnection conn = new OleDbConnection(string.Format(cmdText, filePath));
            try
            {
                //打开连接
                if (conn.State == ConnectionState.Broken || conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                      list.Add(schemaTable.Rows[i]["Table_Name"].ToString());
                }
                return list;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return list;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }



        /// <summary>
        /// 文件复制函数
        /// </summary>
        /// <param name="srcdir">源文件夹</param>
        /// <param name="desdir">目的文件夹</param>
        public static void file_copy(string srcdir, string desdir)
        {
            DirectoryInfo thefolder = new DirectoryInfo(srcdir);
            foreach (FileInfo nextfile in thefolder.GetFiles())
            {
                try
                {
                    string filename = nextfile.Name;
                    string filefullname = nextfile.FullName;
                    string file = desdir + "\\" + filename;
                    //如果目的文件已经存在,先删除,再copy
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                        File.Copy(filefullname, file);
                    }
                    else   //不存在则直接copy
                    {
                        File.Copy(filefullname, file);
                    }
                }
                catch (System.Exception ex)
                {
                    //System.Console.WriteLine(ex.ToString());
                    MessageBox.Show(ex.Message);
                }
            }
        }


    }
}    



