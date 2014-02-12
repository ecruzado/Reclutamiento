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
    //using oracle = Oracle.DataAccess.Client;
    //using Oracle.DataAccess.Types;


    public class EvaluacionCargoRepository : Repository<EvaluacionCargo>, IEvaluacionCargoRepository
    {
        public EvaluacionCargoRepository(ISession session)
            : base(session)
        { 
        }
        /// <summary>
        /// obtiene el tiempo del examen
        /// </summary>
        /// <param name="idExamen"></param>
        /// <returns></returns>
       

    }
}