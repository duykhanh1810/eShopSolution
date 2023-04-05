var CartController = function () {
    this.initialize = function () {
        loadData();

        regsiterEvents();
    }

    function regsiterEvents() {
        // Thêm sản phẩm
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault(); //ngăn ngừa việc load lại trang của thẻ a
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            updateCart(id, quantity);
        });

        //Bớt sản phẩm
        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault(); //ngăn ngừa việc load lại trang của thẻ a
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            updateCart(id, quantity);
        });

        //xóa sản phẩm
        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault(); //ngăn ngừa việc load lại trang của thẻ a
            const id = $(this).data('id');
            updateCart(id, 0);
        });
    }

    function updateCart(id, quantity) {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "POST",
            url: "/" + culture + '/Cart/UpdateCart', //url này phải giống với Endpoint được khai báo trong StartUp
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                $('#lbl_number_items_header').text(res.length);
                loadData(); //thành công thì trả về quantity+1 mà không cần f5 trang web
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function loadData() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListItems',
            success: function (res) {
                if (res.length === 0) { //nếu không có sản phẩm nào thì ẩn giao diện html bên dưới
                    $('#tbl_cart').hide();
                }
                var html = '';
                var total = 0;

                //Lặp lại form với mỗi sản phẩm được thêm vào và tính tổng tiền
                $.each(res, function (i, item) {
                    var amount = item.price * item.quantity;
                    html += "<tr>"
                        + "<td> <img width=\"60\" src=\"" + $('#hidBaseAddress').val() + item.image + "\" alt=\"\" /></td>"
                        + "<td>" + item.description + "</td>"
                        + "<td><div class=\"input-append\"><input class=\"span1\" style=\"max-width: 34px\" placeholder=\"1\" id=\"txt_quantity_" + item.productId + "\" value=\"" + item.quantity + "\" size=\"16\" type=\"text\">"
                        + "<button class=\"btn btn-minus\" data-id=\"" + item.productId + "\" type =\"button\"> <i class=\"icon-minus\"></i></button>"
                        + "<button class=\"btn btn-plus\" type=\"button\" data-id=\"" + item.productId + "\"><i class=\"icon-plus\"></i></button>"
                        + "<button class=\"btn btn-danger btn-remove\" type=\"button\" data-id=\"" + item.productId + "\"><i class=\"icon-remove icon-white\"></i></button>"
                        + "</div>"
                        + "</td>"

                        + "<td>" + numberWithCommas(item.price) + "</td>"
                        + "<td>" + numberWithCommas(amount) + "</td>"
                        + "</tr>";
                    total += amount;
                });
                //Sau khi thêm giao diện thì truy cập đến Cart id
                $('#cart_body').html(html);

                //Đếm có bao nhiêu sản phẩm
                $('#lbl_number_of_item').text(res.length);

                //truyền giá trị total đến thẻ có id tương ứng
                $('#lbl_total').text(numberWithCommas(total));

            },
        });
    }
}

