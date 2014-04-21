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

    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ISession session)
            : base(session)
        { 
        }


        public List<Usuario> GetAnalistaRespoanble(SolReqPersonal obj)
        {


            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrUsuario;
                Usuario lobUsuario;
                List<Usuario> llstUsuario;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_OBTIENE_ANALISTA_RESP");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nidsede", OracleType.Int32).Value = obj.IdeSede;
                lspcmd.Parameters.Add("p_crpta", OracleType.Cursor).Direction = ParameterDirection.Output;
                ldrUsuario = (OracleDataReader)lspcmd.ExecuteReader();
                lobUsuario = null;
                llstUsuario = new List<Usuario>();

                


                while (ldrUsuario.Read())
                {
                    lobUsuario = new Usuario();

                    if (ldrUsuario["IDUSUARIO"]!=DBNull.Value)
                    {
                        lobUsuario.IdUsuario = Convert.ToInt32(ldrUsuario["IDUSUARIO"]);    
                    }
                    if (ldrUsuario["NOMBRE"] != DBNull.Value)
                    {

                        lobUsuario.NombreUsuario = Convert.ToString(ldrUsuario["NOMBRE"]);
                    }
                    
                    

                    llstUsuario.Add(lobUsuario);

                }
                ldrUsuario.Close();
                return llstUsuario;


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