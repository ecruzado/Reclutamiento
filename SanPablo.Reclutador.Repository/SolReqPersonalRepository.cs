

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


    public class SolReqPersonalRepository : Repository<SolReqPersonal>, ISolReqPersonalRepository
    {
         public SolReqPersonalRepository(ISession session)
            : base(session)
        { 
        }

        /// <summary>
        /// obtiene la lista de los cargos activos
        /// </summary>
        /// <param name="IdCargo"></param>
        /// <returns></returns>
         public List<Cargo> GetTipCargo(int IdCargo)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             try
             {
                 IDataReader ldrCargo;
                 Cargo lobCargo;
                 List<Cargo> llstCargo;
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_CARGO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIdCargo", OracleType.Int32).Value = IdCargo;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                 ldrCargo = (OracleDataReader)lspcmd.ExecuteReader();
                 lobCargo = null;
                 llstCargo = new List<Cargo>();


                 while (ldrCargo.Read())
                 {
                     lobCargo = new Cargo();
                     lobCargo.IdeCargo = Convert.ToInt32(ldrCargo["IDECARGO"]);
                     lobCargo.NombreCargo = Convert.ToString(ldrCargo["NOMCARGO"]);
                     llstCargo.Add(lobCargo);
                 }
                 ldrCargo.Close();
                 return llstCargo;
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
        /// Elimina el Reemplazo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public int EliminaListaReemplazo(Reemplazo obj) 
         {

             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             try
             {
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET.SP_ELIMINA_REEMPLAZO");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_ideReemplazo", OracleType.Int32).Value = obj.IdReemplazo;
                 lspcmd.Parameters.Add("p_ideSolReq", OracleType.Int32).Value = obj.IdeSolReqPersonal;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Int32).Direction = ParameterDirection.Output;
                 lspcmd.ExecuteNonQuery();
                 return Convert.ToInt32(lspcmd.Parameters["p_cRetVal"].Value);
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
        /// obtiene las solicitudes resultado de la busqueda inicial
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
         public List<SolReqPersonal> GetListaSolReqPersonal(SolReqPersonal obj)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
             try
             {

                
                 string cFechaIncial = obj.FechaInicioBus==null?"":String.Format("{0:MM/dd/yyyy}", obj.FechaInicioBus);
                 string cFechaFinal = obj.FechaFinBus == null ? "" : String.Format("{0:MM/dd/yyyy}", obj.FechaFinBus);

                 IDataReader ldrSolReqPersonal;
                 SolReqPersonal lobSolReqPersonal;
                 List<SolReqPersonal> llstSolReqPersonal;
                 lcon.Open();
                 OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_LISTAREQ");
                 lspcmd.CommandType = CommandType.StoredProcedure;
                 lspcmd.Connection = lcon;
                 lspcmd.Parameters.Add("p_nIdCargo", OracleType.Int32).Value = obj.IdeCargo;
                 lspcmd.Parameters.Add("p_nIdDependencia", OracleType.Int32).Value = obj.IdeDependencia;
                 lspcmd.Parameters.Add("p_nIdDepartamento", OracleType.Int32).Value = obj.IdeDepartamento;
                 lspcmd.Parameters.Add("p_nIdArea", OracleType.Int32).Value = obj.IdeArea;
                 lspcmd.Parameters.Add("p_cTipEtapa", OracleType.VarChar).Value = obj.TipEtapa;
                 lspcmd.Parameters.Add("p_cTipResp", OracleType.VarChar).Value = obj.TipResponsable;
                 lspcmd.Parameters.Add("p_cEstado", OracleType.VarChar).Value = obj.TipEstado;
                 lspcmd.Parameters.Add("p_cFecIni", OracleType.VarChar).Value = cFechaIncial;
                 lspcmd.Parameters.Add("p_cFeFin", OracleType.VarChar).Value = cFechaFinal;
                 lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                 ldrSolReqPersonal = (OracleDataReader)lspcmd.ExecuteReader();
                 lobSolReqPersonal = null;
                 llstSolReqPersonal = new List<SolReqPersonal>();

              

                 while (ldrSolReqPersonal.Read())
                 {
                     lobSolReqPersonal = new SolReqPersonal();
                     lobSolReqPersonal.IdeSolReqPersonal = Convert.ToInt32(ldrSolReqPersonal["IDESOLREQPERSONAL"]);
                     lobSolReqPersonal.CodSolReqPersonal = Convert.ToString(ldrSolReqPersonal["CODSOLREQPERSONAL"]);
                     lobSolReqPersonal.IdeCargo = Convert.ToInt32(ldrSolReqPersonal["IDECARGO"]);
                     lobSolReqPersonal.DesCargo = Convert.ToString(ldrSolReqPersonal["DESCARGO"]);
                     lobSolReqPersonal.IdeDependencia = Convert.ToInt32(ldrSolReqPersonal["IDEDEPENDENCIA"]);
                     lobSolReqPersonal.Dependencia_des = Convert.ToString(ldrSolReqPersonal["DESDEPENDENCIA"]);
                     lobSolReqPersonal.IdeDepartamento = Convert.ToInt32(ldrSolReqPersonal["IDEDEPARTAMENTO"]);
                     lobSolReqPersonal.Departamento_des = Convert.ToString(ldrSolReqPersonal["DESDEPARTAMENTO"]);
                     lobSolReqPersonal.IdeArea = Convert.ToInt32(ldrSolReqPersonal["IDEAREA"]);
                     lobSolReqPersonal.Area_des = Convert.ToString(ldrSolReqPersonal["DESAREA"]);

                     lobSolReqPersonal.NumVacantes = Convert.ToInt32(ldrSolReqPersonal["NUMVACANTES"]);
                     lobSolReqPersonal.CantPostulante = Convert.ToInt32(ldrSolReqPersonal["POSTULANTE"]);
                     lobSolReqPersonal.CantPreSelec = Convert.ToInt32(ldrSolReqPersonal["PRESELECCIONADOS"]);
                     lobSolReqPersonal.CantEvaluados = Convert.ToInt32(ldrSolReqPersonal["EVALUADOS"]);
                     lobSolReqPersonal.CantSeleccionados = Convert.ToInt32(ldrSolReqPersonal["SELECCIONADOS"]);


                     lobSolReqPersonal.TipResponsable = Convert.ToString(ldrSolReqPersonal["RESPONSABLE"]);
                     lobSolReqPersonal.NomPersonReemplazo = Convert.ToString(ldrSolReqPersonal["NOMBRESPONSABLE"]);
                     lobSolReqPersonal.TipEstado = Convert.ToString(ldrSolReqPersonal["ESTACTIVO"]);
                     lobSolReqPersonal.TipEtapa = Convert.ToString(ldrSolReqPersonal["TIPETAPA"]);
                     lobSolReqPersonal.FecPublicacion = Convert.ToDateTime(ldrSolReqPersonal["FECPUBLICACION"]);
                     lobSolReqPersonal.Feccreacion = Convert.ToDateTime(ldrSolReqPersonal["FECCREACION"]);

                     llstSolReqPersonal.Add(lobSolReqPersonal);
                 }
                 ldrSolReqPersonal.Close();
                 return llstSolReqPersonal;
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

         public List<CompetenciaRequerimiento> ListaCompetencias(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            
             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_COMPETENCIAREMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var competencia = new CompetenciaRequerimiento();
                 var listCompetencias = new List<CompetenciaRequerimiento>();
  
                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         competencia = new CompetenciaRequerimiento();
                         competencia.IdeCompetenciaRequerimiento = Convert.ToInt32(lector["IDECOMPETENCIASOLREQ"]);
                         competencia.DescripcionCompetencia = Convert.ToString(lector["DESCRIPCION"]);
                         listCompetencias.Add(competencia);
                     }
                     lector.Close();
                 }
                 
                
                 return listCompetencias;

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


         public List<OfrecemosRequerimiento> ListaOfrecemos(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_OFRECIMIENTO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var ofrecimiento = new OfrecemosRequerimiento();
                 var listOfrecemos = new List<OfrecemosRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         ofrecimiento = new OfrecemosRequerimiento();
                         ofrecimiento.IdeOfrecemosRequerimiento = Convert.ToInt32(lector["IDEOFRECEMOSSOLREQ"]);
                         ofrecimiento.DescripcionOfrecimiento = Convert.ToString(lector["DESCRIPCION"]);
                         listOfrecemos.Add(ofrecimiento);
                     }
                     lector.Close();
                 }


                 return listOfrecemos;

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

         public List<HorarioRequerimiento> ListaHorarios(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_HORARIO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var horario = new HorarioRequerimiento();
                 var listaHorario = new List<HorarioRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         horario = new HorarioRequerimiento();
                         horario.IdeHorarioRequerimiento = Convert.ToInt32(lector["IDEHORARIOSOLREQ"]);
                         horario.DescripcionHorario = Convert.ToString(lector["DESCRIPCION"]);
                         horario.PuntajeHorario = Convert.ToInt32(lector["PUNTHORARIO"]);
                         listaHorario.Add(horario);
                     }
                     lector.Close();
                 }

                 return listaHorario;

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

         public List<UbigeoReemplazo> ListaUbigeos(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_UBIGEO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var ubigeo = new UbigeoReemplazo();
                 var listaHorario = new List<UbigeoReemplazo>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         ubigeo = new UbigeoReemplazo();
                         ubigeo.IdeUbigeoReemplazo = Convert.ToInt32(lector["IDEUBIGEOSOLREQ"]);
                         ubigeo.IdeUbigeo = Convert.ToInt32(lector["IDEUBIGEO"]);
                         ubigeo.Distrito = Convert.ToString(lector["DISTRITO"]);
                         ubigeo.Provincia = Convert.ToString(lector["PROVINCIA"]);
                         ubigeo.Departamento = Convert.ToString(lector["DEPARTAMENT"]);
                         ubigeo.PuntajeUbigeo = Convert.ToInt32(lector["PUNTUBIGEO"]);
                         listaHorario.Add(ubigeo);
                     }
                     lector.Close();
                 }

                 return listaHorario;

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

         public List<CentroEstudioRequerimiento> ListaCentroEstudio(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_CENT_EST_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var centroEstudio = new CentroEstudioRequerimiento();
                 var listaCentroEstudio = new List<CentroEstudioRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         centroEstudio = new CentroEstudioRequerimiento();
                         centroEstudio.IdeCentroEstudioRequerimiento = Convert.ToInt32(lector["IDECENTESTSOLREQ"]);
                         centroEstudio.DescripcionTipoCentroEstudio = Convert.ToString(lector["TIPINST"]);
                         centroEstudio.DescripcionNombreCentroEstudio = Convert.ToString(lector["NOMBINST"]);
                         centroEstudio.PuntajeCentroEstudios = Convert.ToInt32(lector["PUNTACENTROEST"]);
                         listaCentroEstudio.Add(centroEstudio);
                     }
                     lector.Close();
                 }

                 return listaCentroEstudio;

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

         public List<NivelAcademicoRequerimiento> ListaNivelAcademico(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_NIVELACAD_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var nivelAcademico = new NivelAcademicoRequerimiento();
                 var listaNivelAcademico = new List<NivelAcademicoRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         nivelAcademico = new NivelAcademicoRequerimiento();
                         nivelAcademico.IdeNivelAcademicoRequerimiento = Convert.ToInt32(lector["IDENIVELACADESOLREQ"]);
                         nivelAcademico.DescripcionTipoEducacion = Convert.ToString(lector["TIPEDUCACION"]);
                         nivelAcademico.DescripcionAreaEstudio = Convert.ToString(lector["AREAESTUDIO"]);
                         nivelAcademico.DescripcionNivelAlcanzado = Convert.ToString(lector["NIVELALCANZADO"]);
                         nivelAcademico.CicloSemestre = Convert.ToInt32(lector["CICLOSEMESTRE"]);
                         nivelAcademico.PuntajeNivelEstudio = Convert.ToInt32(lector["PUNTNIVESTUDIO"]);
                         listaNivelAcademico.Add(nivelAcademico);
                     }
                     lector.Close();
                 }

                 return listaNivelAcademico;

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

         public List<ConocimientoGeneralRequerimiento> ListaConocimientos(int ideSolicitudReqPersonal, string conocimiento)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_CONOCIMIENTO_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_tipoConocimiento", OracleType.VarChar).Value = conocimiento;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var conocimientoGral = new ConocimientoGeneralRequerimiento();
                 var listaConocimientoGral = new List<ConocimientoGeneralRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         conocimientoGral = new ConocimientoGeneralRequerimiento();
                         conocimientoGral.IdeConocimientoGeneralRequerimiento = Convert.ToInt32(lector["IDECONOGENSOLREQ"]);
                         conocimientoGral.DescripcionConocimientoOfimatica = Convert.ToString(lector["OFIMATICA"]);
                         conocimientoGral.DescripcionNombreOfimatica = Convert.ToString(lector["DESCOFIMATICA"]);
                         conocimientoGral.DescripcionIdioma = Convert.ToString(lector["IDIOMA"]);
                         conocimientoGral.DescripcionConocimientoIdioma = Convert.ToString(lector["CONOIDIOMA"]);
                         conocimientoGral.DescripcionConocimientoGeneral = Convert.ToString(lector["GENERAL"]);
                         conocimientoGral.DescripcionNombreConocimientoGeneral = Convert.ToString(lector["DESCGENERAL"]);
                         conocimientoGral.DescripcionNivelConocimiento = Convert.ToString(lector["NIVELCONO"]);
                         conocimientoGral.PuntajeConocimiento = Convert.ToInt32(lector["PUNTCONOCIMIENTO"]);
                         listaConocimientoGral.Add(conocimientoGral);
                     }
                     lector.Close();
                 }

                 return listaConocimientoGral;

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

         public List<ExperienciaRequerimiento> ListaExperiencia(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_EXPR_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var experiencia = new ExperienciaRequerimiento();
                 var listaExperiencia = new List<ExperienciaRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         experiencia = new ExperienciaRequerimiento();
                         experiencia.IdeExperienciaRequerimiento = Convert.ToInt32(lector["IDEEXPSOLREQ"]);
                         experiencia.DescripcionExperiencia = Convert.ToString(lector["EXPERIENCIA"]);
                         experiencia.CantidadAnhosExperiencia = Convert.ToInt32(lector["CANTANHOEXP"]);
                         experiencia.CantidadMesesExperiencia = Convert.ToInt32(lector["CANTMESESEXP"]);
                         experiencia.PuntajeExperiencia = Convert.ToInt32(lector["PUNTEXPERIENCIA"]);
                         listaExperiencia.Add(experiencia);
                     }
                     lector.Close();
                 }

                 return listaExperiencia;

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

         public List<EvaluacionRequerimiento> ListaEvaluacion(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_EVAL_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var evaluacion = new EvaluacionRequerimiento();
                 var listaEvaluacion = new List<EvaluacionRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         evaluacion = new EvaluacionRequerimiento();
                         evaluacion.IdeEvaluacionRequerimiento = Convert.ToInt32(lector["IDEEVALUACIONSOLREQ"]);
                         evaluacion.DescripcionExamen = Convert.ToString(lector["NOMEXAMEN"]);
                         evaluacion.DescripcionTipoExamen = Convert.ToString(lector["TIPOEXAMEN"]);
                         evaluacion.DescripcionAreaResponsable = Convert.ToString(lector["NOMAREA"]);
                         evaluacion.PuntajeExamen = Convert.ToInt32(lector["PUNTEXAMEN"]);
                         evaluacion.NotaMinimaExamen = Convert.ToInt32(lector["NOTAMINEXAMEN"]);
                         listaEvaluacion.Add(evaluacion);
                     }
                     lector.Close();
                 }

                 return listaEvaluacion;

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

         public List<DiscapacidadRequerimiento> ListaDiscapacidad(int ideSolicitudReqPersonal)
         {
             OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));

             try
             {
                 lcon.Open();
                 OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_OBTENER_DISCAP_REMP");
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Connection = lcon;

                 cmd.Parameters.Add("p_ideSolicitud", OracleType.Int32).Value = ideSolicitudReqPersonal;
                 cmd.Parameters.Add("p_cRetCursor", OracleType.Cursor).Direction = ParameterDirection.Output;

                 var discapacidad = new DiscapacidadRequerimiento();
                 var listaDiscapacidad = new List<DiscapacidadRequerimiento>();

                 using (IDataReader lector = (OracleDataReader)cmd.ExecuteReader())
                 {

                     while (lector.Read())
                     {
                         discapacidad = new DiscapacidadRequerimiento();
                         discapacidad.IdeDiscapacidadRequerimiento = Convert.ToInt32(lector["IDEDISCAPASOLREQ"]);
                         discapacidad.DescripcionTipoDiscapacidad = Convert.ToString(lector["DESCDISCAP"]);
                         discapacidad.PuntajeDiscapacidad = Convert.ToInt32(lector["PUNTDISCAPA"]);
                         listaDiscapacidad.Add(discapacidad);
                     }
                     lector.Close();
                 }

                 return listaDiscapacidad;

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
