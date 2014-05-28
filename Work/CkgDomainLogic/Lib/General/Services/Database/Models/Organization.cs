﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Organization")]
    public class Organization
    {
        [Key]
        public int OrganizationID { get; set; }

        public string OrganizationName { get; set; }

        public int CustomerID { get; set; }

        public string OrganizationReference { get; set; }
    }
}
