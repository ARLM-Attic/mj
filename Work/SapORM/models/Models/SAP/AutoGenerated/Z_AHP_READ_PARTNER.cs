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
	public partial class Z_AHP_READ_PARTNER
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_AHP_READ_PARTNER).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_AHP_READ_PARTNER).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class GT_OUT : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string KUNNR { get; set; }

			public string PARTART { get; set; }

			public string NAME1 { get; set; }

			public string NAME2 { get; set; }

			public string STRASSE { get; set; }

			public string HAUSNR { get; set; }

			public string PLZNR { get; set; }

			public string ORT { get; set; }

			public string LAND { get; set; }

			public string EMAIL { get; set; }

			public string TELEFON { get; set; }

			public string FAX { get; set; }

			public string BEMERKUNG { get; set; }

			public string GEWERBE { get; set; }

			public string SAVEKDDATEN { get; set; }

			public string REFKUNNR { get; set; }

			public string REFKUNNR2 { get; set; }

			public string EVBNR { get; set; }

			public DateTime? SEPA_STICHTAG { get; set; }

			public static GT_OUT Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_OUT
				{
					KUNNR = (string)row["KUNNR"],
					PARTART = (string)row["PARTART"],
					NAME1 = (string)row["NAME1"],
					NAME2 = (string)row["NAME2"],
					STRASSE = (string)row["STRASSE"],
					HAUSNR = (string)row["HAUSNR"],
					PLZNR = (string)row["PLZNR"],
					ORT = (string)row["ORT"],
					LAND = (string)row["LAND"],
					EMAIL = (string)row["EMAIL"],
					TELEFON = (string)row["TELEFON"],
					FAX = (string)row["FAX"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					GEWERBE = (string)row["GEWERBE"],
					SAVEKDDATEN = (string)row["SAVEKDDATEN"],
					REFKUNNR = (string)row["REFKUNNR"],
					REFKUNNR2 = (string)row["REFKUNNR2"],
					EVBNR = (string)row["EVBNR"],
					SEPA_STICHTAG = (string.IsNullOrEmpty(row["SEPA_STICHTAG"].ToString())) ? null : (DateTime?)row["SEPA_STICHTAG"],

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
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_AHP_READ_PARTNER", inputParameterKeys, inputParameterValues);
				 
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
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_AHP_READ_PARTNER", inputParameterKeys, inputParameterValues);
				 
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

		public static DataTable ToTable(this IEnumerable<Z_AHP_READ_PARTNER.GT_OUT> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_AHP_READ_PARTNER.GT_OUT> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
