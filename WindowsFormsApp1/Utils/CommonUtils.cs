using System.IO;
using System.Windows.Forms;

namespace Utils
{
    class CommonUtils
    {

        public static void loadDataToLists() {
            //加载数据
            //Utils.FileUtils.LoadProjectLists();
            //先清空lists
            Utils.FileUtils.lists.Clear();
            string[] fileNameList = Directory.GetFileSystemEntries(FileUtils.ProjectPath, "*_*_*_*");
            // 遍历所有的文件和目录
            if (fileNameList.Length > 0)
            {
                foreach (string name in fileNameList)
                {
                    //从路径中获取文件夹名
                    int num = name.LastIndexOf("\\");
                    string realName = name.Substring(num + 1, name.Length - num - 1);
                    //从文件夹名中获取起始编号
                    int num1 = realName.LastIndexOf("_");
                    int startNumber = int.Parse(realName.Substring(num1 + 1, realName.Length - num1 - 1));
                    //获取从excel中导入数据库的人数
                    AccessHelper achelp = new AccessHelper(name + "\\dbf\\photoSystem.accdb");
                    int importNumber = int.Parse(achelp.GetDataTableFromDB("select count(*) from info").Rows[0][0].ToString());
                    //获取身份证照数
                    int bmpCount = Directory.GetFiles(name + "\\bmp").Length;
                    //获取照片数量
                    int photoCount = Directory.GetFiles(name + "\\photo").Length;

                    WindowsFormsApp1.Model.Project project = new WindowsFormsApp1.Model.Project();
                    project.Name = realName;
                    project.StartNumber = startNumber;
                    project.ImportNumber = importNumber;
                    project.BmpNumber = bmpCount;
                    project.PhotoNumber = photoCount;

                    FileUtils.lists.Add(project);
                }
            }
        }


        ///<summary>
        /// 使DataGridView的列自适应宽度
        /// </summary>
        /// <param name="dgViewFiles"></param>
        public static void AutoSizeColumn(DataGridView dgViewFiles)
        {
            int width = 0;
            //使列自使用宽度
            //对于DataGridView的每一个列都调整
            for (int i = 0; i<dgViewFiles.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                dgViewFiles.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += dgViewFiles.Columns[i].Width;
            }
            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > dgViewFiles.Size.Width)
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                dgViewFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            //冻结某列 从左开始 0，1，2
            dgViewFiles.Columns[1].Frozen = true;
        }
    }
}
