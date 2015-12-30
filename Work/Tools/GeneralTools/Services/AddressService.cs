﻿using System.Linq;
using System.Text.RegularExpressions;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public class AddressService
    {
        public static void ExtractStreetAndHouseNo(string streetAndHouseNo, out string street, out string houseNo)
        {
            if (streetAndHouseNo.IsNotNullOrEmpty() && streetAndHouseNo.Length > 0 &&
                new[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'}.ToList().Contains(streetAndHouseNo[0]))
            {
                street = streetAndHouseNo;
                houseNo = "";
                return;
            }

            var match = Regex.Match(streetAndHouseNo, @"(?<strasse>.*?\.*)\s*(?<hausnr>\d+\s*.*)");
            
            street = match.Groups["strasse"].Value;
            houseNo = match.Groups["hausnr"].Value;
            
            if (string.IsNullOrEmpty(houseNo))
                street = streetAndHouseNo;
        }

        public static void ApplyStreetAndHouseNo(IAddressStreetHouseNo addressModel)
        {
            string street, houseNo;

            if (addressModel.HausNr.IsNotNullOrEmpty())
                return;

            ExtractStreetAndHouseNo(addressModel.Strasse, out street, out houseNo);

            addressModel.Strasse = street;
            addressModel.HausNr = houseNo;
        }

        public static string FormatStreetAndHouseNo(IAddressStreetHouseNo addressModel)
        {
            return FormatStreetAndHouseNo(addressModel.Strasse, addressModel.HausNr);
        }

        public static string FormatStreetAndHouseNo(string strasse, string hausNr)
        {
            if (hausNr.IsNullOrEmpty())
                return strasse;

            return string.Format("{0} {1}", strasse, hausNr);
        }
    }
}