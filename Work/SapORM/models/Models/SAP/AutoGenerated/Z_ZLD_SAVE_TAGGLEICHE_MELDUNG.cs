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
	public partial class Z_ZLD_SAVE_TAGGLEICHE_MELDUNG
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_SAVE_TAGGLEICHE_MELDUNG).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_SAVE_TAGGLEICHE_MELDUNG).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class IS_TG_MELDUNG : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string VBELN { get; set; }

			public string VKORG { get; set; }

			public string EBELN { get; set; }

			public string FAHRG { get; set; }

			public string BRIEF { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string GEB_AMT { get; set; }

			public string AUSLIEF { get; set; }

			public string ZZSEND2 { get; set; }

			public string ERNAM { get; set; }

			public string SAVE_STATUS { get; set; }

			public static IS_TG_MELDUNG Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new IS_TG_MELDUNG
				{
					VBELN = (string)row["VBELN"],
					VKORG = (string)row["VKORG"],
					EBELN = (string)row["EBELN"],
					FAHRG = (string)row["FAHRG"],
					BRIEF = (string)row["BRIEF"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					ZZKENN = (string)row["ZZKENN"],
					GEB_AMT = (string)row["GEB_AMT"],
					AUSLIEF = (string)row["AUSLIEF"],
					ZZSEND2 = (string)row["ZZSEND2"],
					ERNAM = (string)row["ERNAM"],
					SAVE_STATUS = (string)row["SAVE_STATUS"],

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

			public static IEnumerable<IS_TG_MELDUNG> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<IS_TG_MELDUNG> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<IS_TG_MELDUNG> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(IS_TG_MELDUNG).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<IS_TG_MELDUNG> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<IS_TG_MELDUNG> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<IS_TG_MELDUNG> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IS_TG_MELDUNG>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_SAVE_TAGGLEICHE_MELDUNG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_TG_MELDUNG> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IS_TG_MELDUNG>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_TG_MELDUNG> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IS_TG_MELDUNG>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_TG_MELDUNG> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<IS_TG_MELDUNG>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_SAVE_TAGGLEICHE_MELDUNG", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<IS_TG_MELDUNG> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<IS_TG_MELDUNG>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_SAVE_TAGGLEICHE_MELDUNG.IS_TG_MELDUNG> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_SAVE_TAGGLEICHE_MELDUNG.IS_TG_MELDUNG> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}