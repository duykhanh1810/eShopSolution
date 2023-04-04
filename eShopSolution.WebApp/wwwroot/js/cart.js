var CartController = function () {
    this.initialize = function () {
        loadData();
    }

    function loadData() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListItems',
            success: function (res) {
                var html = '';
                var total = 0;

                //Lặp lại form với mỗi sản phẩm được thêm vào và tính tổng tiền
                $.each(res, function (i, item) {
                    var amount = item.price * item.quantity;
                    html += "<tr>"
                        + "<td> <img width=\"60\" src=\"" + $('#hidBaseAddress').val() + item.image + "\" alt=\"\" /></td>"
                        + "<td>" + item.description + "</td>"
                        + "<td><div class=\"input - append\"><input class=\"span1\" style=\"max-width: 34px\" placeholder=\"1\" id=\"appendedInputButtons\" size=\"16\" type=\"text\">"
                        + "<button class=\"btn\" type =\"button\"> <i class=\"icon-minus\"></i></button>"
                        + "<button class=\"btn\" type=\"button\"><i class=\"icon-plus\"></i></button>"
                        + "<button class=\"btn btn-danger\" type=\"button\"><i class=\"icon-remove icon-white\"></i></button>"
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

