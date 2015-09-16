using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.SessionState;
using GeneralTools.Models;
using Telerik.Web.Mvc.UI;

namespace MvcTools.Web
{
    public static class SessionHelper
    {
        private static HttpContext HttpContext
        {
            get { return HttpContext.Current; }
        }

        private static HttpSessionState Session
        {
            get { return HttpContext == null ? null : HttpContext.Session; }
        }


        public static void ClearSessionObject(string key)
        {
            if (Session == null)
                return;

            Session[key] = null;
        }

        public static void SetSessionValue<T>(string key, T model)
        {
            if (Session == null)
                return;

            Session[key] = model;
        }

        public static void SetSessionObject(string key, object model)
        {
            if (Session == null)
                return;

            Session[key] = model;
        }

        public static T GetSessionValue<T>(string key, T defaultVal)
        {
            if (Session == null)
                return defaultVal;

            if (key.IsNullOrEmpty())
                return defaultVal;

            T result;
            if (Session[key] != null)
                result = (T) Session[key];
            else
                // Session store for value type objects
                Session[key] = result = defaultVal;

            return result;
        }

        public static T GetSessionObject<T>(string key, Func<T> createFunc = null) where T : class
        {
            if (Session == null)
                return null;

            if (key.IsNullOrEmpty())
                return null;

            if (createFunc == null)
                createFunc = () => null;

            T result;
            if (Session[key] != null)
                result = (T)Session[key];
            else
                // Session store for reference type objects
                Session[key] = result = createFunc();

            return result;
        }

        public static object GetSessionObject(string key, Func<object> createFunc = null)
        {
            if (Session == null)
                return null;

            if (key.IsNullOrEmpty())
                return null;

            if (createFunc == null)
                createFunc = () => null;

            object result;
            if (Session[key] != null)
                result = Session[key];
            else
                // Session store for reference type objects
                Session[key] = result = createFunc();

            return result;
        }

        public static List<T> GetSessionList<T>(string key, Func<List<T>> createFunc)
        {
            return GetSessionObject(key, createFunc);
        }

        public static string GetSessionString(string key)
        {
            return (string)GetSessionObject(key);
        }

        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://www.termani.com/", ""); // Also a fake URL 
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });

            return httpContext;
        }

        public static string GridCurrentGetAutoPersistColumnsKey()
        {
            var gridCurrentModelType = (GetSessionObject("Telerik_Grid_CurrentModelTypeForAutoPersistColumns", () => null) as Type);
            if (gridCurrentModelType == null)
                return "";

            if (gridCurrentModelType.GetCustomAttributes(true).OfType<GridColumnsAutoPersistAttribute>().None())
                return "";

            var grid = (IGrid)GetSessionObject(string.Format("Telerik_Grid_{0}", gridCurrentModelType.Name));
            if (grid == null)
                return "";

            var relativeUrl = HttpContext.Current.GetAppUrlCurrent();
            return string.Format("GridColumnsAutoPersist_{0}_{1}", relativeUrl, gridCurrentModelType.Name);
        }
    }
}