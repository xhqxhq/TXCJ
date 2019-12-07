using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Utils
{
    class DeviceUtils
    {
        string filePath = System.Environment.CurrentDirectory;
        int len = 0;
        //string filename = "0";
        [DllImport("Dewlt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi)]
        public static extern int GetBmp(string filename, int nType);

        [DllImport("Sdtapi.dll")]
        public  static extern int InitComm(int iPort);
        [DllImport("sdtapi.dll")]
        public static extern bool HIDSelect(int index); //设定当前操作的HID接口iDR210 
        [DllImport("Sdtapi.dll")]
        public static extern int Authenticate();
        [DllImport("sdtapi.dll")]
        public static extern int CardOn(); //判断身份证是否在设备上
        [DllImport("Sdtapi.dll")]

        public static extern int ReadBaseInfos(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                  StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                        StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd);
        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseInfosPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                   StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                       StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory);
        [DllImport("Sdtapi.dll")]
        public static extern int ReadBaseInfosFPPhoto(StringBuilder Name, StringBuilder Gender, StringBuilder Folk,
                                                   StringBuilder BirthDay, StringBuilder Code, StringBuilder Address,
                                                     StringBuilder Agency, StringBuilder ExpireStart, StringBuilder ExpireEnd, StringBuilder directory, StringBuilder pucFPMsg, ref int puiFPMsgLen);
        [DllImport("Sdtapi.dll")]
        public static extern int CloseComm();
        [DllImport("Sdtapi.dll")]
        // private static extern int ReadBaseMsg(StringBuilder pMsg, int len);
        public static extern int ReadBaseMsgPhoto(StringBuilder pMsg, int len, StringBuilder directory);
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder Name = new StringBuilder(31);
            StringBuilder Gender = new StringBuilder(3);
            StringBuilder Folk = new StringBuilder(10);
            StringBuilder BirthDay = new StringBuilder(9);
            StringBuilder Code = new StringBuilder(19);
            StringBuilder Address = new StringBuilder(71);
            StringBuilder Agency = new StringBuilder(31);
            StringBuilder ExpireStart = new StringBuilder(9);
            StringBuilder ExpireEnd = new StringBuilder(9);
            StringBuilder directory = new StringBuilder(100);
            StringBuilder pucFPMsg = new StringBuilder(1024);
            StringBuilder pMsg = new StringBuilder(100);

            //string srcRelativePath,desRelativePath;
            try
            {


                int intOpenRet = InitComm(1001);
                if (intOpenRet != 1)
                {
                    MessageBox.Show("身份证信息：读卡器连接失败 ");
                    return;
                }
                //卡认证
                int intReadRet = Authenticate();
                //label1.Text = "intReadRet:" + intReadRet;
                if (intReadRet != 1)
                {
                    MessageBox.Show("身份证信息未认证 ");
                    // CloseComm();
                    throw new Exception("");
                    //return;
                }

                int intCardOnRet = CardOn();
                if (intCardOnRet != 1)
                {
                    MessageBox.Show("身份证信息：未放身份证或身份证损坏 ");
                    CloseComm();
                    return;
                }

                // timersfz.Stop();
                directory.Append(".\\bmp");



                if (!File.Exists(@filePath + "/bmp/photo.bmp"))
                {
                    int ReadBaseMsgRet = ReadBaseMsgPhoto(pMsg, len, directory);
                    if (ReadBaseMsgRet != 1)
                    {
                        MessageBox.Show("身份证照片信息读取失败");
                        CloseComm();
                        return;
                    }
                    int ReadBaseInfosRet = ReadBaseInfos(Name, Gender, Folk, BirthDay, Code, Address, Agency, ExpireStart, ExpireEnd);
                    if (ReadBaseInfosRet != 1)

                        MessageBox.Show("身份证信息：读身份证失败");
                    //MessageBox.Show("读卡失败");
                    CloseComm();
                    return;
                }

                // timersfz.Stop();

                /* if (File.Exists(@filePath + "/bmp/photo.bmp"))
                 {
                     File.Move(filePath + "/bmp/photo.bmp", filePath + "/bmp/" + filename + ".bmp");
                     //  label1.Text = "ddsa";
                     //CloseComm();
                     //  return;
                 }
                 else { label1.Text = filename; }
                 pictureBox1.Load("bmp\\" + filename + ".bmp");*/
                MessageBox.Show("身份证信息：获取身份证信息成功");
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show(ex.ToString());

                return;
            }

        }

    }
}
