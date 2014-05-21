namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;

    public class ListaSolicitudNuevoCargoVistaRepository : Repository<ListaSolicitudNuevoCargo>, IListaSolicitudNuevoCargoVistaRepository
    {
        public ListaSolicitudNuevoCargoVistaRepository(ISession session)
            : base(session)
        { 
        }


        public List<SolicitudConsulta> ListaSolicitudesRequerimientos(SolReqPersonal busqueda)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {


                string cFechaIncial = busqueda.FechaInicioBus == null ? "" : String.Format("{0:dd/MM/yyyy}", busqueda.FechaInicioBus);
                string cFechaFinal = busqueda.FechaFinBus == null ? "" : String.Format("{0:dd/MM/yyyy}", busqueda.FechaFinBus);


                IDataReader drSolicitudConsulta ;
                SolicitudConsulta solicitudConsulta;
                List<SolicitudConsulta> listaSolicitudConsulta;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_REQUERIMIENTOS.SP_LISTA_SOLGRAL");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nIdCargo", OracleType.Int32).Value = busqueda.IdeCargo;
                lspcmd.Parameters.Add("p_ideSede", OracleType.Int32).Value = busqueda.IdeSede;
                lspcmd.Parameters.Add("p_nIdDependencia", OracleType.Int32).Value = busqueda.IdeDependencia;
                lspcmd.Parameters.Add("p_nIdDepartamento", OracleType.Int32).Value = busqueda.IdeDepartamento;
                lspcmd.Parameters.Add("p_nIdArea", OracleType.Int32).Value = busqueda.IdeArea;
                lspcmd.Parameters.Add("p_cTipEtapa", OracleType.VarChar).Value = busqueda.TipEtapa;
                lspcmd.Parameters.Add("p_cTipResp", OracleType.VarChar).Value = busqueda.TipResponsable;
                lspcmd.Parameters.Add("p_cEstado", OracleType.VarChar).Value = busqueda.TipEstado;
                lspcmd.Parameters.Add("p_cFecIni", OracleType.VarChar).Value = cFechaIncial;
                lspcmd.Parameters.Add("p_cFeFin", OracleType.VarChar).Value = cFechaFinal;
                lspcmd.Parameters.Add("p_cTipoSolicitud", OracleType.VarChar).Value = busqueda.TipoSolicitud;
                lspcmd.Parameters.Add("p_cCodSolicitud", OracleType.VarChar).Value = busqueda.CodSolReqPersonal;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;

                drSolicitudConsulta = (OracleDataReader)lspcmd.ExecuteReader();
                
                solicitudConsulta = null;
                
                listaSolicitudConsulta = new List<SolicitudConsulta>();


                while (drSolicitudConsulta.Read())
                {
                    solicitudConsulta = new SolicitudConsulta();
                    solicitudConsulta.IdeSolicitud = Convert.ToInt32(drSolicitudConsulta["IDESOLICITUD"]);
                    solicitudConsulta.EstadoSolicitud = Convert.ToString(drSolicitudConsulta["ESTADO"]);
                    solicitudConsulta.CodigoSolicitud = Convert.ToString(drSolicitudConsulta["CODIGO"]);
                    solicitudConsulta.IdeCargo = Convert.ToInt32(drSolicitudConsulta["IDECARGO"]);
                    solicitudConsulta.NombreCargo = Convert.ToString(drSolicitudConsulta["NOMCARGO"]);
                    solicitudConsulta.IdeDependencia = Convert.ToInt32(drSolicitudConsulta["IDEDEPENDENCIA"]);
                    solicitudConsulta.NombreDependencia = Convert.ToString(drSolicitudConsulta["NOMDEPENDENCIA"]);
                    solicitudConsulta.IdeDepartamento = Convert.ToInt32(drSolicitudConsulta["IDEDEPARTAMENTO"]);
                    solicitudConsulta.NombreDepartamento = Convert.ToString(drSolicitudConsulta["NOMDEPARTAMENTO"]);
                    solicitudConsulta.IdeArea = Convert.ToInt32(drSolicitudConsulta["IDEAREA"]);
                    solicitudConsulta.NombreArea = Convert.ToString(drSolicitudConsulta["NOMAREA"]);

                    solicitudConsulta.NumeroVacantes = Convert.ToInt32(drSolicitudConsulta["NUMVACANTES"]);
                    solicitudConsulta.Postulantes = Convert.ToInt32(drSolicitudConsulta["POSTULANTES"]);
                    solicitudConsulta.Preseleccionados = Convert.ToInt32(drSolicitudConsulta["PRESELECCIONADOS"]);
                    solicitudConsulta.Evaluados = Convert.ToInt32(drSolicitudConsulta["EVALUADOS"]);
                    solicitudConsulta.Seleccionados = Convert.ToInt32(drSolicitudConsulta["SELECCIONADOS"]);
                    solicitudConsulta.Contratados = Convert.ToInt32(drSolicitudConsulta["CONTRATADOS"]);
                    var fecCreacion = Convert.ToString(drSolicitudConsulta["INICIO"]);
                    if (fecCreacion.Length > 0)
                    {
                        solicitudConsulta.FechaCreacion = Convert.ToDateTime(drSolicitudConsulta["INICIO"]);
                    }

                    //fecha de cierre

                    solicitudConsulta.IdeRolResponsable = Convert.ToInt32(drSolicitudConsulta["IDRESPONSABLE"]);
                    solicitudConsulta.RolResponsable = Convert.ToString(drSolicitudConsulta["RESPONSABLE"]);
                    solicitudConsulta.NombreResponsable = Convert.ToString(drSolicitudConsulta["NOMRESPONSABLE"]);



                    var fecPublicacion = Convert.ToString(drSolicitudConsulta["FECPUBLICACION"]);
                    if (fecPublicacion.Length > 0)
                    {
                        solicitudConsulta.FechaPublicacion = Convert.ToDateTime(drSolicitudConsulta["FECPUBLICACION"]);
                    }



                    var fecExpiracion = Convert.ToString(drSolicitudConsulta["FECEXPIRACION"]);
                    if (fecExpiracion.Length > 0)
                    {
                        solicitudConsulta.FechaExpiracion = Convert.ToDateTime(drSolicitudConsulta["FECEXPIRACION"]);
                    }

                    solicitudConsulta.Publicado = Convert.ToString(drSolicitudConsulta["PUBLICADO"]);
                    solicitudConsulta.TipoEtapa = Convert.ToString(drSolicitudConsulta["TIPETAPA"]);
                    solicitudConsulta.Etapa = Convert.ToString(drSolicitudConsulta["ETAPA"]);
                    solicitudConsulta.TipoSolicitud = Convert.ToString(drSolicitudConsulta["TIPSOL"]);
                    solicitudConsulta.NombreTipoSolicitud = Convert.ToString(drSolicitudConsulta["TIPOSOLICITUD"]);
                    
                    listaSolicitudConsulta.Add(solicitudConsulta);
                }
                //drSolicitudConsulta.Dispose();
                drSolicitudConsulta.Close();

                return listaSolicitudConsulta;
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
