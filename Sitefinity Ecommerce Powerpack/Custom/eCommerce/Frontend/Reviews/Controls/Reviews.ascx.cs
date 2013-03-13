using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Ecommerce.Catalog.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Ecommerce.Catalog;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Web.UI.PublicControls;
using Telerik.Sitefinity.Lifecycle;
using Telerik.Sitefinity;
using System.Threading;
using System.Globalization;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Modules.Ecommerce.Orders;
using Telerik.Sitefinity.Ecommerce.Orders.Model;
using Telerik.Web.UI;
using Telerik.Sitefinity.Modules.Pages;

namespace SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Controls
{
    [Telerik.Sitefinity.Web.UI.ControlDesign.ControlDesigner(typeof(SitefinityWebApp.Custom.eCommerce.Frontend.Reviews.Designers.ReviewsDesigner))]
    public partial class Reviews : System.Web.UI.UserControl
    {
        //publically exposed values for user to set
        public bool requirePurchase { get; set; }
        public Guid profilePage { get; set; }
        public bool requireApproval { get; set; }
        public string SuccessMessage { get; set; }
        public bool rateOnlyOnce { get; set; }
        public int showCount { get; set; }
        //private values
        private string username;
        private Product product;
        CatalogManager cm = CatalogManager.GetManager();
        IQueryable<DynamicContent> source;
        private bool verifiedPurchase;
        private int counter;
        private bool userReviewed;
        private bool isValid;
        private string url;
        private bool authenticated;
        private Guid userid;
        Customer currentCustomer;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            SUP.Load += SUP_Load;
            btnLoadMore.Click += btnLoadMore_Click;
        }

        void SUP_Load(object sender, EventArgs e)
        {
            //check if user is authenticated
            authenticated = HttpContext.Current.User.Identity.IsAuthenticated;
            btnShowReview.Visible = authenticated;
            //get the current user's username
            username = SecurityManager.GetCurrentUserName();
            userid = SecurityManager.GetCurrentUserId();
            //determine the url name to be compared for product declaration
            url = System.IO.Path.GetFileName(HttpContext.Current.Request.Url.AbsolutePath);
            //execute loading logic if the current page url is of a valid product. This also sets the product object
            isValid = isValidProduct(url);
            if (isValid)
            {
                //get the customer based off the current user ID
                if (userid != null && userid != Guid.Empty)
                {
                    currentCustomer = GetCustomerByUser(userid);
                }
                //start verifying whether the current user (customer) has purchased this product before.
                if (currentCustomer != null)
                {
                    verifiedPurchase = wasPurchased(product.Id, currentCustomer.Id);
                }
                //get the reviews based on the product id
                source = GetReviewsById(product.Id);
                //count the reviews for this product
                int count = source.Count();
                //determine of the current user has reviewed this product before
                userReviewed = alreadyReviewed(source, username);
                //set the labels with the appropriate values
                setLabels(username, userReviewed, count);
                //get the average rating of the product and set it to the RadRating control
                decimal avg = totalRating(source);
                ratingOverview.Value = avg;
                //get the current user's avatar and bind the RadListView with the comments for this product
                imgCurrentAvatar.DataValue = GetImage(username);
                //set the data source
                listReviews.DataSource = source.Take(showCount);
                //we'll bind it here if it's not a postback. Postback bindings should be handled by their controls.
                if (!IsPostBack)
                {
                    hdCount.Value = showCount.ToString();
                    listReviews.DataBind();
                }
                //hide or show the load button
                btnLoadMore.Visible = (showCount < count);
            }
            //if the product is not visible then we hide the controls and stop here
            else
            {
                SUP.Visible = false;
            }
        }

        //Set the title label with the appropriate values.
        public void setLabels(string Username, bool reviewed, int count)
        {

            if (!authenticated)
            {
                lblReviewed.Text = "You must be logged in to write reviews";
                lblReviewed.Visible = true;
            }
            else
            {

                if (requirePurchase && !verifiedPurchase)
                {
                    lblReviewed.Text = "You must have purchased this product in order to review it";
                    lblReviewed.Visible = true;
                    btnShowReview.Visible = false;
                }
                string you = string.Empty;
                //if the current user reviewed the product prefix the label with the proper value
                if (reviewed)
                {
                    you = "You & ";
                    count = count - 1;
                    //if Rate Only Once option is set hide the show review button
                    if (rateOnlyOnce)
                    {
                        btnShowReview.Visible = false;
                        lblReviewed.Visible = true;
                    }
                }
                if (count == 1)
                {
                    lbl.Text = string.Format("{0}{1} person reviewed this product", you, count);
                }
                else if (count > 1)
                {
                    lbl.Text = string.Format("{0}{1} people have reviewed this product", you, count);
                }
                else if (count <= 0)
                {
                    if (you == string.Empty)
                    {
                        lbl.Text = "Be the first to review this product";
                    }
                    else
                    {
                        lbl.Text = "Only you have reviewed this product";
                    }
                }
            }
            
        }

        //check whether or not the current user has written a review for this product
        public bool alreadyReviewed(IQueryable<DynamicContent> source, string currentUser)
        {
            IQueryable<DynamicContent> checkSource = source.Where(p => p.GetValue<string>("Username") == currentUser);
            if (checkSource.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //check if the parsed url returns an actual product in the system based on the product URL name
        public bool isValidProduct(string url)
        {
            product = cm.GetProducts().Where(p => p.UrlName == url).FirstOrDefault();
            if (product != null)
            {
                return true;
            }
            else
            {
                return false;
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
            var myFilteredCollection = dynamicModuleManager.GetDataItems(ratingAndReviewType).Where(p => p.GetValue<Guid>("Product") == id && p.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live).OrderByDescending(p => p.GetValue<DateTime>("Date"));

            return myFilteredCollection;
        }

        //get the total rating - used to calculate the average score
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

        //set the success or error message
        private void SetMessage(string css, bool AddReview, bool ShowReview, bool Response, bool Reviewed, string message)
        {
            lblErrorOrSuccess.Text = message;
            response.CssClass = css;
            addReview.Visible = AddReview;
            btnShowReview.Visible = ShowReview;
            response.Visible = Response;
            lblReviewed.Visible = Reviewed;
        }

        //commit this logic when a new review is submitted.
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //validate the request to submit a review
                bool passVerifyPurchase = (requirePurchase == true && verifiedPurchase == true || requirePurchase == false);
                bool passVerifyRateOnce = (rateOnlyOnce == true && userReviewed == false || rateOnlyOnce == false);
                if (passVerifyPurchase && passVerifyRateOnce && authenticated)
                {
                    CreateRatingAndReview(username, txtTitle.Text, txtComment.Text, (int)addRate.Value, verifiedPurchase, product);
                    setLabels(username, true, source.Count());
                }
                else
                {
                    if (verifiedPurchase != requirePurchase)
                    {
                        SetMessage("responseError", false, false, true, false, "You must have purchased this product in order to review it.");
                    }
                    else if (!authenticated)
                    {
                        SetMessage("responseError", false, false, true, false, "You must be logged in to review products.");
                    }
                    else
                    {
                        SetMessage("responseError", false, false, true, false, "You have already reviewed this product.");
                    }
                    setLabels(username, false, source.Count());
                }
                decimal avg = totalRating(source);
                ratingOverview.Value = avg;
                listReviews.DataBind();
            }
            catch
            {
                SetMessage("responseError", false, false, true, false, "Something went wrong while submitting your review.");
            }
            
        }

        //create the rating and review
        public void CreateRatingAndReview(string username, string title, string thought, int rating, bool verified, Product product)
        {
            // Set the provider name for the DynamicModuleManager here. All available providers are listed in
            // Administration -> Settings -> Advanced -> DynamicModules -> Providers
            var providerName = "OpenAccessProvider";

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
            Type ratingAndReviewType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.RatingsandReviews.RatingAndReview");
            DynamicContent ratingAndReviewItem = dynamicModuleManager.CreateDataItem(ratingAndReviewType);
            string cultureName = CultureInfo.CurrentCulture.Name;
            // This is how values for the properties are set
            ratingAndReviewItem.SetValue("Username", username);
            ratingAndReviewItem.SetValue("FoundHelpful", new Guid[]{ Guid.Empty});
            ratingAndReviewItem.SetValue("Thoughts", thought);
            ratingAndReviewItem.SetValue("Rating", rating);
            ratingAndReviewItem.SetValue("Title", title);
            ratingAndReviewItem.SetValue("VerifiedBuyer", verified);
            ratingAndReviewItem.SetValue("Url", product.UrlName);
            ratingAndReviewItem.SetValue("Date", DateTime.Now);
            ratingAndReviewItem.SetString("UrlName", username + DateTime.Now.Ticks);
            ratingAndReviewItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
            ratingAndReviewItem.SetValue("Product", product.Id);
            ratingAndReviewItem.SetValue("PublicationDate", DateTime.Now);
            if (!requireApproval)
            {
                ratingAndReviewItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Published");
                dynamicModuleManager.Lifecycle.Publish(ratingAndReviewItem, new CultureInfo(cultureName));
                SetMessage("responseSuccess", false, false, true, true, SuccessMessage);
            }
            else
            {
                ratingAndReviewItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "AwaitingApproval");
                SetMessage("responseApproval", false, false, true, true, "Thank you. Your review must be approved before it appears here.");
            }
            dynamicModuleManager.SaveChanges();
        }

        //get the commenter's avatar and set it per item
        protected void listReviews_ItemDataBound(object sender, Telerik.Web.UI.RadListViewItemEventArgs e)
        {
            RadBinaryImage img = (RadBinaryImage)e.Item.FindControl("imgAvatar");
            HyperLink userLink = (HyperLink)e.Item.FindControl("lnkReviewer");
            PageManager pm = PageManager.GetManager();
            if (profilePage != null && profilePage != Guid.Empty)
            {
                userLink.NavigateUrl = pm.GetPageNode(profilePage).GetFullUrl() + "/" +  userLink.Text; 
            }
            
            img.DataValue = GetImage(userLink.Text);
        }

        //returns the URL of the given user's avatar
        protected byte[] GetImage(string username)
        {
            //grab our managers
            UserManager userManager = UserManager.GetManager();
            UserProfileManager profileManager = UserProfileManager.GetManager();
            LibrariesManager librariesManager = LibrariesManager.GetManager();
            User user = userManager.GetUser(username);
            
            
            //find the avatar and return it. null is ok it will use the default profile picture of the RadBinaryImage control
            if (user != null)
            {
                SitefinityProfile profiler = profileManager.GetUserProfile<SitefinityProfile>(user);
                if (profiler.Avatar != null)
                {
                    Telerik.Sitefinity.Libraries.Model.Image image = librariesManager.GetImage(profiler.Avatar.ChildItemId);
                    return image.Thumbnail.Data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //get the customer by user id
        public static Customer GetCustomerByUser(Guid userId)
        {
            OrdersManager ordersManager = OrdersManager.GetManager();
            UserProfileManager userProfileManager = UserProfileManager.GetManager();
            UserManager userManager = UserManager.GetManager();

            User user = userManager.GetUser(userId);

            SitefinityProfile profile = userProfileManager.GetUserProfile<SitefinityProfile>(user);

            Customer customer = ordersManager.GetCustomers().SingleOrDefault(c => c.UserProfileId == profile.Id);

            return customer;
        }

        //Checks whether or not the provided customer purchased the provided product
        public static bool wasPurchased(Guid productId, Guid customerId)
        {
            OrdersManager ordersManager = OrdersManager.GetManager();

            IQueryable<Order> orders = ordersManager.GetOrders().Where(o => o.Details.Where(d => d.ProductId == productId).Count() > 0 && o.Customer.Id == customerId);

            if (orders.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //set the filter based on rating
        protected void comboFilter_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            IQueryable<DynamicContent> sortSource = GetReviewsById(product.Id);
            hdCount.Value = showCount.ToString();
            if (e.Value != "0")
            {
                
                sortSource = sortSource.Where(p => p.GetValue<decimal>("Rating") == decimal.Parse(e.Value));
                listReviews.DataSource = sortSource.Take(showCount);
                listReviews.DataBind();
            }
            else
            {
                sortSource = sortSource;
                listReviews.DataSource = sortSource.Take(showCount);
                listReviews.DataBind();
            }
            btnLoadMore.Visible = (showCount < sortSource.Count());
        }

        protected void btnLoadMore_Click(object sender, EventArgs e)
        {
            counter = int.Parse(hdCount.Value) + 4;
            hdCount.Value = counter.ToString();
            IQueryable<DynamicContent> temp = source;
            if (comboFilter.SelectedItem.Value != "0")
            {
                temp = temp.Where(p => p.GetValue<decimal>("Rating") == decimal.Parse(comboFilter.SelectedItem.Value));
            }
            else
            {
                temp = source;
            }
            if (counter < temp.Count())
            {
                listReviews.DataSource = temp.Take(counter);
            }
            else
            {
                listReviews.DataSource = temp;
            }
            
            listReviews.DataBind();
            btnLoadMore.Visible = (counter < temp.Count());
        }

    }
}