﻿namespace SanPablo.Reclutador.Repository.Interface
{
    using SanPablo.Reclutador.Entity;
using System;
using System.Collections.Generic;
using System.Data;

    public interface ICargoRepository : IRepository<Cargo>
    {
        DatosCargo obtenerDatosCargo(int IdeSolicitud, string IdeUSuarioCreacion, int IdeSede);

        /// <summary>
        /// CREA UNA COPIA DE CARGO PARA REALIZAR MODIFICACIONES
        /// </summary>
        /// <param name="ideCargo"></param>
        /// <param name="IdeUSuarioCreacion"></param>
        int mantenimientoCargo(int ideCargo, string IdeUSuarioCreacion);


        /// <summary>
        /// lista de cargos para lista desplegable de ampliacion
        /// lista por sede y cargos con perfil completo
        /// </summary>
        /// <param name="IdeSede"></param>
        /// <returns></returns>
        List<Cargo> listaCargosCompletos(int IdeSede);

        /// <summary>
        /// Lista de cargos devuelve idecargo, codcargo, nomcargo
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
         List<Cargo> listarCargosSedeCodigo(int ideSede);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cargo"></param>
        /// <param name="estado"></param>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        List<ListaSolicitudNuevoCargo> listaCargosMantenimiento(Cargo cargo, string estado, int ideSede);

        /// <summary>
        /// Consultar la etapa del cargo
        /// </summary>
        /// <param name="ideCargo"></param>
        /// <returns></returns>
        string consultaEditarCargo(int ideCargo);

        /// <summary>
        /// Lista de cargos por determinada Sede
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        List<Cargo> listarCargosSede(int ideSede);


        /// <summary>
        /// CARGOS POR AREA DEPENDENCIA Y SEDE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<Cargo> GetCargoxSede(Cargo obj);

        /// <summary>
        /// CARGOS POR SEDE
        /// </summary>
        /// <param name="ideSede"></param>
        /// <returns></returns>
        List<Cargo> listarCargosSedeConsulta(int ideSede);
    }
}