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
	public partial class Z_ZLD_STO_GET_ORDER2
	{
		public static void Init(ISapDataService sap)
		{
			sap.Init(typeof(Z_ZLD_STO_GET_ORDER2).Name);
		}

		public static void Init(ISapDataService sap, string inputParameterKeys, params object[] inputParameterValues)
		{
			sap.Init(typeof(Z_ZLD_STO_GET_ORDER2).Name, inputParameterKeys, inputParameterValues);
		}

		public partial class ES_BAK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string ZULBELN { get; set; }

			public string VBELN { get; set; }

			public string VKORG { get; set; }

			public string VKBUR { get; set; }

			public DateTime? VE_ERDAT { get; set; }

			public string VE_ERZEIT { get; set; }

			public string VE_ERNAM { get; set; }

			public DateTime? ERDAT { get; set; }

			public string ERNAM { get; set; }

			public string FLAG { get; set; }

			public string STATUS { get; set; }

			public string BLTYP { get; set; }

			public string VZB_STATUS { get; set; }

			public string VZD_VKBUR { get; set; }

			public DateTime? VZERDAT { get; set; }

			public string BARCODE { get; set; }

			public string KUNNR { get; set; }

			public string KUNNR_NEU { get; set; }

			public string ZZREFNR1 { get; set; }

			public string ZZREFNR2 { get; set; }

			public string ZZREFNR3 { get; set; }

			public string ZZREFNR4 { get; set; }

			public string ZZREFNR5 { get; set; }

			public string ZZREFNR6 { get; set; }

			public string ZZREFNR7 { get; set; }

			public string ZZREFNR8 { get; set; }

			public string ZZREFNR9 { get; set; }

			public string ZZREFNR10 { get; set; }

			public string KREISKZ { get; set; }

			public string KREISBEZ { get; set; }

			public string WUNSCHKENN_JN { get; set; }

			public string RESERVKENN_JN { get; set; }

			public string RESERVKENN { get; set; }

			public string FEINSTAUBAMT { get; set; }

			public DateTime? ZZZLDAT { get; set; }

			public string ZZKENN { get; set; }

			public string WU_KENNZ2 { get; set; }

			public string WU_KENNZ3 { get; set; }

			public string KENNZTYP { get; set; }

			public string KENNZFORM { get; set; }

			public string KENNZANZ { get; set; }

			public string EINKENN_JN { get; set; }

			public string ZUSKENNZ { get; set; }

			public string BEMERKUNG { get; set; }

			public string EC_JN { get; set; }

			public string BAR_JN { get; set; }

			public string RE_JN { get; set; }

			public string POOLNR { get; set; }

			public string ZL_RL_FRBNR_HIN { get; set; }

			public string ZL_RL_FRBNR_ZUR { get; set; }

			public string ZL_LIFNR { get; set; }

			public string KUNDEBAR_JN { get; set; }

			public string ZAHLART { get; set; }

			public string LOEKZ { get; set; }

			public string KSTATUS { get; set; }

			public string BARQ_NR { get; set; }

			public string VK_KUERZEL { get; set; }

			public string KUNDEN_REF { get; set; }

			public string KUNDEN_NOTIZ { get; set; }

			public string KENNZ_VH { get; set; }

			public string ALT_KENNZ { get; set; }

			public string ZBII_ALT_NEU { get; set; }

			public string VH_KENNZ_RES { get; set; }

			public DateTime? STILL_DAT { get; set; }

			public string ERROR_TEXT { get; set; }

			public string PRALI_PRINT { get; set; }

			public string UMBU { get; set; }

			public string ABGESAGT { get; set; }

			public string RUECKBU { get; set; }

			public string ZZEVB { get; set; }

			public string FLIEGER { get; set; }

			public string OBJECT_ID { get; set; }

			public string FAHRZ_ART { get; set; }

			public string MNRESW { get; set; }

			public string SERIE { get; set; }

			public string SAISON_KNZ { get; set; }

			public string SAISON_BEG { get; set; }

			public string SAISON_END { get; set; }

			public string TUEV_AU { get; set; }

			public string KURZZEITVS { get; set; }

			public string ZOLLVERS { get; set; }

			public string ZOLLVERS_DAUER { get; set; }

			public string VORFUEHR { get; set; }

			public DateTime? HALTE_DAUER { get; set; }

			public string LTEXT_NR { get; set; }

			public string BEB_STATUS { get; set; }

			public string AH_DOKNAME { get; set; }

			public string ABM_STATUS { get; set; }

			public string O_G_VERSSCHEIN { get; set; }

			public string MOBUSER { get; set; }

			public string INFO_TEXT { get; set; }

			public string NACHBEARBEITEN { get; set; }

			public string VZ_UMBU_BELNR { get; set; }

			public string ONLINE_VG { get; set; }

			public string UMBU_GEB_FIL { get; set; }

			public string BEGRUENDUNG { get; set; }

			public string STORNOGRUND { get; set; }

			public string ZULBELN_NEU { get; set; }

			public string ZULBELN_ALT { get; set; }

			public string QMNUM { get; set; }

			public string SOFORT_ABR_ERL { get; set; }

			public string SA_PFAD { get; set; }

			public string BRIEFNR { get; set; }

			public string ORDERID { get; set; }

			public string HPPOS { get; set; }

			public static ES_BAK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ES_BAK
				{
					MANDT = (string)row["MANDT"],
					ZULBELN = (string)row["ZULBELN"],
					VBELN = (string)row["VBELN"],
					VKORG = (string)row["VKORG"],
					VKBUR = (string)row["VKBUR"],
					VE_ERDAT = (string.IsNullOrEmpty(row["VE_ERDAT"].ToString())) ? null : (DateTime?)row["VE_ERDAT"],
					VE_ERZEIT = (string)row["VE_ERZEIT"],
					VE_ERNAM = (string)row["VE_ERNAM"],
					ERDAT = (string.IsNullOrEmpty(row["ERDAT"].ToString())) ? null : (DateTime?)row["ERDAT"],
					ERNAM = (string)row["ERNAM"],
					FLAG = (string)row["FLAG"],
					STATUS = (string)row["STATUS"],
					BLTYP = (string)row["BLTYP"],
					VZB_STATUS = (string)row["VZB_STATUS"],
					VZD_VKBUR = (string)row["VZD_VKBUR"],
					VZERDAT = (string.IsNullOrEmpty(row["VZERDAT"].ToString())) ? null : (DateTime?)row["VZERDAT"],
					BARCODE = (string)row["BARCODE"],
					KUNNR = (string)row["KUNNR"],
					KUNNR_NEU = (string)row["KUNNR_NEU"],
					ZZREFNR1 = (string)row["ZZREFNR1"],
					ZZREFNR2 = (string)row["ZZREFNR2"],
					ZZREFNR3 = (string)row["ZZREFNR3"],
					ZZREFNR4 = (string)row["ZZREFNR4"],
					ZZREFNR5 = (string)row["ZZREFNR5"],
					ZZREFNR6 = (string)row["ZZREFNR6"],
					ZZREFNR7 = (string)row["ZZREFNR7"],
					ZZREFNR8 = (string)row["ZZREFNR8"],
					ZZREFNR9 = (string)row["ZZREFNR9"],
					ZZREFNR10 = (string)row["ZZREFNR10"],
					KREISKZ = (string)row["KREISKZ"],
					KREISBEZ = (string)row["KREISBEZ"],
					WUNSCHKENN_JN = (string)row["WUNSCHKENN_JN"],
					RESERVKENN_JN = (string)row["RESERVKENN_JN"],
					RESERVKENN = (string)row["RESERVKENN"],
					FEINSTAUBAMT = (string)row["FEINSTAUBAMT"],
					ZZZLDAT = (string.IsNullOrEmpty(row["ZZZLDAT"].ToString())) ? null : (DateTime?)row["ZZZLDAT"],
					ZZKENN = (string)row["ZZKENN"],
					WU_KENNZ2 = (string)row["WU_KENNZ2"],
					WU_KENNZ3 = (string)row["WU_KENNZ3"],
					KENNZTYP = (string)row["KENNZTYP"],
					KENNZFORM = (string)row["KENNZFORM"],
					KENNZANZ = (string)row["KENNZANZ"],
					EINKENN_JN = (string)row["EINKENN_JN"],
					ZUSKENNZ = (string)row["ZUSKENNZ"],
					BEMERKUNG = (string)row["BEMERKUNG"],
					EC_JN = (string)row["EC_JN"],
					BAR_JN = (string)row["BAR_JN"],
					RE_JN = (string)row["RE_JN"],
					POOLNR = (string)row["POOLNR"],
					ZL_RL_FRBNR_HIN = (string)row["ZL_RL_FRBNR_HIN"],
					ZL_RL_FRBNR_ZUR = (string)row["ZL_RL_FRBNR_ZUR"],
					ZL_LIFNR = (string)row["ZL_LIFNR"],
					KUNDEBAR_JN = (string)row["KUNDEBAR_JN"],
					ZAHLART = (string)row["ZAHLART"],
					LOEKZ = (string)row["LOEKZ"],
					KSTATUS = (string)row["KSTATUS"],
					BARQ_NR = (string)row["BARQ_NR"],
					VK_KUERZEL = (string)row["VK_KUERZEL"],
					KUNDEN_REF = (string)row["KUNDEN_REF"],
					KUNDEN_NOTIZ = (string)row["KUNDEN_NOTIZ"],
					KENNZ_VH = (string)row["KENNZ_VH"],
					ALT_KENNZ = (string)row["ALT_KENNZ"],
					ZBII_ALT_NEU = (string)row["ZBII_ALT_NEU"],
					VH_KENNZ_RES = (string)row["VH_KENNZ_RES"],
					STILL_DAT = (string.IsNullOrEmpty(row["STILL_DAT"].ToString())) ? null : (DateTime?)row["STILL_DAT"],
					ERROR_TEXT = (string)row["ERROR_TEXT"],
					PRALI_PRINT = (string)row["PRALI_PRINT"],
					UMBU = (string)row["UMBU"],
					ABGESAGT = (string)row["ABGESAGT"],
					RUECKBU = (string)row["RUECKBU"],
					ZZEVB = (string)row["ZZEVB"],
					FLIEGER = (string)row["FLIEGER"],
					OBJECT_ID = (string)row["OBJECT_ID"],
					FAHRZ_ART = (string)row["FAHRZ_ART"],
					MNRESW = (string)row["MNRESW"],
					SERIE = (string)row["SERIE"],
					SAISON_KNZ = (string)row["SAISON_KNZ"],
					SAISON_BEG = (string)row["SAISON_BEG"],
					SAISON_END = (string)row["SAISON_END"],
					TUEV_AU = (string)row["TUEV_AU"],
					KURZZEITVS = (string)row["KURZZEITVS"],
					ZOLLVERS = (string)row["ZOLLVERS"],
					ZOLLVERS_DAUER = (string)row["ZOLLVERS_DAUER"],
					VORFUEHR = (string)row["VORFUEHR"],
					HALTE_DAUER = (string.IsNullOrEmpty(row["HALTE_DAUER"].ToString())) ? null : (DateTime?)row["HALTE_DAUER"],
					LTEXT_NR = (string)row["LTEXT_NR"],
					BEB_STATUS = (string)row["BEB_STATUS"],
					AH_DOKNAME = (string)row["AH_DOKNAME"],
					ABM_STATUS = (string)row["ABM_STATUS"],
					O_G_VERSSCHEIN = (string)row["O_G_VERSSCHEIN"],
					MOBUSER = (string)row["MOBUSER"],
					INFO_TEXT = (string)row["INFO_TEXT"],
					NACHBEARBEITEN = (string)row["NACHBEARBEITEN"],
					VZ_UMBU_BELNR = (string)row["VZ_UMBU_BELNR"],
					ONLINE_VG = (string)row["ONLINE_VG"],
					UMBU_GEB_FIL = (string)row["UMBU_GEB_FIL"],
					BEGRUENDUNG = (string)row["BEGRUENDUNG"],
					STORNOGRUND = (string)row["STORNOGRUND"],
					ZULBELN_NEU = (string)row["ZULBELN_NEU"],
					ZULBELN_ALT = (string)row["ZULBELN_ALT"],
					QMNUM = (string)row["QMNUM"],
					SOFORT_ABR_ERL = (string)row["SOFORT_ABR_ERL"],
					SA_PFAD = (string)row["SA_PFAD"],
					BRIEFNR = (string)row["BRIEFNR"],
					ORDERID = (string)row["ORDERID"],
					HPPOS = (string)row["HPPOS"],

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

			public static IEnumerable<ES_BAK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ES_BAK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ES_BAK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ES_BAK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ES_BAK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ES_BAK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ES_BAK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BAK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_BAK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BAK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_BAK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BAK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_BAK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BAK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_BAK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class ES_BANK : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string MANDT { get; set; }

			public string ZULBELN { get; set; }

			public string BANKL { get; set; }

			public string BANKN { get; set; }

			public string EBPP_ACCNAME { get; set; }

			public string KOINH { get; set; }

			public string EINZ_JN { get; set; }

			public string RECH_JN { get; set; }

			public string SWIFT { get; set; }

			public string IBAN { get; set; }

			public string LOEKZ { get; set; }

			public static ES_BANK Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new ES_BANK
				{
					MANDT = (string)row["MANDT"],
					ZULBELN = (string)row["ZULBELN"],
					BANKL = (string)row["BANKL"],
					BANKN = (string)row["BANKN"],
					EBPP_ACCNAME = (string)row["EBPP_ACCNAME"],
					KOINH = (string)row["KOINH"],
					EINZ_JN = (string)row["EINZ_JN"],
					RECH_JN = (string)row["RECH_JN"],
					SWIFT = (string)row["SWIFT"],
					IBAN = (string)row["IBAN"],
					LOEKZ = (string)row["LOEKZ"],

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

			public static IEnumerable<ES_BANK> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<ES_BANK> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<ES_BANK> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(ES_BANK).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<ES_BANK> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<ES_BANK> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<ES_BANK> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_BANK>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BANK> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_BANK>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BANK> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_BANK>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BANK> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<ES_BANK>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<ES_BANK> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<ES_BANK>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_ADRS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string PARVW { get; set; }

			public string KUNNR { get; set; }

			public string LI_NAME1 { get; set; }

			public string LI_NAME2 { get; set; }

			public string LI_PLZ { get; set; }

			public string LI_CITY1 { get; set; }

			public string LI_STREET { get; set; }

			public string LOEKZ { get; set; }

			public string BEMERKUNG { get; set; }

			public static GT_ADRS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_ADRS
				{
					ZULBELN = (string)row["ZULBELN"],
					PARVW = (string)row["PARVW"],
					KUNNR = (string)row["KUNNR"],
					LI_NAME1 = (string)row["LI_NAME1"],
					LI_NAME2 = (string)row["LI_NAME2"],
					LI_PLZ = (string)row["LI_PLZ"],
					LI_CITY1 = (string)row["LI_CITY1"],
					LI_STREET = (string)row["LI_STREET"],
					LOEKZ = (string)row["LOEKZ"],
					BEMERKUNG = (string)row["BEMERKUNG"],

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

			public static IEnumerable<GT_ADRS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_ADRS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_ADRS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_ADRS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_ADRS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_ADRS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_ADRS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_ADRS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}

		public partial class GT_POS : IModelMappingApplied
		{
			[SapIgnore]
			[ScriptIgnore]
			public ISapConnection SAPConnection { get; set; }

			[SapIgnore]
			[ScriptIgnore]
			public IDynSapProxyFactory DynSapProxyFactory { get; set; }

			public string ZULBELN { get; set; }

			public string ZULPOSNR { get; set; }

			public string UEPOS { get; set; }

			public string LOEKZ { get; set; }

			public decimal? MENGE { get; set; }

			public string MATNR { get; set; }

			public string MAKTX { get; set; }

			public decimal? PREIS { get; set; }

			public decimal? GEB_AMT { get; set; }

			public decimal? GEB_AMT_ADD { get; set; }

			public string WEBMTART { get; set; }

			public string SD_REL { get; set; }

			public string NULLPREIS_OK { get; set; }

			public string GBPAK { get; set; }

			public decimal? UPREIS { get; set; }

			public decimal? DIFF { get; set; }

			public string KONDTAB { get; set; }

			public string KSCHL { get; set; }

			public DateTime? CALCDAT { get; set; }

			public static GT_POS Create(DataRow row, ISapConnection sapConnection = null, IDynSapProxyFactory dynSapProxyFactory = null)
			{
				var o = new GT_POS
				{
					ZULBELN = (string)row["ZULBELN"],
					ZULPOSNR = (string)row["ZULPOSNR"],
					UEPOS = (string)row["UEPOS"],
					LOEKZ = (string)row["LOEKZ"],
					MENGE = (decimal?)row["MENGE"],
					MATNR = (string)row["MATNR"],
					MAKTX = (string)row["MAKTX"],
					PREIS = (decimal?)row["PREIS"],
					GEB_AMT = (decimal?)row["GEB_AMT"],
					GEB_AMT_ADD = (decimal?)row["GEB_AMT_ADD"],
					WEBMTART = (string)row["WEBMTART"],
					SD_REL = (string)row["SD_REL"],
					NULLPREIS_OK = (string)row["NULLPREIS_OK"],
					GBPAK = (string)row["GBPAK"],
					UPREIS = (decimal?)row["UPREIS"],
					DIFF = (decimal?)row["DIFF"],
					KONDTAB = (string)row["KONDTAB"],
					KSCHL = (string)row["KSCHL"],
					CALCDAT = (string.IsNullOrEmpty(row["CALCDAT"].ToString())) ? null : (DateTime?)row["CALCDAT"],

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

			public static IEnumerable<GT_POS> Select(DataTable dt, ISapConnection sapConnection = null)
			{
				return dt.AsEnumerable().Select(r => Create(r, sapConnection));
			}

			public static List<GT_POS> ToList(DataTable dt, ISapConnection sapConnection = null)
			{
				return Select(dt, sapConnection).ToListOrEmptyList();
			}

			public static IEnumerable<GT_POS> Select(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				var tbl = dts.FirstOrDefault(t => t.TableName.ToLower() == typeof(GT_POS).Name.ToLower());
				if (tbl == null)
					return null;

				return Select(tbl, sapConnection);
			}

			public static List<GT_POS> ToList(IEnumerable<DataTable> dts, ISapConnection sapConnection = null)
			{
				return Select(dts, sapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> ToList(ISapDataService sapDataService)
			{
				return ToList(sapDataService.GetExportTables(), sapDataService.SapConnection);
			}

			public static List<GT_POS> GetExportListWithInitExecute(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithInitExecute("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetExportListWithExecute(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTablesWithExecute();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetExportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetExportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetImportListWithInit(ISapDataService sapDataService, string inputParameterKeys = null, params object[] inputParameterValues)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTablesWithInit("Z_ZLD_STO_GET_ORDER2", inputParameterKeys, inputParameterValues);
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}

			public static List<GT_POS> GetImportList(ISapDataService sapDataService)
			{
				if (sapDataService == null) 
					return new List<GT_POS>();
				 
				var dts = sapDataService.GetImportTables();
				 
				return Select(dts, sapDataService.SapConnection).ToListOrEmptyList();
			}
		}
	}

	public static partial class DataTableExtensions
	{

		public static DataTable ToTable(this IEnumerable<Z_ZLD_STO_GET_ORDER2.ES_BAK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_STO_GET_ORDER2.ES_BAK> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_STO_GET_ORDER2.ES_BANK> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_STO_GET_ORDER2.ES_BANK> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_STO_GET_ORDER2.GT_ADRS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_STO_GET_ORDER2.GT_ADRS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}


		public static DataTable ToTable(this IEnumerable<Z_ZLD_STO_GET_ORDER2.GT_POS> list)
		{
			return SapDataServiceExtensions.ToTable(list);
		}

		public static void Apply(this IEnumerable<Z_ZLD_STO_GET_ORDER2.GT_POS> list, DataTable dtDst)
		{
			SapDataServiceExtensions.Apply(list, dtDst);
		}

	}
}
