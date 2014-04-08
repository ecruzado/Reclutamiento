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

namespace HonorariosMedicos_BLL
{
    public class ColumnaExcel
    {
        private string _nombre;

        /// <summary>
        /// Nombre para mostrar en reporte excel
        /// </summary>
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        private string _origen;

        /// <summary>
        /// Nombre de columna en base de datos
        /// </summary>
        public string Origen
        {
            get { return _origen; }
            set { _origen = value; }
        }
        private int _orden;

        /// <summary>
        /// Posicion en reporte excel
        /// </summary>
        public int Orden
        {
            get { return _orden; }
            set { _orden = value; }
        }


        private int _indice;

        /// <summary>
        /// Posicion del indice de la columna origen
        /// </summary>
        public int Indice
        {
            get { return _indice; }
            set { _indice = value; }
        }

    }

    public class CeldaExcel
    {
        private string _nombre;

        /// <summary>
        /// Nombre de la etiqueta
        /// </summary>
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        private string _valor;

        /// <summary>
        /// nombre del valor
        /// </summary>
        public string Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }
        private int _fila;

        /// <summary>
        /// Posicion en Fila
        /// </summary>
        public int Fila
        {
            get { return _fila; }
            set { _fila = value; }
        }

        private int _columna;
        /// <summary>
        /// Posicion en columna
        /// </summary>
        public int Columna
        {
            get { return _columna; }
            set { _columna = value; }
        }

        

    }

    public class ReporteExcel
    {

        HSSFWorkbook workbook = new HSSFWorkbook();

        ISheet sheet;
       // DataTable tablaImpresion = new DataTable();
        //DataTable tablaCabeceras = new DataTable();
        DataRow row;
        ArrayList listaCab = new ArrayList();
        ArrayList listaDetalle = new ArrayList();
        ArrayList listColumnas = new ArrayList();

        List<ColumnaExcel> columnasMapeo = new List<ColumnaExcel>();

        DataTable origenDatos;
        List<CeldaExcel> objCelda = new List<CeldaExcel>();

       


        //creaHoja returna la pagina de la hoja donde se trabaja
        public void creaHoja(string nombrePagina)
        {
            sheet = workbook.CreateSheet(nombrePagina);
            objCelda.Clear();
            //origenDatos.Reset();
            columnasMapeo.Clear();
            listaCab.Clear();
            listaDetalle.Clear();
            listColumnas.Clear();
            //tablaCabeceras.Reset();
            //tablaImpresion.Reset();
            
        }

        //agrega un titulo excel
        public void addTituloExcel(ISheet sheet, int filaInicio, int filaFin, int colInicial, int colFinal, String valor, HSSFWorkbook workbook)
        {
            String error;
            IRow row;


            try
            {
                row = sheet.CreateRow(filaInicio);
                // crea la celda
                addCelda(row, colInicial, valor);
                // coloca el titulo en el centro de acuerdo a la cantidad de columnas
                sheet.AddMergedRegion(new CellRangeAddress(filaInicio, filaFin, colInicial, colFinal));
                filaInicio += 2;

            }
            catch (Exception e)
            {
                error = "Error al agregar el titulo a la hoja del archivo excel -> " + e;
                throw new Exception(error);
            }
        }

        // agrega titulo excel con estilo

        public void addTituloExcel(int filaInicio, int filaFin, int colInicial, int colFinal, String valor, ICellStyle style)
        {
            String error;
            IRow row;


            try
            {
                row = sheet.CreateRow(filaInicio);
                // crea la celda
                addCelda(row, colInicial, valor, style);
                // coloca el titulo en el centro de acuerdo a la cantidad de columnas
                sheet.AddMergedRegion(new CellRangeAddress(filaInicio, filaFin, colInicial, colFinal));
                filaInicio += 2;

            }
            catch (Exception e)
            {
                error = "Error al agregar el titulo a la hoja del archivo excel -> " + e;
                throw new Exception(error);
            }
        }

        // adiciona una fila a la cabecera
        public void adicionaCamposCab(int fila, int col, string campo, ICellStyle style)
        {
            int cont;
            IRow row;
            cont = fila++;
            row = sheet.CreateRow(cont);

            CeldaExcel celdaExcel = new CeldaExcel();

            celdaExcel.Fila = fila;
            celdaExcel.Columna = col;
            celdaExcel.Nombre = campo;
            objCelda.Add(celdaExcel);


            foreach (CeldaExcel var in objCelda)
            {
                if (fila == var.Fila)
                {
                    addCelda(row, var.Columna, var.Nombre, style);
                }

            }


        }

       


        public void AdicionaLogoSanPablo(String dir, int colIni, int colFinal, int filaIni, int filaFin)
        {
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
            HSSFClientAnchor anchor;
            anchor = new HSSFClientAnchor(100, 255, 0, 0, colIni, filaIni, colFinal, filaFin);
            anchor.AnchorType = 4;
            HSSFPicture picture = (HSSFPicture)patriarch.CreatePicture(anchor, LoadImage(dir, workbook));

        }

        private int LoadImage(string path, HSSFWorkbook wb)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            return wb.AddPicture(buffer, PictureType.JPEG);

        }

        // adiciona una celda
        public void addCelda(IRow row, int col, String valor)
        {
            String error;
            ICell celda;

            try
            {
                celda = row.CreateCell(col);
                
               
                celda.SetCellValue(valor);
                //celda.SetCellType(style);

            }
            catch (Exception e)
            {
                error = "Error al agregar " + valor + " a una celda al excel -> " + e;
                throw new Exception(error);
            }
        }

        //agrega con estilo
        public void addCelda(IRow row, int col, String valor, ICellStyle style)
        {
            String error;
            ICell celda;

            
                celda = row.CreateCell(col);

                try
                {
                    celda.SetCellValue(Convert.ToDouble(valor));
                }
                catch (Exception e) {
                    celda.SetCellValue(valor);
                }

                celda.CellStyle = style;
                
            
            }

        // Agrega los titulos de la cabecera
        public void addEncabezadosColumnas(HSSFWorkbook workbook, ISheet sheet, int fila, int col, ArrayList cab, HSSFCellStyle style)
        {


            IRow row;
            //ICell cell;
            String error;
            int cont = 0;
            IEnumerator myEnumerator;

            try
            {
                myEnumerator = cab.GetEnumerator();
                row = sheet.CreateRow(fila++);

                while (myEnumerator.MoveNext())
                {
                    cont = cont + 1;
                    row = sheet.CreateRow(fila);
                    addCelda(row, cont, "" + myEnumerator.Current);
                }
            }
            catch (Exception e)
            {
                error = "Error al agregar la cabecera al detalle del documento no agrupada -> " + e;
                throw new Exception(error);
            }
        }


        public void addDetalleLista(DataTable tabla, int posColum, string desColum, int orden)
        {


            //tablaImpresion.Columns.Add(tabla.Columns[posColum].ColumnName);
            String nombreColumn = tabla.Columns[posColum].ColumnName;

            // agrega las cabeceras
            listaCab.Add(desColum);
            // agrega la lista de columnas
            listColumnas.Add(posColum);
            // agrega la lista del detalle
            listaDetalle.Clear();
            listaDetalle.Add(tabla);

            ColumnaExcel item = new ColumnaExcel();
            item.Nombre = desColum;
            item.Origen = nombreColumn;
            item.Orden = orden;
            item.Indice = posColum;
            columnasMapeo.Add(item);

           
            
            origenDatos = tabla;

        }
        // imprime el detalle de los datatables

        public void imprimirCabecera(int fila, ICellStyle estilo)
        {
            int cont = 1;
            IRow row;
            row = sheet.CreateRow(fila++);

            foreach (String cab in listaCab)
            {
                addCelda(row, cont++, cab, estilo);
            }

        }

        public void imprimiDetalle(int fila, ICellStyle estilo)
        {
            IRow row;

            int columnas = listColumnas.Count;
            columnasMapeo.Sort(delegate(ColumnaExcel p1, ColumnaExcel p2) { return p1.Orden.CompareTo(p2.Orden); });
            int cont = 0;
            int pos = 0;
           
            string desColumna;
            for (int i = 0; i < origenDatos.Rows.Count; i++)
            {
                cont = fila++;
                row = sheet.CreateRow(cont);
                columnas = 0;
                for (int j = 0; j < origenDatos.Columns.Count; j++)
                {

                    if (columnasMapeo.Exists(delegate(ColumnaExcel comp) { return origenDatos.Columns[j].ColumnName.Contains(comp.Origen); }))
                    {
                        foreach (ColumnaExcel var in columnasMapeo)
                        {
                            desColumna = origenDatos.Columns[j].ColumnName;
                            if (var.Origen.Equals(desColumna))
                            {
                                adicionaCamposCab(cont, var.Orden, origenDatos.Rows[i][j].ToString(), estilo);
                            }
                        }

                    };
                }
            }


        }

        public Constants CENTER;
        public Constants CENTER_SELECTION;
        public Constants DISTRIBUTED;
        public Constants RIGHT;
        public Constants LEFT;


        public ICellStyle addEstiloCadena(Boolean negrita, int point, String alineamiento)
        {

            IFont fontCadena = workbook.CreateFont();
            ICellStyle styleCadena = workbook.CreateCellStyle();

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


        public ICellStyle addEstiloNumero(Boolean negrita, int point, String alineamiento)
        {

            IFont fontNumero = workbook.CreateFont();
            ICellStyle styleNumero = workbook.CreateCellStyle();

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


        public ICellStyle addEstiloTitulo(Boolean negrita, int point, String alineamiento)
        {

            IFont fontTitulo = workbook.CreateFont();
            ICellStyle styleTitulo = workbook.CreateCellStyle();

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


        public ICellStyle addEstiloCadenaNegrita(int point, String alineamiento)
        {

            IFont fontCadenaNegrita = workbook.CreateFont();
            ICellStyle styleCadenaNegrita = workbook.CreateCellStyle();

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


        public MemoryStream imprimeExcel(MemoryStream exportData)
        {
            workbook.Write(exportData);
            return exportData;
        }
    }


    public class LibroExcel
    {
        private List<ColumnaExcel> _columnasMapeo;
        private DataTable _origenDatos;

        private int _inicioColumnaDetalle = 0;
        private int _inicioFilaDetalle = 0;

        public HSSFWorkbook workbook = new HSSFWorkbook();

        public LibroExcel(List<ColumnaExcel> columnas, DataTable origenDatos)
        {
            _columnasMapeo = columnas;
            _origenDatos = origenDatos;
        }

        public void CrearExcel() 
        {
            _columnasMapeo.Sort(delegate(ColumnaExcel p1, ColumnaExcel p2) { return p1.Orden.CompareTo(p2.Orden); });
            //int cont = 0;
            //int pos = 0;
            
            ISheet sheet = workbook.CreateSheet("olakase");

           // string desColumna;
            for (int i = 0; i < _origenDatos.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(_inicioFilaDetalle++);
                
                for (int j = 0; j < _origenDatos.Columns.Count; j++)
                {
                    ColumnaExcel colCoincidencia = _columnasMapeo.FindLast(delegate(ColumnaExcel comp) 
                    {
                        return _origenDatos.Columns[j].Ordinal == comp.Orden;
                    });
                    
                    if (colCoincidencia != null)
                    {
                        ICell celda = row.CreateCell(_inicioColumnaDetalle + colCoincidencia.Indice);
                        if (_origenDatos.Columns[j].DataType == typeof(System.Decimal))
                        {
                            celda.SetCellValue(Convert.ToDouble(_origenDatos.Rows[i][j]));
                        }
                        else 
                        {
                            celda.SetCellValue(_origenDatos.Rows[i][j].ToString());
                        }
                    }
                }
            }
        }
    }

}
