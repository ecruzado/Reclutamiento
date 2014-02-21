﻿namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public interface ISolicitudNuevoCargoRepository : IRepository<SolicitudNuevoCargo>
    {
        List<string> obtenerDatosArea(int ideArea);
    }
}