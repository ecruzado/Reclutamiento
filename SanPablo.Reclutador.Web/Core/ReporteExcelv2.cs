using System;
using System.Collections.Generic;
using System.Data;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.Util;
using System.IO;
using NPOI;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using System.Collections;
using NPOI.HSSF.UserModel;
using NPOI.HPSF.Extractor;

namespace SanPablo.Reclutador.Web.Core
{
    public class ReporteExcelv2 
    {

        public HSSFWorkbook libro{ get; set; }
        public ISheet hoja { get; set; }
        //public DetalleExcel detalleExcel { get; set; }

        //public List<CeldaExcel> listaCeldaExcel { get; set; }


        public void creaHoja(string nombrePagina,string indLibro)
        {
            if (indLibro.Equals("S"))
            {
                libro =  new HSSFWorkbook();
            }
            
            hoja = libro.CreateSheet(nombrePagina);
        }


        /// <summary>
        /// Agrega el titulo al archivo Excel
        /// </summary>
        /// <param name="filaInicio">fila inicial</param>
        /// <param name="filaFin">fila final</param>
        /// <param name="colInicial">columna Inicial</param>
        /// <param name="colFinal">columna final</param>
        /// <param name="valor">Nombre dek titulo</param>
        /// <param name="style">estilo del titulo</param>
        public void addTituloExcel(int filaInicio, int filaFin, int colInicial, int colFinal, String valor, ICellStyle style)
        {
            String error;
            IRow row;


            try
            {
                // crea la fila inicial
                row = hoja.CreateRow(filaInicio);
                // crea la celda
                addCelda(row, colInicial, valor, style,"S");
                // coloca el titulo en el centro de acuerdo a la cantidad de columnas
                hoja.AddMergedRegion(new CellRangeAddress(filaInicio, filaFin, colInicial, colFinal));
                filaInicio += 2;

            }
            catch (Exception e)
            {
                error = "Error al agregar el titulo a la hoja del archivo excel -> " + e;
                throw new Exception(error);
            }
        }

        /// <summary>
        /// aggrega una celda
        /// </summary>
        /// <param name="row">fila</param>
        /// <param name="col">posicion de la columna</param>
        /// <param name="valor">valor</param>
        /// <param name="style">estilo de la letra</param>
        /// <param name="TipoDato">Tipo de dato</param>
        public void addCelda(IRow row, int col, String valor, ICellStyle style,string TipoDato)
        {
            ICell celda; //se crea la celda en la posicion que se indica
            celda = row.CreateCell(col);

            if (TipoDato.Equals("N"))
	        {
		        celda.SetCellValue(Convert.ToDouble(valor));
	        }
            else
	        {
                celda.SetCellValue(valor);
	        }
            
            celda.CellStyle = style;
        }

        /// <summary>
        /// crea la fila y devulve la fila creada
        /// </summary>
        /// <param name="numFila">numero de fila</param>
        /// <returns>Returna un Irow una fila</returns>
        public IRow addFila(int numFila)
        {
            IRow row;
            row = hoja.CreateRow(numFila);
            return row;
        }


        ///// <summary>
        ///// adiciona campos a la cabecera
        ///// adiciona los campos a auna lista para colocarlos en la cabecera
        ///// </summary>
        ///// <param name="fila">fila inicial</param>
        ///// <param name="col">posicion de la columna</param>
        ///// <param name="campo">Campos</param>
        ///// <param name="style">estilo del campo</param>
        ///// <param name="TipoDato">Tipo de Dato</param>
        //public void adicionaCampos(IRow row , int col, string campo, ICellStyle style,string TipoDato)
        //{
        //    //int cont;
        //    //IRow row;
        //    //cont = fila++;
        //    //row = hoja.CreateRow(cont);

        //    addCelda(row, col, campo, style, TipoDato);

        //    }
        //}

        public Constants CENTER;
        public Constants CENTER_SELECTION;
        public Constants DISTRIBUTED;
        public Constants RIGHT;
        public Constants LEFT;

        /// <summary>
        /// Adiciona estilo para cadenas string
        /// </summary>
        /// <param name="negrita"></param>
        /// <param name="point"></param>
        /// <param name="alineamiento"></param>
        /// <returns></returns>
        public ICellStyle addEstiloCadena(Boolean negrita, int point, String alineamiento)
        {

            IFont fontCadena = libro.CreateFont();
            ICellStyle styleCadena = libro.CreateCellStyle();

            fontCadena.Color = HSSFColor.Black.Index;

            if (negrita)
            {
                fontCadena.Boldweight = (short)FontBoldWeight.Bold;
            }


            fontCadena.FontName = HSSFFont.FONT_ARIAL;

            fontCadena.FontHeightInPoints = (short)point;

            if ("CENTER".Equals(alineamiento))
            {
                styleCadena.Alignment = HorizontalAlignment.Center;
            }
            if ("CENTER_SELECTION".Equals(alineamiento))
            {
                styleCadena.Alignment = HorizontalAlignment.CenterSelection;
            }
            if ("DISTRIBUTED".Equals(alineamiento))
            {
                styleCadena.Alignment = HorizontalAlignment.Distributed;
            }
            if ("LEFT".Equals(alineamiento))
            {
                styleCadena.Alignment = HorizontalAlignment.Left;
            }
            if ("RIGHT".Equals(alineamiento))
            {
                styleCadena.Alignment = HorizontalAlignment.Left;
            }



            styleCadena.SetFont(fontCadena);

            return styleCadena;
        }

        /// <summary>
        /// Estilo para numeros
        /// </summary>
        /// <param name="negrita"></param>
        /// <param name="point"></param>
        /// <param name="alineamiento"></param>
        /// <returns></returns>
        public ICellStyle addEstiloNumero(Boolean negrita, int point, String alineamiento)
        {

            IFont fontNumero = libro.CreateFont();
            ICellStyle styleNumero = libro.CreateCellStyle();

            fontNumero.Color = HSSFColor.Black.Index;

            if (negrita)
            {
                fontNumero.Boldweight = (short)FontBoldWeight.Bold;
            }


            fontNumero.FontName = HSSFFont.FONT_ARIAL;

            fontNumero.FontHeightInPoints = (short)point;

            if ("CENTER".Equals(alineamiento))
            {
                styleNumero.Alignment = HorizontalAlignment.Center;
            }
            if ("CENTER_SELECTION".Equals(alineamiento))
            {
                styleNumero.Alignment = HorizontalAlignment.CenterSelection;
            }
            if ("DISTRIBUTED".Equals(alineamiento))
            {
                styleNumero.Alignment = HorizontalAlignment.Distributed;
            }
            if ("LEFT".Equals(alineamiento))
            {
                styleNumero.Alignment = HorizontalAlignment.Left;
            }
            if ("RIGHT".Equals(alineamiento))
            {
                styleNumero.Alignment = HorizontalAlignment.Left;
            }



            styleNumero.SetFont(fontNumero);

            return styleNumero;
        }

        /// <summary>
        /// Estilo para el titulo
        /// </summary>
        /// <param name="negrita"></param>
        /// <param name="point"></param>
        /// <param name="alineamiento"></param>
        /// <returns></returns>
        public ICellStyle addEstiloTitulo(Boolean negrita, int point, String alineamiento)
        {

            IFont fontTitulo = libro.CreateFont();
            ICellStyle styleTitulo = libro.CreateCellStyle();

            fontTitulo.Color = HSSFColor.Black.Index;

            if (negrita)
            {
                fontTitulo.Boldweight = (short)FontBoldWeight.Bold;
            }


            fontTitulo.FontName = HSSFFont.FONT_ARIAL;

            fontTitulo.FontHeightInPoints = (short)point;

            if ("CENTER".Equals(alineamiento))
            {
                styleTitulo.Alignment = HorizontalAlignment.Center;
            }
            if ("CENTER_SELECTION".Equals(alineamiento))
            {
                styleTitulo.Alignment = HorizontalAlignment.CenterSelection;
            }
            if ("DISTRIBUTED".Equals(alineamiento))
            {
                styleTitulo.Alignment = HorizontalAlignment.Distributed;
            }
            if ("LEFT".Equals(alineamiento))
            {
                styleTitulo.Alignment = HorizontalAlignment.Left;
            }
            if ("RIGHT".Equals(alineamiento))
            {
                styleTitulo.Alignment = HorizontalAlignment.Left;
            }

            styleTitulo.SetFont(fontTitulo);

            return styleTitulo;
        }

        /// <summary>
        /// estilo negrita
        /// </summary>
        /// <param name="point"></param>
        /// <param name="alineamiento"></param>
        /// <returns></returns>
        public ICellStyle addEstiloCadenaNegrita(int point, String alineamiento)
        {

            IFont fontCadenaNegrita = libro.CreateFont();
            ICellStyle styleCadenaNegrita = libro.CreateCellStyle();

            fontCadenaNegrita.Color = HSSFColor.Black.Index;
            fontCadenaNegrita.Boldweight = (short)FontBoldWeight.Bold;



            fontCadenaNegrita.FontName = HSSFFont.FONT_ARIAL;

            fontCadenaNegrita.FontHeightInPoints = (short)point;

            if ("CENTER".Equals(alineamiento))
            {
                styleCadenaNegrita.Alignment = HorizontalAlignment.Center;
            }
            if ("CENTER_SELECTION".Equals(alineamiento))
            {
                styleCadenaNegrita.Alignment = HorizontalAlignment.CenterSelection;
            }
            if ("DISTRIBUTED".Equals(alineamiento))
            {
                styleCadenaNegrita.Alignment = HorizontalAlignment.Distributed;
            }
            if ("LEFT".Equals(alineamiento))
            {
                styleCadenaNegrita.Alignment = HorizontalAlignment.Left;
            }
            if ("RIGHT".Equals(alineamiento))
            {
                styleCadenaNegrita.Alignment = HorizontalAlignment.Left;
            }



            styleCadenaNegrita.SetFont(fontCadenaNegrita);

            return styleCadenaNegrita;
        }

        /// <summary>
        /// imprime Excel
        /// </summary>
        /// <param name="exportData"></param>
        /// <returns></returns>
        public MemoryStream imprimeExcel(MemoryStream exportData)
        {
            libro.Write(exportData);
            return exportData;
        }

        /// <summary>
        /// adiciona el un logo
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="colIni"></param>
        /// <param name="colFinal"></param>
        /// <param name="filaIni"></param>
        /// <param name="filaFin"></param>
        public void AdicionaLogoSanPablo(String dir, int colIni, int colFinal, int filaIni, int filaFin)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)hoja.CreateDrawingPatriarch();
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(100, 255, 0, 0, colIni, filaIni, colFinal, filaFin);
            anchor.AnchorType = 4;
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(dir, libro));

        }

        /// <summary>
        /// carga la imagen del logo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="wb"></param>
        /// <returns></returns>
        private int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);

        }

      
    }
}