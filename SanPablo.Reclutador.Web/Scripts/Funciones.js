Number.prototype.formatMoney = function (c, d, t) {
    var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};






Funciones = {
   
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

    GetYearRange: function (anioInicio, anioFin) {
        var i = 0;
        var opcionesAnios = "";

        for (i = anioInicio; i <= anioFin; i = i + 1) {
            opcionesAnios += "<option value=\"" + i.toString() + "\">" + i.toString() + "</option>";
        }

        return opcionesAnios;
    },

    TrimString: function (cadena) {
        var trimmed = cadena.replace(/^\s+|\s+$/g, '');
        return trimmed;
    },

    ParseDate: function (date) {
        var parts = date.split("/");
        var fecha = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
        return fecha.getTime();
    },

    ParsearFechaJSON: function (jsonDate) {
        if (jsonDate == "Sin Especificar")
            return "";

        var fechaActual = new Date(parseInt(jsonDate.slice(6, -2)));

        return Belcorp.PadLeft(fechaActual.getDate(), 2, '0') + "/" + Belcorp.PadLeft((fechaActual.getMonth() + 1), 2, '0') + "/" + fechaActual.getFullYear();
    },

    ParsearFecha: function (jsonDate) {
        if (jsonDate == "Sin Especificar")
            return "";

        var fechaActual = $.datepicker.parseDate("dd/mm/yy", jsonDate);

        return Funciones.PadLeft(fechaActual.getDate(), 2, '0') + "/" + Funciones.PadLeft((fechaActual.getMonth() + 1), 2, '0') + "/" + fechaActual.getFullYear();
    },

    PadLeft: function (i, l, s) {
        var o = i.toString();
        if (!s) { s = '0'; }
        while (o.length < l) {
            o = s + o;
        }
        return o;
    },

    ValidarCombo: function (comboName) {
        var combo = jQuery('#' + comboName);
        var valor = combo.val();
        var isvalid = true;

        if (valor == 0 || valor == null) {
            var padre = combo.parent();
            var haySpan = padre.find("span").length;

            if (haySpan == 0) {
                padre.append("<span class='error'>*</span>");
            }

            isvalid = false;
        }

        return isvalid;
    },

    AsignarRangosDatePicker: function (DateStartPicker, DateEndPicker) {

        var fechaMaxima = jQuery("#" + DateEndPicker).datetimepicker('getDate');
        if (fechaMaxima != null)
            jQuery("#" + DateStartPicker).datetimepicker('option', 'maxDate', new Date(fechaMaxima.getTime()));

        var fechaMinima = jQuery("#" + DateStartPicker).datetimepicker('getDate');
        if (fechaMinima != null)
            jQuery("#" + DateEndPicker).datetimepicker('option', 'minDate', new Date(fechaMinima.getTime()));
    },

    DatePickerRange: function (DateStartPicker, DateEndPicker, dialogError) {

        var popupError = jQuery("#" + dialogError);

        popupError.dialog({
            autoOpen: false,
            resizable: false,
            height: 100,
            width: 380,
            title: 'Alerta',
            modal: true
        });

        jQuery("#" + DateStartPicker.Id).datetimepicker({
            onClose: function (dateText, inst) {
                var endDateTextBox = $('#' + DateEndPicker.Id);
                if (endDateTextBox.val() != '') {
                    var testStartDate = new Date(dateText);
                    var testEndDate = new Date(endDateTextBox.val());

                    if (testStartDate > testEndDate) {
                        endDateTextBox.val(dateText);
                        popupError.html("La Fecha de Inicio no Debe ser Mayor a la Fecha Fin. <br/>Los valores de las Fechas seran intercambiados");
                        popupError.dialog('open');
                    }
                }
                else {
                    if (DateStartPicker.OverrideSelect == null && !DateStartPicker.OverrideSelect) {
                        endDateTextBox.val(dateText);
                    }

                    if (DateEndPicker.SelectFunction != null)
                        DateEndPicker.SelectFunction();
                }
            },
            onSelect: function (selectedDateTime) {
                var start = $(this).datetimepicker('getDate');
                $('#' + DateEndPicker.Id).datetimepicker('option', 'minDate', new Date(start.getTime()));

                if (DateStartPicker.SelectFunction != null)
                    DateStartPicker.SelectFunction();
            }
        });
        jQuery("#" + DateEndPicker.Id).datetimepicker({
            onClose: function (dateText, inst) {
                var startDateTextBox = $('#' + DateStartPicker.Id);
                if (startDateTextBox.val() != '') {
                    var testStartDate = new Date(startDateTextBox.val());
                    var testEndDate = new Date(dateText);

                    if (testStartDate > testEndDate) {
                        startDateTextBox.val(dateText);

                        popupError.html("La Fecha de Inicio no Debe ser Mayor a la Fecha Fin. <br/>Los valores de las Fechas seran intercambiados");
                        popupError.dialog('open');
                    }
                }
                else {
                    if (DateEndPicker.OverrideSelect == null && !DateEndPicker.OverrideSelect) {
                        startDateTextBox.val(dateText);
                    }

                    if (DateStartPicker.SelectFunction != null)
                        DateStartPicker.SelectFunction();
                }
            },
            onSelect: function (selectedDateTime) {
                var end = $('#' + DateStartPicker.Id).datetimepicker('getDate');
                $('#' + DateStartPicker.Id).datetimepicker('option', 'maxDate', new Date(end.getTime()));

                if (DateEndPicker.SelectFunction != null)
                    DateEndPicker.SelectFunction();
            }
        });
    },

    OperacionNormal: function (dialog, url, parameters) {
        $.ajax({
            type: "post",
            url: url,
            data: parameters,
            success: function (response) {
                if (response == "success") {
                    $("#mensajeExito").show().fadeOut(4000);
                }
                else {
                    $("#mensajeFalla").show().fadeOut(4000);
                }

                $("#" + dialog).dialog("close");
            },
            failure: function (msg) {
                $("#" + dialog).dialog("close");
                $('#mensajeFalla').show().fadeOut(4000);
            }
        });
    },

    Operacion: function (grilla, dialog, url, parameters) {
        $.ajax({
            type: "post",
            url: url,
            data: parameters,
            success: function (response) {
                if (response == "success") {
                    $("#" + grilla).trigger("reloadGrid");
                    $("#mensajeExito").show().fadeOut(4000);
                }
                else {
                    $("#mensajeFalla").show().fadeOut(4000);
                }

                $("#" + dialog).dialog("close");
            },
            failure: function (msg) {
                $("#" + dialog).dialog("close");
                $('#mensajeFalla').show().fadeOut(4000);
            }
        });
    },

    OperacionCallBack: function (grilla, dialog, url, parameters, callBackFunction) {
        $.ajax({
            type: "post",
            url: url,
            data: parameters,
            success: function (response) {
                if (response.Mensaje == "success") {
                    if (typeof (callBackFunction) != typeof (undefined))
                        callBackFunction(response.ObjetoRespuesta);

                    $("#" + grilla).trigger("reloadGrid");
                    $("#mensajeExito").show().fadeOut(4000);
                }
                else {
                    $("#mensajeFalla").show().fadeOut(4000);
                }

                $("#" + dialog).dialog("close");
            },
            failure: function (msg) {
                $("#" + dialog).dialog("close");
                $('#mensajeFalla').show().fadeOut(4000);
            }
        });
    },

    LoadDropDownListText: function (name, url, parameters, selected, disabled) {
        return $.ajax({
            type: "POST",
            url: url,
            data: parameters,
            beforeSend: function () {
                var combo = document.getElementById(name);
                combo.options.length = 0;
                combo.options[0] = new Option("Cargando...");
                combo.selectedIndex = 0;
                combo.disabled = true;
            },
            success: function (items) {
                $.each(items, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.Valor);
                });

                if (disabled != null)
                    combo.disabled = disabled;
                else
                    combo.disabled = false;

                if (selected == undefined) selected = '';
                $('#' + name).val(selected);
            },
            error: function () {
                var combo = document.getElementById(name);
                combo.options[0] = new Option("Error al cargar.");
            }
        });
    },

    LoadDropDownList: function (name, url, parameters, selected, disabled, valueMember, displayMember) {
        var combo = document.getElementById(name);

        return $.ajax({
            type: "POST",
            url: url,
            data: parameters,
            beforeSend: function () {
                combo.options.length = 0;
                combo.options[0] = new Option("-Seleccionar-", 0);
                combo.selectedIndex = 0;
            },
            success: function (items) {
                $.each(items, function (index, item) {
                    combo.options[index + 1] = new Option(item[displayMember], item[valueMember]);
                });

                if (selected == undefined) selected = 0;
                $('#' + name).val(selected);
            },
            error: function () {
                var combo = document.getElementById(name);
                combo.options[0] = new Option("Error al cargar.");
            }
        });
    },

    LoadDropDownListByEstado: function (name, url, parameters, selected, estado) {
        return $.ajax({
            type: "POST",
            url: url,
            data: parameters,
            beforeSend: function () {
                var combo = document.getElementById(name);
                combo.options.length = 0;
                combo.options[0] = new Option("Cargando...");
                combo.selectedIndex = 0;
                combo.disabled = true;
            },
            success: function (items) {
                $.each(items, function (index, item) {
                    combo.options[index] = new Option(item.Nombre, item.IdComun);
                });
                if (estado == 'true') {
                    combo.disabled = true;
                }
                else {
                    combo.disabled = false;
                }
                if (selected == undefined) selected = 0;
                $('#' + name).val(selected);
            },
            error: function () {
                var combo = document.getElementById(name);
                combo.options[0] = new Option("Error al cargar.");
            }
        });
    },

    ClearChildren: function (divName) {
        $('#' + divName + ' select').children().remove();
        // iterate over all of the inputs for the form
        // element that was passed in
        var elemt = $('#' + divName);
        $(':input', elemt).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            // it's ok to reset the value attr of text inputs,
            // password inputs, and textareas
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
                // checkboxes and radios need to have their checked state cleared
                // but should *not* have their 'value' changed
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
                // select elements need to have their 'selectedIndex' property set to -1
                // (this works for both single and multiple select elements)
            else if (tag == 'select')
                this.selectedIndex = -1;
        });
    },

    Alert: function (selectorDiv, selectorSpan, msg, title) {
        $('#' + selectorSpan).html(msg);
        //$('#' + selectorDiv).dialog({
        //    title: title,
        //    autoOpen: false,
        //    modal: true,
        //    buttons: {
        //        "Aceptar": function () {
        //            $(this).dialog("close");
        //        }
        //    }
        //});
        //$('#' + selectorDiv).dialog('open');
        var opt = {
            title: title,
            autoOpen: false,
            modal: true,
            buttons: {
                "Aceptar": function () {
                    $(this).dialog("close");
                }
            }
        };
        $('#' + selectorDiv).dialog(opt).dialog('open');


    },


   


    Clear: function (divName) {
        $('#' + divName + ' select').children().remove();
        // iterate over all of the inputs for the form
        // element that was passed in
        var elemt = $('#' + divName);
        $(':input', elemt).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            // it's ok to reset the value attr of text inputs,
            // password inputs, and textareas
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
                // checkboxes and radios need to have their checked state cleared
                // but should *not* have their 'value' changed
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
                // select elements need to have their 'selectedIndex' property set to -1
                // (this works for both single and multiple select elements)
            else if (tag == 'select')
                this.selectedIndex = -1;
        });
    },
    Busqueda: function (selectorDiv, selectorFrame, urlFrame, titulo) {
        $('#' + selectorFrame).attr("src", urlFrame);
        $('#' + selectorDiv).dialog({
            title: titulo,
            modal: true,
            height: '550',
            width: '500',
            close: function () {
                $('#' + selectorFrame).attr("src", "");
                $('#' + selectorDiv).dialog('destroy');
            }
        });
    },
    Numeros: function (selector) {
        var $this = $('#' + selector);
        $this.keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match(/[^0-9]/g)) return false;
        });
    },
    Letras: function (selector) {
        var $this = $('#' + selector);
        $this.keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match(/[^a-zA-Z]/g)) return false;
        });
    },
    Alfanumerico: function (selector) {
        var $this = $('#' + selector);
        $this.keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match(/[^0-9a-zA-Z]/g)) return false;
        });
    },
    MensajeSiNo: function (selectorDiv, selectorSpan, mensaje, callBackFunction) {
        $('#' + selectorSpan).html(mensaje);
        $('#' + selectorDiv).dialog({
            title: 'Confirmación',
            modal: true,
            open: function (event, ui) {
                $(".ui-dialog-titlebar-close").hide();
            },
            close: function () {
                $('#' + selectorDiv).dialog('destroy');
            },
            buttons: {
                "Aceptar": function () {
                    if (typeof (callBackFunction) != typeof (undefined))
                        callBackFunction();
                    $('#' + selectorDiv).dialog('destroy');
                },
                "Cancelar": function () {
                    $('#' + selectorDiv).dialog('destroy');
                }
            }
        });
    },
    HabilitarCheckBoxs: function (selectorTabla, selectorChkColTabla, selectorCssRow) {
        var $chkCol = $('#' + selectorChkColTabla);
        var $this = $('#' + selectorTabla + ' tr.' + selectorCssRow + ' input[type=checkbox]');

        $chkCol.bind('click', function () {
            if ($this.length > 0) {
                if ($chkCol.is(":checked")) {
                    $this.each(function () {
                        $this.attr("checked", "checked");
                    });
                } else {
                    $this.each(function () {
                        $this.removeAttr("checked");
                    });
                }
            }
        });

    },
    isNumberKey: function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    },
    isDecimalKey: function (evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            if (charCode == 46)
                return true;
            else
                return false;
        }
        return true;
    },
    Importes: function (selector) {
        var $this = $('#' + selector);
        $this.keypress(function (e) {
            if (String.fromCharCode(e.keyCode).match(/[^0-9.]/g)) return false;
        });
    },
    diasEntreFechas: function (fechaInicio, fechaFin) {
        var diasDiferencia = "";
        var UN_DIA = 1000 * 60 * 60 * 24;

        if (fechaInicio != "" && fechaFin != "") {
            var anhoFechaInicio = parseInt(fechaInicio.substr(6, 4));
            var mesFechaInicio = parseFloat(fechaInicio.substr(3, 2)) - 1;
            var diaFechaInicio = parseInt(fechaInicio.substr(0, 2));
            var anhoFechaFin = parseInt(fechaFin.substr(6, 4));
            var mesFechaFin = parseFloat(fechaFin.substr(3, 2)) - 1;
            var diaFechaFin = parseInt(fechaFin.substr(0, 2));

            var dteInicio = new Date(anhoFechaInicio, mesFechaInicio, diaFechaInicio);
            var dteFin = new Date(anhoFechaFin, mesFechaFin, diaFechaFin);

            var millisInicio = dteInicio.getTime();
            var millisFin = dteFin.getTime();

            var millisDiferencia = Math.abs(millisFin - millisInicio);

            var diasDiferencia = Math.round(millisDiferencia / UN_DIA);
        }

        return diasDiferencia;
    },

    compareDate: function (fechamenor, fechamayor) {
        var dtCh = "/";
        var minYear = 1900;
        var maxYear = 2100;

        var valor = 0

        var pos1 = fechamenor.indexOf(dtCh)
        var pos2 = fechamenor.indexOf(dtCh, pos1 + 1)
        var strDayMe = fechamenor.substring(0, pos1)
        var strMonthMe = fechamenor.substring(pos1 + 1, pos2)
        var strYearMe = fechamenor.substring(pos2 + 1)

        var pos3 = fechamayor.indexOf(dtCh)
        var pos4 = fechamayor.indexOf(dtCh, pos3 + 1)
        var strDayMa = fechamayor.substring(0, pos3)
        var strMonthMa = fechamayor.substring(pos3 + 1, pos4)
        var strYearMa = fechamayor.substring(pos4 + 1)

        var fecMenor = strYearMe + strMonthMe + strDayMe
        var fecMayor = strYearMa + strMonthMa + strDayMa

        if (fecMenor == fecMayor) {
            valor = 0
        } else {
            if (fecMenor < fecMayor) {
                valor = 1
            } else {
                valor = -1
            }
        }
        return valor
    }



};