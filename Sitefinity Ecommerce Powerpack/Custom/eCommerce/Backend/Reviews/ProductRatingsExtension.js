var masterGridView;
debugger;
function handleBinderItemDataBoundRatings(sender, args) {
    var dataItem = args.get_dataItem();
    var request = $.ajax({
        url: "/Sitefinity/Services/DynamicModules/Data.svc/?managerType=Telerik.Sitefinity.DynamicModules.DynamicModuleManager&providerName=OpenAccessProvider&itemType=Telerik.Sitefinity.DynamicTypes.Model.RatingsandReviews.RatingAndReview&provider=OpenAccessProvider&sortExpression=LastModified%20DESC&skip=0&take=50&filter=Product%20%3D%3D%20(" + dataItem.Id + ")",
        type: "GET",
        dataType: "json"
    });

    request.done(function (json) {
        var total = 0;
        for (var i = 0; i < json.TotalCount; i++) {
            total = total + parseInt(json.Items[i].Rating);
        }
        var avg
        if (json.TotalCount > 0)
            avg = total / json.TotalCount;
        else
            avg = 0;

        var ratingsContainer = $(args.get_itemElement()).find(".starRating .starRatingInner");
        var reviewsLink = $(args.get_itemElement()).find(".starRating .starReviewLink");
        ratingsContainer.attr("style", "width:" + (20 * avg) + "px");
        reviewsLink.html(json.TotalCount + " reviews");
        reviewsLink.attr("href", "/Sitefinity/Content/ratings-and-reviews/?reviewByProduct=" + dataItem.Id);

    });

    request.fail(function (jqXHR, textStatus) {
        //todo
    });

}

function productsMaster_RatingsViewLoaded(_masterGridView) {
    debugger;
    $('head').append($('<link rel="stylesheet" type="text/css" />').attr('href', '/Custom/eCommerce/Backend/Resources/ProductsRatingStar.css'));

    var binder = masterGridView.get_binder();
    binder.add_onItemDataBound(handleBinderItemDataBoundRatings);
}
