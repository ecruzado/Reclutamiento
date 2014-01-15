SanPablo = {
    ShowElement: function (elemento) {
        $(elemento).slideDown(200).animate({ opacity: 1 }, 300);
    },
    HideElement: function (elemento) {
        $(elemento).animate({
            opacity: 0.25
        }, 300, function () {
            $(elemento).slideUp(200);
        });
    },
    Mayuscula: function (e, elemento) {
        elemento.value = elemento.value.toUpperCase();
    },
    MenuSelected: function (idFormulario) {
        if (idFormulario == '')
            idFormulario = "01";
        $("li.pselectedForm").removeClass("pselectedForm");
        $("#form" + idFormulario).addClass("pselectedForm");
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
    RemoveArray: function (arr) {
        var what, a = arguments, l = a.length, ax;
        while (l > 1 && arr.length) {
            what = a[--l];
            while ((ax = arr.indexOf(what)) != -1) {
                arr.splice(ax, 1);
            }
        }
        return arr;
    },
    PadLeft: function (value, len, character) {
        len = len - value.length;
        for (var i = 1; i <= len; i++) {
            value = character + value;
        }
        return value;
    },
    ValidarInput: function (args) {
        var isValid = true;
        $.each(args, function (index, item) {
            if ($('#' + item).val() == '') {
                $('#' + item).parent().append("<span class='error'>*</span>");
                isValid = false;
            }
        });

        return isValid;
    },
    ClearInput: function (args) {
        $.each(args, function (index, item) {
            $('#' + item).val('');
        });
    },
    DisableInput: function (args, estado) {
        $.each(args, function (index, item) {
            $('#' + item).attr("disabled", estado);
        });
    },
    RemoveSpan: function (args) {
        $.each(args, function (index, item) {
            $('#' + item + " span").remove();
        });
    },
    HideMantenimiento: function (estado) {
        if (estado) {
            $("#Lista").show();
            $("#Mantenedor").hide();
            $("#divDetalle").hide();
        } else {
            $("#Lista").hide();
            $("#Mantenedor").show();
            $("#divDetalle").show();
        }
    },
    ReordenarInput: function (args) {
        $.each(args, function (item) {
            $("input[name^=" + item + "]").each(function (index) {
                $(this).attr("id", item + index);
            });
        });
    },
    Ajax: function (url, parameters, async) {
        var rsp;
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (response) {
                rsp = response;
            },
            failure: function (msg) {
                alert(msg);
                rsp = msg;
            },
            error: function (xhr, status, error) {
                alert(error);
                rsp = error;
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

    ShowAlert: function (dialog, mensaje) {
        if (dialog == '') {
            dialog = 'dialog-alert';
        }

        $('#' + dialog).html("");
        $('#' + dialog).append("<br/>" + mensaje);
        $('#' + dialog).dialog("open");
    },

    CreateDialogs: function (arrayDialog) {
        for (var i = 0; i < arrayDialog.length; i++) {
            $("#" + arrayDialog[i].name).dialog({
                autoOpen: false,
                resizable: false,
                height: arrayDialog[i].height,
                width: arrayDialog[i].width,
                title: arrayDialog[i].title,
                modal: true,
                open: function () {
                    $(this).parent().appendTo($('#aspnetForm'));
                }
            });
        }
    },
    CreateDialogsConfirm: function (arrayDialog) {
        for (var i = 0; i < arrayDialog.length; i++) {
            $("#" + arrayDialog[i].name).dialog({
                autoOpen: false,
                resizable: false,
                height: arrayDialog[i].height,
                width: arrayDialog[i].width,
                title: arrayDialog[i].title,
                modal: true,
                open: function () {
                    $(this).parent().appendTo($('#aspnetForm'));
                },
                buttons: [
                        {
                            text: arrayDialog[i].titleBtn1,
                            click: function () {
                                var name = $(this).attr('id');
                                $.each(arrayDialog, function (index, v) {
                                    if (v.name == name) {
                                        var fun = window[v.strFun];
                                        fun();
                                        return;
                                    }
                                });
                            }
                        },
                        {
                            text: arrayDialog[i].titleBtn2,
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
            });
        }
    },

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
            caption: caption,
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
    },

    IniPopUp: function (main, mainTitle, alerta, msgdelete, mainHeight, mainWidth) {
        $("#" + main).dialog({
            autoOpen: false,
            resizable: false,
            title: mainTitle,
            height: mainHeight,
            width: mainWidth,
            modal: true,
            open: function () {
                $(this).closest('.ui-dialog').find('.ui-dialog-titlebar-close').hide();
                $(this).parent().appendTo($('#aspnetForm'));
            }
        });

        $("#" + alerta).dialog({
            autoOpen: false,
            resizable: false,
            height: 100,
            width: 280,
            title: 'Alerta',
            modal: true
        });

        $("#" + msgdelete).dialog({
            autoOpen: false,
            resizable: false,
            title: "Eliminar",
            height: "150",
            width: "380",
            modal: true,
            buttons: [
                    {
                        text: "Eliminar",
                        click: function () {
                            Eliminar();
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
    },
    LoadDropDownList: function (name, url, parameters, selected, isValIndex, async) {
        var combo = document.getElementById(name);
        combo.options.length = 0;
        combo.options[0] = new Option("");
        combo.selectedIndex = 0;

        $('#' + name).ajaxError(function (event, request, settings) {
            combo.options[0] = new Option("Error al cargar.");
        });
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (items) {
                $.each(items, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (selected == undefined) selected = 0;

                if (isValIndex) {
                    $('#' + name).attr('selectedIndex', selected);
                } else {
                    $('#' + name).val(selected);
                }
            },
            failure: function (msg) {
            },
            error: function (xhr, status, error) {
            }
        });
    },
    LoadDropDownListMulti: function (name, url, parameters, selected, async) {
        var combo = document.getElementById(name);

        $('#' + name).ajaxError(function (event, request, settings) {
            combo.options[0] = new Option("Error al cargar.");
        });
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (items) {
                var list = items;
                $.each(list, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (selected == undefined) selected = 0;
                $('#' + name).val(selected);
            },
            failure: function (msg) {
            },
            error: function (xhr, status, error) {
            }
        });
    },
    LoadDropDownListSinFormato: function (name, url, parameters, selected, async) {
        var combo = document.getElementById(name);
        combo.options.length = 0;
        combo.options[0] = new Option("");
        combo.selectedIndex = 0;

        $('#' + name).ajaxError(function (event, request, settings) {
            combo.options[0] = new Option("Error al cargar.");
        });
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: async,
            data: JSON.stringify(parameters),
            success: function (items) {
                var list = items;
                $.each(list, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (selected == undefined) selected = 0;
                $('#' + name).val(selected);
            },
            failure: function (msg) {
            },
            error: function (xhr, status, error) {
            }
        });
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
    Operacion: function (url) {
        $.ajax({
            url: url,
            dataType: 'html',
            success: function (result) {
                $('#listado').hide();
                $('#informacion').show();
                $('#informacion').html(result);
            },
            error: function (request, status, error) {
                $('#listado').hide();
                $('#informacion').show();
                alert(request.responseText);
            }
        });
    }
};