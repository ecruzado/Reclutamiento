using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SanPablo.Reclutador.Repository;
using SanPablo.Reclutador.Entity;
using NHibernate.Criterion;
using System.Linq;
using SanPablo.Reclutador.Repository.Interface;

namespace SanPablo.Reclutador.Test.Repository
{
    [TestClass]
    public class CriterioRepositoryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var repository = new CriterioRepository(NHibernateHelper.OpenSession());
            var criterio = new Criterio();
            criterio.TipoMedicion = "02";
            criterio.TipoModo = "02";
            criterio.TipoCriterio = "02";
            criterio.Pregunta = "02";
            criterio.TipoCalificacion = "02";
            criterio.FechaCreacion = DateTime.Now;
            repository.Add(criterio);
        }

         [TestMethod]
          public void TestMethod3()
          {
           
              var repository = new AlternativaRepository(NHibernateHelper.OpenSession());
              var alternativa = repository.GetSingle(x => x.IdeAlternativa == 24);
              repository.Remove(alternativa);

          }

        /*
          [TestMethod]
          public void TestMethod2()
          {
              var repository = new AlternativaRepository(NHibernateHelper.OpenSession());

              DetachedCriteria where = DetachedCriteria.For<Alternativa>();
              where.Add(Expression.Like("IdCriterio",27));

              var lista = repository.GetPaging("IdeAlternativa", true, 0, 100, where);
          }
          */
        /*  [TestMethod]
          public void TestMethod3()
          {
           
              var repository = new AlternativaRepository(NHibernateHelper.OpenSession());
              var alternativa = repository.GetSingle(x => x.IdeAlternativa == 23);
              alternativa.NombreAlternativa = "cambio";
              repository.Update(alternativa);

          }*/

        [TestMethod]
        public void Test_DetachedCriteria()
        {
            //var repository = new CriterioRepository(NHibernateHelper.OpenSession());

            //DetachedCriteria where = DetachedCriteria.For<Criterio>();
            //where.Add(Expression.Eq("Criterio.IdeCriterio", 52));

            //var lista = repository.GetPaging("IdeCriterio", true, 0, 100, null);
            //var session = NHibernateHelper.OpenSession();
            //var logs = session.CreateQuery("Select C, D from Criterio C, DetalleGeneral D where C.TipoMedicion = D.Valor and D.IdeGeneral = 1")
            //    .List();
            var repo = new CriterioPorSubcategoriaRepository(NHibernateHelper.OpenSession());
            var list = repo.All();
            foreach (var item in list)
            {
                var test = item.Criterio.Pregunta;
            }

        }

        [TestMethod]
        public void Test_formula()
        {
            //var repository = new CriterioRepository(NHibernateHelper.OpenSession());

            //var lista = repository.GetPaging("IdeCriterio", true, 0, 100, null);
            var repository = new CompetenciasCargoRepository(NHibernateHelper.OpenSession());
            var cargoRepository = new CargoRepository(NHibernateHelper.OpenSession());
            var cargo = cargoRepository.GetSingle(x => x.IdeCargo == 1);
            CompetenciaCargo competenciaCargo = new CompetenciaCargo();
            competenciaCargo.EstadoActivo = "A";
            competenciaCargo.FechaCreacion = DateTime.Now;
            competenciaCargo.UsuarioCreacion = "YO";
            //competenciaCargo.Cargo = new Cargo();
            //competenciaCargo.Cargo.IdeCargo = 1;
            //competenciaCargo.Cargo = cargo;
            cargo.agregarCompetencia(competenciaCargo);

            
            repository.Add(competenciaCargo);
        }

        [TestMethod]
        public void test_sub_categoria_por_criterio() 
        {
            var repository = new RolRepository(NHibernateHelper.OpenSession());
            //var where = DetachedCriteria.For<RolOpcion>();
            //where.Add(Expression.Eq("Rol.IdRol", 1));

            //var lista = repository.GetPaging("IDROLOPCION", true, 0, 100,where);
            Rol entidad = new Rol();
            entidad.FlgEstado = "A";
            entidad.FlgSede = "S";
            entidad.FechaCreacion = DateTime.Today;
            entidad.UsuarioCreacion = "system";
            entidad.CodRol = "roltest";
            repository.Add(entidad);
        }
    }
}
