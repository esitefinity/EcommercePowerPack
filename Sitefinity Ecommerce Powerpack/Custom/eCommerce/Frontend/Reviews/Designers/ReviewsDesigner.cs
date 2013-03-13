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
    /// Represents a designer for the <typeparamref name="SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Controls.Reviews"/> widget
    /// </summary>
    public class ReviewsDesigner : ControlDesignerBase
    {
        #region Properties
        /// <summary>
        /// Gets the layout template path
        /// </summary>
        public override string LayoutTemplatePath
        {
            get
            {
                return ReviewsDesigner.layoutTemplatePath;
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
        protected internal virtual PagesSelector PageSelectorprofilePage
        {
            get
            {
                return this.Container.GetControl<PagesSelector>("pageSelectorprofilePage", true);
            }
        }

        /// <summary>
        /// Gets the selector tag.
        /// </summary>
        /// <value>The selector tag.</value>
        public HtmlGenericControl SelectorTagprofilePage
        {
            get
            {
                return this.Container.GetControl<HtmlGenericControl>("selectorTagprofilePage", true);
            }
        }

        /// <summary>
        /// Gets the control that is bound to the SuccessMessage property
        /// </summary>
        protected virtual Control SuccessMessage
        {
            get
            {
                return this.Container.GetControl<Control>("SuccessMessage", true);
            }
        }

        /// <summary>
        /// Gets the control that is bound to the showCount property
        /// </summary>
        protected virtual Control showCount
        {
            get
            {
                return this.Container.GetControl<Control>("showCount", true);
            }
        }

        /// <summary>
        /// Gets the control that is bound to the requireApproval property
        /// </summary>
        protected virtual Control requireApproval
        {
            get
            {
                return this.Container.GetControl<Control>("requireApproval", true);
            }
        }

        /// <summary>
        /// Gets the control that is bound to the requirePurchase property
        /// </summary>
        protected virtual Control requirePurchase
        {
            get
            {
                return this.Container.GetControl<Control>("requirePurchase", true);
            }
        }

        /// <summary>
        /// Gets the control that is bound to the rateOnlyOnce property
        /// </summary>
        protected virtual Control rateOnlyOnce
        {
            get
            {
                return this.Container.GetControl<Control>("rateOnlyOnce", true);
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
                this.PageSelectorprofilePage.UICulture = uiCulture;
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

            descriptor.AddComponentProperty("pageSelectorprofilePage", this.PageSelectorprofilePage.ClientID);
            descriptor.AddElementProperty("selectorTagprofilePage", this.SelectorTagprofilePage.ClientID);
            descriptor.AddElementProperty("successMessage", this.SuccessMessage.ClientID);
            descriptor.AddElementProperty("showCount", this.showCount.ClientID);
            descriptor.AddElementProperty("requireApproval", this.requireApproval.ClientID);
            descriptor.AddElementProperty("requirePurchase", this.requirePurchase.ClientID);
            descriptor.AddElementProperty("rateOnlyOnce", this.rateOnlyOnce.ClientID);

            return scriptDescriptors;
        }

        /// <summary>
        /// Gets a collection of ScriptReference objects that define script resources that the control requires.
        /// </summary>
        public override System.Collections.Generic.IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            var scripts = new List<ScriptReference>(base.GetScriptReferences());
            scripts.Add(new ScriptReference(ReviewsDesigner.scriptReference));
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
        public static readonly string layoutTemplatePath = "~/Custom/eCommerce/Frontend/Reviews/Designers/ReviewsDesigner.ascx";
        public const string scriptReference = "~/Custom/eCommerce/Frontend/Reviews/Designers/ReviewsDesigner.js";
        #endregion
    }
}
 
