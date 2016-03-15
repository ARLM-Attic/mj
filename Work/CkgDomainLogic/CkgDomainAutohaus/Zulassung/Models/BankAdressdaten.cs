﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.Autohaus.Models
{
    public class BankAdressdaten : IValidatableObject 
    {
        public bool DatenFuerCpd { get; set; }

        public Adressdaten Adressdaten { get; set; }

        public bool Cpdkunde { get; set; }

        public bool CpdMitEinzugsermaechtigung { get; set; }

        public Bankdaten Bankdaten { get; set; }

        public BankAdressdaten()
        {
            // parameterless dummy constructor for MVC model binder
        }

        public BankAdressdaten(string partnerrolle, bool datenfuercpd, string kennungAdresse = "", string land = "DE")
        {
            DatenFuerCpd = datenfuercpd;
            Adressdaten = new Adressdaten(kennungAdresse, land) { Partnerrolle = partnerrolle};
            Bankdaten = new Bankdaten { Partnerrolle = partnerrolle, KontoinhaberErforderlich = datenfuercpd };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatenFuerCpd)
            {
                if (Cpdkunde && !Adressdaten.AdresseVollstaendig)
                    yield return new ValidationResult(Localize.CompleteAddressRequired);

                if (Bankdaten.Einzugsermaechtigung && !Bankdaten.BankdatenVollstaendig)
                    yield return new ValidationResult(Localize.CompleteBankDataRequired);

                if (Adressdaten.AdresseVollstaendig && String.IsNullOrEmpty(Bankdaten.Zahlungsart))
                    yield return new ValidationResult(Localize.PaymentTypeRequired);
            }
            else
            {
                if (!Adressdaten.AdresseVollstaendig)
                    yield return new ValidationResult(Localize.CompleteAddressRequired);
            }
        }

        public string GetSummaryString()
        {
            var s = "";

            if (Adressdaten != null && !String.IsNullOrEmpty(Adressdaten.Adresse.Name1))
            {
                s += Adressdaten.Adresse.GetPostLabelString();

                if (Bankdaten.Einzugsermaechtigung || Bankdaten.Rechnung || Bankdaten.Bar)
                {
                    s += String.Format("<br/><br/>{0}: {1}", Localize.PaymentType, (Bankdaten.Einzugsermaechtigung ? Localize.DirectDebitMandate : (Bankdaten.Rechnung ? Localize.Invoice : Localize.Cash)));
                }

                if (!String.IsNullOrEmpty(Bankdaten.Iban))
                {
                    s += "<br/>";

                    if (!String.IsNullOrEmpty(Bankdaten.Kontoinhaber))
                        s += String.Format("<br/>{0}: {1}", Localize.AccountHolder, Bankdaten.Kontoinhaber);

                    s += String.Format("<br/>{0}: {1}", Localize.Iban, Bankdaten.Iban);
                    s += String.Format("<br/>{0}: {1}", Localize.Swift, Bankdaten.Swift);
                    s += String.Format("<br/>{0}: {1}", Localize.CreditInstitution, Bankdaten.Geldinstitut);
                }
            }

            return s;
        }
    }
}