namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;

    public interface IConocimientoGeneralCargoRepository : IRepository<ConocimientoGeneralCargo>
    {
        void actualizarPuntaje(int valor, int valorEliminado, int IdeCargo, string tipoConocimiento);
        
    }
}