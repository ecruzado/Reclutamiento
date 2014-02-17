namespace SanPablo.Reclutador.Repository
{
    using NHibernate;
    using SanPablo.Reclutador.Entity;
    using SanPablo.Reclutador.Repository.Interface;
    using System;
    using System.Data;
    using System.Data.OracleClient;

    public class ConocimientoGeneralCargoRepository : Repository<ConocimientoGeneralCargo>, IConocimientoGeneralCargoRepository
    {
        public ConocimientoGeneralCargoRepository(ISession session)
            : base(session)
        { 
        }
        public void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo, string tipoConocimiento)
        {
            string nombreCampo = "";
            if (tipoConocimiento == "Ofimatica")
                nombreCampo = "PUNTTOTOFIMATI";
            else
            {    if (tipoConocimiento == "Idioma")
                    nombreCampo = "PUNTTOTIDIOMA";
                else
                    nombreCampo = "PUNTTOTCONOGEN";
            }
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_ACTUALIZAR_PUNTAJES");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_nombreCampoDestino", OracleType.VarChar).Value = nombreCampo;
                cmd.Parameters.Add("p_ideCargo", OracleType.Int32).Value = IdeCargo;
                cmd.Parameters.Add("p_valor", OracleType.Int32).Value = valor;
                cmd.Parameters.Add("p_valorEliminar", OracleType.Int32).Value = valorEliminado;
                cmd.ExecuteNonQuery();
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