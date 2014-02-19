

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


    public class UsuarioRolSedeRepository : Repository<UsuarioRolSede>, IUsuarioRolSedeRepository
    {
        public UsuarioRolSedeRepository(ISession session)
            : base(session)
        {
        }

        public List<Rol> GetListaRol(int idUsuario)
        {


            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
               
                IDataReader ldrRol;
                Rol lobRol;
                List<Rol> llstRol;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_ROLXUSUARIO");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nIdUsua", OracleType.Int32).Value = idUsuario;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                ldrRol = (OracleDataReader)lspcmd.ExecuteReader();
                lobRol = null;
                llstRol = new List<Rol>();


                while (ldrRol.Read())
                {
                    lobRol = new Rol();

                    lobRol.IdRol = Convert.ToInt32(ldrRol["IDROL"]);
                    lobRol.CodRol = Convert.ToString(ldrRol["CODIGOROL"]);
                    
                    llstRol.Add(lobRol);

                }
                ldrRol.Close();
                return llstRol;


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
        /// obtiene las sedes por codigo de usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        public List<Sede> GetListaSede(int idUsuario,int codRol)
        {


            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {

                IDataReader ldrSede;
                Sede lobSede;
                List<Sede> llstSede;
                lcon.Open();
                OracleCommand lspcmd = new OracleCommand("PR_INTRANET.FN_GET_SEDEXUSUARIO");
                lspcmd.CommandType = CommandType.StoredProcedure;
                lspcmd.Connection = lcon;
                lspcmd.Parameters.Add("p_nIdUsua", OracleType.Int32).Value = idUsuario;
                lspcmd.Parameters.Add("p_nIdRol", OracleType.Int32).Value = codRol;
                lspcmd.Parameters.Add("p_cRetVal", OracleType.Cursor).Direction = ParameterDirection.Output;
                ldrSede = (OracleDataReader)lspcmd.ExecuteReader();
                lobSede = null;
                llstSede = new List<Sede>();


                while (ldrSede.Read())
                {
                    lobSede = new Sede();

                    lobSede.CodigoSede = Convert.ToString(ldrSede["IDESEDE"]);
                    lobSede.DescripcionSede = Convert.ToString(ldrSede["DESCRIPCION"]);

                    llstSede.Add(lobSede);

                }

                ldrSede.Close();
                return llstSede;


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
