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
	public partial class Z_ZLD_EXPORT_LS
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_LS).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_EXPORT_LS).Name, inputParameterKeys, inputParameterValues);
		}


		public static void SetImportParameter_I_GRUPPE(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_GRUPPE", value);
		}

		public static void SetImportParameter_I_KREISKZ_BIS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ_BIS", value);
		}

		public static void SetImportParameter_I_KREISKZ_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KREISKZ_VON", value);
		}

		public static void SetImportParameter_I_KUNNR_BIS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_BIS", value);
		}

		public static void SetImportParameter_I_KUNNR_VON(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_KUNNR_VON", value);
		}

		public static void SetImportParameter_I_LS(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_LS", value);
		}

		public static void SetImportParameter_I_VKBUR(ISapDataService sap, string value)
		{
			sap.SetImportParameter("I_VKBUR", value);
		}

		public static void SetImportParameter_I_ZZZLDAT_BIS(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZZLDAT_BIS", value);
		}

		public static void SetImportParameter_I_ZZZLDAT_VON(ISapDataService sap, DateTime? value)
		{
			sap.SetImportParameter("I_ZZZLDAT_VON", value);
		}

		public partial class GT_FILENAME : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VKBUR { get; set; }

			public string KUNNR { get; set; }

			public string FILENAME { get; set; }

			public static GT_FILENAME Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_FILENAME
				{
					VKBUR = (string)row["VKBUR"],
					KUNNR = (string)row["KUNNR"],
					FILENAME = (string)row["FILENAME"],

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

			public static IEnumerable<GT_FILENAME> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_FILENAME> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_FILENAME> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_FILENAME).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_FILENAME> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_FILENAME> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_EXPORT_LS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_EXPORT_LS", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_FILENAME> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_FILENAME>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_EXPORT_LS.GT_FILENAME> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

	}
}