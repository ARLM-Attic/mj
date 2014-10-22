﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.1008
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Der Quellcode wurde automatisch mit Microsoft.VSDesigner generiert. Version 4.0.30319.1008.
// 
#pragma warning disable 1591

namespace DAT_BankenLinie_Connector.de.dat.www.authentication {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AuthenticationPortBinding", Namespace="http://sphinx.dat.de/services/Authentication")]
    public partial class Authentication : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback doLoginOperationCompleted;
        
        private System.Threading.SendOrPostCallback doLogoutOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Authentication() {
            this.Url = global::DAT_BankenLinie_Connector.Properties.Settings.Default.DAT_BankenLinie_Connector_de_dat_www_authentication_Authentication;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event doLoginCompletedEventHandler doLoginCompleted;
        
        /// <remarks/>
        public event doLogoutCompletedEventHandler doLogoutCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("doLogin", RequestNamespace="http://sphinx.dat.de/services/Authentication", ResponseNamespace="http://sphinx.dat.de/services/Authentication", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("sessionID", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string doLogin([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] doLoginRequest request) {
            object[] results = this.Invoke("doLogin", new object[] {
                        request});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void doLoginAsync(doLoginRequest request) {
            this.doLoginAsync(request, null);
        }
        
        /// <remarks/>
        public void doLoginAsync(doLoginRequest request, object userState) {
            if ((this.doLoginOperationCompleted == null)) {
                this.doLoginOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoLoginOperationCompleted);
            }
            this.InvokeAsync("doLogin", new object[] {
                        request}, this.doLoginOperationCompleted, userState);
        }
        
        private void OndoLoginOperationCompleted(object arg) {
            if ((this.doLoginCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doLoginCompleted(this, new doLoginCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("doLogout", RequestNamespace="http://sphinx.dat.de/services/Authentication", ResponseNamespace="http://sphinx.dat.de/services/Authentication", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void doLogout([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] out bool result, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] [System.Xml.Serialization.XmlIgnoreAttribute()] out bool resultSpecified) {
            object[] results = this.Invoke("doLogout", new object[0]);
            result = ((bool)(results[0]));
            resultSpecified = ((bool)(results[1]));
        }
        
        /// <remarks/>
        public void doLogoutAsync() {
            this.doLogoutAsync(null);
        }
        
        /// <remarks/>
        public void doLogoutAsync(object userState) {
            if ((this.doLogoutOperationCompleted == null)) {
                this.doLogoutOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoLogoutOperationCompleted);
            }
            this.InvokeAsync("doLogout", new object[0], this.doLogoutOperationCompleted, userState);
        }
        
        private void OndoLogoutOperationCompleted(object arg) {
            if ((this.doLogoutCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doLogoutCompleted(this, new doLogoutCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://sphinx.dat.de/services/Authentication")]
    public partial class doLoginRequest {
        
        private string customerLoginField;
        
        private string customerNumberField;
        
        private string customerSignatureField;
        
        private string interfacePartnerNumberField;
        
        private string interfacePartnerSignatureField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string customerLogin {
            get {
                return this.customerLoginField;
            }
            set {
                this.customerLoginField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string customerNumber {
            get {
                return this.customerNumberField;
            }
            set {
                this.customerNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string customerSignature {
            get {
                return this.customerSignatureField;
            }
            set {
                this.customerSignatureField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string interfacePartnerNumber {
            get {
                return this.interfacePartnerNumberField;
            }
            set {
                this.interfacePartnerNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string interfacePartnerSignature {
            get {
                return this.interfacePartnerSignatureField;
            }
            set {
                this.interfacePartnerSignatureField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void doLoginCompletedEventHandler(object sender, doLoginCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doLoginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal doLoginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void doLogoutCompletedEventHandler(object sender, doLogoutCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doLogoutCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal doLogoutCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public bool resultSpecified {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[1]));
            }
        }
    }
}

#pragma warning restore 1591