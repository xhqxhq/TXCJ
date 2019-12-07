using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Utils
{
    class AccessHelper
    {
        private string conn_str = null;
        private OleDbConnection ole_connection = null;
        private OleDbCommand ole_command = null;
        private OleDbDataReader ole_reader = null;
        private DataTable dt = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AccessHelper()
        {
            //conn_str = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + Environment.CurrentDirectory + "\\project\\2019年11月8日_长江大学_袁启航_1001\\dbf\\photoSystem.accdb'";
            conn_str = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + Environment.CurrentDirectory + "\\project\\2019年11月8日_长江大学_袁启航_1001\\dbf\\photoSystem.accdb'";

            InitDB();
        }

        private void InitDB()
        {
            ole_connection = new OleDbConnection(conn_str);//创建实例
            ole_command = new OleDbCommand();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        ///<param name="db_path">数据库路径  Environment.CurrentDirectory + "\\project\\2019年11月8日_长江大学_袁启航_1001\\dbf\\photoSystem.accdb
        public AccessHelper(string db_path)
        {
            //conn_str ="Provider=Microsoft.Jet.OLEDB.4.0;Data Source='"+ db_path + "'";
            conn_str = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + db_path + "'";

            InitDB();
        }

        /// <summary>
        /// 转换数据格式
        /// </summary>
        ///<param name="reader">数据源
        /// <returns>数据列表</returns>
        private DataTable ConvertOleDbReaderToDataTable(ref OleDbDataReader reader)
        {
            DataTable dt_tmp = null;
            DataRow dr = null;
            int data_column_count = 0;
            int i = 0;

            data_column_count = reader.FieldCount;
            dt_tmp = BuildAndInitDataTable(data_column_count);

            if (dt_tmp == null)
            {
                return null;
            }

            while (reader.Read())
            {
                dr = dt_tmp.NewRow();

                for (i = 0; i < data_column_count; ++i)
                {
                    dr[i] = reader[i];
                }

                dt_tmp.Rows.Add(dr);
            }

            return dt_tmp;
        }

        /// <summary>
        /// 创建并初始化数据列表
        /// </summary>
        ///<param name="Field_Count">列的个数
        /// <returns>数据列表</returns>
        private DataTable BuildAndInitDataTable(int Field_Count)
        {
            DataTable dt_tmp = null;
            DataColumn dc = null;
            int i = 0;

            if (Field_Count <= 0)
            {
                return null;
            }

            dt_tmp = new DataTable();

            for (i = 0; i < Field_Count; ++i)
            {
                dc = new DataColumn(i.ToString());
                dt_tmp.Columns.Add(dc);
            }

            return dt_tmp;
        }

        /// <summary>
        /// 从数据库里面获取数据
        /// </summary>
        ///<param name="strSql">查询语句
        /// <returns>数据列表</returns>
        public DataTable GetDataTableFromDB(string strSql)
        {
            if (conn_str == null)
            {
                return null;
            }

            try
            {
                ole_connection.Open();//打开连接

                if (ole_connection.State == ConnectionState.Closed)
                {
                    return null;
                }

                ole_command.CommandText = strSql;
                ole_command.Connection = ole_connection;

                ole_reader = ole_command.ExecuteReader(CommandBehavior.Default);

                dt = ConvertOleDbReaderToDataTable(ref ole_reader);

                ole_reader.Close();
                ole_reader.Dispose();
            }
            catch (System.Exception e)
            {
                //Console.WriteLine(e.ToString());
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (ole_connection.State != ConnectionState.Closed)
                {
                    ole_connection.Close();
                }
            }

            return dt;
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        ///<param name="strSql">sql语句
        /// <returns>返回结果</returns>
        public int ExcuteSql(string strSql)
        {
            int nResult = 0;

            try
            {
                ole_connection.Open();//打开数据库连接
                if (ole_connection.State == ConnectionState.Closed)
                {
                    return nResult;
                }

                ole_command.Connection = ole_connection;
                ole_command.CommandText = strSql;

                nResult = ole_command.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                //Console.WriteLine(e.ToString());
                MessageBox.Show(e.Message);
                return nResult;
            }
            finally
            {
                if (ole_connection.State != ConnectionState.Closed)
                {
                    ole_connection.Close();
                }
            }

            return nResult;
        }


        /// <summary>
        /// 从System.Data.DataTable导入数据到数据库中的info表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int InsertDataFormDatataleToInfo(DataTable dt)
        {
            int count = 0;

            //int ID            = 0;
            string stuName = "";
            string stuSex = "";
            string idCardNum = "";
            string stuID = "";
            string stuDepartment = "";
            string stuProfession = "";
            string stuClass = "";
            //string testNum = "";
            string stuLevel = "";
            //string collectImg = "";
            //string idCardImg = "";
            string years = "";
            string graduateYear = "";
            string isPay = "";
            string grade = "";
            //string isManual = "";

            //string conn_str = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + Environment.CurrentDirectory + "\\project\\2019年11月8日_长江大学_袁启航_1001\\dbf\\photoSystem.accdb'";
            //OleDbConnection ole_connection = new OleDbConnection(conn_str);//创建实例
            //OleDbCommand ole_command = new OleDbCommand();

            try
            {
                ole_connection.Open();//打开数据库连接
                if (ole_connection.State == ConnectionState.Closed)
                { return count; }
                ole_command.Connection = ole_connection;


                foreach (DataRow dr in dt.Rows)
                {
                    // ID = int.Parse(dr["照片编号"].ToString().Trim());
                    stuName = dr["姓名"].ToString().Trim();
                    stuSex = dr["性别"].ToString().Trim();
                    idCardNum = dr["身份证号"].ToString().Trim();
                    stuID = dr["学号"].ToString().Trim();
                    stuDepartment = dr["院系"].ToString().Trim();
                    stuProfession = dr["专业"].ToString().Trim();
                    stuClass = dr["班级"].ToString().Trim();
                    stuLevel = dr["层次"].ToString().Trim();
                    years = dr["学制"].ToString().Trim();
                    grade = dr["当前所在年级"].ToString().Trim();
                    graduateYear = dr["毕业年份"].ToString().Trim();
                    isPay = dr["是否缴费"].ToString().Trim();



                    string strSql = string.Format("Insert into info (" +
                             "stuName,	stuSex,	idCardNum,	stuID,	stuDepartment,	stuProfession,	stuClass,	stuLevel,	years,   grade, graduateYear,	isPay" +
                             ") Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", stuName, stuSex, idCardNum, stuID, stuDepartment, stuProfession, stuClass, stuLevel, years, grade, graduateYear, isPay);

                    ole_command.CommandText = strSql;
                    ole_command.ExecuteNonQuery();
                    count++;

                }

            }
            catch (System.Exception e)
            {
                //Console.WriteLine(e.ToString());
                MessageBox.Show(e.Message);
                return count;
            }
            finally
            {
                if (ole_connection.State != ConnectionState.Closed)
                {
                    ole_connection.Close();
                }
            }
            return count;
        }


        /// <summary>
        /// 从System.Data.DataTable导入数据到数据库中的Summaryinfo表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int InsertDataFormDatataleToSummaryInfo(DataTable dt)
        {
            int count = 0;

            int ID;
            string stuName;
            string stuSex;
            string idCardNum;
            string stuID;
            string stuDepartment;
            string stuProfession;
            string stuClass;
            string stuLevel;
            string years;
            string grade;
            string graduateYear;
            string isPay;
            string isPhoto;
            string isManual;

            try
            {
                ole_connection.Open();//打开数据库连接
                if (ole_connection.State == ConnectionState.Closed)
                { return count; }
                ole_command.Connection = ole_connection;


                foreach (DataRow dr in dt.Rows)
                {
                    ID = string.IsNullOrEmpty(dr[0].ToString().Trim()) ? 0 : int.Parse(dr[0].ToString().Trim());
                    stuName = dr[1].ToString().Trim();
                    stuSex = dr[2].ToString().Trim();
                    idCardNum = dr[3].ToString().Trim();
                    stuID = dr[4].ToString().Trim();
                    stuDepartment = dr[5].ToString().Trim();
                    stuProfession = dr[6].ToString().Trim();
                    stuClass = dr[7].ToString().Trim();
                    stuLevel = dr[8].ToString().Trim();
                    years = dr[9].ToString().Trim();
                    grade = dr[10].ToString().Trim();
                    graduateYear = dr[11].ToString().Trim();
                    isPay = dr[12].ToString().Trim();
                    isPhoto = dr[13].ToString().Trim();
                    isManual = dr[14].ToString().Trim();

                    string strSql;
                    if (ID == 0)
                    {
                        strSql = string.Format("Insert into SummaryInfo (" +
                               " stuName,	stuSex,	idCardNum,	stuID,	stuDepartment,	stuProfession,	stuClass,	stuLevel,	years,   grade, graduateYear,	isPay, isPhoto , isManual" +
                               ") Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')", stuName, stuSex, idCardNum, stuID, stuDepartment, stuProfession, stuClass, stuLevel, years, grade, graduateYear, isPay, isPhoto, isManual);
                    }
                    else
                    {
                        strSql = string.Format("Insert into SummaryInfo (" +
                             "ID, stuName,	stuSex,	idCardNum,	stuID,	stuDepartment,	stuProfession,	stuClass,	stuLevel,	years,   grade, graduateYear,	isPay, isPhoto , isManual" +
                             ") Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')", ID, stuName, stuSex, idCardNum, stuID, stuDepartment, stuProfession, stuClass, stuLevel, years, grade, graduateYear, isPay, isPhoto, isManual);
                    }


                    ole_command.CommandText = strSql;
                    ole_command.ExecuteNonQuery();
                    count++;

                }

            }
            catch (System.Exception e)
            {
                //Console.WriteLine(e.ToString());
                MessageBox.Show(e.Message);
                return count;
            }
            finally
            {
                if (ole_connection.State != ConnectionState.Closed)
                {
                    ole_connection.Close();
                }
            }
            return count;
        }
        
        
       

        


        public void accessToExcel(string filePath, string tableName)
        {
            //string filePath = "D:\\info.xls";
            string strSql = "select count(*) from [" + tableName + "]";

            try
            {
                ole_connection.Open();//打开数据库连接
                if (ole_connection.State == ConnectionState.Closed)
                {
                    return;
                }

                ole_command.Connection = ole_connection;
                ole_command.CommandText = strSql;

                //ole_command.ExecuteScalar();
                int num = (int)(ole_command.ExecuteScalar());
                //如果数据项的个数大于一个sheet表的最大行数，则拆分保存在多个sheet表中
                if (num <= 65535)
                {
                    // [Excel 8.0;database= excel名].[sheet名] 如果是新建sheet表不能加$，如果向sheet里插入数据要加$
                    // Excel 2003的sheet表最大行数65536，最大列数256。因为列头要占据一行，所以最多存储65535条数据

                    ole_command = new OleDbCommand("select top 65535 stuName as 姓名, stuSex as 性别, idCardNum as 身份证号, stuID as 学号,stuDepartment as 院系,stuProfession as 专业,stuClass as 班级,stuLevel as 层次,years as 学制, grade as 当前所在年级,graduateYear as 毕业年份,isPay as 是否缴费,isPhoto as 是否照相,isManual as 是否手动输入" +
                        "  into [Excel 8.0;database=" + filePath + "].[表1] from  " + tableName, ole_connection);
                    ole_command.ExecuteNonQuery();
                }
                else
                {
                    int num2 = num, i = 1;
                    while (num2 > 0)
                    {
                        ole_command = new OleDbCommand("select top 65535 * into [Excel 8.0;database=" + filePath + "].[表" + i + "] from (select top " + num2 + " stuName as 姓名, stuSex as 性别, idCardNum as 身份证号, stuID as 学号,stuDepartment as 院系,stuProfession as 专业,stuClass as 班级,stuLevel as 层次,years as 学制, grade as 当前所在年级,graduateYear as 毕业年份,isPay as 是否缴费,isPhoto as 是否照相,isManual as 是否手动输入 from " + tableName + " )  ", ole_connection);
                        /*ole_command = new OleDbCommand("select top 65535 stuName as 姓名, stuSex as 性别, idCardNum as 身份证号, stuID as 学号,stuDepartment as 学院,stuProfession as 专业,stuClass as 班级,stuLevel as 层次,years as 学制, grade as 当前年级,graduateYear as 毕业年份,isPay as 是否缴费,isPhoto as 是否照相,isManual as 是否手动输入  " +
                         "into [Excel 8.0;database=" + filePath + "].[sheet1] from info ", ole_connection);*/
                        ole_command.ExecuteNonQuery();
                        num2 -= 65535;
                        i++;
                    }
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            finally
            {
                if (ole_connection.State != ConnectionState.Closed)
                {
                    ole_connection.Close();
                }
            }

        }


    }
}
