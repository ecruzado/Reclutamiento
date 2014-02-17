namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;

    public interface IDiscapacidadCargoRepository : IRepository<DiscapacidadCargo>
    {
        void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo);
    }
}