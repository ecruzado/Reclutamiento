using System;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Validator.Constraints;

namespace SanPablo.Reclutador.Entity
{
  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class General
  {
    public virtual int Idegeneral { get; set; }
    [NotNull]
    [Length(Max=15)]
    public virtual string Tiptabla { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string Descripcion { get; set; }
    [NotNull]
    [Length(Max=8)]
    public virtual string Tipdato { get; set; }
    [NotNull]
    [Length(Max=3)]
    public virtual string Loncampo { get; set; }
    [Length(Max=15)]
    public virtual string Usrcreacion { get; set; }
    public virtual System.Nullable<System.DateTime> Feccreacion { get; set; }
    [Length(Max=15)]
    public virtual string Usrmodificacion { get; set; }
    public virtual System.Nullable<System.DateTime> Fecmodificacion { get; set; }

    private IList<DetalleGeneral> _detalleGenerals = new List<DetalleGeneral>();

    public virtual IList<DetalleGeneral> DetalleGenerals
    {
      get { return _detalleGenerals; }
      set { _detalleGenerals = value; }
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(General).Assembly.GetName().Name + @"'
                   namespace='SanPablo.Reclutador.Entity'
                   >
  <class name='General'
         table='`GENERAL`'
         >
    <id name='Idegeneral'
        column='`IDEGENERAL`'
        >
      <generator class='sequence'>
        <param name='sequence'>BVB</param>
      </generator>
    </id>
    <property name='Tiptabla'
              column='`TIPTABLA`'
              />
    <property name='Descripcion'
              column='`DESCRIPCION`'
              />
    <property name='Tipdato'
              column='`TIPDATO`'
              />
    <property name='Loncampo'
              column='`LONCAMPO`'
              />
    <property name='Usrcreacion'
              column='`USRCREACION`'
              />
    <property name='Feccreacion'
              column='`FECCREACION`'
              />
    <property name='Usrmodificacion'
              column='`USRMODIFICACION`'
              />
    <property name='Fecmodificacion'
              column='`FECMODIFICACION`'
              />
    <bag name='DetalleGenerals'
          inverse='false'
          >
      <key column='`IdegeneralId`' />
      <one-to-many class='DetalleGeneral' />
    </bag>
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }

  [System.CodeDom.Compiler.GeneratedCode("NHibernateModelGenerator", "1.0.0.0")]
  public partial class DetalleGeneral
  {
    public virtual int Idegeneral { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string Valor { get; set; }
    [NotNull]
    [Length(Max=50)]
    public virtual string Descripcion { get; set; }
    [NotNull]
    [Length(Max=1)]
    public virtual string Indactivo { get; set; }
    [Length(Max=15)]
    public virtual string Usrcreacion { get; set; }
    public virtual System.Nullable<System.DateTime> Feccreacion { get; set; }
    [Length(Max=15)]
    public virtual string Usrmodifica { get; set; }
    public virtual System.Nullable<System.DateTime> Fecmodifica { get; set; }

    public virtual General General { get; set; }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != GetType())
      {
        return false;
      }

      DetalleGeneral other = (DetalleGeneral)obj;
      if (other.Idegeneral != Idegeneral)
      {
        return false;
      }
      if (other.Valor != Valor)
      {
        return false;
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hashCode = 0;
      hashCode = 19 * hashCode + Idegeneral.GetHashCode();
      hashCode = 19 * hashCode + Valor.GetHashCode();
      return hashCode;
    }

    static partial void CustomizeMappingDocument(System.Xml.Linq.XDocument mappingDocument);

    internal static System.Xml.Linq.XDocument MappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(DetalleGeneral).Assembly.GetName().Name + @"'
                   namespace='SanPablo.Reclutador.Entity'
                   >
  <class name='DetalleGeneral'
         table='`DETALLE_GENERAL`'
         >
    <composite-id>
      <key-property name='Idegeneral'
                    column='`IDEGENERAL`'
                    />
      <key-property name='Valor'
                    column='`VALOR`'
                    />
    </composite-id>
    <property name='Descripcion'
              column='`DESCRIPCION`'
              />
    <property name='Indactivo'
              column='`INDACTIVO`'
              />
    <property name='Usrcreacion'
              column='`USRCREACION`'
              />
    <property name='Feccreacion'
              column='`FECCREACION`'
              />
    <property name='Usrmodifica'
              column='`USRMODIFICA`'
              />
    <property name='Fecmodifica'
              column='`FECMODIFICA`'
              />
    <many-to-one name='General' class='General' column='`IdegeneralId`' />
  </class>
</hibernate-mapping>");
        CustomizeMappingDocument(mappingDocument);
        return mappingDocument;
      }
    }
  }


  /// <summary>
  /// Provides a NHibernate configuration object containing mappings for this model.
  /// </summary>
  public static class ConfigurationHelper
  {
    /// <summary>
    /// Creates a NHibernate configuration object containing mappings for this model.
    /// </summary>
    /// <returns>A NHibernate configuration object containing mappings for this model.</returns>
    public static Configuration CreateConfiguration()
    {
      var configuration = new Configuration();
      configuration.Configure();
      ApplyConfiguration(configuration);
      return configuration;
    }

    /// <summary>
    /// Adds mappings for this model to a NHibernate configuration object.
    /// </summary>
    /// <param name="configuration">A NHibernate configuration object to which to add mappings for this model.</param>
    public static void ApplyConfiguration(Configuration configuration)
    {
      configuration.AddXml(ModelMappingXml.ToString());
      configuration.AddXml(General.MappingXml.ToString());
      configuration.AddXml(DetalleGeneral.MappingXml.ToString());
      configuration.AddAssembly(typeof(ConfigurationHelper).Assembly);
    }

    /// <summary>
    /// Provides global mappings not associated with a specific entity.
    /// </summary>
    public static System.Xml.Linq.XDocument ModelMappingXml
    {
      get
      {
        var mappingDocument = System.Xml.Linq.XDocument.Parse(@"<?xml version='1.0' encoding='utf-8' ?>
<hibernate-mapping xmlns='urn:nhibernate-mapping-2.2'
                   assembly='" + typeof(ConfigurationHelper).Assembly.GetName().Name + @"'
                   namespace='SanPablo.Reclutador.Entity'
                   >
</hibernate-mapping>");
        return mappingDocument;
      }
    }
  }
}
