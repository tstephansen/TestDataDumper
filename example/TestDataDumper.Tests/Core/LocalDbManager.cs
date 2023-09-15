using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace TestDataDumper.Tests.Core;

public static class LocalDbManager
{
    /// <summary>
    ///     Restores a database with the specified name from the backup file.
    /// </summary>
    /// <remarks>
    ///     The mac version of this is customized to work for me because I have to use Docker for SQL Server on my mac.
    ///     If you are using a mac you will need to customize the paths based on your own environment. The Windows
    ///     version just uses the default MSSQLLocalDb instance so customization shouldn't be required. 
    /// </remarks>
    /// <param name="databaseName">The database name.</param>
    /// <param name="dataFolderPath">The path where the database will be restore to.</param>
    /// <param name="backupFileName">The name of the database's .bak file.</param>
    /// <returns>Returns <c>true</c> if the database is restored successfully.</returns>
    public static bool RestoreDatabase(string databaseName, string dataFolderPath, string backupFileName)
    {
        string databaseConnectionString;
#if !MAC
        // Create the TestDbs folder where the database will be restored to.
        if (!Directory.Exists(dataFolderPath))
            Directory.CreateDirectory(dataFolderPath);
        var dataFilePath = Path.Combine(dataFolderPath, $"{databaseName}.mdf");
        var logFilePath = Path.Combine(dataFolderPath, $"{databaseName}.ldf");
        var backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, backupFileName);
        const string connectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True;";
        if (File.Exists(dataFilePath))
            return true;
#else
        var dataFilePath = Path.Combine(dataFolderPath, $"{databaseName}.mdf");
        var logFilePath = Path.Combine(dataFolderPath, $"{databaseName}.ldf");
        var origBackupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, backupFileName);
        var copyPath = Path.Combine("/Users/Shared/Docker/sqlserver/backup", backupFileName);
        var writeBackupToDisk = true;
        if (File.Exists(copyPath))
        {
            var backupHash = GetFileHash(origBackupPath);
            var existingHash = GetFileHash(copyPath);
            if (backupHash == existingHash)
                writeBackupToDisk = false;
        }

        if (writeBackupToDisk)
            File.Copy(origBackupPath, copyPath, true);
        var backupPath = Path.Combine("/var/opt/mssql/backup/", backupFileName);
        const string connectionString =
            "Server=localhost,14333;Database=master;Trusted_Connection=False;user id=admin;password=Password123;MultipleActiveResultSets=false;Encrypt=False;TrustServerCertificate=True;";
        try
        {
            using var sqlConnection = new SqlConnection(connectionString);
            var serverConnection = new ServerConnection(sqlConnection);
            var server = new Server(serverConnection);
            if (server.Databases.Contains(databaseName))
                server.KillDatabase(databaseName);
            serverConnection.Disconnect();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message.Trim());
        }
#endif
        return RestoreDatabase(connectionString, databaseName, backupPath, dataFilePath, logFilePath);
    }

    private static bool RestoreDatabase(string connectionString, string databaseName, string backupPath,
        string dataFilePath, string logFilePath)
    {
        try
        {
            using var sqlConnection = new SqlConnection(connectionString);
            var serverConnection = new ServerConnection(sqlConnection);
            var server = new Server(serverConnection);
            var restore = new Restore { Database = databaseName, NoRecovery = false, ReplaceDatabase = true };
            restore.Devices.AddDevice(backupPath, DeviceType.File);
            var logicalDataFileName = restore.ReadFileList(server).Rows[0][0].ToString();
            var logicalLogFileName = restore.ReadFileList(server).Rows[1][0].ToString();
            restore.RelocateFiles.Add(
                new RelocateFile { LogicalFileName = logicalDataFileName, PhysicalFileName = dataFilePath });
            restore.RelocateFiles.Add(new RelocateFile
            {
                LogicalFileName = logicalLogFileName, PhysicalFileName = logFilePath
            });
            restore.SqlRestore(server);
            serverConnection.Disconnect();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message.Trim());
            return false;
        }

        return true;
    }

    private static string GetFileHash(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}