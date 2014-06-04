create or replace package PR_INTRANET_ED is

  type cur_cursor is REF CURSOR;

  --estados del postulantes
  P_POSTREGISTRADO           CONSTANT VARCHAR2(2) := '01';
  P_POSTPRESELEC_AUTOMATICO  CONSTANT VARCHAR2(2) := '02';
  P_POSTPRESELEC_MANUAL      CONSTANT VARCHAR2(2) := '03';
  P_POSTNO_PRESELECCIONADO   CONSTANT VARCHAR2(2) := '04';
  P_POSTEXCLUIDO             CONSTANT VARCHAR2(2) := '05';
  P_POSTNO_APTO              CONSTANT VARCHAR2(2) := '06';
  P_POSTEN_EVALUACION        CONSTANT VARCHAR2(2) := '07';
  P_POSTSELECCIONADO         CONSTANT VARCHAR2(2) := '08';
  P_POSTCONTRATADO           CONSTANT VARCHAR2(2) := '09';
  P_POSTPOSTULANTE_POTENCIAL CONSTANT VARCHAR2(2) := '10';
  P_POSTFINALIZADO           CONSTANT VARCHAR2(2) := '11';

  --TIPOS DE SOLICITUD
  P_TIPSOLNUEVO      CONSTANT VARCHAR2(2) := '01';
  P_TIPSOLAMPLIACION CONSTANT VARCHAR2(2) := '02';
  P_TIPSOLREEMPLAZO  CONSTANT VARCHAR2(2) := '03';

  --ESTADOS DE LA SOLICITUD
  P_SOLPENDIENTE         CONSTANT VARCHAR2(2) := '01';
  P_SOLVALIDADO          CONSTANT VARCHAR2(2) := '02';
  P_SOLAPROBADO          CONSTANT VARCHAR2(2) := '03';
  P_SOLPUBLICADO         CONSTANT VARCHAR2(2) := '04';
  P_SOLGENERACION_PERFIL CONSTANT VARCHAR2(2) := '05';
  P_SOLAPROBACION_PERFIL CONSTANT VARCHAR2(2) := '06';
  P_SOLOBSERVADO         CONSTANT VARCHAR2(2) := '07';
  P_SOLFINALIZADO        CONSTANT VARCHAR2(2) := '08';
  P_SOLRECHAZADO         CONSTANT VARCHAR2(2) := '09';

  --NIVEL DE CONOCIMIENTO
  P_NIVELBASICO     CONSTANT VARCHAR2(2) := '01';
  P_NIVELINTERMEDIO CONSTANT VARCHAR2(2) := '02';
  P_NIVELAVANZADO   CONSTANT VARCHAR2(2) := '03';

  FUNCTION FN_CANT_SOLICITUD_COLA(p_nIdSol     IN NUMBER,
                                  p_cTipSol    IN VARCHAR2,
                                  p_cTipPuesto IN VARCHAR2,
                                  p_nIdSede    IN NUMBER,
                                  p_nIdCargo   IN NUMBER) RETURN NUMBER;

  FUNCTION FN_OBTIENE_DATOS_POST(p_cIdsol     number,
                                 p_cTipSol    varchar2,
                                 p_cIdSede    number,
                                 p_cIdCargo   number,
                                 p_cTipPuesto varchar2,
                                 p_nPosicion  number,
                                 p_nCampo     number) RETURN VARCHAR2;

  FUNCTION FN_OBTIENE_VACANTES(p_IndEtapa   number,
                               p_nIdUsuario number,
                               p_cFecInicio varchar2,
                               p_cFecFin    varchar2,
                               p_cIdSede    Number) RETURN NUMBER;

  PROCEDURE SP_VALIDA_FIN_SOLICITUD(p_nIdSol     IN NUMBER,
                                    p_cTipSol    IN VARCHAR2,
                                    p_cTipPuesto IN VARCHAR2,
                                    p_nIdSede    IN NUMBER,
                                    p_nIdCargo   IN NUMBER,
                                    p_nVancantes IN NUMBER,
                                    p_cRpta      OUT varchar2);

  PROCEDURE SP_ELIMINA_REEMPLAZO(p_idReemplazo IN NUMBER,
                                 p_idSolReq    IN NUMBER,
                                 p_idPersona   IN NUMBER,
                                 p_cRetVal     OUT NUMBER);

  PROCEDURE SP_OBTIENE_USUARIOS(p_nIdRol      IN NUMBER,
                                p_nIdSede     IN NUMBER,
                                p_cNombres    IN VARCHAR2,
                                p_cCodUsuario IN VARCHAR2,
                                p_cDesApePat  IN VARCHAR2,
                                p_cDesApeMat  IN VARCHAR2,
                                p_cRpta       OUT cur_cursor);

  FUNCTION FN_OBTIENE_VACANTES_FIN(p_indCub     number,
                                   p_nIdUsuario number,
                                   p_cFecInicio varchar2,
                                   p_cFecFin    varchar2,
                                   p_cIdSede    Number) RETURN NUMBER;

  PROCEDURE SP_CV_POSTULANTE(p_nIdPostulante IN postulante.idepostulante%type,
                             p_Rpta          OUT cur_cursor);

  PROCEDURE SP_CV_EXPERIENCIA(p_nIdPostulante IN postulante.idepostulante%type,
                              p_Rpta          OUT cur_cursor);

  PROCEDURE SP_CV_NIVEL_ACADEMICO(p_nIdPostulante IN postulante.idepostulante%type,
                                  p_Rpta          OUT cur_cursor);
  PROCEDURE SP_CV_CONOFIMATICA(p_nIdPostulante IN postulante.idepostulante%type,
                               p_Rpta          OUT cur_cursor);

  PROCEDURE SP_CV_CONIDIOMA(p_nIdPostulante IN postulante.idepostulante%type,
                            p_Rpta          OUT cur_cursor);

  PROCEDURE SP_CV_OTROSCONOCIMIENTOS(p_nIdPostulante IN postulante.idepostulante%type,
                                     p_Rpta          OUT cur_cursor);

  PROCEDURE SP_CV_PARIENTES(p_nIdPostulante IN postulante.idepostulante%type,
                            p_Rpta          OUT cur_cursor);

  PROCEDURE SP_CV_DISCAPACIDAD(p_nIdPostulante IN postulante.idepostulante%type,
                               p_Rpta          OUT cur_cursor);

  FUNCTION FN_DES_MES(p_nMes NUMBER, p_nInFormato NUMBER) RETURN VARCHAR2;

  PROCEDURE SP_ELIMINA_SOLICITUD(p_nIdSol     IN NUMBER,
                                 p_cTipSol    IN VARCHAR2,
                                 p_nIdUsuario IN number,
                                 p_nRolUsario IN number,
                                 p_cRetVal    OUT NUMBER,
                                 P_cMensaje   OUT VARCHAR2
                                 
                                 );

  PROCEDURE SP_OBTIENE_POTENCIALES(p_nIdSol     IN NUMBER,
                                   p_cTipSol    IN VARCHAR2,
                                   p_cTipPuesto IN VARCHAR2,
                                   p_nIdSede    IN NUMBER,
                                   p_nIdCargo   IN NUMBER);

  PROCEDURE SP_INSERTA_REEMPLAZO(p_cApePaterno         IN VARCHAR2,
                                 p_cNomBres            IN VARCHAR2,
                                 p_cFecInicioReemplazo IN VARCHAR2,
                                 p_cFecFinReemplazo    IN VARCHAR2,
                                 p_cUsrCreacion        IN VARCHAR2,
                                 p_cFecCreacion        IN VARCHAR2,
                                 p_nIdeSolReqPersonal  IN NUMBER,
                                 p_cRetVal             OUT NUMBER);

  PROCEDURE SP_VALIDA_SELECCION(p_nIdPostulante IN NUMBER,
                                p_nIdSede       IN NUMBER,
                                p_cRpta         OUT VARCHAR2);

  FUNCTION FN_OBTIENE_POSTULACIONES(p_nIdPostulante IN NUMBER,
                                    p_nIdSede       IN NUMBER)
    RETURN VARCHAR2;

  FUNCTION FN_OBTIENE_DESUBIGEO(p_nIdUbigeo IN postulante.ideubigeo%type,
                                p_nInd      IN NUMBER) RETURN VARCHAR2;

  PROCEDURE SP_MIGRA_CONTRATADOS;

  PROCEDURE SP_INSERT_LOG_SOLNUEVO_CARGO(p_nIDESOL          IN LOGSOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                         p_cTIPETAPA        IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE,
                                         p_cFECSUCESO       IN VARCHAR2,
                                         p_nIdUsuarioSuceco IN LOGSOLNUEVO_CARGO.USRSUCESO%TYPE,
                                         p_nIdRolSuceso     IN LOGSOLNUEVO_CARGO.ROLSUCESO%TYPE,
                                         p_nIdRolResp       IN LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                                         p_nIdResponble     IN LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE,
                                         p_observacion      IN VARCHAR2);

  PROCEDURE SP_INSERTA_RECLUTA_PERSONA(p_nIdPostulante      IN RECLUTAMIENTO_PERSONA.IDEPOSTULANTE%TYPE,
                                       p_cIdSol             IN RECLUTAMIENTO_PERSONA.IDESOL%TYPE,
                                       p_cTipSol            IN RECLUTAMIENTO_PERSONA.TIPSOL%TYPE,
                                       p_nIdCargo           IN RECLUTAMIENTO_PERSONA.IDECARGO%TYPE,
                                       p_nIdCv              IN RECLUTAMIENTO_PERSONA.IDECV%TYPE,
                                       p_cEstadoPost        IN RECLUTAMIENTO_PERSONA.ESTPOSTULANTE%TYPE,
                                       p_cIndContacto       IN RECLUTAMIENTO_PERSONA.INDCONTACTADO%TYPE,
                                       p_nEvaluacion        IN RECLUTAMIENTO_PERSONA.EVALUACION%TYPE,
                                       p_nPuntajeTotal      IN RECLUTAMIENTO_PERSONA.PTOTOTAL%TYPE,
                                       p_cComentario        IN RECLUTAMIENTO_PERSONA.COMENTARIO%TYPE,
                                       p_cTipPuesto         IN RECLUTAMIENTO_PERSONA.TIPPUESTO%TYPE,
                                       p_nIdSede            IN RECLUTAMIENTO_PERSONA.IDSEDE%TYPE,
                                       p_cCodCargo          IN RECLUTAMIENTO_PERSONA.CODCARGO%TYPE,
                                       p_cPromedioEx        IN RECLUTAMIENTO_PERSONA.PROMEDIOEXAMEN%TYPE,
                                       p_cIndProceso        IN RECLUTAMIENTO_PERSONA.INDPROCESO%TYPE,
                                       p_nIdeReclutaPersona OUT NUMBER);

  PROCEDURE SP_INSERTA_RECLUTA_EXAMEM(p_nIDERECLUTAPERSONA     IN RECLU_PERSO_EXAMEN.IDERECLUTAPERSONA%TYPE,
                                      p_nIDEEVALUACION         IN RECLU_PERSO_EXAMEN.IDEEVALUACION%TYPE,
                                      p_cTIPSOLICITUD          IN RECLU_PERSO_EXAMEN.TIPSOLICITUD%TYPE,
                                      p_nIDUSUARESPONS         IN RECLU_PERSO_EXAMEN.IDUSUARESPONS%TYPE,
                                      p_dFECEVALUACION         IN RECLU_PERSO_EXAMEN.FECEVALUACION%TYPE,
                                      p_dHORAEVALUACION        IN RECLU_PERSO_EXAMEN.HORAEVALUACION%TYPE,
                                      p_nNOTAFINAL             IN RECLU_PERSO_EXAMEN.NOTAFINAL%TYPE,
                                      p_cARCHIVO               IN RECLU_PERSO_EXAMEN.ARCHIVO%TYPE,
                                      p_cCOMENTARIORESUL       IN RECLU_PERSO_EXAMEN.COMENTARIORESUL%TYPE,
                                      p_cTIPESTEVALUACION      IN varchar2,
                                      p_cOBSERVACION           IN varchar2,
                                      p_indEntrevista          IN varchar2,
                                      p_nIdeReclutaPersoExamen OUT number);

  PROCEDURE SP_GET_REEMPLAZO(p_nIdeSolReqPersonal IN NUMBER,
                             p_cRetVal            OUT cur_cursor);

  PROCEDURE SP_ENVIA_SOL_REEMPLAZO(p_nIdeSolReqPersonal IN NUMBER,
                                   p_nidUsuarioSuceso   IN NUMBER,
                                   p_cDesUsuarioSuceso  IN VARCHAR2,
                                   p_cFechaSuceso       IN VARCHAR2,
                                   p_cIdRolSuceso       IN NUMBER,
                                   p_cEtapa             IN VARCHAR2,
                                   p_idUsuarioResp      IN NUMBER,
                                   p_idRolResp          IN NUMBER,
                                   p_nIdeCargo          IN NUMBER,
                                   p_cRetVal            OUT NUMBER);

  PROCEDURE SP_ASIGNACION_REMPLAZO(p_cTipoDerivacion IN VARCHAR2,
                                   p_nIdSede         IN NUMBER,
                                   p_nTipoReq        IN VARCHAR2,
                                   p_nIdUsuarioResp  OUT NUMBER,
                                   p_nIdRol          OUT NUMBER);

  PROCEDURE SP_OPORTUNIDAD_LABORAL(p_cTipPuesto   IN VARCHAR2,
                                   p_nIdCargo     IN NUMBER,
                                   p_nIdSede      IN NUMBER,
                                   p_cFechaInicio IN VARCHAR2,
                                   p_cFecFin      IN VARCHAR2,
                                   p_cRetVal      OUT cur_cursor);

  PROCEDURE SP_VALIDA_POSTULACION(p_nIPostulante IN NUMBER,
                                  p_ctippuesto   IN VARCHAR2,
                                  p_nidcargo     IN NUMBER,
                                  p_nidsede      IN NUMBER,
                                  p_retorno      OUT NUMBER,
                                  p_Mensaje      OUT VARCHAR2);

  PROCEDURE SP_POSTULACION(p_nIdPostulante IN NUMBER,
                           p_cTipPuesto    IN VARCHAR2,
                           p_nIdCargo      IN NUMBER,
                           p_nIdSede       IN NUMBER);

  PROCEDURE SP_OBTIENE_CARGOS_PUBLICADOS(p_nIdSede IN NUMBER,
                                         p_cRpta   OUT cur_cursor);

  FUNCTION FN_OBTIENE_TIEMPO_TOTAL(p_nIdExamen EXAMEN.IDEEXAMEN%TYPE)
    RETURN NUMBER;

  PROCEDURE SP_OBTIENE_ANALISTA_RESP(p_nIdSede IN SEDE.IDESEDE%TYPE,
                                     p_cRpta   OUT cur_cursor);

  PROCEDURE SP_INSERT_LOG_SOLREQPERSONAL(p_nIDESOLREQPERSONAL IN LOGSOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                         p_cTIPETAPA          IN LOGSOLREQ_PERSONAL.TIPETAPA%TYPE,
                                         p_cFECSUCESO         IN VARCHAR2,
                                         p_nIdUsuarioSuceco   IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                         p_nIdRolSuceso       IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                         p_nIdRolResp         IN LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,
                                         p_nIdResponble       IN LOGSOLREQ_PERSONAL.USRESPONSABLE%TYPE,
                                         p_observacion        IN VARCHAR2);

  PROCEDURE SP_MIS_POSTULACIONES(p_nIdPostulante IN NUMBER,
                                 p_cRetVal       OUT cur_cursor);

  PROCEDURE SP_OBTIENE_DATOS_SOL(p_nIdSol  IN NUMBER,
                                 p_cTipSol IN VARCHAR2,
                                 p_cRetVal OUT cur_cursor);

  PROCEDURE SP_OBTIENE_POSTULANTES_RANKING(p_nIdSol      IN NUMBER,
                                           p_cTipSol     IN VARCHAR2,
                                           p_cApePaterno IN VARCHAR2,
                                           p_cApeMaterno IN VARCHAR2,
                                           p_nombre      IN VARCHAR2,
                                           p_cEstado     IN VARCHAR2,
                                           p_cRetVal     OUT cur_cursor);

  PROCEDURE SP_CAMBIA_ESTADO_POST(p_nIdRegistro    IN NUMBER,
                                  p_cCodEstadoPost IN VARCHAR2);

  PROCEDURE SP_POSTULANTE_PRESELEC(p_nIdSol  IN NUMBER,
                                   p_cTipSol IN VARCHAR2,
                                   p_cRetVal OUT cur_cursor);

  FUNCTION FN_OBTIENE_PROMEDIO(p_nIdReclutaPersona IN NUMBER,
                               p_nPuntaje          IN NUMBER,
                               p_nPomedioAnt       IN NUMBER,
                               p_cEstadoPost       IN Reclutamiento_Persona.Estpostulante%type,
                               p_cIndProceso       IN Reclutamiento_Persona.Indproceso%type)
    RETURN NUMBER;

  FUNCTION FN_OBTIENE_IND_APROB(p_nIdReclutaPersona IN NUMBER,
                                p_nPuntaje          IN NUMBER,
                                p_nPromedio         IN NUMBER,
                                p_cEstadoPost       IN RECLUTAMIENTO_PERSONA.ESTPOSTULANTE%TYPE,
                                p_cIndProceso       IN Reclutamiento_Persona.Indproceso%type)
    RETURN VARCHAR2;

  PROCEDURE SP_POSTULANTE_SELECCIONADOS(p_nIdSol  IN NUMBER,
                                        p_cTipSol IN VARCHAR2,
                                        p_cRetVal OUT cur_cursor);

  PROCEDURE SP_FINALIZA_CONTRATACION(p_nIdSol       IN NUMBER,
                                     p_cTipSol      IN VARCHAR2,
                                     p_cTipPuesto   IN VARCHAR2,
                                     p_nIdSede      IN NUMBER,
                                     p_nIdCargo     IN NUMBER,
                                     p_nIdResp      IN NUMBER,
                                     p_nIdSuceso    IN NUMBER,
                                     p_nIdRolResp   IN NUMBER,
                                     p_nIdRolSuceso IN NUMBER);

  PROCEDURE SP_OBTIENE_MAX_SOL_GRUPO_CARGO(p_cTipPuesto   IN VARCHAR2,
                                           p_nIdCargo     IN NUMBER,
                                           p_nIdSede      IN NUMBER,
                                           p_nIdSol       OUT NUMBER,
                                           p_cSedeDes     OUT VARCHAR2,
                                           p_cDesRangoSal OUT VARCHAR2,
                                           p_cCodRangoSal OUT VARCHAR2,
                                           p_nIdArea      OUT NUMBER,
                                           p_cDesArea     OUT VARCHAR2,
                                           p_cTipSol      OUT VARCHAR2,
                                           p_cNombreCargo OUT VARCHAR2,
                                           p_cFunciones   OUT VARCHAR2,
                                           p_cObcervacion OUT VARCHAR2);

  PROCEDURE SP_OBTIENE_REPORTE_SELECCION(p_cFecDesde       IN varchar2,
                                         p_cFecHasta       IN varchar2,
                                         p_cTipSol         IN varchar2,
                                         p_cEstadpReq      IN varchar2,
                                         p_nIdResponsable  IN NUMBER,
                                         p_nIdDependencia  IN NUMBER,
                                         p_nIdDepartamento IN NUMBER,
                                         p_nIdArea         IN NUMBER,
                                         p_cMotivoReemp    IN NUMBER,
                                         p_cSede           IN NUMBER,
                                         p_cRpta           OUT cur_cursor);

  FUNCTION FN_OBTIENE_OBSERVACION(p_nIndObs       IN NUMBER,
                                  p_nIdReclutaPer IN NUMBER) RETURN VARCHAR2;

  procedure reiniciar_secuencia(p_nombre in varchar2);

  PROCEDURE SP_CREA_SOLREEMPLAZO(p_nIdSede           IN NUMBER,
                                 p_nIdeDependencia   IN NUMBER,
                                 p_nIdeCargo         IN NUMBER,
                                 p_nIdeDepartamento  IN NUMBER,
                                 p_nIdeArea          IN NUMBER,
                                 p_cTipVacante       IN VARCHAR2,
                                 p_nNumVacantes      IN NUMBER,
                                 p_cTipPuesto        IN VARCHAR2,
                                 p_cObservacion      IN VARCHAR2,
                                 p_idUsuarioSuceso   IN NUMBER,
                                 p_cDesUsuarioSuceso IN VARCHAR2,
                                 p_cFechaSuceso      IN VARCHAR2,
                                 p_cIdRolSuceso      IN NUMBER,
                                 p_cCodReemplazo     IN VARCHAR2,
                                 p_cEtapa            IN VARCHAR2,
                                 p_idUsuarioResp     IN NUMBER,
                                 p_idRolResp         IN NUMBER,
                                 p_cTipSol           IN VARCHAR2,
                                 p_cRetVal           OUT NUMBER);

  PROCEDURE SP_REPORTE_RESUMEN_RQ(p_cFecInicio   IN VARCHAR2,
                                  p_cFecFin      IN VARCHAR2,
                                  p_nIdEncargado IN NUMBER,
                                  p_nIdSede      IN NUMBER,
                                  p_curRpta      OUT cur_cursor);

  FUNCTION FN_SALDO_FECHA(p_nIdUsuario IN NUMBER,
                          p_cFecInicio IN VARCHAR2,
                          p_nIdSede    IN NUMBER) RETURN VARCHAR2;

  PROCEDURE FN_OBTIENE_CRITERIOS(p_cTipMedicion IN Criterio.Tipmedicion%type,
                                 p_cPregunta    IN Criterio.Pregunta%type,
                                 p_cTipCriterio IN Criterio.Tipcriterio%type,
                                 p_cEstado      IN Criterio.Estactivo%type,
                                 p_cTipoModo    IN Criterio.Tipmodo%TYPE,
                                 p_nIdSede      IN Criterio.Idesede%type,
                                 p_cRpta        OUT cur_cursor);

  PROCEDURE SP_OBTIENE_CATEGORIAS(p_cTipCategoria IN VARCHAR2,
                                  p_cDescrip      IN VARCHAR2,
                                  p_nIdSede       IN CATEGORIA.IDESEDE%TYPE,
                                  p_cNombreCat    IN CATEGORIA.NOMCATEGORIA%TYPE,
                                  p_cRpta         OUT cur_cursor) ;

  PROCEDURE FN_OBTIENE_OPCIONES(p_cDesOpcion   IN VARCHAR2,
                                p_cDescripcion IN VARCHAR2,
                                p_cEstado      IN VARCHAR2,
                                p_cTipMenu     IN VARCHAR2,
                                p_cRpta        OUT cur_cursor);

  PROCEDURE SP_GET_CARGOXSEDE(p_nIdSede         IN NUMBER,
                              p_nIdDependencia  IN NUMBER,
                              p_nIdDepartamento IN NUMBER,
                              p_nIdArea         IN NUMBER,
                              p_cRetVal         OUT CUR_CURSOR);

  PROCEDURE SP_OBTIENE_ROLXEMAIL(p_cIdSol       IN NUMBER,
                                 p_cIdRolSuceso IN VARCHAR2,
                                 p_cTipSol      IN VARCHAR2,
                                 p_cAccion      IN VARCHAR2,
                                 p_cIdSede      IN NUMBER,
                                 p_cRetVal      OUT CUR_CURSOR);

  procedure SP_OBTIENE_EMAIL(p_nIdRol  IN number,
                             p_nIdSede IN number,
                             p_cRetVal OUT CUR_CURSOR);

end PR_INTRANET_ED;
/
create or replace package body PR_INTRANET_ED is

  /* ------------------------------------------------------------
    Nombre      : SP_ELIMINA_REEMPLAZO
    Proposito   : Obtiene la lista de cargos o el cargo por el id de cargo
  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :
  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_ELIMINA_REEMPLAZO(p_idReemplazo IN NUMBER,
                                 p_idSolReq    IN NUMBER,
                                 p_idPersona   IN NUMBER,
                                 p_cRetVal     OUT NUMBER) IS
  BEGIN
  
    delete from REEMPLAZOS r
     where r.idreemplazo = p_idReemplazo
       and r.idesolreqpersonal = p_idSolReq
       and r.IDPERSONA = p_idPersona;
  
    COMMIT;
  
    p_cRetVal := 1;
  
  END SP_ELIMINA_REEMPLAZO;

  /* ------------------------------------------------------------
    Nombre      : SP_INSERTA_REEMPLAZO
    Proposito   : Obtiene la lista de cargos o el cargo por el id de cargo
  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :
  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_INSERTA_REEMPLAZO(p_cApePaterno         IN VARCHAR2,
                                 p_cNomBres            IN VARCHAR2,
                                 p_cFecInicioReemplazo IN VARCHAR2,
                                 p_cFecFinReemplazo    IN VARCHAR2,
                                 p_cUsrCreacion        IN VARCHAR2,
                                 p_cFecCreacion        IN VARCHAR2,
                                 p_nIdeSolReqPersonal  IN NUMBER,
                                 p_cRetVal             OUT NUMBER) IS
    nIdpersona   number(8);
    nIdReemplazo number(8);
  
  BEGIN
    SELECT IDREEMPLAZO_SQ.nextval into nIdReemplazo FROM dual;
  
    select nvl(max(r.IDPERSONA), 0) + 1
      into nIdpersona
      from REEMPLAZOS r
     where r.idesolreqpersonal = p_nIdeSolReqPersonal;
  
    INSERT INTO REEMPLAZOS
      (IDESOLREQPERSONAL,
       IDREEMPLAZO,
       IDPERSONA,
       APEPATERNO,
       NOMBRES,
       FECINICIOREEMPLAZO,
       FECFINREEMPLAZO,
       USRCREACION,
       FECCREACION)
    VALUES
      (p_nIdeSolReqPersonal,
       nIdReemplazo,
       nIdpersona,
       p_cApePaterno,
       p_cNomBres,
       TO_DATE(p_cFecInicioReemplazo, 'DD/MM/YYYY'),
       TO_DATE(p_cFecFinReemplazo, 'DD/MM/YYYY'),
       p_cUsrCreacion,
       TO_DATE(p_cFecCreacion, 'DD/MM/YYYY'));
  
    COMMIT;
  
    p_cRetVal := 1;
  
  END SP_INSERTA_REEMPLAZO;

  /* ------------------------------------------------------------
  Nombre      : SP_GET_REEMPLAZO
  Proposito   : Obtiene la lista de cargos o el cargo por el id de cargo
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_GET_REEMPLAZO(p_nIdeSolReqPersonal IN NUMBER,
                             p_cRetVal            OUT cur_cursor) IS
  BEGIN
  
    OPEN p_cRetVal FOR
      SELECT IDREEMPLAZO,
             APEPATERNO,
             NOMBRES,
             FECINICIOREEMPLAZO,
             FECFINREEMPLAZO,
             USRCREACION,
             FECCREACION,
             IDESOLREQPERSONAL,
             IDPERSONA
        FROM REEMPLAZOS R
       WHERE R.IDESOLREQPERSONAL = p_nIdeSolReqPersonal
       ORDER BY IDREEMPLAZO;
  
  END SP_GET_REEMPLAZO;

  /* ------------------------------------------------------------
  Nombre      : SP_CREA_SOLREEMPLAZO
  Proposito   : crea Solicitud de reemplazo de cargo
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_CREA_SOLREEMPLAZO(p_nIdSede           IN NUMBER,
                                 p_nIdeDependencia   IN NUMBER,
                                 p_nIdeCargo         IN NUMBER,
                                 p_nIdeDepartamento  IN NUMBER,
                                 p_nIdeArea          IN NUMBER,
                                 p_cTipVacante       IN VARCHAR2,
                                 p_nNumVacantes      IN NUMBER,
                                 p_cTipPuesto        IN VARCHAR2,
                                 p_cObservacion      IN VARCHAR2,
                                 p_idUsuarioSuceso   IN NUMBER,
                                 p_cDesUsuarioSuceso IN VARCHAR2,
                                 p_cFechaSuceso      IN VARCHAR2,
                                 p_cIdRolSuceso      IN NUMBER,
                                 p_cCodReemplazo     IN VARCHAR2,
                                 p_cEtapa            IN VARCHAR2,
                                 p_idUsuarioResp     IN NUMBER,
                                 p_idRolResp         IN NUMBER,
                                 p_cTipSol           IN VARCHAR2,
                                 p_cRetVal           OUT NUMBER) IS
  
    nIDESOLREQPERSONAL     NUMBER(8);
    nCODSOLREQPERSONAL     NUMBER(8);
    nIDELOGSOLREQ_PERSONAL NUMBER(8);
  
    CURSOR c_Reemplazo IS
      SELECT IDREEMPLAZO,
             APEPATERNO,
             NOMBRES,
             FECINICIOREEMPLAZO,
             FECFINREEMPLAZO,
             USRCREACION,
             FECCREACION,
             IDESOLREQPERSONAL
        FROM TEMP_REEMPLAZOS T
       where t.idtemp = p_cCodReemplazo;
  
  BEGIN
  
    SELECT SOLREQ_PERSONAL_SQ.nextval into nIDESOLREQPERSONAL FROM dual;
  
    SELECT CODSOLREQPERSONAL_SQ.nextval into nCODSOLREQPERSONAL FROM dual;
  
    BEGIN
      INSERT INTO SOLREQ_PERSONAL
        (IDESOLREQPERSONAL,
         CODSOLREQPERSONAL,
         IDESEDE,
         IDEDEPENDENCIA,
         IDEDEPARTAMENTO,
         IDEAREA,
         IDECARGO,
         TIPVACANTE,
         NUMVACANTES,
         TIPPUESTO,
         OBSERVACION,
         TIPSOL,
         USRCREACION,
         FECCREACION)
      VALUES
        (nIDESOLREQPERSONAL,
         nCODSOLREQPERSONAL,
         p_nIdSede,
         p_nIdeDependencia,
         p_nIdeDepartamento,
         p_nIdeArea,
         p_nIdeCargo,
         p_cTipVacante,
         p_nNumVacantes,
         p_cTipPuesto,
         p_cObservacion,
         p_cTipSol,
         p_cDesUsuarioSuceso,
         sysdate);
      --TO_DATE(p_cFechaSuceso, 'DD/MM/YYYY'));
      COMMIT;
    
    EXCEPTION
      WHEN OTHERS THEN
        ROLLBACK;
        p_cRetVal := 0;
    END;
  
    p_cRetVal := nCODSOLREQPERSONAL;
  
  END SP_CREA_SOLREEMPLAZO;

  /* ------------------------------------------------------------
  Nombre      : SP_INSERT_LOG_SOLREQPERSONAL
  Proposito   : inserta en el log de solicitud de requerimiento de personal
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_INSERT_LOG_SOLREQPERSONAL(p_nIDESOLREQPERSONAL IN LOGSOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                         p_cTIPETAPA          IN LOGSOLREQ_PERSONAL.TIPETAPA%TYPE,
                                         p_cFECSUCESO         IN VARCHAR2,
                                         p_nIdUsuarioSuceco   IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                         p_nIdRolSuceso       IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                         p_nIdRolResp         IN LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,
                                         p_nIdResponble       IN LOGSOLREQ_PERSONAL.USRESPONSABLE%TYPE,
                                         p_observacion        IN VARCHAR2) IS
  
    nIDELOGSOLREQ_PERSONAL NUMBER(8);
  BEGIN
  
    BEGIN
      SELECT IDELOGSOLREQ_PERSONAL_SQ.nextval
        into nIDELOGSOLREQ_PERSONAL
        FROM dual;
    
      INSERT INTO LOGSOLREQ_PERSONAL
        (IDELOGSOLREQ_PERSONAL,
         IDESOLREQPERSONAL,
         TIPETAPA,
         ROLRESPONSABLE,
         USRESPONSABLE,
         OBSERVACION,
         FECSUCESO,
         USRSUCESO,
         ROLSUCESO)
      VALUES
        (nIDELOGSOLREQ_PERSONAL,
         p_nIDESOLREQPERSONAL,
         p_cTIPETAPA,
         p_nIdRolResp,
         p_nIdResponble,
         p_observacion,
         SYSDATE,
         p_nIdUsuarioSuceco,
         p_nIdRolSuceso);
    
      COMMIT;
    EXCEPTION
      WHEN OTHERS THEN
        ROLLBACK;
    END;
  
  END SP_INSERT_LOG_SOLREQPERSONAL;

  /* ------------------------------------------------------------
  Nombre      : SP_ENVIA_SOL_REEMPLAZO
  Proposito   : Envia la solicitud para su aprobacion
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_ENVIA_SOL_REEMPLAZO(p_nIdeSolReqPersonal IN NUMBER,
                                   p_nidUsuarioSuceso   IN NUMBER,
                                   p_cDesUsuarioSuceso  IN VARCHAR2,
                                   p_cFechaSuceso       IN VARCHAR2,
                                   p_cIdRolSuceso       IN NUMBER,
                                   p_cEtapa             IN VARCHAR2,
                                   p_idUsuarioResp      IN NUMBER,
                                   p_idRolResp          IN NUMBER,
                                   p_nIdeCargo          IN NUMBER,
                                   p_cRetVal            OUT NUMBER) IS
  
  BEGIN
  
    BEGIN
    
      UPDATE SOLREQ_PERSONAL S
         SET Tipetapa = p_cEtapa, ESTACTIVO = 'A'
       WHERE IDESOLREQPERSONAL = p_nIdeSolReqPersonal;
      COMMIT;
    
    EXCEPTION
      WHEN OTHERS THEN
        ROLLBACK;
        p_cRetVal := 0;
    END;
  
    PR_REQUERIMIENTOS.COPIA_CARGO(p_nIdeCargo,
                                  p_nIdeSolReqPersonal,
                                  p_cDesUsuarioSuceso);
  
    sp_insert_log_solreqpersonal(p_nIdeSolReqPersonal,
                                 p_cEtapa,
                                 p_cFechaSuceso,
                                 p_nidUsuarioSuceso,
                                 p_cIdRolSuceso,
                                 p_idRolResp,
                                 p_idUsuarioResp,
                                 NULL);
  
    p_cRetVal := p_nIdeSolReqPersonal;
  
  END SP_ENVIA_SOL_REEMPLAZO;

  /* ------------------------------------------------------------
  Nombre      : SP_ASIGNACION_REMPLAZO
  Proposito   : obtiene los encargados o analista de seleccion por tipo de requerimiento
  
                P : PENDIENTE
                A : APROBADO
                U : PUBLICADO
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_ASIGNACION_REMPLAZO(p_cTipoDerivacion IN VARCHAR2,
                                   p_nIdSede         IN NUMBER,
                                   p_nTipoReq        IN VARCHAR2,
                                   p_nIdUsuarioResp  OUT NUMBER,
                                   p_nIdRol          OUT NUMBER) IS
    nIdUsuario Number(8);
    nIdRol     Number(8);
  
  BEGIN
  
    IF p_cTipoDerivacion = 'U' THEN
    
      BEGIN
      
        SELECT Z.IDUSUARIO, Z.IDROL
          INTO nIdUsuario, nIdRol
          FROM USUAROLSEDE Z, USUARIOREQ X
         WHERE Z.IDUSUARIO = X.IDUSUARIO
           AND Z.IDESEDE = p_nIdSede
           AND X.TIPREQ = p_nTipoReq
           AND Z.IDROL IN (8, 9)
           AND ROWNUM < 2
         ORDER BY Z.IDUSUARIO, Z.IDROL;
      
      EXCEPTION
        WHEN OTHERS THEN
          nIdUsuario := 0;
          nIdRol     := 0;
        
      END;
    
      p_nIdUsuarioResp := nIdUsuario;
      p_nIdRol         := nIdRol;
    
    END IF;
  
  END SP_ASIGNACION_REMPLAZO;

  /* ------------------------------------------------------------
  Nombre      : SP_OPORTUNIDAD_LABORAL
  Proposito   : obtiene las oportunidades laborales
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OPORTUNIDAD_LABORAL(p_cTipPuesto   IN VARCHAR2,
                                   p_nIdCargo     IN NUMBER,
                                   p_nIdSede      IN NUMBER,
                                   p_cFechaInicio IN VARCHAR2,
                                   p_cFecFin      IN VARCHAR2,
                                   p_cRetVal      OUT cur_cursor) IS
    nIdCargo NUMBER(8);
    nIdSede  NUMBER(8);
  
    mensaje VARCHAR2(1500);
  BEGIN
  
    IF p_nIdCargo IS NULL THEN
      nIdCargo := 0;
    else
      nIdCargo := p_nIdCargo;
    END IF;
  
    IF p_nIdSede IS NULL THEN
      nIdSede := 0;
    else
      nIdSede := p_nIdSede;
    END IF;
  
    BEGIN
    
      OPEN p_cRetVal FOR
      
        select Z.TIPPUESTO,
               Z.DESPUESTO,
               Z.IDECARGO,
               Z.IDESEDE,
               Z.DESSEDE,
               Z.NOMCARGO,
               Z.NUMVACANTES,
               Z.FECINICIALMAX,
               Z.FECFINALMAX
          FROM (SELECT Y.TIPPUESTO,
                       Y.DESPUESTO,
                       Y.IDECARGO,
                       Y.IDESEDE,
                       Y.DESSEDE,
                       Y.NOMCARGO,
                       Y.NUMVACANTES,
                       Y.FECINICIALMAX,
                       Y.FECFINALMAX
                  FROM (SELECT X.TIPPUESTO,
                               (SELECT D.DESCRIPCION
                                  FROM DETALLE_GENERAL D
                                 WHERE D.IDEGENERAL = 14
                                   AND D.VALOR = X.TIPPUESTO) DESPUESTO,
                               X.IDECARGO,
                               X.IDESEDE,
                               (SELECT E.DESCRIPCION
                                  FROM SEDE E
                                 WHERE E.IDESEDE = X.IDESEDE
                                   AND E.ESTREGISTRO = 'A') DESSEDE,
                               X.NOMCARGO,
                               SUM(X.NUMVACANTES) NUMVACANTES,
                               MIN(X.FECPUBLICACION) FECINICIALMAX,
                               MAX(X.FECEXPIRACACION) FECFINALMAX
                          FROM (SELECT S.TIPPUESTO,
                                       nvl(S.IDECARGO, 0) IDECARGO,
                                       S.IDESEDE,
                                       S.NOMCARGO NOMCARGO,
                                       S.NUMVACANTES,
                                       S.FECPUBLICACION,
                                       S.FECEXPIRACACION
                                  FROM SOLREQ_PERSONAL S
                                 WHERE 1 = 1
                                   AND S.TIPSOL IN ('03', '02')
                                   AND S.FECPUBLICACION IS NOT NULL
                                   AND S.TIPETAPA = '04'
                                   AND S.NOMCARGO IS NOT NULL
                                   AND S.TIPPUESTO IS NOT NULL
                                   AND S.ESTACTIVO = 'A'
                                   AND s.FECPUBLICACION IS NOT NULL
                                   and s.fecexpiracacion is not null
                                   AND (nvl(p_cTipPuesto, null) IS NULL OR
                                       S.TIPPUESTO = p_cTipPuesto)
                                   AND (nIdCargo = 0 OR S.IDECARGO = nIdCargo)
                                   AND (nIdSede = 0 OR S.IDESEDE = nIdSede)
                                
                                UNION ALL
                                
                                SELECT F.TIPPUESTO,
                                       F.IDECARGO,
                                       F.IDESEDE,
                                       F.NOMCARGO,
                                       F.NUMVACANTES,
                                       F.FECPUBLICACION,
                                       F.FECEXPIRACACION
                                  FROM (SELECT (SELECT H.TIPHORARIO
                                                  FROM HORARIO_CARGO H
                                                 WHERE H.IDECARGO = N.IDECARGO
                                                   AND H.PUNTHORARIO =
                                                       (SELECT MAX(G.PUNTHORARIO)
                                                          FROM HORARIO_CARGO G
                                                         WHERE G.IDECARGO =
                                                               H.IDECARGO)) TIPPUESTO,
                                               nvl(N.IDECARGO, 0) IDECARGO,
                                               N.IDESEDE,
                                               N.NOMBRE NOMCARGO,
                                               N.NUMPOSICIONES NUMVACANTES,
                                               N.FECPUBLICACION FECPUBLICACION,
                                               N.FECEXPIRACION FECEXPIRACACION
                                          FROM SOLNUEVO_CARGO N
                                         WHERE 1 = 1
                                           AND N.FECPUBLICACION IS NOT NULL
                                           AND N.FECEXPIRACION IS NOT NULL
                                           AND N.NOMBRE IS NOT NULL
                                           AND N.ESTACTIVO = 'A'
                                           AND N.TIPETAPA = '04') F
                                 WHERE (nvl(p_cTipPuesto, null) IS NULL OR
                                       F.TIPPUESTO = p_cTipPuesto)
                                   AND (nIdCargo = 0 OR F.IDECARGO = nIdCargo)
                                   AND (nIdSede = 0 OR F.IDESEDE = nIdSede)
                                
                                ) X
                         GROUP BY X.TIPPUESTO,
                                  X.IDECARGO,
                                  X.IDESEDE,
                                  X.NOMCARGO) Y
                 WHERE (nvl(p_cFechaInicio, null) IS NULL OR
                       (Y.FECINICIALMAX >=
                       TO_DATE(p_cFechaInicio, 'DD/MM/YYYY')))
                   AND (nvl(p_cFecFin, null) IS NULL OR
                       (Y.FECINICIALMAX <
                       TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1))
                --ORDER BY Y.FECINICIALMAX DESC,Y.FECINICIALMAX DESC
                
                ) Z
         where to_char(z.FECFINALMAX,'DD/MM/YYYY') >= TO_CHAR(sysdate,'DD/MM/YYYY')
         ORDER BY FECINICIALMAX DESC, FECFINALMAX ASC;
    
    END;
  
  END SP_OPORTUNIDAD_LABORAL;

  /* ------------------------------------------------------------
  Nombre      : SP_OPORTUNIDAD_LABORAL
  Proposito   : obtiene las oportunidades laborales
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_MAX_SOL_GRUPO_CARGO(p_cTipPuesto   IN VARCHAR2,
                                           p_nIdCargo     IN NUMBER,
                                           p_nIdSede      IN NUMBER,
                                           p_nIdSol       OUT NUMBER,
                                           p_cSedeDes     OUT VARCHAR2,
                                           p_cDesRangoSal OUT VARCHAR2,
                                           p_cCodRangoSal OUT VARCHAR2,
                                           p_nIdArea      OUT NUMBER,
                                           p_cDesArea     OUT VARCHAR2,
                                           p_cTipSol      OUT VARCHAR2,
                                           p_cNombreCargo OUT VARCHAR2,
                                           p_cFunciones   OUT VARCHAR2,
                                           p_cObcervacion OUT VARCHAR2) IS
  
  BEGIN
  
    BEGIN
    
      SELECT NVL(N.IDESOLNUEVOCARGO, 0),
             (SELECT C.TIPRANGOSALARIO
                FROM CARGO C
               WHERE C.IDECARGO = p_nIdCargo) TIPRANGOSALARIO,
             N.IDEAREA,
             'N' TIPSOL,
             N.NOMBRE,
             N.Funciones,
             N.Obspublicacion
        INTO p_nIdSol,
             p_cCodRangoSal,
             p_nIdArea,
             p_cTipSol,
             p_cNombreCargo,
             p_cFunciones,
             p_cObcervacion
        FROM SOLNUEVO_CARGO N
       WHERE N.FECPUBLICACION IS NOT NULL
         AND N.FECEXPIRACION IS NOT NULL
         AND N.NOMBRE IS NOT NULL
         AND N.ESTACTIVO = 'A'
         AND N.TIPETAPA = '04'
         AND N.IDECARGO = p_nIdCargo
         AND N.IDESEDE = p_nIdSede
         AND p_cTipPuesto =
             (SELECT H.TIPHORARIO
                FROM HORARIO_CARGO H
               WHERE H.IDECARGO = p_nIdCargo
                 AND H.PUNTHORARIO =
                     (SELECT MAX(G.PUNTHORARIO)
                        FROM HORARIO_CARGO G
                       WHERE G.IDECARGO = p_nIdCargo))
            
         AND N.FECPUBLICACION =
             (SELECT MAX(Y.FECPUBLICACION)
                FROM SOLNUEVO_CARGO Y
               WHERE Y.FECPUBLICACION IS NOT NULL
                 AND Y.FECEXPIRACION IS NOT NULL
                 AND Y.NOMBRE IS NOT NULL
                 AND Y.ESTACTIVO = 'A'
                 AND Y.IDECARGO = p_nIdCargo
                 AND Y.IDESEDE = p_nIdSede
                 AND Y.TIPETAPA = '04'
                 AND p_cTipPuesto =
                     (SELECT H.TIPHORARIO
                        FROM HORARIO_CARGO H
                       WHERE H.IDECARGO = p_nIdCargo
                         AND H.PUNTHORARIO =
                             (SELECT MAX(G.PUNTHORARIO)
                                FROM HORARIO_CARGO G
                               WHERE G.IDECARGO = p_nIdCargo))
              
              )
         AND ROWNUM < 2;
    EXCEPTION
      WHEN OTHERS THEN
        p_nIdSol       := 0;
        p_cCodRangoSal := NULL;
        p_nIdArea      := 0;
        p_cTipSol      := null;
        p_cNombreCargo := null;
        p_cFunciones   := null;
    END;
  
    IF p_nIdSol = 0 THEN
      BEGIN
        SELECT S.IDESOLREQPERSONAL,
               S.TIPRANGOSALARIO,
               S.IDEAREA,
               S.TIPSOL,
               s.nomcargo,
               s.Funcionescargo,
               S.Observacionpublica
          INTO p_nIdSol,
               p_cCodRangoSal,
               p_nIdArea,
               p_cTipSol,
               p_cNombreCargo,
               p_cFunciones,
               p_cObcervacion
          FROM SOLREQ_PERSONAL S
         WHERE S.TIPSOL IN ('02', '03')
           AND S.FECPUBLICACION IS NOT NULL
           AND S.ESTACTIVO = 'A'
           AND S.TIPETAPA = '04'
           AND S.IDECARGO = p_nIdCargo
           AND S.TIPPUESTO = p_cTipPuesto
           AND S.IDESEDE = p_nIdSede
           AND S.FECPUBLICACION =
               (SELECT MAX(S.FECPUBLICACION) FECMAX
                  FROM SOLREQ_PERSONAL S
                 WHERE S.TIPSOL IN ('02', '03')
                   AND S.FECPUBLICACION IS NOT NULL
                   AND S.ESTACTIVO = 'A'
                   AND S.TIPETAPA = '04'
                   AND S.IDECARGO = p_nIdCargo
                   AND S.TIPPUESTO = p_cTipPuesto
                   AND S.IDESEDE = p_nIdSede)
           AND ROWNUM < 2;
      EXCEPTION
        WHEN OTHERS THEN
          p_nIdSol       := 0;
          p_cCodRangoSal := NULL;
          p_nIdArea      := 0;
          p_cTipSol      := null;
          p_cNombreCargo := null;
          p_cFunciones   := null;
      END;
    END IF;
  
    IF p_cCodRangoSal IS NOT NULL THEN
      BEGIN
        SELECT D.DESCRIPCION
          INTO p_cDesRangoSal
          FROM DETALLE_GENERAL D
         WHERE D.IDEGENERAL = 11
           AND D.VALOR = p_cCodRangoSal;
      EXCEPTION
        WHEN OTHERS THEN
          p_cDesRangoSal := '';
      END;
    END IF;
  
    IF p_nIdArea > 0 THEN
      BEGIN
        SELECT A.NOMAREA
          INTO p_cDesArea
          FROM AREA A
         WHERE A.IDEAREA = p_nIdArea
           AND ROWNUM < 2;
      EXCEPTION
        WHEN OTHERS THEN
          p_cDesArea := '';
      END;
    END IF;
  
    IF p_nIdSede > 0 THEN
      BEGIN
        SELECT trim(S.DESCRIPCION)
          INTO p_cSedeDes
          FROM SEDE S
         WHERE S.IDESEDE = p_nIdSede
           AND S.ESTREGISTRO = 'A';
      EXCEPTION
        WHEN OTHERS THEN
          p_cSedeDes := '';
      END;
    END IF;
  
  END SP_OBTIENE_MAX_SOL_GRUPO_CARGO;

  /* ------------------------------------------------------------
  Nombre      : SP_VALIDA_POSTULACION
  Proposito   : obtiene codigo de validacion del postulante
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  : p_nIPostulante id del postulante
                p_retorno retorno del postulante:
                1 faltan estudios
                2 falta experiencias
                3 falta conocimientos
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_VALIDA_POSTULACION(p_nIPostulante IN NUMBER,
                                  p_ctippuesto   IN VARCHAR2,
                                  p_nidcargo     IN NUMBER,
                                  p_nidsede      IN NUMBER,
                                  p_retorno      OUT NUMBER,
                                  p_Mensaje      OUT VARCHAR2) IS
  
    nCountEstudio      number(8);
    nCountExperencia   number(8);
    nCountConocimiento number(8);
    cEtapa             varchar2(2);
    cMensaje           Varchar2(2000);
    cDescrip           detalle_general.descripcion%type;
  
    CURSOR C_SOL IS
      SELECT RP.IDESOL, RP.TIPSOL
        FROM RECLUTAMIENTO_PERSONA RP
       WHERE RP.IDEPOSTULANTE = p_nIPostulante
         AND RP.IDECARGO = p_nidcargo
         AND RP.TIPPUESTO = p_ctippuesto
         AND RP.IDSEDE = p_nidsede;
  
  BEGIN
  
    p_retorno := 0;
  
    BEGIN
      SELECT COUNT(*)
        INTO nCountEstudio
        FROM ESTUDIOS_POSTULANTE EP
       WHERE EP.IDEPOSTULANTE = p_nIPostulante;
    EXCEPTION
      WHEN OTHERS THEN
        nCountEstudio := 0;
    END;
  
    BEGIN
      SELECT COUNT(*)
        INTO nCountExperencia
        FROM EXP_POSTULANTE EX
       WHERE eX.IDEPOSTULANTE = p_nIPostulante;
    EXCEPTION
      WHEN OTHERS THEN
        nCountExperencia := 0;
    END;
  
    BEGIN
      SELECT COUNT(*)
        INTO nCountConocimiento
        FROM CONOGEN_POSTULANTE C
       WHERE C.IDEPOSTULANTE = p_nIPostulante;
    EXCEPTION
      WHEN OTHERS THEN
        nCountConocimiento := 0;
    END;
  
    if nCountEstudio = 0 then
    
      p_retorno := 1;
    
      select d.descripcion
        into cDescrip
        from detalle_general d
       where d.idegeneral = 55
         and d.valor = '01';
    
      IF LENGTH(TRIM(cMensaje)) > 0 THEN
        cMensaje := cMensaje || ', ' || cDescrip;
      ELSE
        cMensaje := cMensaje || ' ' || cDescrip;
      END IF;
    
    end if;
  
    if nCountExperencia = 0 then
    
      p_retorno := 1;
    
      select d.descripcion
        into cDescrip
        from detalle_general d
       where d.idegeneral = 55
         and d.valor = '02';
    
      IF LENGTH(TRIM(cMensaje)) > 0 THEN
        cMensaje := cMensaje || ', ' || cDescrip;
      ELSE
        cMensaje := cMensaje || ' ' || cDescrip;
      END IF;
    end if;
  
    if nCountConocimiento = 0 then
    
      p_retorno := 1;
      select d.descripcion
        into cDescrip
        from detalle_general d
       where d.idegeneral = 55
         and d.valor = '03';
    
      IF LENGTH(TRIM(cMensaje)) > 0 THEN
        cMensaje := cMensaje || ', ' || cDescrip;
      ELSE
        cMensaje := cMensaje || ' ' || cDescrip;
      END IF;
    
    end if;
  
    FOR C1 IN C_SOL LOOP
    
      IF (C1.TIPSOL = '01') THEN
        SELECT SN.TIPETAPA
          INTO cEtapa
          FROM SOLNUEVO_CARGO SN
         WHERE SN.Idesolnuevocargo = C1.IDESOL;
      ELSE
        SELECT SP.TIPETAPA
          INTO cEtapa
          FROM Solreq_Personal SP
         WHERE SP.IDESOLREQPERSONAL = C1.IDESOL;
      END IF;
    
      IF cEtapa <> '08' THEN
        p_retorno := 2;
      
        select d.descripcion
          into cDescrip
          from detalle_general d
         where d.idegeneral = 55
           and d.valor = '04';
      
        cMensaje := cDescrip;
      
        EXIT;
      END IF;
    
    END LOOP;
  
    p_Mensaje := NVL(cMensaje, '');
  
  END SP_VALIDA_POSTULACION;

  /* ------------------------------------------------------------
  Nombre      : SP_POSTULACION
  Proposito   : proceso que realiza la postulacion
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
    24/05/2014  Jaqueline ccana      Modificacion de calculo de conocimientos y experiencia
  ------------------------------------------------------------ */

  PROCEDURE SP_POSTULACION(p_nIdPostulante IN NUMBER,
                           p_cTipPuesto    IN VARCHAR2,
                           p_nIdCargo      IN NUMBER,
                           p_nIdSede       IN NUMBER) IS
  
    CURSOR C_SOL IS
      SELECT IDSOLICITUD,
             TIPRANGOSALARIO,
             IDEAREA,
             TIPSOL,
             NOMCARGO,
             FECPUBLICACION,
             CANTPRESELEC,
             CODCARGO
        FROM (SELECT IDSOLICITUD,
                     TIPRANGOSALARIO,
                     IDEAREA,
                     TIPSOL,
                     NOMCARGO,
                     FECPUBLICACION,
                     CANTPRESELEC,
                     CODCARGO
                FROM (SELECT NVL(N.IDESOLNUEVOCARGO, 0) IDSOLICITUD,
                             (SELECT C.TIPRANGOSALARIO
                                FROM CARGO C
                               WHERE C.IDECARGO = p_nIdCargo) TIPRANGOSALARIO,
                             N.IDEAREA,
                             '01' TIPSOL,
                             N.NOMBRE NOMCARGO,
                             N.FECPUBLICACION,
                             (SELECT CA.CANTPRESELEC
                                FROM CARGO CA
                               WHERE CA.IDECARGO = N.IDECARGO
                                 AND ROWNUM < 2) CANTPRESELEC,
                             N.CODCARGO
                      
                        FROM SOLNUEVO_CARGO N
                       WHERE N.FECPUBLICACION IS NOT NULL
                         AND N.FECEXPIRACION IS NOT NULL
                         AND N.NOMBRE IS NOT NULL
                         AND N.ESTACTIVO = 'A'
                         AND N.TIPETAPA = '04'
                         AND N.IDECARGO = p_nIdCargo
                         AND N.IDESEDE = p_nIdSede
                         AND p_cTipPuesto =
                             (SELECT H.TIPHORARIO
                                FROM HORARIO_CARGO H
                               WHERE H.IDECARGO = p_nIdCargo
                                 AND H.PUNTHORARIO =
                                     (SELECT MAX(G.PUNTHORARIO)
                                        FROM HORARIO_CARGO G
                                       WHERE G.IDECARGO = H.IDECARGO))
                      UNION ALL
                      
                      SELECT S.IDESOLREQPERSONAL IDSOLICITUD,
                             S.TIPRANGOSALARIO TIPRANGOSALARIO,
                             S.IDEAREA,
                             S.TIPSOL,
                             S.NOMCARGO,
                             S.FECPUBLICACION FECPUBLICACION,
                             S.CANTPRESELEC,
                             S.CODCARGO
                      
                        FROM SOLREQ_PERSONAL S
                       WHERE S.TIPSOL IN ('02', '03')
                         AND S.FECPUBLICACION IS NOT NULL
                         AND S.ESTACTIVO = 'A'
                         AND S.TIPETAPA = '04'
                         AND S.IDECARGO = p_nIdCargo
                         AND S.TIPPUESTO = p_cTipPuesto
                         AND S.IDESEDE = p_nIdSede) X
               WHERE 1 = 1
               ORDER BY X.TIPSOL, X.FECPUBLICACION) Y
       WHERE ROWNUM < 2;
  
    /**********************datos generales*****************/
    --Datos de la solicitud de requerimiento
    CURSOR C_SOL_REQ(p_idSol in number) IS
      SELECT SOL.PUNTPOSTUINTE    PUNT_POSTULANTE_INTERNO,
             SOL.PUNTEDAD         PUNT_EDAD,
             SOL.EDADINICIO       EDAD_INICIO,
             SOL.EDADFIN          EDAD_FIN,
             SOL.TIPREQUERIMIENTO TIPO_REQ,
             SOL.TIPRANGOSALARIO  TIP_RANGO_SAL,
             SOL.PUNTSALARIO      PUNT_RANGO_SAL,
             SOL.PUNTSEXO         PUNT_SEXO,
             SOL.SEXO             SEXO,
             SOL.PUNTMINGRAL      PUNTMIN,
             SOL.INDSALARIO       INDSALARIO,
             SOL.INDSEXO          INDSEXO,
             SOL.INDEDAD          INDEDAD,
             SOL.CANTPRESELEC     CANTPRESELECCCION,
             sol.codcargo         CODCARGO
        FROM SOLREQ_PERSONAL SOL
       WHERE SOL.IDECARGO = p_nIdCargo
         AND SOL.IDESOLREQPERSONAL = p_idSol
         AND SOL.ESTACTIVO = 'A';
    -- dato de un nuevo cargo
    CURSOR C_CARGO IS
      SELECT C.PUNTTOTPOSTUINTE PUNT_POSTULANTE_INTERNO,
             C.PUNTEDAD         PUNT_EDAD,
             C.EDADINICIO       EDAD_INICIO,
             C.EDADFIN          EDAD_FIN,
             C.TIPREQUERIMIENTO TIPO_REQ,
             C.TIPRANGOSALARIO  TIP_RANGO_SAL,
             C.PUNTSALARIO      PUNT_RANGO_SAL,
             C.PUNTSEXO         PUNT_SEXO,
             C.SEXO             SEXO,
             C.PUNTMINGRAL      PUNTMIN,
             C.INDSALARIO       INDSALARIO,
             C.INDSEXO          INDSEXO,
             C.INDEDAD          INDEDAD,
             C.CANTPRESELEC     CANTPRESELECCCION,
             C.CODCARGO         CODCARGO
        FROM CARGO C
       WHERE C.IDECARGO = p_nIdCargo;
    -- datos del postulante
    CURSOR C_POSTULANTE IS
      SELECT ROUND(((SYSDATE - P.FECNACIMIENTO) / 365)) EDAD,
             P.TIPESTCIVIL,
             P.TIPSALARIO SALARIO,
             P.INDSEXO SEXO,
             'N' INTERNO,
             P.TIPNACIONALIDAD NACIONALIDAD,
             P.TIPDOCUMENTO TIPDOC,
             P.NUMDOCUMENTO NUMDOC,
             P.TIPESTCIVIL ESTADO_CIVIL,
             P.IDEUBIGEO UBIGEO,
             P.TIPSALARIO RANGO_SALARIO,
             P.TIPDISPTRABAJO DISPONIBILIDAD_HORARIO,
             P.TIPHORARIO HORARIO
        FROM POSTULANTE P
       WHERE P.IDEPOSTULANTE = p_nIdPostulante;
    /**********************datos generales*****************/
  
    /**********************Ubigeo*************************/
  
    CURSOR C_UBIGEO_CARGO IS
      SELECT UC.IDEUBIGEO, UC.PUNTUBIGEO
        FROM UBIGEO_CARGO UC
       WHERE UC.IDECARGO = p_nIdCargo;
  
    CURSOR C_UBIGEO_SOLREQ(p_idSol in number) IS
      SELECT US.IDEUBIGEO, US.PUNTUBIGEO
        FROM UBIGEO_SOLREQ US
       WHERE US.IDESOLREQPERSONAL = p_idSol;
  
    /**********************Ubigeo*************************/
  
    /**********************Conocimientos******************/
    -- concimiento del postulante
    CURSOR C_CONOCIMIENTOS_POSTULANTE IS
      SELECT TIPCONOFIMATICA,
             TIPNOMOFIMATICA,
             TIPIDIOMA,
             TIPCONOCIDIOMA,
             TIPCONOCGENERALES,
             TIPNOMCONOCGRALES,
             TIPNIVELCONOCIMIENTO,
             INDCERTIFICACION,
             (CASE
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELBASICO THEN 1
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELINTERMEDIO THEN 2
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELAVANZADO THEN 3
               ELSE 0
             END) NIVELCONO --jccana 24/05/2014
        FROM CONOGEN_POSTULANTE
       WHERE IDEPOSTULANTE = p_nIdPostulante
         AND ESTACTIVO = 'A';
  
    -- conocimientos de cargo
    CURSOR C_CONOCIMIENTOS_CARGO IS
      SELECT TIPCONOFIMATICA,
             TIPNOMOFIMATICA,
             TIPIDIOMA,
             TIPCONOCIDIOMA,
             TIPCONOGENERAL,
             TIPNOMCONOCGRALES,
             TIPNIVELCONOCIMIENTO,
             INDCERTIFICACION,
             PUNTCONOCIMIENTO,
             (CASE
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELBASICO THEN  1
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELINTERMEDIO THEN  2
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELAVANZADO THEN 3
               ELSE 0
             END) NIVELCONO --jccana 24/05/2014
        FROM CONOGENERAL_CARGO A
       WHERE A.IDECARGO = p_nIdCargo;
  
    --conocimientos solicitud
    CURSOR C_CONOCIMIENTOS_SOL(p_idSol in number) IS
      SELECT TIPCONOFIMATICA,
             TIPNOMOFIMATICA,
             TIPIDIOMA,
             TIPCONOCIDIOMA,
             TIPCONOGENERAL,
             TIPNOMCONOCGRALES,
             TIPNIVELCONOCIMIENTO,
             INDCERTIFICACION,
             PUNTCONOCIMIENTO,
             (CASE
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELBASICO THEN 1
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELINTERMEDIO THEN 2
               WHEN TIPNIVELCONOCIMIENTO = P_NIVELAVANZADO THEN 3
               ELSE 0
             END) NIVELCONO --jccana 24/05/2014
        FROM CONOGENERAL_SOLREQ A
       WHERE A.IDESOLREQPERSONAL = p_idSol;
    /**********************Conocimientos******************/
  
    /**********************Nivel Academico*****************/
    --nivel academieco del cargo
    CURSOR C_NIVELACADEMICO_CARGO IS
      SELECT N.TIPEDUCACION    EDUCACION,
             N.TIPAREAESTUDIO  AREA,
             N.TIPNIVELCANZADO NIVELCANZADO,
             N.PUNTNIVESTUDIO  PUNTAJE
        FROM NIVELACADEMICO_CARGO N
       WHERE N.IDECARGO = p_nIdCargo
         AND N.ESTACTIVO = 'A';
  
    -- nivel academivo del postulante
    CURSOR C_NIVELACADEMICO_POSTULANTE IS
      SELECT P.TIPEDUCACION       EDUCACION,
             P.TIPAREA            AREA,
             P.TIPNIVELALCANZADO  NIVELCANZADO,
             P.TIPTIPOINSTITUCION TIPINSTITUCION,
             P.TIPNOMINSTITUCION  CODINSTITUCION
        FROM ESTUDIOS_POSTULANTE P
       WHERE P.IDEPOSTULANTE = p_nIdPostulante
         AND P.ESTACTIVO = 'A';
  
    --nivel academico de la solcitud
    CURSOR C_NIVELACADEMICO_SOL(p_idSol IN NUMBER) IS
      SELECT S.TIPEDUCACION    EDUCACION,
             S.TIPAREAESTUDIO  AREA,
             S.TIPNIVELCANZADO NIVELCANZADO,
             S.PUNTNIVESTUDIO  PUNTAJE
        FROM NIVELACADEMICO_SOLREQ S
       WHERE S.IDESOLREQPERSONAL = p_idSol
         AND S.ESTACTIVO = 'A';
    /**********************Nivel Academico*****************/
  
    /**********************Centro Estudios*****************/
  
    CURSOR C_CENTROEST_CARGO IS
      SELECT CE.TIPCENESTU     TIPINSTITUCION,
             CE.TIPNOMCENESTU  CODINSTITUCION,
             CE.PUNTACENTROEST PUNTAJE
        FROM CENTROEST_CARGO CE
       WHERE CE.IDECARGO = p_nIdCargo;
  
    CURSOR C_CENTROEST_SOLREQ(p_idSol IN NUMBER) IS
      SELECT S.TIPCENESTU     TIPINSTITUCION,
             S.TIPNOMCENESTU  CODINSTITUCION,
             S.PUNTACENTROEST PUNTAJE
        FROM CENTROEST_SOLREQ S
       WHERE S.IDESOLREQPERSONAL = p_idSol;
  
    /**********************Centro Estudios*****************/
  
    /**********************Experiencia*********************/
    CURSOR C_EXP_POSTULANTE IS
      SELECT EP.TIPCARGOTRABAJO, SUM(TIEMPOEXP) TIMETOT
        FROM (SELECT TIPCARGOTRABAJO,
                     FECTRABINICIO,
                     FECTRABFIN,
                     INDTRABACTUALMENTE,
                     (CASE
                       WHEN 'S' = INDTRABACTUALMENTE then
                        round(nvl(months_between(sysdate, FECTRABINICIO),0))
                       ELSE
                       round(nvl( months_between(FECTRABFIN, FECTRABINICIO),0))
                     END) TIEMPOEXP
                FROM EXP_POSTULANTE E
               WHERE E.IDEPOSTULANTE = p_nIdPostulante) EP
       GROUP BY EP.TIPCARGOTRABAJO;
  
    CURSOR C_EXPERIENCIA_CARGO IS
      SELECT TIPEXPLABORAL, CANTANHOEXP, CANTMESESEXP, PUNTEXPERIENCIA
        FROM EXPERIENCIA_CARGO EC
       WHERE EC.IDECARGO = p_nIdCargo;
  
    CURSOR C_EXPERIENCIA_SOL(p_idSol IN NUMBER) IS
      SELECT ES.TIPEXPLABORAL,
             ES.CANTANHOEXP,
             ES.CANTMESESEXP,
             ES.PUNTEXPERIENCIA
        FROM EXPERIENCIA_SOLREQ ES
       WHERE ES.IDEEXPSOLREQ = p_idSol
         AND ES.ESTACTIVO = 'A';
  
    /**********************Experiencia*********************/
  
    /**********************Discapacidad***********************/
    CURSOR C_DISCAPACIDAD_CARGO IS
      SELECT DC.TIPDISCAPA TIPO, DC.PUNTDISCAPA PUNTAJE
        FROM DISCAPACIDAD_CARGO DC
       WHERE DC.IDECARGO = p_nIdCargo
         AND DC.ESTACTIVO = 'A';
  
    CURSOR C_DISCAPACIDAD_POSTULANTE IS
      SELECT DP.TIPODISCAPACIDAD TIPO
        FROM DISCAPACIDAD_POSTULANTE DP
       WHERE DP.IDEPOSTULANTE = p_nIdPostulante
         AND DP.ESTACTIVO = 'A';
  
    CURSOR C_DISCAPACIDAD_SOLREQ(p_idSol IN NUMBER) IS
      SELECT DS.TIPDISCAPA TIPO, DS.PUNTDISCAPA PUNTAJE
        FROM DISCAPACIDAD_SOLREQ DS
       WHERE DS.IDESOLREQPERSONAL = p_idSol
         AND DS.ESTACTIVO = 'A';
  
    /**********************Discapacidad***********************/
  
    cIdSol            number;
    nCont             number;
    cTipCargoTrabajo  varchar2(3);
    dFecTrabInicio    date;
    dFecTrabFin       date;
    cIndTrabajoActual varchar2(1);
    nNumMesesCargo    NUMBER;
    nNumMesesPos      NUMBER;
    nPuntajeHorario   NUMBER;
    cTipHorario       varchar2(3);
    nCodUbigeo        number(8);
    nTotal            NUMBER;
    nPuntMin          NUMBER;
    cTipSol           VARCHAR2(2);
    nCantPreseleMin   NUMBER;
    nCantPreseleSol   NUMBER;
    cCodCargo         CARGO.CODCARGO%TYPE;
    p_IndPostulacion  VARCHAR2(1);
    cEtapaPost        RECLUTAMIENTO_PERSONA.ESTPOSTULANTE%TYPE;
    nNumMeses         NUMBER(8);
    errPostNoApto EXCEPTION;
    nIndNoApto    NUMBER(8);
    nNumMesesConf number(8);
  BEGIN
  
    BEGIN
      nCont := 0;
    
      FOR C1 IN C_SOL LOOP
      
        ---Validacion de no apto 
      
        nNumMeses := round(nvl(months_between(SYSDATE, C1.FECPUBLICACION),0));
      
        -- numero de meses configurados en la tabla 
        BEGIN
          SELECT TO_NUMBER(NVL(D.VALOR, 0))
            INTO nNumMesesConf
            FROM DETALLE_GENERAL D
           WHERE D.IDEGENERAL = 53
             AND D.ESTACTIVO = 'A';
        EXCEPTION
          WHEN OTHERS THEN
            nNumMesesConf := 0;
        END;
      
        BEGIN
          SELECT nvl(COUNT(*), 0) IndNoApto
            INTO nIndNoApto
            FROM reclutamiento_persona c
           WHERE c.codcargo = c1.CODCARGO
             AND c.idSede = p_nIdSede
             AND c.TipPuesto = p_cTipPuesto
             AND c.idepostulante = p_nIdPostulante
             AND c.estpostulante = P_POSTNO_APTO;
        EXCEPTION
          WHEN OTHERS THEN
            nIndNoApto := 0;
        END;
      
        IF nIndNoApto > 0 THEN
          IF nNumMesesConf > nNumMeses THEN
            RAISE errPostNoApto;
          END IF;
        END IF;
      
        -- Fin validacion de no apto
      
        BEGIN
          SELECT COUNT(*)
            INTO nCantPreseleMin
            FROM RECLUTAMIENTO_PERSONA R
           WHERE R.IDESOL = C1.IDSOLICITUD
             AND R.TIPSOL = C1.TIPSOL
             AND R.ESTPOSTULANTE = P_POSTPRESELEC_AUTOMATICO;
        EXCEPTION
          WHEN OTHERS THEN
            nCantPreseleMin := 0;
        END;
      
        nCantPreseleSol := c1.cantpreselec;
      
        BEGIN
          SELECT C.CODCARGO
            INTO cCodCargo
            FROM CARGO C
           WHERE C.IDECARGO = p_nIdCargo;
        EXCEPTION
          WHEN OTHERS THEN
            cCodCargo := NULL;
        END;
      
        --nuevo cargo
        IF C1.TIPSOL = '01' THEN
          cTipSol := C1.TIPSOL;
          cIdSol  := C1.IDSOLICITUD;
          -- obtiene los datos del postulante
          FOR C3 IN C_POSTULANTE LOOP
            cTipHorario := c3.horario;
            -- 2 tab
            --obtiene los datos del cargo
            -- obtiene el puntaje del ubigeo
            FOR C13 IN C_UBIGEO_CARGO LOOP
              IF C3.UBIGEO = C13.IDEUBIGEO THEN
                nCont := nCont + C13.PUNTUBIGEO;
              END IF;
            END LOOP;
          
            FOR C2 IN C_CARGO LOOP
              --realiza las validacion y obtiene los puntajes
            
              --valida el sexo
              IF C2.INDSEXO = 'S' THEN
                IF C2.SEXO = C3.SEXO OR C2.SEXO = 'A' THEN
                  nCont := nCont + C2.PUNT_SEXO;
                ELSE
                  --postulante no apto
                  RAISE errPostNoApto;
                END IF;
              ELSE
                IF C2.SEXO = C3.SEXO OR C2.SEXO = 'A' THEN
                  nCont := nCont + C2.PUNT_SEXO;
                END IF;
              END IF;
            
              --valida el rango de edad
            
              IF C2.INDEDAD = 'S' THEN
                IF C2.EDAD_INICIO <= C3.EDAD AND C2.EDAD_FIN >= C3.EDAD THEN
                  nCont := nCont + C2.PUNT_EDAD;
                ELSE
                  --postulante no apto
                  RAISE errPostNoApto;
                END IF;
              ELSE
                IF C2.EDAD_INICIO <= C3.EDAD AND C2.EDAD_FIN >= C3.EDAD THEN
                  nCont := nCont + C2.PUNT_EDAD;
                END IF;
              END IF;
            
              -- valida el rango salarial
              IF C2.INDSALARIO = 'S' THEN
                IF C2.TIP_RANGO_SAL = C3.RANGO_SALARIO THEN
                  nCont := nCont + C2.PUNT_RANGO_SAL;
                ELSE
                  --postulante no apto
                  RAISE errPostNoApto;
                END IF;
              ELSE
                IF C2.TIP_RANGO_SAL = C3.RANGO_SALARIO THEN
                  nCont := nCont + C2.PUNT_RANGO_SAL;
                END IF;
              END IF;
            
            END LOOP;
          END LOOP;
        
          --3 tab
          --valida horario
          BEGIN
            SELECT HO.PUNTHORARIO
              INTO nPuntajeHorario
              FROM HORARIO_CARGO HO
             WHERE HO.IDECARGO = p_nIdCargo
               AND HO.TIPHORARIO = cTipHorario;
          EXCEPTION
            WHEN OTHERS THEN
              nPuntajeHorario := 0;
          END;
        
          IF nvl(nPuntajeHorario, 0) > 0 THEN
            nCont := nCont + nPuntajeHorario;
          END IF;
        
          --valida nivel academico
          FOR C8 IN C_NIVELACADEMICO_POSTULANTE LOOP
            FOR C9 IN C_NIVELACADEMICO_CARGO LOOP
            
              IF C8.NIVELCANZADO = C9.NIVELCANZADO AND C8.AREA = C9.AREA THEN
                nCont := nCont + C9.PUNTAJE;
              END IF;
            END LOOP;
            --valida tipo de institucion
            FOR C10 IN C_CENTROEST_CARGO LOOP
              IF C8.TIPINSTITUCION = C10.TIPINSTITUCION AND
                 C8.CODINSTITUCION = C10.CODINSTITUCION THEN
                nCont := nCont + C10.PUNTAJE;
              END IF;
            END LOOP;
          END LOOP;
        
          -- valida experencia 4 tab
          FOR C6 IN C_EXP_POSTULANTE LOOP
            FOR C7 IN C_EXPERIENCIA_CARGO LOOP
            
              IF C6.TIPCARGOTRABAJO = C7.TIPEXPLABORAL THEN
                /*IF 'S' = C6.INDTRABACTUALMENTE then
                  nNumMesesPos := months_between(sysdate, C6.FECTRABINICIO);
                ELSE
                  nNumMesesPos := months_between(C6.FECTRABFIN,
                                                 C6.FECTRABINICIO);
                
                END IF;*/
              
                --nNumMesesCargo := (C7.CANTANHOEXP / 12);
                nNumMesesCargo := (C7.CANTANHOEXP * 12); --jccana 24/05/2014
                nNumMesesCargo := C7.CANTMESESEXP + nNumMesesCargo;
              
                --IF nNumMesesPos >= nNumMesesCargo THEN
                IF C6.TIMETOT >= nNumMesesCargo THEN
                  nCont := nCont + C7.PUNTEXPERIENCIA;
                END IF;
              
              END IF;
            
            END LOOP;
          END LOOP;
        
          --valida conocimientos 5 tab
          /* FOR C4 IN C_CONOCIMIENTOS_POSTULANTE LOOP
            FOR C5 IN C_CONOCIMIENTOS_CARGO LOOP
          
              IF C4.TIPCONOFIMATICA = C5.TIPCONOFIMATICA THEN
                nCont := nCont + C5.PUNTCONOCIMIENTO;
              END IF;
          
              IF C4.TIPIDIOMA = C5.TIPIDIOMA THEN
                nCont := nCont + C5.PUNTCONOCIMIENTO;
              END IF;
          
              IF C4.TIPCONOCGENERALES = C5.TIPCONOGENERAL  THEN
                nCont := nCont + C5.PUNTCONOCIMIENTO;
              END IF;
          
            END LOOP;
          END LOOP;*/
        
          --jccana 24/05/2014 
        
          FOR C4 IN C_CONOCIMIENTOS_POSTULANTE LOOP
            FOR C5 IN C_CONOCIMIENTOS_CARGO LOOP
            
              IF C4.TIPCONOFIMATICA = C5.TIPCONOFIMATICA AND
                 C4.TIPNOMOFIMATICA = C5.TIPNOMOFIMATICA THEN
                IF C4.NIVELCONO >= C5.NIVELCONO THEN
                  nCont := nCont + C5.PUNTCONOCIMIENTO;
                END IF;
              END IF;
            
              IF C4.TIPIDIOMA = C5.TIPIDIOMA AND
                 C4.TIPCONOCIDIOMA = C5.TIPCONOCIDIOMA THEN
                IF C4.NIVELCONO >= C5.NIVELCONO THEN
                  nCont := nCont + C5.PUNTCONOCIMIENTO;
                END IF;
              END IF;
            
              IF C4.TIPCONOCGENERALES = C5.TIPCONOGENERAL AND
                 C4.TIPNOMCONOCGRALES = C5.TIPNOMCONOCGRALES THEN
                IF C4.NIVELCONO >= C5.NIVELCONO THEN
                  nCont := nCont + C5.PUNTCONOCIMIENTO;
                END IF;
              END IF;
            
            END LOOP;
          END LOOP;
        
          --end jccana
          --valida discapacidad
          FOR C11 IN C_DISCAPACIDAD_POSTULANTE LOOP
            FOR C22 IN C_DISCAPACIDAD_CARGO LOOP
            
              IF C11.TIPO = C22.TIPO THEN
                nCont := nCont + C22.PUNTAJE;
              END IF;
            
            END LOOP;
          END LOOP;
        
        ELSE
          --Si es solicitud de ampliacion o reemplazo
          cTipSol := C1.TIPSOL;
          cIdSol  := C1.IDSOLICITUD;
          --obtiene el codigo de cargo
          BEGIN
            SELECT C.CODCARGO
              INTO cCodCargo
              FROM CARGO C
             WHERE C.IDECARGO = p_nIdCargo;
          EXCEPTION
            WHEN OTHERS THEN
              cCodCargo := NULL;
          END;
        
          -- obtiene los datos del postulante
          FOR C3 IN C_POSTULANTE LOOP
            cTipHorario := c3.horario;
          
            -- obtiene el puntaje del ubigeo
            FOR C13 IN C_UBIGEO_SOLREQ(cIdSol) LOOP
              IF C3.UBIGEO = C13.IDEUBIGEO THEN
                nCont := nCont + C13.PUNTUBIGEO;
              END IF;
            END LOOP;
          
            -- 2 tab
            --obtiene los de la solcitud de requerimiento
            FOR C2 IN C_SOL_REQ(cIdSol) LOOP
              --realiza las validacion y obtiene los puntajes
              --valida el sexo
              IF C2.INDSEXO = 'S' THEN
                IF C2.SEXO = C3.SEXO THEN
                  nCont := nCont + C2.PUNT_SEXO;
                ELSE
                  --postulante no apto
                  RAISE errPostNoApto;
                END IF;
              ELSE
                IF C2.SEXO = C3.SEXO THEN
                  nCont := nCont + C2.PUNT_SEXO;
                END IF;
              END IF;
            
              --valida el rango de edad
            
              IF C2.INDEDAD = 'S' THEN
                IF C2.EDAD_INICIO <= C3.EDAD AND C2.EDAD_FIN >= C3.EDAD THEN
                  nCont := nCont + C2.PUNT_EDAD;
                ELSE
                  --postulante no apto
                  RAISE errPostNoApto;
                END IF;
              ELSE
                IF C2.EDAD_INICIO <= C3.EDAD AND C2.EDAD_FIN >= C3.EDAD THEN
                  nCont := nCont + C2.PUNT_EDAD;
                END IF;
              END IF;
            
              -- valida el rango salarial
              IF C2.INDSALARIO = 'S' THEN
                IF C2.TIP_RANGO_SAL = C3.RANGO_SALARIO THEN
                  nCont := nCont + C2.PUNT_RANGO_SAL;
                ELSE
                  --postulante no apto
                  RAISE errPostNoApto;
                END IF;
              ELSE
                IF C2.TIP_RANGO_SAL = C3.RANGO_SALARIO THEN
                  nCont := nCont + C2.PUNT_RANGO_SAL;
                END IF;
              END IF;
            
            END LOOP;
          END LOOP;
        
          --3 tab
          --valida horario
          BEGIN
            SELECT Hs.PUNTHORARIO
              INTO nPuntajeHorario
              FROM HORARIO_SOLREQ Hs
             WHERE Hs.IDESOLREQPERSONAL = cIdSol
               AND Hs.TIPHORARIO = cTipHorario;
          EXCEPTION
            WHEN OTHERS THEN
              nPuntajeHorario := 0;
          END;
        
          IF nvl(nPuntajeHorario, 0) > 0 THEN
            nCont := nCont + nPuntajeHorario;
          END IF;
        
          --valida nivel academico
          FOR C8 IN C_NIVELACADEMICO_POSTULANTE LOOP
            FOR C9 IN C_NIVELACADEMICO_SOL(cIdSol) LOOP
            
              IF C8.NIVELCANZADO = C9.NIVELCANZADO AND C8.AREA = C9.AREA THEN
                nCont := nCont + C9.PUNTAJE;
              END IF;
            END LOOP;
            --valida tipo de institucion
            FOR C10 IN C_CENTROEST_SOLREQ(cIdSol) LOOP
              IF C8.TIPINSTITUCION = C10.TIPINSTITUCION AND
                 C8.CODINSTITUCION = C10.CODINSTITUCION THEN
                nCont := nCont + C10.PUNTAJE;
              END IF;
            END LOOP;
          END LOOP;
        
          -- valida experencia 4 tab
          FOR C6 IN C_EXP_POSTULANTE LOOP
            FOR C7 IN C_EXPERIENCIA_SOL(cIdSol) LOOP
            
              IF C6.TIPCARGOTRABAJO = C7.TIPEXPLABORAL THEN
                /*IF 'S' = C6.INDTRABACTUALMENTE then
                  nNumMesesPos := months_between(sysdate, C6.FECTRABINICIO);
                ELSE
                  nNumMesesPos := months_between(C6.FECTRABFIN,
                                                 C6.FECTRABINICIO);
                
                END IF;*/
              
                --nNumMesesCargo := (C7.CANTANHOEXP / 12);--JCCANA
                nNumMesesCargo := (C7.CANTANHOEXP * 12);
                nNumMesesCargo := C7.CANTMESESEXP + nNumMesesCargo;
              
                --IF nNumMesesPos >= nNumMesesCargo THEN --jccana
                IF C6.TIMETOT >= nNumMesesCargo THEN
                  nCont := nCont + C7.PUNTEXPERIENCIA;
                END IF;
              
              END IF;
            
            END LOOP;
          END LOOP;
        
          --valida conocimientos 5 tab
          /* FOR C4 IN C_CONOCIMIENTOS_POSTULANTE LOOP
            FOR C5 IN C_CONOCIMIENTOS_SOL(cIdSol) LOOP
          
              IF C4.TIPCONOFIMATICA = C5.TIPCONOFIMATICA THEN
                nCont := nCont + C5.PUNTCONOCIMIENTO;
              END IF;
          
              IF C4.TIPIDIOMA = C5.TIPIDIOMA THEN
                nCont := nCont + C5.PUNTCONOCIMIENTO;
              END IF;
          
              IF C4.TIPCONOCGENERALES = C5.TIPCONOGENERAL THEN
                nCont := nCont + C5.PUNTCONOCIMIENTO;
              END IF;
          
            END LOOP;
          END LOOP;*/
        
          --jccana 24/05/2014
          FOR C4 IN C_CONOCIMIENTOS_POSTULANTE LOOP
            FOR C5 IN C_CONOCIMIENTOS_CARGO LOOP
            
              IF C4.TIPCONOFIMATICA = C5.TIPCONOFIMATICA AND
                 C4.TIPNOMOFIMATICA = C5.TIPNOMOFIMATICA THEN
                IF C4.NIVELCONO >= C5.NIVELCONO THEN
                  nCont := nCont + C5.PUNTCONOCIMIENTO;
                END IF;
              END IF;
            
              IF C4.TIPIDIOMA = C5.TIPIDIOMA AND
                 C4.TIPCONOCIDIOMA = C5.TIPCONOCIDIOMA THEN
                IF C4.NIVELCONO >= C5.NIVELCONO THEN
                  nCont := nCont + C5.PUNTCONOCIMIENTO;
                END IF;
              END IF;
            
              IF C4.TIPCONOCGENERALES = C5.TIPCONOGENERAL AND
                 C4.TIPNOMCONOCGRALES = C5.TIPNOMCONOCGRALES THEN
                IF C4.NIVELCONO >= C5.NIVELCONO THEN
                  nCont := nCont + C5.PUNTCONOCIMIENTO;
                END IF;
              END IF;
            
            END LOOP;
          END LOOP;
        
          --end jccana
          --valida discapacidad
          FOR C11 IN C_DISCAPACIDAD_POSTULANTE LOOP
            FOR C22 IN C_DISCAPACIDAD_SOLREQ(cIdSol) LOOP
            
              IF C11.TIPO = C22.TIPO THEN
                nCont := nCont + C22.PUNTAJE;
              END IF;
            
            END LOOP;
          END LOOP;
        
        END IF;
      
      END LOOP;
    
      nTotal := nCont;
    
      IF cTipSol = '01' THEN
      
        BEGIN
          SELECT NVL(C.PUNTMINGRAL, 0) PUNTMIN
            INTO nPuntMin
            FROM CARGO C
           WHERE C.IDECARGO = p_nIdCargo;
        EXCEPTION
          WHEN OTHERS THEN
            nPuntMin := 0;
        END;
      
      ELSE
      
        BEGIN
          SELECT NVL(sr.PUNTMINGRAL, 0) PUNTMIN
            INTO nPuntMin
            FROM SOLREQ_PERSONAL sr
           WHERE sr.idesolreqpersonal = cIdSol;
        EXCEPTION
          WHEN OTHERS THEN
            nPuntMin := 0;
        END;
      
      END IF;
    
      IF nTotal >= nPuntMin THEN
        --INSERTA CON ESTADO PRESELECCIONADO
      
        --IF nCantPreseleSol >= nCantPreseleMin THEN
        IF nCantPreseleSol > nCantPreseleMin THEN
          --jccana
        
          PR_INTRANET_ED.SP_VALIDA_SELECCION(p_nIdPostulante,
                                             p_nIdSede,
                                             p_IndPostulacion);
        
          IF 'S' = p_IndPostulacion THEN
            cEtapaPost := P_POSTNO_PRESELECCIONADO;
          ELSE
            cEtapaPost := P_POSTPRESELEC_AUTOMATICO;
          END IF;
        
          INSERT INTO RECLUTAMIENTO_PERSONA
            (IDERECLUTAPERSONA,
             IDEPOSTULANTE,
             IDESOL,
             TIPSOL,
             IDECARGO,
             ESTACTIVO,
             ESTPOSTULANTE,
             PTOTOTAL,
             FECCREACION,
             TIPPUESTO,
             IDSEDE,
             codCargo)
          VALUES
            (IDERECLUTAPERSONA_SQ.NEXTVAL,
             p_nIdPostulante,
             cIdSol,
             cTipSol,
             p_nIdCargo,
             'A',
             cEtapaPost,
             nTotal,
             SYSDATE,
             p_cTipPuesto,
             p_nIdSede,
             cCodCargo);
          COMMIT;
        ELSE
        
          INSERT INTO RECLUTAMIENTO_PERSONA
            (IDERECLUTAPERSONA,
             IDEPOSTULANTE,
             IDESOL,
             TIPSOL,
             IDECARGO,
             ESTACTIVO,
             ESTPOSTULANTE,
             PTOTOTAL,
             FECCREACION,
             TIPPUESTO,
             IDSEDE,
             codCargo)
          VALUES
            (IDERECLUTAPERSONA_SQ.NEXTVAL,
             p_nIdPostulante,
             cIdSol,
             cTipSol,
             p_nIdCargo,
             'A',
             P_POSTNO_PRESELECCIONADO,
             nTotal,
             SYSDATE,
             p_cTipPuesto,
             p_nIdSede,
             cCodCargo);
          COMMIT;
        END IF;
      
      ELSE
        --INSERTA CON ESTADO NO_PRESELECCIONADO
      
        INSERT INTO RECLUTAMIENTO_PERSONA
          (IDERECLUTAPERSONA,
           IDEPOSTULANTE,
           IDESOL,
           TIPSOL,
           IDECARGO,
           ESTACTIVO,
           ESTPOSTULANTE,
           PTOTOTAL,
           FECCREACION,
           TIPPUESTO,
           IDSEDE,
           codCargo)
        VALUES
          (IDERECLUTAPERSONA_SQ.NEXTVAL,
           p_nIdPostulante,
           cIdSol,
           cTipSol,
           p_nIdCargo,
           'A',
           P_POSTNO_PRESELECCIONADO,
           nTotal,
           SYSDATE,
           p_cTipPuesto,
           p_nIdSede,
           cCodCargo);
        COMMIT;
      END IF;
    
    EXCEPTION
      WHEN errPostNoApto THEN
        INSERT INTO RECLUTAMIENTO_PERSONA
          (IDERECLUTAPERSONA,
           IDEPOSTULANTE,
           IDESOL,
           TIPSOL,
           IDECARGO,
           ESTACTIVO,
           ESTPOSTULANTE,
           PTOTOTAL,
           FECCREACION,
           TIPPUESTO,
           IDSEDE,
           CodCargo)
        VALUES
          (IDERECLUTAPERSONA_SQ.NEXTVAL,
           p_nIdPostulante,
           cIdSol,
           cTipSol,
           p_nIdCargo,
           'A',
           P_POSTNO_APTO,
           0,
           SYSDATE,
           p_cTipPuesto,
           p_nIdSede,
           cCodCargo);
        commit;
    END;
  
  END SP_POSTULACION;

  /* ------------------------------------------------------------
  Nombre      : SP_MIS_POSTULACIONES
  Proposito   : obtiene las postulaciones
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_MIS_POSTULACIONES(p_nIdPostulante IN NUMBER,
                                 p_cRetVal       OUT cur_cursor) IS
  
  BEGIN
  
    OPEN p_cRetVal FOR
    
      SELECT X.IDSEDE,
             X.DESSEDE,
             X.IDESOL,
             X.IDECARGO,
             X.TIPSOL,
             X.TIPPUESTO,
             X.DES_PUESTO,
             X.FECHAPOSTULACION,
             X.NOMBRE,
             X.FECEXPIRACION
        FROM (
              
              SELECT RP.Idsede,
                      (SELECT SD.DESCRIPCION
                         FROM SEDE SD
                        WHERE SD.IDESEDE = RP.IDSEDE
                          AND SD.ESTREGISTRO = 'A'
                          AND ROWNUM < 2) DESSEDE,
                      nvl(RP.IDESOL, 0) IDESOL,
                      nvl(RP.IDECARGO, 0) IDECARGO,
                      nvl(RP.TIPSOL, '') TIPSOL,
                      nvl(RP.TIPPUESTO, '') TIPPUESTO,
                      nvl((SELECT D.DESCRIPCION
                            FROM DETALLE_GENERAL D
                           WHERE D.IDEGENERAL = '14'
                             AND D.VALOR = RP.TIPPUESTO),
                          '') DES_PUESTO,
                      RP.FECCREACION FECHAPOSTULACION,
                      (SELECT C.NOMCARGO
                         FROM CARGO C
                        WHERE C.IDECARGO = RP.IDECARGO
                          AND C.ESTACTIVO = 'A'
                          AND ROWNUM < 2) NOMBRE,
                      DECODE(RP.TIPSOL,
                             '01',
                             (SELECT SC.FECEXPIRACION
                                FROM SOLNUEVO_CARGO SC
                               WHERE SC.IDESOLNUEVOCARGO = RP.IDESOL),
                             (SELECT SP.FECEXPIRACACION
                                FROM SOLREQ_PERSONAL SP
                               WHERE SP.IDESOLREQPERSONAL = RP.IDESOL)) FECEXPIRACION
                FROM RECLUTAMIENTO_PERSONA RP
               WHERE RP.ESTACTIVO = 'A'
                 AND RP.IDEPOSTULANTE = p_nIdPostulante
              -- group by RP.Idsede,RP.FECCREACION
              ) X
       WHERE X.FECEXPIRACION IS NOT NULL
       ORDER BY X.FECHAPOSTULACION DESC;
  
  END SP_MIS_POSTULACIONES;

  /* ------------------------------------------------------------
  Nombre      : SP_OBTIENE_DATOS_SOL
  Proposito   : obtiene datos de la solicitud
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_DATOS_SOL(p_nIdSol  IN NUMBER,
                                 p_cTipSol IN VARCHAR2,
                                 p_cRetVal OUT cur_cursor) IS
  BEGIN
  
    IF p_cTipSol = '01' THEN
    
      OPEN p_cRetVal FOR
        SELECT 
               SC.TIPETAPA TIPETAPA,      
               SC.IDECARGO IDECARGO,
               SC.CODCARGO CODCARGO,
               SC.ESTACTIVO ESTADO,
               DECODE(SC.ESTACTIVO, 'A', 'ACTIVO', 'INACTIVO') DESESTADO,
               SC.NOMBRE NOMBRECARGO,
               SC.IDEDEPENDENCIA IDEDEPENDENCIA,
               SC.IDEDEPARTAMENTO IDEDEPARTAMENTO,
               SC.IDEAREA IDEAREA,
               SC.IDESEDE IDESEDE,
               
               (SELECT S.DESCRIPCION
                  FROM SEDE S
                 WHERE S.IDESEDE = SC.IDESEDE
                   AND S.ESTREGISTRO = 'A') NOMBSEDE,
               (SELECT A.NOMDEPENDENCIA
                  FROM DEPENDENCIA A
                 WHERE A.IDEDEPENDENCIA = SC.IDEDEPENDENCIA
                   AND A.IDESEDE = SC.IDESEDE
                   AND A.ESTACTIVO = 'A') NOMBDEPENDENCIA,
               (SELECT B.NOMDEPARTAMENTO
                  FROM DEPARTAMENTO B
                 WHERE B.IDEDEPARTAMENTO = SC.IDEDEPARTAMENTO
                   AND B.IDEDEPENDENCIA = SC.IDEDEPENDENCIA
                   AND B.ESTACTIVO = 'A') NOMBDEPARTAMENTO,
               (SELECT C.NOMAREA
                  FROM AREA C
                 WHERE C.IDEAREA = SC.IDEAREA
                   AND C.IDEDEPARTAMENTO = SC.IDEDEPARTAMENTO
                   AND C.ESTACTIVO = 'A') NOMBAREA,
               NVL(SC.NUMPOSICIONES, 0) NUMVACANTE,
               '01' TIPSOL,
               (SELECT H.TIPHORARIO
                  FROM HORARIO_CARGO H
                 WHERE H.IDECARGO = SC.IDECARGO
                   AND H.PUNTHORARIO =
                       (SELECT MAX(G.PUNTHORARIO)
                          FROM HORARIO_CARGO G
                         WHERE G.IDECARGO = H.IDECARGO)) TIPPUESTO,
                       SC.IDESOLNUEVOCARGO IDESOL
                        
        
          FROM SOLNUEVO_CARGO SC
         WHERE SC.Idesolnuevocargo = p_nIdSol
           AND SC.ESTACTIVO = 'A';
    ELSE
      OPEN p_cRetVal FOR
        SELECT 
               SR.Tipetapa TIPETAPA, 
               SR.IDECARGO IDECARGO,
               SR.CODSOLREQPERSONAL CODCARGO,
               SR.ESTACTIVO ESTADO,
               DECODE(SR.ESTACTIVO, 'A', 'ACTIVO', 'INACTIVO') DESESTADO,
               SR.NOMCARGO NOMBRECARGO,
               SR.IDEDEPENDENCIA IDEDEPENDENCIA,
               SR.IDEDEPARTAMENTO IDEDEPARTAMENTO,
               SR.IDEAREA IDEAREA,
               SR.IDESEDE IDESEDE,
               (SELECT S.DESCRIPCION
                  FROM SEDE S
                 WHERE S.IDESEDE = SR.IDESEDE
                   AND S.Estregistro = 'A') NOMBSEDE,
               (SELECT A.NOMDEPENDENCIA
                  FROM DEPENDENCIA A
                 WHERE A.IDEDEPENDENCIA = SR.IDEDEPENDENCIA
                   AND A.IDESEDE = SR.IDESEDE
                   AND A.ESTACTIVO = 'A') NOMBDEPENDENCIA,
               (SELECT B.NOMDEPARTAMENTO
                  FROM DEPARTAMENTO B
                 WHERE B.IDEDEPARTAMENTO = SR.IDEDEPARTAMENTO
                   AND B.IDEDEPENDENCIA = SR.IDEDEPENDENCIA
                   AND B.ESTACTIVO = 'A') NOMBDEPARTAMENTO,
               (SELECT C.NOMAREA
                  FROM AREA C
                 WHERE C.IDEAREA = SR.IDEAREA
                   AND C.IDEDEPARTAMENTO = SR.IDEDEPARTAMENTO
                   AND C.ESTACTIVO = 'A') NOMBAREA,
               NVL(SR.NUMVACANTES, 0) NUMVACANTE,
               SR.TIPSOL TIPSOL,
               SR.TIPPUESTO TIPPUESTO,
               SR.IDESOLREQPERSONAL IDESOL
          FROM SOLREQ_PERSONAL SR
         WHERE SR.IDESOLREQPERSONAL = p_nIdSol
           AND SR.ESTACTIVO = 'A';
    
    END IF;
  END SP_OBTIENE_DATOS_SOL;

  /* ------------------------------------------------------------
  Nombre      : SP_OBTIENE_POSTULANTES_RANKING
  Proposito   : obtiene los postulantes que se visualizan en el ranking
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_POSTULANTES_RANKING(p_nIdSol      IN NUMBER,
                                           p_cTipSol     IN VARCHAR2,
                                           p_cApePaterno IN VARCHAR2,
                                           p_cApeMaterno IN VARCHAR2,
                                           p_nombre      IN VARCHAR2,
                                           p_cEstado     IN VARCHAR2,
                                           p_cRetVal     OUT cur_cursor) IS
    cEstado varchar2(2);
  BEGIN
  
    OPEN p_cRetVal FOR
      SELECT NVL(R.IDEPOSTULANTE, 0) IDEPOSTULANTE,
             NVL(R.IDERECLUTAPERSONA, 0) IDERECLUTAPERSONA,
             NVL(R.IDESOL, 0) IDESOL,
             NVL(R.TIPSOL, '') TIPSOL,
             NVL(R.IDECARGO, 0) IDECARGO,
             NVL(R.IDECV, 0) IDECV,
             NVL(R.ESTACTIVO, '') ESTACTIVO,
             NVL(R.ESTPOSTULANTE, '') ESTPOSTULANTE,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 51
                 AND D.VALOR = R.ESTPOSTULANTE) DESESTADOPOSTULANTE,
             R.INDCONTACTADO,
             NVL(R.EVALUACION, 0) EVALUACION,
             NVL(R.PTOTOTAL, 0) PTOTOTAL,
             NVL(R.COMENTARIO, '') COMENTARIO,
             
             R.TIPPUESTO,
             R.IDSEDE,
             UPPER(P.APEPATERNO || ' ' || P.APEMATERNO) AS APELLIDOS,
             UPPER(P.PRINOMBRE || ' ' || P.SEGNOMBRE) AS NOMBRES,
             P.TELFIJO TELEFONO_FIJO,
             P.TELMOVIL TELEFONO_MOVIL,
             R.INDCONTACTADO,
             (NVL(R.EVALUACION, 0) || '/' ||
             (DECODE(R.TIPSOL,
                      '01',
                      (SELECT COUNT(*)
                         FROM EVALUACION_CARGO EC
                        WHERE EC.IDECARGO = R.IDECARGO),
                      (SELECT COUNT(*)
                         FROM EVALUACION_SOLREQ ES
                        WHERE ES.IDESOLREQPERSONAL = R.IDESOL)))) NUMERO_EVALUACION,
             (PR_INTRANET_ED.FN_OBTIENE_POSTULACIONES(r.idepostulante,
                                                      r.idsede)) AS POSTULACIONES
        FROM RECLUTAMIENTO_PERSONA R, POSTULANTE P
       WHERE P.IDEPOSTULANTE = R.IDEPOSTULANTE
         AND R.IDESOL = p_nIdSol
         AND TIPSOL = p_cTipSol
         AND R.ESTACTIVO = 'A'
         AND P.ESTACTIVO = 'A'
         AND (p_cEstado IS NULL OR R.ESTPOSTULANTE = p_cEstado)
         AND (p_cApePaterno IS NULL OR
             UPPER(P.APEPATERNO) LIKE '%' || UPPER(p_cApePaterno) || '%')
         AND (p_cApeMaterno IS NULL OR
             UPPER(P.APEMATERNO) LIKE '%' || UPPER(p_cApeMaterno) || '%')
         AND (p_nombre IS NULL OR
             UPPER(P.PRINOMBRE) LIKE '%' || UPPER(p_nombre) || '%')
       ORDER BY R.ESTPOSTULANTE;
  
  END;

  /* ------------------------------------------------------------
  Nombre      : SP_CAMBIA_ESTADO_POST
  Proposito   : Actualiza el estado del postulante
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_CAMBIA_ESTADO_POST(p_nIdRegistro    IN NUMBER,
                                  p_cCodEstadoPost IN VARCHAR2) IS
  
  BEGIN
  
    UPDATE RECLUTAMIENTO_PERSONA r
       SET ESTPOSTULANTE = p_cCodEstadoPost
     WHERE IDERECLUTAPERSONA = p_nIdRegistro
       AND ESTACTIVO = 'A';
    COMMIT;
  
  END SP_CAMBIA_ESTADO_POST;

  /* ------------------------------------------------------------
  Nombre      : SP_POSTULANTE_PRESELEC
  Proposito   : Obtiene los postulantes preseleccionados
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_POSTULANTE_PRESELEC(p_nIdSol  IN NUMBER,
                                   p_cTipSol IN VARCHAR2,
                                   p_cRetVal OUT cur_cursor) IS
    cEstado varchar2(2);
  BEGIN
  
    OPEN p_cRetVal FOR
    
      SELECT X.IDEPOSTULANTE,
             X.IDERECLUTAPERSONA,
             X.IDESOL,
             X.TIPSOL,
             X.IDECARGO,
             X.IDECV,
             X.ESTACTIVO,
             X.ESTPOSTULANTE,
             X.DESESTADOPOSTULANTE,
             X.INDCONTACTADO,
             X.EVALUACION,
             
             ROUND(NVL(PR_INTRANET_ED.FN_OBTIENE_PROMEDIO(X.IDERECLUTAPERSONA,
                                                          X.PUNTMIN,
                                                          X.PROMEDIOEXAMEN,
                                                          X.ESTPOSTULANTE,
                                                          X.INDPROCESO),
                       0)) PTOTOTAL,
             X.COMENTARIO,
             X.TIPPUESTO,
             X.IDSEDE,
             X.APELLIDOS,
             X.NOMBRES,
             X.TELEFONO_FIJO,
             X.TELEFONO_MOVIL,
             X.INDCONTACTADO,
             DECODE(X.ESTPOSTULANTE,
                    '10',
                    DECODE(X.INDPROCESO,
                           'C',
                           ((X.NUMERO_EVALUACION) || '/' ||
                           X.NUMERO_EVALUACION),
                           'N',
                           ((X.NUMERO_EVALUACION - 1) || '/' ||
                           X.NUMERO_EVALUACION),(X.EVALUACION || '/' || X.NUMERO_EVALUACION)),
                    (X.EVALUACION || '/' || X.NUMERO_EVALUACION)) NUMERO_EVALUACION,
             X.PUNTMIN,
             PR_INTRANET_ED.FN_OBTIENE_IND_APROB(X.IDERECLUTAPERSONA,
                                                 X.PUNTMIN,
                                                 X.PROMEDIOEXAMEN,
                                                 X.ESTPOSTULANTE,
                                                 X.INDPROCESO) INDAPROB
        FROM (SELECT NVL(R.IDEPOSTULANTE, 0) IDEPOSTULANTE,
                     NVL(R.IDERECLUTAPERSONA, 0) IDERECLUTAPERSONA,
                     NVL(R.IDESOL, 0) IDESOL,
                     NVL(R.TIPSOL, '') TIPSOL,
                     NVL(R.IDECARGO, 0) IDECARGO,
                     NVL(R.IDECV, 0) IDECV,
                     NVL(R.ESTACTIVO, '') ESTACTIVO,
                     NVL(R.ESTPOSTULANTE, '') ESTPOSTULANTE,
                     (SELECT D.DESCRIPCION
                        FROM DETALLE_GENERAL D
                       WHERE D.IDEGENERAL = 51
                         AND D.VALOR = R.ESTPOSTULANTE) DESESTADOPOSTULANTE,
                     R.INDCONTACTADO,
                     NVL(R.EVALUACION, 0) EVALUACION,
                     
                     NVL(R.COMENTARIO, '') COMENTARIO,
                     R.TIPPUESTO,
                     R.IDSEDE,
                     UPPER(P.APEPATERNO || ' ' || P.APEMATERNO) AS APELLIDOS,
                     UPPER(P.PRINOMBRE || ' ' || P.SEGNOMBRE) AS NOMBRES,
                     P.TELFIJO TELEFONO_FIJO,
                     P.TELMOVIL TELEFONO_MOVIL,
                     R.INDPROCESO,
                     (DECODE(R.TIPSOL,
                             '01',
                             (SELECT COUNT(*)
                                FROM EVALUACION_CARGO EC
                               WHERE EC.IDECARGO = R.IDECARGO),
                             (SELECT COUNT(*)
                                FROM EVALUACION_SOLREQ ES
                               WHERE ES.IDESOLREQPERSONAL = R.IDESOL))) NUMERO_EVALUACION,
                     (DECODE(R.TIPSOL,
                             '01',
                             (SELECT C.PUNTMINEXAMEN
                                FROM CARGO C
                               WHERE C.IDECARGO = R.IDECARGO),
                             (SELECT SP.PUNTMINEXAMEN
                                FROM SOLREQ_PERSONAL SP
                               WHERE SP.IDESOLREQPERSONAL = R.IDESOL))) PUNTMIN,
                     R.PROMEDIOEXAMEN
              
                FROM RECLUTAMIENTO_PERSONA R, POSTULANTE P
               WHERE P.IDEPOSTULANTE = R.IDEPOSTULANTE
                 AND R.IDESOL = p_nIdSol
                 AND TIPSOL = p_cTipSol
                 AND R.ESTACTIVO = 'A'
                 AND R.ESTPOSTULANTE IN ('03', '02', '07', '08', '09', '10')
              
               ORDER BY R.ESTPOSTULANTE) X;
  
  END SP_POSTULANTE_PRESELEC;

  /* ------------------------------------------------------------
  Nombre      : FN_POSTULANTE_PRESELEC
  Proposito   : Obtiene el promedio de los examenes
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  : p_nIdReclutaPersona id del reclutamiento persona
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_OBTIENE_PROMEDIO(p_nIdReclutaPersona IN NUMBER,
                               p_nPuntaje          IN NUMBER,
                               p_nPomedioAnt       IN NUMBER,
                               p_cEstadoPost       IN Reclutamiento_Persona.Estpostulante%type,
                               p_cIndProceso       IN Reclutamiento_Persona.Indproceso%type)
    RETURN NUMBER IS
  
    nPromedio     number;
    cIndAprobado  varchar2(1);
    nCantEvalReal NUMBER;
    nCantEvalConf NUMBER;
  BEGIN
  
    IF p_cEstadoPost != P_POSTPOSTULANTE_POTENCIAL THEN
      BEGIN
        SELECT R.EVALUACION,
               DECODE(R.TIPSOL,
                      '01',
                      (SELECT COUNT(*)
                         FROM EVALUACION_CARGO EC
                        WHERE EC.IDECARGO = R.IDECARGO),
                      (SELECT COUNT(*)
                         FROM EVALUACION_SOLREQ ES
                        WHERE ES.IDESOLREQPERSONAL = R.IDESOL)) NUMERO_EVALUACION
          INTO nCantEvalReal, nCantEvalConf
          FROM RECLUTAMIENTO_PERSONA R
         WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona;
      EXCEPTION
        WHEN OTHERS THEN
          nCantEvalReal := 0;
          nCantEvalConf := -1;
      END;
    
      -- promedio a la fecha
    
      BEGIN
        SELECT (X.NOTA / X.TOTAL)
          INTO nPromedio
          FROM (SELECT NVL(COUNT(*), 0) TOTAL, NVL(SUM(R.NOTAFINAL), 0) NOTA
                  FROM RECLU_PERSO_EXAMEN R
                 WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                   AND R.TIPESTEVALUACION IN ('04', '05')) X;
      EXCEPTION
        WHEN OTHERS THEN
          nPromedio := -1;
      END;
    
      RETURN nvl(nPromedio, 0);
    
    ELSE
    
      IF p_cIndProceso IS NULL THEN
      
        BEGIN
          SELECT R.EVALUACION,
                 DECODE(R.TIPSOL,
                        '01',
                        (SELECT COUNT(*)
                           FROM EVALUACION_CARGO EC
                          WHERE EC.IDECARGO = R.IDECARGO),
                        (SELECT COUNT(*)
                           FROM EVALUACION_SOLREQ ES
                          WHERE ES.IDESOLREQPERSONAL = R.IDESOL)) NUMERO_EVALUACION
            INTO nCantEvalReal, nCantEvalConf
            FROM RECLUTAMIENTO_PERSONA R
           WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona;
        EXCEPTION
          WHEN OTHERS THEN
            nCantEvalReal := 0;
            nCantEvalConf := -1;
        END;
      
        -- promedio a la fecha
      
        BEGIN
          SELECT (X.NOTA / X.TOTAL)
            INTO nPromedio
            FROM (SELECT NVL(COUNT(*), 0) TOTAL,
                         NVL(SUM(R.NOTAFINAL), 0) NOTA
                    FROM RECLU_PERSO_EXAMEN R
                   WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                     AND R.TIPESTEVALUACION IN ('04', '05')) X;
        EXCEPTION
          WHEN OTHERS THEN
            nPromedio := -1;
        END;
      
      ELSE
        /*IF p_nPomedioAnt >= p_nPuntaje THEN
          BEGIN
            SELECT ((R.NOTAFINAL + p_nPomedioAnt) / 2)
              INTO nPromedio
              FROM RECLU_PERSO_EXAMEN R
             WHERE R.IDERECLUTAPERSONA = P_NIDRECLUTAPERSONA
               AND R.INDENTREVFINAL = 'S'
               AND R.TIPESTEVALUACION IN ('04', '05');
          exception
            when others then
              nPromedio := 0;
          END;
        ELSE
          nPromedio := 0;
        END IF;*/
      
        IF p_cIndProceso = 'N' THEN
        
          BEGIN
            SELECT (X.NOTA / X.TOTAL)
              INTO nPromedio
              FROM (SELECT NVL(COUNT(*), 0) TOTAL,
                           NVL(SUM(R.NOTAFINAL), 0) NOTA
                      FROM RECLU_PERSO_EXAMEN R
                     WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                       AND nvl(R.INDENTREVFINAL,'X') <> 'S'
                       AND R.TIPESTEVALUACION IN ('04', '05')) X;
          EXCEPTION
            WHEN OTHERS THEN
              nPromedio := -1;
          END;
        end if;
        
        IF p_cIndProceso = 'S' THEN
          BEGIN
            SELECT (X.NOTA / X.TOTAL)
              INTO nPromedio
              FROM (SELECT NVL(COUNT(*), 0) TOTAL,
                           NVL(SUM(R.NOTAFINAL), 0) NOTA
                      FROM RECLU_PERSO_EXAMEN R
                     WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                       AND nvl(R.INDENTREVFINAL,'X') <> 'S'
                       AND R.TIPESTEVALUACION IN ('04', '05')) X;
          EXCEPTION
            WHEN OTHERS THEN
              nPromedio := -1;
          END;
        END IF;
      
      END IF;
    
      RETURN nvl(nPromedio, 0);
    END IF;
  
  END FN_OBTIENE_PROMEDIO;

  /* ------------------------------------------------------------
  Nombre      : FN_OBTIENE_IND_APROB
  Proposito   : Obtiene inidicador de aprobacion
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  : p_nIdReclutaPersona id del reclutamiento persona
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_OBTIENE_IND_APROB(p_nIdReclutaPersona IN NUMBER,
                                p_nPuntaje          IN NUMBER,
                                p_nPromedio         IN NUMBER,
                                p_cEstadoPost       IN RECLUTAMIENTO_PERSONA.ESTPOSTULANTE%TYPE,
                                p_cIndProceso       IN Reclutamiento_Persona.Indproceso%type)
    RETURN VARCHAR2 IS
  
    nPromedio     number;
    cIndAprobado  varchar2(1);
    nCantEvalReal NUMBER;
    nCantEvalConf NUMBER;
    nNotaFinal    NUMBER;
  BEGIN
  
    IF p_cEstadoPost != P_POSTPOSTULANTE_POTENCIAL THEN
    
      BEGIN
        SELECT R.EVALUACION,
               DECODE(R.TIPSOL,
                      '01',
                      (SELECT COUNT(*)
                         FROM EVALUACION_CARGO EC
                        WHERE EC.IDECARGO = R.IDECARGO),
                      (SELECT COUNT(*)
                         FROM EVALUACION_SOLREQ ES
                        WHERE ES.IDESOLREQPERSONAL = R.IDESOL)) NUMERO_EVALUACION
          INTO nCantEvalReal, nCantEvalConf
          FROM RECLUTAMIENTO_PERSONA R
         WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona;
      EXCEPTION
        WHEN OTHERS THEN
          nCantEvalReal := 0;
          nCantEvalConf := -1;
      END;
    
      IF nCantEvalReal = nCantEvalConf THEN
      
        BEGIN
          SELECT (X.NOTA / X.TOTAL)
            INTO nPromedio
            FROM (SELECT NVL(COUNT(*), 0) TOTAL,
                         NVL(SUM(R.NOTAFINAL), 0) NOTA
                    FROM RECLU_PERSO_EXAMEN R
                   WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                     AND R.TIPESTEVALUACION IN ('04', '05')) X;
        
        EXCEPTION
          WHEN OTHERS THEN
            nPromedio := -1;
        END;
      
      ELSE
        RETURN nvl(cIndAprobado, 'N');
      END IF;
    ELSE
      -- se calcula promedio para potenciales no migrados
    
      IF p_cIndProceso IS NULL THEN
      
        BEGIN
          SELECT R.EVALUACION,
                 DECODE(R.TIPSOL,
                        '01',
                        (SELECT COUNT(*)
                           FROM EVALUACION_CARGO EC
                          WHERE EC.IDECARGO = R.IDECARGO),
                        (SELECT COUNT(*)
                           FROM EVALUACION_SOLREQ ES
                          WHERE ES.IDESOLREQPERSONAL = R.IDESOL)) NUMERO_EVALUACION
            INTO nCantEvalReal, nCantEvalConf
            FROM RECLUTAMIENTO_PERSONA R
           WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona;
        EXCEPTION
          WHEN OTHERS THEN
            nCantEvalReal := 0;
            nCantEvalConf := -1;
        END;
      
        IF nCantEvalReal = nCantEvalConf THEN
        
          BEGIN
            SELECT (X.NOTA / X.TOTAL)
              INTO nPromedio
              FROM (SELECT NVL(COUNT(*), 0) TOTAL,
                           NVL(SUM(R.NOTAFINAL), 0) NOTA
                      FROM RECLU_PERSO_EXAMEN R
                     WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                       AND R.TIPESTEVALUACION IN ('04', '05')) X;
          
          EXCEPTION
            WHEN OTHERS THEN
              nPromedio := -1;
          END;
        
        ELSE
          RETURN nvl(cIndAprobado, 'N');
        END IF;
      
      ELSE
        -- se calcula promedio para potenciales migrados
        BEGIN
          /* SELECT --((R.NOTAFINAL + p_nPromedio) / 2)
          
           INTO nPromedio
           FROM RECLU_PERSO_EXAMEN R
          WHERE R.IDERECLUTAPERSONA = P_NIDRECLUTAPERSONA
            AND R.INDENTREVFINAL <> 'S'
            AND R.TIPESTEVALUACION IN ('04', '05');*/
          IF p_cIndProceso = 'N' THEN
          cIndAprobado := 'N';
           /* SELECT (X.NOTA / X.TOTAL)
              INTO nPromedio
              FROM (SELECT NVL(COUNT(*), 0) TOTAL,
                           NVL(SUM(R.NOTAFINAL), 0) NOTA
                      FROM RECLU_PERSO_EXAMEN R
                     WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                       AND R.INDENTREVFINAL <> 'S'
                       AND R.TIPESTEVALUACION IN ('04', '05')) X;*/
          end if;
          IF p_cIndProceso = 'S' THEN
          cIndAprobado := 'N';
           /* SELECT (X.NOTA / X.TOTAL)
              INTO nPromedio
              FROM (SELECT NVL(COUNT(*), 0) TOTAL,
                           NVL(SUM(R.NOTAFINAL), 0) NOTA
                      FROM RECLU_PERSO_EXAMEN R
                     WHERE R.IDERECLUTAPERSONA = p_nIdReclutaPersona
                       AND R.INDENTREVFINAL <> 'S'
                       AND R.TIPESTEVALUACION IN ('04', '05')) X;*/
          END IF;
        
        exception
          when others then
            nPromedio := 0;
        END;
      
      END IF;
    
    END IF;
  
    IF nPromedio > p_nPuntaje THEN
      cIndAprobado := 'S';
    ELSE
      cIndAprobado := 'N';
    END IF;
  
    RETURN nvl(cIndAprobado, 'N');
  
  END FN_OBTIENE_IND_APROB;

  /* ------------------------------------------------------------
  Nombre      : SP_POSTULANTE_SELECCIONADOS
  Proposito   : Obtiene los postulantes preseleccionados
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_POSTULANTE_SELECCIONADOS(p_nIdSol  IN NUMBER,
                                        p_cTipSol IN VARCHAR2,
                                        p_cRetVal OUT cur_cursor) IS
    cEstado varchar2(2);
  BEGIN
  
    OPEN p_cRetVal FOR
    
      SELECT X.IDEPOSTULANTE,
             X.IDERECLUTAPERSONA,
             X.IDESOL,
             X.TIPSOL,
             X.IDECARGO,
             X.IDECV,
             X.ESTACTIVO,
             X.ESTPOSTULANTE,
             X.DESESTADOPOSTULANTE,
             X.INDCONTACTADO,
             X.EVALUACION,
             
             round(nvl(PR_INTRANET_ED.FN_OBTIENE_PROMEDIO(X.IDERECLUTAPERSONA,
                                                          X.PUNTMIN,
                                                          X.PROMEDIOEXAMEN,
                                                          X.ESTPOSTULANTE,
                                                          X.INDPROCESO),
                       0)) PTOTOTAL,
             X.COMENTARIO,
             X.TIPPUESTO,
             X.IDSEDE,
             X.APELLIDOS,
             X.NOMBRES,
             X.TELEFONO_FIJO,
             X.TELEFONO_MOVIL,
             X.INDCONTACTADO,
             DECODE(X.ESTPOSTULANTE,
                    '10',
                    DECODE(X.INDPROCESO,
                           'C',
                           ((X.NUMERO_EVALUACION) || '/' ||
                           X.NUMERO_EVALUACION),
                           'N',
                           ((X.NUMERO_EVALUACION - 1) || '/' ||
                           X.NUMERO_EVALUACION),(X.EVALUACION || '/' || X.NUMERO_EVALUACION)),
                    (X.EVALUACION || '/' || X.NUMERO_EVALUACION)) NUMERO_EVALUACION,
             X.PUNTMIN,
             PR_INTRANET_ED.FN_OBTIENE_IND_APROB(X.IDERECLUTAPERSONA,
                                                 X.PUNTMIN,
                                                 X.PROMEDIOEXAMEN,
                                                 X.ESTPOSTULANTE,
                                                 X.INDPROCESO) INDAPROB
      
        FROM (SELECT NVL(R.IDEPOSTULANTE, 0) IDEPOSTULANTE,
                     NVL(R.IDERECLUTAPERSONA, 0) IDERECLUTAPERSONA,
                     NVL(R.IDESOL, 0) IDESOL,
                     NVL(R.TIPSOL, '') TIPSOL,
                     NVL(R.IDECARGO, 0) IDECARGO,
                     NVL(R.IDECV, 0) IDECV,
                     NVL(R.ESTACTIVO, '') ESTACTIVO,
                     NVL(R.ESTPOSTULANTE, '') ESTPOSTULANTE,
                     (SELECT D.DESCRIPCION
                        FROM DETALLE_GENERAL D
                       WHERE D.IDEGENERAL = 51
                         AND D.VALOR = R.ESTPOSTULANTE) DESESTADOPOSTULANTE,
                     R.INDCONTACTADO,
                     NVL(R.EVALUACION, 0) EVALUACION,
                     
                     NVL(R.COMENTARIO, '') COMENTARIO,
                     R.TIPPUESTO,
                     R.IDSEDE,
                     UPPER(P.APEPATERNO || ' ' || P.APEMATERNO) AS APELLIDOS,
                     UPPER(P.PRINOMBRE || ' ' || P.SEGNOMBRE) AS NOMBRES,
                     P.TELFIJO TELEFONO_FIJO,
                     P.TELMOVIL TELEFONO_MOVIL,
                     R.INDPROCESO,
                     (DECODE(R.TIPSOL,
                             '01',
                             (SELECT COUNT(*)
                                FROM EVALUACION_CARGO EC
                               WHERE EC.IDECARGO = R.IDECARGO),
                             (SELECT COUNT(*)
                                FROM EVALUACION_SOLREQ ES
                               WHERE ES.IDESOLREQPERSONAL = R.IDESOL))) NUMERO_EVALUACION,
                     (DECODE(R.TIPSOL,
                             '01',
                             (SELECT C.PUNTMINEXAMEN
                                FROM CARGO C
                               WHERE C.IDECARGO = R.IDECARGO),
                             (SELECT SP.PUNTMINEXAMEN
                                FROM SOLREQ_PERSONAL SP
                               WHERE SP.IDESOLREQPERSONAL = R.IDESOL))) PUNTMIN,
                     R.PROMEDIOEXAMEN
                FROM RECLUTAMIENTO_PERSONA R, POSTULANTE P
               WHERE P.IDEPOSTULANTE = R.IDEPOSTULANTE
                 AND R.IDESOL = p_nIdSol
                 AND TIPSOL = p_cTipSol
                 AND R.ESTACTIVO = 'A'
                 AND R.ESTPOSTULANTE IN ('08', '09')
              
               ORDER BY R.ESTPOSTULANTE) X;
  
  END SP_POSTULANTE_SELECCIONADOS;

  /* ------------------------------------------------------------
  NOMBRE      : SP_FINALIZA_CONTRATACION
  Proposito   : Obtiene los postulantes preseleccionados
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_FINALIZA_CONTRATACION(p_nIdSol       IN NUMBER,
                                     p_cTipSol      IN VARCHAR2,
                                     p_cTipPuesto   IN VARCHAR2,
                                     p_nIdSede      IN NUMBER,
                                     p_nIdCargo     IN NUMBER,
                                     p_nIdResp      IN NUMBER,
                                     p_nIdSuceso    IN NUMBER,
                                     p_nIdRolResp   IN NUMBER,
                                     p_nIdRolSuceso IN NUMBER) IS
    --obtiene las solicitudes en cola
    nPromediActual number;
  
    CURSOR C_SOL IS
      SELECT IDSOLICITUD,
             TIPRANGOSALARIO,
             IDEAREA,
             TIPSOL,
             NOMCARGO,
             FECPUBLICACION,
             CANTPRESELEC
        FROM (SELECT IDSOLICITUD,
                     TIPRANGOSALARIO,
                     IDEAREA,
                     TIPSOL,
                     NOMCARGO,
                     FECPUBLICACION,
                     CANTPRESELEC
                FROM (SELECT NVL(N.IDESOLNUEVOCARGO, 0) IDSOLICITUD,
                             (SELECT C.TIPRANGOSALARIO
                                FROM CARGO C
                               WHERE C.IDECARGO = p_nIdCargo) TIPRANGOSALARIO,
                             N.IDEAREA,
                             '01' TIPSOL,
                             N.NOMBRE NOMCARGO,
                             N.FECPUBLICACION,
                             (SELECT CA.CANTPRESELEC
                                FROM CARGO CA
                               WHERE CA.IDECARGO = N.IDECARGO
                                 AND ROWNUM < 2) CANTPRESELEC
                        FROM SOLNUEVO_CARGO N
                       WHERE N.FECPUBLICACION IS NOT NULL
                         AND N.FECEXPIRACION IS NOT NULL
                         AND N.NOMBRE IS NOT NULL
                         AND N.ESTACTIVO = 'A'
                         AND N.TIPETAPA = P_SOLPUBLICADO
                         AND N.IDECARGO = p_nIdCargo
                         AND N.IDESEDE = p_nIdSede
                         AND N.Idesolnuevocargo <> p_nIdSol
                         AND p_cTipPuesto =
                             (SELECT H.TIPHORARIO
                                FROM HORARIO_CARGO H
                               WHERE H.IDECARGO = p_nIdCargo
                                 AND H.PUNTHORARIO =
                                     (SELECT MAX(G.PUNTHORARIO)
                                        FROM HORARIO_CARGO G
                                       WHERE G.IDECARGO = H.IDECARGO))
                      UNION ALL
                      
                      SELECT S.IDESOLREQPERSONAL IDSOLICITUD,
                             S.TIPRANGOSALARIO TIPRANGOSALARIO,
                             S.IDEAREA,
                             S.TIPSOL,
                             S.NOMCARGO,
                             S.FECPUBLICACION FECPUBLICACION,
                             S.CANTPRESELEC
                        FROM SOLREQ_PERSONAL S
                       WHERE S.TIPSOL IN ('02', '03')
                         AND S.FECPUBLICACION IS NOT NULL
                         AND S.ESTACTIVO = 'A'
                         AND S.TIPETAPA = P_SOLPUBLICADO
                         AND S.IDECARGO = p_nIdCargo
                         AND S.TIPPUESTO = p_cTipPuesto
                         AND S.IDESEDE = p_nIdSede
                         AND S.Idesolreqpersonal <> p_nIdSol) X
               WHERE 1 = 1
               ORDER BY X.TIPSOL, X.FECPUBLICACION) Y
       WHERE ROWNUM < 2;
  
    --obtiene los postulantes de una solicitud
    CURSOR C_RECLUTA_PERSONA IS
      SELECT R.IDERECLUTAPERSONA,
             R.IDEPOSTULANTE,
             R.IDESOL,
             R.TIPSOL,
             R.IDECARGO,
             R.IDECV,
             R.ESTPOSTULANTE,
             R.INDCONTACTADO,
             R.EVALUACION,
             R.PTOTOTAL,
             R.COMENTARIO,
             R.TIPPUESTO,
             R.IDSEDE,
             R.CODCARGO,
             R.PROMEDIOEXAMEN,
             R.INDPROCESO
        FROM RECLUTAMIENTO_PERSONA R
       WHERE R.IDESOL = p_nIdSol
         AND R.TIPSOL = p_cTipSol
         AND R.ESTPOSTULANTE NOT IN (P_POSTCONTRATADO, P_POSTEXCLUIDO,
              P_POSTNO_APTO, P_POSTFINALIZADO)
         AND R.ESTACTIVO = 'A';
  
    --obtiene los examenes del postulantes
    CURSOR C_RECLU_PERSO_EXAMEN(p_nIdeReclutaPersona IN RECLU_PERSO_EXAMEN.IDERECLUTAPERSONA%TYPE) IS
      SELECT RPE.IDERECLUTAPERSONA,
             RPE.IDEEVALUACION,
             RPE.TIPSOLICITUD,
             RPE.IDUSUARESPONS,
             RPE.FECEVALUACION,
             RPE.HORAEVALUACION,
             --RPE.NOTAFINAL,
             RPE.ARCHIVO,
             --RPE.COMENTARIORESUL,
            -- RPE.TIPESTEVALUACION,
             RPE.OBSERVACION,
             DECODE(NVL(RPE.INDENTREVFINAL,'X'),'S',RPE.COMENTARIORESUL,'') COMENTARIORESUL,
             DECODE(NVL(RPE.INDENTREVFINAL,'X'),'S',RPE.TIPESTEVALUACION,'') TIPESTEVALUACION,
			       DECODE(NVL(RPE.INDENTREVFINAL,'X'),'S',RPE.NOTAFINAL,0) NOTAFINAL,
             RPE.INDENTREVFINAL
         FROM RECLU_PERSO_EXAMEN RPE
       WHERE RPE.Idereclutapersona = p_nIdeReclutaPersona
       order by rpe.fecmodifica desc;
  
    nIdeReclutaPersona       NUMBER;
    nIdeReclutaPersonaExamen NUMBER;
    nPuntMin                 NUMBER;
    cIndAprobado             varchar2(1);
    nContSol                 NUMBER;
    nIdEvaluacion            NUMBER;
    cTipEtapa                SOLNUEVO_CARGO.TIPETAPA%TYPE;
  
    cIndPotencial varchar2(1);
  BEGIN
  
    IF P_TIPSOLNUEVO = p_cTipSol THEN
    
      SELECT TIPETAPA
        INTO cTipEtapa
        FROM SOLNUEVO_CARGO SN
       WHERE SN.IDESOLNUEVOCARGO = p_nIdSol
         AND SN.IDECARGO = p_nIdCargo
         AND SN.ESTACTIVO = 'A';
    ELSE
    
      SELECT TIPETAPA
        INTO cTipEtapa
        FROM SOLREQ_PERSONAL SR
       WHERE SR.IDESOLREQPERSONAL = p_nIdSol
         AND SR.IDECARGO = p_nIdCargo
         AND SR.ESTACTIVO = 'A';
    
    END IF;
  
    IF cTipEtapa <> P_SOLFINALIZADO THEN
    
      nContSol := pr_intranet_ed.FN_CANT_SOLICITUD_COLA(p_nIdSol,
                                                        p_cTipSol,
                                                        p_cTipPuesto,
                                                        p_nIdSede,
                                                        p_nIdCargo);
      -- si no hay cola C
      IF nContSol = 0 THEN
      
        FOR C2 IN C_RECLUTA_PERSONA LOOP
          IF C2.ESTPOSTULANTE = P_POSTSELECCIONADO OR
             C2.ESTPOSTULANTE = P_POSTPOSTULANTE_POTENCIAL THEN
          
            --actualiza el estado del postulante a potencial
            SELECT R.INDPOTENCIAL
              INTO cIndPotencial
              FROM RECLUTAMIENTO_PERSONA R
             WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
          
            IF cIndPotencial = 'S' THEN
              NULL;
            ELSE
            
              UPDATE RECLUTAMIENTO_PERSONA R
                 SET R.INDPOTENCIAL = 'S', R.FECPOTENCIAL = SYSDATE
              --,R.INDPROCESO = 'N'
               WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
            
            END IF;
          
            UPDATE RECLUTAMIENTO_PERSONA R
               SET R.ESTPOSTULANTE = P_POSTPOSTULANTE_POTENCIAL
            -- R.INDPROCESO = 'N'
             WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
          
            COMMIT;
          
          END IF;
        
          -- se obtiene le puntaje promedio min
          BEGIN
            SELECT NVL(DECODE(R.TIPSOL,
                              '01',
                              (SELECT C.PUNTMINEXAMEN
                                 FROM CARGO C
                                WHERE C.IDECARGO = R.IDECARGO),
                              (SELECT ES.PUNTMINEXAMEN
                                 FROM SOLREQ_PERSONAL ES
                                WHERE ES.IDESOLREQPERSONAL = R.IDESOL)),
                       0) PUNTMIN
              INTO nPuntMin
              FROM RECLUTAMIENTO_PERSONA R
             WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA
               AND R.ESTACTIVO = 'A'
               AND ROWNUM < 2;
          EXCEPTION
            WHEN OTHERS THEN
              nPuntMin := 0;
          END;
        
          cIndAprobado := PR_INTRANET_ED.FN_OBTIENE_IND_APROB(C2.IDERECLUTAPERSONA,
                                                              nPuntMin,
                                                              C2.PROMEDIOEXAMEN,
                                                              C2.ESTPOSTULANTE,
                                                              C2.INDPROCESO);
          --si estan aprobados se actualiza a postulante potencial
          IF C2.ESTPOSTULANTE = P_POSTEN_EVALUACION THEN
            IF cIndAprobado = 'S' THEN
            
              SELECT R.INDPOTENCIAL
                INTO cIndPotencial
                FROM RECLUTAMIENTO_PERSONA R
               WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
            
              IF cIndPotencial = 'S' THEN
                NULL;
              ELSE
              
                UPDATE RECLUTAMIENTO_PERSONA R
                   SET R.INDPOTENCIAL = 'S', R.FECPOTENCIAL = SYSDATE
                --R.INDPROCESO = 'N'
                 WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
              
              END IF;
            
              UPDATE RECLUTAMIENTO_PERSONA R
                 SET R.ESTPOSTULANTE = P_POSTPOSTULANTE_POTENCIAL
              -- ,R.INDPROCESO = 'N'
               ,R.INDPOTENCIAL  = 'S',
               R.FECPOTENCIAL = SYSDATE
               WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
              COMMIT;
              -- si estan desaprobados se finaliza al postulante
            ELSE
            
              UPDATE RECLUTAMIENTO_PERSONA R
                 SET R.ESTPOSTULANTE = P_POSTFINALIZADO
              --  ,R.INDPROCESO = 'N'
               WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
              COMMIT;
            
            END IF;
          END IF;
        
          IF P_POSTPRESELEC_AUTOMATICO = C2.ESTPOSTULANTE OR
             P_POSTPRESELEC_MANUAL = C2.ESTPOSTULANTE OR
             P_POSTNO_PRESELECCIONADO = C2.ESTPOSTULANTE OR
             P_POSTEXCLUIDO = C2.ESTPOSTULANTE THEN
            -- se finaliza al postulante por no cumplir los requisitos
            UPDATE RECLUTAMIENTO_PERSONA R
               SET R.ESTPOSTULANTE = P_POSTFINALIZADO
            -- ,R.INDPROCESO = 'N'
             WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
            COMMIT;
          
          END IF;
        
        END LOOP;
      
      ELSE
      
        -- si hay solicitudes en cola se obtiene la siguiente que esta publicada C
        FOR C1 IN C_SOL LOOP
        
          -- se obtiene el id de evaluacion
          /* IF C1.Tipsol = P_TIPSOLNUEVO THEN
            BEGIN
              SELECT DISTINCT EC.IDEEVALUACIONCARGO
                INTO nIdEvaluacion
                FROM EVALUACION_CARGO EC
               WHERE EC.IDECARGO = p_nIdCargo;
            EXCEPTION
              WHEN OTHERS THEN
                nIdEvaluacion := 0;
            END;
          
          ELSE
            BEGIN
              SELECT DISTINCT ES.IDEEVALUACIONSOLREQ
                INTO nIdEvaluacion
                FROM EVALUACION_SOLREQ ES
               WHERE ES.IDESOLREQPERSONAL = C1.IDSOLICITUD;
            EXCEPTION
              WHEN OTHERS THEN
                nIdEvaluacion := 0;
            END;
          
          END IF;*/
        
          FOR C2 IN C_RECLUTA_PERSONA LOOP
          
            IF C2.ESTPOSTULANTE = P_POSTSELECCIONADO OR
               C2.ESTPOSTULANTE = P_POSTPOSTULANTE_POTENCIAL THEN
              --migra los datos del postulante asociado a una solicitud
              PR_INTRANET_ED.SP_INSERTA_RECLUTA_PERSONA(C2.IDEPOSTULANTE,
                                                        C1.IDSOLICITUD,
                                                        C1.TIPSOL,
                                                        C2.IDECARGO,
                                                        C2.IDECV,
                                                        P_POSTPOSTULANTE_POTENCIAL,
                                                        C2.INDCONTACTADO,
                                                        C2.EVALUACION,
                                                        C2.PTOTOTAL,
                                                        C2.COMENTARIO,
                                                        C2.TIPPUESTO,
                                                        C2.IDSEDE,
                                                        C2.CODCARGO,
                                                        C2.Promedioexamen,
                                                        'C',
                                                        nIdeReclutaPersona);
            
              --finaliza los postulante en la solicitud actual
              UPDATE RECLUTAMIENTO_PERSONA R
                 SET R.ESTPOSTULANTE = P_POSTFINALIZADO
               WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
              COMMIT;
            
              --migra los examenes del postulante
              --Debe tener un id de evaluacion para poder migrar
              ---IF nIdEvaluacion > 0 THEN
              FOR C3 IN C_RECLU_PERSO_EXAMEN(C2.IDERECLUTAPERSONA) LOOP
              
                -- se migra las evaluaciones a la solicitud siguiente
                begin
                PR_INTRANET_ED.SP_INSERTA_RECLUTA_EXAMEM(nIdeReclutaPersona,
                                                         C3.Ideevaluacion,
                                                         C1.TIPSOL,
                                                         C3.IDUSUARESPONS,
                                                         C3.FECEVALUACION,
                                                         C3.HORAEVALUACION,
                                                         C3.NOTAFINAL,
                                                         C3.ARCHIVO,
                                                         C3.COMENTARIORESUL,
                                                         C3.TIPESTEVALUACION,
                                                         C3.OBSERVACION,
                                                         C3.Indentrevfinal,
                                                         nIdeReclutaPersonaExamen
                                                         );
                exception
                 when others then
                   raise_application_error(-20001,'An error was encountered - '||SQLCODE||' -ERROR- '||SQLERRM);
                end;
              END LOOP;
              --END IF;
            
            END IF;
          
            --obtiene a los postulantes aprobados
            BEGIN
              SELECT NVL(DECODE(R.TIPSOL,
                                '01',
                                (SELECT C.PUNTMINEXAMEN
                                   FROM CARGO C
                                  WHERE C.IDECARGO = R.IDECARGO
                                    AND C.ESTACTIVO = 'A'),
                                (SELECT ES.PUNTMINEXAMEN
                                   FROM SOLREQ_PERSONAL ES
                                  WHERE ES.IDESOLREQPERSONAL = R.IDESOL
                                    AND R.ESTACTIVO = 'A')),
                         0) PUNTMIN
                INTO nPuntMin
                FROM RECLUTAMIENTO_PERSONA R
               WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA
                 AND R.ESTACTIVO = 'A';
            EXCEPTION
              WHEN OTHERS THEN
                nPuntMin := 0;
            END;
            --obtiene el indicador de aprobado
            cIndAprobado := 'N';
            cIndAprobado := PR_INTRANET_ED.FN_OBTIENE_IND_APROB(C2.IDERECLUTAPERSONA,
                                                                NPUNTMIN,
                                                                C2.PROMEDIOEXAMEN,
                                                                C2.ESTPOSTULANTE,
                                                                C2.INDPROCESO);
            --si estan aprobados migran a la siguiente solicitud
            IF C2.ESTPOSTULANTE = P_POSTEN_EVALUACION THEN
              --IF cIndAprobado = 'S' THEN
              
                BEGIN
                  SELECT H.PROMEDIOEXAMEN
                    INTO nPromediActual
                    FROM RECLUTAMIENTO_PERSONA H
                   WHERE H.IDERECLUTAPERSONA = c2.idereclutapersona;
                EXCEPTION
                  WHEN OTHERS THEN
                    nPromediActual := 0;
                END;
                
                PR_INTRANET_ED.SP_INSERTA_RECLUTA_PERSONA(C2.IDEPOSTULANTE,
                                                          C1.IDSOLICITUD,
                                                          C1.TIPSOL,
                                                          C2.IDECARGO,
                                                          C2.IDECV,
                                                          C2.ESTPOSTULANTE,
                                                          C2.INDCONTACTADO,
                                                          C2.EVALUACION,
                                                          C2.PTOTOTAL,
                                                          C2.COMENTARIO,
                                                          C2.TIPPUESTO,
                                                          C2.IDSEDE,
                                                          C2.CODCARGO,
                                                          nPromediActual,
                                                          'C',
                                                          nIdeReclutaPersona);
              
                --finaliza los postulantes aprobados en la solciitud actual
                BEGIN
                  UPDATE RECLUTAMIENTO_PERSONA R
                     SET R.ESTPOSTULANTE = P_POSTFINALIZADO
                   WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
                  COMMIT;
                END;
              
                -- IF nIdEvaluacion > 0 THEN
                FOR C3 IN C_RECLU_PERSO_EXAMEN(C2.IDERECLUTAPERSONA) LOOP
                
                  PR_INTRANET_ED.SP_INSERTA_RECLUTA_EXAMEM(nIdeReclutaPersona,
                                                           C3.Ideevaluacion,
                                                           C1.TIPSOL,
                                                           C3.IDUSUARESPONS,
                                                           C3.FECEVALUACION,
                                                           C3.HORAEVALUACION,
                                                           C3.NOTAFINAL,
                                                           C3.ARCHIVO,
                                                           C3.COMENTARIORESUL,
                                                           C3.TIPESTEVALUACION,
                                                           C3.OBSERVACION,
                                                           C3.Indentrevfinal,
                                                           nIdeReclutaPersonaExamen
                                                          );
                
                END LOOP;
                --END IF;
                -- si estan desaprobados se finaliza al postulante
/*              ELSE
              
                UPDATE RECLUTAMIENTO_PERSONA R
                   SET R.ESTPOSTULANTE = P_POSTFINALIZADO
                 WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
                COMMIT;
              
              END IF;
*/            END IF;
          
            -- si esta preseleccionado o no preseleccionado pasan con sus mismos datos
            IF P_POSTPRESELEC_AUTOMATICO = C2.ESTPOSTULANTE OR
               P_POSTPRESELEC_MANUAL = C2.ESTPOSTULANTE OR
               P_POSTNO_PRESELECCIONADO = C2.ESTPOSTULANTE THEN
            
              PR_INTRANET_ED.SP_INSERTA_RECLUTA_PERSONA(C2.IDEPOSTULANTE,
                                                        C1.IDSOLICITUD,
                                                        C1.TIPSOL,
                                                        C2.IDECARGO,
                                                        C2.IDECV,
                                                        C2.ESTPOSTULANTE,
                                                        C2.INDCONTACTADO,
                                                        C2.EVALUACION,
                                                        C2.PTOTOTAL,
                                                        C2.COMENTARIO,
                                                        C2.TIPPUESTO,
                                                        C2.IDSEDE,
                                                        C2.CODCARGO,
                                                        C2.PROMEDIOEXAMEN,
                                                        'C',
                                                        nIdeReclutaPersona);
            
              BEGIN
                UPDATE RECLUTAMIENTO_PERSONA R
                   SET R.ESTPOSTULANTE = P_POSTFINALIZADO
                 WHERE R.IDERECLUTAPERSONA = C2.IDERECLUTAPERSONA;
                COMMIT;
              END;
            
              --IF nIdEvaluacion > 0 THEN
              FOR C3 IN C_RECLU_PERSO_EXAMEN(C2.IDERECLUTAPERSONA) LOOP
              
                PR_INTRANET_ED.SP_INSERTA_RECLUTA_EXAMEM(nIdeReclutaPersona,
                                                         C3.Ideevaluacion,
                                                         C1.TIPSOL,
                                                         C3.IDUSUARESPONS,
                                                         C3.FECEVALUACION,
                                                         C3.HORAEVALUACION,
                                                         C3.NOTAFINAL,
                                                         C3.ARCHIVO,
                                                         C3.COMENTARIORESUL,
                                                         C3.TIPESTEVALUACION,
                                                         C3.OBSERVACION,
                                                         c3.indentrevfinal,
                                                         nIdeReclutaPersonaExamen
                                                         );
              
              END LOOP;
              --END IF;
            END IF;
          
          END LOOP;
        
        END LOOP;
      END IF;
    
      --Finaliza la solicitud Actual
      IF P_TIPSOLNUEVO = p_cTipSol THEN
        UPDATE SOLNUEVO_CARGO
           SET TIPETAPA = P_SOLFINALIZADO
         WHERE IDESEDE = p_nIdSede
           AND IDESOLNUEVOCARGO = p_nIdSol
           AND IDECARGO = p_nIdCargo;
        COMMIT;
      
        PR_INTRANET_ED.SP_INSERT_LOG_SOLNUEVO_CARGO(p_nIdSol,
                                                    P_SOLFINALIZADO,
                                                    TO_CHAR(SYSDATE,
                                                            'DD/MM/YYYY'),
                                                    p_nIdSuceso,
                                                    p_nIdRolSuceso,
                                                    p_nIdRolResp,
                                                    p_nIdResp,
                                                    NULL);
      
      ELSE
        UPDATE SOLREQ_PERSONAL
           SET TIPETAPA = P_SOLFINALIZADO
         WHERE IDESEDE = p_nIdSede
           AND IDESOLREQPERSONAL = p_nIdSol
           AND IDECARGO = p_nIdCargo
           AND TIPSOL = p_cTipSol;
        COMMIT;
      
        pr_intranet_ed.sp_insert_log_solreqpersonal(p_nIdSol,
                                                    P_SOLFINALIZADO,
                                                    TO_CHAR(SYSDATE,
                                                            'DD/MM/YYYY'),
                                                    p_nIdSuceso,
                                                    p_nIdRolSuceso,
                                                    p_nIdRolResp,
                                                    p_nIdResp,
                                                    NULL);
      
      END IF;
    
    END IF;
  END SP_FINALIZA_CONTRATACION;

  /* ------------------------------------------------------------
  NOMBRE      : SP_INSERTA_RECLUTA_PERSONA
  Proposito   : Realiza el proceso de cierre de solicitud y migracion de
                postulantes a la siguinte solicitud;
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_INSERTA_RECLUTA_PERSONA(p_nIdPostulante      IN RECLUTAMIENTO_PERSONA.IDEPOSTULANTE%TYPE,
                                       p_cIdSol             IN RECLUTAMIENTO_PERSONA.IDESOL%TYPE,
                                       p_cTipSol            IN RECLUTAMIENTO_PERSONA.TIPSOL%TYPE,
                                       p_nIdCargo           IN RECLUTAMIENTO_PERSONA.IDECARGO%TYPE,
                                       p_nIdCv              IN RECLUTAMIENTO_PERSONA.IDECV%TYPE,
                                       p_cEstadoPost        IN RECLUTAMIENTO_PERSONA.ESTPOSTULANTE%TYPE,
                                       p_cIndContacto       IN RECLUTAMIENTO_PERSONA.INDCONTACTADO%TYPE,
                                       p_nEvaluacion        IN RECLUTAMIENTO_PERSONA.EVALUACION%TYPE,
                                       p_nPuntajeTotal      IN RECLUTAMIENTO_PERSONA.PTOTOTAL%TYPE,
                                       p_cComentario        IN RECLUTAMIENTO_PERSONA.COMENTARIO%TYPE,
                                       p_cTipPuesto         IN RECLUTAMIENTO_PERSONA.TIPPUESTO%TYPE,
                                       p_nIdSede            IN RECLUTAMIENTO_PERSONA.IDSEDE%TYPE,
                                       p_cCodCargo          IN RECLUTAMIENTO_PERSONA.CODCARGO%TYPE,
                                       p_cPromedioEx        IN RECLUTAMIENTO_PERSONA.PROMEDIOEXAMEN%TYPE,
                                       p_cIndProceso        IN RECLUTAMIENTO_PERSONA.INDPROCESO%TYPE,
                                       p_nIdeReclutaPersona OUT NUMBER) IS
  
    nIdeReclutaPersona NUMBER;
    cIndPotencial      VARCHAR2(1);
  BEGIN
  
    BEGIN
      SELECT IDERECLUTAPERSONA_SQ.NEXTVAL
        INTO nIdeReclutaPersona
        FROM DUAL;
    END;
  
    BEGIN
    
      INSERT INTO RECLUTAMIENTO_PERSONA
        (IDERECLUTAPERSONA,
         IDEPOSTULANTE,
         IDESOL,
         TIPSOL,
         IDECARGO,
         IDECV,
         ESTPOSTULANTE,
         INDCONTACTADO,
         EVALUACION,
         PTOTOTAL,
         COMENTARIO,
         FECCREACION,
         TIPPUESTO,
         IDSEDE,
         ESTACTIVO,
         CODCARGO,
         FECMODIFICA,
         PROMEDIOEXAMEN,
         INDPROCESO)
      VALUES
        (nIdeReclutaPersona,
         p_nIdPostulante,
         p_cIdSol,
         p_cTipSol,
         p_nIdCargo,
         p_nIdCv,
         p_cEstadoPost,
         p_cIndContacto,
         p_nEvaluacion,
         p_nPuntajeTotal,
         p_cComentario,
         SYSDATE,
         p_cTipPuesto,
         p_nIdSede,
         'A',
         p_cCodCargo,
         sysdate,
         p_cPromedioEx,
         p_cIndProceso);
    END;
    COMMIT;
  
    IF p_cEstadoPost = P_POSTPOSTULANTE_POTENCIAL THEN
    
      SELECT R.INDPOTENCIAL
        INTO cIndPotencial
        FROM RECLUTAMIENTO_PERSONA R
       WHERE R.IDERECLUTAPERSONA = nIdeReclutaPersona;
    
      IF cIndPotencial = 'S' THEN
        NULL;
      ELSE
      
        UPDATE RECLUTAMIENTO_PERSONA R
           SET R.INDPOTENCIAL = 'S', R.FECPOTENCIAL = SYSDATE
         WHERE R.IDERECLUTAPERSONA = nIdeReclutaPersona;
      
      END IF;
    
    END IF;
    COMMIT;
  
    p_nIdeReclutaPersona := nIdeReclutaPersona;
  
  END SP_INSERTA_RECLUTA_PERSONA;

  /* ------------------------------------------------------------
  NOMBRE      : SP_INSERTA_RECLUTA_PERSONA
  Proposito   : Realiza el proceso de cierre de solicitud y migracion de
                postulantes a la siguinte solicitud;
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_INSERTA_RECLUTA_EXAMEM(p_nIDERECLUTAPERSONA     IN RECLU_PERSO_EXAMEN.IDERECLUTAPERSONA%TYPE,
                                      p_nIDEEVALUACION         IN RECLU_PERSO_EXAMEN.IDEEVALUACION%TYPE,
                                      p_cTIPSOLICITUD          IN RECLU_PERSO_EXAMEN.TIPSOLICITUD%TYPE,
                                      p_nIDUSUARESPONS         IN RECLU_PERSO_EXAMEN.IDUSUARESPONS%TYPE,
                                      p_dFECEVALUACION         IN RECLU_PERSO_EXAMEN.FECEVALUACION%TYPE,
                                      p_dHORAEVALUACION        IN RECLU_PERSO_EXAMEN.HORAEVALUACION%TYPE,
                                      p_nNOTAFINAL             IN RECLU_PERSO_EXAMEN.NOTAFINAL%TYPE,
                                      p_cARCHIVO               IN RECLU_PERSO_EXAMEN.ARCHIVO%TYPE,
                                      p_cCOMENTARIORESUL       IN RECLU_PERSO_EXAMEN.COMENTARIORESUL%TYPE,
                                      p_cTIPESTEVALUACION      IN varchar2,
                                      p_cOBSERVACION           IN varchar2,
                                      p_indEntrevista          IN varchar2,
                                      p_nIdeReclutaPersoExamen OUT number) IS
  
    nIdeReclutaPersoExamen number(8);
  
  BEGIN
  
    BEGIN
      SELECT IDERECLUPERSOEXAMEN_SQ.NEXTVAL
        INTO nIdeReclutaPersoExamen
        FROM DUAL;
    END;
  
    BEGIN
    
      INSERT INTO RECLU_PERSO_EXAMEN
        (IDERECLUPERSOEXAMEN,
         IDERECLUTAPERSONA,
         IDEEVALUACION,
         TIPSOLICITUD,
         IDUSUARESPONS,
         FECEVALUACION,
         HORAEVALUACION,
         NOTAFINAL,
         ARCHIVO,
         COMENTARIORESUL,
         TIPESTEVALUACION,
         OBSERVACION,
         FECMODIFICA,
         INDENTREVFINAL
         )
      VALUES
        (nIdeReclutaPersoExamen,
         p_nIDERECLUTAPERSONA,
         p_nIDEEVALUACION,
         p_cTIPSOLICITUD,
         p_nIDUSUARESPONS,
         p_dFECEVALUACION,
         p_dHORAEVALUACION,
         nvl(p_nNOTAFINAL,0),
         p_cARCHIVO,
         nvl(trim(p_cCOMENTARIORESUL),null),
         p_cTIPESTEVALUACION,
         nvl(trim(p_cOBSERVACION),null),
         SYSDATE,
         p_indEntrevista);
    EXCEPTION
       WHEN OTHERS THEN
         RAISE_APPLICATION_ERROR(-20001,'AN ERROR WAS ENCOUNTERED - '||SQLCODE||' -ERROR- '||SQLERRM);

    END;
    
    
    COMMIT;
  
    p_nIdeReclutaPersoExamen := nIdeReclutaPersoExamen;
  
  END SP_INSERTA_RECLUTA_EXAMEM;

  /* ------------------------------------------------------------
  NOMBRE      : SP_OBTIENE_POTENCIALES
  Proposito   : Obtiene los postulantes potenciales
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_POTENCIALES(p_nIdSol     IN NUMBER,
                                   p_cTipSol    IN VARCHAR2,
                                   p_cTipPuesto IN VARCHAR2,
                                   p_nIdSede    IN NUMBER,
                                   p_nIdCargo   IN NUMBER) IS
  
    --obtiene los examenes del postulantes
    CURSOR C_RECLU_PERSO_EXAMEN(p_nIdeReclutaPersona IN RECLU_PERSO_EXAMEN.IDERECLUTAPERSONA%TYPE) IS
      SELECT RPE.IDERECLUTAPERSONA,
             RPE.IDEEVALUACION,
             RPE.TIPSOLICITUD,
             RPE.IDUSUARESPONS,
             RPE.FECEVALUACION,
             RPE.HORAEVALUACION,
             DECODE(NVL(RPE.INDENTREVFINAL,'X'),'S',RPE.NOTAFINAL,0) NOTAFINAL,
             RPE.ARCHIVO,
             DECODE(NVL(RPE.INDENTREVFINAL,'X'),'S',RPE.COMENTARIORESUL,'') COMENTARIORESUL,
             DECODE(NVL(RPE.INDENTREVFINAL,'X'),'S',RPE.TIPESTEVALUACION,'') TIPESTEVALUACION,
             RPE.OBSERVACION,
             RPE.INDENTREVFINAL
        FROM RECLU_PERSO_EXAMEN RPE
       WHERE RPE.Idereclutapersona = p_nIdeReclutaPersona
        -- AND nvl(RPE.INDENTREVFINAL,'X') <> 'S'
       order by RPE.Fecmodifica desc;
  
    --obtiene los datos del recluta
    CURSOR C_RECLUTAMIENTO_PERSONA(p_cCodCargo cargo.codcargo%type) IS
      SELECT IDERECLUTAPERSONA,
             IDEPOSTULANTE,
             IDESOL,
             TIPSOL,
             IDECARGO,
             IDECV,
             ESTPOSTULANTE,
             INDCONTACTADO,
             EVALUACION,
             PTOTOTAL,
             COMENTARIO,
             TIPPUESTO,
             IDSEDE,
             RP.FECMODIFICA,
             RP.CODCARGO,
             RP.PROMEDIOEXAMEN,
             rp.fecpotencial
        FROM RECLUTAMIENTO_PERSONA RP
       WHERE RP.Codcargo = p_cCodCargo
         AND RP.ESTACTIVO = 'A'
         AND RP.ESTPOSTULANTE = P_POSTPOSTULANTE_POTENCIAL
         AND RP.IDSEDE = p_nIdSede;
  
    nIdeReclutaPersona       NUMBER(8);
    nIdeReclutaPersonaExamen NUMBER(8);
    nIdeReclutaExamenMax     NUMBER(8);
    cCodCargoActual          CARGO.CODCARGO%TYPE;
    cCodCargoAnterior        CARGO.CODCARGO%TYPE;
    nIdEvaluacion            NUMBER(8);
    nEvaluacion              NUMBER(8);
    nValida                  NUMBER(8);
    nNumExamenes             NUMBER(8);
    nNumMeses                NUMBER(8);
    nNumMesesConf            NUMBER(8);
    nVersionActual           NUMBER(8);
    nVersionAnterior         NUMBER(8);
    nContSol                 NUMBER(8);
  
  BEGIN
    BEGIN
    
      -- obtiene si hay solicitudes publicadas
      nContSol := FN_CANT_SOLICITUD_COLA(p_nIdSol,
                                         p_cTipSol,
                                         p_cTipPuesto,
                                         p_nIdSede,
                                         p_nIdCargo);
    
      IF nvl(nContSol, 0) = 0 THEN
        -- codigo de cargo actual
        BEGIN
          SELECT C.CODCARGO, C.VERSION
            INTO cCodCargoActual, nVersionActual
            FROM CARGO C
           WHERE C.IDECARGO = p_nIdCargo
             AND C.ESTACTIVO = 'A';
        EXCEPTION
          WHEN OTHERS THEN
            cCodCargoActual := NULL;
            nVersionActual  := 0;
        END;
        -- Obtiene el id de evaluacion del cargo de la solicitud actual
        /*IF P_TIPSOLNUEVO = p_cTipSol THEN
          BEGIN
            SELECT DISTINCT EC.IDEEVALUACIONCARGO
              INTO nIdEvaluacion
              FROM EVALUACION_CARGO EC
             WHERE EC.IDECARGO = p_nIdCargo;
          EXCEPTION
            WHEN OTHERS THEN
              nIdEvaluacion := 0;
          END;
        ELSE
          BEGIN
            SELECT DISTINCT ES.IDEEVALUACIONSOLREQ
              INTO nIdEvaluacion
              FROM EVALUACION_SOLREQ ES
             WHERE ES.IDESOLREQPERSONAL = p_nIdSol;
          EXCEPTION
            WHEN OTHERS THEN
              nIdEvaluacion := 0;
          END;
        END IF;*/
      
        -- obtiene los postulantes potenciales
        FOR C1 IN C_RECLUTAMIENTO_PERSONA(cCodCargoActual) LOOP
          --valida el numero de meses
          nNumMeses := round(nvl(months_between(SYSDATE, C1.fecpotencial),0));
        
          SELECT TO_NUMBER(NVL(D.VALOR, 0))
            INTO nNumMesesConf
            FROM DETALLE_GENERAL D
           WHERE D.IDEGENERAL = 53
             AND D.ESTACTIVO = 'A';
        
          IF nNumMeses > nNumMesesConf then
          
            --finalizo al postulante porque ya paso el periodo de espera configurado
            UPDATE Reclutamiento_Persona RP
               SET RP.ESTPOSTULANTE = P_POSTFINALIZADO
             WHERE RP.IDERECLUTAPERSONA = C1.IDERECLUTAPERSONA;
            COMMIT;
          
            ---CONTINUE;
          ELSE
          
            -- codigo de cargo anterior
            BEGIN
              SELECT C.CODCARGO, C.VERSION
                INTO cCodCargoAnterior, nVersionAnterior
                FROM CARGO C
               WHERE C.IDECARGO = C1.IDECARGO;
            EXCEPTION
              WHEN OTHERS THEN
                cCodCargoAnterior := NULL;
                nVersionAnterior  := -1;
            END;
          
            --migra al postulante potencial a la nueva solicitud
          
            PR_INTRANET_ED.SP_INSERTA_RECLUTA_PERSONA(c1.idepostulante,
                                                      p_nIdSol,
                                                      p_cTipSol,
                                                      p_nIdCargo,
                                                      c1.IDECV,
                                                      c1.estpostulante,
                                                      c1.INDCONTACTADO,
                                                      c1.EVALUACION,
                                                      c1.PTOTOTAL,
                                                      c1.COMENTARIO,
                                                      p_cTipPuesto,
                                                      p_nIdSede,
                                                      C1.CODCARGO,
                                                      c1.PROMEDIOEXAMEN,
                                                      'N',
                                                      nIdeReclutaPersona);
          
            BEGIN
              -- se finaliza al postulante en la solicitud que lo contenia anteriormente
              UPDATE RECLUTAMIENTO_PERSONA R
                 SET R.ESTPOSTULANTE = P_POSTFINALIZADO
               WHERE R.IDERECLUTAPERSONA = C1.IDERECLUTAPERSONA;
              COMMIT;
            END;
            -- si las versiones son iguales
          
            -- IF nIdEvaluacion > 0 then
            IF nVersionAnterior = nVersionActual THEN
              -- se migra las evaluaciones porque contiene el mismo numero de evaluaciones
              FOR C2 IN C_RECLU_PERSO_EXAMEN(C1.IDERECLUTAPERSONA) LOOP
              
                PR_INTRANET_ED.SP_INSERTA_RECLUTA_EXAMEM(nIdeReclutaPersona,
                                                         C2.IDEEVALUACION,
                                                         p_cTipSol,
                                                         C2.IDUSUARESPONS,
                                                         C2.FECEVALUACION,
                                                         C2.HORAEVALUACION,
                                                         C2.NOTAFINAL,
                                                         C2.ARCHIVO,
                                                         C2.COMENTARIORESUL,
                                                         C2.TIPESTEVALUACION,
                                                         C2.OBSERVACION,
                                                         c2.indentrevfinal,
                                                         nIdeReclutaPersonaExamen);
              
              END LOOP;
            END IF;
            -- END IF;
          END IF;
        END LOOP;
      END IF;
    END;
  END SP_OBTIENE_POTENCIALES;

  /* ------------------------------------------------------------
  NOMBRE      : FN_CANT_SOLICITUD_COLA
  Proposito   : Obtiene numero de soliciudes en cola
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  FUNCTION FN_CANT_SOLICITUD_COLA(p_nIdSol     IN NUMBER,
                                  p_cTipSol    IN VARCHAR2,
                                  p_cTipPuesto IN VARCHAR2,
                                  p_nIdSede    IN NUMBER,
                                  p_nIdCargo   IN NUMBER) RETURN NUMBER IS
    nContSol NUMBER;
  BEGIN
  
    BEGIN
      SELECT COUNT(*)
        INTO nContSol
        FROM (SELECT NVL(N.IDESOLNUEVOCARGO, 0) IDSOLICITUD,
                     (SELECT C.TIPRANGOSALARIO
                        FROM CARGO C
                       WHERE C.IDECARGO = p_nIdCargo) TIPRANGOSALARIO,
                     N.IDEAREA,
                     '01' TIPSOL,
                     N.NOMBRE NOMCARGO,
                     N.FECPUBLICACION,
                     (SELECT CA.CANTPRESELEC
                        FROM CARGO CA
                       WHERE CA.IDECARGO = N.IDECARGO
                         AND ROWNUM < 2) CANTPRESELEC
                FROM SOLNUEVO_CARGO N
               WHERE N.FECPUBLICACION IS NOT NULL
                 AND N.FECEXPIRACION IS NOT NULL
                 AND N.NOMBRE IS NOT NULL
                 AND N.ESTACTIVO = 'A'
                 AND N.TIPETAPA = '04'
                 AND N.IDECARGO = p_nIdCargo
                 AND N.IDESEDE = p_nIdSede
                 AND N.Idesolnuevocargo <> p_nIdSol
                 AND p_cTipPuesto =
                     (SELECT H.TIPHORARIO
                        FROM HORARIO_CARGO H
                       WHERE H.IDECARGO = p_nIdCargo
                         AND H.PUNTHORARIO =
                             (SELECT MAX(G.PUNTHORARIO)
                                FROM HORARIO_CARGO G
                               WHERE G.IDECARGO = H.IDECARGO))
              UNION ALL
              
              SELECT S.IDESOLREQPERSONAL IDSOLICITUD,
                     S.TIPRANGOSALARIO TIPRANGOSALARIO,
                     S.IDEAREA,
                     S.TIPSOL,
                     S.NOMCARGO,
                     S.FECPUBLICACION FECPUBLICACION,
                     S.CANTPRESELEC
                FROM SOLREQ_PERSONAL S
               WHERE S.TIPSOL IN ('02', '03')
                 AND S.FECPUBLICACION IS NOT NULL
                 AND S.ESTACTIVO = 'A'
                 AND S.TIPETAPA = '04'
                 AND S.IDESOLREQPERSONAL <> p_nIdSol
                 AND S.IDECARGO = p_nIdCargo
                 AND S.TIPPUESTO = p_cTipPuesto
                 AND S.IDESEDE = p_nIdSede) X;
    exception
      when others then
        nContSol := 0;
    END;
  
    return nvl(nContSol, 0);
  
  END FN_CANT_SOLICITUD_COLA;

  /* ------------------------------------------------------------
  NOMBRE      : SP_VALIDA_FIN_SOLICITUD
  Proposito   : valida la finalizacion de una solicitud
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_VALIDA_FIN_SOLICITUD(p_nIdSol     IN NUMBER,
                                    p_cTipSol    IN VARCHAR2,
                                    p_cTipPuesto IN VARCHAR2,
                                    p_nIdSede    IN NUMBER,
                                    p_nIdCargo   IN NUMBER,
                                    p_nVancantes IN NUMBER,
                                    p_cRpta      OUT varchar2) IS
  
    cNumContratados NUMBER;
  BEGIN
  
    BEGIN
      SELECT COUNT(*)
        INTO cNumContratados
        FROM RECLUTAMIENTO_PERSONA R
       WHERE R.IDESOL = 1
         AND R.ESTACTIVO = 'A'
         AND R.TIPSOL = P_CTIPSOL
         AND R.IDSEDE = P_NIDSEDE
         AND R.IDECARGO = P_NIDCARGO
         AND R.TIPPUESTO = P_CTIPPUESTO
         AND R.ESTPOSTULANTE = P_POSTCONTRATADO;
    EXCEPTION
      WHEN OTHERS THEN
        cNumContratados := 0;
    END;
  
    IF cNumContratados = p_nVancantes THEN
      p_cRpta := 'S';
    ELSE
      p_cRpta := 'N';
    END IF;
  
  END SP_VALIDA_FIN_SOLICITUD;

  /* ------------------------------------------------------------
  NOMBRE      : SP_OBTIENE_POSTULACION
  Proposito   : obtiene los cargos y solicitudes donde esta postulando el postulante
               si postula a mas de una
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  : p_nIdPostulante id del postulante
                p_nIdSede id de la sede
                p_cRpta devuelve S si esta preseleccionado o contratado
                                 N no esta preseleccionado ni aprobado
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_VALIDA_SELECCION(p_nIdPostulante IN NUMBER,
                                p_nIdSede       IN NUMBER,
                                p_cRpta         OUT VARCHAR2) IS
  
    nPostCargo number;
  BEGIN
    --valida que el postulante no este preseleccionado, contratado
    BEGIN
      SELECT nvl(count(*), 0)
        into nPostCargo
        FROM RECLUTAMIENTO_PERSONA r
       WHERE R.IDEPOSTULANTE = p_nIdPostulante
         AND R.IDSEDE = p_nIdSede
         AND R.ESTACTIVO = 'A'
         /* AND '08' <> DECODE(R.TIPSOL,
              '01',
              (SELECT S.TIPETAPA
                 FROM SOLNUEVO_CARGO S
                WHERE S.IDESOLNUEVOCARGO = R.IDESOL),
              (SELECT G.TIPETAPA
                 FROM SOLREQ_PERSONAL G
                WHERE G.IDESOLREQPERSONAL = R.IDESOL))*/
         AND R.ESTPOSTULANTE IN
             (P_POSTPRESELEC_AUTOMATICO, P_POSTPRESELEC_MANUAL,
              P_POSTCONTRATADO, P_POSTNO_APTO, P_POSTEN_EVALUACION);
    
    EXCEPTION
      WHEN OTHERS THEN
        nPostCargo := 0;
    END;
  
    IF nPostCargo > 0 THEN
      p_cRpta := 'S';
    ELSE
      p_cRpta := 'N';
    END IF;
  
  END SP_VALIDA_SELECCION;

  /* ------------------------------------------------------------
  NOMBRE      : SP_OBTIENE_POSTULACION
  Proposito   : obtiene los cargos y solicitudes donde esta postulando el postulante
               si postula a mas de una
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  : p_nIdPostulante id del postulante
                p_nIdSede id de la sede
                p_cRpta devuelve las postulaciones del postulante
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_OBTIENE_POSTULACIONES(p_nIdPostulante IN NUMBER,
                                    p_nIdSede       IN NUMBER)
    RETURN VARCHAR2 IS
  
    CUrsor C_LISTA_POSTULACIONES IS
      SELECT UPPER(DECODE(R.TIPSOL,
                          '01',
                          (SELECT C.NOMCARGO
                             FROM CARGO C
                            WHERE C.IDECARGO = R.IDECARGO),
                          (SELECT S.NOMCARGO
                             FROM SOLREQ_PERSONAL S
                            WHERE S.IDESOLREQPERSONAL = R.IDESOL))) ||
             ' - ' || (SELECT NVL(D.DESCRIPCION, '')
                         FROM DETALLE_GENERAL D
                        WHERE D.IDEGENERAL = 14
                          AND D.VALOR = R.TIPPUESTO) POSTULACIONES
        FROM RECLUTAMIENTO_PERSONA R
       WHERE R.IDEPOSTULANTE = p_nIdPostulante
         AND R.IDSEDE = p_nIdSede
         AND R.ESTACTIVO = 'A';
  
    cDescripcion VARCHAR2(2000);
    nCont        NUMBER;
  
  BEGIN
  
    nCont := 1;
  
    FOR C1 IN C_LISTA_POSTULACIONES LOOP
      IF nCont = 1 THEN
        cDescripcion := cDescripcion || C1.POSTULACIONES;
      ELSE
        cDescripcion := cDescripcion || ', ' || C1.POSTULACIONES;
      END IF;
    
      nCont := nCont + 1;
    
    END LOOP;
  
    return NVL(cDescripcion, '');
  
  END FN_OBTIENE_POSTULACIONES;

  /* ------------------------------------------------------------
  Nombre      : SP_INSERT_LOG_SOLNUEVO_CARGO
  Proposito   : inserta en el log de solicitud cargo
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_INSERT_LOG_SOLNUEVO_CARGO(p_nIDESOL          IN LOGSOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                         p_cTIPETAPA        IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE,
                                         p_cFECSUCESO       IN VARCHAR2,
                                         p_nIdUsuarioSuceco IN LOGSOLNUEVO_CARGO.USRSUCESO%TYPE,
                                         p_nIdRolSuceso     IN LOGSOLNUEVO_CARGO.ROLSUCESO%TYPE,
                                         p_nIdRolResp       IN LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                                         p_nIdResponble     IN LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE,
                                         p_observacion      IN VARCHAR2) IS
  
    nIDELOGSOLNUEVOCARGO NUMBER(8);
  BEGIN
  
    BEGIN
      SELECT IDELOGSOLNUEVOCARGO_SQ.nextval
        into nIDELOGSOLNUEVOCARGO
        FROM dual;
    
      INSERT INTO LOGSOLNUEVO_CARGO
        (IDELOGSOLNUEVOCARGO,
         IDESOLNUEVOCARGO,
         TIPETAPA,
         ROLRESPONSABLE,
         USRESPONSABLE,
         OBSERVACION,
         FECSUCESO,
         USRSUCESO,
         ROLSUCESO)
      VALUES
        (nIDELOGSOLNUEVOCARGO,
         p_nIDESOL,
         p_cTIPETAPA,
         p_nIdRolResp,
         p_nIdResponble,
         p_observacion,
         SYSDATE,
         p_nIdUsuarioSuceco,
         p_nIdRolSuceso);
    
      COMMIT;
    EXCEPTION
      WHEN OTHERS THEN
        ROLLBACK;
    END;
  
  END SP_INSERT_LOG_SOLNUEVO_CARGO;

  /* ------------------------------------------------------------
  Nombre      : SP_MIGRA_CONTRATADOS
  Proposito   : Migra los controtados
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_MIGRA_CONTRATADOS IS
  
    --obtiene los postulantes contratados
    CURSOR C_RECLUTAMIENTO_PERSONA IS
      SELECT R.IDERECLUTAPERSONA,
             R.IDEPOSTULANTE,
             R.IDESOL,
             R.TIPSOL,
             R.IDECARGO,
             R.IDECV,
             R.ESTACTIVO,
             R.ESTPOSTULANTE,
             R.INDCONTACTADO,
             R.EVALUACION,
             R.PTOTOTAL,
             R.COMENTARIO,
             R.FECCREACION,
             R.USRMODIFICA,
             R.FECMODIFICA,
             R.TIPPUESTO,
             R.IDSEDE,
             R.CODCARGO,
             R.PROMEDIOEXAMEN
        FROM RECLUTAMIENTO_PERSONA R
       WHERE R.IDERECLUTAPERSONA NOT IN
             (SELECT RM.IDERECLUTAPERSONA FROM RECLUTAMIENTO_PERSONA_MIG RM)
         AND R.ESTPOSTULANTE = P_POSTCONTRATADO
         AND R.ESTACTIVO = 'A'
       ORDER BY R.IDSEDE, R.TIPPUESTO, R.CODCARGO;
  
    --obtiene los datos del postulante
    CURSOR C_POSTULANTE(p_idPostulante POSTULANTE.IDEPOSTULANTE%TYPE) IS
      SELECT IDEPOSTULANTE,
             TIPDOCUMENTO,
             NUMDOCUMENTO,
             APEPATERNO,
             APEMATERNO,
             PRINOMBRE,
             SEGNOMBRE,
             TIPNACIONALIDAD,
             FECNACIMIENTO,
             INDSEXO,
             NUMLICENCIA,
             TIPESTCIVIL,
             OBSERVACION,
             TIPVIA,
             NOMVIA,
             NUMDIRECCION,
             INTERIOR,
             MANZANA,
             LOTE,
             CORREO,
             BLOQUE,
             ETAPA,
             IDEUBIGEO,
             TELMOVIL,
             TIPZONA,
             TELFIJO,
             NOMZONA,
             REFERENCIA,
             FOTOPOSTULANTE,
             TIPSALARIO,
             TIPDISPTRABAJO,
             TIPDISPHORARIO,
             TIPHORARIO,
             INDREUBIINTEPAIS,
             INDPARIENTECHSP,
             TIPPARIENTESEDE,
             PARIENTENOMBRE,
             PARIENTECARGO,
             TIPCOMOSEENTERO,
             DESOTROMEDIO,
             INDTREGISTRO,
             ESTACTIVO
        FROM POSTULANTE P
       WHERE P.IDEPOSTULANTE = p_idPostulante
         AND P.ESTACTIVO = 'A';
  
    --obtiene los conocimientos del postulante
    CURSOR C_CONOGEN_POSTULANTE(p_idPostulante POSTULANTE.IDEPOSTULANTE%TYPE) IS
      SELECT IDEPOSTULANTE,
             TIPCONOFIMATICA,
             TIPNOMOFIMATICA,
             TIPIDIOMA,
             TIPCONOCIDIOMA,
             TIPCONOCGENERALES,
             TIPNOMCONOCGRALES,
             TIPNIVELCONOCIMIENTO,
             INDCERTIFICACION,
             ESTACTIVO,
             USRCREACION,
             FECCREACION,
             USRMODIFICACION,
             FECMODIFICACION
        FROM CONOGEN_POSTULANTE CP
       WHERE CP.IDEPOSTULANTE = p_idPostulante
         AND CP.ESTACTIVO = 'A';
  
    --obtiene los parientes del postulante
    CURSOR C_PARIENTES_POSTULANTE(p_nIdPostulante POSTULANTE.IDEPOSTULANTE%TYPE) IS
      SELECT IDEPOSTULANTE,
             APEMATERNO,
             APEPATERNO,
             NOMBRES,
             TIPVINCULO,
             FECNACIMIENTO,
             ESTACTIVO
        FROM PARIENTES_POSTULANTE P
       WHERE P.IDEPOSTULANTE = p_nIdPostulante
         AND P.ESTACTIVO = 'A';
  
    --obtiene las experiencias laborales
  
    CURSOR C_EXP_POSTULANTE(p_nIdPostulante POSTULANTE.IDEPOSTULANTE%TYPE) IS
      SELECT IDEPOSTULANTE,
             NOMEMPRESA,
             TIPCARGOTRABAJO,
             NOMCARGOTRABAJO,
             FECTRABINICIO,
             FECTRABFIN,
             INDTRABACTUALMENTE,
             TIEMPOSERVICIO,
             TIPMOTIVOCESE,
             NOMREFERENTE,
             NUMTELEFMOVILREF,
             CORREOREFERENTE,
             TIPCARGOTRABAJOREF,
             NUMTELEFONOFIJOINST,
             NUMANEXOINST,
             ESTACTIVO,
             FUNCIONESDESEMP
        FROM EXP_POSTULANTE
       WHERE IDEPOSTULANTE = p_nIdPostulante
         AND ESTACTIVO = 'A';
  
    --obtiene los estudios del postulante
  
    CURSOR C_ESTUDIOS_POSTULANTE(p_nIdPostulante POSTULANTE.IDEPOSTULANTE%TYPE) IS
      SELECT IDEPOSTULANTE,
             TIPTIPOINSTITUCION,
             TIPNOMINSTITUCION,
             NOMINSTITUCION,
             TIPAREA,
             TIPEDUCACION,
             TIPNIVELALCANZADO,
             FECESTUDIOINICIO,
             FECESTUDIOFIN,
             INDESTACTUALMENTE,
             ESTACTIVO
        FROM ESTUDIOS_POSTULANTE E
       WHERE E.ESTACTIVO = 'A'
         AND E.IDEPOSTULANTE = p_nIdPostulante;
  
    --obtiene las discapacidades del postulante
    CURSOR C_DISCAPACIDAD_POSTULANTE(p_nIdPostulante POSTULANTE.IDEPOSTULANTE%TYPE) IS
      SELECT IDEDISCAPACIDADPOSTULANTE,
             IDEPOSTULANTE,
             TIPODISCAPACIDAD,
             DESDISCAPACIDAD,
             ESTACTIVO
        FROM DISCAPACIDAD_POSTULANTE D
       WHERE D.IDEPOSTULANTE = p_nIdPostulante
         AND D.ESTACTIVO = 'A';
  
    -- obtiene los examenes del postulante ordenados por fecha
    CURSOR C_RECLU_PERSO_EXAMEN(p_nIdPostulante POSTULANTE.IDEPOSTULANTE%TYPE) IS
      SELECT TIPESTEVALUACION,
             IDERECLUPERSOEXAMEN,
             IDERECLUTAPERSONA,
             IDEEVALUACION,
             TIPSOLICITUD,
             IDUSUARESPONS,
             FECEVALUACION,
             HORAEVALUACION,
             NOTAFINAL,
             ARCHIVO,
             COMENTARIORESUL,
             OBSERVACION,
             NOMARCHIVO,
             INDENTREVFINAL
        FROM RECLU_PERSO_EXAMEN ER
       WHERE ER.IDERECLUTAPERSONA IN
             (SELECT DISTINCT RP.IDERECLUTAPERSONA
                FROM RECLUTAMIENTO_PERSONA RP
               WHERE RP.IDEPOSTULANTE = p_nIdPostulante)
         AND ER.TIPESTEVALUACION IN ('04', '05')
      
       ORDER BY ER.FECMODIFICA ASC;
  
    nIdePpostulante_Mig NUMBER(8);
    nExiste             NUMBER(8);
    dFecha              date;
    cUsuario            POSTULANTE_MIG.USRCREACION%TYPE;
    cMensaje            varchar2(1000);
    err_code            number;
    err_msg             varchar2(250);
    cDato               number;
  BEGIN
  
    FOR C1 IN C_RECLUTAMIENTO_PERSONA LOOP
      BEGIN
      
        BEGIN
          SELECT 1
            INTO nExiste
            FROM RECLUTAMIENTO_PERSONA_MIG P
           WHERE P.IDERECLUTAPERSONA = C1.IDERECLUTAPERSONA;
        EXCEPTION
          WHEN OTHERS THEN
            nExiste := 0;
        END;
        --si existe pasa al siguiente registro
        IF nExiste > 0 THEN
          --continue;
          cDato := 0;
        
        ELSE
        
          BEGIN
            SELECT IDEPOSTULANTE_MIG_SQ.NEXTVAL
              INTO nIdePpostulante_Mig
              FROM DUAL;
          END;
        
          cUsuario := SUBSTR(user, 0, 15);
          dFecha   := sysdate;
        
          INSERT INTO RECLUTAMIENTO_PERSONA_MIG
            (IDERECLUTAPERSONA,
             IDEPOSTULANTE,
             IDESOL,
             TIPSOL,
             IDECARGO,
             IDECV,
             ESTACTIVO,
             ESTPOSTULANTE,
             INDCONTACTADO,
             EVALUACION,
             PTOTOTAL,
             COMENTARIO,
             FECCREACION,
             TIPPUESTO,
             IDSEDE,
             CODCARGO,
             PROMEDIOEXAMEN,
             IDE_MIG)
          VALUES
            (C1.IDERECLUTAPERSONA,
             C1.IDEPOSTULANTE,
             C1.IDESOL,
             C1.TIPSOL,
             C1.IDECARGO,
             C1.IDECV,
             C1.ESTACTIVO,
             C1.ESTPOSTULANTE,
             C1.INDCONTACTADO,
             C1.EVALUACION,
             C1.PTOTOTAL,
             C1.COMENTARIO,
             dFecha,
             C1.TIPPUESTO,
             C1.IDSEDE,
             C1.CODCARGO,
             C1.PROMEDIOEXAMEN,
             nIdePpostulante_Mig);
        
          --Inserta datos del postulante
          cMensaje := 'cursor C_CONOGEN_POSTULANTE';
        
          BEGIN
            SELECT 1
              INTO nExiste
              FROM RECLUTAMIENTO_PERSONA_MIG RM
             WHERE RM.IDEPOSTULANTE = C1.IDEPOSTULANTE;
          EXCEPTION
            WHEN OTHERS THEN
              nExiste := 0;
          END;
        
          IF NVL(nExiste, 0) = 0 THEN
          
            FOR C2 in C_POSTULANTE(C1.IDEPOSTULANTE) LOOP
            
              INSERT INTO POSTULANTE_MIG
                (IDEPOSTULANTEMIG,
                 IDEPOSTULANTE,
                 TIPDOCUMENTO,
                 NUMDOCUMENTO,
                 APEPATERNO,
                 APEMATERNO,
                 PRINOMBRE,
                 SEGNOMBRE,
                 TIPNACIONALIDAD,
                 FECNACIMIENTO,
                 INDSEXO,
                 NUMLICENCIA,
                 TIPESTCIVIL,
                 OBSERVACION,
                 TIPVIA,
                 NOMVIA,
                 NUMDIRECCION,
                 INTERIOR,
                 MANZANA,
                 LOTE,
                 CORREO,
                 BLOQUE,
                 ETAPA,
                 IDEUBIGEO,
                 TELMOVIL,
                 TIPZONA,
                 TELFIJO,
                 NOMZONA,
                 REFERENCIA,
                 FOTOPOSTULANTE,
                 TIPSALARIO,
                 TIPDISPTRABAJO,
                 TIPDISPHORARIO,
                 TIPHORARIO,
                 INDREUBIINTEPAIS,
                 INDPARIENTECHSP,
                 TIPPARIENTESEDE,
                 PARIENTENOMBRE,
                 PARIENTECARGO,
                 TIPCOMOSEENTERO,
                 DESOTROMEDIO,
                 INDTREGISTRO,
                 ESTACTIVO,
                 USRCREACION,
                 FECCREACION)
              VALUES
                (IDEPOSTULANTEMIG_SQ.NEXTVAL,
                 C2.IDEPOSTULANTE,
                 C2.TIPDOCUMENTO,
                 C2.NUMDOCUMENTO,
                 C2.APEPATERNO,
                 C2.APEMATERNO,
                 C2.PRINOMBRE,
                 C2.SEGNOMBRE,
                 C2.TIPNACIONALIDAD,
                 C2.FECNACIMIENTO,
                 C2.INDSEXO,
                 C2.NUMLICENCIA,
                 C2.TIPESTCIVIL,
                 C2.OBSERVACION,
                 C2.TIPVIA,
                 C2.NOMVIA,
                 C2.NUMDIRECCION,
                 C2.INTERIOR,
                 C2.MANZANA,
                 C2.LOTE,
                 C2.CORREO,
                 C2.BLOQUE,
                 C2.ETAPA,
                 C2.IDEUBIGEO,
                 C2.TELMOVIL,
                 C2.TIPZONA,
                 C2.TELFIJO,
                 C2.NOMZONA,
                 C2.REFERENCIA,
                 C2.FOTOPOSTULANTE,
                 C2.TIPSALARIO,
                 C2.TIPDISPTRABAJO,
                 C2.TIPDISPHORARIO,
                 C2.TIPHORARIO,
                 C2.INDREUBIINTEPAIS,
                 C2.INDPARIENTECHSP,
                 C2.TIPPARIENTESEDE,
                 C2.PARIENTENOMBRE,
                 C2.PARIENTECARGO,
                 C2.TIPCOMOSEENTERO,
                 C2.DESOTROMEDIO,
                 C2.INDTREGISTRO,
                 C2.ESTACTIVO,
                 cUsuario,
                 dFecha);
            
            END LOOP;
          ELSE
            -- se eliminan todos los registros del postulante
            DELETE FROM CONOGEN_POSTULANTE_MIG
             WHERE IDEPOSTULANTE = C1.IDEPOSTULANTE;
          
            DELETE FROM PARIENTES_POSTULANTE_MIG
             WHERE IDEPOSTULANTE = C1.IDEPOSTULANTE;
          
            DELETE FROM EXP_POSTULANTE_MIG
             WHERE IDEPOSTULANTE = C1.IDEPOSTULANTE;
          
            DELETE FROM EXP_POSTULANTE_MIG
             WHERE IDEPOSTULANTE = C1.IDEPOSTULANTE;
          
            DELETE FROM ESTUDIOS_POSTULANTE_MIG
             WHERE IDEPOSTULANTE = C1.IDEPOSTULANTE;
          
            DELETE FROM DISCAPACIDAD_POSTULANTE_MIG
             WHERE IDEPOSTULANTE = C1.IDEPOSTULANTE;
          
            COMMIT;
          
            FOR C9 in C_POSTULANTE(C1.IDEPOSTULANTE) LOOP
            
              UPDATE POSTULANTE_MIG
                 SET TIPDOCUMENTO     = C9.TIPDOCUMENTO,
                     NUMDOCUMENTO     = C9.NUMDOCUMENTO,
                     APEPATERNO       = C9.APEPATERNO,
                     APEMATERNO       = C9.APEMATERNO,
                     PRINOMBRE        = C9.PRINOMBRE,
                     SEGNOMBRE        = C9.SEGNOMBRE,
                     TIPNACIONALIDAD  = C9.TIPNACIONALIDAD,
                     FECNACIMIENTO    = C9.FECNACIMIENTO,
                     INDSEXO          = C9.INDSEXO,
                     NUMLICENCIA      = C9.NUMLICENCIA,
                     TIPESTCIVIL      = C9.TIPESTCIVIL,
                     OBSERVACION      = C9.OBSERVACION,
                     TIPVIA           = C9.TIPVIA,
                     NOMVIA           = C9.NOMVIA,
                     NUMDIRECCION     = C9.NUMDIRECCION,
                     INTERIOR         = C9.INTERIOR,
                     MANZANA          = C9.MANZANA,
                     LOTE             = C9.LOTE,
                     CORREO           = C9.CORREO,
                     BLOQUE           = C9.BLOQUE,
                     ETAPA            = C9.ETAPA,
                     IDEUBIGEO        = C9.IDEUBIGEO,
                     TELMOVIL         = C9.TELMOVIL,
                     TIPZONA          = C9.TIPZONA,
                     TELFIJO          = C9.TELFIJO,
                     NOMZONA          = C9.NOMZONA,
                     REFERENCIA       = C9.REFERENCIA,
                     FOTOPOSTULANTE   = C9.FOTOPOSTULANTE,
                     TIPSALARIO       = C9.TIPSALARIO,
                     TIPDISPTRABAJO   = C9.TIPDISPTRABAJO,
                     TIPDISPHORARIO   = C9.TIPDISPHORARIO,
                     TIPHORARIO       = C9.TIPHORARIO,
                     INDREUBIINTEPAIS = C9.INDREUBIINTEPAIS,
                     INDPARIENTECHSP  = C9.INDPARIENTECHSP,
                     TIPPARIENTESEDE  = C9.TIPPARIENTESEDE,
                     PARIENTENOMBRE   = C9.PARIENTENOMBRE,
                     PARIENTECARGO    = C9.PARIENTECARGO,
                     TIPCOMOSEENTERO  = C9.TIPCOMOSEENTERO,
                     DESOTROMEDIO     = C9.DESOTROMEDIO,
                     INDTREGISTRO     = C9.INDTREGISTRO,
                     ESTACTIVO        = C9.ESTACTIVO,
                     USRMODIFICACION  = cUsuario,
                     FECMODIFICACION  = dFecha
               WHERE IDEPOSTULANTE = C1.IDEPOSTULANTE;
            
            END LOOP;
          END IF;
          -- Inserta conocimiento del postulante
          cMensaje := 'cursor C_CONOGEN_POSTULANTE';
        
          FOR C3 IN C_CONOGEN_POSTULANTE(C1.IDEPOSTULANTE) LOOP
          
            INSERT INTO CONOGEN_POSTULANTE_MIG
              (IDECONOGENPOSTULANTE_MIG,
               
               IDEPOSTULANTE,
               TIPCONOFIMATICA,
               TIPNOMOFIMATICA,
               TIPIDIOMA,
               TIPCONOCIDIOMA,
               TIPCONOCGENERALES,
               TIPNOMCONOCGRALES,
               TIPNIVELCONOCIMIENTO,
               INDCERTIFICACION,
               ESTACTIVO,
               USRCREACION,
               FECCREACION)
            VALUES
              (IDECONOGENPOSTULANTE_MIG_SQ.NEXTVAL,
               
               C3.IDEPOSTULANTE,
               C3.TIPCONOFIMATICA,
               C3.TIPNOMOFIMATICA,
               C3.TIPIDIOMA,
               C3.TIPCONOCIDIOMA,
               C3.TIPCONOCGENERALES,
               C3.TIPNOMCONOCGRALES,
               C3.TIPNIVELCONOCIMIENTO,
               C3.INDCERTIFICACION,
               C3.ESTACTIVO,
               cUsuario,
               dFecha);
          
          END LOOP;
        
          --inserta los datos de los parientes
          cMensaje := 'cursor C_PARIENTES_POSTULANTE';
        
          FOR C4 IN C_PARIENTES_POSTULANTE(C1.IDEPOSTULANTE) LOOP
          
            INSERT INTO PARIENTES_POSTULANTE_MIG
              (IDEPARIENTESPOSTULANTEMIG,
               
               IDEPOSTULANTE,
               APEMATERNO,
               APEPATERNO,
               NOMBRES,
               TIPVINCULO,
               FECNACIMIENTO,
               ESTACTIVO,
               USRCREACION,
               FECCREACION)
            VALUES
              (IDEPARIENTESPOSTULANTEMIG_SQ.NEXTVAL,
               
               C4.IDEPOSTULANTE,
               C4.APEMATERNO,
               C4.APEPATERNO,
               C4.NOMBRES,
               C4.TIPVINCULO,
               C4.FECNACIMIENTO,
               C4.ESTACTIVO,
               cUsuario,
               dFecha);
          
          END LOOP;
        
          --inserta las experiencias del postulante
          cMensaje := 'cursor C_EXP_POSTULANTE';
        
          FOR C5 IN C_EXP_POSTULANTE(C1.IDEPOSTULANTE) LOOP
          
            INSERT INTO EXP_POSTULANTE_MIG
              (IDEEXPPOSTULANTEMIG,
               
               IDEPOSTULANTE,
               NOMEMPRESA,
               TIPCARGOTRABAJO,
               NOMCARGOTRABAJO,
               FECTRABINICIO,
               FECTRABFIN,
               INDTRABACTUALMENTE,
               TIEMPOSERVICIO,
               TIPMOTIVOCESE,
               NOMREFERENTE,
               NUMTELEFMOVILREF,
               CORREOREFERENTE,
               TIPCARGOTRABAJOREF,
               NUMTELEFONOFIJOINST,
               NUMANEXOINST,
               ESTACTIVO,
               USRCREACION,
               FECCREACION,
               FUNCIONESDESEMP)
            VALUES
              (IDEEXPPOSTULANTEMIG_SQ.NEXTVAL,
               
               C5.IDEPOSTULANTE,
               C5.NOMEMPRESA,
               C5.TIPCARGOTRABAJO,
               C5.NOMCARGOTRABAJO,
               C5.FECTRABINICIO,
               C5.FECTRABFIN,
               C5.INDTRABACTUALMENTE,
               C5.TIEMPOSERVICIO,
               C5.TIPMOTIVOCESE,
               C5.NOMREFERENTE,
               C5.NUMTELEFMOVILREF,
               C5.CORREOREFERENTE,
               C5.TIPCARGOTRABAJOREF,
               C5.NUMTELEFONOFIJOINST,
               C5.NUMANEXOINST,
               C5.ESTACTIVO,
               cUsuario,
               dFecha,
               C5.FUNCIONESDESEMP);
          
          END LOOP;
        
          --Inserta estudios del postulante
          cMensaje := 'cursor C_ESTUDIOS_POSTULANTE';
        
          FOR C6 IN C_ESTUDIOS_POSTULANTE(C1.IDEPOSTULANTE) LOOP
          
            INSERT INTO ESTUDIOS_POSTULANTE_MIG
              (IDEESTUDIOSPOSTULANTEMIG,
               
               IDEPOSTULANTE,
               TIPTIPOINSTITUCION,
               TIPNOMINSTITUCION,
               NOMINSTITUCION,
               TIPAREA,
               TIPEDUCACION,
               TIPNIVELALCANZADO,
               FECESTUDIOINICIO,
               FECESTUDIOFIN,
               INDESTACTUALMENTE,
               ESTACTIVO,
               USRCREACION,
               FECCREACION)
            VALUES
              (IDEESTUDIOSPOSTULANTEMIG_SQ.NEXTVAL,
               
               C6.IDEPOSTULANTE,
               C6.TIPTIPOINSTITUCION,
               C6.TIPNOMINSTITUCION,
               C6.NOMINSTITUCION,
               C6.TIPAREA,
               C6.TIPEDUCACION,
               C6.TIPNIVELALCANZADO,
               C6.FECESTUDIOINICIO,
               C6.FECESTUDIOFIN,
               C6.INDESTACTUALMENTE,
               C6.ESTACTIVO,
               cUsuario,
               dFecha);
          
          END LOOP;
        
          --Inserta discapacidades del postulante
          cMensaje := 'cursor C_DISCAPACIDAD_POSTULANTE';
        
          FOR C7 IN C_DISCAPACIDAD_POSTULANTE(C1.IDEPOSTULANTE) LOOP
          
            INSERT INTO DISCAPACIDAD_POSTULANTE_MIG
              (IDEDISCAPACIDADPOSTMIG,
               
               IDEPOSTULANTE,
               TIPODISCAPACIDAD,
               DESDISCAPACIDAD,
               ESTACTIVO,
               USRCREACION,
               FECCREACION)
            VALUES
              (IDEDISCAPACIDADPOSTMIG_SQ.NEXTVAL,
               
               C7.IDEPOSTULANTE,
               C7.TIPODISCAPACIDAD,
               C7.DESDISCAPACIDAD,
               C7.ESTACTIVO,
               cUsuario,
               dFecha);
          
          END LOOP;
        
          FOR C8 IN C_RECLU_PERSO_EXAMEN(C1.IDEPOSTULANTE) LOOP
            nExiste := 0;
          
            BEGIN
              SELECT 1
                INTO nExiste
                FROM RECLU_PERSO_EXAMEN_MIG
               WHERE IDERECLUPERSOEXAMEN = C8.IDERECLUPERSOEXAMEN;
            EXCEPTION
              WHEN OTHERS THEN
                nExiste := 0;
            END;
          
            IF nExiste = 1 THEN
              cDato := 0;
              --continue
            ELSE
            
              INSERT INTO RECLU_PERSO_EXAMEN_MIG
                (IDERECLUPERSOEXAMIG,
                 IDERECLUPERSOEXAMEN,
                 IDERECLUTAPERSONA,
                 IDEEVALUACION,
                 TIPSOLICITUD,
                 IDUSUARESPONS,
                 FECEVALUACION,
                 HORAEVALUACION,
                 NOTAFINAL,
                 ARCHIVO,
                 COMENTARIORESUL,
                 TIPESTEVALUACION,
                 OBSERVACION,
                 USRCREACION,
                 FECCREACION,
                 NOMARCHIVO,
                 INDENTREVFINAL)
              VALUES
                (IDERECLUPERSOEXAMIG_SQ.NEXTVAL,
                 C8.IDERECLUPERSOEXAMEN,
                 C8.IDERECLUTAPERSONA,
                 C8.IDEEVALUACION,
                 C8.TIPSOLICITUD,
                 C8.IDUSUARESPONS,
                 C8.FECEVALUACION,
                 C8.HORAEVALUACION,
                 C8.NOTAFINAL,
                 C8.ARCHIVO,
                 C8.COMENTARIORESUL,
                 C8.TIPESTEVALUACION,
                 C8.OBSERVACION,
                 cUsuario,
                 dFecha,
                 C8.NOMARCHIVO,
                 C8.INDENTREVFINAL);
            END IF;
          END LOOP;
          COMMIT;
        END IF;
      
      EXCEPTION
        WHEN OTHERS THEN
        
          err_code := SQLCODE;
          err_msg  := SUBSTR(SQLERRM, 1, 200);
        
          cMensaje := cMensaje || ' - ' || err_code || ' - ' || err_msg;
        
          INSERT INTO LOG_MIG_CONTRATADOS
            (IDLOGMIGCONT,
             IDE_MIG,
             IDEPOSTULANTE,
             IDESE,
             CODCARGO,
             TIPPUESTO,
             IDERECLUTAPERSONA,
             ERROR)
          VALUES
            (IDLOGMIGCONT_SQ.NEXTVAL,
             nIdePpostulante_Mig,
             C1.IDEPOSTULANTE,
             C1.IDSEDE,
             C1.CODCARGO,
             C1.TIPPUESTO,
             C1.IDERECLUTAPERSONA,
             cMensaje);
        
          COMMIT;
        
        --CONTINUE;
      END;
    
    END LOOP;
  
  END SP_MIGRA_CONTRATADOS;

  /* ------------------------------------------------------------
  Nombre      : FN_OBTIENE_DESUBIGEO
  Proposito   : obtiene la descripcion de ubigeo
                departamento  3
                provincia  2
                distrito   1
  
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_OBTIENE_DESUBIGEO(p_nIdUbigeo IN postulante.ideubigeo%type,
                                p_nInd      IN NUMBER) RETURN VARCHAR2 IS
  
    CURSOR C_UBIGEO_PADRE(p_nIdUbigeoPadre ubigeo.ideubigeopadre%type) IS
      SELECT IDEUBIGEO, IDEUBIGEOPADRE, NOMBRE
        FROM UBIGEO U
       WHERE U.Ideubigeopadre = p_nIdUbigeoPadre;
  
    cDistrito     ubigeo.nombre%type;
    cProvincia    ubigeo.nombre%type;
    cDepartamento ubigeo.nombre%type;
  
    nIdUbigeo       ubigeo.ideubigeo%type;
    nIdeUbigeoPadre ubigeo.ideubigeopadre%type;
    cNombre         ubigeo.nombre%type;
  
  BEGIN
  
    BEGIN
      BEGIN
        SELECT IDEUBIGEO, IDEUBIGEOPADRE, NOMBRE
          INTO nIdUbigeo, nIdeUbigeoPadre, cNombre
          FROM UBIGEO U
         WHERE U.IDEUBIGEO = p_nIdUbigeo;
      EXCEPTION
        WHEN OTHERS THEN
          nIdUbigeo       := 0;
          nIdeUbigeoPadre := 0;
          cNombre         := NULL;
      END;
      cDistrito := cNombre;
    
      IF nIdeUbigeoPadre > 0 THEN
        BEGIN
          SELECT IDEUBIGEO, IDEUBIGEOPADRE, NOMBRE
            INTO nIdUbigeo, nIdeUbigeoPadre, cNombre
            FROM UBIGEO U
           WHERE U.IDEUBIGEO = nIdeUbigeoPadre;
        EXCEPTION
          WHEN OTHERS THEN
            nIdUbigeo       := 0;
            nIdeUbigeoPadre := 0;
            cNombre         := NULL;
        END;
        cProvincia := cNombre;
      
        IF nIdeUbigeoPadre > 0 THEN
          BEGIN
            SELECT IDEUBIGEO, IDEUBIGEOPADRE, NOMBRE
              INTO nIdUbigeo, nIdeUbigeoPadre, cNombre
              FROM UBIGEO U
             WHERE U.IDEUBIGEO = nIdeUbigeoPadre;
          EXCEPTION
            WHEN OTHERS THEN
              nIdUbigeo       := 0;
              nIdeUbigeoPadre := 0;
              cNombre         := NULL;
          END;
        END IF;
        cDepartamento := cNombre;
      
      END IF;
    
      IF p_nInd = 1 THEN
        RETURN UPPER(cDistrito);
      END IF;
      IF p_nInd = 2 THEN
        RETURN UPPER(cProvincia);
      END IF;
      IF p_nInd = 3 THEN
        RETURN UPPER(cDepartamento);
      END IF;
    
      IF p_nInd IS NULL THEN
        RETURN NULL;
      END IF;
    EXCEPTION
      WHEN OTHERS THEN
        RETURN NULL;
      
    END;
  
  END FN_OBTIENE_DESUBIGEO;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_POSTULANTE
  Proposito   : obtiene los datos del postulante para el reporte
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_POSTULANTE(p_nIdPostulante IN postulante.idepostulante%type,
                             p_Rpta          OUT cur_cursor)
  
   IS
  
  BEGIN
  
    OPEN p_Rpta FOR
      SELECT P.TIPDOCUMENTO CODTIPDOCUMENTO,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 6
                 AND D.VALOR = P.TIPDOCUMENTO) DESTIPDOCUMENTO,
             P.NUMDOCUMENTO,
             UPPER(P.APEPATERNO) APEPATERNO,
             UPPER(P.APEMATERNO) APEMATERNO,
             UPPER(P.PRINOMBRE) PRINOMBRE,
             UPPER(P.SEGNOMBRE) SEGNOMBRE,
             P.TIPNACIONALIDAD CODNACIONALIDAD,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 7
                 AND D.VALOR = P.TIPNACIONALIDAD) DESNACIONALIDAD,
             TO_CHAR(P.FECNACIMIENTO, 'DD/MM/YYYY') FECNACIMIENTO,
             P.INDSEXO CODSEXO,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 15
                 AND D.VALOR = P.INDSEXO) DESSEXO,
             P.TIPESTCIVIL CODESTADOCIVIL,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 8
                 AND D.VALOR = P.TIPESTCIVIL) DESESTADOCIVIL,
             P.NUMLICENCIA,
             P.OBSERVACION,
             'PERU' PAIS,
             P.IDEUBIGEO,
             PR_INTRANET_ED.FN_OBTIENE_DESUBIGEO(P.IDEUBIGEO, 1) AS DESDISTRITO,
             PR_INTRANET_ED.FN_OBTIENE_DESUBIGEO(P.IDEUBIGEO, 2) AS DESPROVINCIA,
             PR_INTRANET_ED.FN_OBTIENE_DESUBIGEO(P.IDEUBIGEO, 3) AS DESDEPARTAMENTO,
             P.CORREO,
             TO_CHAR(P.TELMOVIL) TELMOVIL,
             P.TELFIJO,
             P.REFERENCIA,
             P.TIPVIA CODTIPVIA,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 9
                 AND D.VALOR = P.TIPVIA) DESTIPVIA,
             P.NOMVIA,
             P.NUMDIRECCION,
             P.MANZANA,
             P.BLOQUE,
             P.TIPZONA CODTIPZONA,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 10
                 AND D.VALOR = P.TIPZONA) DESTIPZONA,
             P.NOMZONA,
             P.INTERIOR,
             P.LOTE,
             P.ETAPA,
             P.FOTOPOSTULANTE,
             
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 11
                 AND D.VALOR = P.TIPSALARIO) SALARIO,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 12
                 AND D.VALOR = P.TIPDISPTRABAJO) DISPTRABAJO,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 13
                 AND D.VALOR = P.TIPDISPHORARIO) DISPPHORARIO,
             (SELECT D.DESCRIPCION
                FROM DETALLE_GENERAL D
               WHERE D.IDEGENERAL = 14
                 AND D.VALOR = P.TIPHORARIO) HORATRABAJO,
             DECODE(P.INDREUBIINTEPAIS, 'S', 'SI', 'N', 'NO', '') REUBICACION,
             DECODE(P.INDPARIENTECHSP, 'S', 'SI', 'N', 'NO', '') PARIENTETRAB,
             UPPER((SELECT S.DESCRIPCION
                     FROM SEDE S
                    WHERE S.IDESEDE = P.TIPPARIENTESEDE
                      AND S.ESTREGISTRO = 'A')) AS PARIENTESEDE,
             UPPER(P.PARIENTECARGO) PARIENTECARGO,
             UPPER((SELECT D.DESCRIPCION
                     FROM DETALLE_GENERAL D
                    WHERE D.IDEGENERAL = 54
                      AND D.VALOR = P.TIPCOMOSEENTERO)) COMOSEENTERO,
             '( ' ||
             (DECODE(P.FECNACIMIENTO,
                     NULL,
                     '',
                     TRUNC((SYSDATE -
                           TO_DATE(TO_CHAR(P.FECNACIMIENTO, 'DD/MM/RRRR'),
                                    'DD/MM/RRRR')) / 365,
                           0))) || ' )' EDAD,
             upper(DECODE(P.FECNACIMIENTO,
                          NULL,
                          '',
                          TO_CHAR(P.FECNACIMIENTO, 'DD')) || ' de ' ||
                   PR_INTRANET_ED.FN_DES_MES(DECODE(P.FECNACIMIENTO,
                                                    NULL,
                                                    0,
                                                    TO_CHAR(P.FECNACIMIENTO,
                                                            'MM')),
                                             1) || ' de ' ||
                   DECODE(P.FECNACIMIENTO,
                          NULL,
                          '',
                          TO_CHAR(P.FECNACIMIENTO, 'YYYY'))) DESEDAD,
             UPPER(P.PRINOMBRE || ' ' || P.APEPATERNO || ' ' ||
                   P.APEMATERNO) NOMBRECOMPLETO,
             p.idepostulante,
             (((SELECT G.DESCRIPCION
                  FROM DETALLE_GENERAL G
                 WHERE G.IDEGENERAL = 9
                   AND G.VALOR = P.TIPVIA) || ' ' || P.NOMVIA || '' ||
             DECODE(NVL(P.NUMDIRECCION, 0), 0, '', ' ,' || P.NUMDIRECCION) || '' ||
             DECODE((SELECT G.DESCRIPCION
                        FROM DETALLE_GENERAL G
                       WHERE G.IDEGENERAL = 10
                         AND G.VALOR = P.TIPZONA),
                      NULL,
                      '',
                      ' ,' || (SELECT G.DESCRIPCION
                                 FROM DETALLE_GENERAL G
                                WHERE G.IDEGENERAL = 10
                                  AND G.VALOR = P.TIPZONA)) || '' ||
             DECODE(P.NOMZONA, NULL, '', ' ,' || P.NOMZONA) || '' ||
             DECODE(P.MANZANA, NULL, '', ' ,' || P.MANZANA) || '' ||
             DECODE(P.LOTE, NULL, '', ' ,' || P.LOTE) || '' ||
             DECODE(P.BLOQUE, NULL, '', ' ,' || P.BLOQUE) || '' ||
             DECODE(P.ETAPA, NULL, '', ' ,' || P.ETAPA) || '' ||
             DECODE(NVL(P.INTERIOR, 0), 0, '', ' ,' || P.INTERIOR)) || ', ' ||
             PR_INTRANET_ED.FN_OBTIENE_DESUBIGEO(P.IDEUBIGEO, 1) || ', ' ||
             PR_INTRANET_ED.FN_OBTIENE_DESUBIGEO(P.IDEUBIGEO, 3) ||
             ', PERU') DESDIR,
             
             decode(to_char(P.fecmodificacion, 'DD/MM/YYYY'),
                    '01/01/0001',
                    (upper(DECODE(P.Feccreacion,
                                  NULL,
                                  '',
                                  TO_CHAR(P.Feccreacion, 'DD')) || ' de ' ||
                           PR_INTRANET_ED.FN_DES_MES(DECODE(P.Feccreacion,
                                                            NULL,
                                                            0,
                                                            TO_CHAR(P.Feccreacion,
                                                                    'MM')),
                                                     1) || ' de ' ||
                           DECODE(P.Feccreacion,
                                  NULL,
                                  '',
                                  TO_CHAR(P.Feccreacion, 'YYYY')))),
                    
                    (upper(DECODE(P.fecmodificacion,
                                  NULL,
                                  '',
                                  TO_CHAR(P.fecmodificacion, 'DD')) ||
                           ' de ' || PR_INTRANET_ED.FN_DES_MES(DECODE(P.fecmodificacion,
                                                                      NULL,
                                                                      0,
                                                                      TO_CHAR(P.fecmodificacion,
                                                                              'MM')),
                                                               1) || ' de ' ||
                           DECODE(P.fecmodificacion,
                                  NULL,
                                  '',
                                  TO_CHAR(P.fecmodificacion, 'YYYY'))))) MODIFICACION
      
        FROM POSTULANTE P
       WHERE P.IDEPOSTULANTE = p_nIdPostulante
         AND P.ESTACTIVO = 'A';
  
  END SP_CV_POSTULANTE;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_NIVEL_ACADEMICO
  Proposito   : obtiene el nivel academico para el reporte
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_NIVEL_ACADEMICO(p_nIdPostulante IN postulante.idepostulante%type,
                                  p_Rpta          OUT cur_cursor)
  
   IS
  BEGIN
  
    OPEN p_Rpta FOR
      SELECT E.IDEESTUDIOSPOSTULANTE,
             (decode(E.TIPNOMINSTITUCION,
                     'XX',
                     E.NOMINSTITUCION,
                     (SELECT DG.DESCRIPCION
                        FROM DETALLE_GENERAL DG
                       WHERE DG.IDEGENERAL = 17
                         AND DG.VALOR = E.TIPNOMINSTITUCION))) INSTITUCION,
             (SELECT DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               WHERE DG.IDEGENERAL = 19
                 AND DG.VALOR = E.TIPAREA) AREAESTUDIO,
             (SELECT DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               WHERE DG.IDEGENERAL = 20
                 AND DG.VALOR = E.TIPEDUCACION) NIVELESTUDIO,
             (SELECT DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               WHERE DG.IDEGENERAL = 20
                 AND DG.VALOR = E.TIPNIVELALCANZADO) NIVELALCANZADO,
             FECESTUDIOINICIO DESDE,
             FECESTUDIOFIN HASTA,
             (PR_INTRANET_ED.FN_DES_MES(TO_CHAR(e.FECESTUDIOINICIO, 'MM'),
                                        2) || ' ' ||
             TO_CHAR(e.FECESTUDIOINICIO, 'YYYY')) || ' - ' ||
             decode(e.indestactualmente,
                    'S',
                    'Actualmente',
                    (PR_INTRANET_ED.FN_DES_MES(decode(e.indestactualmente,
                                                      'S',
                                                      TO_CHAR(SYSDATE, 'MM'),
                                                      TO_CHAR(e.FECESTUDIOFIN,
                                                              'MM')),
                                               2) || ' ' ||
                    decode(e.indestactualmente,
                            'S',
                            TO_CHAR(SYSDATE, 'YYYY'),
                            TO_CHAR(e.FECESTUDIOFIN, 'YYYY')))) FECESTUDIO
      
        FROM ESTUDIOS_POSTULANTE E
       WHERE IDEPOSTULANTE = p_nIdPostulante;
  
  END SP_CV_NIVEL_ACADEMICO;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_EXPERIENCIA
  Proposito   : obtiene las experiencias para el reporte
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_EXPERIENCIA(p_nIdPostulante IN postulante.idepostulante%type,
                              p_Rpta          OUT cur_cursor)
  
   IS
  BEGIN
  
    OPEN p_Rpta FOR
      SELECT
      
       EX.IDEEXPPOSTULANTE,
       EX.NOMEMPRESA,
       EX.TIEMPOSERVICIO,
       (decode(trim(EX.TIPCARGOTRABAJO),
               'XX',
               EX.NOMCARGOTRABAJO,
               (select DG.DESCRIPCION
                  FROM DETALLE_GENERAL DG
                 WHERE DG.IDEGENERAL = 24
                   AND DG.VALOR = EX.TIPCARGOTRABAJO))) CARGO,
       (PR_INTRANET_ED.FN_DES_MES(TO_CHAR(ex.fectrabinicio, 'MM'), 2) || ' ' ||
       TO_CHAR(ex.fectrabinicio, 'YYYY')) || ' - ' ||
       decode(ex.indtrabactualmente,
              'S',
              'Actualmente',
              (PR_INTRANET_ED.FN_DES_MES(decode(ex.indtrabactualmente,
                                                'S',
                                                TO_CHAR(SYSDATE, 'MM'),
                                                TO_CHAR(ex.fectrabfin, 'MM')),
                                         2) || ' ' ||
              decode(ex.indtrabactualmente,
                      'S',
                      TO_CHAR(SYSDATE, 'YYYY'),
                      TO_CHAR(ex.fectrabfin, 'YYYY')))) FECTRABAJO,
       
       EX.FUNCIONESDESEMP FUCNIONES,
       (select DG.DESCRIPCION
          FROM DETALLE_GENERAL DG
         where DG.IDEGENERAL = 25
           AND DG.VALOR = EX.TIPMOTIVOCESE) MOTIVOCESE,
       EX.NOMREFERENTE,
       TO_CHAR(EX.NUMTELEFONOFIJOINST) FONOINST,
       EX.NUMANEXOINST ANEXOINST,
       (select DG.DESCRIPCION
          FROM DETALLE_GENERAL DG
         where DG.IDEGENERAL = 24
           AND DG.VALOR = EX.TIPCARGOTRABAJOREF) CARGOREFERENTE,
       TO_CHAR(EX.NUMTELEFMOVILREF) FONOREFERENTE,
       EX.CORREOREFERENTE
      
        FROM EXP_POSTULANTE EX
       WHERE EX.IDEPOSTULANTE = p_nIdPostulante;
  
  END SP_CV_EXPERIENCIA;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_CONOFIMATICA
  Proposito   : obtiene los conocimientos de ofimatica
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_CONOFIMATICA(p_nIdPostulante IN postulante.idepostulante%type,
                               p_Rpta          OUT cur_cursor)
  
   IS
  BEGIN
  
    OPEN p_Rpta FOR
    
      SELECT IDECONOGENPOSTULANTE,
             TIPO,
             DESCRIPCION,
             TIPNIVELCONOCIMIENTO,
             INDCERTIFICACION
        FROM (SELECT C.IDECONOGENPOSTULANTE,
                     (select DG.DESCRIPCION
                        FROM DETALLE_GENERAL DG
                       where DG.IDEGENERAL = 27
                         AND DG.VALOR = TIPCONOFIMATICA) TIPO,
                     (select DG.DESCRIPCION
                        FROM DETALLE_GENERAL DG
                       where DG.IDEGENERAL = 28
                         AND DG.VALOR = TIPNOMOFIMATICA) DESCRIPCION,
                     (select DG.DESCRIPCION
                        FROM DETALLE_GENERAL DG
                       where DG.IDEGENERAL = 29
                         AND DG.VALOR = TIPNIVELCONOCIMIENTO) TIPNIVELCONOCIMIENTO,
                     C.INDCERTIFICACION
                FROM CONOGEN_POSTULANTE C
               WHERE C.IDEPOSTULANTE = p_nIdPostulante
                 AND C.Tipconofimatica IS NOT NULL
              UNION ALL
              SELECT C.IDECONOGENPOSTULANTE,
                     (select DG.DESCRIPCION
                        FROM DETALLE_GENERAL DG
                       where DG.IDEGENERAL = 32
                         AND DG.VALOR = TIPCONOCGENERALES) TIPO,
                     (select CASE
                               WHEN TIPNOMCONOCGRALES = 'XX' THEN
                                NOMCONOCGRALES
                               ELSE
                                DG.DESCRIPCION
                             END
                        FROM DETALLE_GENERAL DG
                       where DG.IDEGENERAL = 32
                         AND DG.VALOR = TIPNOMCONOCGRALES) DESCRIPCION,
                     (select DG.DESCRIPCION
                        FROM DETALLE_GENERAL DG
                       where DG.IDEGENERAL = 29
                         AND DG.VALOR = TIPNIVELCONOCIMIENTO) TIPNIVELCONOCIMIENTO,
                     C.INDCERTIFICACION
                FROM CONOGEN_POSTULANTE C
               WHERE C.IDEPOSTULANTE = p_nIdPostulante
                 AND C.TIPCONOCGENERALES IS NOT NULL) X
       ORDER BY X.IDECONOGENPOSTULANTE;
  
  END SP_CV_CONOFIMATICA;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_CONIDIOMA
  Proposito   : obtiene los idiomas para el reporte
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_CONIDIOMA(p_nIdPostulante IN postulante.idepostulante%type,
                            p_Rpta          OUT cur_cursor)
  
   IS
  BEGIN
  
    OPEN p_Rpta FOR
      SELECT C.IDECONOGENPOSTULANTE,
             (select DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               where DG.IDEGENERAL = 30
                 AND DG.VALOR = TIPIDIOMA) TIPIDIOMA,
             (select DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               where DG.IDEGENERAL = 31
                 AND DG.VALOR = TIPCONOCIDIOMA) TIPCONOCIDIOMA,
             (select DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               where DG.IDEGENERAL = 29
                 AND DG.VALOR = TIPNIVELCONOCIMIENTO) TIPNIVELCONOCIMIENTO,
             c.INDCERTIFICACION
        FROM CONOGEN_POSTULANTE C
       WHERE C.IDEPOSTULANTE = p_nIdPostulante
         AND C.TIPIDIOMA IS NOT NULL;
  
  END SP_CV_CONIDIOMA;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_OTROSCONOCIMIENTOS
  Proposito   :obtiene los conocmientos para el reporte
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_OTROSCONOCIMIENTOS(p_nIdPostulante IN postulante.idepostulante%type,
                                     p_Rpta          OUT cur_cursor)
  
   IS
  BEGIN
  
    OPEN p_Rpta FOR
      SELECT C.IDECONOGENPOSTULANTE,
             C.TIPCONOCGENERALES,
             C.TIPNOMCONOCGRALES,
             C.TIPNIVELCONOCIMIENTO,
             C.INDCERTIFICACION
        FROM CONOGEN_POSTULANTE C
       WHERE C.IDEPOSTULANTE = p_nIdPostulante
         AND C.TIPCONOCGENERALES IS NOT NULL;
  
  END SP_CV_OTROSCONOCIMIENTOS;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_PARIENTES
  Proposito   : obtiene los parientes para el reporte
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_PARIENTES(p_nIdPostulante IN postulante.idepostulante%type,
                            p_Rpta          OUT cur_cursor)
  
   IS
  BEGIN
  
    OPEN p_Rpta FOR
      SELECT PP.APEPATERNO,
             PP.APEMATERNO,
             PP.NOMBRES,
             (SELECT DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               WHERE DG.IDEGENERAL = 36
                 AND DG.VALOR = PP.TIPVINCULO) VINCULO,
             PP.FECNACIMIENTO
        FROM PARIENTES_POSTULANTE PP
       WHERE PP.IDEPOSTULANTE = p_nIdPostulante;
  
  END SP_CV_PARIENTES;

  /* ------------------------------------------------------------
  Nombre      : SP_CV_DISCAPACIDAD
  Proposito   : obtiene las discapacidades para el reporte
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_CV_DISCAPACIDAD(p_nIdPostulante IN postulante.idepostulante%type,
                               p_Rpta          OUT cur_cursor)
  
   IS
  BEGIN
  
    OPEN p_Rpta FOR
      SELECT (SELECT DG.DESCRIPCION
                FROM DETALLE_GENERAL DG
               WHERE DG.IDEGENERAL = 37
                 AND DG.VALOR = D.TIPODISCAPACIDAD) TIPDISCACIDAD,
             D.DESDISCAPACIDAD
        FROM DISCAPACIDAD_POSTULANTE D
       WHERE D.IDEPOSTULANTE = p_nIdPostulante;
  
  END SP_CV_DISCAPACIDAD;

  /* ------------------------------------------------------------
  Nombre      : FN_DES_MES
  Proposito   : Obtiene la descripcion mes
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :    1 completo
                   2 abreviado
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_DES_MES(p_nMes NUMBER, p_nInFormato NUMBER) RETURN VARCHAR2 IS
    p_cDesMes varchar2(1000);
  BEGIN
  
    BEGIN
    
      p_cDesMes := NULL;
    
      IF p_nMes = 1 THEN
        p_cDesMes := 'enero';
      END IF;
      IF p_nMes = 2 THEN
        p_cDesMes := 'febrero';
      END IF;
      IF p_nMes = 3 THEN
        p_cDesMes := 'marzo';
      END IF;
      IF p_nMes = 4 THEN
        p_cDesMes := 'abril';
      END IF;
      IF p_nMes = 5 THEN
        p_cDesMes := 'mayo';
      END IF;
      IF p_nMes = 6 THEN
        p_cDesMes := 'junio';
      END IF;
      IF p_nMes = 7 THEN
        p_cDesMes := 'julio';
      END IF;
      IF p_nMes = 8 THEN
        p_cDesMes := 'agosto';
      END IF;
      IF p_nMes = 9 THEN
        p_cDesMes := 'setiembre';
      END IF;
      IF p_nMes = 10 THEN
        p_cDesMes := 'octubre';
      END IF;
      IF p_nMes = 11 THEN
        p_cDesMes := 'noviembre';
      END IF;
      IF p_nMes = 12 THEN
        p_cDesMes := 'diciembre';
      END IF;
    
      IF p_nInFormato = 2 THEN
        p_cDesMes := SUBSTR(p_cDesMes, 0, 3);
      END IF;
    
      RETURN NVL(p_cDesMes, '');
    
    END;
  
  END FN_DES_MES;

  /* ------------------------------------------------------------
  Nombre      : FN_OBTIENE_CRITERIOS
  Proposito   : obtiene la lista de criterio que no se utilizan en otraa subcategorias
                proseco en cola total/total y proceso nuevo sin cola
                (total-1)/total
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE FN_OBTIENE_CRITERIOS(p_cTipMedicion IN Criterio.Tipmedicion%type,
                                 p_cPregunta    IN Criterio.Pregunta%type,
                                 p_cTipCriterio IN Criterio.Tipcriterio%type,
                                 p_cEstado      IN Criterio.Estactivo%type,
                                 p_cTipoModo    IN Criterio.Tipmodo%TYPE,
                                 p_nIdSede      IN Criterio.Idesede%type,
                                 p_cRpta        OUT cur_cursor) IS
  
    nIdSede number;
  BEGIN
  
    IF p_nIdSede IS NULL THEN
    
      nIdSede := 0;
    ELSE
      nIdSede := p_nIdSede;
    END IF;
  
    OPEN p_cRpta FOR
      SELECT IDECRITERIO,
             TIPMEDICION,
             TIPCRITERIO,
             TIPMODO,
             PREGUNTA,
             TIPCALIFICACION,
             ORDENIMPRESION,
             ESTACTIVO,
             USRCREACION,
             FECCREACION,
             USRMODIFICACION,
             FECMODIFICACION,
             IMAGENCRIT,
             NOMIMAGEN,
             PR_INTRANET.SP_LISTA_LVAL('2', C.TIPMEDICION) DESTIPMEDICION,
             PR_INTRANET.SP_LISTA_LVAL('1', C.TIPCRITERIO) DESTIPCRITERIO,
             PR_INTRANET.SP_LISTA_LVAL('4', C.TIPMODO) DESTIPMODO,
             PR_INTRANET.SP_LISTA_LVAL('5', C.TIPCALIFICACION) DESTIPCALIFICACION
        FROM CRITERIO C
       WHERE NOT EXISTS
       (SELECT D.IDECRITERIO
                FROM CRITERIO_X_SUBCATEGORIA D
               WHERE D.IDECRITERIO = C.IDECRITERIO)
         AND (p_cTipMedicion IS NULL OR c.tipmedicion = p_cTipMedicion)
         AND (p_cPregunta IS NULL OR
             UPPER(c.pregunta) LIKE '%' || UPPER(p_cPregunta) || '%')
         AND (p_cTipCriterio IS NULL OR c.tipcriterio = p_cTipCriterio)
         AND (p_cEstado IS NULL OR c.estactivo = p_cEstado)
         AND (p_cTipoModo IS NULL OR c.Tipmodo = p_cTipoModo)
         AND (nIdSede = 0 OR c.idesede = nIdSede)
            
         AND C.ESTACTIVO = 'A'
       ORDER BY C.IDECRITERIO, C.ESTACTIVO;
  
  END FN_OBTIENE_CRITERIOS;

  /* ------------------------------------------------------------
  Nombre      : FN_OBTIENE_OPCIONES
  Proposito   : obtiene las opciones del menu
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE FN_OBTIENE_OPCIONES(p_cDesOpcion   IN VARCHAR2,
                                p_cDescripcion IN VARCHAR2,
                                p_cEstado      IN VARCHAR2,
                                p_cTipMenu     IN VARCHAR2,
                                p_cRpta        OUT cur_cursor) IS
  
  BEGIN
  
    OPEN p_cRpta FOR
      select FLGHABILITADO,
             IDOPCIONPADRE,
             IDOPCION,
             DSCOPCION,
             DESCRIPCION,
             TIPMENU,
             DECODE(TIPMENU, 'I', 'INTRANET', 'E', 'EXTRANET', '') DESMENU,
             op.IDITEM
        from opciones op
       where (p_cDesOpcion IS NULL OR
             UPPER(op.dscopcion) LIKE '%' || UPPER(p_cDesOpcion) || '%')
         and (p_cDescripcion IS NULL OR UPPER(op.descripcion) LIKE
             '%' || UPPER(p_cDescripcion) || '%')
         AND (p_cEstado IS NULL OR op.flghabilitado = p_cEstado)
         and (p_cTipMenu IS NULL OR op.tipmenu = p_cTipMenu)
         and op.idopcion is not null
         AND OP.FLGHABILITADO = 'A';
  
  END FN_OBTIENE_OPCIONES;

  /* ------------------------------------------------------------
  Nombre      : FN_OBTIENE_CARGOS_PUBLICADOS
  Proposito   : obtiene las opciones del menu
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_OBTIENE_CARGOS_PUBLICADOS(p_nIdSede IN NUMBER,
                                         p_cRpta   OUT cur_cursor) IS
  
  BEGIN
  
    OPEN p_cRpta FOR
      
      SELECT X.IDECARGO, UPPER(NVL(X.NOMCARGO, '')) NOMCARGO
        FROM (SELECT NVL(S.IDECARGO, 0) IDECARGO, S.NOMCARGO NOMCARGO
                FROM SOLREQ_PERSONAL S
               WHERE 1 = 1
                 AND S.TIPSOL IN ('03', '02')
                 AND S.FECPUBLICACION IS NOT NULL
                 AND S.TIPETAPA = '04'
                 AND S.NOMCARGO IS NOT NULL
                 AND S.TIPPUESTO IS NOT NULL
                 AND S.ESTACTIVO = 'A'
                 AND S.FECEXPIRACACION IS NOT NULL
                 AND (p_nIdSede = 0 OR S.IDESEDE = p_nIdSede)
              UNION ALL
              SELECT NVL(N.IDECARGO, 0) IDECARGO, N.NOMBRE NOMCARGO
                FROM SOLNUEVO_CARGO N
               WHERE 1 = 1
                 AND N.FECPUBLICACION IS NOT NULL
                 AND N.FECEXPIRACION IS NOT NULL
                 AND N.NOMBRE IS NOT NULL
                 AND N.ESTACTIVO = 'A'
                 AND N.TIPETAPA = '04'
                 AND (p_nIdSede = 0 OR N.IDESEDE = p_nIdSede)) X
       where X.IDECARGO > 0
       group by X.IDECARGO, X.NOMCARGO
       order by X.NOMCARGO;
  
  END SP_OBTIENE_CARGOS_PUBLICADOS;

  /* ------------------------------------------------------------
  Nombre      : FN_OBTIENE_TIEMPO_TOTAL
  Proposito   : obtiene el tiempo total de los examenes
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  FUNCTION FN_OBTIENE_TIEMPO_TOTAL(p_nIdExamen EXAMEN.IDEEXAMEN%TYPE)
    RETURN NUMBER IS
    nTotal number;
  BEGIN
  
    BEGIN
      SELECT SUM(X.TIEMPO) TIEMPOTOTAL
        INTO nTotal
        FROM (SELECT EC.IDECATEGORIA,
                     NVL((SELECT C.TIEMPO
                           FROM CATEGORIA C
                          WHERE C.IDECATEGORIA = EC.IDECATEGORIA
                            AND C.ESTACTIVO = 'A'),
                         0) TIEMPO
                FROM EXAMEN_X_CATEGORIA EC
               WHERE EC.IDEEXAMEN = p_nIdExamen
                 AND EC.ESTACTIVO = 'A'
               GROUP BY EC.IDECATEGORIA) X;
    EXCEPTION
      WHEN OTHERS THEN
        nTotal := 0;
    END;
  
    RETURN nTotal;
  
  END FN_OBTIENE_TIEMPO_TOTAL;

  /* ------------------------------------------------------------
  Nombre      : SP_OBTIENE_ANALISTA_RESP
  Proposito   : obtiene el analista reponsable
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_ANALISTA_RESP(p_nIdSede IN SEDE.IDESEDE%TYPE,
                                     p_cRpta   OUT cur_cursor) IS
  
  BEGIN
  
    OPEN p_cRpta FOR
      SELECT DISTINCT Z.IDUSUARIO,
                      (SELECT CODUSUARIO
                         FROM USUARIO U
                        WHERE U.IDUSUARIO = Z.IDUSUARIO
                          AND U.TIPUSUARIO = 'I') NOMBRE
        FROM USUAROLSEDE Z, USUARIOREQ X
       WHERE Z.IDUSUARIO = X.IDUSUARIO
         AND (p_nIdSede IS NULL OR Z.IDESEDE = p_nIdSede)
         AND Z.IDROL IN (8, 9)
       ORDER BY Z.IDUSUARIO;
  
  END SP_OBTIENE_ANALISTA_RESP;

  /* ------------------------------------------------------------
  NOMBRE      : SP_OBTIENE_REPORTE_SELECCION
  Proposito   : obtiene el analista reponsable
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_OBTIENE_REPORTE_SELECCION(p_cFecDesde       IN varchar2,
                                         p_cFecHasta       IN varchar2,
                                         p_cTipSol         IN varchar2,
                                         p_cEstadpReq      IN varchar2,
                                         p_nIdResponsable  IN NUMBER,
                                         p_nIdDependencia  IN NUMBER,
                                         p_nIdDepartamento IN NUMBER,
                                         p_nIdArea         IN NUMBER,
                                         p_cMotivoReemp    IN NUMBER,
                                         p_cSede           IN NUMBER,
                                         p_cRpta           OUT cur_cursor) IS
  
    nIdResponsable  number(8);
    nIdDependencia  number(8);
    nIdDepartamento number(8);
    nIdArea         number(8);
    nMotivoReemp    number(8);
    nSede           number(8);
    nContVacantes   number(8);
  
  BEGIN
  
    delete from TEMP_REP_SELECCION;
  
    IF p_cSede IS NULL THEN
      nSede := 0;
    ELSE
      nSede := p_cSede;
    END IF;
  
    IF p_nIdResponsable IS NULL THEN
      nIdResponsable := 0;
    ELSE
      nIdResponsable := p_nIdResponsable;
    END IF;
  
    IF p_nIdDependencia IS NULL THEN
      nIdDependencia := 0;
    ELSE
      nIdDependencia := p_nIdDependencia;
    END IF;
  
    IF p_nIdDepartamento IS NULL THEN
      nIdDepartamento := 0;
    ELSE
      nIdDepartamento := p_nIdDepartamento;
    END IF;
    IF p_nIdArea IS NULL THEN
      nIdArea := 0;
    ELSE
      nIdArea := p_nIdArea;
    END IF;
  
    IF p_cMotivoReemp IS NULL THEN
      nMotivoReemp := 0;
    ELSE
      nMotivoReemp := p_cMotivoReemp;
    END IF;
  
    FOR C1 IN (
               
               SELECT X.IDSOL,
                       X.JEFE,
                       ROUND(X.DIAS) DIAS,
                       DECODE(X.TIPSOL,
                              '01',
                              'NUEVO',
                              '02',
                              'AMPLIACION',
                              '03',
                              'REEMPLAZO',
                              '') DESTIPSOL,
                       (SELECT D.DESCRIPCION
                          FROM DETALLE_GENERAL D
                         WHERE D.IDEGENERAL = 50
                           AND D.VALOR = X.TIPETAPA) ESTADO_PROCESO,
                       TO_CHAR(X.FECCREACION, 'DD/MM/YYYY') FECHA_REQUERIMIENTO,
                       (SELECT S.DESCRIPCION
                          FROM SEDE S
                         WHERE S.IDESEDE = X.IDESEDE) SEDE,
                       (SELECT DP.NOMDEPENDENCIA
                          FROM DEPENDENCIA DP
                         WHERE DP.IDEDEPENDENCIA = X.IDEDEPENDENCIA
                           AND DP.IDESEDE = X.IDESEDE) DESDEPENDENCIA,
                       (SELECT DA.NOMDEPARTAMENTO
                          FROM DEPARTAMENTO DA
                         WHERE DA.IDEDEPARTAMENTO = X.IDEDEPARTAMENTO
                           AND X.IDEDEPENDENCIA = DA.IDEDEPENDENCIA) DESDEPARTAMENTO,
                       (SELECT A.NOMAREA
                          FROM AREA A
                         WHERE A.IDEAREA = X.IDEAREA
                           AND A.IDEDEPARTAMENTO = X.IDEDEPARTAMENTO) DESAREA,
                       (SELECT CA.NOMCARGO
                          FROM CARGO CA
                         WHERE CA.IDECARGO = X.IDCARGO
                           AND ROWNUM = 1) CARGO,
                       X.IDCARGO,
                       X.NOMBRE_CARGO,
                       X.IDESEDE,
                       X.IDEDEPENDENCIA,
                       X.IDEDEPARTAMENTO,
                       X.IDEAREA,
                       X.FECPUBLICACION,
                       X.FECEXPIRACACION,
                       X.TIPVACANTE,
                       X.NUMVACANTES,
                       X.MOTIVO,
                       X.ESTACTIVO,
                       X.TIPSOL,
                       X.TIPETAPA,
                       TO_CHAR(X.FECCREACION, 'DD/MM/YYYY') FECCREACION,
                       nvl(X.MOTIVOCIERRE, '') MOTIVOCIERRE,
                       FECSUCESO,
                       USRSUCESO,
                       X.TIPPUESTO,
                       (SELECT U.DSCAPEPATERNO || ' ' || U.DSCAPEMATERNO || ' ' ||
                               U.DSCNOMBRES
                          FROM USUARIO U
                         WHERE U.IDUSUARIO = X.USRSUCESO
                           AND U.TIPUSUARIO = 'I') ANALISTA_RESP,
                       '' OBSPSICOLOGO,
                       '' OBSENTREVISTA,
                       '' FONO,
                       '' P_INGRESA,
                       '' REEMPLAZA_A,
                       '' FECREEMPLAZO,
                       DECODE(X.TIPSOL,
                              '02',
                              '',
                              '03',
                              (select d.descripcion
                                 from detalle_general d
                                where d.idegeneral = 48
                                  and d.valor = X.tipvacante)) MOTIVOREEMPLAZO,
                       '' NUMDOCUMENTO
               
                 FROM (SELECT (SELECT US.DSCAPEPATERNO || ' ' ||
                                       US.DSCAPEMATERNO || ' ' || US.DSCNOMBRES
                                  FROM USUARIO US
                                 WHERE US.IDUSUARIO =
                                       (SELECT LP.USRESPONSABLE
                                          FROM LOGSOLREQ_PERSONAL LP
                                         WHERE LP.IDESOLREQPERSONAL =
                                               SP.IDESOLREQPERSONAL
                                           AND LP.FECSUCESO =
                                               (SELECT MIN(LP1.FECSUCESO)
                                                  FROM LOGSOLREQ_PERSONAL LP1
                                                 WHERE LP1.IDESOLREQPERSONAL =
                                                       LP.IDESOLREQPERSONAL))
                                   AND US.TIPUSUARIO = 'I') JEFE,
                               ROUND(DECODE(SP.TIPETAPA,
                                            '04',
                                            (SYSDATE -
                                            (SELECT LO1.FECSUCESO
                                                FROM LOGSOLREQ_PERSONAL LO1
                                               WHERE LO1.IDESOLREQPERSONAL =
                                                     SP.IDESOLREQPERSONAL
                                                 AND LO1.FECSUCESO =
                                                     (SELECT MIN(LG1.FECSUCESO)
                                                        FROM LOGSOLREQ_PERSONAL LG1
                                                       WHERE LG1.IDESOLREQPERSONAL =
                                                             LO1.IDESOLREQPERSONAL))),
                                            '08',
                                            ((SELECT LO2.FECSUCESO
                                                FROM LOGSOLREQ_PERSONAL LO2
                                               WHERE LO2.IDESOLREQPERSONAL =
                                                     SP.IDESOLREQPERSONAL
                                                 AND LO2.TIPETAPA = '08'
                                                 AND LO2.FECSUCESO =
                                                     (SELECT MAX(LO5.FECSUCESO)
                                                        FROM LOGSOLREQ_PERSONAL LO5
                                                       WHERE LO5.IDESOLREQPERSONAL =
                                                             LO2.IDESOLREQPERSONAL
                                                         AND LO5.TIPETAPA = '08')) -
                                            (SELECT LO3.FECSUCESO
                                                FROM LOGSOLREQ_PERSONAL LO3
                                               WHERE LO3.IDESOLREQPERSONAL =
                                                     SP.IDESOLREQPERSONAL
                                                 AND LO3.TIPETAPA = '01'
                                                 AND LO3.FECSUCESO =
                                                     (SELECT MAX(LO4.FECSUCESO)
                                                        FROM LOGSOLREQ_PERSONAL LO4
                                                       WHERE LO4.IDESOLREQPERSONAL =
                                                             LO3.IDESOLREQPERSONAL
                                                         AND LO4.FECSUCESO =
                                                             (SELECT MIN(LG2.FECSUCESO)
                                                                FROM LOGSOLREQ_PERSONAL LG2
                                                               WHERE LG2.IDESOLREQPERSONAL =
                                                                     LO4.IDESOLREQPERSONAL)))))) DIAS,
                               
                               SP.IDESOLREQPERSONAL IDSOL,
                               SP.IDECARGO IDCARGO,
                               SP.NOMCARGO NOMBRE_CARGO,
                               SP.IDESEDE,
                               SP.IDEDEPENDENCIA,
                               SP.IDEDEPARTAMENTO,
                               SP.IDEAREA,
                               SP.FECPUBLICACION,
                               SP.FECEXPIRACACION,
                               SP.TIPVACANTE,
                               SP.NUMVACANTES NUMVACANTES,
                               SP.MOTIVO,
                               SP.ESTACTIVO,
                               SP.TIPSOL,
                               SP.TIPETAPA,
                               SP.MOTIVOCIERRE,
                               
                               (SELECT MAX(LSP1.FECSUCESO)
                                  FROM LOGSOLREQ_PERSONAL LSP1
                                 WHERE LSP1.IDESOLREQPERSONAL =
                                       SP.IDESOLREQPERSONAL
                                   AND LSP1.TIPETAPA = '04') FECCREACION,
                               
                               TO_CHAR((SELECT MAX(LSP1.FECSUCESO)
                                         FROM LOGSOLREQ_PERSONAL LSP1
                                        WHERE LSP1.IDESOLREQPERSONAL =
                                              SP.IDESOLREQPERSONAL),
                                       'DD/MM/YYYY') FECSUCESO,
                               
                               (SELECT LSP01.USRSUCESO
                                  FROM LOGSOLREQ_PERSONAL LSP01
                                 WHERE LSP01.FECSUCESO =
                                       (SELECT MAX(LSP1.FECSUCESO)
                                          FROM LOGSOLREQ_PERSONAL LSP1
                                         WHERE LSP1.IDESOLREQPERSONAL =
                                               SP.IDESOLREQPERSONAL)) USRSUCESO,
                               SP.TIPPUESTO
                        
                          FROM SOLREQ_PERSONAL SP
                         WHERE SP.TIPETAPA IN ('04', '08')
                           AND SP.ESTACTIVO = 'A'
                        UNION ALL
                        SELECT (SELECT US.DSCAPEPATERNO || ' ' ||
                                       US.DSCAPEMATERNO || ' ' || US.DSCNOMBRES
                                  FROM USUARIO US
                                 WHERE US.IDUSUARIO =
                                       (SELECT LP3.USRESPONSABLE
                                          FROM Logsolnuevo_Cargo LP3
                                         WHERE LP3.IDESOLNUEVOCARGO =
                                               SC.IDESOLNUEVOCARGO
                                           AND LP3.FECSUCESO =
                                               (SELECT MIN(LP2.FECSUCESO)
                                                  FROM Logsolnuevo_Cargo LP2
                                                 WHERE LP2.IDESOLNUEVOCARGO =
                                                       LP3.IDESOLNUEVOCARGO)
                                           AND US.TIPUSUARIO = 'I')) JEFE,
                               ROUND(DECODE(SC.TIPETAPA,
                                            '04',
                                            (SYSDATE -
                                            (SELECT LO1.FECSUCESO
                                                FROM LOGSOLNUEVO_CARGO LO1
                                               WHERE LO1.IDESOLNUEVOCARGO =
                                                     SC.IDESOLNUEVOCARGO
                                                 AND LO1.FECSUCESO =
                                                     (SELECT MIN(LG3.FECSUCESO)
                                                        FROM LOGSOLNUEVO_CARGO LG3
                                                       WHERE LG3.IDESOLNUEVOCARGO =
                                                             LO1.IDESOLNUEVOCARGO))),
                                            '08',
                                            ((SELECT LO2.FECSUCESO
                                                FROM LOGSOLNUEVO_CARGO LO2
                                               WHERE LO2.IDESOLNUEVOCARGO =
                                                     SC.IDESOLNUEVOCARGO
                                                 AND LO2.TIPETAPA = '08'
                                                 AND LO2.FECSUCESO =
                                                     (SELECT MAX(LO5.FECSUCESO)
                                                        FROM LOGSOLNUEVO_CARGO LO5
                                                       WHERE LO5.IDESOLNUEVOCARGO =
                                                             LO2.IDESOLNUEVOCARGO
                                                         AND LO5.TIPETAPA = '08')) -
                                            (SELECT LO3.FECSUCESO
                                                FROM LOGSOLNUEVO_CARGO LO3
                                               WHERE LO3.IDESOLNUEVOCARGO =
                                                     SC.IDESOLNUEVOCARGO
                                                 AND LO3.TIPETAPA = '01'
                                                 AND LO3.FECSUCESO =
                                                     (SELECT MAX(LO4.FECSUCESO)
                                                        FROM LOGSOLNUEVO_CARGO LO4
                                                       WHERE LO4.IDESOLNUEVOCARGO =
                                                             LO3.IDESOLNUEVOCARGO
                                                         AND LO4.FECSUCESO =
                                                             (SELECT MIN(LG4.FECSUCESO)
                                                                FROM LOGSOLNUEVO_CARGO LG4
                                                               WHERE LG4.IDESOLNUEVOCARGO =
                                                                     LO4.IDESOLNUEVOCARGO)))))) DIAS,
                               SC.IDESOLNUEVOCARGO IDSOL,
                               SC.IDECARGO IDCARGO,
                               SC.NOMBRE NOMBRE_CARGO,
                               SC.IDESEDE,
                               SC.IDEDEPENDENCIA,
                               SC.IDEDEPARTAMENTO,
                               SC.IDEAREA,
                               SC.FECPUBLICACION,
                               SC.FECEXPIRACION,
                               '9999999' TIPVACANTE,
                               SC.NUMPOSICIONES NUMVACANTES,
                               SC.MOTIVO,
                               SC.ESTACTIVO,
                               '01' TIPSOL,
                               SC.TIPETAPA,
                               SC.MOTIVOCIERRE,
                               (SELECT MAX(LNC1.FECSUCESO)
                                  FROM LOGSOLNUEVO_CARGO LNC1
                                 WHERE LNC1.IDESOLNUEVOCARGO =
                                       SC.IDESOLNUEVOCARGO
                                   AND LNC1.TIPETAPA = '04') FECCREACION,
                               TO_CHAR((SELECT MAX(LNC1.FECSUCESO)
                                         FROM LOGSOLNUEVO_CARGO LNC1
                                        WHERE LNC1.IDESOLNUEVOCARGO =
                                              SC.IDESOLNUEVOCARGO),
                                       'DD/MM/YYYY') FECSUCESO,
                               
                               (SELECT LC1.USRSUCESO
                                  FROM LOGSOLNUEVO_CARGO LC1
                                 WHERE LC1.FECSUCESO =
                                       (SELECT MAX(LNC1.FECSUCESO)
                                          FROM LOGSOLNUEVO_CARGO LNC1
                                         WHERE LNC1.IDESOLNUEVOCARGO =
                                               SC.IDESOLNUEVOCARGO)) USRSUCESO,
                               (SELECT H.TIPHORARIO
                                  FROM HORARIO_CARGO H
                                 WHERE H.IDECARGO = SC.IDECARGO
                                   AND H.PUNTHORARIO =
                                       (SELECT MAX(G.PUNTHORARIO)
                                          FROM HORARIO_CARGO G
                                         WHERE G.IDECARGO = H.IDECARGO)) TIPPUESTO
                        
                          FROM SOLNUEVO_CARGO SC
                         WHERE SC.TIPETAPA IN ('04', '08')
                           AND SC.ESTACTIVO = 'A'
                        
                        ) X
                WHERE (nvl(p_cFecDesde, null) IS NULL OR
                      (TO_DATE(X.FECSUCESO, 'DD/MM/YYYY') >=
                      TO_DATE(p_cFecDesde, 'DD/MM/YYYY')))
                  AND (nvl(p_cFecHasta, null) IS NULL OR
                      (TO_DATE(X.FECSUCESO, 'DD/MM/YYYY') <
                      TO_DATE(p_cFecHasta, 'DD/MM/YYYY') + 1))
                  AND (p_cTipSol IS NULL OR x.tipsol = p_cTipSol)
                  AND (p_cEstadpReq IS NULL OR X.TIPETAPA = p_cEstadpReq)
                  AND (nIdResponsable = 0 OR X.USRSUCESO = nIdResponsable)
                  AND (nIdDependencia = 0 OR
                      X.IDEDEPENDENCIA = nIdDependencia)
                  AND (nIdDepartamento = 0 OR
                      X.IDEDEPARTAMENTO = nIdDepartamento)
                  AND (nIdArea = 0 OR X.IDEAREA = nIdArea)
                  AND (nMotivoReemp = 0 OR X.TIPVACANTE = nMotivoReemp)
                  AND (nSede = 0 OR X.IDESEDE = nSede)
                ORDER BY X.FECCREACION)
    
     LOOP
    
      nContVacantes := C1.NUMVACANTES;
    
      FOR i IN 1 .. C1.NUMVACANTES LOOP
      
        INSERT INTO TEMP_REP_SELECCION
          (ANALISTA_RESP,
           CARGO,
           DESAREA,
           DESDEPARTAMENTO,
           DESDEPENDENCIA,
           DESTIPSOL,
           DIAS,
           ESTACTIVO,
           ESTADO_PROCESO,
           FECCREACION,
           FECEXPIRACACION,
           FECHA_REQUERIMIENTO,
           FECPUBLICACION,
           FECREEMPLAZO,
           FECSUCESO,
           FONO,
           IDCARGO,
           IDEAREA,
           IDEDEPARTAMENTO,
           IDEDEPENDENCIA,
           IDESEDE,
           IDSOL,
           JEFE,
           MOTIVO,
           MOTIVOCIERRE,
           MOTIVOREEMPLAZO,
           NOMBRE_CARGO,
           NUMVACANTES,
           OBSENTREVISTA,
           OBSPSICOLOGO,
           P_INGRESA,
           REEMPLAZA_A,
           SEDE,
           TIPETAPA,
           TIPSOL,
           TIPVACANTE,
           USRSUCESO,
           NUMDOCUMENTO,
           FECHA_CONTRATACION)
        VALUES
          (nvl(C1.ANALISTA_RESP, ''),
           nvl(C1.CARGO, ''),
           nvl(C1.DESAREA, ''),
           nvl(C1.DESDEPARTAMENTO, ''),
           nvl(C1.DESDEPENDENCIA, ''),
           nvl(C1.DESTIPSOL, ''),
           nvl(C1.DIAS, ''),
           nvl(C1.ESTACTIVO, ''),
           nvl(C1.ESTADO_PROCESO, ''),
           nvl(C1.FECCREACION, ''),
           nvl(C1.FECEXPIRACACION, ''),
           nvl(C1.FECHA_REQUERIMIENTO, ''),
           nvl(C1.FECPUBLICACION, ''),
           (pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                 c1.tipsol,
                                                 c1.idesede,
                                                 c1.idcargo,
                                                 c1.tippuesto,
                                                 i,
                                                 7)),
           C1.FECSUCESO,
           (pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                 c1.tipsol,
                                                 c1.idesede,
                                                 c1.idcargo,
                                                 c1.tippuesto,
                                                 i,
                                                 3)),
           C1.IDCARGO,
           C1.IDEAREA,
           C1.IDEDEPARTAMENTO,
           C1.IDEDEPENDENCIA,
           C1.IDESEDE,
           C1.IDSOL,
           C1.JEFE,
           C1.MOTIVO,
           C1.MOTIVOCIERRE,
           C1.MOTIVOREEMPLAZO,
           C1.NOMBRE_CARGO,
           C1.NUMVACANTES,
           ----------------------
           pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                c1.tipsol,
                                                c1.idesede,
                                                c1.idcargo,
                                                c1.tippuesto,
                                                i,
                                                2),
           pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                c1.tipsol,
                                                c1.idesede,
                                                c1.idcargo,
                                                c1.tippuesto,
                                                i,
                                                1),
           pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                c1.tipsol,
                                                c1.idesede,
                                                c1.idcargo,
                                                c1.tippuesto,
                                                i,
                                                4),
           pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                c1.tipsol,
                                                c1.idesede,
                                                c1.idcargo,
                                                c1.tippuesto,
                                                i,
                                                5),
           C1.SEDE,
           C1.TIPETAPA,
           C1.TIPSOL,
           C1.TIPVACANTE,
           C1.USRSUCESO,
           pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                c1.tipsol,
                                                c1.idesede,
                                                c1.idcargo,
                                                c1.tippuesto,
                                                i,
                                                6),
           pr_intranet_ed.fn_obtiene_datos_post(c1.idsol,
                                                c1.tipsol,
                                                c1.idesede,
                                                c1.idcargo,
                                                c1.tippuesto,
                                                i,
                                                8));
      
      END LOOP;
    
    END LOOP;
  
    OPEN p_cRpta FOR
      SELECT * FROM TEMP_REP_SELECCION;
  
  END SP_OBTIENE_REPORTE_SELECCION;

  /* ------------------------------------------------------------
  NOMBRE      : FN_OBTIENE_DATOS_POST
  Proposito   : Obtiene los datos del postulante
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :  p_nIndObs: 1 observaciones de los psicologos
                            2 observaciones de las entrevistas
                 p_nIdReclutaPer id de la persona reclutada
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  FUNCTION FN_OBTIENE_DATOS_POST(p_cIdsol     number,
                                 p_cTipSol    varchar2,
                                 p_cIdSede    number,
                                 p_cIdCargo   number,
                                 p_cTipPuesto varchar2,
                                 p_nPosicion  number,
                                 p_nCampo     number) RETURN VARCHAR2 IS
    cObsPiscologica varchar2(2500);
    cObsEntrevista  varchar2(2500);
    cFono           varchar2(100);
    cPerIngresa     varchar2(2000);
    cReemplaza      varchar2(2000);
    cNumDoc         varchar2(20);
    cFecReemplazo   varchar2(50);
    cINGRESA        varchar2(2000);
    cFechaContrata  varchar2(50);
  
    CURSOR C_POST_CONTRATADOS IS
      select rp.idereclutapersona, rp.idepostulante, rp.fecmodifica
        from reclutamiento_persona rp
       where rp.Idesol = p_cIdsol
         and rp.tipsol = p_cTipSol
         and rp.idsede = p_cIdSede
         and rp.idecargo = p_cIdCargo
         and rp.tippuesto = p_cTipPuesto
         and rp.estpostulante = P_POSTCONTRATADO;
  
    nFila          number(8);
    nFilaReemplazo NUMBER(8);
  BEGIN
  
    nFila          := 0;
    nFilaReemplazo := 0;
    FOR C1 IN C_POST_CONTRATADOS LOOP
      nFila := nFila + 1;
      IF p_nPosicion = nFila THEN
        --observacion Psicologica
        IF p_nCampo = 1 THEN
          begin
            cObsPiscologica := PR_INTRANET_ED.FN_OBTIENE_OBSERVACION(1,
                                                                     C1.IDERECLUTAPERSONA);
          exception
            when others then
              cObsPiscologica := '';
              dbms_output.put_line('Error 1');
          end;
          return nvl(cObsPiscologica, '');
        END IF;
        -- observacion de entrevista
        IF p_nCampo = 2 THEN
          cObsEntrevista := PR_INTRANET_ED.FN_OBTIENE_OBSERVACION(2,
                                                                  C1.IDERECLUTAPERSONA);
        
          return nvl(cObsEntrevista, '');
        END IF;
        --Telefono fijo y cel
        IF p_nCampo = 3 THEN
          SELECT (DECODE(P.TELFIJO, NULL, '', P.TELFIJO||' / ') || '' ||
                 DECODE(P.TELMOVIL, NULL, '', P.TELMOVIL)) NUMFONO
            INTO cFono
            FROM POSTULANTE P
           WHERE P.IDEPOSTULANTE = C1.IDEPOSTULANTE
             and rownum = 1;
        
          return nvl(cFono, '');
        END IF;
        -- Persona que ingresa
        IF p_nCampo = 4 THEN
        
          SELECT NVL((P.APEPATERNO || ' ' || P.APEMATERNO || ' ' ||
                     P.PRINOMBRE),
                     '')
            INTO cPerIngresa
            FROM POSTULANTE P
           WHERE P.IDEPOSTULANTE = C1.IDEPOSTULANTE
             and rownum = 1;
        
          return nvl(cPerIngresa, '');
        END IF;
        -- nombre de la persona a la que se reemplaza
        IF p_nCampo = 5 THEN
          nFilaReemplazo := 0;
          IF p_cTipSol = '03' THEN
            FOR C2 IN (select *
                         from reemplazos r
                        where r.idesolreqpersonal = p_cIdsol) LOOP
            
              nFilaReemplazo := nFilaReemplazo + 1;
              IF p_nPosicion = nFilaReemplazo THEN
                cReemplaza := (NVL(C2.APEPATERNO, '') || ' ' ||
                              NVL(C2.NOMBRES, ''));
                return nvl(cReemplaza, '');
                exit;
              END IF;
            END LOOP;
          END IF;
        END IF;
        -- numero de la persona contratada
        IF p_nCampo = 6 THEN
          SELECT P.NUMDOCUMENTO
            INTO cNumDoc
            FROM POSTULANTE P
           WHERE P.IDEPOSTULANTE = c1.IDEPOSTULANTE
             and rownum = 1;
        
          return nvl(cNumDoc, '');
        END IF;
        -- fecha de inicio y fin de reemplazo
        IF p_nCampo = 7 THEN
          nFilaReemplazo := 0;
          IF p_cTipSol = '03' THEN
            FOR C3 IN (select (DECODE(R.FECINICIOREEMPLAZO,
                                      NULL,
                                      '',
                                      TO_CHAR(R.FECINICIOREEMPLAZO,
                                              'DD/MM/YYYY') || ' - ') || '' ||
                              DECODE(R.FECFINREEMPLAZO,
                                      NULL,
                                      '',
                                      TO_CHAR(R.FECFINREEMPLAZO, 'DD/MM/YYYY'))) FECREEMPLAZO
                         from reemplazos r
                        where r.idesolreqpersonal = p_cIdsol) LOOP
            
              nFilaReemplazo := nFilaReemplazo + 1;
              IF p_nPosicion = nFilaReemplazo THEN
              
                cFecReemplazo := C3.FECREEMPLAZO;
                return nvl(cFecReemplazo, '');
              END IF;
            END LOOP;
          END IF;
        END IF;
      
        IF p_nCampo = 8 THEN
          cFechaContrata := TO_CHAR(c1.FECMODIFICA, 'DD/MM/YYYY');
          return nvl(cFechaContrata, '');
        END IF;
      
      END IF;
    
    END LOOP;
  
    return '';
  
  END FN_OBTIENE_DATOS_POST;

  /* ------------------------------------------------------------
  NOMBRE      : FN_OBTIENE_OBSERVACION
  Proposito   : Obtiene las observaciones
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :  p_nIndObs: 1 observaciones de los psicologos
                            2 observaciones de las entrevistas
                 p_nIdReclutaPer id de la persona reclutada
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_OBTIENE_OBSERVACION(p_nIndObs       IN NUMBER,
                                  p_nIdReclutaPer IN NUMBER) RETURN VARCHAR2 IS
  
    -- obtiene las obsevaciones del psicologo
    CURSOR C_PSICOLOGO(p_nIdRecluta reclutamiento_persona.idereclutapersona%type) IS
      SELECT R.OBSERVACION, R.USRMODIFICA
        FROM RECLU_PERSO_EXAMEN R
       WHERE R.TIPEXAMEN = '04'
         AND R.IDERECLUTAPERSONA = p_nIdRecluta;
  
    --obtiene las observaviones de la entrevista
    CURSOR C_ENTREVISTA(p_nIdRecluta reclutamiento_persona.idereclutapersona%type) IS
      SELECT R.OBSERVACION, R.USRMODIFICA
        FROM RECLU_PERSO_EXAMEN R
       WHERE R.TIPEXAMEN = '02'
         AND R.IDERECLUTAPERSONA = p_nIdRecluta;
  
    contObs    number;
    cResultado varchar2(3000);
  BEGIN
  
    contObs := 1;
    IF p_nIndObs = 1 THEN
    
      FOR C1 IN C_PSICOLOGO(p_nIdReclutaPer) LOOP
      
        IF contObs = 1 THEN
          cResultado := cResultado || ' ' ||
                        (C1.USRMODIFICA || ': ' || C1.OBSERVACION);
        ELSE
          cResultado := cResultado || '/ ' ||
                        (C1.USRMODIFICA || ': ' || C1.OBSERVACION);
        END IF;
      
        contObs := contObs + 1;
      
      END LOOP;
    
    END IF;
  
    IF p_nIndObs = 2 THEN
    
      FOR C2 IN C_ENTREVISTA(p_nIdReclutaPer) LOOP
      
        IF contObs = 1 THEN
          cResultado := cResultado || ' ' ||
                        (C2.USRMODIFICA || ': ' || C2.OBSERVACION);
        ELSE
          cResultado := cResultado || '/ ' ||
                        (C2.USRMODIFICA || ': ' || C2.OBSERVACION);
        END IF;
      
        contObs := contObs + 1;
      
      END LOOP;
    
    END IF;
  
    RETURN NVL(TRIM(SUBSTR(cResultado, 0, 3000)), '');
  
  END;

  /* ------------------------------------------------------------
  NOMBRE      : SP_REPORTE_RESUMEN_RQ
  Proposito   : Obtiene el reporte de resumen de requerimientos,
                obtiene los analista de solicitudes que fueron publicadas
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :  p_nIdEncargado id de la persona responsable
                 p_nIdSede id de la sede
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_REPORTE_RESUMEN_RQ(p_cFecInicio   IN VARCHAR2,
                                  p_cFecFin      IN VARCHAR2,
                                  p_nIdEncargado IN NUMBER,
                                  p_nIdSede      IN NUMBER,
                                  p_curRpta      OUT cur_cursor) IS
  
  BEGIN
    OPEN p_curRpta FOR
    
      SELECT NVL((SELECT S.DESCRIPCION
                   FROM SEDE S
                  WHERE S.IDESEDE = p_nIdSede
                    AND S.ESTREGISTRO = 'A'),
                 '') SEDE,
             NVL(Y.USRSUCESO, '') USRSUCESO,
             NVL((SELECT U.DSCAPEPATERNO || ' ' || U.DSCAPEMATERNO || ' ' ||
                        U.DSCNOMBRES
                   FROM USUARIO U
                  WHERE U.IDUSUARIO = Y.USRSUCESO
                    AND U.FLGESTADO = 'A'
                    AND U.TIPUSUARIO = 'I'),
                 '') DESUSUARIO,
             ROUND(NVL(Y.SALDO, 0)) SALDO,
             ROUND(NVL(Y.REEMPLAZO, 0)) REEMPLAZO,
             ROUND(NVL(Y.AMPLIACION, 0)) AMPLIACION,
             ROUND(NVL(Y.NUEVO, 0)) NUEVO,
             ROUND(NVL(Y.NO_CUBIERTO, 0)) NO_CUBIERTO,
             ROUND(NVL(Y.CUBIERTO, 0)) CUBIERTO,
             ROUND(NVL((Y.SALDO + Y.REEMPLAZO + Y.AMPLIACION + Y.NUEVO +
                       Y.NO_CUBIERTO + Y.CUBIERTO),
                       0)) TOTAL
        FROM (
              
              SELECT DISTINCT X.USRSUCESO,
                               (NVL(PR_INTRANET_ED.FN_SALDO_FECHA(X.USRSUCESO,
                                                                  p_cFecInicio,
                                                                  p_nIdSede),
                                    0)) SALDO,
                               (pr_intranet_ed.fn_obtiene_vacantes(3,
                                                                   X.USRSUCESO,
                                                                   p_cFecInicio,
                                                                   p_cFecFin,
                                                                   p_nIdSede)) REEMPLAZO,
                               (pr_intranet_ed.fn_obtiene_vacantes(2,
                                                                   X.USRSUCESO,
                                                                   p_cFecInicio,
                                                                   p_cFecFin,
                                                                   p_nIdSede)) AMPLIACION,
                               (pr_intranet_ed.fn_obtiene_vacantes(1,
                                                                   X.USRSUCESO,
                                                                   p_cFecInicio,
                                                                   p_cFecFin,
                                                                   p_nIdSede)) NUEVO,
                               (pr_intranet_ed.fn_obtiene_vacantes_fin(1,
                                                                       X.USRSUCESO,
                                                                       p_cFecInicio,
                                                                       p_cFecFin,
                                                                       p_nIdSede)) NO_CUBIERTO,
                               (pr_intranet_ed.fn_obtiene_vacantes_fin(2,
                                                                       X.USRSUCESO,
                                                                       p_cFecInicio,
                                                                       p_cFecFin,
                                                                       p_nIdSede)) CUBIERTO
              
                FROM (SELECT DISTINCT LO.USRSUCESO
                         FROM SOLREQ_PERSONAL SP, LOGSOLREQ_PERSONAL LO
                        WHERE LO.IDESOLREQPERSONAL = SP.IDESOLREQPERSONAL
                          AND SP.ESTACTIVO = 'A'
                          AND LO.FECSUCESO =
                              (SELECT MAX(L.FECSUCESO)
                                 FROM LOGSOLREQ_PERSONAL L
                                WHERE L.IDESOLREQPERSONAL = SP.IDESOLREQPERSONAL
                                  AND L.TIPETAPA = '04')
                          AND (p_nIdEncargado = 0 OR
                              p_nIdEncargado = LO.USRSUCESO)
                          AND (p_nIdSede IS NULL OR SP.IDESEDE = p_nIdSede)
                          AND (NVL(p_cFecInicio, NULL) IS NULL OR
                              TO_DATE(TO_CHAR(LO.fecsuceso, 'DD/MM/YYYY'),
                                       'DD/MM/YYYY') >=
                              TO_DATE(p_cFecInicio, 'DD/MM/YYYY'))
                          AND (nvl(p_cFecFin, null) IS NULL OR
                              TO_DATE(TO_CHAR(LO.fecsuceso, 'DD/MM/YYYY'),
                                       'DD/MM/YYYY') <
                              TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1)
                        GROUP BY LO.USRSUCESO, LO.TIPETAPA
                       UNION ALL
                       SELECT DISTINCT LN.USRSUCESO
                         FROM SOLNUEVO_CARGO SN, LOGSOLNUEVO_CARGO LN
                        WHERE LN.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO
                          AND SN.ESTACTIVO = 'A'
                          AND LN.FECSUCESO =
                              (SELECT MAX(LN.FECSUCESO)
                                 FROM LOGSOLNUEVO_CARGO LN2
                                WHERE LN2.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO
                                  AND LN2.TIPETAPA = '04')
                          AND (p_nIdEncargado = 0 OR
                              p_nIdEncargado = LN.USRSUCESO)
                          AND (p_nIdSede IS NULL OR SN.IDESEDE = p_nIdSede)
                          AND (NVL(p_cFecInicio, NULL) IS NULL OR
                              TO_DATE(TO_CHAR(LN.fecsuceso, 'DD/MM/YYYY'),
                                       'DD/MM/YYYY') >=
                              TO_DATE(p_cFecInicio, 'DD/MM/YYYY'))
                          AND (nvl(p_cFecFin, null) IS NULL OR
                              TO_DATE(TO_CHAR(LN.fecsuceso, 'DD/MM/YYYY'),
                                       'DD/MM/YYYY') <
                              TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1)
                        GROUP BY LN.USRSUCESO, LN.TIPETAPA) X) Y;
  
  END SP_REPORTE_RESUMEN_RQ;

  /* ------------------------------------------------------------
  NOMBRE      : FN_SALDO_FECHA
  Proposito   : Obtiene el saldo a la fecha, es decir el numero
                de vacantes no cubiertas del inicio hace unos 6 meses
                de solicitudes publicadas
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :  p_nIdUsuario id de Usuario
                 p_cFecInicio Fecha inicial para tormar la referencia de la fecha
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_SALDO_FECHA(p_nIdUsuario IN NUMBER,
                          p_cFecInicio IN VARCHAR2,
                          p_nIdSede    IN NUMBER) RETURN VARCHAR2 IS
  
    nSaldoTotal number(8);
    nFechaSaldo varchar2(20);
  BEGIN
    --obtiene la fecha hace 6 meses
    nFechaSaldo := TO_CHAR(ADD_MONTHS(TO_DATE(p_cFecInicio, 'DD/MM/YYYY')-1,
                                      -6),
                           'DD/MM/YYYY');
  
    BEGIN
      -- se calcula el numero de vacantes para solicitudes publicadas
      SELECT SUM(X.FALTANTES)
        INTO nSaldoTotal
        FROM (SELECT SUM((S.NUMVACANTES -
                         (SELECT COUNT(R.IDESOL)
                             FROM RECLUTAMIENTO_PERSONA R
                            WHERE R.IDESOL = S.IDESOLREQPERSONAL
                              AND R.ESTPOSTULANTE = '09'
                              and r.idsede = s.idesede))) FALTANTES
              
                FROM SOLREQ_PERSONAL S, LOGSOLREQ_PERSONAL LO
               WHERE S.IDESOLREQPERSONAL = LO.IDESOLREQPERSONAL
                 AND S.ESTACTIVO = 'A'
                 AND (p_nIdSede IS NULL OR S.IDESEDE = p_nIdSede)
                 AND LO.FECSUCESO =
                     (SELECT MAX(L.FECSUCESO)
                        FROM LOGSOLREQ_PERSONAL L
                       WHERE L.IDESOLREQPERSONAL = S.IDESOLREQPERSONAL)
                 AND S.TIPETAPA IN ('04')
                 AND S.Tipetapa = LO.TIPETAPA
                 AND LO.USRSUCESO = p_nIdUsuario
                 AND (NVL(nFechaSaldo, NULL) IS NULL OR
                     TO_DATE(TO_CHAR(lo.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') >=
                     TO_DATE(nFechaSaldo, 'DD/MM/YYYY'))
                 AND (nvl(p_cFecInicio, null) IS NULL OR
                     TO_DATE(TO_CHAR(lo.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') <
                     TO_DATE(p_cFecInicio, 'DD/MM/YYYY') + 1)
              UNION ALL
              SELECT SUM((SN.NUMPOSICIONES -
                         (SELECT COUNT(R.IDESOL)
                             FROM RECLUTAMIENTO_PERSONA R
                            WHERE R.IDESOL = SN.IDESOLNUEVOCARGO
                              AND R.ESTPOSTULANTE = '09'
                              and r.idsede = sn.idesede))) FALTANTES
              
                FROM SOLNUEVO_CARGO SN, LOGSOLNUEVO_CARGO LN
               WHERE SN.IDESOLNUEVOCARGO = LN.IDESOLNUEVOCARGO
                 AND SN.ESTACTIVO = 'A'
                 AND (p_nIdSede IS NULL OR SN.IDESEDE = p_nIdSede)
                 AND LN.FECSUCESO =
                     (SELECT MAX(L1.FECSUCESO)
                        FROM LOGSOLNUEVO_CARGO L1
                       WHERE L1.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO)
                 AND SN.TIPETAPA IN ('04')
                 AND SN.Tipetapa = LN.TIPETAPA
                 AND LN.USRSUCESO = p_nIdUsuario
                 AND (NVL(nFechaSaldo, NULL) IS NULL OR
                     TO_DATE(TO_CHAR(LN.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') >=
                     TO_DATE(nFechaSaldo, 'DD/MM/YYYY'))
                 AND (nvl(p_cFecInicio, null) IS NULL OR
                     TO_DATE(TO_CHAR(LN.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') <
                     TO_DATE(p_cFecInicio, 'DD/MM/YYYY') + 1)) X;
    
    END;
  
    RETURN nvl(nSaldoTotal, 0);
  
  END FN_SALDO_FECHA;

  /* ------------------------------------------------------------
  NOMBRE      : FN_OBTIENE_VACANTES
  Proposito   : obtiene el numero de vacantes de las solictudes que fueron publicadas
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :  p_nIdUsuario id de Usuario
                 p_cFecInicio Fecha inicial para tormar la referencia de la fecha
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_OBTIENE_VACANTES(p_IndEtapa   number,
                               p_nIdUsuario number,
                               p_cFecInicio varchar2,
                               p_cFecFin    varchar2,
                               p_cIdSede    Number) RETURN NUMBER IS
    nTotal number(8);
  
  BEGIN
    -- reemplazo
    IF p_IndEtapa = 3 THEN
      SELECT SUM(SOL.NUMVACANTES) TOTAL
        INTO nTotal
        FROM LOGSOLREQ_PERSONAL LOG, SOLREQ_PERSONAL SOL
       WHERE LOG.IDESOLREQPERSONAL = SOL.IDESOLREQPERSONAL
         AND SOL.ESTACTIVO = 'A'
         AND LOG.TIPETAPA = '04'
         AND SOL.TIPSOL = '03'
         AND LOG.USRSUCESO = p_nIdUsuario
         AND SOL.IDESEDE = p_cIdSede
         AND (NVL(p_cFecInicio, NULL) IS NULL OR
             TO_DATE(TO_CHAR(LOG.fecsuceso, 'DD/MM/YYYY'), 'DD/MM/YYYY') >=
             TO_DATE(p_cFecInicio, 'DD/MM/YYYY'))
         AND (nvl(p_cFecFin, null) IS NULL OR
             TO_DATE(TO_CHAR(LOG.fecsuceso, 'DD/MM/YYYY'), 'DD/MM/YYYY') <
             TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1);
    END IF;
    --ampliacion
    IF p_IndEtapa = 2 THEN
      SELECT SUM(SOL.NUMVACANTES) TOTAL
        INTO nTotal
        FROM LOGSOLREQ_PERSONAL LOG, SOLREQ_PERSONAL SOL
       WHERE LOG.IDESOLREQPERSONAL = SOL.IDESOLREQPERSONAL
         AND SOL.ESTACTIVO = 'A'
         AND LOG.TIPETAPA = '04'
         AND SOL.TIPSOL = '02'
         AND LOG.USRSUCESO = p_nIdUsuario
         AND SOL.IDESEDE = p_cIdSede
         AND (NVL(p_cFecInicio, NULL) IS NULL OR
             TO_DATE(TO_CHAR(LOG.fecsuceso, 'DD/MM/YYYY'), 'DD/MM/YYYY') >=
             TO_DATE(p_cFecInicio, 'DD/MM/YYYY'))
         AND (nvl(p_cFecFin, null) IS NULL OR
             TO_DATE(TO_CHAR(LOG.fecsuceso, 'DD/MM/YYYY'), 'DD/MM/YYYY') <
             TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1);
    END IF;
  
    --nuevo
    IF p_IndEtapa = 1 THEN
      SELECT SUM(SN.NUMPOSICIONES) TOTAL
        INTO nTotal
        FROM LOGSOLNUEVO_CARGO LN, SOLNUEVO_CARGO SN
       WHERE LN.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO
         AND SN.ESTACTIVO = 'A'
         AND LN.TIPETAPA = '04'
         AND LN.USRSUCESO = p_nIdUsuario
         AND SN.IDESEDE = p_cIdSede
         AND (NVL(p_cFecInicio, NULL) IS NULL OR
             TO_DATE(TO_CHAR(LN.fecsuceso, 'DD/MM/YYYY'), 'DD/MM/YYYY') >=
             TO_DATE(p_cFecInicio, 'DD/MM/YYYY'))
         AND (nvl(p_cFecFin, null) IS NULL OR
             TO_DATE(TO_CHAR(LN.fecsuceso, 'DD/MM/YYYY'), 'DD/MM/YYYY') <
             TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1);
    END IF;
  
    RETURN nvl(nTotal, 0);
  
  END FN_OBTIENE_VACANTES;

  /* ------------------------------------------------------------
  NOMBRE      : FN_OBTIENE_VACANTES_FIN
  Proposito   : obtiene la cantidad de vacantes cubiertas de las solictudes
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  FUNCTION FN_OBTIENE_VACANTES_FIN(p_indCub     number,
                                   p_nIdUsuario number,
                                   p_cFecInicio varchar2,
                                   p_cFecFin    varchar2,
                                   p_cIdSede    Number) RETURN NUMBER IS
  
    nCubierto   number(8);
    nNoCubierto number(8);
  BEGIN
  
    --cubiertos
    BEGIN
      SELECT SUM(X.NO_CUBIERTO) NO_CUBIERTO, SUM(X.CUBIERTO) CUBIERTO
        into nNoCubierto, nCubierto
        FROM (SELECT (SOL.NUMVACANTES -
                     (SELECT COUNT(R.IDESOL)
                         FROM RECLUTAMIENTO_PERSONA R
                        WHERE R.IDESOL = SOL.IDESOLREQPERSONAL
                          AND R.ESTPOSTULANTE = '09'
                          AND R.IDSEDE = SOL.IDESEDE)) NO_CUBIERTO,
                     (SELECT COUNT(R.IDESOL)
                        FROM RECLUTAMIENTO_PERSONA R
                       WHERE R.IDESOL = SOL.IDESOLREQPERSONAL
                         AND R.ESTPOSTULANTE = '09'
                         AND R.IDSEDE = SOL.IDESEDE) CUBIERTO
                FROM SOLREQ_PERSONAL SOL, LOGSOLREQ_PERSONAL LOG
               WHERE LOG.IDESOLREQPERSONAL = SOL.IDESOLREQPERSONAL
                 AND SOL.ESTACTIVO = 'A'
                 AND LOG.USRSUCESO = p_nIdUsuario
                 AND LOG.TIPETAPA = SOL.TIPETAPA
                 AND SOL.IDESEDE = P_CIDSEDE
                 AND SOL.TIPETAPA = '08'
                    
                 AND (NVL(p_cFecInicio, NULL) IS NULL OR
                     TO_DATE(TO_CHAR(log.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') >=
                     TO_DATE(p_cFecInicio, 'DD/MM/YYYY'))
                 AND (nvl(p_cFecFin, null) IS NULL OR
                     TO_DATE(TO_CHAR(log.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') <
                     TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1)
              UNION ALL
              SELECT (SN.NUMPOSICIONES -
                     (SELECT COUNT(R.IDESOL)
                         FROM RECLUTAMIENTO_PERSONA R
                        WHERE R.IDESOL = SN.IDESOLNUEVOCARGO
                          AND R.ESTPOSTULANTE = '09'
                          AND R.IDSEDE = SN.IDESEDE)) NO_CUBIERTO,
                     (SELECT COUNT(R.IDESOL)
                        FROM RECLUTAMIENTO_PERSONA R
                       WHERE R.IDESOL = SN.IDESOLNUEVOCARGO
                         AND R.ESTPOSTULANTE = '09'
                         AND R.IDSEDE = SN.IDESEDE) CUBIERTO
                FROM SOLNUEVO_CARGO SN, LOGSOLNUEVO_CARGO LN
               WHERE LN.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO
                 AND SN.ESTACTIVO = 'A'
                 AND LN.TIPETAPA = SN.TIPETAPA
                 AND LN.USRSUCESO = p_nIdUsuario
                 AND SN.IDESEDE = P_CIDSEDE
                 AND SN.TIPETAPA = '08'
                    
                 AND (NVL(p_cFecInicio, NULL) IS NULL OR
                     TO_DATE(TO_CHAR(ln.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') >=
                     TO_DATE(p_cFecInicio, 'DD/MM/YYYY'))
                 AND (nvl(p_cFecFin, null) IS NULL OR
                     TO_DATE(TO_CHAR(ln.fecsuceso, 'DD/MM/YYYY'),
                              'DD/MM/YYYY') <
                     TO_DATE(p_cFecFin, 'DD/MM/YYYY') + 1)) X;
    EXCEPTION
      WHEN OTHERS THEN
        nNoCubierto := 0;
        nCubierto   := 0;
    END;
    -- devuelve los no cubiertos
    IF p_indCub = 1 THEN
      IF nNoCubierto > 0 THEN
        RETURN TO_NUMBER('-' || nNoCubierto);
      ELSE
        RETURN 0;
      END IF;
    END IF;
  
    -- devuelve los cubiertos
    IF p_indCub = 2 THEN
      IF nCubierto > 0 THEN
        RETURN TO_NUMBER('-' || nCubierto);
      ELSE
        RETURN 0;
      END IF;
    END IF;
  
  END FN_OBTIENE_VACANTES_FIN;

  /* ------------------------------------------------------------
  NOMBRE      : reiniciar_secuencia
  Proposito   : Elimina una solicitud que no fue trabajada
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  procedure reiniciar_secuencia(p_nombre in varchar2) is
    res number;
  begin
    execute immediate 'select ' || p_nombre || '.nextval from dual'
      INTO res;
  
    execute immediate 'alter sequence ' || p_nombre || ' increment by -' || res ||
                      ' minvalue 0';
  
    execute immediate 'select ' || p_nombre || '.nextval from dual'
      INTO res;
  
    execute immediate 'alter sequence ' || p_nombre ||
                      ' increment by 1 minvalue 0';
  end;

  /* ------------------------------------------------------------
  NOMBRE      : SP_ELIMINA_SOLICITUD
  Proposito   : Elimina una solicitud que no fue trabajada
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_ELIMINA_SOLICITUD(p_nIdSol     IN NUMBER,
                                 p_cTipSol    IN VARCHAR2,
                                 p_nIdUsuario IN number,
                                 p_nRolUsario IN number,
                                 p_cRetVal    OUT NUMBER,
                                 P_cMensaje   OUT VARCHAR2) IS
    nContSol    NUMBER;
    cDesUsuario usuario.codusuario%type;
    cRolUsuario rol.dscrol%type;
  
  BEGIN
  
    IF p_cTipSol <> '01' THEN
    
      SELECT COUNT(*)
        INTO nContSol
        FROM LOGSOLREQ_PERSONAL LP
       WHERE LP.IDESOLREQPERSONAL = p_nIdSol;
    
    ELSE
    
      SELECT COUNT(*)
        INTO nContSol
        FROM LOGSOLNUEVO_CARGO LN
       WHERE LN.IDESOLNUEVOCARGO = p_nIdSol;
    
    END IF;
  
    IF nContSol = 1 THEN
    
      IF p_cTipSol <> '01' THEN
      
        UPDATE SOLREQ_PERSONAL S
           SET S.ESTACTIVO = 'I'
         WHERE S.IDESOLREQPERSONAL = p_nIdSol;
      
        pr_intranet_ed.sp_insert_log_solreqpersonal(p_nIdSol,
                                                    '09',
                                                    sysdate,
                                                    p_nIdUsuario,
                                                    p_nRolUsario,
                                                    0,
                                                    0,
                                                    '');
      
      ELSE
      
        UPDATE SOLNUEVO_CARGO N
           SET N.ESTACTIVO = 'I'
         WHERE N.IDESOLNUEVOCARGO = p_nIdSol;
      
        pr_intranet_ed.sp_insert_log_solnuevo_cargo(p_nIdSol,
                                                    '09',
                                                    sysdate,
                                                    p_nIdUsuario,
                                                    p_nRolUsario,
                                                    0,
                                                    0,
                                                    '');
      
      END IF;
    
      COMMIT;
    
      p_cRetVal  := 1;
      P_cMensaje := 'Se elimino la solicitud';
    
    ELSE
    
      IF p_cTipSol <> '01' THEN
      
        --REQ AMPLIACION Y REEMPLAZO
        BEGIN
          SELECT (SELECT U.CODUSUARIO
                    FROM USUARIO U
                   WHERE U.IDUSUARIO = LS.USRESPONSABLE
                     AND U.TIPUSUARIO = 'I'
                     AND U.FLGESTADO = 'A') DESUSUARIO,
                 (SELECT R.DSCROL
                    FROM ROL R
                   WHERE R.IDROL = LS.ROLRESPONSABLE
                     AND R.FLGESTADO = 'A') DESROL
            INTO cDesUsuario, cRolUsuario
            FROM LOGSOLREQ_PERSONAL LS
           WHERE LS.IDESOLREQPERSONAL = p_nIdSol
             AND LS.FECSUCESO =
                 (SELECT MAX(LS1.FECSUCESO)
                    FROM LOGSOLREQ_PERSONAL LS1
                   WHERE LS1.IDESOLREQPERSONAL = LS.IDESOLREQPERSONAL);
        EXCEPTION
          WHEN OTHERS THEN
            cDesUsuario := '';
            cRolUsuario := '';
        END;
      
        IF LENGTH(TRIM(cDesUsuario)) > 0 THEN
        
          IF p_cTipSol = '02' THEN
            P_cMensaje := 'La solicitud de ampliacion no se puede eliminar, se encuentra asignada al usuario: ' ||
                          cDesUsuario || ' con rol: ' || cRolUsuario;
          END IF;
        
          IF p_cTipSol = '03' THEN
            P_cMensaje := 'La solicitud de reemplazo no se puede eliminar, se encuentra asignada al usuario: ' ||
                          cDesUsuario || ' con rol: ' || cRolUsuario;
          END IF;
        
        END IF;
      
      ELSE
        --REQ NUEVO CARGO
      
        BEGIN
          SELECT (SELECT U.CODUSUARIO
                    FROM USUARIO U
                   WHERE U.IDUSUARIO = L.USRESPONSABLE
                     AND U.TIPUSUARIO = 'I'
                     AND U.FLGESTADO = 'A') DESUSUARIO,
                 (SELECT R.DSCROL
                    FROM ROL R
                   WHERE R.IDROL = L.ROLRESPONSABLE
                     AND R.FLGESTADO = 'A') DESROL
            INTO cDesUsuario, cRolUsuario
            FROM LOGSOLNUEVO_CARGO L
           WHERE L.IDESOLNUEVOCARGO = p_nIdSol
             AND L.FECSUCESO =
                 (SELECT MAX(LG1.FECSUCESO)
                    FROM LOGSOLNUEVO_CARGO LG1
                   WHERE LG1.IDESOLNUEVOCARGO = L.IDESOLNUEVOCARGO);
        EXCEPTION
          WHEN OTHERS THEN
            cDesUsuario := '';
            cRolUsuario := '';
        END;
      
        IF cDesUsuario != '' THEN
          P_cMensaje := 'La solicitud de nuevo cargo no se puede eliminar, se encuentra asignada al usuario: ' ||
                        cDesUsuario || ' con rol: ' || cRolUsuario;
        END IF;
      
      END IF;
    
      p_cRetVal := 0;
    
    END IF;
  
  END SP_ELIMINA_SOLICITUD;

  /* ------------------------------------------------------------
  NOMBRE      : SP_OBTIENE_USUARIOS
  Proposito   : obtiene la lista de usuarios
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_USUARIOS(p_nIdRol      IN NUMBER,
                                p_nIdSede     IN NUMBER,
                                p_cNombres    IN VARCHAR2,
                                p_cCodUsuario IN VARCHAR2,
                                p_cDesApePat  IN VARCHAR2,
                                p_cDesApeMat  IN VARCHAR2,
                                p_cRpta       OUT cur_cursor) IS
  
    nIdRol  NUMBER;
    nIdSede NUMBER;
  
  BEGIN
  
    IF p_nIdRol IS NULL THEN
      nIdRol := 0;
    ELSE
      nIdRol := p_nIdRol;
    END IF;
  
    IF p_nIdSede IS NULL THEN
      nIdSede := 0;
    ELSE
      nIdSede := p_nIdSede;
    END IF;
  
    BEGIN
      OPEN p_cRpta FOR
        SELECT DISTINCT V.FLGESTADO,
                        V.IDUSUARIO,
                        V.CODUSUARIO,
                        V.DSCNOMBRES,
                        V.DSCAPEPATERNO,
                        V.DSCAPEMATERNO,
                        V.DESROL,
                        V.DESSEDE,
                        V.TIPUSUARIO
          FROM VISTA_USUARIO V
         WHERE V.TIPUSUARIO = 'I'
              --Filtros
           AND (nIdRol = 0 OR V.IDROL = nIdRol)
           AND (nIdSede = 0 OR V.IDESEDE = nIdSede)
           AND (p_cNombres IS NULL OR
               UPPER(V.DSCNOMBRES) LIKE '%' || UPPER(p_cNombres) || '%')
           AND (p_cCodUsuario IS NULL OR
               UPPER(V.CODUSUARIO) LIKE '%' || UPPER(p_cCodUsuario) || '%')
           AND (p_cDesApePat IS NULL OR UPPER(V.DSCAPEPATERNO) LIKE
               '%' || UPPER(p_cDesApePat) || '%')
           AND (p_cDesApeMat IS NULL OR UPPER(V.DSCAPEMATERNO) LIKE
               '%' || UPPER(p_cDesApeMat) || '%')
         ORDER BY V.IDUSUARIO DESC;
    END;
  
  END SP_OBTIENE_USUARIOS;

  /* ------------------------------------------------------------
  Nombre      : SP_OBTIENE_CATEGORIAS
  Proposito   : obtiene las categorias que no estan relacionadas a un examen
                de una sede
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_CATEGORIAS(p_cTipCategoria IN VARCHAR2,
                                  p_cDescrip      IN VARCHAR2,
                                  p_nIdSede       IN CATEGORIA.IDESEDE%TYPE,
                                  p_cNombreCat    IN CATEGORIA.NOMCATEGORIA%TYPE,
                                  p_cRpta         OUT cur_cursor) IS
  
    nIdSede number;
  BEGIN
  
    IF p_nIdSede IS NULL THEN
    
      nIdSede := 0;
    ELSE
      nIdSede := p_nIdSede;
    END IF;
  
    OPEN p_cRpta FOR
      SELECT CA.ESTACTIVO,
             CA.IDECATEGORIA,
             CA.NOMCATEGORIA,
             CA.DESCCATEGORIA,
             CA.TIPCATEGORIA,
             (pr_intranet.sp_lista_lval(1, CA.TIPCATEGORIA)) DESTIPCATEGORIA,
             CA.idesede
        FROM CATEGORIA CA
       WHERE NOT EXISTS
       (SELECT EC.IDECATEGORIA
                FROM EXAMEN_X_CATEGORIA EC
               WHERE EC.IDECATEGORIA = CA.IDECATEGORIA)
         AND (p_cTipCategoria IS NULL OR CA.TIPCATEGORIA = p_cTipCategoria)
         AND (p_cDescrip IS NULL OR
             UPPER(CA.DESCCATEGORIA) LIKE '%' || UPPER(p_cDescrip) || '%')
         AND (p_cNombreCat IS NULL OR
             UPPER(CA.NOMCATEGORIA) LIKE '%' || UPPER(p_cNombreCat) || '%')
         AND (nIdSede = 0 OR CA.IDESEDE = nIdSede)
         AND CA.ESTACTIVO = 'A'
       ORDER BY ca.idecategoria, ca.estactivo;
  
  END SP_OBTIENE_CATEGORIAS;

  /* ------------------------------------------------------------
  Nombre      : SP_GET_CARGOXSEDE
  Proposito   : obtiene cargo por la sede, dependencia, departamento,area
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */
  PROCEDURE SP_GET_CARGOXSEDE(p_nIdSede         IN NUMBER,
                              p_nIdDependencia  IN NUMBER,
                              p_nIdDepartamento IN NUMBER,
                              p_nIdArea         IN NUMBER,
                              p_cRetVal         OUT CUR_CURSOR) IS
  
    nIdSede         number(8);
    nIdDependencia  number(8);
    nIdDepartamento number(8);
    nIdArea         number(8);
  
  BEGIN
  
    IF p_nIdSede IS NULL THEN
      nIdSede := 0;
    ELSE
      nIdSede := p_nIdSede;
    END IF;
  
    IF p_nIdDependencia IS NULL THEN
      nIdDependencia := 0;
    ELSE
      nIdDependencia := p_nIdDependencia;
    END IF;
  
    IF p_nIdDepartamento IS NULL THEN
      nIdDepartamento := 0;
    ELSE
      nIdDepartamento := p_nIdDepartamento;
    END IF;
    IF p_nIdArea IS NULL THEN
      nIdArea := 0;
    ELSE
      nIdArea := p_nIdArea;
    END IF;
  
    OPEN P_CRETVAL FOR
      SELECT DISTINCT C.IDECARGO, C.NOMCARGO
        FROM CARGO C
       WHERE C.ESTACTIVO = 'A'
         AND NVL(C.VERSION, 1) =
             (SELECT NVL(MAX(C1.VERSION), 1)
                FROM CARGO C1
               WHERE C1.ESTACTIVO = 'A'
                 AND C1.CODCARGO = C.CODCARGO)
         AND (nIdSede = 0 OR C.IDESEDE = nIdSede)
         AND (nIdDependencia = 0 OR C.IDEDEPENDENCIA = nIdDependencia)
         AND (nIdDepartamento = 0 OR C.IDEDEPARTAMENTO = nIdDepartamento)
         AND (nIdArea = 0 OR C.IDEAREA = nIdArea)
       ORDER BY C.NOMCARGO asc;
  
  END SP_GET_CARGOXSEDE;

  /* ------------------------------------------------------------
  Nombre      : SP_OBTIENE_ROLXEMAIL
  Proposito   : obtiene los id de los usaurios de envio y copia de correos
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_ROLXEMAIL(p_cIdSol       IN NUMBER,
                                 p_cIdRolSuceso IN VARCHAR2,
                                 p_cTipSol      IN VARCHAR2,
                                 p_cAccion      IN VARCHAR2,
                                 p_cIdSede      IN NUMBER,
                                 p_cRetVal      OUT CUR_CURSOR) IS
  
    cIdCreador number;
    cTipEtapa  varchar2(10);
    nIdSede    number;
  BEGIN
  
    IF p_cIdSede IS NULL THEN
      nIdSede := 0;
    ELSE
      nIdSede := p_cIdSede;
    END IF;
  
    IF p_cTipSol = '01' THEN
      --se obtiene la etapa antes de que se actualice la solicitud
      BEGIN
        select sn.tipetapa
          into cTipEtapa
          from solnuevo_cargo sn
         where sn.idesolnuevocargo = p_cIdSol;
      EXCEPTION
        when others then
          cTipEtapa := null;
      END;
      --se obtiene el id del creador
      BEGIN
        SELECT NVL(L.ROLSUCESO, 0)
          INTO cIdCreador
          FROM LOGSOLNUEVO_CARGO L
         WHERE L.IDESOLNUEVOCARGO = p_cIdSol
           AND L.IDELOGSOLNUEVOCARGO =
               (SELECT MIN(L1.IDELOGSOLNUEVOCARGO)
                  FROM LOGSOLNUEVO_CARGO L1
                 WHERE L1.IDESOLNUEVOCARGO = L.IDESOLNUEVOCARGO);
      EXCEPTION
        WHEN OTHERS THEN
          cIdCreador := p_cIdRolSuceso;
      END;
    ELSE
      --se obtiene la etapa antes de que se actualice la solicitud
      BEGIN
        select sr.Tipetapa
          into cTipEtapa
          from solreq_personal sr
         where sr.idesolreqpersonal = p_cIdSol;
      EXCEPTION
        when others then
          cTipEtapa := null;
      END;
      --se obtiene el id del creador
      BEGIN
        SELECT NVL(Lq.Rolsuceso, 0)
          INTO cIdCreador
          FROM LOGSOLREQ_PERSONAL Lq
         WHERE Lq.IDESOLREQPERSONAL = p_cIdSol
           AND Lq.Idelogsolreq_Personal =
               (SELECT MIN(Lq1.Idelogsolreq_Personal)
                  FROM LOGSOLREQ_PERSONAL Lq1
                 WHERE Lq1.Idesolreqpersonal = lq.idesolreqpersonal);
      EXCEPTION
        WHEN OTHERS THEN
          cIdCreador := p_cIdRolSuceso;
      END;
    END IF;
  
    OPEN P_CRETVAL FOR
      SELECT SENDTO, COPYTO1, COPYTO2, COPYTO3
        FROM CONFIG_EMAIL C
       WHERE (cIdCreador = 0 OR C.IDCREADOR = cIdCreador)
         and (p_cIdRolSuceso IS NULL OR C.IDSUCESO = p_cIdRolSuceso)
         and (p_cTipSol IS NULL OR C.TIPSOL = p_cTipSol)
         and (cTipEtapa IS NULL OR C.TIPETAPA = cTipEtapa)
         and (p_cAccion IS NULL OR C.ACCION = p_cAccion)
         and (nIdSede = 0 OR C.IDSEDE = nIdSede)
         and C.ESTACTIVO = 'A';
  
  END SP_OBTIENE_ROLXEMAIL;

  /* ------------------------------------------------------------
  Nombre      : SP_OBTIENE_EMAIL
  Proposito   : obtiene los email por rol y sede
  Referencias : Sistema de Reclutamiento y Selecci?n de Personal
  Parametros  :
  
  Log de Cambios
    Fecha       Autor                Descripcion
    14/02/2014  Edward Llamoca       Creaci?n
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTIENE_EMAIL(p_nIdRol  IN number,
                             p_nIdSede IN number,
                             p_cRetVal OUT CUR_CURSOR) IS
  
  BEGIN
  
    OPEN P_CRETVAL FOR
      SELECT trim(nvl((SELECT U.EMAIL
                        FROM USUARIO U
                       WHERE U.IDUSUARIO = E.IDUSUARIO),
                      '')) EMAIL
        FROM USUAROLSEDE E
       WHERE IDROL = p_nIdRol
         AND E.IDESEDE = p_nIdSede
       GROUP BY E.IDUSUARIO;
  
  END SP_OBTIENE_EMAIL;

END PR_INTRANET_ED;
/
