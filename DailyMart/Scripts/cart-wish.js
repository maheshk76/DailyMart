$(document).ready(function () {
    
$(".addTocartBtn").on("click", function () {
    var productId = parseInt($(this).attr("data-id"));
    var quantity = parseInt($(this).attr("data-quantity"));
    $.ajax({
        url: '/Shared/AddToCart/',

        data: {
            ProductId: productId,
            quantity: quantity
        }
    })
        .done(function (response) {
            console.log(response);
            if (response.Success) {
                $("#cartProductsCount").html(response.CartLength);

                swal("Done", "Product Added To Cart", "success");
            }
            else {
                window.location.href = "/Account/Login";
            }
        })
        .fail(function (XMLHttpRequest, textStatus, errorThrown) {
            swal("Error", "Unable to add product to cart", "error");
        });
});

$(".addTowish").on("click", function () {
    var productId = parseInt($(this).attr("data-id"));
    $.ajax({
        url: '/Shared/MakeWishlist/',

        data: {
            ProductId: productId,
            act: 1
        }
    })
        .done(function (response) {
            console.log(response);
            if (response.Success) {
                swal({
                    icon: 'success',
                    title: 'Done',
                    text: response.Message,
                    timer: 1500,
                    buttons: {
                        OK: "OK",
                    },
                }).then((value) => {
                    switch (value) {
                        case "OK":
                            //refreshProduct();
                            //window.location.reload();
                            break;
                    }
                });

                setTimeout(function () { refreshProduct(); /*window.location.reload()*/ }, 1500);
            }
            else {
                window.location.href = "/Account/Login";
            }
        })
        .fail(function (XMLHttpRequest, textStatus, errorThrown) {
            swal("Error","FAIL to Add Product to wishlist","error");
        });
});
$(".removefromwish").on("click", function () {
    var productId = parseInt($(this).attr("data-id"));

    $.ajax({
        url: '/Shared/MakeWishlist/',
        data: {
            ProductId: productId,
            act: 0
        }

    }).done(function (response) {
        if (response.Success) {
            swal({
                icon: 'success',
                title: 'Done',
                text: response.Message,
                timer: 1500,
                buttons: {
                    OK: "OK",
                },
            }).then((value) => {
                switch (value) {
                    case "OK":
                        //refreshProduct();
                        break;
                }
            });

            setTimeout(function () { refreshProduct(); }, 1500);
        }
    })
        .fail(function (XMLHttpRequest, textStatus, errorThrown) {
            swal("Error","FAIL to Remove from wishlist","error");
        });
});
    function refreshProduct() {
        showLoader();
        $.ajax({
            url: '/Home/Products/',
            data: {
                search: $("#SearchTerm").val(),
                sortBy: $("#SortBy").val(),
                categoryID: $("#CategoryID").val()
            }
        })
            .done(function (response) {
                $("#productsDiv").html(response);
            })
            .fail(function (XMLHttpRequest, textStatus, errorThrown) {
                alert("FAIL");
            })
            .always(function () {
                console.log("Refreshing Product list");
                hideLoader();
            });
    }
});