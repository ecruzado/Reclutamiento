namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
   
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.OracleClient;
    using System.Configuration;
    using System.Collections;
   
    using System.Linq;
    using System.Transactions;
    using System.IO;
    //using oracle = Oracle.DataAccess.Client;
    //using Oracle.DataAccess.Types;


    public class ExamenRepository : Repository<Examen>, IExamenRepository
    {
        public ExamenRepository(ISession session)
            : base(session)
        { 
        }
        /// <summary>
        /// obtiene el tiempo del examen
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public int getTiempoExamen(int idExamen) {

           
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.FN_DURACIONEXAMEN");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_nidexamen", OracleType.Int32).Value = idExamen;
                cmd.Parameters.Add("p_rRetVal", OracleType.Number).Direction = ParameterDirection.ReturnValue;
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(cmd.Parameters[cmd.Parameters.IndexOf("p_rRetVal")].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lcon.Close();
            }
        }
        /// <summary>
        /// otbtiene los datos para la impresion del examen
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
        public DataTable getDataRepExamen(int idExamen)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

            try
            {
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_GETREPEXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                if (idExamen > 0)
                {
                    lspcmd.Parameters.Add("P_NIDEXAMEN", OracleType.Number).Value = idExamen;
                }
                else {
                    lspcmd.Parameters.Add("P_NIDEXAMEN", OracleType.Number).Value = 0;
                }

                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter da = new OracleDataAdapter(lspcmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                da.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lcon.Close();
            }


        }

        /// <summary>
        /// obtiene los datos para armar el pdf
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
       public List<PdfExamen> ObtenerPdfExamens(PdfExamen obj)
        {

            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader drPdfExamens;
                PdfExamen objPdfExamen;
                List<PdfExamen> listaPdfExamens = new List<PdfExamen>();
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_GETREPEXAMEN");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
               

                if (obj.Ideexamen > 0)
                {
                    lspcmd.Parameters.Add("P_NIDEXAMEN", OracleType.Number).Value = obj.Ideexamen;
                }
                else {
                    lspcmd.Parameters.Add("P_NIDEXAMEN", OracleType.Number).Value = 0;
                }

                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;



                drPdfExamens = (OracleDataReader)lspcmd.ExecuteReader();
                objPdfExamen = null;
                
                //listaPdfExamens.PdfExamens = new List<PdfExamen>();
                
               //drPdfExamens.geto
                while (drPdfExamens.Read())
                {
                    objPdfExamen = new PdfExamen();

                   
                    objPdfExamen.Ideexamen = Convert.ToInt32(drPdfExamens["IDEEXAMEN"]);
                    objPdfExamen.Idesede = Convert.ToInt32(drPdfExamens["IDESEDE"]);
                    objPdfExamen.Nomexamen = Convert.ToString(drPdfExamens["NOMEXAMEN"]);
                    objPdfExamen.Descexamen = Convert.ToString(drPdfExamens["DESCEXAMEN"]);
                    objPdfExamen.Idesubcategoria = Convert.ToInt32(drPdfExamens["IDESUBCATEGORIA"]);
                    objPdfExamen.Idecategoria = Convert.ToInt32(drPdfExamens["IDECATEGORIA"]);

                    objPdfExamen.Nomcategoria = Convert.ToString(drPdfExamens["NOMCATEGORIA"]);

                    objPdfExamen.Desccategoria = Convert.ToString(drPdfExamens["DESCCATEGORIA"]);
                    objPdfExamen.Tipcategoria = Convert.ToString(drPdfExamens["TIPCATEGORIA"]);
                    objPdfExamen.DesTipoCategoria = Convert.ToString(drPdfExamens["DESTIPCATEGORIA"]);

                    objPdfExamen.Instrucciones = Convert.ToString(drPdfExamens["INSTRUCCIONES"]);
                    objPdfExamen.Tipoejemplo = Convert.ToString(drPdfExamens["TIPOEJEMPLO"]);
                   
                    if (drPdfExamens["IMAGENEJEMPLO"] != null && drPdfExamens["IMAGENEJEMPLO"] != DBNull.Value)
                    {
                        objPdfExamen.Imagenejemplo = (byte[])(drPdfExamens["IMAGENEJEMPLO"]);
                    }


                    objPdfExamen.Textoejemplo = (drPdfExamens["TEXTOEJEMPLO"]==null?"":Convert.ToString(drPdfExamens["TEXTOEJEMPLO"]));
                    objPdfExamen.Descsubcategoria = (drPdfExamens["DESCSUBCATEGORIA"]==null?"":Convert.ToString(drPdfExamens["DESCSUBCATEGORIA"]));
                    objPdfExamen.Nomsubcategoria = (drPdfExamens["NOMSUBCATEGORIA"]==null?"":Convert.ToString(drPdfExamens["NOMSUBCATEGORIA"]));
                    objPdfExamen.Idecriterio = (drPdfExamens["IDECRITERIO"]==null?0:Convert.ToInt32(drPdfExamens["IDECRITERIO"]));


                    objPdfExamen.Idealternativa = (drPdfExamens["IDEALTERNATIVA"]==null?0:Convert.ToInt32(drPdfExamens["IDEALTERNATIVA"]));

                    objPdfExamen.Codmod = (drPdfExamens["CODMOD"]==null?"":Convert.ToString(drPdfExamens["CODMOD"]));
                    objPdfExamen.Desmodo = (drPdfExamens["DESMODO"]==null?"":Convert.ToString(drPdfExamens["DESMODO"]));
                    objPdfExamen.Tipcriterio = (drPdfExamens["TIPCRITERIO"]==null?"":Convert.ToString(drPdfExamens["TIPCRITERIO"]));
                    objPdfExamen.Pregunta = (drPdfExamens["PREGUNTA"]==null?"":Convert.ToString(drPdfExamens["PREGUNTA"]));


                    if (drPdfExamens["IMAGENCRIT"] != null && drPdfExamens["IMAGENCRIT"] != DBNull.Value)
                    {
                        objPdfExamen.Imagencrit = (byte[])(drPdfExamens["IMAGENCRIT"]);
                    }

                    if (drPdfExamens["IMAGE"] != null && drPdfExamens["IMAGE"] != DBNull.Value)
                    {
                        objPdfExamen.Image = (byte[])(drPdfExamens["IMAGE"]);
                    }
                    
                    objPdfExamen.Alternativa = Convert.ToString(drPdfExamens["ALTERNATIVA"]);
                    objPdfExamen.Estactivo = Convert.ToString(drPdfExamens["ESTACTIVO"]);
                    objPdfExamen.Ordensub = Convert.ToInt32(drPdfExamens["ORDENSUB"]);

                    objPdfExamen.Ordencrit = Convert.ToInt32(drPdfExamens["ORDENCRIT"]);
                    objPdfExamen.Tiempocat = Convert.ToString(drPdfExamens["TIEMPOCAT"]);
                    objPdfExamen.Timpoexamen = Convert.ToString(drPdfExamens["TIMPOEXAMEN"]);

                    listaPdfExamens.Add(objPdfExamen);
                    
                }

                drPdfExamens.Close();
                return listaPdfExamens;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                lcon.Close();
            }
        }



    }
}