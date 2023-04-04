// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('body').on('click', '.btn-add-cart', function (e) {
    //câu lệnh có nghĩa là click vào bất cứ chỗ nào trong body, nó sẽ binding sự kiện click đó vào nút add cart

    e.preventDefault(); //ngăn ngừa việc load lại trang của thẻ a

    const culture = $('#hidCulture').val(); //lấy ra languageId
    const id = $(this).data('id');  //Lấy ra id
    //alert(id);

    $.ajax({
        type: "POST",
        dataType: 'json',
        url: '/'+culture + '/Cart/AddToCart', //url này phải giống với Endpoint được khai báo trong StartUp
        data: {
            id: id,
            languageId: culture
        },
        success: function (res) {
            console.log(res)
        },
        error: function (err) {
            console.log(err);
        }
    });
})