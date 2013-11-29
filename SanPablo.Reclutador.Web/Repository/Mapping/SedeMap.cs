﻿namespace SanPablo.Reclutador.Web.Repository.Mapping
{
    using FluentNHibernate.Mapping;
    using SanPablo.Reclutador.Web.Entity;

    public class SedeMap : ClassMap<Sede>
    {
        public SedeMap()
        {
            /*Id(m => m.SedeId);*/
            Id(m => m.CodigoSede, "IDESEDE");
            Map(x => x.DescripcionSede, "DESCRIPCION");
            Map(x => x.EstadoRegistro, "ESTREGISTRO");
            Table("SEDE");
        }
    }
}