namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Collections.Generic;

    public interface ICategoriaRepository : IRepository<Categoria>
    {

        List<Categoria> ObtenerCategorias(Categoria obj);
    }
}
