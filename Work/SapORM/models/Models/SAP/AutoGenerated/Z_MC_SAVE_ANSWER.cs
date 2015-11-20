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
	public partial class Z_MC_SAVE_ANSWER
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_MC_SAVE_ANSWER).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_MC_SAVE_ANSWER).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_AN(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_AN", value);
		}

		public void SetImportParameter_I_BD_NR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_BD_NR", value);
		}

		public void SetImportParameter_I_LFDNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LFDNR", value);
		}

		public void SetImportParameter_I_LTXNR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LTXNR", value);
		}

		public void SetImportParameter_I_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VON", value);
		}

		public void SetImportParameter_I_VORGID(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VORGID", value);
		}

		public string GetExportParameter_E_MESSAGE(ISapDataService sap)
		{
			return sap.GetExportParameter<string>("E_MESSAGE");
		}

		public int? GetExportParameter_E_SUBRC(ISapDataService sap)
		{
			return sap.GetExportParameter<int?>("E_SUBRC");
		}
	}

	public static partial class DataTableExtensions
	{
	}
}
