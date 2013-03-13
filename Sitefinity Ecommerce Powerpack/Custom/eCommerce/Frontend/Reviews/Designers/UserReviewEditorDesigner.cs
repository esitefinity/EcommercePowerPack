using System;
using System.Linq;
using System.Web.UI;
using Telerik.Sitefinity.Web.UI;
using Telerik.Sitefinity.Web.UI.ControlDesign;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Pages;
using System.Web.UI.HtmlControls;

namespace SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers
{
    /// <summary>
    /// Represents a designer for the <typeparamref name="SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Controls.UserRewiewEditor"/> widget
    /// </summary>
    public class UserReviewEditorDesigner : ControlDesignerBase
    {
        #region Properties
        /// <summary>
        /// Gets the layout template path
        /// </summary>
        public override string LayoutTemplatePath
        {
            get
            {
                return UserReviewEditorDesigner.layoutTemplatePath;
            }
        }

        /// <summary>
        /// Gets the layout template name
        /// </summary>
        protected override string LayoutTemplateName
        {
            get
            {
                return String.Empty;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }
        #endregion

        #region Control references
        /// <summary>
        /// Gets the page selector control.
        /// </summary>
        /// <value>The page selector control.</value>
        protected internal virtual PagesSelector PageSelectorProductsPage
        {
            get
            {
                return this.Container.GetControl<PagesSelector>("pageSelectorProductsPage", true);
            }
        }

        /// <summary>
        /// Gets the selector tag.
        /// </summary>
        /// <value>The selector tag.</value>
        public HtmlGenericControl SelectorTagProductsPage
        {
            get
            {
                return this.Container.GetControl<HtmlGenericControl>("selectorTagProductsPage", true);
            }
        }

        /// <summary>
        /// Gets the control that is bound to the initialLoad property
        /// </summary>
        protected virtual Control initialLoad
        {
            get
            {
                return this.Container.GetControl<Control>("initialLoad", true);
            }
        }

        #endregion

        #region Methods
        protected override void InitializeControls(Telerik.Sitefinity.Web.UI.GenericContainer container)
        {
            // Place your initialization logic here

            if (this.PropertyEditor != null)
            {
                var uiCulture = this.PropertyEditor.PropertyValuesCulture;
                this.PageSelectorProductsPage.UICulture = uiCulture;
            }
        }
        #endregion

        #region IScriptControl implementation
        /// <summary>
        /// Gets a collection of script descriptors that represent ECMAScript (JavaScript) client components.
        /// </summary>
        public override System.Collections.Generic.IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            var scriptDescriptors = new List<ScriptDescriptor>(base.GetScriptDescriptors());
            var descriptor = (ScriptControlDescriptor)scriptDescriptors.Last();

            descriptor.AddComponentProperty("pageSelectorProductsPage", this.PageSelectorProductsPage.ClientID);
            descriptor.AddElementProperty("selectorTagProductsPage", this.SelectorTagProductsPage.ClientID);
            descriptor.AddElementProperty("initialLoad", this.initialLoad.ClientID);

            return scriptDescriptors;
        }

        /// <summary>
        /// Gets a collection of ScriptReference objects that define script resources that the control requires.
        /// </summary>
        public override System.Collections.Generic.IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            var scripts = new List<ScriptReference>(base.GetScriptReferences());
            scripts.Add(new ScriptReference(UserReviewEditorDesigner.scriptReference));
            return scripts;
        }

        /// <summary>
        /// Gets the required by the control, core library scripts predefined in the <see cref="ScriptRef"/> enum.
        /// </summary>
        protected override ScriptRef GetRequiredCoreScripts()
        {
            return ScriptRef.JQuery | ScriptRef.JQueryUI;
        }
        #endregion

        #region Private members & constants
        public static readonly string layoutTemplatePath = "~/Custom/eCommerce/Frontend/Reviews/Designers/UserReviewEditorDesigner.ascx";
        public const string scriptReference = "~/Custom/eCommerce/Frontend/Reviews/Designers/UserReviewEditorDesigner.js";
        #endregion
    }
}
 
