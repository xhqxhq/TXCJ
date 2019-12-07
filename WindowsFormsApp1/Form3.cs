using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using System.Xml;
using System.Threading;
using ZXing;
using ZXing.Common;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        //用于接收form1传递过来的项目名   2019年11月10日_湖北大学_张三_2001
        public string projectName;
        //form3下方展示列表的数据源
        DataTable dataTable = new DataTable();
        //新开辟的线程
        Thread thread;

        //  public string ProjectName { get; set; }

        public Form3()
        {
            InitializeComponent();
        }




        // 显示主界面
        private void Form3_Load(object sender, EventArgs e)
        {
            //允许其他线程访问主线程创建的控件
            //Control.CheckForIllegalCrossThreadCalls = false;

            //string sqlStr = "select * from info where ID <> 0 order by ID desc;";//筛选出数据库中编号不为0和空的数据
            string sqlStr = "select * from info order by ID desc;";//所有数据
            dataTable = new Utils.AccessHelper(Utils.FileUtils.ProjectPath + projectName + "\\dbf\\photoSystem.accdb").GetDataTableFromDB(sqlStr);
            //设置datagridview的数据源
            dataGridView1.DataSource = dataTable;
            //给datagridview第一行设置红色背景(编号最大行)
            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("此项目中已编号学生数据为空!");
            }
            else
            {
                dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
            }
            //列宽自适应
            Utils.CommonUtils.AutoSizeColumn(dataGridView1);


        }




        //保存按钮
        private void button7_Click(object sender, EventArgs e)
        {

            //进行处理
            string stuName = textBox1.Text.Trim();
            string stuSex = textBox2.Text.Trim();
            string idCardNum = textBox3.Text.Trim();
            string stuId = textBox4.Text.Trim();
            string stuDept = textBox5.Text.Trim();
            string stuProfession = textBox6.Text.Trim();
            string stuClass = textBox7.Text.Trim();
            string stuLevel = textBox8.Text.Trim();
            string years = textBox9.Text.Trim();
            string grade = textBox10.Text.Trim();
            string graduateYear = textBox11.Text.Trim();
            string isPay;
            //ID，用来区分是新增保存还是直接修改
            string ID = IDtextBox12.Text.Trim();

            if (radioButton1.Checked == true)
            {
                isPay = "1";
            }
            else
            {
                isPay = "0";
            }

            //其中，身份证号，姓名必填，其他依照需要选填
            if ("" == idCardNum)
                MessageBox.Show("请输入身份证号！");
            else if ("" == stuName)
                MessageBox.Show("请输入学生姓名！");
            //else if ("" == stuSex)
            //    MessageBox.Show("请输入学生性别！");
            else if (idCardNum.Trim().Trim().ToCharArray().Length != 18)
            {
                MessageBox.Show("身份证号输入有误！请重新输入！");
            }
            else
            {
                //先进行提示：
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("保存后不可删除，只可修改！确定要保存吗？ ", "系统提示", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {
                    Utils.AccessHelper accessHelper = new AccessHelper(Utils.FileUtils.ProjectPath + projectName + "\\dbf\\photoSystem.accdb");
                    if (string.IsNullOrEmpty(ID))
                    {           //说明此人没有进行编号，则进行新增保存操作

                        //查出数据库中最大的id
                        string maxIdSqlStr = "select Max(ID) from info";
                        //如果表中一条数据都没有,则执行结果为""
                        int maxId;
                        if (string.IsNullOrEmpty(accessHelper.GetDataTableFromDB(maxIdSqlStr).Rows[0][0].ToString()))
                        {
                            maxId = 1;
                        }
                        else
                        {
                            maxId = int.Parse(accessHelper.GetDataTableFromDB(maxIdSqlStr).Rows[0][0].ToString()) + 1;
                        }
                        string isPhoto = "1";   //已照相
                        string isManual = "1";   //手动输入标记
                        //新增之后立马输出这个学生的身份证，姓名，照片编号，代表此学生已经照相
                        string sqlStr = string.Format("Insert into info (" +
                                 "ID , stuName,	stuSex,	idCardNum,	stuID,	stuDepartment,	stuProfession,	stuClass,	stuLevel,	years,   grade, graduateYear,	isPay ,isPhoto , isManual" +
                                 ") Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')", maxId, stuName, stuSex, idCardNum, stuId, stuDept, stuProfession, stuClass, stuLevel, years, grade, graduateYear, isPay, isPhoto, isManual);
                        accessHelper.ExcuteSql(sqlStr);
                        //清空textbox框，避免多次点击按钮时会插入多次
                        clearTextbox();
                        //进行弹窗照相操作
                        string showStr = idCardNum + "\n" + stuName + "\n" + maxId;
                        MessageBox.Show(showStr);

                    }
                    else        //说明此人有编号，则进行修改保操作操作
                    {
                        string sqlStr = string.Format("update info set  stuName = '{0}', stuSex = '{1}', idCardNum = '{2}', stuID = '{3}', stuDepartment = '{4}', stuProfession = '{5}', stuClass = '{6}', stuLevel = '{7}', years = '{8}', grade = '{9}', graduateYear = '{10}', isPay = '{11}' where ID = {12}",
                            stuName, stuSex, idCardNum, stuId, stuDept, stuProfession, stuClass, stuLevel, years, grade, graduateYear, isPay, ID);
                        accessHelper.ExcuteSql(sqlStr);
                    }

                    //更新table表
                    string sqlStr1 = "select * from info where ID <> 0 order by ID desc;";//筛选出数据库中编号不为0和空的数据
                    dataTable = accessHelper.GetDataTableFromDB(sqlStr1);
                    //设置datagridview的数据源
                    dataGridView1.DataSource = dataTable;
                    //第一行显示为红色
                    dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }



        //新增按钮
        private void button9_Click(object sender, EventArgs e)
        {
            //清除上面textbox中的信息
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
            PhotoNumberLabel.Text = "";
            studentNameLabel.Text = "";
            IDtextBox12.Text = "";
            //默认未缴费
            radioButton1.Checked = false;
            radioButton2.Checked = true;

        }

        //单击datagridview的单元格时触发
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //把选中行的数据自动添加到form3的身份信息框中
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString().Trim();
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString().Trim();
                textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString().Trim();
                textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim();
                textBox5.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString().Trim();
                textBox6.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString().Trim();
                textBox7.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString().Trim();
                textBox8.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString().Trim();
                textBox9.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString().Trim();
                textBox10.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString().Trim();
                textBox11.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString().Trim();
                IDtextBox12.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim();


                PhotoNumberLabel.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim();
                studentNameLabel.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString().Trim();

                //是否缴费，1已缴费，0未缴费
                if (int.Parse(dataGridView1.CurrentRow.Cells[12].Value.ToString().Trim()) == 1)
                {
                    //已缴费：
                    radioButton1.Checked = true;
                }
                else if (int.Parse(dataGridView1.CurrentRow.Cells[12].Value.ToString().Trim()) == 0)
                {
                    radioButton2.Checked = true;
                }

                //清除图片
                pictureBox1.Image = null;
                pictureBox2.Image = null;

                //显示身份证图片和采集图片(如果有则显示)
                //身份证图片绝对路径
                string idCardPicture = Utils.FileUtils.ProjectPath + projectName + "\\bmp\\" + textBox3.Text + ".bmp";
                //采集照片的绝对路径
                string photo = Utils.FileUtils.ProjectPath + projectName + "\\photo\\" + textBox3.Text + ".JPG";

                if (File.Exists(idCardPicture))
                {
                    //pictureBox1.Image = Image.FromFile(idCardPicture);
                    System.Drawing.Image img = System.Drawing.Image.FromFile(idCardPicture);
                    System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                    img.Dispose();
                    pictureBox1.Image = bmp;
                }
                if (File.Exists(photo))
                {
                    /*pictureBox2.Load("d:\\DSC_7006.JPG");*/
                    //pictureBox2.Image = Image.FromFile(photo);
                    System.Drawing.Image img = System.Drawing.Image.FromFile(photo);
                    System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                    img.Dispose();
                    pictureBox2.Image = bmp;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }


        //扫描条形码编号
        private void readBarCode_Click(object sender, EventArgs e)
        {

        }

        //编号数据导出
        //将datatable中的数据导出到excel中
        private void exportNumberedData_Click(object sender, EventArgs e)
        {
            try
            {


                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确认要导出编号数据吗？", "系统警示", messButton);

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
                            string dealPath = "\\编号数据导出-" + projectName + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(":", "-") + ".xls";
                            string path = filePath + dealPath;

                            //先将datatable中的数据导入到access中
                            new AccessHelper(Application.StartupPath + "\\template\\base\\SummaryInfoDatabase\\photoSystem.accdb").InsertDataFormDatataleToSummaryInfo(dataTable);

                            //把SummaryInfo导入到excel表
                            new AccessHelper(Application.StartupPath + "\\template\\base\\SummaryInfoDatabase\\photoSystem.accdb").accessToExcel(path, "SummaryInfo");
                            //清空SummaryInfo表
                            new AccessHelper(Application.StartupPath + "\\template\\base\\SummaryInfoDatabase\\photoSystem.accdb").ExcuteSql("delete * from SummaryInfo where 1=1;");
                            MessageBox.Show("数据导出完毕!");
                        }

                    }


                }
            }
            catch (Exception exce)
            {
                MessageBox.Show(exce.Message);
            }
        }


        //图片文件名转换
        private void conversePhotoName_Click(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("请确认照相完毕,然后进行转换!", "系统警示", messButton);

            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.Description = "请选择照片所在的文件夹";

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
                        //对字符串进行处理
                        //string srcDir= filePath ;
                        //string descDir = FileUtils.ProjectPath + projectName + "\\photo";
                        //复制图片
                        // FileUtils.file_copy(srcDir, descDir);
                        //更新form1中的lists
                        CommonUtils.loadDataToLists();
                        //进行改名处理

                        //数据库中编号1对应照片名称DSC_0001.JPG    以此类推......
                        //先获取已编号的学生数据
                        List<string> noPhotoList = new List<string>();
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {

                            string strId = int.Parse(dataTable.Rows[i][0].ToString()).ToString("0000");
                            string strIdCardNum = dataTable.Rows[i][3].ToString();
                            //string srcPath = FileUtils.ProjectPath + projectName + "\\photo\\DSC_" + strId + ".JPG";
                            string srcPath = filePath + "\\DSC_" + strId + ".JPG";
                            //进行改名并复制操作：
                            if (File.Exists(srcPath))
                            {
                                string destPath = FileUtils.ProjectPath + projectName + "\\photo\\" + strIdCardNum + ".JPG";
                                File.Copy(srcPath, destPath, true);
                            }
                            else
                            {
                                //说明此学生已经编号,但是没有导入此学生的照片,
                                noPhotoList.Add(strId);
                            }
                        }


                        //处理完毕
                        if (noPhotoList.Count == 0)
                        {
                            //正常
                            MessageBox.Show("图片文件名转换完毕!");
                        }
                        else
                        {
                            //数据处理不正常
                            StringBuilder sb = new StringBuilder();
                            sb.Append("编号为: ");
                            for (int i = 0; i < noPhotoList.Count; i++)
                            {
                                string s = noPhotoList[i] + "  ";
                                sb.Append(s);
                            }
                            sb.Append("的学生没有照片!");
                            MessageBox.Show(sb.ToString());
                        }

                    }

                }


            }
        }


        //阅读身份证编号
        private void readIdCard_Click(object sender, EventArgs e)
        {
            /*if (thread!=null)           //说明此线程已经被开辟过，此时再进入这个代码就说明时重复点击按钮，弹出提示
            {
                MessageBox.Show("请不要重复点击！");
                return;
            }*/
            try
            {
                //先确保端口关闭
                DeviceUtils.CloseComm();
                //初始化端口
                int intOpenRet = DeviceUtils.InitComm(1001);
                if (intOpenRet != 1)
                {
                    MessageBox.Show("身份证信息：读卡器连接失败 ");
                    return;
                }
                //ThreadPool.QueueUserWorkItem(new WaitCallback(readCard));
                /*if (thread == null)
                {
                    thread = new Thread(readCard);
                    thread.Start();
                }*/



                //开启副线程进行循环认证
                if (thread == null)
                {
                    thread = new Thread(new ThreadStart(Authenticate));
                    thread.Start();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void Authenticate()
        {

            while (true)
            {
                //卡认证
                int intReadRet = DeviceUtils.Authenticate();
                //label1.Text = "intReadRet:" + intReadRet;
                if (intReadRet != 1)
                {
                    // MessageBox.Show("身份证信息:未认证 ");
                    continue;
                }
                readCard();
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            //窗口 关闭时关闭读卡器端口
            //DeviceUtils.CloseComm();

            //更新form1的lists
            CommonUtils.loadDataToLists();


            if (thread != null)
            {
                thread.Abort();
            }

        }


        delegate void delegateReadCard();
        private void readCard()
        {
            if (this.InvokeRequired)
            {
                delegateReadCard drc = new delegateReadCard(readCard);
                this.Invoke(drc);
            }
            else
            {
                try
                {
                    //int len = 0;
                    StringBuilder Name = new StringBuilder(31);
                    StringBuilder Gender = new StringBuilder(3);
                    StringBuilder Folk = new StringBuilder(10);
                    StringBuilder BirthDay = new StringBuilder(9);
                    StringBuilder Code = new StringBuilder(19);
                    StringBuilder Address = new StringBuilder(71);
                    StringBuilder Agency = new StringBuilder(31);
                    StringBuilder ExpireStart = new StringBuilder(9);
                    StringBuilder ExpireEnd = new StringBuilder(9);
                    //StringBuilder directory = new StringBuilder(100);
                    StringBuilder pucFPMsg = new StringBuilder(1024);
                    StringBuilder pMsg = new StringBuilder(100);


                    //如果有照片，先删除
                    if (File.Exists(System.Environment.CurrentDirectory + "\\photo.bmp"))
                    {
                        File.Delete(System.Environment.CurrentDirectory + "\\photo.bmp");
                    }
                    //读取信息，存照片
                    int ReadBaseInfosRet = DeviceUtils.ReadBaseInfos(Name, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd);  //读取信息同时会保存图片，
                    if (ReadBaseInfosRet != 1)
                    {
                        MessageBox.Show("身份证信息：读身份证失败");
                        DeviceUtils.CloseComm();
                        return;
                    }
                    else
                    {
                        //读取成功，此时照片被保存到bin//debug目录下
                        //挪动照片
                        if ((File.Exists(System.Environment.CurrentDirectory + "\\photo.bmp")) && (File.Exists(FileUtils.ProjectPath + projectName + "\\bmp\\" + Code + ".bmp")))
                        {
                            File.Copy(System.Environment.CurrentDirectory + "\\photo.bmp", FileUtils.ProjectPath + projectName + "\\bmp\\" + Code + ".bmp", true);
                            File.Delete(System.Environment.CurrentDirectory + "\\photo.bmp");
                        }
                        if ((File.Exists(System.Environment.CurrentDirectory + "\\photo.bmp")) && (!File.Exists(FileUtils.ProjectPath + projectName + "\\bmp\\" + Code + ".bmp")))
                        {
                            File.Move(System.Environment.CurrentDirectory + "\\photo.bmp", FileUtils.ProjectPath + projectName + "\\bmp\\" + Code + ".bmp");
                        }
                    }

                    Utils.AccessHelper accessHelper = new AccessHelper(Utils.FileUtils.ProjectPath + projectName + "\\dbf\\photoSystem.accdb");
                    //先判断已排除这种情况：此学生已经刷身份证了，在数据库中有信息，也已经编号，此时再次刷身份证，则提示已刷过请不要重复刷
                    DataTable d = accessHelper.GetDataTableFromDB("select * from info where idCardNum = '" + Code + "' and ID is not null ");
                    int i = d.Rows.Count;
                    if (i != 0)       //存在已有信息且这个学生已编号的情况
                    {
                        MessageBox.Show("此学生已照相过，请勿重复刷身份证！");
                        return;
                    }
                    //查出数据库中最大的id
                    string maxIdSqlStr = "select Max(ID) from info";
                    //如果表中一条数据都没有,则执行结果为""
                    int maxId;
                    if (string.IsNullOrEmpty(accessHelper.GetDataTableFromDB(maxIdSqlStr).Rows[0][0].ToString()))
                    {
                        maxId = 1;
                    }
                    else
                    {
                        maxId = int.Parse(accessHelper.GetDataTableFromDB(maxIdSqlStr).Rows[0][0].ToString()) + 1;
                    }

                    //根据身份证去数据库查是否有这个学生的记录，
                    DataTable dataTable1 = accessHelper.GetDataTableFromDB("select * from info where idCardNum = '" + Code + "' ");
                    int count = dataTable1.Rows.Count;

                    //如果有：
                    if (count != 0)
                    {
                        //给次学生编号
                        accessHelper.ExcuteSql("update info set ID = " + maxId + " where idCardNum = '" + Code + "' ");
                    }
                    else
                    {
                        //数据库中没有此学生
                        //添加学生信息并编号
                        string isPhoto = "1";   //已照相
                        string isManual = "0";   //手动输入标记
                        string isPay = "0";    //未缴费
                        //新增之后立马输出这个学生的身份证，姓名，照片编号，代表此学生已经照相
                        string sqlStr = string.Format("Insert into info (" +
                                 "ID , stuName,	stuSex,	idCardNum,	stuID,	stuDepartment,	stuProfession,	stuClass,	stuLevel,	years,   grade, graduateYear,	isPay ,isPhoto , isManual" +
                                 ") Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')", maxId, Name, Gender, Code, "", "", "", "", "", "", "", "", isPay, isPhoto, isManual);
                        accessHelper.ExcuteSql(sqlStr);
                    }


                    //填充中间的姓名和照片编号
                    studentNameLabel.Text = Name.ToString();
                    PhotoNumberLabel.Text = maxId.ToString();
                    //展示身份证图片(先清除后展示)
                    //清除图片
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;

                    //显示身份证图片和采集图片(如果有则显示)
                    //身份证图片绝对路径
                    string idCardPicture = Utils.FileUtils.ProjectPath + projectName + "\\bmp\\" + Code + ".bmp";
                    //采集照片的绝对路径
                    string photo = Utils.FileUtils.ProjectPath + projectName + "\\photo\\" + Code + ".JPG";

                    if (File.Exists(idCardPicture))
                    {
                        //pictureBox1.Image = Image.FromFile(idCardPicture);   会占用此图片
                        System.Drawing.Image img = System.Drawing.Image.FromFile(idCardPicture);
                        System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                        img.Dispose();
                        pictureBox1.Image = bmp;
                    }
                    if (File.Exists(photo))
                    {       /*pictureBox2.Load("d:\\DSC_7006.JPG");*/
                        //pictureBox2.Image = Image.FromFile(photo);
                        System.Drawing.Image img = System.Drawing.Image.FromFile(photo);
                        System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                        img.Dispose();
                        pictureBox2.Image = bmp;
                    }


                    //更新展示列表数据源
                    string sqlStr1 = "select * from info where ID <> 0 order by ID desc;";//筛选出数据库中编号不为0和空的数据
                    dataTable = accessHelper.GetDataTableFromDB(sqlStr1);
                    //设置datagridview的数据源
                    dataGridView1.DataSource = dataTable;
                    //第一行显示为红色
                    dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;

                    //清除textbox
                    clearTextbox();

                    //进行弹窗照相操作
                    string showStr = Code + "\n" + Name + "\n" + maxId;
                    MessageBox.Show(showStr);

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);

                }
            }
        }


        private void clearTextbox()
        {
            IDtextBox12.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox11.Text = "";
        }

        //// 根据身份证产生条形码
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    // 1.设置条形码规格
        //    EncodingOptions encodeOption = new EncodingOptions();
        //    encodeOption.Height = 130; // 必须制定高度、宽度
        //    encodeOption.Width = 240;

        //    // 2.生成条形码图片并保存
        //    ZXing.BarcodeWriter wr = new BarcodeWriter();
        //    wr.Options = encodeOption;
        //    wr.Format = BarcodeFormat.CODE_128;
        //    Bitmap img = wr.Write(textBox3.Text); // 生成条形码图片
        //    Bitmap newbm = KiSetText(img, textBox1.Text, 10, 5);//添加名字
        //    string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\project\\" + projectName + "\\jpg\\" + textBox3.Text + ".jpg";
        //    newbm.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
        //    DialogResult dr = MessageBox.Show("条形码已经生成！ ", "系统提示", messButton);
        //}

        // 批量产生条形码
        private void button1_Click(object sender, EventArgs e)
        {
            // 1.设置条形码规格
            EncodingOptions encodeOption = new EncodingOptions();
            encodeOption.Height = 130; // 必须制定高度、宽度
            encodeOption.Width = 240;
            ZXing.BarcodeWriter wr = new BarcodeWriter();
            wr.Options = encodeOption;
            wr.Format = BarcodeFormat.CODE_128;

            // 2.从datatable批量生成条形码图片并保存
            string sqlStr = "select * from info order by ID desc;";//所有数据
            dataTable = new Utils.AccessHelper(Utils.FileUtils.ProjectPath + projectName + "\\dbf\\photoSystem.accdb").GetDataTableFromDB(sqlStr);

            for (int i=0; i < dataTable.Rows.Count; i++)
            {
                string idCardNum = dataTable.Rows[i][3].ToString();
                Bitmap img = wr.Write(idCardNum);   // 根据身份证号生成条形码图片
                Bitmap newbm = KiSetText(img, dataTable.Rows[i][1].ToString(), 10, 5);    //添加名字
                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\project\\" + projectName + "\\jpg\\" + idCardNum + ".jpg";
                newbm.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            // 生成结束弹出通知
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("条形码已经生成！ ", "系统提示", messButton);
        }

        public static Bitmap KiSetText(Bitmap b, string txt, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            Bitmap resizeImage = new Bitmap(b.Width, b.Height + 40);
            Graphics gfx = Graphics.FromImage(resizeImage);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, 400, 100));

            gfx.DrawImageUnscaled(b, 0, 40);

            // 作为演示，我们用Arial字体，大小为32，红色。
            FontFamily fm = new FontFamily("Arial");
            Font font = new Font(fm, 24, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush sb = new SolidBrush(Color.Black);

            gfx.DrawString(txt, font, sb, new PointF(x, y));
            gfx.Dispose();

            return resizeImage;
        }

        // 根据身份证查询，并显示
        private void button4_Click(object sender, EventArgs e)
        {
            string idCardNum = textBox3.Text.Trim();

            // 根据身份证查询
            if ("" == idCardNum)
                MessageBox.Show("请输入身份证号！");
            else if (idCardNum.Trim().Trim().ToCharArray().Length != 18)
            {
                MessageBox.Show("身份证号输入有误！请重新输入！");
            }
            else
            {
                Utils.AccessHelper accessHelper = new AccessHelper(Utils.FileUtils.ProjectPath + projectName + "\\dbf\\photoSystem.accdb");
                
                //根据身份证号查询数据库
                string selectIdSqlStr = "select * from info where idCardNum = '" + idCardNum + "'";
                //如果表中一条数据都没有,则执行结果为""
                if (string.IsNullOrEmpty(accessHelper.GetDataTableFromDB(selectIdSqlStr).Rows[0][0].ToString()))
                {
                    //提示表中无数据：
                    MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                    DialogResult dr = MessageBox.Show("没有找到该生！", "系统提示", messButton);
                }
                else
                {
                    dataTable = accessHelper.GetDataTableFromDB(selectIdSqlStr);

                    //上方显示
                    //把查询的数据显示到form3的身份信息框中
                    textBox1.Text = dataTable.Rows[0][1].ToString();//姓名
                    textBox2.Text = dataTable.Rows[0][2].ToString();//性别
                    textBox3.Text = dataTable.Rows[0][3].ToString();//身份证号
                    textBox4.Text = dataTable.Rows[0][4].ToString();//学号
                    textBox5.Text = dataTable.Rows[0][5].ToString();//院系
                    textBox6.Text = dataTable.Rows[0][6].ToString();//专业
                    textBox7.Text = dataTable.Rows[0][7].ToString();//班级
                    textBox8.Text = dataTable.Rows[0][8].ToString();//层次
                    textBox9.Text = dataTable.Rows[0][9].ToString();//学制
                    textBox10.Text = dataTable.Rows[0][10].ToString();//当前所在年级
                    textBox11.Text = dataTable.Rows[0][11].ToString();//毕业年份
                    IDtextBox12.Text = dataTable.Rows[0][0].ToString();//是否缴费

                    PhotoNumberLabel.Text = dataTable.Rows[0][0].ToString();//
                    studentNameLabel.Text = dataTable.Rows[0][1].ToString();//

                    //是否缴费，1已缴费，0未缴费
                    if (int.Parse(dataTable.Rows[0][12].ToString()) == 1)
                    {
                        //已缴费：
                        radioButton1.Checked = true;
                    }
                    else if (int.Parse(dataTable.Rows[0][12].ToString()) == 0)
                    {
                        radioButton2.Checked = true;
                    }

                    //清除图片
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;

                    //显示身份证图片和采集图片(如果有则显示)
                    //身份证图片绝对路径
                    string idCardPicture = Utils.FileUtils.ProjectPath + projectName + "\\bmp\\" + textBox3.Text + ".bmp";
                    //采集照片的绝对路径
                    string photo = Utils.FileUtils.ProjectPath + projectName + "\\photo\\" + textBox3.Text + ".JPG";
                    if (File.Exists(idCardPicture)) //身份证图片
                    {
                        //pictureBox1.Image = Image.FromFile(idCardPicture);
                        System.Drawing.Image img = System.Drawing.Image.FromFile(idCardPicture);
                        System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                        img.Dispose();
                        pictureBox1.Image = bmp;
                    }
                    if (File.Exists(photo)) //采集照片
                    {
                        /*pictureBox2.Load("d:\\DSC_7006.JPG");*/
                        //pictureBox2.Image = Image.FromFile(photo);
                        System.Drawing.Image img = System.Drawing.Image.FromFile(photo);
                        System.Drawing.Image bmp = new System.Drawing.Bitmap(img);
                        img.Dispose();
                        pictureBox2.Image = bmp;
                    }

                    //下方显示
                    //设置datagridview的数据源
                    dataGridView1.DataSource = dataTable;
                    //第一行显示为红色
                    dataGridView1.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
