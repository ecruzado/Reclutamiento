namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
    using System.Data;

    public interface ICargoRepository : IRepository<Cargo>
    {
        DatosCargo obtenerDatosCargo(int IdeSolicitud);
    }
}