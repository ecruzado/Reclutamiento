﻿namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Data.OracleClient;
    using System.Configuration;
    using System.Collections;

    using System.Linq;
    using System.Transactions;

    public interface IEvaluacionCargoRepository : IRepository<EvaluacionCargo>
    {
        void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo);
    }
}