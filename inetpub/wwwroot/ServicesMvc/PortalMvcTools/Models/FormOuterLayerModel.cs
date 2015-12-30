﻿namespace PortalMvcTools.Models
{
    public class FormOuterLayerModel
    {
        public int ID { get; set; }

        public string Header { get; set; }

        public string IconCssClass { get; set; }

        public string PortletCssClass { get; set; }

        public bool IsCollapsible { get; set; }

        public bool IsClosable { get; set; }
    }
}