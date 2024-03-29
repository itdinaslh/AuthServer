function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    showSuccessMessage();
                } else if (result.invalid) {
                    showInvalidMessage();
                } else if (result.failed) {
                    showFailedMessage();
                } else {
                    $('#myModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}

function showSuccessMessage() {
    swal({
        position: 'top-end',
        type: 'error',
        title: 'Data berhasil disimpan!',
        showConfirmButton: false,
        timer: 1000
    }).then(function () {
        loadContent();
    });
}

function showFailedMessage() {
    swal({
        position: 'center',
        type: 'warning',
        title: 'Data Gagal Disimpan',
        showConfirmButton: false
    });
}

function showInvalidMessage() {
    $('.mod-warning').css("visibility", "visible");
}

$(document).on('shown.bs.modal', function () {
    $(this).find('[autofocus]').focus();
});

$(document).on('click', '.showMe', function () {
    $('#myModalContent').load($(this).attr('data-href'), function () {

        $('#myModal').modal();

        bindForm(this);
    });

    return false;
});
