namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;

    public interface ICentroEstudioCargoRepository : IRepository<CentroEstudioCargo>
    {
        void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo);
    }
}