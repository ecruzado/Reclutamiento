namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public interface IAreaRepository : IRepository<Area>
    {
        List<string> obtenerDatosArea(int ideArea);
    }
}