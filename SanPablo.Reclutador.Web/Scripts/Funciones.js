Funciones = {
    Mayuscula: function (e, elemento) {
        elemento.value = elemento.value.toUpperCase();
    },

    Completar: function (ctrl, len) {
        var numero = ctrl.value;
        if (numero.length == len || numero.length == 0) return true;
        for (var i = 1; numero.length < len; i++) {
            numero = '0' + numero;
        }
        ctrl.value = numero;
        return true;
    },

    PadLeft: function (value, len, character) {
        len = len - value.length;
        for (var i = 1; i <= len; i++) {
            value = character + value;
        }
        return value;
    },

    Ajax: function (url, parameters, async) {
        var rsp;
        $.ajax({
            type: "POST",
            url: url,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (response) {
                rsp = response;
            },
            failure: function (msg) {
                rsp = -1;
            }
        });
        return rsp;
    },

    AjaxJson: function (type, url, parameters, async, methodSuccess) {
        var rsp;
        $.ajax({
            type: type,
            url: url,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: methodSuccess,
            failure: function (msg) {
                rsp = -1;
            }
        });
        return rsp;
    },

    ParseDate: function (date) {
        var parts = date.split("/");
        var fecha = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
        return fecha.getTime();
    },

    ValidarCombo: function (args) {
        var isvalid = true;
        $.each(args, function (index, item) {
            var combo = jQuery('#' + item);
            var valor = combo.val();

            if (valor == 0 || valor == null) {
                var padre = combo.parent();
                var haySpan = padre.find("span").length;

                if (haySpan == 0) {
                    padre.append("<span class='error'>*</span>");
                }
                isvalid = false;
            }
        });
        return isvalid;
    },

    ObtenerFormulario: function (url, contenedorInformacion) {
        $.ajax({
            url: url,
            cache: false,
            dataType: 'html',
            success: function (result) {
                $('#' + contenedorInformacion).show();
                $('#' + contenedorInformacion).html(result);
            },
            error: function (request, status, error) {
                $('#' + contenedorInformacion).hide();
                alert(request.responseText);
            }
        });
    },

    MostrarInformacion: function (url, contenedorInformacion) {
        $.ajax({
            url: url,
            cache: false,
            dataType: 'html',
            success: function (result) {
                $('#' + contenedorInformacion).show();
                $('#' + contenedorInformacion).html(result);
            },
            error: function (request, status, error) {
                $('#' + contenedorInformacion).hide();
                alert(request.responseText);
            }
        });
    },

    MostrarInformacionPopup: function (url, contenedorInformacion) {
        $.ajax({
            url: url,
            dataType: 'html',
            async: false,
            cache: false,
            success: function (result) {
                $('#' + contenedorInformacion).html(result);
                $.validator.unobtrusive.parse($('#' + contenedorInformacion));
                $('#' + contenedorInformacion).dialog("open");
            },
            error: function (request, status, error) {
                alert(request.responseText);
            }
        });
    },

    GrillaCompleta: function (grilla, pager, height, width, caption, urlListar, id, colsNames, colsModel, sortName, opciones, metodoNuevo, metodoEditar, metodoEliminar) {
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

        if (opciones.rowList == null) {
            opciones.rowList = [opciones.rowNumber, 20, 50, 100, 150];
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
            rowList: opciones.rowList,
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
            caption: caption,
            //shrinkToFit: false,
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

                if (opciones.cambiarFila) {
                    cambiarFila(rowKey);
                }
            },
            gridComplete: function () {
                $('.loading').hide();
                if ($('#' + grilla).getGridParam('records') == 0) {
                    //BI.ShowAlert("dialog-alert", "Sin Registro");
                }
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
                    cache: false,
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(params),
                    success: function (data, st) {
                        if (st == 'success') {
                            var jq = $('#' + grilla)[0];
                            if (jq != undefined) {
                                jq.addJSONData(data);
                            }
                        }
                    },
                    error: function () {
                        //alert('Error with AJAX callback');
                    }
                });
            }
        }).navGrid("#" + pager, { edit: false, add: false, del: false, search: opciones.search },
            {}, // use default settings for edit
            {}, // use default settings for add
            {}, // delete instead that del:false we need this
            {
                multipleSearch: true,
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
                        $("#" + opciones.dialogDelete).dialog({
                            resizable: false,
                            title: "Eliminar",
                            height: "150",
                            width: "380",
                            modal: true,
                            buttons: [
                                    {
                                        text: "Eliminar",
                                        click: function () {
                                            metodoEliminar(rowKey);
                                        }
                                    },
                                    {
                                        text: "Cancelar",
                                        click: function () {
                                            $(this).dialog("close");
                                        }
                                    }
                            ]
                        });
                    } else {
                        //BI.ShowAlert(opciones.dialogAlert, "Seleccione Un Registro");
                    }
                }
            });
        }

        if (opciones.editar) {
            $('#' + grilla).navButtonAdd('#' + pager, {
                caption: opciones.nombreEditar == null ? 'Editar' : opciones.nombreEditar,
                title: opciones.nombreEditar == null ? 'Editar' : opciones.nombreEditar,
                buttonicon: 'ui-icon-pencil',
                position: 'first',
                onClickButton: function () {
                    if (rowKey != null) {
                        metodoEditar(rowKey);
                    } else {
                        //BI.ShowAlert(opciones.dialogAlert, "Seleccione Un Registro");
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
                    metodoNuevo();
                }
            });
        }
    },

    LoadDropDownList: function (name, url, parameters, selected) {
        var combo = document.getElementById(name);
        combo.options.length = 0;
        combo.options[0] = new Option("");
        combo.selectedIndex = 0;

        $.ajax({
            type: "POST",
            url: url,
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(parameters),
            success: function (items) {
                var list = (typeof items.d) == 'string' ? eval('(' + items.d + ')') : items.d;

                $.each(list, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (selected == undefined) selected = 0;
                $('#' + name).val(selected);
            }
        });
    },

    LoadDropDownListItems: function (name, url, parameters, selected) {
        var combo = document.getElementById(name);
        combo.options.length = 0;
        combo.options[0] = new Option("");
        combo.selectedIndex = 0;
        combo.disabled = true;

        var resultado = BI.Ajax(url, parameters, false);

        $.each(resultado, function (index, item) {
            combo.options[index] = new Option(item.Text, item.Value);
        });
        combo.disabled = false;
        if (selected == undefined) selected = 0;
        $('#' + name).val(selected);
    },

    Clear: function (divName) {
        $('#' + divName + ' select').children().remove();
        var elemt = $('#' + divName);
        $(':input', elemt).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase();
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = -1;
        });
    },

    Regresar: function Regresar(tabla, contenedorListado, contenedorInformacion) {
        try {
            $('#' + tabla)[0].clearToolbar();
        } catch (e) { }
        $('#' + tabla).trigger('reloadGrid');
        $('#' + contenedorListado).show();
        $('#' + contenedorInformacion).html('');
        $('#' + contenedorInformacion).hide();
    },

    RegresarPopup: function Regresar(contenedorInformacion) {
        $('#' + contenedorInformacion).dialog("close");
        Refrescar();
    },

    ValidarDecimal: function (numero) {
        var patron = /^([0-9])*[.]?[0-9]*$/;
        if (patron.test(numero))
            return true;
        return false;
    },

    ValidarFecha: function (fecha) {
        var patron = /^((([0][1-9]|[12][\d])|[3][01])[-\/]([0][13578]|[1][02])[-\/][1-9]\d\d\d)|((([0][1-9]|[12][\d])|[3][0])[-\/]([0][13456789]|[1][012])[-\/][1-9]\d\d\d)|(([0][1-9]|[12][\d])[-\/][0][2][-\/][1-9]\d([02468][048]|[13579][26]))|(([0][1-9]|[12][0-8])[-\/][0][2][-\/][1-9]\d\d\d)$/;
        if (patron.test(fecha))
            return true;
        return false;
    },

    ValidarEntero: function (numero) {
        var patron = /^\d+$/;
        if (patron.test(numero))
            return true;
        return false;
    },

    CrearPopup: function (nombre) {
        $('#' + nombre).dialog("destroy");
        $('#' + nombre).dialog({
            width: 'auto',
            resizable: false,
            modal: true,
            autoOpen: false,
            closeOnEscape: false,
            open: function (event, ui) {
                $(this).parent().appendTo("form");
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
            },
            close: function () { }
        });
    }
};