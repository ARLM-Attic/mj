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
	public partial class Z_DPM_READ_STL_MAHNUNGEN_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_DPM_READ_STL_MAHNUNGEN_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_DPM_READ_STL_MAHNUNGEN_01).Name, inputParameterKeys, inputParameterValues);
		}


		public void SetImportParameter_I_CHASSIS_NUM(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_CHASSIS_NUM", value);
		}

		public void SetImportParameter_I_KUNNR_AG(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_AG", value);
		}

		public void SetImportParameter_I_ZZMADAT(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZMADAT", value);
		}

		public void SetImportParameter_I_ZZMAHNS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZMAHNS", value);
		}

		public void SetImportParameter_I_ZZMANSP(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_ZZMANSP", value);
		}

		public void SetImportParameter_I_ZZTMPDT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZTMPDT_BIS", value);
		}

		public void SetImportParameter_I_ZZTMPDT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZTMPDT_VON", value);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string CHASSIS_NUM { get; set; }

			public string IDNRK { get; set; }

			public string MAKTX { get; set; }

			public string VERS_ID { get; set; }

			public string EQUNR { get; set; }

			public string STATUS_AKTUELL { get; set; }

			public string ZZMANSP { get; set; }

			public DateTime? ZZMADAS { get; set; }

			public DateTime? ZZMANSP_DATBI { get; set; }

			public string KONTONR { get; set; }

			public string CIN { get; set; }

			public string PAID { get; set; }

			public string ZVERT_ART { get; set; }

			public string ZZMAHNA { get; set; }

			public DateTime? MAHNDAT { get; set; }

			public string ZZMAHNS { get; set; }

			public DateTime? NEXT_MAHNDAT { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STREET { get; set; }

			public string HOUSE_NUM1 { get; set; }

			public string POST_CODE1 { get; set; }

			public string CITY1 { get; set; }

			public string COUNTRY { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					CHASSIS_NUM = (string)row["CHASSIS_NUM"],
					IDNRK = (string)row["IDNRK"],
					MAKTX = (string)row["MAKTX"],
					VERS_ID = (string)row["VERS_ID"],
					EQUNR = (string)row["EQUNR"],
					STATUS_AKTUELL = (string)row["STATUS_AKTUELL"],
					ZZMANSP = (string)row["ZZMANSP"],
					ZZMADAS = string.IsNullOrEmpty(row["ZZMADAS"].ToString()) ? null : (DateTime?)row["ZZMADAS"],
					ZZMANSP_DATBI = string.IsNullOrEmpty(row["ZZMANSP_DATBI"].ToString()) ? null : (DateTime?)row["ZZMANSP_DATBI"],
					KONTONR = (string)row["KONTONR"],
					CIN = (string)row["CIN"],
					PAID = (string)row["PAID"],
					ZVERT_ART = (string)row["ZVERT_ART"],
					ZZMAHNA = (string)row["ZZMAHNA"],
					MAHNDAT = string.IsNullOrEmpty(row["MAHNDAT"].ToString()) ? null : (DateTime?)row["MAHNDAT"],
					ZZMAHNS = (string)row["ZZMAHNS"],
					NEXT_MAHNDAT = string.IsNullOrEmpty(row["NEXT_MAHNDAT"].ToString()) ? null : (DateTime?)row["NEXT_MAHNDAT"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STREET = (string)row["STREET"],
					HOUSE_NUM1 = (string)row["HOUSE_NUM1"],
					POST_CODE1 = (string)row["POST_CODE1"],
					CITY1 = (string)row["CITY1"],
					COUNTRY = (string)row["COUNTRY"],

					SAPConnection = sapConnection,
					DynSapProxyFactory = dynSapProxyFactory,
				};
				o.OnInitFromSap();
				return o;
			}

			partial void OnInitFromSap();

			partial void OnInitFromExtern();

			public void OnModelMappingApplied()
			{
				OnInitFromExtern();
			}

			public static IEnumerable<GT_OUT> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_OUT> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_OUT> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_OUT).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_OUT> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_OUT> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_DPM_READ_STL_MAHNUNGEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_DPM_READ_STL_MAHNUNGEN_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_OUT> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_OUT>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_DPM_READ_STL_MAHNUNGEN_01.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}
