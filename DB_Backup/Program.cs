using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Data.SqlClient;

namespace DB_Backup
{
    public class Program
    {
        public static string Data_Backup_Path = Convert.ToString(ConfigurationManager.AppSettings["BackupFilePath"]);
        public static int BackupDateOption = Convert.ToInt16(ConfigurationManager.AppSettings["BackupDateOption"]);
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public static string sServerName = String.Empty;
        public static string sDataBaseName = String.Empty;
        public static string sPassword = String.Empty;
        public static string sUserName = String.Empty;

        public static void Main(string[] args)
        {
            try
            {
                GetConnectionStringProperty();
                BackupDatabase(sDataBaseName, sUserName, sPassword, sServerName, Data_Backup_Path);
                //DeleteBackup();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private static FileInfo[] GetFileList()
        {
            DirectoryInfo d = new DirectoryInfo(@Data_Backup_Path);
            FileInfo[] Files = d.GetFiles("*.bak");
            return Files;
        }

        private static void DeleteBackup()
        {
            try
            {
                FileInfo[] Files = GetFileList();

                int iDateDiff = 0;
                DateTime fileCreatedDate;

                Console.WriteLine("Deleting old backup file " + BackupDateOption.ToString() + "days less than system date : " +
                                    String.Format("{0:dd-MM-yyyy}", DateTime.Now));

                foreach (FileInfo file in Files)
                {
                    fileCreatedDate = File.GetCreationTime(@Data_Backup_Path + file);

                    iDateDiff = GetDateDiffWithTodayDate(fileCreatedDate, DateTime.Now);

                    if (iDateDiff > BackupDateOption)
                    {
                        Process Process = new Process();

                        Process.StartInfo.FileName = "cmd.exe";
                        Process.StartInfo.Arguments = "/C DEL /Q /F \"" + Data_Backup_Path + file + "\"";
                        Process.StartInfo.CreateNoWindow = true;
                        Process.StartInfo.UseShellExecute = false;
                        Process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                        Process.Start();

                        Process.WaitForExit();

                        while (!Process.HasExited)
                        {
                            Console.Write("Error");
                        }
                    }
                }

                Console.WriteLine("Successfully delete old backup file.");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private static int GetDateDiffWithTodayDate(DateTime dBackupDateTime1, DateTime dTodayDateTime2)
        {
            int iDateDiff = 0;

            TimeSpan tDateDiff = dTodayDateTime2.Subtract(dBackupDateTime1);
            iDateDiff = tDateDiff.Days;

            return iDateDiff;
        }

        public static void BackupDatabase(String databaseName, String userName,String password, String serverName, String destinationPath)
        {
            Backup sqlBackup = new Backup();

            string sDestinationPth = destinationPath + databaseName + "_" + String.Format("{0:yyyyMMddhhmmss}", DateTime.Now) + ".bak";

            Console.WriteLine("Starting Backup Process........");
            Console.WriteLine("Backup File Path : " + sDestinationPth);

            try
            {
                sqlBackup.Action = BackupActionType.Database;
                sqlBackup.BackupSetDescription = "ArchiveDataBase:" + DateTime.Now.ToShortDateString();
                sqlBackup.BackupSetName = "Archive";

                sqlBackup.Database = databaseName;

                BackupDeviceItem deviceItem = new BackupDeviceItem(sDestinationPth, DeviceType.File);
                ServerConnection connection = new ServerConnection(serverName, userName, password);
                Server sqlServer = new Server(connection);

                Database db = sqlServer.Databases[databaseName];

                sqlBackup.Initialize = false;
                sqlBackup.Checksum = true;
                sqlBackup.ContinueAfterError = true;

                sqlBackup.Devices.Add(deviceItem);
                sqlBackup.Incremental = false;

                sqlBackup.ExpirationDate = DateTime.Now.AddDays(3);
                sqlBackup.LogTruncation = BackupTruncateLogType.Truncate;

                sqlBackup.FormatMedia = false;

                sqlBackup.SqlBackup(sqlServer);

                Console.WriteLine("Successfully Backup!");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private static void GetConnectionStringProperty()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString);

            sServerName = builder.DataSource;
            sDataBaseName = builder.InitialCatalog;
            sUserName = builder.UserID;
            sPassword = builder.Password;
        }
    }
}
