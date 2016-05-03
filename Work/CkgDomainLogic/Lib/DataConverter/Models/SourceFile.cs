﻿using System.Collections.Generic;
using GeneralTools.Models;

namespace CkgDomainLogic.DataConverter.Models
{
    public class SourceFile
    {
        public string FilenameOrig { get; set; }    // Original file name
        public string FilenameInternal { get; set; }     // Internal file name -> {guid}.csv
        public List<Field> Fields { get; set; }     // All Fields in source file

        public bool FirstRowIsCaption { get; set; }
        public char Delimiter { get; set; }

        public int RowCount { get { return (Fields == null || Fields.None() ? 0 : Fields[0].Records.Count); } }

        public SourceFile()
        {
            Delimiter = ';';
            FirstRowIsCaption = true;
        }
    }
}