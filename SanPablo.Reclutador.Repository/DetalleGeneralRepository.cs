namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using NHibernate.Criterion;
    using System;
    using System.Data.OracleClient;
    using System.Data;
   
    public class DetalleGeneralRepository : Repository<DetalleGeneral>, IDetalleGeneralRepository
    {
        public DetalleGeneralRepository(ISession session)
            : base(session)
        { 
        }

        public IList<DetalleGeneral> GetByTipoTabla(TipoTabla tipoTabla)
        {
            var lista = GetBy(x => x.IdeGeneral == (int)tipoTabla 
                && x.EstadoActivo == IndicadorActivo.Activo && x.Referencia == null);
            return lista;
        }

        public IList<DetalleGeneral> GetByTableReference(TipoTabla tipoTabla, String referencia)
        {
            var lista = GetBy(x => x.IdeGeneral == (int)tipoTabla
                           && x.EstadoActivo == IndicadorActivo.Activo && x.Referencia == referencia);
             
            return lista;
        }

        public string GetByTableDescription(TipoTabla tipoTabla, String valor)
        {
            var lista = GetSingle(x => x.IdeGeneral == (int)tipoTabla
                           && x.EstadoActivo == IndicadorActivo.Activo && x.Valor == valor);

            return lista.Descripcion;
        }

        /// <summary>
        /// Inserta un detalle nuevo 
        /// </summary>
        /// <param name="detalle"></param>
        /// <returns></returns>
        public int insertarDetalle(DetalleGeneral detalle)
        {
            if (detalle.Referencia == null)
            {
                detalle.Referencia = "";
            }
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                var usuario = detalle.UsuarioCreacion;
                detalle.UsuarioCreacion = usuario.Length <= 15 ? usuario : usuario.Substring(0, 15);

                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_AGREGAR_DETALLE");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_ideGeneral", OracleType.Int32).Value = detalle.General.IdeGeneral;
                cmd.Parameters.Add("p_valor", OracleType.VarChar).Value = detalle.Valor;
                cmd.Parameters.Add("p_descripcion", OracleType.VarChar).Value = detalle.Descripcion;
                cmd.Parameters.Add("p_referencia", OracleType.VarChar).Value = detalle.Referencia;
                cmd.Parameters.Add("p_usrCreacion", OracleType.VarChar).Value = detalle.UsuarioCreacion;
                cmd.Parameters.Add("p_retVal", OracleType.Int32).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["p_retVal"].Value);
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