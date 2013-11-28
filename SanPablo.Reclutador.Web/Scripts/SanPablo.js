SanPablo = {
    Grilla: function (grilla, pager, identificador, height, width, caption, urlListar, id, colsNames, colsModel, sortName, opciones) {
        var grid = jQuery('#' + grilla);
        var estadoSubGrid = false;

        if (opciones.sort == null) {
            opciones.sort = 'desc';
        }

        if (opciones.subGrid != null) {
            estadoSubGrid = true;
        }

        if (opciones.rowNumber == null) {
            opciones.rowNumber = 15;
        }

        if (opciones.rules == null) {
            opciones.rules = false;
        }

        if (opciones.dialogDelete == null) {
            opciones.dialogDelete = 'dialog-delete';
        }

        if (opciones.dialogAlert == null) {
            opciones.dialogAlert = 'dialog-alert';
        }

        if (opciones.search == null) {
            opciones.search = false;
        }

        if (opciones.multiselect == null) {
            opciones.multiselect = false;
        }

        var rowKey;
        $('#' + grilla).jqGrid({
            prmNames: {
                search: 'isSearch',
                nd: null,
                rows: 'rows',
                page: 'page',
                sort: 'sortField',
                order: 'sortOrder',
                filters: 'filters'
            },

            postData: { searchString: '', searchField: '', searchOper: '', filters: '' },
            jsonReader: {
                root: 'rows',
                page: 'page',
                total: 'total',
                records: 'records',
                cell: 'cell',
                id: id, //index of the column with the PK in it
                userdata: 'userdata',
                repeatitems: true
            },
            rowNum: opciones.rowNumber,
            rowList: [opciones.rowNumber, 20, 50, 100, 150],
            pager: '#' + pager,
            sortname: sortName,
            viewrecords: true,
            multiselect: opciones.multiselect,
            rownumbers: true,
            sortorder: opciones.sort,
            height: height,
            width: width,
            colNames: colsNames,
            colModel: colsModel,
            subGrid: estadoSubGrid,
            subGridRowColapsed: function (subgrid_id, row_id) {
                var subgrid_table_id, pager_id;
                subgrid_table_id = subgrid_id + "_t";
                pager_id = "p_" + subgrid_table_id;
                jQuery("#" + subgrid_table_id).remove();
                jQuery("#" + pager_id).remove();
            },
            subGridRowExpanded: function (subgrid_id, row_id) {
                var subGrid = opciones.subGrid;

                var subgrid_table_id, pager_id;
                subgrid_table_id = subgrid_id + "_t";
                pager_id = "p_" + subgrid_table_id;

                $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");

                var parameters = { cDocNro: row_id };
                $.ajax({
                    type: "POST",
                    url: subGrid.Url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(parameters),
                    success: function (rsp) {
                        var data = (typeof rsp.d) == 'string' ? eval('(' + rsp.d + ')') : rsp.d;

                        $("#" + subgrid_table_id).jqGrid({
                            datatype: "local",
                            colNames: subGrid.ColNames,
                            colModel: subGrid.ColModels,
                            rowNum: 10,
                            rowList: [10, 20, 50, 100],
                            sortorder: "desc",
                            viewrecords: true,
                            rownumbers: true,
                            pager: "#" + pager_id,
                            loadonce: true,
                            sortable: true,
                            height: subGrid.Height,
                            width: subGrid.Width
                        });

                        for (var i = 0; i <= data.length; i++)
                            jQuery("#" + subgrid_table_id).jqGrid('addRowData', i + 1, data[i]);

                        $("#" + subgrid_table_id).trigger("reloadGrid");
                    },
                    failure: function (msg) {
                        $('#mensajeFalla').show().fadeOut(8000);
                    }
                });
            },

            ondblClickRow: function (rowid) {
                if (opciones.search) {
                    var ret = grid.getRowData(rowid);
                    SelectRow(ret);
                }
            },
            onSelectRow: function () {
                rowKey = grid.getGridParam('selrow');

                if (rowKey != null) {
                    $("#" + identificador).val(rowKey);
                }
            },
            gridComplete: function () {
                if ($('#' + grilla).getGridParam('records') == 0) {
                    MotoTravel.ShowAlert("dialog-alert", "Sin Registro");
                }

                rowKey = null;
            },
            datatype: function (postdata) {
                var migrilla = new Object();
                migrilla.page = postdata.page;
                migrilla.rows = postdata.rows;
                migrilla.sidx = postdata.sortField;
                migrilla.sord = postdata.sortOrder;
                migrilla._search = postdata.isSearch;
                migrilla.filters = postdata.filters;
                if (opciones.rules != false) {
                    migrilla.Rules = GetRules();
                }

                if (migrilla._search == true) {
                    migrilla.searchField = postdata.searchField;
                    migrilla.searchOper = postdata.searchOper;
                    migrilla.searchString = postdata.searchString;
                }

                var params = { grid: migrilla };

                $.ajax({
                    url: urlListar,
                    type: 'post',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(params),
                    async: false,
                    success: function (data, st) {
                        if (st == 'success') {
                            var jq = $('#' + grilla)[0];
                            jq.addJSONData(data);
                        }
                    },
                    error: function () {
                        alert('Error with AJAX callback');
                    }
                });
            },
            filterToolbar: {}
        }).navGrid("#" + pager, { edit: false, add: false, del: false, search: opciones.search },
                    {}, // use default settings for edit
                    {}, // use default settings for add
                    {}, // delete instead that del:false we need this
                    {
                        multipleSearch: false,
                        beforeShowSearch: function () {
                            $(".ui-reset").trigger("click");
                            return true;
                        }
                    });

        if (opciones.eliminar) {
            $('#' + grilla).navButtonAdd('#' + pager, {
                caption: 'Eliminar',
                title: 'Eliminar',
                buttonicon: 'ui-icon-trash',
                position: 'first',
                onClickButton: function () {
                    if (rowKey != null) {
                        $("#" + opciones.dialogDelete).dialog("open");
                    } else {
                        MotoTravel.ShowAlert(opciones.dialogAlert, "Seleccione Un Registro");
                    }
                }
            });
        }

        if (opciones.editar) {
            $('#' + grilla).navButtonAdd('#' + pager, {
                caption: 'Editar',
                title: 'Editar',
                buttonicon: 'ui-icon-pencil',
                position: 'first',
                onClickButton: function () {
                    if (rowKey != null) {
                        Editar(rowKey);
                    } else {
                        MotoTravel.ShowAlert(opciones.dialogAlert, "Seleccione Un Registro");
                    }
                }
            });
        }

        if (opciones.nuevo) {
            $('#' + grilla).navButtonAdd('#' + pager, {
                caption: 'Nuevo',
                title: 'Nuevo',
                buttonicon: 'ui-icon-plus',
                position: 'first',
                onClickButton: function () {
                    Nuevo();
                }
            });
        }
    }
};