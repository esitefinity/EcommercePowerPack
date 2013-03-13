var masterGridView;

var reviewsForProductBound = false;

// called by the MasterGridView when it is loaded
function OnModuleMasterViewLoadedRatings(sender, args) {

    masterGridView = sender;
    $('head').append($('<link rel="stylesheet" type="text/css" />').attr('href', '/Custom/eCommerce/Backend/Resources/ProductsRatingStar.css'));
    var binder = sender.get_binder();
        binder.add_onDataBound(handleBinderDataBoundRatings);
        binder.add_onItemDataBound(handleBinderItemDataBoundRatings);
    
    
}

function handleBinderDataBoundRatings(sender, args) {

    if (reviewsForProductBound != true) {

        debugger;
        var reviewByProduct = getURLParameter("reviewByProduct");
       
        if (reviewByProduct.length > 4) {
            masterGridView.get_binder().get_urlParams()['filter'] = "Product == (" + reviewByProduct + ")";
            masterGridView.get_binder().set_isFiltering(true);
            reviewsForProductBound = true;
            masterGridView.get_binder().DataBind();
            
        }
        
    }
}

function handleBinderItemDataBoundRatings(sender, args) {
    var dataItem = args.get_dataItem();

    //display the rating
    var rating = dataItem.Rating;
    var ratingsContainer = $(args.get_itemElement()).find(".starRating .starRatingInner");
    ratingsContainer.attr("style", "width:" + (20 * rating) + "px");

    var request = $.ajax({
        url: "/Sitefinity/Services/Ecommerce/Catalog/ProductService.svc/" + dataItem.Product + "/",
        type: "GET",
        dataType: "json",
        async: false
    });
    request.done(function (json) {

        var productsLink = $(args.get_itemElement()).find(".sfProductsLink");

        productsLink.html(json.Item.Title.Value);
        productsLink.append("<div><img src=\"" + json.Item.Thumbnail.Url + "\" width=80px/></div>");
        
    });

    request.fail(function (jqXHR, textStatus) {
        //todo
    });

}

function getURLParameter(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
        );
}