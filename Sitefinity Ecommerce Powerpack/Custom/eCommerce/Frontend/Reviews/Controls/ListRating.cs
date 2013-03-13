using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog;
using Telerik.Web.UI;
using Telerik.Sitefinity.Web.UI.ContentUI;

namespace SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Controls
{
    public class ListRating : SimpleView
    {
        #region Public Controls
        public Guid Product { get; set; }
        public DetailsViewHyperLink lblGoToReviews
        { 
            get
            {
                return this.Container.GetControl<DetailsViewHyperLink>("lblGoToReviews", true);
            }
        }

        public RadRating rating
        {
            get
            {
                return this.Container.GetControl<RadRating>("rating", true);
            }
        }
        #endregion

        #region Public Methods
        protected override void InitializeControls(GenericContainer container)
        {
            //string productStringId = Product.ToString();
            //Guid productId = Guid.Parse(Product);

            var cm = CatalogManager.GetManager();
            var dm = DynamicModuleManager.GetManager();
            IQueryable<DynamicContent> source = GetReviewsById(Product);
            lblGoToReviews.Text = source.Count() + " reviews";
            rating.Value = totalRating(source);
        }
        public override string LayoutTemplatePath
        {
            get
            {
                return "~/Custom/eCommerce/Frontend/Reviews/Controls/ListRating.ascx";
            }
            set
            {
                base.LayoutTemplatePath = value;
            }
        }

        //get the reviews for the current product by matching the determined product ID with the product ID stored in the ratings module
        public IQueryable<DynamicContent> GetReviewsById(Guid id)
        {
            // Set the provider name for the DynamicModuleManager here. All available providers are listed in
            // Administration -> Settings -> Advanced -> DynamicModules -> Providers
            var providerName = String.Empty;

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
            Type ratingAndReviewType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.RatingsandReviews.RatingAndReview");

            // This is how we get the ratingAndReview items through filtering
            var myFilteredCollection = dynamicModuleManager.GetDataItems(ratingAndReviewType).Where(p => p.GetValue<Guid>("Product") == id && p.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live);

            return myFilteredCollection;
        }

        private decimal totalRating(IQueryable<DynamicContent> source)
        {
            decimal rating = 0;
            foreach (DynamicContent content in source)
            {
                rating = rating + content.GetValue<decimal>("Rating");
            }
            int total = source.Count();
            if (total > 0)
            {
                rating = rating / total;
            }
            return rating;
        }
        #endregion
    }
}