﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    public class VersandartOptionen : Store, IValidatableObject 
    {
        /// <summary>
        /// Versandart: 1 = temp, 2 = endg
        /// </summary>
        [Required]
        [LocalizedDisplay(LocalizeConstants.DispatchType)]
        public string Versandart { get; set; }

        [ModelMappingCompareIgnore]
        public bool IstEndgueltigerVersand 
        { 
            get
            {
                return (Versandart == "2");
            }
            set
            {
                Versandart = (value ? "2" : "1");
            }
        }

        [XmlIgnore]
        public static string VersandartAuswahl { get { return string.Format("2,{0};1,{1}", Localize.DispatchTypeFinal, Localize.DispatchTypeTemporaryVerbose); } }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(Versandart))
            {
                yield return
                    new ValidationResult(Localize.InvalidSelection, new[] { "Versandart" });
            }
        }

        [ModelMappingCompareIgnore]
        public bool IsValid { get; set; }

        public string GetSummaryString()
        {
            var s = "";

            if (Versandart == "1")
                s += string.Format("{0}", Localize.DispatchTypeTemporaryVerbose);

            if (Versandart == "2")
                s += string.Format("{0}{1}", (s.IsNullOrEmpty() ? "" : "<br/>"), Localize.DispatchTypeFinal);

            return s;
        }
    }
}