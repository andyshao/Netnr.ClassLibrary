﻿namespace Netnr.Core
{
    /// <summary>
    /// 调用cmd
    /// </summary>
    public class CmdTo
    {
        /// <summary>  
        /// Windows操作系统，执行cmd命令
        /// 多命令请使用批处理命令连接符：  
        /// <![CDATA[  
        /// &:同时执行两个命令  
        /// |:将上一个命令的输出,作为下一个命令的输入  
        /// &&：当&&前的命令成功时,才执行&&后的命令  
        /// ||：当||前的命令失败时,才执行||后的命令
        /// ]]>
        /// </summary>  
        public static string Run(string cmdText, string cmdPath = "cmd.exe")
        {
            if (cmdPath == "cmd.exe")
            {
                cmdPath = System.Environment.SystemDirectory + "\\" + cmdPath;
            }

            string strOutput = "";

            //说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态  
            var cmd = cmdText + " &exit";
            using (var p = new System.Diagnostics.Process())
            {
                p.StartInfo.FileName = cmdPath;
                p.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动  
                p.StartInfo.RedirectStandardInput = true; //接受来自调用程序的输入信息  
                p.StartInfo.RedirectStandardOutput = true; //由调用程序获取输出信息  
                p.StartInfo.RedirectStandardError = true; //重定向标准错误输出  
                p.StartInfo.CreateNoWindow = true; //不显示程序窗口  
                p.Start(); //启动程序  

                //向cmd窗口写入命令  
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;
                strOutput = p.StandardOutput.ReadToEnd();
                p.WaitForExit(); //等待程序执行完退出进程  
                p.Close();
            }

            return strOutput;
        }

#if !NET40
        /// <summary>
        /// Linux操作系统，执行Shell，返回JSON结果
        /// </summary>
        /// <param name="cmd"></param>
        public static string Shell(string cmd)
        {
            var rt = new Shell.NET.Bash().Command(cmd, false);
            return rt.ToJson();
        }
#endif
    }
}