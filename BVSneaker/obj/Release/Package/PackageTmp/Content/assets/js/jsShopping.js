        $(document).ready(function () {
            showCount();
            $('body').on('click', '.btnAddToCart', function (e) {
                e.preventDefault();
                var id = $(this).data('id');
                var quatity = 1;
                var price = 0;
                var tQuantity = $('#quantity_value').text();
                var tPrice = $('#price_value').text();
                var tStock = $('.size_active > span').text();
                if (tQuantity != '') {

                    quatity = parseInt(tQuantity);
                }
                if (tPrice != '') {
                    price = parseInt(tPrice);
                }
                if (tStock != '') {
                    stock = parseInt(tStock);
                }



                $.ajax({
                    url: '/shoppingcart/addtocart',
                    type: 'POST',
                    data: { id: id, quantity: quatity, price: price, stock: stock },
                    success: function (rs) {
                        if (rs.Success) {
                            $('#checkout_items').html(rs.count);
                            toastr.optionsOverride = 'positionclass = "toast-bottom-full-width"';
                            toastr.options.positionClass = 'toast-bottom-right';
                            toastr.success(rs.msg);
                        }
                    }   
                });
            });






            $('body').on('click', '.btnDelete', function (e) {
                e.preventDefault();
                var id = $(this).data('id');
                toastr.warning("<br /><button type='button' id='confirmationButtonYes' class='btn clear' style='margin:0px; float:right;'>Yes</button>", 'Do you want to remove all items in bag?',
                    {
                        closeButton: false,
                        allowHtml: true,
                        onShown: function (toast) {
                            $("#confirmationButtonYes").click(function () {
                                $.ajax({
                                    url: '/shoppingcart/delete',
                                    type: 'POST',
                                    data: { id: id},
                                    success: function (rs) {
                                        if (rs.Success) {
                                            $('#checkout_items').html(rs.count);
                                            $('#trow_' + id).remove();
                                            LoadCart();
                                        }
                                    }
                                })
                            });
                        }
                    });
            });



            $('body').on('click', '.btnDeleteAll', function (e) {
                e.preventDefault();
                toastr.warning("<br /><button type='button' id='confirmationButtonYes' class='btn clear' style='margin:0px; float:right;'>Yes</button>", 'Do you want to remove all items in bag?',
                    {
                        closeButton: false,
                        allowHtml: true,
                        onShown: function (toast) {
                            $("#confirmationButtonYes").click(function () {
                                DeleteAll();
                            });
                        }
                    });
            });




            $('body').on('click', '.btnUpdate', function (e) {
                e.preventDefault();
                var id = $(this).data("id");
                var quantity = $('#Quantity_' + id).val();
                var stock = $('#Stock_' + id).text();
                Update(id, quantity, stock);
                toastr.optionsOverride = 'positionclass = "toast-bottom-full-width"';
                toastr.options.positionClass = 'toast-bottom-right';
                toastr.success('Item updated');
            });
        });




        function showCount() {
            $.ajax({
                url: '/shoppingcart/showcount',
                type: 'GET',
                success: function (rs) {
                    $('#checkout_items').html(rs.count);
                }
            });
        };



        function DeleteAll() {
            $.ajax({
                url: '/shoppingcart/DeleteAll',
                type: 'POST',
                success: function (rs) {
                    if (rs.Success){
                        LoadCart();
                    }
                }
            })
        }



        function Update(id, quantity, stock) {
            $.ajax({
                url: '/shoppingcart/Update',
                type: 'POST',
                data: { id: id, quantity: quantity, stock: stock},
                success: function (rs) {
                    if (rs.Success) {
                        LoadCart();
                    }
                }
            })
        }




        function LoadCart() {
            $.ajax({
                url: '/shoppingcart/Partial_Item_Cart',
                type: 'GET',
                success: function (rs) {
                    $('#load_data').html(rs);
                }
            });
        }