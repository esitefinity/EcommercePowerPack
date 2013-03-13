using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Web.UI;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog;

namespace SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Controls
{
    [Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesigner(typeof(SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.UserReviewEditorDesigner))]
    public partial class UserRewiewEditor : System.Web.UI.UserControl
    {
        public Guid ProductsPage { get; set; }
        public int initialLoad { get; set; }

        private string username;
        private bool authenticated;
        IQueryable<DynamicContent> source;

        protected void Page_Init(object sender, EventArgs e)
        {
            //listReviews.ItemCommand += new EventHandler<ListViewCommandEventArgs>(listReviews_ItemCommand);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            authenticated = HttpContext.Current.User.Identity.IsAuthenticated;
            if (authenticated)
            {
                username = SecurityManager.GetCurrentUserName();
                source = GetReviewsByUser(username);
                listReviews.DataSource = source.Take(initialLoad);
                listReviews.DataBind();
                if (!IsPostBack)
                {
                    hdLoadCount.Value = initialLoad.ToString();
                }
                if (int.Parse(hdLoadCount.Value) >= source.Count())
                {
                    btnLoadMoreReviews.Visible = false;
                }
            }
            else
            {
                SUP.Visible = false;
            }
        }

        public IQueryable<DynamicContent> GetReviewsByUser(string user)
        {
            // Set the provider name for the DynamicModuleManager here. All available providers are listed in
            // Administration -> Settings -> Advanced -> DynamicModules -> Providers
            var providerName = String.Empty;

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
            Type ratingAndReviewType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.RatingsandReviews.RatingAndReview");

            // This is how we get the ratingAndReview items through filtering
            var myFilteredCollection = dynamicModuleManager.GetDataItems(ratingAndReviewType).Where(p => p.GetValue<string>("Username") == user && p.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live).OrderByDescending(p => p.GetValue<DateTime>("Date"));

            return myFilteredCollection;
        }
        public bool DeleteRatingAndReview(Guid id)
        {
            try
            {

                // Set the provider name for the DynamicModuleManager here. All available providers are listed in
                // Administration -> Settings -> Advanced -> DynamicModules -> Providers
                var providerName = "OpenAccessProvider";
                DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
                Type ratingAndReviewType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.RatingsandReviews.RatingAndReview");
                DynamicContent ratingAndReviewItem = dynamicModuleManager.GetItem(ratingAndReviewType, id) as DynamicContent;
                // This is how you delete the ratingAndReviewItem
                var ratingandReviewMaster = dynamicModuleManager.GetItem(ratingAndReviewType, ratingAndReviewItem.OriginalContentId) as DynamicContent;
                dynamicModuleManager.DeleteDataItem(ratingandReviewMaster);
                // You need to call SaveChanges() in order for the items to be actually persisted to data store
                dynamicModuleManager.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void listReviews_ItemDataBound1(object sender, Telerik.Web.UI.RadListViewItemEventArgs e)
        {
            HyperLink lnk = e.Item.FindControl("lnkProduct") as HyperLink;
            var pm = PageManager.GetManager();
            RadBinaryImage img = e.Item.FindControl("imgProduct") as RadBinaryImage;
            var cm = CatalogManager.GetManager();
            var product = cm.GetProduct(Guid.Parse(img.AlternateText));
            img.AlternateText = product.Title;
            if (product.Images.Count() > 0)
            {
                img.ImageUrl = product.Images.FirstOrDefault().ThumbnailUrl;
            }
            if (ProductsPage != null && ProductsPage != Guid.Empty)
            {
                
                var node = pm.GetPageNode(ProductsPage);
                lnk.NavigateUrl = node.GetUrl() + "/" + product.UrlName;
            }
            Label lblProduct = e.Item.FindControl("lblProductName") as Label;
            lblProduct.Text = product.Title;
           
        }

        protected void listReviews_ItemCommand1(object sender, Telerik.Web.UI.RadListViewCommandEventArgs e)
        {
            Guid killReview = Guid.Parse(e.CommandArgument.ToString());
            if (DeleteRatingAndReview(killReview))
            {
                source = GetReviewsByUser(username);
                listReviews.DataSource = source;
                listReviews.DataBind();
                pnlMessage.Visible = true;
                pnlMessage.CssClass = "sfReviewDeleteSuccess";
            }
        }

        protected void btnLoadMoreReviews_Click(object sender, EventArgs e)
        {
            hdLoadCount.Value = (int.Parse(hdLoadCount.Value) + 4).ToString();
            listReviews.DataSource = source.Take(int.Parse(hdLoadCount.Value));
            listReviews.DataBind();
            if (source.Count() <= int.Parse(hdLoadCount.Value))
            {
                btnLoadMoreReviews.Visible = false;
            }
        }


    }
}