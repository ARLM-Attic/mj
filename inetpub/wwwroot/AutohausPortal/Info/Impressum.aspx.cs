﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace AutohausPortal.Info
{
    public partial class Impressum : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlGenericControl backlink = (HtmlGenericControl)Master.FindControl("backlink");
            if (backlink != null)
                backlink.Visible = true;
        }
    }
}