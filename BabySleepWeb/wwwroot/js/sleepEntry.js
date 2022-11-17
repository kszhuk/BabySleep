//new tempusDominus.TempusDominus(document.getElementById('datetimepicker1'), {});
//new tempusDominus.TempusDominus(document.getElementById('datetimepicker2'), {});

const linkedPicker1Element = document.getElementById('datetimepicker1');
const linked1 = new tempusDominus.TempusDominus(linkedPicker1Element);

//using event listeners
linkedPicker1Element.addEventListener(tempusDominus.Namespace.events.show, (e) => {
    $(".tempus-dominus-widget").css({ 'position': 'absolute', 'top': $('#datetimepicker1').position().top + 30, 'left': $('#datetimepicker1').position().left });
});

const linkedPicker2Element = document.getElementById('datetimepicker2');
const linked2 = new tempusDominus.TempusDominus(linkedPicker2Element, {
    useCurrent: false,
});

//using event listeners
linkedPicker2Element.addEventListener(tempusDominus.Namespace.events.show, (e) => {
    $(".tempus-dominus-widget").css({ 'position': 'absolute', 'top': $('#datetimepicker2').position().top + 30, 'left': $('#datetimepicker2').position().left });
});

//using event listeners
linkedPicker1Element.addEventListener(tempusDominus.Namespace.events.change, (e) => {
    linked2.updateOptions({
        restrictions: {
            minDate: e.detail.date,
        },
    });
});

//using subscribe method
const subscription = linked2.subscribe(tempusDominus.Namespace.events.change, (e) => {
    linked1.updateOptions({
        restrictions: {
            maxDate: e.date,
        },
    });
});