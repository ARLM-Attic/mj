﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Models;
using MvcTools.Web;
using System.Web.Mvc.Ajax;

namespace PortalMvcTools.Web
{
    public static class PortalHtmlHelperExtensions
    {
        #region Divs

        public static MvcHtmlString DivLinie(this HtmlHelper html, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        {
            return html.EmptyDiv("linie", marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);
        }

        public static MvcHtmlString FormNewLine(this HtmlHelper html, string additionalCssClass = "top10")
        {
            return html.EmptyDiv("trenner " + additionalCssClass);
        }

        public static MvcHtmlString DivMetaTrenner(this HtmlHelper html)
        {
            return html.EmptyDiv("metatrenner");
        }
        public static MvcHtmlString DivMetaLinks(this HtmlHelper html)
        {
            return html.EmptyDiv("metalinks");
        }
        public static MvcHtmlString DivMetaRechts(this HtmlHelper html)
        {
            return html.EmptyDiv("metarechts");
        }

        // ReSharper disable UnusedParameter.Local
        private static MvcTag EmptyDivMvcTag(this HtmlHelper html, string divClass, int? paddingLeft = null, int? paddingTop = null, int? paddingBottom = null, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        // ReSharper restore UnusedParameter.Local
        {
            return new MvcTag(null, "div", cssClass: divClass, paddingLeft: paddingLeft, paddingTop: paddingTop, paddingBottom: paddingBottom, marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);
        }

        public static MvcHtmlString EmptyDiv(this HtmlHelper html, string divClass, int? paddingLeft = null, int? paddingTop = null, int? paddingBottom = null, int? marginLeft = null, int? marginTop = null, int? marginBottom = null)
        {
            var tag = html.EmptyDivMvcTag(divClass, paddingLeft: paddingLeft, paddingTop: paddingTop, paddingBottom: paddingBottom, marginLeft: marginLeft, marginTop: marginTop, marginBottom: marginBottom);

            return MvcHtmlString.Create(tag.Begin() + "&nbsp;" + tag.End());
        }


        #endregion


        #region Misc


        public static MvcHtmlString FormClear(this HtmlHelper html, int formID, int marginLeft = 0)
        {
            var outerTag = new MvcTag(null, "div", cssClass: "recyclebutton", marginLeft: marginLeft, onClick: string.Format("ClearModelForm({0});", formID));

            var helpLayer = new TagBuilder("div");
            helpLayer.AddCssClass("helplayer");

            var p = new TagBuilder("p") { InnerHtml = "Leert dieses Formular." };
            helpLayer.InnerHtml = p.ToString();

            return MvcHtmlString.Create(outerTag.Begin() + helpLayer + outerTag.End());
        }

        public static MvcHtmlString FormAction(this HtmlHelper html, string javascriptAction, string cssClass, string toolTip = null, string id = null, bool hidden = false)
        {
            var outerTag = new TagBuilder("div");
            outerTag.Attributes.Add("class", cssClass);
            if (javascriptAction.IsNotNullOrEmpty())
                outerTag.Attributes.Add("onclick", javascriptAction);
            if (!string.IsNullOrEmpty(id))
                outerTag.Attributes.Add("id", id);
            if (hidden)
                outerTag.Attributes.Add("style", "display:none;");

            if (!string.IsNullOrEmpty(toolTip))
            {
                var helpLayer = new TagBuilder("div");
                helpLayer.AddCssClass("helplayer");

                var p = new TagBuilder("p") { InnerHtml = toolTip };
                helpLayer.InnerHtml = p.ToString();

                outerTag.InnerHtml = helpLayer.ToString();
            }

            return MvcHtmlString.Create(outerTag.ToString());
        }

        #endregion


        #region Misc Partial Views

        public static MvcWrapper Wrapper(this HtmlHelper html, string partialViewName, object model = null)
        {
            return new MvcWrapper(html.ViewContext, partialViewName, model);
        }

        public static MvcWrapper FormSearchBox(this HtmlHelper html, object model = null)
        {
            return new MvcWrapper(html.ViewContext, "FormSearchBox", model);
        }

        public static MvcWrapper FormSearchResultsGrid(this HtmlHelper html, object model = null)
        {
            return new MvcWrapper(html.ViewContext, "FormSearchResultsGrid", model);
        }

        public static MvcWrapper FormSearchResults(this HtmlHelper html, object model = null)
        {
            return new MvcWrapper(html.ViewContext, "FormSearchResults", model);
        }

        public static MvcHtmlString FormValidationSummaryResponsive(this HtmlHelper html, Func<Exception, IHtmlString> responsiveErrorUrlFunction = null)
        {
            html.ViewBag.ResponsiveErrorUrlFunction = responsiveErrorUrlFunction;
            return html.Partial("Partial/FormValidationSummaryResponsive", html.ViewData.ModelState);
        }

        public static MvcHtmlString IeWarning(this HtmlHelper html, int ieVersion, bool ieExplicitVersion = false)
        {
            html.ViewBag.IeVersion = ieVersion;
            html.ViewBag.IeExplicitVersion = ieExplicitVersion;
            html.ViewBag.IeWarningMessage = 
                string.Format("Diese Seite ist nicht kompatibel mit Browsern vom Typ 'Internet Explorer', Version {0} {1}",
                                ieVersion, (ieExplicitVersion ? "" : " und ältere Versionen")
                );

            return html.Partial("Partial/BrowserWarnings/IeWarning");
        }

        public static MvcHtmlString FormValidationSummary(this HtmlHelper html, bool excludePropertyErrors = true)
        {
            return html.ValidationSummary(excludePropertyErrors, Localize.PleaseCheckYourInputs);
        }

        //public static MvcHtmlString FormValidationSummaryBlankGeneralMessage(this HtmlHelper html, bool excludePropertyErrors = true)
        //{
        //    return html.ValidationSummary(excludePropertyErrors);
        //}

        public static MvcHtmlString FormWizard(this HtmlHelper html, string headerIconCssClass, string header, IEnumerable<string> stepTitles, IEnumerable<string> stepKeys = null, bool stepTitlesInNewLine = false)
        {
            var model = new FormWizardModel
            {
                Header = header,
                HeaderIconCssClass = headerIconCssClass,
                StepTitles = stepTitles.Select(t => new HtmlString(t)).ToArray(),
                StepKeys = stepKeys.ToArrayOrEmptyArray(),
                StepTitlesInNewLine = stepTitlesInNewLine,
            };

            return html.Partial("Partial/FormWizard", model);
        }

        public static MvcHtmlString FormRequiredFieldsSummary(this HtmlHelper html, Func<Exception, IHtmlString> responsiveErrorUrlFunction = null)
        {
            html.ViewBag.ResponsiveErrorUrlFunction = responsiveErrorUrlFunction;
            return html.Partial("Partial/FormRequiredFieldsSummary", html.ViewData.ModelState);
        }

        public static MvcForm AutoForm<T>(this AjaxHelper ajax, T model, string controllerName, int id) where T : class
        {
            return ajax.BeginForm(typeof(T).Name + "Form", controllerName, null,
                                  new MvcAjaxOptions { UpdateTargetId = ajax.AutoFormWrapperDivID(id), OnComplete = "AjaxFormComplete(" + id +");" },
                                  htmlAttributes: new { @class = "form-horizontal", id = "AjaxForm" + id });
        }

        public static string AutoFormWrapperDivID(this AjaxHelper ajax, int id) 
        {
            return string.Format("Div_{0}", id);
        }

        #endregion


        #region Default Form Controls, Twitter Bootstrap

        # region Label

        public static MvcHtmlString FormLabel(this HtmlHelper html, string propertyName, string cssClass = null)
        {
            return html.Label(propertyName, new { @class = cssClass });
        }

        public static MvcHtmlString FormLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string cssClass = null, string labelText = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabel", model);
        }

        private static object DictionaryToObject(IDictionary<string, object> dictionary)
        {
            var eo = new ExpandoObject();
            var eoColl = (IDictionary<string, object>)eo;

            foreach (var kvp in dictionary)
                eoColl.Add(kvp);

            return eo;
        }

        private static string GetKnockoutDataBindAttributeValue(string propertyName, string controlType = "")
        {
            var dataBindPropertyValue = string.Format("value: {0}", propertyName);
            if (controlType == "textbox")
                dataBindPropertyValue = dataBindPropertyValue + ", valueUpdate:'afterkeydown'"; 

            if (controlType.IsNotNullOrEmpty())
            {
                if (controlType == "checkbox")
                    dataBindPropertyValue = string.Format("checkedUniform: {0}", propertyName);

                //if (controlType == "datepicker")
                //    dataBindPropertyValue = string.Format("value: eval('new ' + {0}.slice(1,-1)).toString('dd.MM.yyyy')", propertyName);
            }

            return dataBindPropertyValue;
        }

        private static IDictionary<string, object> MergeKnockoutDataBindAttributes(object controlHtmlAttributes, string propertyName, string controlType = "")
        {
            var dict = controlHtmlAttributes.ToHtmlDictionary();

            try
            {

                if (!dict.ContainsKey("data-bind"))
                    return dict;

                object existingDataBindPropertyValue;
                dict.TryGetValue("data-bind", out existingDataBindPropertyValue);
                if (existingDataBindPropertyValue == null)
                    return dict;

                var knockoutDataBindAttributeValue = GetKnockoutDataBindAttributeValue(propertyName, controlType);
                var s = existingDataBindPropertyValue.ToString().Trim();
                if (s.StartsWith("xauto"))
                    dict["data-bind"] = knockoutDataBindAttributeValue + s.Replace("xauto", "");
            }
            catch{}

            return dict;
        }

        #endregion

        #region TextBox, TextArea

        static object GetAutoPostcodeCityMapping<TModel, TValue>(Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes)
        {
            var cityPropertyName = expression.GetPropertyName();
            var cityProperty = typeof(TModel).GetProperty(cityPropertyName);
            if (cityProperty == null)
                return controlHtmlAttributes;

            var postcodeCityMappingAttribute = cityProperty.GetCustomAttributes(typeof(AddressPostcodeCityMappingAttribute), true).OfType<AddressPostcodeCityMappingAttribute>().FirstOrDefault();
            if (postcodeCityMappingAttribute == null)
                return controlHtmlAttributes;

            var postCodePropertyName = postcodeCityMappingAttribute.PostCodePropertyName;
            var countryPropertyName = postcodeCityMappingAttribute.CountryPropertyName.NotNullOrEmpty();

            return TypeMerger.MergeTypes(controlHtmlAttributes, new
            {
                onfocus = string.Format("AutoPostcodeCityMapping_OnFocus($(this), '{0}', '{1}')", postCodePropertyName, countryPropertyName)
            });
        }

        public static MvcHtmlString FormTextBlockFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null)
        {
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textblock");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression, hideAsteriskTag: true),
                ControlHtml = html.TextBlockFor(expression, controlHtmlAttributesDict),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        public static MvcHtmlString FormTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            controlHtmlAttributes = GetAutoPostcodeCityMapping(expression, controlHtmlAttributes);
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textbox");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.TextBoxFor(expression, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        public static MvcHtmlString FormTextBox(this HtmlHelper html, string propertyName, object controlHtmlAttributes = null, string labelHtml = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = labelHtml.IsNotNullOrEmpty() ? new MvcHtmlString(labelHtml) : html.DisplayName(propertyName),
                RequiredIndicatorHtml = labelHtml.IsNotNullOrEmpty() ? MvcHtmlString.Empty : html.RequiredIndicator(propertyName),
                ControlHtml = html.TextBox(propertyName, null, controlHtmlAttributes),
                ValidationMessageHtml = html.ValidationMessage(propertyName),
                IconCssClass = null,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        public static MvcHtmlString FormPlaceholderTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null)
        {
            controlHtmlAttributes = GetAutoPostcodeCityMapping(expression, controlHtmlAttributes);
            controlHtmlAttributes = TypeMerger.MergeTypes(controlHtmlAttributes, new { placeholder = html.DisplayNameFor(expression).ToString() });
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textbox");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.TextBoxFor(expression, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
            };

            return html.Partial("Partial/FormControls/Form/ControlWithPlaceholder", model);
        }

        public static MvcHtmlString FormTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null, int columns = 40, int rows = 4)
        {
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "textbox");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.TextAreaFor(expression, rows, columns, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        public static MvcHtmlString FormTemplateControl(this HtmlHelper html, string label, Func<object, HelperResult> templateControlHtml, object controlHtmlAttributes = null)
        {
            var model = new FormControlModel
            {
                DisplayNameHtml = new MvcHtmlString(label),
                RequiredIndicatorHtml = MvcHtmlString.Empty,
                ControlHtml = new MvcHtmlString(templateControlHtml.Invoke(null).ToString()),
                ValidationMessageHtml = null,
                IconCssClass = null,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }
        #endregion

        #region DatePicker

        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object controlHtmlAttributes = null, string iconCssClass = null)
        {
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "datepicker");

            return FormDatePickerForInner(html, expression, controlHtmlAttributesDict, iconCssClass);
        }

        public static MvcHtmlString FormDatePickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string cssClass = "m-wrap medium", string labelCssClass = "control-label")
        {
            var htmlAttributes = FormTextBoxForGetAttributes(cssClass);

            return FormDatePickerForInner(html, expression, htmlAttributes);
        }

        private static MvcHtmlString FormDatePickerForInner<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> controlHtmlAttributes = null, string iconCssClass = null)
        {
            var formatString = "{0:d}";
            var datePickerFor = html.TextBoxFor(expression, formatString, controlHtmlAttributes)
                                    .Concat(new MvcHtmlString("<span class=\"add-on\"><i class=\"icon-calendar\"></i></span>"));
            datePickerFor = FormControlForGetSurroundingDiv(datePickerFor, "input-append datepicker");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = datePickerFor,
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes,
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        #endregion

        #region DropDownList

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            return html.FormDropDownListForInner(expression, selectList, controlHtmlAttributes, preControlHtml, postControlHtml);
        }

        public static MvcHtmlString FormDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<string> selectList,
                                                                object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            return html.FormDropDownListFor(expression, selectList.ToSelectList(), controlHtmlAttributes, preControlHtml, postControlHtml);
        }

        private static MvcHtmlString FormDropDownListForInner<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "dropdown");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.DropDownListFor(expression, selectList, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = "",
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }


        public static MvcHtmlString FormMultiSelectListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null)
        {
            return html.FormMultiSelectListForInner(expression, selectList, controlHtmlAttributes);
        }

        public static MvcHtmlString FormMultiSelectListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<string> selectList,
                                                                object controlHtmlAttributes = null)
        {
            return html.FormMultiSelectListForInner(expression, selectList.ToMultiSelectList(), controlHtmlAttributes);
        }

        private static MvcHtmlString FormMultiSelectListForInner<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null)
        {
            var controlHtmlAttributesDict = controlHtmlAttributes.MergePropertiesStrictly(new { multiple = "multiple", @class = "hide" });
            controlHtmlAttributesDict.Add("data-placeholder", "..."); // because of the hyphen it is necessary to add this attribute here and not right above
            controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributesDict, expression.GetPropertyName(), "multiselect");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.ListBoxFor(expression, selectList, controlHtmlAttributesDict),
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = "",
                ControlHtmlAttributes = controlHtmlAttributesDict,
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        #endregion

        #region RadioButtonList

        public static MvcHtmlString FormRadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string selectOptions, object controlHtmlAttributes = null)
        {
            return html.FormRadioButtonListFor(expression, selectOptions.ToSelectList(), controlHtmlAttributes);
        }

        public static MvcHtmlString FormRadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectItem> selectList, object controlHtmlAttributes = null)
        {
            return html.FormRadioButtonListFor(expression, new SelectList(selectList, "Key", "Text"), controlHtmlAttributes);
        }

        public static MvcHtmlString FormRadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, object controlHtmlAttributes = null, string iconCssClass = null)
        {
            var radioButtonsFor = MvcHtmlString.Empty.Concat(selectList.Select(item => html.FormRadioButtonForInner(expression, item)).ToArray());

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = radioButtonsFor,
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        private static MvcHtmlString FormRadioButtonForInner<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                        Expression<Func<TModel, TValue>> expression,
                                                                        SelectListItem item,
                                                                        string radioLabelCssClass = "radio")
        {
            var radio = html.RadioButtonFor(expression, item.Value, new { style = "opacity:0;" })
                            .Concat(new MvcHtmlString(" " + item.Text));  // leading html space is necessary in bootstrap for checkboxes and radiobuttons

            return FormControlForGetSurroundingDiv(radio, radioLabelCssClass, "label");
        }

        #endregion

        #region CheckBox, CheckBoxList

        public static MvcHtmlString FormCheckBoxFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> expression, object controlHtmlAttributes = null, string iconCssClass = null, Func<object, HelperResult> preControlHtml = null, Func<object, HelperResult> postControlHtml = null)
        {
            var controlHtmlAttributesDict = MergeKnockoutDataBindAttributes(controlHtmlAttributes, expression.GetPropertyName(), "checkbox");

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(expression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(expression),
                ControlHtml = html.CheckBoxFor(expression, controlHtmlAttributesDict), // MJE, deactivated this explicitely for knockout bindings:  .MergePropertiesStrictly(new { @class = "hide" })), 
                ValidationMessageHtml = html.ValidationMessageFor(expression),
                IconCssClass = iconCssClass,
                ControlHtmlAttributes = controlHtmlAttributesDict,
                PreControlHtml = preControlHtml == null ? null : preControlHtml.Invoke(null),
                PostControlHtml = postControlHtml == null ? null : postControlHtml.Invoke(null),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        public static MvcHtmlString FormCheckBoxListFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, object>> labelExpression, params Expression<Func<TModel, bool>>[] expressionArray)
        {
            return html.FormCheckBoxListForInner(expressionArray.ToList(), html.DisplayNameFor(labelExpression));
        }

        public static MvcHtmlString FormCheckBoxListFor<TModel>(this HtmlHelper<TModel> html, string labelText, params Expression<Func<TModel, bool>>[] expressionArray)
        {
            return html.FormCheckBoxListForInner(expressionArray.ToList(), new MvcHtmlString(labelText));
        }

        private static MvcHtmlString FormCheckBoxListForInner<TModel>(this HtmlHelper<TModel> html,
                                                                List<Expression<Func<TModel, bool>>> expressionList,
                                                                MvcHtmlString labelText)
        {
            var checkBoxesFor = MvcHtmlString.Empty.Concat(expressionList.Select(expression => html.FormCheckBoxForInner(expression)).ToArray());

            var firstExpression = expressionList[0];
            var validationMessageHtml = html.ValidationMessageFor(firstExpression);
            var model = new FormControlModel
            {
                DisplayNameHtml = labelText,
                RequiredIndicatorHtml = html.RequiredIndicatorFor(firstExpression),
                ControlHtml = checkBoxesFor,
                ValidationMessageHtml = validationMessageHtml,
                IconCssClass = null,
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }

        private static MvcHtmlString FormCheckBoxForInner<TModel>(this HtmlHelper<TModel> html,
                                                                        Expression<Func<TModel, bool>> expression,
                                                                        string checkBoxCssClass = "checkbox")
        {
            var radio = html.CheckBoxFor(expression, new { style = "opacity:0;" })
                            .Concat(new MvcHtmlString(" " + html.GetDisplayName(expression))); // leading html space is necessary in bootstrap for checkboxes and radiobuttons

            return FormControlForGetSurroundingDiv(radio, checkBoxCssClass, "label");
        }

        public static MvcHtmlString FormDateRangePickerFor<TModel>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, DateRange>> dateRangeExpression,
                                                                object controlHtmlAttributes = null)
        {
            //var dateRangeValue = (bool)GetPropertyValue(typeof(TModel), html.ViewData.Model, dateRangeExpression);
            var dateRangePropertyName = dateRangeExpression.GetPropertyName();

            var innerModel = new FormDateRangePickerModel
            {
                InlineDisplayNameHtml = new MvcHtmlString("Datumsbereich wählen..."),
                ControlHtml = html.CheckBox(dateRangePropertyName + ".IsSelected", controlHtmlAttributes.MergePropertiesStrictly(new { @class = "hide" })),
                ControlHtmlStartDate = html.Hidden(dateRangePropertyName + ".StartDate", null, controlHtmlAttributes),
                ControlHtmlEndDate = html.Hidden(dateRangePropertyName + ".EndDate", null, controlHtmlAttributes),
                IconCssClass = null,

                //UseDateRangeValue = useDateRangeValue,
                DateRangeProperty = dateRangePropertyName,

                //DateRangeStart = dateRangeStartValue,
                //DateRangeEnd = dateRangeEndValue,
            };

            var innerHtml = html.Partial("Partial/FormControls/Form/DateRangePicker", innerModel);

            var model = new FormControlModel
            {
                DisplayNameHtml = html.DisplayNameFor(dateRangeExpression),
                RequiredIndicatorHtml = html.RequiredIndicatorFor(dateRangeExpression),
                ControlHtml = innerHtml,
                ValidationMessageHtml = html.ValidationMessageFor(dateRangeExpression),
                IconCssClass = null,
                ControlHtmlAttributes = controlHtmlAttributes.ToHtmlDictionary(),
            };

            return html.Partial("Partial/FormControls/Form/LeftLabelControl", model);
        }
        #endregion

        private static MvcHtmlString FormControlForGetSurroundingDiv(MvcHtmlString controlHtml, string cssClass = "controls", string tagName = "div")
        {
            var outerDiv = new MvcTag(null, tagName, cssClass);
            return new MvcHtmlString(
                outerDiv.Begin() +
                controlHtml +
                outerDiv.End()
                );
        }

        private static IDictionary<string, object> FormTextBoxForGetAttributes(string cssClass, string placeHolder = "")
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("placeholder", placeHolder);

            return htmlAttributes.MergeHtmlAttributes(FormControlForGetAttributes(cssClass));
        }

        private static IEnumerable<KeyValuePair<string, object>> FormControlForGetAttributes(string cssClass)
        {
            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
            htmlAttributes.Add("class", cssClass);

            return htmlAttributes;
        }

        #endregion
    }
}