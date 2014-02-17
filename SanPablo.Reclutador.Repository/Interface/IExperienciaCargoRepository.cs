namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;

    public interface IExperienciaCargoRepository : IRepository<ExperienciaCargo>
    {
        void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo);
    }
}