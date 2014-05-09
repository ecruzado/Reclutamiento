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

        /// <summary>
        /// obtiene los usuarios de intranet
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<UsuarioVista> GetUsuarioVista(UsuarioVista obj)
        {


            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrUsuarioVista;
                UsuarioVista lobUsuarioVista;
                List<UsuarioVista> llstUsuarioVista;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET_ED.SP_OBTIENE_USUARIOS");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                
                lspcmd.Parameters.Add("p_nidrol", OracleType.Number).Value = obj.IDROL;
                lspcmd.Parameters.Add("p_nidsede", OracleType.Number).Value = obj.IDESEDE;
                lspcmd.Parameters.Add("p_cnombres", OracleType.VarChar).Value = (obj.DSCNOMBRES==null?"":obj.DSCNOMBRES.Trim());
                lspcmd.Parameters.Add("p_ccodusuario", OracleType.VarChar).Value = (obj.CODUSUARIO==null?"":obj.CODUSUARIO.Trim());
                lspcmd.Parameters.Add("p_cdesapepat", OracleType.VarChar).Value = (obj.DSCAPEPATERNO==null?"":obj.DSCAPEPATERNO.Trim());
                lspcmd.Parameters.Add("p_cdesapemat", OracleType.VarChar).Value = (obj.DSCAPEMATERNO==null?"":obj.DSCAPEMATERNO.Trim());

                lspcmd.Parameters.Add("p_crpta", OracleType.Cursor).Direction = ParameterDirection.Output;
                ldrUsuarioVista = (OracleDataReader)lspcmd.ExecuteReader();
                lobUsuarioVista = null;
                llstUsuarioVista = new List<UsuarioVista>();


                while (ldrUsuarioVista.Read())
                {
                    lobUsuarioVista = new UsuarioVista();

                    if (ldrUsuarioVista["FLGESTADO"] != DBNull.Value)
                    {
                        lobUsuarioVista.FLGESTADO = Convert.ToString(ldrUsuarioVista["FLGESTADO"]);
                    }
                    if (ldrUsuarioVista["IDUSUARIO"] != DBNull.Value)
                    {

                        lobUsuarioVista.IDUSUARIO = Convert.ToInt32(ldrUsuarioVista["IDUSUARIO"]);
                    }
                    if (ldrUsuarioVista["CODUSUARIO"] != DBNull.Value)
                    {

                        lobUsuarioVista.CODUSUARIO = Convert.ToString(ldrUsuarioVista["CODUSUARIO"]);
                    }
                    if (ldrUsuarioVista["DSCNOMBRES"] != DBNull.Value)
                    {

                        lobUsuarioVista.DSCNOMBRES = Convert.ToString(ldrUsuarioVista["DSCNOMBRES"]);
                    }
                    if (ldrUsuarioVista["DSCAPEPATERNO"] != DBNull.Value)
                    {

                        lobUsuarioVista.DSCAPEPATERNO = Convert.ToString(ldrUsuarioVista["DSCAPEPATERNO"]);
                    }
                    if (ldrUsuarioVista["DSCAPEMATERNO"] != DBNull.Value)
                    {

                        lobUsuarioVista.DSCAPEMATERNO = Convert.ToString(ldrUsuarioVista["DSCAPEMATERNO"]);
                    }
                    if (ldrUsuarioVista["DESROL"] != DBNull.Value)
                    {

                        lobUsuarioVista.DESROL = Convert.ToString(ldrUsuarioVista["DESROL"]);
                    }
                    if (ldrUsuarioVista["DESSEDE"] != DBNull.Value)
                    {

                        lobUsuarioVista.DESSEDE = Convert.ToString(ldrUsuarioVista["DESSEDE"]);
                    }
                    

                    llstUsuarioVista.Add(lobUsuarioVista);

                }
                ldrUsuarioVista.Close();
                return llstUsuarioVista;

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