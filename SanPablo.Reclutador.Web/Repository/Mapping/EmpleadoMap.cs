namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class EmpleadoMap : ClassMap<Empleado>
    {
        public EmpleadoMap()
        {
            Id(m => m.CodigoEmpleado, "IDEEMPLEADO");
            Map(x => x.CodigoCargo, "ID_CARGO");
            Map(x => x.TipoDocumentoIdentidad, "TIPDOCUMENTO");
            Map(x => x.NumeroDocumentoIdentidad, "NUMDOCUMENTO");
            Map(x => x.ApellidoPaterno, "APEPATERNO");
            Map(x => x.ApellidoMaterno, "APEMATERNO");
            Map(x => x.Nombres, "NOMBRES");
            Map(x => x.Correo, "CORREO");
            References(x => x.Sede).Column("IDESEDE");
            /*HasMany(x => x.Usuarios)
                .Inverse()
                .Cascade.all();*/
            Table("EMPLEADO");
        }
    }
}
