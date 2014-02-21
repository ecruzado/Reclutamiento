﻿namespace SanPablo.Reclutador.Repository
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
    //using oracle = Oracle.DataAccess.Client;
    //using Oracle.DataAccess.Types;


    public class EvaluacionCargoRepository : Repository<EvaluacionCargo>, IEvaluacionCargoRepository
    {
        public EvaluacionCargoRepository(ISession session)
            : base(session)
        { 
        }

        public void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo)
        {
            OracleConnection lcon = new OracleConnection(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DbDevConnectionString"]));
            try
            {
                lcon.Open();
                OracleCommand cmd = new OracleCommand("PR_INTRANET.SP_ACTUALIZAR_PUNTAJES");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = lcon;

                cmd.Parameters.Add("p_nombreCampoDestino", OracleType.VarChar).Value = "PUNTTOTEXAMEN";
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