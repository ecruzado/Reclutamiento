(function ($) {
    $.fn.numerico = function () {
        this.keypress(function (evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        });
    };

    $.fn.grilla = function (url, datosBusqueda, opciones) {
        var that = this;
        var defecto = {
            pager: '#grdPager',
            rowNum: 10,
            rules: true,
            rowList: [10, 30, 50],
            sortname: 'Ide',
            sortorder: 'desc',
            viewrecords: true,
            height: 350,
            width: 920,
            rowNum: 0,
            cellsubmit: 'clientArray',
            hidegrid: false,
            rownumbers: true,
            shrinkToFit: false,
            datatype: function (postdata) {
                var migrilla = new Object();
                migrilla.page = postdata.page;
                migrilla.rows = postdata.rows;
                migrilla.sidx = postdata.sortField;
                migrilla.sord = postdata.sortOrder;
                migrilla._search = postdata.isSearch;
                migrilla.filters = postdata.filters;
                migrilla.Rules = datosBusqueda;
                if (migrilla._search == true) {
                    migrilla.searchField = postdata.searchField;
                    migrilla.searchOper = postdata.searchOper;
                    migrilla.searchString = postdata.searchString;
                }

                var params = { grid: migrilla };

                $.ajax({
                    url: url,
                    type: 'post',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(params),
                    async: false,
                    success: function (data, st) {
                        if (st == 'success') {
                            var jq = that[0];
                            jq.addJSONData(data);
                        }
                    },
                    error: function () {
                        alert('Error with AJAX callback');
                    }
                });
            },
        };
        var opciones = $.extend(defecto, opciones);

        this.jqGrid(opciones);
    };

    $.fn.dialogo = function (opciones) {
        var defecto = {
            autoOpen: false,
            modal: true,
            title: "Sistema de Reclutamiento Y Seleccion de Personal"
        };
        var opciones = $.extend(defecto, opciones);
        this.dialog(opciones);
    };
})(jQuery);