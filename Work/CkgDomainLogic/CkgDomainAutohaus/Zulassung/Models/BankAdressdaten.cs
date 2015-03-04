﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class BankAdressdaten : IValidatableObject 
    {
        public Adressdaten Rechnungsempfaenger { get; set; }

        public bool Cpdkunde { get; set; }

        public bool CpdMitEinzugsermaechtigung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DirectDebitMandate)]
        [XmlIgnore]
        public bool Einzugsermaechtigung { get { return Zahlungsart.NotNullOrEmpty() == "E"; } }

        [LocalizedDisplay(LocalizeConstants.Invoice)]
        [XmlIgnore]
        public bool Rechnung { get { return Zahlungsart.NotNullOrEmpty() == "R"; } }

        [LocalizedDisplay(LocalizeConstants.Cash)]
        [XmlIgnore]
        public bool Bar { get { return Zahlungsart.NotNullOrEmpty() == "B"; } }

        [LocalizedDisplay(LocalizeConstants.PaymentType)]
        public string Zahlungsart { get; set; }

        [XmlIgnore]
        public static string Zahlungsarten { get { return string.Format("E,{0};R,{1};B,{2}", Localize.DirectDebitMandate, Localize.Invoice, Localize.Cash); } }

        [LocalizedDisplay(LocalizeConstants.AccountHolder)]
        public string Kontoinhaber { get; set; }

        [LocalizedDisplay(LocalizeConstants.Iban)]
        public string Iban { get; set; }

        [LocalizedDisplay(LocalizeConstants.Swift)]
        public string Swift { get; set; }

        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string KontoNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.BankCode)]
        public string Bankleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreditInstitution)]
        public string Geldinstitut { get; set; }

        public bool BankdatenVollstaendig { get { return (Iban.IsNotNullOrEmpty() && Swift.IsNotNullOrEmpty() && Kontoinhaber.IsNotNullOrEmpty()); } }

        public BankAdressdaten()
        {
            Rechnungsempfaenger = new Adressdaten();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Cpdkunde && !Rechnungsempfaenger.AdresseVollstaendig)
                yield return new ValidationResult(Localize.CompleteAddressRequired);

            if (Einzugsermaechtigung && !BankdatenVollstaendig)
                yield return new ValidationResult(Localize.CompleteBankDataRequired);

            if (Rechnungsempfaenger.AdresseVollstaendig && String.IsNullOrEmpty(Zahlungsart))
                yield return new ValidationResult(Localize.PaymentTypeRequired, new[] { "Zahlungsart" });
        }

        public string GetSummaryString()
        {
            var s = "";

            if (Rechnungsempfaenger != null && !String.IsNullOrEmpty(Rechnungsempfaenger.Name1))
            {
                s += String.Format("{0}: {1}<br/>{2}", Localize.Name, Rechnungsempfaenger.Name1, Rechnungsempfaenger.Name2);
                s += String.Format("<br/>{0}: {1}", Localize.Street, Rechnungsempfaenger.Strasse);
                s += String.Format("<br/>{0}: {1} {2}", Localize.City, Rechnungsempfaenger.Plz, Rechnungsempfaenger.Ort);

                if (Einzugsermaechtigung || Rechnung || Bar)
                {
                    s += String.Format("<br/>{0}: {1}", Localize.PaymentType, (Einzugsermaechtigung ? Localize.DirectDebitMandate : (Rechnung ? Localize.Invoice : Localize.Cash)));
                }
                
                if (!String.IsNullOrEmpty(Iban))
                {
                    s += String.Format("<br/>{0}: {1}", Localize.AccountHolder, Kontoinhaber);
                    s += String.Format("<br/>{0}: {1}", Localize.Iban, Iban);
                    s += String.Format("<br/>{0}: {1}", Localize.Swift, Swift);
                    s += String.Format("<br/>{0}: {1}", Localize.CreditInstitution, Geldinstitut);
                }
            }

            return s;
        }
    }
}