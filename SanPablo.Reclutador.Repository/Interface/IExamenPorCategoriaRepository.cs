namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public interface IExamenPorCategoriaRepository : IRepository<ExamenPorCategoria>
    {
        List<ExamenPorCategoria> ListarExamenesPorCategoria(int ideReclutaPersona, int ideSede);
    }
}