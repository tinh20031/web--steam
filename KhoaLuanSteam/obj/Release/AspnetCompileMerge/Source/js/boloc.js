function create_custom_dropdowns() {
    $('select').each(function (i, select) {
        if (!$(this).next().hasClass('dropdown-select')) {
            $(this).after('<div class="dropdown-select wide ' + ($(this).attr('class') || '') + '" tabindex="0"><span class="current"></span><div class="list"><ul></ul></div></div>');
            var dropdown = $(this).next();
            var options = $(select).find('option');
            var selected = $(this).find('option:selected');
            dropdown.find('.current').html(selected.data('display-text') || selected.text());
            options.each(function (j, o) {
                var display = $(o).data('display-text') || '';
                dropdown.find('ul').append('<li class="option ' + ($(o).is(':selected') ? 'selected' : '') + '" data-value="' + $(o).val() + '" data-display-text="' + display + '">' + $(o).text() + '</li>');
            });
        }
    });

    $('.dropdown-select ul').before('<div class="dd-search"><input id="txtSearchValue" autocomplete="off" onkeyup="filter()" class="dd-searchbox" type="text"></div>');
}

// Event listeners

// Open/close
$(document).on('click', '.dropdown-select', function (event) {
    if ($(event.target).hasClass('dd-searchbox')) {
        return;
    }
    $('.dropdown-select').not($(this)).removeClass('open');
    $(this).toggleClass('open');
    if ($(this).hasClass('open')) {
        $(this).find('.option').attr('tabindex', 0);
        $(this).find('.selected').focus();
    } else {
        $(this).find('.option').removeAttr('tabindex');
        $(this).focus();
    }
});

// Close when clicking outside
$(document).on('click', function (event) {
    if ($(event.target).closest('.dropdown-select').length === 0) {
        $('.dropdown-select').removeClass('open');
        $('.dropdown-select .option').removeAttr('tabindex');
    }
    event.stopPropagation();
});

function filter() {
    var valThis = $('#txtSearchValue').val();
    $('.dropdown-select ul > li').each(function () {
        var text = $(this).text();
        (text.toLowerCase().indexOf(valThis.toLowerCase()) > -1) ? $(this).show() : $(this).hide();
    });
};
// Search

// Option click
$(document).on('click', '.dropdown-select .option', function (event) {
    $(this).closest('.list').find('.selected').removeClass('selected');
    $(this).addClass('selected');
    var text = $(this).data('display-text') || $(this).text();
    $(this).closest('.dropdown-select').find('.current').text(text);
    $(this).closest('.dropdown-select').prev('select').val($(this).data('value')).trigger('change');
});

// Keyboard events
$(document).on('keydown', '.dropdown-select', function (event) {
    var focused_option = $($(this).find('.list .option:focus')[0] || $(this).find('.list .option.selected')[0]);
    // Space or Enter
    //if (event.keyCode == 32 || event.keyCode == 13) {
    if (event.keyCode == 13) {
        if ($(this).hasClass('open')) {
            focused_option.trigger('click');
        } else {
            $(this).trigger('click');
        }
        return false;
        // Down
    } else if (event.keyCode == 40) {
        if (!$(this).hasClass('open')) {
            $(this).trigger('click');
        } else {
            focused_option.next().focus();
        }
        return false;
        // Up
    } else if (event.keyCode == 38) {
        if (!$(this).hasClass('open')) {
            $(this).trigger('click');
        } else {
            var focused_option = $($(this).find('.list .option:focus')[0] || $(this).find('.list .option.selected')[0]);
            focused_option.prev().focus();
        }
        return false;
        // Esc
    } else if (event.keyCode == 27) {
        if ($(this).hasClass('open')) {
            $(this).trigger('click');
        }
        return false;
    }
});

$(document).ready(function () {
    create_custom_dropdowns();
});


////// title

//// function([string1, string2],target id,[color1,color2])    
//consoleText(['Thiết Bị Steam'], 'text', ['tomato', 'rebeccapurple', 'lightblue']);

//function consoleText(words, id, colors) {
//    if (colors === undefined) colors = ['#fff'];
//    var visible = true;
//    var con = document.getElementById('console');
//    var letterCount = 1;
//    var x = 1;
//    var waiting = false;
//    var target = document.getElementById(id)
//    target.setAttribute('style', 'color:' + colors[0])
//    window.setInterval(function () {

//        if (letterCount === 0 && waiting === false) {
//            waiting = true;
//            target.innerHTML = words[0].substring(0, letterCount)
//            window.setTimeout(function () {
//                var usedColor = colors.shift();
//                colors.push(usedColor);
//                var usedWord = words.shift();
//                words.push(usedWord);
//                x = 1;
//                target.setAttribute('style', 'color:' + colors[0])
//                letterCount += x;
//                waiting = false;
//            }, 1000)
//        } else if (letterCount === words[0].length + 1 && waiting === false) {
//            waiting = true;
//            window.setTimeout(function () {
//                x = -1;
//                letterCount += x;
//                waiting = false;
//            }, 1000)
//        } else if (waiting === false) {
//            target.innerHTML = words[0].substring(0, letterCount)
//            letterCount += x;
//        }
//    }, 120)
//    window.setInterval(function () {
//        if (visible === true) {
//            con.className = 'console-underscore hidden'
//            visible = false;

//        } else {
//            con.className = 'console-underscore'

//            visible = true;
//        }
//    }, 400)
//}


