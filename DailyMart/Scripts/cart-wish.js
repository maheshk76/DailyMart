$(document).ready(function () {
$(".addTocartBtn").on("click", function () {
    var productId = parseInt($(this).attr("data-id"));
    $.ajax({
        url: 'Shared/AddToCart/',

        data: {
            ProductId: productId,
            quantity: 1
        }
    })
        .done(function (response) {
            console.log(response);
            if (response.Success) {
                $("#cartProductsCount").html(response.CartLength);
            }
            swal("Done", "Product Added To Cart", "success");
        })
        .fail(function (XMLHttpRequest, textStatus, errorThrown) {
            alert("FAIL to Add Product In Cart");
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
                            window.location.reload();
                            break;
                    }
                });
            }
            setTimeout(function () { window.location.reload() }, 2500);
        })
        .fail(function (XMLHttpRequest, textStatus, errorThrown) {
            alert("FAIL to Add Product to wishlist");
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
                        window.location.reload();
                        break;
                }
            });
        }
        setTimeout(function () { window.location.reload() }, 2500);
    })
        .fail(function (XMLHttpRequest, textStatus, errorThrown) {
            alert("FAIL to Remove from wishlist");
        });
});

});