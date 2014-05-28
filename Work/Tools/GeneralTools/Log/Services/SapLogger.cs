﻿using System;
using System.Configuration;
using System.Web;
using GeneralTools.Models;
using GeneralTools.Services;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace GeneralTools.Log.Services
{
    /// <summary>
    /// NLog speziefische Implementierung für das Loggen der SAP Aufrufe
    /// </summary>
    public class SapLogger
    {
        private readonly Logger _log;

        /// <summary>
        /// Logger für SAP Nachrichten
        /// TODO IoC Integration implementiern: Logger soll den nLog Logger über IoC erhalten
        /// </summary>
        public SapLogger()
        {
            _log = LogManager.GetLogger("SapLogger");
        }

        /// <summary>
        /// Hängt die Verbindungsdaten und die Parameter Daten an einem LogEventInfo Objekt.
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <param name="kunnr"></param>
        /// <param name="portalType"></param>
        /// <param name="anmeldeName"></param>
        /// <param name="bapi"></param>
        /// <param name="importParameter"></param>
        /// <param name="importTable"></param>
        /// <param name="dataContext"></param>
        /// <param name="logonContext"></param>
        /// <param name="status"></param>
        /// <param name="dauer"></param>
        /// <param name="exportParamter"></param>
        /// <param name="exportTable"></param>
        /// <returns></returns>
        public void Log(int appID, int userID, int customerID, int kunnr, int portalType, string anmeldeName, string bapi, string importParameter, string importTable, string dataContext, string logonContext, bool status, double dauer, string exportParamter, string exportTable)
        {
            var logEventInfo = new LogEventInfo { Level = LogLevel.Trace, TimeStamp = DateTime.Now };

            var connectionString = ConfigurationManager.AppSettings["Logs"];

            // Falls keine Verbindungsdaten vorhanden sind das Logging unterlassen
            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }

            logEventInfo.Properties["connectionString"] = connectionString;
            logEventInfo.Properties["commandText"] = "insert into SapBapi(AppID, UserID, CustomerID, KUNNR, PortalType, Anmeldename, Bapi,  ImportParameters,  ImportTables,  DataContext,  LogonContext,  Status,  dauer,  ExportParameters,  ExportTables, Browser ) " +
                                                                 "values (@AppID, @UserID, @CustomerID, @KUNNR, @PortalType, @Anmeldename, @Bapi, @ImportParameters, @ImportTables, @DataContext, @LogonContext, @Status, @dauer, @ExportParameters, @ExportTables, @Browser );";


            // 27.02.2014, MJE
            // minimize data overhead
            logEventInfo.Properties["DataContext"] = ""; // dataContext;
            logEventInfo.Properties["LogonContext"] = ""; //logonContext;

            if (HttpContext.Current != null)
            {
                var context = HttpContext.Current.Request.QueryString.ToString();
                logEventInfo.Properties["LogonContext"] = context;

                //
                // log stack context
                //
                try
                {
                    var stackContext = new StackContext();
                    stackContext.Init();
                    logEventInfo.Properties["DataContext"] = XmlService.XmlSerializeToString(stackContext);
                }
                catch {}

                //
                // log user context, like "appID, userID, customerID, kunnr, portalType"
                //
                if (appID == 0 && userID == 0)
                {
                    // try to obtain "appID, userID, customerID, kunnr, portalType"
                    // => .. from Request QueryString
                    var cp = HttpContext.Current.Request["cp"];
                    if (cp.IsNullOrEmpty() && HttpContext.Current.Session["cp"] != null)
                        // => .. or from Session
                        cp = HttpContext.Current.Session["cp"].ToString();

                    if (cp.IsNotNullOrEmpty())
                    {
                        var userContextParams = cp.Split('_');
                        if (userContextParams.Length >= 5)
                        {
                            appID = userContextParams[0].ToInt();
                            userID = userContextParams[1].ToInt();
                            customerID = userContextParams[2].ToInt();
                            kunnr = userContextParams[3].ToInt();
                            portalType = userContextParams[4].ToInt();
                        }
                    }
                }
            }

            logEventInfo.Properties["AppID"] = appID;
            logEventInfo.Properties["UserID"] = userID;
            logEventInfo.Properties["CustomerID"] = customerID;
            logEventInfo.Properties["KUNNR"] = kunnr;
            logEventInfo.Properties["PortalType"] = portalType;

            logEventInfo.Properties["Anmeldename"] = anmeldeName;
            logEventInfo.Properties["Bapi"] = bapi;
            logEventInfo.Properties["ImportParameters"] = importParameter;
            logEventInfo.Properties["ImportTables"] = importTable;
            logEventInfo.Properties["Status"] = status;
            logEventInfo.Properties["Dauer"] = Convert.ToDecimal(dauer);
            logEventInfo.Properties["ExportParameters"] = exportParamter;
            logEventInfo.Properties["ExportTables"] = exportTable;

            if (HttpContext.Current != null)
            {
                logEventInfo.Properties["Browser"] = HttpContext.Current.Request.Browser.Type;    
            }
            else
            {
                logEventInfo.Properties["Browser"] = string.Empty;    
            }

            logEventInfo.Parameters = new object[] {
                new DatabaseParameterInfo("@AppID", Layout.FromString("${event-context:item=AppID}")),
                new DatabaseParameterInfo("@UserID", Layout.FromString("${event-context:item=UserID}")),
                new DatabaseParameterInfo("@CustomerID", Layout.FromString("${event-context:item=CustomerID}")),
                new DatabaseParameterInfo("@KUNNR", Layout.FromString("${event-context:item=KUNNR}")),
                new DatabaseParameterInfo("@PortalType", Layout.FromString("${event-context:item=PortalType}")),
                new DatabaseParameterInfo("@Anmeldename", Layout.FromString("${event-context:item=Anmeldename}")), 
                new DatabaseParameterInfo("@Bapi", Layout.FromString("${event-context:item=Bapi}")),
                new DatabaseParameterInfo("@ImportParameters", Layout.FromString("${event-context:item=ImportParameters}")),
                new DatabaseParameterInfo("@ImportTables", Layout.FromString("${event-context:item=ImportTables}")),
                new DatabaseParameterInfo("@DataContext", Layout.FromString("${event-context:item=DataContext}")),
                new DatabaseParameterInfo("@LogonContext", Layout.FromString("${event-context:item=LogonContext}")),
                new DatabaseParameterInfo("@Status", Layout.FromString("${event-context:item=Status}")),
                new DatabaseParameterInfo("@Dauer", Layout.FromString("${event-context:item=Dauer}")),
                new DatabaseParameterInfo("@ExportParameters", Layout.FromString("${event-context:item=ExportParameters}")),
                new DatabaseParameterInfo("@ExportTables", Layout.FromString("${event-context:item=ExportTables}")),
                new DatabaseParameterInfo("@Browser", Layout.FromString("${event-context:item=Browser}"))
            };

            _log.Log(logEventInfo);
        }
    }
}
