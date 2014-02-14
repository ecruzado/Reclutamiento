namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;

    public interface IRolOpcionRepository : IRepository<RolOpcion>
    {

        int EliminaOpcion(int idRol, int idOpcion);
    }
}