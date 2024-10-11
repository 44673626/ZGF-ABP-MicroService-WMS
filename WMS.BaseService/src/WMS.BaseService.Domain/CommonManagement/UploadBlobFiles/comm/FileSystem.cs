using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;

namespace WMS.BaseService.CommonManagement.UploadBlobFiles.comm
{
    /// <summary>
    /// 公共调用类--操作文件类
    /// </summary>
    public class FileSystem
    {
        /// <summary>
        /// 删除指定路径下的文件
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteFiles(string directoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
        /// <summary>
        /// 文件夹复制
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="aimPath"></param>
        /// <param name="files"></param>
        public static void CopyDir(string srcPath, string aimPath, List<string> files)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    aimPath += Path.DirectorySeparatorChar;
                }
                // 判断目标目录是否存在如果不存在则新建
                if (!Directory.Exists(aimPath))
                {
                    Directory.CreateDirectory(aimPath);
                }
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles（srcPath）；
                string[] fileList = Directory.GetFileSystemEntries(srcPath).Where(p => p.Contains(".xlsx")).ToArray();
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                    {
                        CopyDir(file, aimPath + Path.GetFileName(file), files);
                    }
                    // 否则直接Copy文件
                    else
                    {
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                        File.Delete(file);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        public static void Bytes2File(byte[] buff, string savepath)
        {
            if (File.Exists(savepath))
            {
                File.Delete(savepath);
            }

            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] File2Bytes(string path)
        {
            if (!File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];


            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();


            return buff;
        }

        /// <summary>
        /// 删除文件夹下的所有文件
        /// </summary>
        /// <param name="dirRoot"></param>
        public static void DeleteDirAllFile(string dirRoot)
        {
            DirectoryInfo aDirectoryInfo = new DirectoryInfo(Path.GetDirectoryName(dirRoot));
            FileInfo[] files = aDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (FileInfo f in files)
            {
                File.Delete(f.FullName);
            }
        }

        #region 制作压缩包（单个文件压缩）
        /// <summary>
        /// 制作压缩包（单个文件压缩）
        /// </summary>
        /// <param name="sourceFileName">原文件</param>
        /// <param name="zipFileName">压缩文件</param>
        /// <param name="zipEnum">压缩算法枚举</param>
        /// <returns>压缩成功标志</returns>
        //public static bool ZipFile(string srcFileName, string zipFileName, ZipEnum zipEnum)
        //{
        //    bool flag = true;
        //    try
        //    {
        //        switch (zipEnum)
        //        {
        //            case ZipEnum.BZIP2:

        //                FileStream inStream = File.OpenRead(srcFileName);
        //                FileStream outStream = File.Open(zipFileName, FileMode.Create);

        //                //参数true表示压缩完成后，inStream和outStream连接都释放
        //                BZip2.Compress(inStream, outStream, true, 4096);

        //                inStream.Close();
        //                outStream.Close();


        //                break;
        //            case ZipEnum.GZIP:

        //                FileStream srcFile = File.OpenRead(srcFileName);

        //                GZipOutputStream zipFile = new GZipOutputStream(File.Open(zipFileName, FileMode.Create));

        //                byte[] fileData = new byte[srcFile.Length];
        //                srcFile.Read(fileData, 0, (int)srcFile.Length);
        //                zipFile.Write(fileData, 0, fileData.Length);

        //                srcFile.Close();
        //                zipFile.Close();

        //                break;
        //            default: break;
        //        }
        //    }
        //    catch
        //    {

        //        flag = false;
        //    }
        //    return flag;
        //}
        #endregion

        //    #region 解压缩,引入ICSharpCode.SharpZipLib.Zip

        //    /// 压缩成zip
        //            /// </summary>
        //            /// <param name="filesPath">d:\</param>
        //            /// <param name="zipFilePath">d:\a.zip</param>
        //    public static void CreateZipFile(string folderToZip, string zipedFile)
        //    {
        //        bool result = false;
        //        if (!Directory.Exists(folderToZip))
        //            return;

        //        ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
        //        zipStream.SetLevel(6);
        //        //if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

        //        result = ZipDirectory(folderToZip, zipStream, "");

        //        zipStream.Finish();
        //        zipStream.Close();

        //        return;

        //    }

        //    /// <summary>   
        //    /// 递归压缩文件夹的内部方法   
        //    /// </summary>   
        //    /// <param name="folderToZip">要压缩的文件夹路径</param>   
        //    /// <param name="zipStream">压缩输出流</param>   
        //    /// <param name="parentFolderName">此文件夹的上级文件夹</param>   
        //    /// <returns></returns>   
        //    private static bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        //    {
        //        bool result = true;
        //        string[] folders, files;
        //        ZipEntry ent = null;
        //        FileStream fs = null;
        //        //Crc32 crc = new Crc32();

        //        try
        //        {
        //            ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
        //            zipStream.PutNextEntry(ent);
        //            zipStream.Flush();

        //            files = Directory.GetFiles(folderToZip);
        //            foreach (string file in files)
        //            {
        //                fs = File.OpenRead(file);

        //                byte[] buffer = new byte[fs.Length];
        //                fs.Read(buffer, 0, buffer.Length);
        //                ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
        //                ent.DateTime = DateTime.Now;
        //                ent.Size = fs.Length;

        //                fs.Close();

        //                //crc.Reset();
        //                //crc.Update(buffer);

        //                //ent.Crc = crc.Value;
        //                zipStream.PutNextEntry(ent);
        //                zipStream.Write(buffer, 0, buffer.Length);
        //            }

        //        }
        //        catch
        //        {
        //            result = false;
        //        }
        //        finally
        //        {
        //            if (fs != null)
        //            {
        //                fs.Close();
        //                fs.Dispose();
        //            }
        //            if (ent != null)
        //            {
        //                ent = null;
        //            }
        //            GC.Collect();
        //            GC.Collect(1);
        //        }

        //        folders = Directory.GetDirectories(folderToZip);
        //        foreach (string folder in folders)
        //            if (!ZipDirectory(folder, zipStream, Path.GetFileName(folderToZip)))
        //                return false;

        //        return result;
        //    }


        //    /// <summary>
        //    /// 解压文件
        //    /// </summary>
        //    /// <param name="TargetFile"></param>
        //    /// <param name="fileDir"></param>
        //    /// <param name="msg"></param>
        //    /// <returns>解压文件地址列表</returns>
        //    public static List<string> UnZipFile(string TargetFile, string fileDir, ref string msg)
        //    {
        //        var unzipedFiles = new List<string>();

        //        string rootFile = "";
        //        msg = "";
        //        string unzpiedFilePath;
        //        try
        //        {
        //            //读取压缩文件（zip文件），准备解压缩
        //            ZipInputStream inputstream = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
        //            ZipEntry entry;
        //            string path = fileDir;
        //            //解压出来的文件保存路径
        //            string rootDir = "";
        //            //根目录下的第一个子文件夹的名称
        //            while ((entry = inputstream.GetNextEntry()) != null)
        //            {
        //                rootDir = Path.GetDirectoryName(entry.Name);
        //                //得到根目录下的第一级子文件夹的名称
        //                if (rootDir.IndexOf("\\") >= 0)
        //                {
        //                    rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
        //                }
        //                string dir = Path.GetDirectoryName(entry.Name);
        //                //得到根目录下的第一级子文件夹下的子文件夹名称
        //                string fileName = Path.GetFileName(entry.Name);
        //                //根目录下的文件名称
        //                if (dir != "")
        //                {
        //                    //创建根目录下的子文件夹，不限制级别
        //                    if (!Directory.Exists(fileDir + "\\" + dir))
        //                    {
        //                        path = fileDir + "\\" + dir;
        //                        //在指定的路径创建文件夹
        //                        Directory.CreateDirectory(path);
        //                    }
        //                }
        //                else if (dir == "" && fileName != "")
        //                {
        //                    //根目录下的文件
        //                    path = fileDir;
        //                    rootFile = fileName;
        //                }
        //                else if (dir != "" && fileName != "")
        //                {
        //                    //根目录下的第一级子文件夹下的文件
        //                    if (dir.IndexOf("\\") > 0)
        //                    {
        //                        //指定文件保存路径
        //                        path = fileDir + "\\" + dir;
        //                    }
        //                }
        //                if (dir == rootDir)
        //                {
        //                    //判断是不是需要保存在根目录下的文件
        //                    path = fileDir + "\\" + rootDir;
        //                }

        //                //以下为解压zip文件的基本步骤
        //                //基本思路：遍历压缩文件里的所有文件，创建一个相同的文件
        //                if (fileName != String.Empty)
        //                {
        //                    unzpiedFilePath = Path.Combine(path, fileName);
        //                    FileStream fs = File.Create(unzpiedFilePath);
        //                    int size = 2048;
        //                    byte[] data = new byte[2048];
        //                    while (true)
        //                    {
        //                        size = inputstream.Read(data, 0, data.Length);
        //                        if (size > 0)
        //                        {
        //                            fs.Write(data, 0, size);
        //                        }
        //                        else
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    fs.Close();

        //                    unzipedFiles.Add(unzpiedFilePath);
        //                }
        //            }
        //            inputstream.Close();
        //            msg = "解压成功！";

        //        }
        //        catch (Exception ex)
        //        {
        //            throw new BusinessException(message: "解压失败，原因：" + ex.Message);

        //        }

        //        return unzipedFiles;
        //    }

        //    /// <summary>
        //    /// 解压文件
        //    /// </summary>
        //    /// <param name="TargetFile"></param>
        //    /// <param name="getSavePathFunc"></param>
        //    /// <param name="msg"></param>
        //    /// <returns></returns>
        //    public static List<string> UnZipFile(string TargetFile, Func<string, string> getSavePathFunc, ref string msg)
        //    {
        //        var unzipedFiles = new List<string>();

        //        msg = "";
        //        string unzpiedFilePath;
        //        try
        //        {
        //            //读取压缩文件（zip文件），准备解压缩
        //            ZipInputStream inputstream = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
        //            ZipEntry entry;

        //            //解压出来的文件保存路径
        //            string rootDir = "";
        //            //根目录下的第一个子文件夹的名称
        //            while ((entry = inputstream.GetNextEntry()) != null)
        //            {
        //                rootDir = Path.GetDirectoryName(entry.Name);
        //                //得到根目录下的第一级子文件夹的名称
        //                if (rootDir.IndexOf("\\") >= 0)
        //                {
        //                    rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
        //                }
        //                string dir = Path.GetDirectoryName(entry.Name);
        //                //得到根目录下的第一级子文件夹下的子文件夹名称
        //                string fileName = Path.GetFileName(entry.Name);

        //                //以下为解压zip文件的基本步骤
        //                //基本思路：遍历压缩文件里的所有文件，创建一个相同的文件
        //                if (fileName != String.Empty)
        //                {
        //                    unzpiedFilePath = getSavePathFunc(fileName);

        //                    // 如果解压路径不存在则跳过
        //                    if (unzpiedFilePath == null) continue;

        //                    //// 如果文件已存在则跳过
        //                    //if (File.Exists(unzpiedFilePath)) continue;

        //                    // 如果文件已存在则覆盖
        //                    if (File.Exists(unzpiedFilePath)) File.Delete(unzpiedFilePath);

        //                    if (!Directory.Exists(Path.GetDirectoryName(unzpiedFilePath)))
        //                    {
        //                        Directory.CreateDirectory(Path.GetDirectoryName(unzpiedFilePath));
        //                    }

        //                    FileStream fs = File.Create(unzpiedFilePath);
        //                    int size = 2048;
        //                    byte[] data = new byte[2048];
        //                    while (true)
        //                    {
        //                        size = inputstream.Read(data, 0, data.Length);
        //                        if (size > 0)
        //                        {
        //                            fs.Write(data, 0, size);
        //                        }
        //                        else
        //                        {
        //                            break;
        //                        }
        //                    }
        //                    fs.Close();

        //                    unzipedFiles.Add(unzpiedFilePath);
        //                }
        //            }
        //            inputstream.Close();
        //            msg = "解压成功！";

        //        }
        //        catch (Exception ex)
        //        {
        //            msg = "解压失败，原因：" + ex.Message;
        //        }

        //        return unzipedFiles;
        //    }

        //    /// <summary>
        //    /// 从压缩包读取文件列表
        //    /// </summary>
        //    /// <param name="zipFile"></param>
        //    /// <param name="error"></param>
        //    /// <returns></returns>
        //    public static List<string> GetFilenamesFromZip(string zipFile, out string error)
        //    {
        //        error = null;

        //        var filenames = new List<string>();

        //        ZipInputStream inputstream = null;
        //        try
        //        {
        //            inputstream = new ZipInputStream(File.OpenRead(zipFile.Trim()));
        //            ZipEntry entry;

        //            //根目录下的第一个子文件夹的名称
        //            while ((entry = inputstream.GetNextEntry()) != null)
        //            {
        //                filenames.Add(entry.Name);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            error = ex.Message;
        //        }
        //        finally
        //        {
        //            if (inputstream != null) inputstream.Close();
        //        }
        //        return filenames;
        //    }
        //}
        //#endregion
    }
}
