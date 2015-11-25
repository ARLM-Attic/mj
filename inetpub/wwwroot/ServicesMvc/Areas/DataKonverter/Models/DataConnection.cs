﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesMvc.Areas.DataKonverter.Models
{
    public class DataConnection
    {
        public Guid Guid { get; set; }

        public string GuidSource { get; set; }
        public string GuidDest { get; set; }
        public bool SourceIsProcessor { get; set; }
        public bool DestIsProcessor { get; set; }

        public string ValueSource { get; set; }
        public string ValueDest { get; set; }


        public DataConnection()
        {
            Guid = Guid.NewGuid();
        }
    }
}