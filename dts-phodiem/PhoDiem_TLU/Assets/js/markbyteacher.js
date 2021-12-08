$(document).ready(function () {
    handleChanged();
    (function () {
        $.ajax({
            url: '../Mark/test',
            datatype: 'json',
            type: 'get',
            // other AJAX settings goes here
            // ..
            success: function (data) {
                console.log(data);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
                console.log(ajaxOptions);
                console.log(thrownError);
            }
        })
    }())
})
function handleChanged() {
    $('#startYear').on('change', function() {
        const startYear = this.value
        const endYear = document.querySelector('#endYear').value
        
        if (startYear && endYear){
            setSemester(startYear, endYear)
        }
        else setDefaultSemester()
    })
    $('#endYear').on('change', function () {
        const endYear = this.value
        const startYear = document.querySelector('#startYear').value
        
        if (startYear && endYear) {
            setSemester(startYear, endYear)
        }
        else setDefaultSemester()
    })
}
function setSemester(startYear, endYear) {
    $.ajax({
        url: '../Mark/getSemester',
        type: 'post',
        dataType: 'json',
        data: {
            startYear: startYear,
            endYear: endYear
        },
        success: function (data) {
            if (data.code === 200) {
                setDefaultSemester()
                let element = data.semester.map(item => {
                    return `<option value = ${item.id}> ${item.name} </option>`
                })
                element.join('')
                $('#semester').append(element)
            }
        }
    })
}
function setDefaultSemester() {
    var semester = document.querySelector('#semester')
    semester.innerHTML = `<option value> Kì học </option>`
}
function handleSubmit() {
    $('form').submit(function (even) {
        if (even.originalEvent.submitter.value == 'Hiển thị') {
            $.ajax({
                method: $(this).attr('method'),
                url: $(this).attr('action'),
                data: $(this).serialize(),
                datatype: 'json',
                type: 'post',
                // other AJAX settings goes here
                // ..

                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr);
                    console.log(ajaxOptions);
                    console.log(thrownError);
                }
            }).done(function (response) {

                if (response.data != null) {
                    if (response.code == 200) {
                        console.log(response.data)
                    }
                    else {
                        console.log(reponse.data);
                    }
                }
                else {
                    console.log("Kết nối thất bại!");
                    alert('Không có dữ liệu');
                }
            });
            event.preventDefault(); // <- avoid reloading
        }

    })
}