﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GeneralTools.Log.Models.MultiPlatform;
using GeneralTools.Models;
using GeneralTools.Services;
using LogMaintenance.Models;
using MySql.Data.MySqlClient;

namespace LogMaintenance.Services
{
    public class BusinessDataCopyService
    {
        private static Action<string> _infoMessageAction;


        #region MaintenanceLogsDb

        public static bool MaintenanceLogsDb(Action<string> infoMessageAction, string appDataFileName)
        {
            _infoMessageAction = infoMessageAction;

            var success = MaintenanceLogsDbForServer("Test", appDataFileName);

            return success;
        }

        private static bool MaintenanceLogsDbForServer(string serverType, string appDataFileName)
        {
            if (serverType.IsNullOrEmpty())
                return false;

            var logsDbContext = CreateLogsDbContext(serverType);

            var sqlMaintenanceTables = XmlService.XmlTryDeserializeFromFile<DbMaintenanceTable[]>(appDataFileName);
            foreach (var sqlMaintenanceTable in sqlMaintenanceTables)
            {
                Alert(string.Format("***  Refreshing content of table '{0}'  ***", sqlMaintenanceTable.DestTableName));

                foreach (var sqlMaintenanceStep in sqlMaintenanceTable.Steps)
                {
                    Alert(sqlMaintenanceTable.PrepareStatement(sqlMaintenanceStep.Description));

                    var multiSqlSteps = new [] { "" };
                    if (sqlMaintenanceStep.IsSqlIndexStatement)
                        multiSqlSteps = sqlMaintenanceTable.GetTableIndexColumnNames();

                    foreach (var multiSqlStep in multiSqlSteps)
                    {
                        try
                        {
                            var sql = sqlMaintenanceTable.PrepareStatement(sqlMaintenanceStep.Sql, multiSqlStep);
                            logsDbContext.Database.ExecuteSqlCommand(sql);
                        }
                        catch (MySqlException e)
                        {
                            if (!sqlMaintenanceStep.IgnoreSqlException)
                            {
                                Alert(string.Format("\r\nA MySql Error has occured:\r\n>> {0} <<\r\nAborting !!!\r\n", e.Message));
                                return false;
                            }

                        }
                    }
                }
            }

            Alert("\r\n***   Ok, all finished ;-)   ***\r\n");
            return true;
        }

        #endregion


        #region CopyToLogsDB

        public static void CopyToLogsDb(Action<string> infoMessageAction)
        {
            _infoMessageAction = infoMessageAction;

            CopyToLogsDbForServer("Dev");
            CopyToLogsDbForServer("Test");
            CopyToLogsDbForServer("Prod");
        }

        private static void CopyToLogsDbForServer(string serverType)
        {
            if (serverType.IsNullOrEmpty())
                return;

            CopyToLogsDb<MpWebUser>(serverType);
            CopyToLogsDb<MpCustomer>(serverType);
            CopyToLogsDb<MpApplicationTranslated>(serverType);

            Alert("");
        }

        private static void CopyToLogsDb<T>(string serverType) where T : class, new()
        {
            var tableName = typeof (T).Name;
            var tableAttribute = typeof (T).GetCustomAttributes(false).OfType<TableAttribute>().FirstOrDefault();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;

            var businessDbContext = CreateBusinessDbContext(serverType);
            var logsDbContext = CreateLogsDbContext(serverType);

            logsDbContext.Database.ExecuteSqlCommand("delete from " + tableName);
            logsDbContext.SaveChanges();

            var businessData = businessDbContext.GetData<T>(tableName);
            if (businessData.None())
                return;

            businessData.ToList().ForEach(m => logsDbContext.AddData(ModelMapping.Copy(m)));
            logsDbContext.SaveChanges();

            Alert(string.Format("{0}-Server: Successfully copied data for '{1}' !", serverType, tableName));
        }

        #endregion


        #region Misc

        private static MultiDbPlatformContext CreateLogsDbContext(string serverType)
        {
            var logsConnectionString = string.Format("Logs{0}", serverType);
            return new MultiDbPlatformContext(logsConnectionString);
        }

        private static MultiDbPlatformContext CreateBusinessDbContext(string serverType)
        {
            var businessConnectionString = string.Format("Source{0}", serverType);
            return new MultiDbPlatformContext(businessConnectionString);
        }

        private static void Alert(string info)
        {
            if (_infoMessageAction != null)
                _infoMessageAction(info);
        }

        #endregion
    }
}
