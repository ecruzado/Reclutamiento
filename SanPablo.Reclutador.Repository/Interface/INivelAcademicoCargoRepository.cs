namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;

    public interface INivelAcademicoCargoRepository : IRepository<NivelAcademicoCargo>
    {
        void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo);
    }
}