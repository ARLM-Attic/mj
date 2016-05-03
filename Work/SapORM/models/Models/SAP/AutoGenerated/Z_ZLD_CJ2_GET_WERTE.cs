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
	public partial class Z_ZLD_CJ2_GET_WERTE
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_WERTE).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_CJ2_GET_WERTE).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_BEG_DATE(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_BEG_DATE", value);
		}

		public static void SetImportParameter_I_END_DATE(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_END_DATE", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static decimal? GetExportParameter_E_BEG_SALDO(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("E_BEG_SALDO");
		}

		public static decimal? GetExportParameter_E_END_SALDO(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("E_END_SALDO");
		}

		public static string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE").NotNullOrEmpty().Trim();
		}

		public static int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}

		public static decimal? GetExportParameter_E_SUM_AUS(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("E_SUM_AUS");
		}

		public static decimal? GetExportParameter_E_SUM_EIN(ISapDataService sap)
		{
			return sap.GetExportParameter<decimal?>("E_SUM_EIN");
		}

		public static string GetExportParameter_E_WAERS(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_WAERS").NotNullOrEmpty().Trim();
		}
	}

	public static partial class DataTableExtensions
	{
	}
}