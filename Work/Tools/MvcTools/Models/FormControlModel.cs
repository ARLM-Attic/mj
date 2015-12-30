﻿using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcTools.Models
{
    public enum FormMultiColumnMode { None, Left, Right, Left3, Middle3, Right3 }

    public class FormControlModel
    {
        /// <summary>
        /// label text, grid header, etc
        /// </summary>
        public MvcHtmlString DisplayNameHtml { get; set; }

        /// <summary>
        /// will be automatically filled if the corresponding property has a "Required" attribute
        /// </summary>
        public MvcHtmlString RequiredIndicatorHtml { get; set; }

        /// <summary>
        /// the raw html control (textbox, checkbox, select, etc)
        /// </summary>
        public MvcHtmlString ControlHtml { get; set; }

        /// <summary>
        /// validation error message for this property
        /// </summary>
        public MvcHtmlString ValidationMessageHtml { get; set; }

        /// <summary>
        /// optional: Icon behind the control
        /// </summary>
        public string IconCssClass { get; set; }

        /// <summary>
        /// used for inline labels (i. e. directly behind checkboxes)
        /// </summary>
        public MvcHtmlString InlineDisplayNameHtml { get; set; }

        /// <summary>
        /// used for special template control purpose
        /// </summary>
        public IDictionary<string, object> ControlHtmlAttributes { get; set; }

        /// <summary>
        /// Is this a checkbox?
        /// </summary>
        public bool IsCheckBox { get; set; }

        /// <summary>
        /// optionally hide left positioned label 
        /// </summary>
        public bool LabelHidden { get; set; }

        private FormMultiColumnMode _columnMode = FormMultiColumnMode.None;
        public FormMultiColumnMode ColumnMode
        {
            get
            {
                if (_columnMode != FormMultiColumnMode.None)
                    return _columnMode;

                if (ControlHtmlAttributes == null || !ControlHtmlAttributes.ContainsKey("col"))
                    return FormMultiColumnMode.None;

                switch (ControlHtmlAttributes["col"].ToString())
                {
                    case "left":
                        return FormMultiColumnMode.Left;

                    case "right":
                        return FormMultiColumnMode.Right;

                    case "left3":
                        return FormMultiColumnMode.Left3;

                    case "middle3":
                        return FormMultiColumnMode.Middle3;

                    case "right3":
                        return FormMultiColumnMode.Right3;

                    default:
                        return FormMultiColumnMode.None;
                }
            }
            set { _columnMode = value; }
        }

        public IHtmlString PreControlHtml { get; set; }

        public IHtmlString PostControlHtml { get; set; }

        /// <summary>
        /// indicates if this property is persistable
        /// </summary>
        public MvcHtmlString PerstistenceIndicatorHtml { get; set; }
    }
}