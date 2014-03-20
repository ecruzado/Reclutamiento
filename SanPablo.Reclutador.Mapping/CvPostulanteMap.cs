

namespace SanPablo.Reclutador.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Entity;


    public class CvPostulanteMap : ClassMap<CvPostulante>
    {

        public CvPostulanteMap()
        {
            Id(m => m.IdCvPostulante, "IDCVPOSTULANTE")
                .GeneratedBy
                .Sequence("IDCVPOSTULANTE_SQ");
            Map(x => x.ApePaterno, "APEPATERNO");
            Map(x => x.ApeMaterno, "APEMATERNO");
            Map(x => x.Dni, "DNI");
            Map(x => x.Telefono, "TELEFONO");
            Map(x => x.Fechacita, "FECHACITA");
            Map(x => x.HoraCita, "HORACITA");
            Map(x => x.Citado, "CITADO");
            Map(x => x.Asistio, "ASISTIO");

            Map(x => x.FecCreacion, "FECCREACION");
            Map(x => x.FecModificacion, "FECMODIFICACION");
            Map(x => x.UsrCreacion, "USRCREACION");
            Map(x => x.UsrModifcacion, "USRMODIFCACION");
            Map(x => x.IdSolicitud, "IDSOLICITUD");
            Map(x => x.TipSol, "TIPOSOL");
            
            Map(x => x.Nombre, "NOMBRE");
            
            

            Table("CVPOSTULANTE");

        }

    }
}
