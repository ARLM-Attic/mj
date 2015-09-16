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
	public partial class Z_WFM_LIST_DOKU_01
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_WFM_LIST_DOKU_01).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_WFM_LIST_DOKU_01).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_DOKUMENTE : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VORG_NR_ABM_AUF { get; set; }

			public string AR_OBJECT { get; set; }

			public string ARCHIV_ID { get; set; }

			public string DATEINAME { get; set; }

			public string OBJECT_ID { get; set; }

			public static GT_DOKUMENTE Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_DOKUMENTE
				{
					VORG_NR_ABM_AUF = (string)row["VORG_NR_ABM_AUF"],
					AR_OBJECT = (string)row["AR_OBJECT"],
					ARCHIV_ID = (string)row["ARCHIV_ID"],
					DATEINAME = (string)row["DATEINAME"],
					OBJECT_ID = (string)row["OBJECT_ID"],

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

			public static IEnumerable<GT_DOKUMENTE> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_DOKUMENTE> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_DOKUMENTE> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_DOKUMENTE).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_DOKUMENTE> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOKUMENTE> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_DOKUMENTE> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOKUMENTE>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_WFM_LIST_DOKU_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOKUMENTE> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOKUMENTE>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOKUMENTE> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOKUMENTE>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOKUMENTE> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_DOKUMENTE>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_WFM_LIST_DOKU_01", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_DOKUMENTE> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_DOKUMENTE>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_WFM_LIST_DOKU_01.GT_DOKUMENTE> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_WFM_LIST_DOKU_01.GT_DOKUMENTE> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}