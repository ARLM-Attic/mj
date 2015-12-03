using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using GeneralTools.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;

namespace SapORM.Models
{
	public partial class Z_FIL_EFA_GET_KOSTL
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_FIL_EFA_GET_KOSTL).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_FIL_EFA_GET_KOSTL).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_KOSTL_RECEIVE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KOSTL_RECEIVE", value);
		}

		public static void SetImportParameter_I_KOSTL_SEND(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KOSTL_SEND", value);
		}

		public static string GetExportParameter_E_KOSTL(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_KOSTL").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_KTEXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_KTEXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_LTEXT(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_LTEXT").NotNullOrEmpty().Trim();
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
