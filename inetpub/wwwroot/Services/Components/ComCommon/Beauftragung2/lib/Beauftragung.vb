Option Explicit On
Option Strict On

Imports CKG.Base.Business
Imports CKG.Base.Common
Imports System.Security.Cryptography
Imports System.IO
Imports CKG.Base.Kernel.Security

Namespace Beauftragung2

    Public Enum HalterErfOptionen
        Ja
        Nein
        JaOhneGeburtsort
        JaOhneGeburtsdatum
        JaOhneGeburtsortUndGeburtsdatum
        JaNurName1
    End Enum

    Public Class Beauftragung2
        Inherits DatenimportBase

#Region "Declarations"

        Private mVerkaufsorganisation As String
        Private mVerkaufsbuero As String
        Private mGruppe As String

        Private mKunden As DataTable
        Private mGrosskunden As DataTable
        Private mDienstleistungen As DataTable
        Private mZusatzdienstleistungen As DataTable
        Private mKreise As DataTable
        Private mPrueforganisationen As DataTable
        Private mOrteZurPlz As DataTable
        Private mAutohausVorgangListe As DataTable
        Private mFarben As DataTable
        Private mVersicherungsunternehmen As DataTable

        Private mKunnr As String
        Private mStVANr As String
        Private mStVANrAlt As String
        Private mMaterialnummer As String
        Private mMaterialnummerAlt As String
        Private mGrosskunde As String
        Private mGrosskundennr As String
        Private mHalterAnrede As String
        Private mHalterReferenz As String
        Private mBestellnummer As String
        Private mVerkKuerz As String
        Private mKiReferenz As String
        Private mNotiz As String

        Private mGrunddatenVisible As Boolean
        Private mFahrzeugdatenVisible As Boolean
        Private mDienstleistungenVisible As Boolean
        Private mZusatzdienstleistungenVisible As Boolean
        Private mZusammenfassungVisible As Boolean
        Private mNpaVisible As Boolean

        Private mBankdatenNeeded As Char = "N"c
        Private mSEPA As Boolean
        Private mEvBNeeded As Char = "N"c
        Private mNaechsteHUNeeded As Char
        Private mGutachtenNeeded As Char
        Private mAltKennzeichenNeeded As Char

        Private mTypdatenMessage As String
        Private mHaltername1 As String
        Private mHaltername2 As String
        Private mName As String
        Private mGeburtsname As String
        Private mGeburtstag As String
        Private mGeburtsort As String
        Private mHalterStrasse As String
        Private mHalterHausnr As String
        Private mHalterHausnrZusatz As String
        Private mHalterPLZ As String
        Private mHalterOrt As String

        Private mHersteller As String
        Private mTyp As String
        Private mVarianteVersion As String
        Private mTypPruef As String
        Private mFahrgestellnummer As String
        Private mFahrgestellnummerPruef As String
        Private mEVB As String
        Private mZulassungsdatum As String
        Private mKennzeichen As String
        Private mBemerkung As String
        Private mEinzelkennzeichen As Boolean
        Private mKrad As Boolean
        Private mKennzeichenTyp As String
        Private mWunschkennzeichen As Boolean
        Private mReserviert As Boolean
        Private mReservierungsnr As String
        Private mStatustext As String
        Private mBriefnummer As String
        Private mBLZ As String
        Private mKontonr As String
        Private mIBAN As String
        Private mSWIFT As String
        Private mEinzug As Boolean
        Private mHe As String
        Private mFr As String
        Private mFi As String
        Private mNPaUsed As Boolean = False
        Private mErrText As String
        Private mSapId As String
        Private mHalterNeeded As HalterErfOptionen
        Private mHalterMerken As Boolean
        Private mTypDatenNeeded As Char
        Private mNaechsteHU As String
        Private mArtGenehmigung As String
        Private mPrueforganisation As String
        Private mGutachtenNummer As String
        Private mAltKennzeichen As String
        Private mAusdruckNeeded As Char = "J"c
        Private mBarcodeNeeded As Char = "J"c
        Private mReferenzCode As String
        Private mFahrzeugdatenNeeded As String
        Private mFarbeNeeded As String
        Private mZB1Needed As String
        Private mVersicherungsnummerNeeded As String
        Private mSepaDatumNeeded As String

        Private mSelKundennr As String
        Private mSelKennzeichen As String
        Private mSelReferenz As String
        Private mSelLoeschkennzeichen As String
        Private mSelZuldatVon As String
        Private mSelZuldatBis As String
        Private mAgeConfirmationRequired As Boolean = False
        Private mVinConfirmationRequired As Boolean = False

        Private mAltkennzeichenSpeichern As Boolean = False
        Private mArtGenehmigungSpeichern As Boolean = False
        Private mPrueforganisationSpeichern As Boolean = False
        Private mGutachtenNrSpeichern As Boolean = False

        Private mAutohausvorgang As Boolean = False
        Private mDadVorgang As Boolean = False

        Private mFahrzeugklasse As String
        Private mAufbauArt As String
        Private mFarbe As String
        Private mZB1Nummer As String

        Private mVersicherungsnummer As String
        Private mSepaDatum As String

        Private mFormular As Byte()

#End Region

#Region " Properties"

        Public ReadOnly Property Prueforganisationen() As DataTable
            Get
                Return mPrueforganisationen
            End Get
        End Property

        Public ReadOnly Property Farben() As DataTable
            Get
                Return mFarben
            End Get
        End Property

        Public Property EvBNeeded() As Char
            Get
                Return mEvBNeeded
            End Get
            Set(ByVal value As Char)
                mEvBNeeded = value
            End Set
        End Property

        Public Property GutachtenNeeded() As Char
            Get
                Return mGutachtenNeeded
            End Get
            Set(ByVal value As Char)
                mGutachtenNeeded = value
            End Set
        End Property

        Public Property NaechsteHUNeeded() As Char
            Get
                Return mNaechsteHUNeeded
            End Get
            Set(ByVal value As Char)
                mNaechsteHUNeeded = value
            End Set
        End Property

        Public Property BankdatenNeeded() As Char
            Get
                Return mBankdatenNeeded
            End Get
            Set(ByVal value As Char)
                mBankdatenNeeded = value
            End Set
        End Property

        Public Property SEPA() As Boolean
            Get
                Return mSEPA
            End Get
            Set(ByVal value As Boolean)
                mSEPA = value
            End Set
        End Property

        Public Property ReferenzCode() As String
            Get
                Return mReferenzCode
            End Get
            Set(value As String)
                mReferenzCode = value
            End Set
        End Property

        Public Property NaechsteHU() As String
            Get
                Return mNaechsteHU
            End Get
            Set(ByVal value As String)
                mNaechsteHU = value
            End Set
        End Property

        Public Property ArtGenehmigung() As String
            Get
                Return mArtGenehmigung
            End Get
            Set(ByVal value As String)
                mArtGenehmigung = value
            End Set
        End Property

        Public Property Prueforganisation() As String
            Get
                Return mPrueforganisation
            End Get
            Set(ByVal value As String)
                mPrueforganisation = value
            End Set
        End Property

        Public Property GutachtenNummer() As String
            Get
                Return mGutachtenNummer
            End Get
            Set(ByVal value As String)
                mGutachtenNummer = value
            End Set
        End Property

        Public Property Verkaufsorganisation() As String
            Get
                Return mVerkaufsorganisation
            End Get
            Set(ByVal value As System.String)
                mVerkaufsorganisation = value
            End Set
        End Property

        Public Property Verkaufsbuero() As String
            Get
                Return mVerkaufsbuero
            End Get
            Set(ByVal value As System.String)
                mVerkaufsbuero = value
            End Set
        End Property

        Public Property Gruppe() As String
            Get
                Return mGruppe
            End Get
            Set(ByVal value As System.String)
                mGruppe = value
            End Set
        End Property

        Public Property TypdatenMessage() As String
            Get
                Return mTypdatenMessage
            End Get
            Set(ByVal value As System.String)
                mTypdatenMessage = value
            End Set
        End Property

        Public Property Kunden() As DataTable
            Get
                Return mKunden
            End Get
            Set(ByVal value As DataTable)
                mKunden = value
            End Set
        End Property

        Public Property Grosskunden() As DataTable
            Get
                Return mGrosskunden
            End Get
            Set(ByVal value As DataTable)
                mGrosskunden = value
            End Set
        End Property

        Public Property Dienstleistungen() As DataTable
            Get
                Return mDienstleistungen
            End Get
            Set(ByVal value As DataTable)
                mDienstleistungen = value
            End Set
        End Property

        Public Property Zusatzdienstleistungen() As DataTable
            Get
                Return mZusatzdienstleistungen
            End Get
            Set(ByVal value As DataTable)
                mZusatzdienstleistungen = value
            End Set
        End Property

        Public Property Kreise() As DataTable
            Get
                Return mKreise
            End Get
            Set(ByVal value As DataTable)
                mKreise = value
            End Set
        End Property

        Public Property Kundennr() As String
            Get
                Return mKunnr
            End Get
            Set(ByVal value As String)
                mKunnr = value
            End Set
        End Property

        Public Property Grosskunde() As String
            Get
                Return mGrosskunde
            End Get
            Set(ByVal value As String)
                mGrosskunde = value
            End Set
        End Property

        Public Property Grosskundennr() As String
            Get
                Return mGrosskundennr
            End Get
            Set(ByVal value As String)
                mGrosskundennr = value
            End Set
        End Property

        Public Property HalterAnrede() As String
            Get
                Return mHalterAnrede
            End Get
            Set(ByVal value As String)
                mHalterAnrede = value
            End Set
        End Property

        Public Property Haltername1() As String
            Get
                Return mHaltername1
            End Get
            Set(ByVal value As String)
                mHaltername1 = value
            End Set
        End Property

        Public Property Haltername2() As String
            Get
                Return mHaltername2
            End Get
            Set(ByVal value As String)
                mHaltername2 = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return mName
            End Get
            Set(ByVal value As String)
                mName = value
            End Set
        End Property

        Public Property Geburtsname() As String
            Get
                Return mGeburtsname
            End Get
            Set(ByVal value As String)
                mGeburtsname = value
            End Set
        End Property

        Public Property Geburtstag() As String
            Get
                Return mGeburtstag
            End Get
            Set(ByVal value As String)
                mGeburtstag = value
            End Set
        End Property

        Public Property Geburtsort() As String
            Get
                Return mGeburtsort
            End Get
            Set(ByVal value As String)
                mGeburtsort = value
            End Set
        End Property

        Public Property HalterStrasse() As String
            Get
                Return mHalterStrasse
            End Get
            Set(ByVal value As String)
                mHalterStrasse = value
            End Set
        End Property

        Public Property HalterHausnr() As String
            Get
                Return mHalterHausnr
            End Get
            Set(ByVal value As String)
                mHalterHausnr = value
            End Set
        End Property

        Public Property HalterHausnrZusatz() As String
            Get
                Return mHalterHausnrZusatz
            End Get
            Set(ByVal value As String)
                mHalterHausnrZusatz = value
            End Set
        End Property

        Public Property HalterPLZ() As String
            Get
                Return mHalterPLZ
            End Get
            Set(ByVal value As String)
                mHalterPLZ = value
            End Set
        End Property

        Public Property HalterOrt() As String
            Get
                Return mHalterOrt
            End Get
            Set(ByVal value As String)
                mHalterOrt = value
            End Set
        End Property

        Public Property VerkKuerz() As String
            Get
                Return mVerkKuerz
            End Get
            Set(ByVal value As String)
                mVerkKuerz = value
            End Set
        End Property

        Public Property KiReferenz() As String
            Get
                Return mKiReferenz
            End Get
            Set(ByVal value As String)
                mKiReferenz = value
            End Set
        End Property

        Public Property Notiz() As String
            Get
                Return mNotiz
            End Get
            Set(ByVal value As String)
                mNotiz = value
            End Set
        End Property

        Public Property Hersteller() As String
            Get
                Return mHersteller
            End Get
            Set(ByVal value As String)
                mHersteller = value
            End Set
        End Property

        Public Property Typ() As String
            Get
                Return mTyp
            End Get
            Set(ByVal value As String)
                mTyp = value
            End Set
        End Property

        Public Property VarianteVersion() As String
            Get
                Return mVarianteVersion
            End Get
            Set(ByVal value As String)
                mVarianteVersion = value
            End Set
        End Property

        Public Property TypPruef() As String
            Get
                Return mTypPruef
            End Get
            Set(ByVal value As String)
                mTypPruef = value
            End Set
        End Property

        Public Property Fahrgestellnummer() As String
            Get
                Return mFahrgestellnummer
            End Get
            Set(ByVal value As String)
                mFahrgestellnummer = value
            End Set
        End Property

        Public Property FahrgestellnummerPruef() As String
            Get
                Return mFahrgestellnummerPruef
            End Get
            Set(ByVal value As String)
                mFahrgestellnummerPruef = value
            End Set
        End Property

        Public Property StVANr() As String
            Get
                Return mStVANr
            End Get
            Set(ByVal value As String)
                mStVANr = value
            End Set
        End Property

        Public Property EVB() As String
            Get
                Return mEVB
            End Get
            Set(ByVal value As String)
                mEVB = value
            End Set
        End Property

        Public Property Zulassungsdatum() As String
            Get
                Return mZulassungsdatum
            End Get
            Set(ByVal value As String)
                mZulassungsdatum = value
            End Set
        End Property

        Public Property Kennzeichen() As String
            Get
                Return mKennzeichen
            End Get
            Set(ByVal value As String)
                mKennzeichen = value
            End Set
        End Property

        Public Property Bemerkung() As String
            Get
                Return mBemerkung
            End Get
            Set(ByVal value As String)
                mBemerkung = value
            End Set
        End Property

        Public Property Einzelkennzeichen() As Boolean
            Get
                Return mEinzelkennzeichen
            End Get
            Set(ByVal value As Boolean)
                mEinzelkennzeichen = value
            End Set
        End Property

        Public Property Krad() As Boolean
            Get
                Return mKrad
            End Get
            Set(ByVal value As Boolean)
                mKrad = value
            End Set
        End Property

        Public Property KennzeichenTyp() As String
            Get
                Return mKennzeichenTyp
            End Get
            Set(ByVal value As String)
                mKennzeichenTyp = value
            End Set
        End Property

        Public Property Wunschkennzeichen() As Boolean
            Get
                Return mWunschkennzeichen
            End Get
            Set(ByVal value As Boolean)
                mWunschkennzeichen = value
            End Set
        End Property

        Public Property Reserviert() As Boolean
            Get
                Return mReserviert
            End Get
            Set(ByVal value As Boolean)
                mReserviert = value
            End Set
        End Property

        Public Property Reservierungsnr() As String
            Get
                Return mReservierungsnr
            End Get
            Set(ByVal value As String)
                mReservierungsnr = value
            End Set
        End Property

        Public Property Statustext() As String
            Get
                Return mStatustext
            End Get
            Set(ByVal value As String)
                mStatustext = value
            End Set
        End Property

        Public Property Materialnummer() As String
            Get
                Return mMaterialnummer
            End Get
            Set(ByVal value As String)
                mMaterialnummer = value
            End Set
        End Property

        Public Property MaterialnummerAlt() As String
            Get
                Return mMaterialnummerAlt
            End Get
            Set(ByVal value As String)
                mMaterialnummerAlt = value
            End Set
        End Property

        Public Property StVANrAlt() As String
            Get
                Return mStVANrAlt
            End Get
            Set(ByVal value As String)
                mStVANrAlt = value
            End Set
        End Property

        Public Property HalterReferenz() As String
            Get
                Return mHalterReferenz
            End Get
            Set(ByVal value As String)
                mHalterReferenz = value
            End Set
        End Property

        Public Property Bestellnummer() As String
            Get
                Return mBestellnummer
            End Get
            Set(ByVal value As String)
                mBestellnummer = value
            End Set
        End Property

        Public Property Briefnummer() As String
            Get
                Return mBriefnummer
            End Get
            Set(ByVal value As String)
                mBriefnummer = value
            End Set
        End Property

        Public Property BLZ() As String
            Get
                Return mBLZ
            End Get
            Set(ByVal value As String)
                mBLZ = value
            End Set
        End Property

        Public Property Kontonummer() As String
            Get
                Return mKontonr
            End Get
            Set(ByVal value As String)
                mKontonr = value
            End Set
        End Property

        Public Property IBAN() As String
            Get
                Return mIBAN
            End Get
            Set(ByVal value As String)
                mIBAN = value
            End Set
        End Property

        Public Property SWIFT() As String
            Get
                Return mSWIFT
            End Get
            Set(ByVal value As String)
                mSWIFT = value
            End Set
        End Property

        Public Property Einzug() As Boolean
            Get
                Return mEinzug
            End Get
            Set(ByVal value As Boolean)
                mEinzug = value
            End Set
        End Property

        Public Property He() As String
            Get
                Return mHe
            End Get
            Set(ByVal value As String)
                mHe = value
            End Set
        End Property

        Public Property Fr() As String
            Get
                Return mFr
            End Get
            Set(ByVal value As String)
                mFr = value
            End Set
        End Property

        Public Property Fi() As String
            Get
                Return mFi
            End Get
            Set(ByVal value As String)
                mFi = value
            End Set
        End Property

        Public Property nPaUsed() As Boolean
            Get
                Return mNPaUsed
            End Get
            Set(ByVal value As Boolean)
                mNPaUsed = value
            End Set
        End Property

        Public Property ErrorText() As String
            Get
                Return mErrText
            End Get
            Set(ByVal value As String)
                mErrText = value
            End Set
        End Property

        Public Property SapId() As String
            Get
                Return mSapId
            End Get
            Set(ByVal value As String)
                mSapId = value
            End Set
        End Property

        Public Property HalterNeeded As HalterErfOptionen
            Get
                Return mHalterNeeded
            End Get
            Set(value As HalterErfOptionen)
                mHalterNeeded = value
            End Set
        End Property

        Public Property HalterMerken As Boolean
            Get
                Return mHalterMerken
            End Get
            Set(value As Boolean)
                mHalterMerken = value
            End Set
        End Property

        Public Property TypDatenNeeded As Char
            Get
                Return mTypDatenNeeded
            End Get
            Set(value As Char)
                mTypDatenNeeded = value
            End Set
        End Property

        Public Property AltKennzeichenNeeded As Char
            Get
                Return mAltKennzeichenNeeded
            End Get
            Set(value As Char)
                mAltKennzeichenNeeded = value
            End Set
        End Property

        Public Property AltKennzeichen As String
            Get
                Return mAltKennzeichen
            End Get
            Set(value As String)
                mAltKennzeichen = value
            End Set
        End Property

        Property AusdruckNeeded As Char
            Get
                Return mAusdruckNeeded
            End Get
            Set(value As Char)
                mAusdruckNeeded = value
            End Set
        End Property

        Property BarcodeNeeded As Char
            Get
                Return mBarcodeNeeded
            End Get
            Set(value As Char)
                mBarcodeNeeded = value
            End Set
        End Property

        Property GrunddatenVisible As Boolean
            Get
                Return mGrunddatenVisible
            End Get
            Set(value As Boolean)
                mGrunddatenVisible = value
            End Set
        End Property

        Property FahrzeugdatenVisible As Boolean
            Get
                Return mFahrzeugdatenVisible
            End Get
            Set(value As Boolean)
                mFahrzeugdatenVisible = value
            End Set
        End Property

        Property DienstleistungenVisible As Boolean
            Get
                Return mDienstleistungenVisible
            End Get
            Set(value As Boolean)
                mDienstleistungenVisible = value
            End Set
        End Property

        Property ZusatzdienstleistungenVisible As Boolean
            Get
                Return mZusatzdienstleistungenVisible
            End Get
            Set(value As Boolean)
                mZusatzdienstleistungenVisible = value
            End Set
        End Property

        Property ZusammenfassungVisible As Boolean
            Get
                Return mZusammenfassungVisible
            End Get
            Set(value As Boolean)
                mZusammenfassungVisible = value
            End Set
        End Property

        Property NpaVisible As Boolean
            Get
                Return mNpaVisible
            End Get
            Set(value As Boolean)
                mNpaVisible = value
            End Set
        End Property

        Property OrteZurPlz As DataTable
            Get
                Return mOrteZurPlz
            End Get
            Set(value As DataTable)
                mOrteZurPlz = value
            End Set
        End Property

        Public Property SelKundennr() As String
            Get
                Return mSelKundennr
            End Get
            Set(ByVal value As String)
                mSelKundennr = value
            End Set
        End Property

        Public Property SelKennzeichen() As String
            Get
                Return mSelKennzeichen
            End Get
            Set(ByVal value As String)
                mSelKennzeichen = value
            End Set
        End Property

        Public Property SelReferenz() As String
            Get
                Return mSelReferenz
            End Get
            Set(ByVal value As String)
                mSelReferenz = value
            End Set
        End Property

        Public Property SelLoeschkennzeichen() As String
            Get
                Return mSelLoeschkennzeichen
            End Get
            Set(ByVal value As String)
                mSelLoeschkennzeichen = value
            End Set
        End Property

        Public Property SelZuldatVon() As String
            Get
                Return mSelZuldatVon
            End Get
            Set(ByVal value As String)
                mSelZuldatVon = value
            End Set
        End Property

        Public Property SelZuldatBis() As String
            Get
                Return mSelZuldatBis
            End Get
            Set(ByVal value As String)
                mSelZuldatBis = value
            End Set
        End Property

        Public Property AgeConfirmationRequired() As Boolean
            Get
                Return mAgeConfirmationRequired
            End Get
            Set(ByVal value As Boolean)
                mAgeConfirmationRequired = value
            End Set
        End Property

        Public Property VinConfirmationRequired() As Boolean
            Get
                Return mVinConfirmationRequired
            End Get
            Set(ByVal value As Boolean)
                mVinConfirmationRequired = value
            End Set
        End Property

        Public Property AltkennzeichenSpeichern() As Boolean
            Get
                Return mAltkennzeichenSpeichern
            End Get
            Set(ByVal value As Boolean)
                mAltkennzeichenSpeichern = value
            End Set
        End Property

        Public Property ArtGenehmigungSpeichern() As Boolean
            Get
                Return mArtGenehmigungSpeichern
            End Get
            Set(ByVal value As Boolean)
                mArtGenehmigungSpeichern = value
            End Set
        End Property

        Public Property PrueforganisationSpeichern() As Boolean
            Get
                Return mPrueforganisationSpeichern
            End Get
            Set(ByVal value As Boolean)
                mPrueforganisationSpeichern = value
            End Set
        End Property

        Public Property GutachtenNrSpeichern() As Boolean
            Get
                Return mGutachtenNrSpeichern
            End Get
            Set(ByVal value As Boolean)
                mGutachtenNrSpeichern = value
            End Set
        End Property

        Public ReadOnly Property AutohausVorgangListe() As DataTable
            Get
                Return mAutohausVorgangListe
            End Get
        End Property

        Public Property Autohausvorgang() As Boolean
            Get
                Return mAutohausvorgang
            End Get
            Set(ByVal value As Boolean)
                mAutohausvorgang = value
            End Set
        End Property

        Public Property DadVorgang() As Boolean
            Get
                Return mDadVorgang
            End Get
            Set(ByVal value As Boolean)
                mDadVorgang = value
            End Set
        End Property

        Public Property FahrzeugdatenNeeded() As String
            Get
                Return mFahrzeugdatenNeeded
            End Get
            Set(value As String)
                mFahrzeugdatenNeeded = value
            End Set
        End Property

        Public Property FarbeNeeded() As String
            Get
                Return mFarbeNeeded
            End Get
            Set(value As String)
                mFarbeNeeded = value
            End Set
        End Property

        Public Property ZB1Needed() As String
            Get
                Return mZB1Needed
            End Get
            Set(value As String)
                mZB1Needed = value
            End Set
        End Property

        Public Property Fahrzeugklasse() As String
            Get
                Return mFahrzeugklasse
            End Get
            Set(value As String)
                mFahrzeugklasse = value
            End Set
        End Property

        Public Property AufbauArt() As String
            Get
                Return mAufbauArt
            End Get
            Set(value As String)
                mAufbauArt = value
            End Set
        End Property

        Public Property Farbe() As String
            Get
                Return mFarbe
            End Get
            Set(value As String)
                mFarbe = value
            End Set
        End Property

        Public Property ZB1Nummer() As String
            Get
                Return mZB1Nummer
            End Get
            Set(value As String)
                mZB1Nummer = value
            End Set
        End Property

        Public Property VersicherungsnummerNeeded() As String
            Get
                Return mVersicherungsnummerNeeded
            End Get
            Set(value As String)
                mVersicherungsnummerNeeded = value
            End Set
        End Property

        Public Property SepaDatumNeeded() As String
            Get
                Return mSepaDatumNeeded
            End Get
            Set(value As String)
                mSepaDatumNeeded = value
            End Set
        End Property

        Public Property Versicherungsnummer() As String
            Get
                Return mVersicherungsnummer
            End Get
            Set(value As String)
                mVersicherungsnummer = value
            End Set
        End Property

        Public Property SepaDatum() As String
            Get
                Return mSepaDatum
            End Get
            Set(value As String)
                mSepaDatum = value
            End Set
        End Property

        Public ReadOnly Property Versicherungsunternehmen() As DataTable
            Get
                Return mVersicherungsunternehmen
            End Get
        End Property

        Public ReadOnly Property Formular() As Byte()
            Get
                Return mFormular
            End Get
        End Property

#End Region

#Region " Methods"

        Public Sub New(ByRef objUser As User, ByRef objApp As App, ByVal strAppId As String, ByVal strSessionId As String, ByVal fileName As String)
            MyBase.New(objUser, objApp, fileName)
            m_strAppID = strAppId
            m_strSessionID = strSessionId
        End Sub

        Public Overloads Sub Fill(ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.FILL"

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONL_GET_STAMMDATEN", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_VKORG", mVerkaufsorganisation)
                    myProxy.setImportParameter("I_VKBUR", mVerkaufsbuero)
                    myProxy.setImportParameter("I_GRUPPE", mGruppe)

                    myProxy.callBapi()

                    mKunden = myProxy.getExportTable("GT_KUNNR")
                    mKreise = myProxy.getExportTable("GT_STVA")
                    mDienstleistungen = myProxy.getExportTable("GT_ONLMAT")
                    mZusatzdienstleistungen = myProxy.getExportTable("GT_MAT")
                    mPrueforganisationen = myProxy.getExportTable("GT_WERTE")
                    mVersicherungsunternehmen = myProxy.getExportTable("GT_VERSICHERER")

                    'Kundennamen aufbereiten
                    For Each dRow As DataRow In mKunden.Rows
                        dRow("NAME1") = dRow("NAME1").ToString() & " ~ " & dRow("KUNNR").ToString().PadLeft(10, "0"c)

                        If dRow("DATLT").ToString() <> String.Empty Then
                            dRow("NAME1") = dRow("NAME1").ToString() & " / " & dRow("DATLT").ToString()
                        End If
                    Next
                    mKunden.AcceptChanges()

                    'StVA-Bezeichnungen aufbereiten            
                    For Each dRow As DataRow In mKreise.Rows
                        dRow("KREISBEZ") = Left(dRow("ZKFZKZ").ToString() & "....", 4) & dRow("KREISBEZ").ToString()
                    Next
                    mKreise.AcceptChanges()

                    'ZusatzDL-Bezeichnungen aufbereiten
                    mZusatzdienstleistungen.Columns.Add("AUSWAHL")
                    For Each dRow As DataRow In mZusatzdienstleistungen.Rows
                        dRow("MAKTX") = dRow("MATNR").ToString().TrimStart("0"c) & " - " & dRow("MAKTX").ToString()
                    Next
                    mZusatzdienstleistungen.AcceptChanges()

                    'DL-Bezeichnungen aufbereiten
                    For Each dRow As DataRow In mDienstleistungen.Rows
                        dRow("MAKTX") = dRow("MATNR").ToString().TrimStart("0"c) & " - " & dRow("MAKTX").ToString()
                    Next
                    mDienstleistungen.AcceptChanges()

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Overloads Sub FillUebersicht2(ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.FillUebersicht"

            m_intStatus = 0
            m_strMessage = ""

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ZULASSUNGSDATEN_ONL", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("VKORG", Verkaufsorganisation)
                    myProxy.setImportParameter("VKBUR", Verkaufsbuero)
                    If Not String.IsNullOrEmpty(SelKundennr) AndAlso SelKundennr.TrimStart("0"c).Length > 0 Then
                    myProxy.setImportParameter("KUNNR", SelKundennr.PadLeft(10, "0"c))
                    End If
                    myProxy.setImportParameter("ZZREFNR", SelReferenz)
                    myProxy.setImportParameter("ZZKENN", SelKennzeichen)
                    myProxy.setImportParameter("ZZZLDAT_VON", SelZuldatVon)
                    myProxy.setImportParameter("ZZZLDAT_BIS", SelZuldatBis)
                    myProxy.setImportParameter("ZZLOESCH", SelLoeschkennzeichen)

                    myProxy.callBapi()

                    Dim tempTable As DataTable
                    tempTable = myProxy.getExportTable("EXTAB")

                    tempTable.Columns("ZZSTATUSUHRZEIT").MaxLength = 8
                    tempTable.AcceptChanges()

                    For Each dr As DataRow In tempTable.Rows
                        If dr("ZZSTATUSDATUM").ToString.Length = 0 Then
                            dr("ZZSTATUSUHRZEIT") = ""
                        End If
                    Next
                        tempTable.AcceptChanges()

                    CreateOutPut(tempTable, appId)

                Catch ex As Exception
                    m_intStatus = -9999
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "NO_DATA"
                            m_strMessage = "Es konnten keine Daten ermittelt werden."
                        Case "NO_INTERVAL"
                            m_strMessage = "Ung�ltiger Zeitraum."
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Select
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Function Save2(ByVal page As Page) As Boolean
            m_strClassAndMethod = "Beauftragung.Save2"

            m_intStatus = 0
            m_strMessage = ""

            Dim success As Boolean = False
            Dim encryptData As String = ""

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONL_IMPORT_VORERFASSUNG", m_objApp, m_objUser, page)

                    Dim belegTable As DataTable = myProxy.getImportTable("IS_BELEG")
                    Dim belegRow As DataRow = belegTable.NewRow

                    belegRow("VKORG") = m_objUser.Reference.Substring(0, 4)
                    belegRow("VKBUR") = m_objUser.Reference.Substring(4, 4)
                    belegRow("KUNNR") = Kundennr.PadLeft(10, "0"c)
                    belegRow("ZZKENN") = Kennzeichen

                    If Zulassungsdatum Is Nothing Then
                        belegRow("ZZZLDAT") = String.Empty
                    Else
                        belegRow("ZZZLDAT") = Zulassungsdatum
                    End If

                    belegRow("USERID") = m_objUser.UserID.ToString
                    belegRow("USERNAME") = m_objUser.UserName
                    belegRow("RESERVIERT") = IIf(Reserviert, "X", "")
                    belegRow("RESERVID") = Reservierungsnr
                    belegRow("KENNZTYP") = KennzeichenTyp
                    belegRow("EINKZ") = IIf(Einzelkennzeichen, "X", "")
                    belegRow("ZZWUNSCH") = IIf(Wunschkennzeichen, "X", "")
                    belegRow("ZKUNDREF") = KiReferenz
                    belegRow("ZKUNDNOTIZ") = Notiz
                    belegRow("ZKRAD_KZ") = IIf(Krad, "X", "")
                    belegRow("ZZFAHRG") = Bestellnummer
                    belegRow("KREIS") = StVANr
                    belegRow("MATNR") = Materialnummer
                    belegRow("ZZHALTER") = HalterReferenz
                    belegRow("ZVERKAEUFER") = VerkKuerz
                    belegRow("ZZTEXT") = Bemerkung
                    belegRow("BANKL") = BLZ
                    belegRow("BANKN") = Kontonummer
                    belegRow("XEZER") = IIf(Einzug, "X", "")
                    belegRow("FGNU") = Fahrgestellnummer
                    belegRow("ANR") = HalterAnrede
                    belegRow("VNAM") = Haltername1
                    belegRow("RNAM") = Haltername2
                    belegRow("STRN") = HalterStrasse
                    belegRow("STRH") = HalterHausnr
                    belegRow("STRB") = HalterHausnrZusatz
                    belegRow("PLZ") = HalterPLZ
                    belegRow("ORT") = HalterOrt

                    If Zulassungsdatum Is Nothing Then
                        belegRow("ZUDA") = DBNull.Value
                    Else
                        belegRow("ZUDA") = Zulassungsdatum
                    End If

                    belegRow("HERS") = Hersteller
                    belegRow("TYP") = Typ
                    belegRow("VVS") = VarianteVersion
                    belegRow("TYPZ") = TypPruef
                    belegRow("FGPZ") = FahrgestellnummerPruef
                    belegRow("ZZREFERENZCODE") = ReferenzCode
                    If Grosskundennr <> "0" Then
                        belegRow("ZZGROSSKUNDNR") = Grosskundennr
                    Else
                        belegRow("ZZGROSSKUNDNR") = DBNull.Value
                    End If
                    belegRow("ZZEVB") = EVB
                    belegRow("BRNR") = Briefnummer
                    belegRow("BLZ") = BLZ
                    belegRow("KONTO") = Kontonummer
                    belegRow("IBAN") = IBAN
                    belegRow("SWIFT") = SWIFT

                    If mNaechsteHUNeeded = "H"c Or mNaechsteHUNeeded = "G"c Or mNaechsteHUNeeded = "B"c Then
                        belegRow("NAHU") = NaechsteHU
                    Else
                        belegRow("NAHU") = DBNull.Value
                    End If

                    belegRow("ARTGE") = ArtGenehmigung
                    belegRow("PRUEFORG") = Prueforganisation
                    belegRow("NRGUT") = GutachtenNummer

                    If Geburtstag Is Nothing Then
                        belegRow("GEBDAT") = DBNull.Value
                    ElseIf Geburtstag = String.Empty Then
                        belegRow("GEBDAT") = DBNull.Value
                    Else
                        belegRow("GEBDAT") = Geburtstag
                    End If

                    belegRow("GEBORT") = Geburtsort

                    If mAltKennzeichenNeeded = "P"c Or mAltKennzeichenNeeded = "O"c Then
                        belegRow("ALT_AKZ") = AltKennzeichen
                    End If

                    belegRow("FZKL") = Fahrzeugklasse
                    belegRow("ZZCODE_AUFBAU") = AufbauArt
                    belegRow("FARBE1") = Farbe
                    belegRow("ZB1_NR") = ZB1Nummer

                    belegRow("VSU_NR") = IIf(mVersicherungsnummerNeeded = "J", mVersicherungsnummer, "")
                    belegRow("SEPA_DATE") = IIf(mSepaDatumNeeded = "J", SepaDatum, DBNull.Value)

                    belegTable.Rows.Add(belegRow)
                    belegTable.AcceptChanges()

                    'Zusatzdienstleistungen, wenn vorhanden
                    Dim zusDLs As DataRow() = Zusatzdienstleistungen.Select("AUSWAHL='X'")
                    If zusDLs.Count > 0 Then
                        Dim posTable As DataTable = myProxy.getImportTable("GT_ZUS_POS")
                        For Each dRow As DataRow In zusDLs
                            Dim newRow As DataRow = posTable.NewRow()
                            newRow("MATNR") = dRow("MATNR")
                            posTable.Rows.Add(newRow)
                        Next
                        posTable.AcceptChanges()
                    End If

                    'Importparameter
                    If nPaUsed = True Then

                        Dim stringToEncrypt As String

                        stringToEncrypt = Haltername1 & "|"
                        stringToEncrypt &= Haltername2 & "|"
                        stringToEncrypt &= HalterStrasse & "|"
                        stringToEncrypt &= HalterPLZ & "|"
                        stringToEncrypt &= HalterOrt & "|"
                        stringToEncrypt &= Geburtstag & "|"
                        stringToEncrypt &= Geburtsort

                        encryptData = EncrData(stringToEncrypt)

                    End If

                    myProxy.setImportParameter("I_STRING", encryptData)

                    'Bei Vorg�ngen, die aus der Autohaus-Vorerfassung kommen, die Belegnummer mitgeben
                    If Autohausvorgang Then
                        myProxy.setImportParameter("I_ZULBELN", SapId)
                    End If

                    myProxy.setImportParameter("I_ERNAM", m_objUser.UserName)

                    myProxy.callBapi()

                    Dim subrc As String = myProxy.getExportParameter("E_SUBRC")
                    If subrc <> "0" Then
                        success = False
                        ErrorText = myProxy.getExportParameter("E_MESSAGE")
                        m_intStatus = -5555
                        m_strMessage = "Fehler beim Speichern.<br>(" & ErrorText & ")"
                        mFormular = Nothing
                    Else
                        mSapId = myProxy.getExportParameter("E_ZULBELN")
                        mFormular = myProxy.getExportParameterByte("E_PDF")
                        Dim errorTable As DataTable = myProxy.getExportTable("ET_FEHLER")

                        If errorTable.Rows.Count > 0 Then
                            success = False
                            ErrorText = errorTable.Rows(0)("FEHLERTEXT").ToString

                        Else
                            ErrorText = encryptData
                            success = True
                        End If
                    End If

                Catch ex As Exception
                    m_intStatus = -5555
                    success = False
                    ErrorText = ex.Message
                    Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        Case "NO_DATA"
                            m_strMessage = "Keine Daten."
                        Case Else
                            m_intStatus = -9999
                            m_strMessage = "Fehler beim Speichern.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                    End Select
                Finally
                    m_blnGestartet = False

                End Try
            End If

            Return success

        End Function

        Public Function CheckVin(ByVal fahrgnr As String, ByVal pruefziffer As String, ByVal page As Page) As Integer
            m_strClassAndMethod = "Beauftragung.CheckVin"

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_PRUEF_FIN_001", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_FGNU", fahrgnr)
                    myProxy.setImportParameter("I_FGPZ", pruefziffer)

                    myProxy.callBapi()

                    Return CInt(myProxy.getExportParameter("E_STATUS"))

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If
            Return 0
        End Function

        Public Function FillTypdaten(ByVal herst As String, _
                                     ByVal typschluessel As String, _
                                     ByVal vvs As String, _
                                     ByVal pruefziffer As String, _
                                     ByVal page As Page) As DataTable

            m_strClassAndMethod = "Beauftragung.FillTypdaten"

            Dim tempTable As New DataTable

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_TYPDATEN_001", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_ZZHERSTELLER_SCH", herst)
                    myProxy.setImportParameter("I_ZZTYP_SCHL", typschluessel)
                    myProxy.setImportParameter("I_ZZVVS_SCHLUESSEL", vvs)
                    myProxy.setImportParameter("I_ZZTYP_VVS_PRUEF", pruefziffer)

                    myProxy.callBapi()

                    mTypdatenMessage = myProxy.getExportParameter("E_MESSAGE")

                    tempTable = myProxy.getExportTable("GT_WEB")

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

            Return tempTable

        End Function

        Public Function CheckGrosskundennummer(ByVal kba As String, ByVal grosskundennummer As String, ByVal page As Page) As String
            m_strClassAndMethod = "Beauftragung.CheckGrosskundennummer"

            Dim returnValue As String = ""

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_GROSSKUNDEN_PRUEF", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_ZKBA1", kba)
                    myProxy.setImportParameter("I_ZZGROSSKUNDNR", grosskundennummer)
                    myProxy.setImportParameter("I_ZKUNNR", Kundennr.PadLeft(10, "0"c))

                    myProxy.callBapi()

                    Dim tempTable As DataTable = myProxy.getExportTable("GT_OUT")

                    If tempTable.Rows.Count > 0 Then
                        returnValue = tempTable.Rows(0)("ZNAME1").ToString
                    End If

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

            Return returnValue

        End Function

        Public Sub FillGrosskunden(ByVal kba As String, ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.FillGrosskunden"

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_GROSSKUNDEN_PRUEF", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_ZKBA1", kba)
                    myProxy.setImportParameter("I_ZKUNNR", Kundennr.PadLeft(10, "0"c))

                    myProxy.callBapi()

                    mGrosskunden = myProxy.getExportTable("GT_OUT")

                    'Gro�kundennamen aufbereiten
                    For Each dRow As DataRow In mGrosskunden.Rows
                        dRow("ZNAME1") = dRow("ZNAME1").ToString() & " ~ " & dRow("ZZGROSSKUNDNR").ToString().PadLeft(10, "0"c)
                    Next
                    mGrosskunden.AcceptChanges()

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

        End Sub

        Public Sub GetOrteZurPlz(ByVal plz As String, ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.GetOrteZurPlz"

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONL_GET_ORT", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_PLZ", plz)

                    myProxy.callBapi()

                    mOrteZurPlz = myProxy.getExportTable("GT_ORTE")

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

        End Sub

        Public Function CheckHerstellerFahrgestellnummer(ByVal herst As String, ByVal vin As String, ByVal page As Page) As Boolean
            m_strClassAndMethod = "Beauftragung.CheckHerstellerFahrgestellnummer"

            Dim blnSuccess As Boolean = False

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONL_CHECK_FIN", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_CHASSIS_NUM", vin.Substring(0, 3))
                    myProxy.setImportParameter("I_ZZHERSTELLER_SCH", herst)

                    myProxy.callBapi()

                    Dim subrc As String = myProxy.getExportParameter("E_SUBRC")
                    blnSuccess = (subrc = "0")

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

            Return blnSuccess

        End Function

        Public Function CheckBarcode(ByVal barcode As String, ByVal page As Page) As String
            m_strClassAndMethod = "Beauftragung.CheckBarcode"

            Dim returnValue As String = ""

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_STATUS_REFCODE", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_ZZREFERENZCODE", barcode)

                    myProxy.callBapi()

                    returnValue = myProxy.getExportParameter("E_RETURNCODE")
                    mStatustext = myProxy.getExportParameter("E_ZZSTATUS")

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

            Return returnValue

        End Function

        Public Function SetNewZulassungsID(ByVal idSap As Int32) As Int32
            Using conn As New SqlClient.SqlConnection(ConfigurationManager.AppSettings("Connectionstring"))
                conn.Open()

                Using cmd As SqlClient.SqlCommand = conn.CreateCommand()
                    cmd.CommandType = CommandType.Text
                    cmd.CommandText = "SELECT PValue FROM Parameters WHERE  (PName = 'HoechsteZulassungsID')"
                    If idSap > CType(cmd.ExecuteScalar, Int32) Then
                        cmd.CommandText = "UPDATE Parameters SET PValue = " & idSap.ToString & " WHERE  (PName = 'HoechsteZulassungsID')"
                        cmd.ExecuteNonQuery()
                    End If
                End Using

                conn.Close()
            End Using

            Return idSap
        End Function

        Private Function EncrData(ByVal textToEncrypt As String) As String

            Dim encText As String
            Dim rd As New RijndaelManaged

            Dim md5 As New MD5CryptoServiceProvider
            Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes("@S33wolf"))

            md5.Clear()
            rd.Key = key
            rd.GenerateIV()

            Dim iv() As Byte = rd.IV
            Dim ms As New MemoryStream

            ms.Write(iv, 0, iv.Length)

            Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
            Dim data() As Byte = Encoding.UTF8.GetBytes(textToEncrypt)

            cs.Write(data, 0, data.Length)
            cs.FlushFinalBlock()

            Dim encdata() As Byte = ms.ToArray()
            encText = Convert.ToBase64String(encdata)
            cs.Close()
            rd.Clear()

            Return encText

        End Function

        Public Shared Function CheckValidKennzeichenTeil1(ByVal kennz1 As String, Optional ByVal additionalExpressions As String() = Nothing) As Boolean

            Dim expressions() As String = {"[A-Z ���]{1,3}"}

            For i = 0 To expressions.Length - 1
                Dim match As Match = Regex.Match(kennz1, expressions(i))

                If match.Success Then
                    Do
                        If (match.Value = kennz1) Then
                            Return True
                        Else
                            match = match.NextMatch()
                        End If
                    Loop While (match.Success)
                End If
            Next

            If additionalExpressions IsNot Nothing Then
                For i = 0 To additionalExpressions.Length - 1
                    Dim match As Match = Regex.Match(kennz1, additionalExpressions(i))

                    If match.Success Then
                        Do
                            If (match.Value = kennz1) Then
                                Return True
                            Else
                                match = match.NextMatch()
                            End If
                        Loop While (match.Success)
                    End If
                Next

            End If

            Return False
        End Function

        Public Shared Function CheckValidKennzeichenTeil2(ByVal kennz2 As String, Optional ByVal additionalExpressions As String() = Nothing) As Boolean

            Dim expressions() As String = {"[A-Z]{1,2}[0-9]{1,4}[H]{0,1}", "[0-9]{1,6}", "[0-9]{2,4}[A-Z]{1}"}

            For i = 0 To expressions.Length - 1
                Dim match As Match = Regex.Match(kennz2, expressions(i))

                If match.Success Then
                    Do
                        If (match.Value = kennz2) Then
                            Return True
                        Else
                            match = match.NextMatch()
                        End If
                    Loop While (match.Success)
                End If
            Next

            If additionalExpressions IsNot Nothing Then
                For i = 0 To additionalExpressions.Length - 1
                    Dim match As Match = Regex.Match(kennz2, additionalExpressions(i))

                    If match.Success Then
                        Do
                            If (match.Value = kennz2) Then
                                Return True
                            Else
                                match = match.NextMatch()
                            End If
                        Loop While (match.Success)
                    End If
                Next
            End If

            Return False
        End Function

        Public Function GetSWIFT(ByVal strIBAN As String, ByVal page As Page) As String
            m_strClassAndMethod = "Beauftragung.GetSWIFT"

            Dim strSWIFT As String = ""

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_FI_CONV_IBAN_2_BANK_ACCOUNT", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_IBAN", strIBAN)

                    myProxy.callBapi()

                    strSWIFT = myProxy.getExportParameter("E_SWIFT")

                    m_strMessage = myProxy.getExportParameter("E_MESSAGE").ToString()
                    If Not String.IsNullOrEmpty(m_strMessage) Then
                        m_strMessage = "IBAN fehlerhaft: " & m_strMessage
                    End If

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

            Return strSWIFT

        End Function

        Public Sub LoadAutohausVorgangListe(ByVal strID As String, ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.LoadAutohausVorgangListe"

            m_intStatus = 0

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONL_GET_AH_LISTE", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_VKORG", mVerkaufsorganisation)
                    myProxy.setImportParameter("I_VKBUR", mVerkaufsbuero)
                    myProxy.setImportParameter("I_GRUPPE", mGruppe)

                    If Not String.IsNullOrEmpty(strID) Then
                        myProxy.setImportParameter("I_ZULBELN", strID.PadLeft(10, "0"c))
                    End If

                    myProxy.callBapi()

                    mAutohausVorgangListe = myProxy.getExportTable("GT_VORGANG")

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub LoadAutohausVorgangDetails(ByVal strID As String, ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.LoadAutohausVorgangDetails"

            m_intStatus = 0

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONL_GET_AH_VORGANG", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_ZULBELN", strID)

                    myProxy.callBapi()

                    Dim tblVorgang As DataTable = myProxy.getExportTable("ES_BELEG")
                    Dim tblZusDL As DataTable = myProxy.getExportTable("GT_ZUS_POS")

                    If tblVorgang.Rows.Count = 0 Then
                        Throw New Exception("Vorgang nicht vorhanden")
                    End If

                    Dim dRow As DataRow = tblVorgang.Rows(0)

                    'Grunddaten
                    SapId = strID
                    Autohausvorgang = True
                    Kundennr = dRow("KUNNR").ToString().TrimStart("0"c)
                    StVANr = dRow("KREIS").ToString()
                    Materialnummer = dRow("MATNR").ToString().TrimStart("0"c)

                    'Halterdaten
                    HalterAnrede = dRow("ANR").ToString()
                    Haltername1 = dRow("VNAM").ToString()
                    Haltername2 = dRow("RNAM").ToString()
                    Geburtstag = dRow("GEBDAT").ToString()
                    Geburtsort = dRow("GEBORT").ToString()
                    HalterStrasse = dRow("STRN").ToString()
                    HalterHausnr = dRow("STRH").ToString()
                    HalterHausnrZusatz = dRow("STRB").ToString()
                    HalterPLZ = dRow("PLZ").ToString()
                    HalterOrt = dRow("ORT").ToString()
                    HalterReferenz = dRow("ZZHALTER").ToString()
                    Bestellnummer = dRow("ZZFAHRG").ToString()
                    VerkKuerz = dRow("ZVERKAEUFER").ToString()
                    KiReferenz = dRow("ZKUNDREF").ToString()
                    Notiz = dRow("ZKUNDNOTIZ").ToString()

                    'Fahrzeugdaten
                    Hersteller = dRow("HERS").ToString()
                    Typ = dRow("TYP").ToString()
                    VarianteVersion = dRow("VVS").ToString()
                    TypPruef = dRow("TYPZ").ToString()
                    Fahrgestellnummer = dRow("FGNU").ToString()
                    FahrgestellnummerPruef = dRow("FGPZ").ToString()
                    Briefnummer = dRow("BRNR").ToString()

                    'Dienstleistung
                    EVB = dRow("ZZEVB").ToString()
                    Zulassungsdatum = dRow("ZZZLDAT").ToString()
                    Kennzeichen = dRow("ZZKENN").ToString()
                    AltKennzeichen = dRow("ALT_AKZ").ToString()
                    Bemerkung = dRow("ZZTEXT").ToString()
                    Kontonummer = dRow("BANKN").ToString()
                    BLZ = dRow("BANKL").ToString()
                    IBAN = dRow("IBAN").ToString()
                    SWIFT = dRow("SWIFT").ToString()

                    Einzelkennzeichen = (dRow("EINKZ").ToString() = "X")
                    Krad = (dRow("ZKRAD_KZ").ToString() = "X")
                    KennzeichenTyp = dRow("KENNZTYP").ToString()
                    Wunschkennzeichen = (dRow("ZZWUNSCH").ToString() = "X")
                    Reserviert = (dRow("RESERVIERT").ToString() = "X")
                    Reservierungsnr = dRow("RESERVID").ToString()

                    'Zusatzdienstleistungen
                    For Each dlRow As DataRow In Zusatzdienstleistungen.Rows
                        Dim sapRows As DataRow() = tblZusDL.Select("MATNR='" & dlRow("MATNR").ToString() & "'")
                        If sapRows.Length > 0 Then
                            dlRow("AUSWAHL") = "X"
                        Else
                            dlRow("AUSWAHL") = ""
                        End If
                    Next

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub LoadDadVorgang(ByVal strAuftragsNr As String, ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.LoadDadVorgang"

            m_intStatus = 0

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_GET_DAD_SD_ORDER", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_VBELN", strAuftragsNr)
                    myProxy.setImportParameter("I_VKBUR", mVerkaufsbuero)

                    myProxy.callBapi()

                    Dim subrc As String = myProxy.getExportParameter("E_SUBRC")

                    If subrc <> "0" Then
                        ErrorText = myProxy.getExportParameter("E_MESSAGE")
                        m_intStatus = -5555
                        m_strMessage = "Fehler beim Laden.<br>(" & ErrorText & ")"

                    Else
                        Dim tblVorgang As DataTable = myProxy.getExportTable("GS_DAD_ORDER")
                        Dim tblZusDL As DataTable = myProxy.getExportTable("GT_MAT")

                        If tblVorgang.Rows.Count = 0 Then
                            Throw New Exception("Vorgang nicht vorhanden")
                        End If

                        Dim dRow As DataRow = tblVorgang.Rows(0)

                        'Grunddaten
                        DadVorgang = True

                        Dim kdRows As DataRow() = Kunden.Select("KUNNR='" & dRow("KUNNR").ToString() & "'")
                        If kdRows.Length > 0 Then
                            Kundennr = dRow("KUNNR").ToString().TrimStart("0"c)
                        End If

                        If dRow("ZZKENN").ToString().Contains("-"c) Then
                            Dim amt As String = dRow("ZZKENN").ToString().Split("-"c)(0)

                            Dim stvaRows As DataRow() = Kreise.Select("ZKFZKZ='" & amt & "'")
                            If stvaRows.Length > 0 Then
                                StVANr = amt

                                For Each sapRow As DataRow In tblZusDL.Rows
                                    Dim dlRows As DataRow() = Dienstleistungen.Select("AMT='" & StVANr & "' AND MATNR='" & sapRow("MATNR").ToString() & "'")
                                    If dlRows.Length > 0 Then
                                        Materialnummer = sapRow("MATNR").ToString().TrimStart("0"c)
                                        Exit For
                                    End If
                                Next
                            End If
                        End If

                        'Halterdaten
                        Dim anredeDad As String = dRow("ZH_TITLE").ToString()
                        Select Case anredeDad
                            Case "0001"
                                'Frau
                                HalterAnrede = "2"
                            Case "0002"
                                'Herr
                                HalterAnrede = "1"
                            Case "0003"
                                'Firma
                                HalterAnrede = "0"
                        End Select
                        Haltername1 = dRow("ZH_NAME1").ToString()
                        Haltername2 = dRow("ZH_NAME2").ToString()
                        HalterStrasse = dRow("ZH_STREET").ToString()
                        HalterHausnr = dRow("ZH_HOUSE_NUM1").ToString()
                        HalterPLZ = dRow("ZH_POST_CODE1").ToString()
                        HalterOrt = dRow("ZH_CITY1").ToString()
                        HalterReferenz = dRow("ZZREFNR1").ToString()
                        Bestellnummer = dRow("ZZREFNR2").ToString()

                        'Fahrzeugdaten
                        Hersteller = dRow("ZZHERSTELLER_SCH").ToString()
                        Typ = dRow("ZZTYP_SCHL").ToString()
                        VarianteVersion = dRow("ZZVVS_SCHLUESSEL").ToString()
                        TypPruef = dRow("ZZTYP_VVS_PRUEF").ToString()
                        Fahrgestellnummer = dRow("CHASSIS_NUM").ToString()
                        FahrgestellnummerPruef = dRow("ZPRFZ").ToString()
                        Briefnummer = dRow("TIDNR").ToString()

                        'Dienstleistung
                        EVB = dRow("ZZVSNR").ToString()
                        Zulassungsdatum = dRow("ZZZLDAT").ToString()
                        Kennzeichen = dRow("ZZKENN").ToString()

                        Wunschkennzeichen = (dRow("WUNSCHKENN_JN").ToString() = "X")

                        'Zusatzdienstleistungen
                        For Each dlRow As DataRow In Zusatzdienstleistungen.Rows
                            Dim sapRows As DataRow() = tblZusDL.Select("MATNR='" & dlRow("MATNR").ToString() & "'")
                            If sapRows.Length > 0 Then
                                dlRow("AUSWAHL") = "X"
                            Else
                                dlRow("AUSWAHL") = ""
                            End If
                        Next

                    End If

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub ResetAutohausVorgang(ByVal strID As String, ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.ResetAutohausVorgang"

            m_intStatus = 0

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_ONL_CLEAR_ONLINE_FLAG", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_ZULBELN", strID)

                    myProxy.callBapi()

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub FillFarben(ByVal page As Page)
            m_strClassAndMethod = "Beauftragung.FillFarben"

            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_ZLD_DOMAENEN_WERTE", m_objApp, m_objUser, page)

                    myProxy.setImportParameter("I_DOMNAME", "ZFARBE")

                    myProxy.callBapi()

                    mFarben = myProxy.getExportTable("GT_WERTE")

                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                Finally
                    m_blnGestartet = False
                End Try
            End If

        End Sub

#End Region

    End Class

End Namespace