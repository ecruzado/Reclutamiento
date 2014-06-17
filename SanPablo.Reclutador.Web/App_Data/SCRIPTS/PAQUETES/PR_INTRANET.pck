create or replace package PR_INTRANET is

  type cur_cursor is REF CURSOR;
  
  --ESTADOS DE POSTULANTE
  P_PRESEL_AUTOMATC         CONSTANT VARCHAR2(2) := '02';
  P_PRESEL_MANUAL           CONSTANT VARCHAR2(2) := '03';
  P_EN_EVALUACION           CONSTANT VARCHAR2(2) := '07';
  P_ESTCAT_PENDIENTE        CONSTANT VARCHAR2(2) := '01';
  P_ESTCAT_EVALUADO         CONSTANT VARCHAR2(2) := '02';
  --TIPO DE EXAMEN - TIPCRITERIO
  P_TIPEXA_EXAMEN           CONSTANT VARCHAR2(2) := '01';
  P_TIPEXA_EVALUACION       CONSTANT VARCHAR2(2) := '04';

  FUNCTION SP_LISTA_LVAL(p_idGeneral IN VARCHAR2, p_valor IN VARCHAR2)
    RETURN VARCHAR2;

  FUNCTION FN_GETMAXPUNTAJE(p_nIdCriterio IN NUMBER) RETURN NUMBER;

  PROCEDURE SP_GETREPEXAMEN(P_NIDEXAMEN IN NUMBER,
                            p_cRetVal   OUT CUR_CURSOR);

  FUNCTION FN_DURACIONEXAMEN(p_nIdExamen IN NUMBER) RETURN NUMBER;

  FUNCTION FN_ELIMINA_ROL(p_nIdRol IN NUMBER) RETURN NUMBER;

  FUNCTION FN_ELIMINA_ROL_OPCION(p_nIdRol IN NUMBER, p_nIdOp IN NUMBER)
    RETURN NUMBER;

  PROCEDURE SP_ACTUALIZAR_PUNTAJES(p_nombreCampoDestino VARCHAR2,
                                   p_ideCargo           CARGO.IDECARGO%TYPE,
                                   p_valor              IN NUMBER,
                                   p_valorEliminar      IN NUMBER);

  PROCEDURE SP_AGREGAR_DETALLE(p_ideGeneral  IN DETALLE_GENERAL.IDEGENERAL%TYPE,
                               p_valor       IN DETALLE_GENERAL.VALOR%TYPE,
                               p_descripcion IN DETALLE_GENERAL.DESCRIPCION%TYPE,
                               p_referencia  IN DETALLE_GENERAL.REFERENCIA%TYPE,
                               p_usrCreacion IN DETALLE_GENERAL.USRCREACION%TYPE,
                               p_retVal      OUT NUMBER);

  PROCEDURE SP_PROMEDIO_EVAL_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE);

  PROCEDURE SP_OBTENER_CARGO(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                             p_ideUsuario   IN CARGO.USRCREACION%TYPE,
                             p_ideSede      IN SEDE.IDESEDE%TYPE,
                             p_cRetCursor   OUT SYS_REFCURSOR);

 /* PROCEDURE SP_CONSULTAR_DATOS_AREA(p_ideArea          IN AREA.IDEAREA%TYPE,
                                    p_ideDepartamento  IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                    p_ideDependencia   IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                    p_ideSede          IN SEDE.IDESEDE%TYPE,
                                    p_cRetCursor       OUT SYS_REFCURSOR);*/

  PROCEDURE SP_OBTENER_COMPETENCIAREMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                       p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_OFRECIMIENTO_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                         p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_HORARIO_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_UBIGEO_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                   p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_NIVELACAD_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                      p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_CENT_EST_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                     p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_CONOCIMIENTO_REMP(p_ideSolicitud     IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                         p_tipoConocimiento IN VARCHAR2,
                                         p_cRetCursor       OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_EXPR_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                 p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_EVAL_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                 p_cRetCursor   OUT SYS_REFCURSOR);

  PROCEDURE SP_OBTENER_DISCAP_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                   p_cRetCursor   OUT SYS_REFCURSOR);

  FUNCTION FN_GET_ROL(p_idUsuario IN NUMBER) RETURN VARCHAR2;

  FUNCTION FN_GET_SEDE(p_idUsuario IN NUMBER) RETURN VARCHAR2;

  PROCEDURE FN_GET_ROLXUSUARIO(p_nIdUsua IN NUMBER,
                               p_cRetVal OUT CUR_CURSOR);

  PROCEDURE FN_GET_SEDEXUSUARIO(p_nIdUsua IN NUMBER,
                                p_nIdRol  IN NUMBER,
                                p_cRetVal OUT CUR_CURSOR);

  PROCEDURE FN_GET_OPCIONESxROL(p_nIdRol   IN NUMBER,
                                p_ctipMenu IN varchar2,
                                p_cRetVal  OUT CUR_CURSOR);

  PROCEDURE FN_GET_OPCIONESPADRExROL(p_nIdRol   IN NUMBER,
                                     p_ctipMenu IN varchar2,
                                     p_cRetVal  OUT CUR_CURSOR);

  FUNCTION FN_VALOR_GENERAL(p_tipoTabla IN GENERAL.TIPTABLA%TYPE,
                            p_valor     IN DETALLE_GENERAL.VALOR%TYPE)
    RETURN DETALLE_GENERAL.DESCRIPCION%TYPE;

  PROCEDURE FN_GET_CARGO(p_nIdCargo IN NUMBER, p_cRetVal OUT CUR_CURSOR);

  /*PROCEDURE FN_GET_LISTAREQ(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                  p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                  p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                  p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                  p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                  p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                  p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                  p_cFecIni   IN    varchar2,
                  p_cFeFin   IN     varchar2,
                  p_cRetVal         OUT CUR_CURSOR
  );*/

  PROCEDURE FN_GET_LISTAREQ2(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                             p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                             p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                             p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                             p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                             p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,
                             p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                             p_cFecIni         IN varchar2,
                             p_cFeFin          IN varchar2,
                             p_cTipSol         IN varchar2,
                             p_nIdRoL          IN NUMBER,
                             p_nIdUsuario      IN NUMBER,
                             p_nIdSede         IN NUMBER,
                             p_cRetVal         OUT CUR_CURSOR);

  PROCEDURE FN_GET_LISTACARGO(p_nIdSolicitud    IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_nIdDependencia  IN SOLNUEVO_CARGO.Idedependencia%TYPE,
                              p_nIdDepartamento IN SOLNUEVO_CARGO.Idedepartamento%TYPE,
                              p_nIdArea         in SOLNUEVO_CARGO.Idearea%TYPE,
                              p_cTipEtapa       IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE,
                              p_cTipResp        in LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                              p_cEstado         IN SOLNUEVO_CARGO.ESTACTIVO%TYPE,
                              p_cFecIni         IN varchar2,
                              p_cFeFin          IN varchar2,
                              p_cRolResp        IN ROL.IDROL%TYPE,
                              p_cUsrResponsable IN USUARIO.IDUSUARIO%TYPE,
                              p_nIdSede         IN NUMBER,
                              p_cRetVal         OUT CUR_CURSOR);
                              
  PROCEDURE SP_GET_LISTA_AMPLIACION(p_nCodCargo        IN SOLREQ_PERSONAL.CODCARGO%TYPE,
                                    p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                                    p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                                    p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                                    p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                                    p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,
                                    p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                                    p_cFecIni         IN varchar2,
                                    p_cFeFin          IN varchar2,
                                    p_cTipSol         IN varchar2,
                                    p_nIdRoL          IN NUMBER,
                                    p_nIdUsuario      IN NUMBER,
                                    p_nIdSede         IN NUMBER,
                                    p_cRetVal         OUT CUR_CURSOR);

  FUNCTION FN_ESTADO_SOLICITUD(p_idSolicitud   IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                               p_tipoSolicitud IN VARCHAR2) RETURN VARCHAR2;

  PROCEDURE SP_CARGOS_SOLICITUD(p_idSede    IN SEDE.IDESEDE%TYPE,
                                p_idUsrResp IN USUARIO.IDUSUARIO%TYPE,
                                p_idRolResp IN ROL.IDROL%TYPE,
                                p_cRetVal   OUT SYS_REFCURSOR);
                                
  PROCEDURE SP_GENERAR_EXAMEN_CAT_RECL(p_idReclutaPerso   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                        p_usuario          IN USUARIO.CODUSUARIO%TYPE,
                                        p_RetVal         OUT NUMBER);

  PROCEDURE SP_OBTENER_EXAMENES_POST(p_idReclutaPerso   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                     p_cuRetVal         OUT SYS_REFCURSOR);

  PROCEDURE SP_EXAMEN_CATEGORIA(p_ideCategoria IN CATEGORIA.IDECATEGORIA%TYPE,
                                p_cuRetVal     OUT SYS_REFCURSOR);
                                
  PROCEDURE SP_GET_IDRECLU_PERSON(p_idePostulante    IN POSTULANTE.IDEPOSTULANTE%TYPE,
                                  p_ideSede          IN SEDE.IDESEDE%TYPE,
                                  p_RetVal           OUT NUMBER);
                                  
  PROCEDURE SP_GUARDAR_ALTERNATIVA(p_ideReclutaPersona  IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                   p_ideCriterioSubCat  IN CRITERIO_X_SUBCATEGORIA.IDECRITERIOXSUBCATEGORIA%TYPE,
                                   p_ideReclPersExaCat  IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                                   p_ideAlternativa     IN ALTERNATIVA.IDEALTERNATIVA%TYPE,
                                   p_usuarioCreacion    IN USUARIO.CODUSUARIO%TYPE,
                                   p_RetVal             OUT NUMBER);
                                   
 PROCEDURE SP_OBTENER_IDECATEGORIA(p_idReclPerExaCat    IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                                   p_RetVal           OUT NUMBER);
                                   
 FUNCTION FN_OBTENER_NOMBRE_POST(p_idReclutaPersona    IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE
                                  )RETURN VARCHAR2;
                                  
 PROCEDURE SP_CONOCIMIENTOS_PUBLICA(p_ideCargo    IN CARGO.IDECARGO%TYPE,
                                    p_cRetVal     OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_CONO_SOLREQ_PUBLICA(p_ideSolReq    IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                           p_cRetVal     OUT SYS_REFCURSOR);
                                           
 PROCEDURE SP_LISTAR_USUARIOS(p_apePaterno    IN USUARIO.DSCAPEPATERNO%TYPE,
                              p_apeMaterno    IN USUARIO.DSCAPEMATERNO%TYPE,
                              p_nombres       IN USUARIO.DSCNOMBRES%TYPE,
                              p_idRol         IN ROL.IDROL%TYPE,
                              p_idSede        IN SEDE.IDESEDE%TYPE,
                              p_retVal        OUT SYS_REFCURSOR);
                                    
end PR_INTRANET;
/
create or replace package body PR_INTRANET is

  /* ------------------------------------------------------------
    Nombre      : SP_LISTA_LVAL
    Proposito   : devuelve una lista de acuerdo al valor del tipo del lval
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  FUNCTION SP_LISTA_LVAL(p_idGeneral IN VARCHAR2, p_valor IN VARCHAR2)
    return VARCHAR2 IS
  
    cDato varchar2(1000);
  BEGIN
  
    BEGIN
      SELECT G.DESCRIPCION AS DESCRIPCION
        INTO cDato
        FROM DETALLE_GENERAL G
       WHERE G.IDEGENERAL = P_IDGENERAL
         AND G.VALOR = P_VALOR;
    EXCEPTION
      WHEN OTHERS THEN
        cDato := null;
    END;
  
    RETURN cDato;
  
  END SP_LISTA_LVAL;

  /* ------------------------------------------------------------
    Nombre      : FN_GETMAXPUNTAJE
    Proposito   : devuelve el maximo puntaje del criterio
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  FUNCTION FN_GETMAXPUNTAJE(p_nIdCriterio IN NUMBER) RETURN NUMBER IS
    nMaxPuntaje NUMBER;
  BEGIN
  
    BEGIN
    
      SELECT MAX(A.PESO) AS MAXPUNTAJE
        INTO nMaxPuntaje
        FROM ALTERNATIVA A
       WHERE A.IDECRITERIO = P_NIDCRITERIO;
    
    EXCEPTION
      WHEN OTHERS THEN
        nMaxPuntaje := 0;
    END;
  
    RETURN nMaxPuntaje;
  
  end FN_GETMAXPUNTAJE;

  /* ------------------------------------------------------------
    Nombre      : FN_GETREPEXAMEN
    Proposito   : devuelve los datos para el reporte del PDF
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_GETREPEXAMEN(P_NIDEXAMEN IN NUMBER,
                            p_cRetVal   OUT CUR_CURSOR) IS
  
  BEGIN
  
    OPEN p_cRetVal FOR
      SELECT NVL(EX1.IDEEXAMEN,0) IDEEXAMEN,
             NVL(EX1.IDESEDE,0) IDESEDE,
             nvl(EX1.NOMEXAMEN,'') NOMEXAMEN,
             nvl(EX1.DESCEXAMEN,'') DESCEXAMEN,
             NVL(S1.IDESUBCATEGORIA,0) IDESUBCATEGORIA,
             NVL(C1.IDECATEGORIA,0) IDECATEGORIA,
             upper(nvl(C1.NOMCATEGORIA,'')) NOMCATEGORIA,
             upper(nvl(C1.DESCCATEGORIA,'')) DESCCATEGORIA,
             nvl (C1.TIPCATEGORIA,'') TIPCATEGORIA,
             (SELECT D.DESCRIPCION FROM DETALLE_GENERAL D WHERE D.IDEGENERAL = 1
              AND D.VALOR = C1.TIPCATEGORIA) DESTIPCATEGORIA,
             
             nvl(C1.INSTRUCCIONES,'') INSTRUCCIONES,
             nvl(C1.TIPOEJEMPLO,'') TIPOEJEMPLO,
             C1.IMAGENEJEMPLO,
             C1.TEXTOEJEMPLO,
             S1.DESCSUBCATEGORIA,
             nvl(S1.NOMSUBCATEGORIA,'') NOMSUBCATEGORIA,
             NVL(A1.IDECRITERIO,0) IDECRITERIO,
             NVL(A1.IDEALTERNATIVA,0) IDEALTERNATIVA,
             NVL(CR1.TIPMODO,'') AS CODMOD,
             DECODE(CR1.TIPMODO, '01', 'TEXTO', '02', 'IMAGE') AS DESMODO,
             NVL(CR1.TIPCRITERIO,'') TIPCRITERIO,
             nvl(CR1.PREGUNTA,'') PREGUNTA,
             CR1.IMAGENCRIT,
             A1.IMAGE,
             NVL(A1.ALTERNATIVA,'') ALTERNATIVA,
             NVL(A1.ESTACTIVO,'') ESTACTIVO,
             NVL(s1.ordenimpresion,0) as ORDENSUB,
             NVL(CS1.PRIORIDAD,0) AS ORDENCRIT,
             (SELECT NVL(SUM(S.TIEMPO), 0)
                FROM SUBCATEGORIA S
               WHERE S.IDECATEGORIA = C1.IDECATEGORIA) AS TIEMPOCAT,
             PR_INTRANET.FN_DURACIONEXAMEN(ex1.ideexamen) AS TIMPOEXAMEN
        FROM CRITERIO         CR1,
             ALTERNATIVA             A1,
             CRITERIO_X_SUBCATEGORIA CS1,
             SUBCATEGORIA            S1,
             CATEGORIA               C1,
             EXAMEN_X_CATEGORIA      EC1,
             Examen                  EX1
       WHERE/* ex1.ideexamen = EC1.Ideexamen
         and EC1.Idecategoria = c1.idecategoria
         and C1.IDECATEGORIA = S1.IDECATEGORIA
         --AND C1.TIPCATEGORIA = CR1.TIPCRITERIO
         AND CS1.IDESUBCATEGORIA = S1.IDESUBCATEGORIA
         AND CR1.IDECRITERIO = CS1.IDECRITERIO
         AND A1.IDECRITERIO = CR1.IDECRITERIO
         and c1.estactivo = 'A'
         AND A1.ESTACTIVO = 'A'
         AND CR1.ESTACTIVO = 'A'
         AND CS1.ESTREGISTRO = 'A'
         and ec1.estactivo = 'A'
         and EX1.Estactivo = 'A'*/
         ex1.ideexamen = EC1.Ideexamen(+)
         and EC1.Idecategoria = c1.idecategoria(+)
         and C1.IDECATEGORIA = S1.IDECATEGORIA(+)
        
         AND S1.IDESUBCATEGORIA = CS1.IDESUBCATEGORIA(+)
         AND CS1.IDECRITERIO = CR1.IDECRITERIO(+)
         AND CR1.IDECRITERIO = A1.IDECRITERIO(+)
         AND C1.ESTACTIVO = 'A'
         AND CR1.ESTACTIVO = 'A'
         AND CS1.ESTREGISTRO = 'A'
         AND EC1.ESTACTIVO = 'A'
         AND EX1.ESTACTIVO = 'A'
         and ex1.ideexamen = P_NIDEXAMEN
       ORDER BY c1.idecategoria,
                s1.ordenimpresion,
                CS1.Prioridad,
                A1.IDEALTERNATIVA DESC;
  
  END SP_GETREPEXAMEN;

  /* ------------------------------------------------------------
    Nombre      : FN_DURACIONEXAMEN
    Proposito   : devuelve el tiempo de examen total en min
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
  FUNCTION FN_DURACIONEXAMEN(p_nIdExamen IN NUMBER) RETURN NUMBER IS
  
    nCont   Number;
    nTiempo Number;
    CURSOR cData IS
      select c.idecategoria
        from examen_x_categoria c
       where c.ideexamen = p_nIdExamen;
  
  BEGIN
    nCont   := 0;
    nTiempo := 0;
  
    FOR C1 IN cData LOOP
      BEGIN
        SELECT NVL(SUM(S.TIEMPO), 0)
          INTO nTiempo
          FROM SUBCATEGORIA S
         WHERE S.IDECATEGORIA = C1.IDECATEGORIA;
      EXCEPTION
        WHEN OTHERS THEN
          nTiempo := 0;
      END;
    
      nCont := nCont + nTiempo;
    
    END LOOP;
  
    RETURN nvl(nCont, 0);
  
  END;

  /* ------------------------------------------------------------
    Nombre      : FN_ELIMINA_ROL
    Proposito   : Elimina el rol si no depende de un usuario o sede
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
  FUNCTION FN_ELIMINA_ROL(p_nIdRol IN NUMBER) RETURN NUMBER IS
    nDato NUMBER;
  BEGIN
  
    delete from ROLOPCION where idrol = p_nIdRol;
    delete from rol r where r.idrol = p_nIdRol;
    commit;
  
    nDato := 1;
  
    RETURN nDato;
  
  END FN_ELIMINA_ROL;

  /* ------------------------------------------------------------
    Nombre      : FN_ELIMINA_ROL_OPCION
    Proposito   : Elimina la opcion asociada ala rol
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
  FUNCTION FN_ELIMINA_ROL_OPCION(p_nIdRol IN NUMBER, p_nIdOp IN NUMBER)
    RETURN NUMBER IS
    nDato NUMBER;
  BEGIN
  
    delete from ROLOPCION r
     where r.idrol = p_nIdRol
       and r.idopcion = p_nIdOp;
    commit;
  
    nDato := 1;
  
    RETURN nDato;
  
  END FN_ELIMINA_ROL_OPCION;

  /* ------------------------------------------------------------
    Nombre      : SP_ACTUALIZAR_PUNTAJES
    Proposito   : Actualizar los puntajes en la tabla cargo 
                  al modificar tablas asociadas
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_ACTUALIZAR_PUNTAJES(p_nombreCampoDestino IN VARCHAR2,
                                   p_ideCargo           IN CARGO.IDECARGO%TYPE,
                                   p_valor              IN NUMBER,
                                   p_valorEliminar      IN NUMBER) IS
  
    consulta01 VARCHAR2(100);
    consulta02 VARCHAR2(100);
  
  BEGIN
  
    consulta01 := 'SELECT ' || p_nombreCampoDestino || ' FROM CARGO C ' ||
                  ' WHERE C.IDECARGO = ' || p_ideCargo || ' FOR UPDATE';
    consulta02 := 'UPDATE CARGO C SET ' || p_nombreCampoDestino || ' = ' ||
                  p_nombreCampoDestino || ' + ' || p_valor || ' - ' ||
                  p_valorEliminar || ' WHERE C.IDECARGO = ' || p_ideCargo;
  
    EXECUTE IMMEDIATE consulta01;
  
    EXECUTE IMMEDIATE consulta02;
  
    commit;
  
  END SP_ACTUALIZAR_PUNTAJES;

  /* ------------------------------------------------------------
    Nombre      : SP_AGREGAR_DETALLE
    Proposito   : Agregar detalle general 
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      20/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_AGREGAR_DETALLE(p_ideGeneral  IN DETALLE_GENERAL.IDEGENERAL%TYPE,
                               p_valor       IN DETALLE_GENERAL.VALOR%TYPE,
                               p_descripcion IN DETALLE_GENERAL.DESCRIPCION%TYPE,
                               p_referencia  IN DETALLE_GENERAL.REFERENCIA%TYPE,
                               p_usrCreacion IN DETALLE_GENERAL.USRCREACION%TYPE,
                               p_retVal      OUT NUMBER) IS
  
  BEGIN
  
    BEGIN
      INSERT INTO DETALLE_GENERAL
        (IDEGENERAL,
         VALOR,
         DESCRIPCION,
         ESTACTIVO,
         USRCREACION,
         FECCREACION,
         REFERENCIA)
      VALUES
        (p_ideGeneral,
         p_valor,
         p_descripcion,
         'A',
         p_usrCreacion,
         SYSDATE,
         p_referencia);
    
      commit;
      p_retVal := 1; --CORRECTO
    EXCEPTION
      WHEN OTHERS THEN
        ROLLBACK;
        p_retVal := 0; --INCORRECTO
    END;
  
  END SP_AGREGAR_DETALLE;

  /* ------------------------------------------------------------
    Nombre      : SP_PROMEDIO_EVAL_CARGO
    Proposito   : Actualizar los puntajes en la tabla cargo 
                  al modificar tablas asociadas
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_PROMEDIO_EVAL_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE) IS
  
    c_promedio EVALUACION_CARGO.PUNTEXAMEN%TYPE;
    c_Suma     number;
    c_contador number;
  
    CURSOR c_puntajes(p_idCargo CARGO.IDECARGO%TYPE) IS
      SELECT EC.NOTAMINEXAMEN
        FROM EVALUACION_CARGO EC
       WHERE EC.IDECARGO = p_idCargo;
  
  BEGIN
    c_contador := 0;
    c_Suma     := 0;
    c_promedio := 0;
    BEGIN
      FOR REG_PUNTAJES IN c_puntajes(p_ideCargo) LOOP
        c_contador := c_contador + 1;
        c_Suma     := c_Suma + REG_PUNTAJES.NOTAMINEXAMEN;
      END LOOP;
    
      IF (c_contador <> 0) THEN
        c_promedio := c_Suma / c_contador;
      END IF;
    
      UPDATE CARGO C
         SET C.PUNTTOTEXAMEN = c_promedio
       WHERE C.IDECARGO = p_ideCargo;
    
    EXCEPTION
      WHEN OTHERS THEN
        ROLLBACK;
    END;
  
  END SP_PROMEDIO_EVAL_CARGO;

  /* ------------------------------------------------------------
    Nombre      : SP_CREAR_CARGO
    Proposito   : Verificar si el cargo esta creado  o crear el
                  registro y recuperar datos.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_CARGO(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                             p_ideUsuario   IN CARGO.USRCREACION%TYPE,
                             p_ideSede      IN SEDE.IDESEDE%TYPE,
                             p_cRetCursor   OUT SYS_REFCURSOR) IS
  
    nroCont          NUMBER;
    nCodCargo        CARGO.CODCARGO%TYPE;
    nIdeCargo        CARGO.IDECARGO%TYPE;
    nNomCargo        CARGO.NOMCARGO%TYPE;
    nDescCargo       CARGO.DESCARGO%TYPE;
    nIdeArea         CARGO.IDEAREA%TYPE;
    nIdeDepartamento CARGO.IDEDEPARTAMENTO%TYPE;
    nIdeDependencia  CARGO.IDEDEPENDENCIA%TYPE;
    nNumPosic        SOLNUEVO_CARGO.NUMPOSICIONES%TYPE;
    nIdeCargo_sq     CARGO.IDECARGO%TYPE;
    nTipoRangoSal    CARGO.TIPRANGOSALARIO%TYPE;
  
  BEGIN
  
    SELECT SN.CODCARGO,
           SN.NOMBRE,
           SN.DESCRIPCION,
           SN.IDEAREA,
           SN.NUMPOSICIONES,
           SN.IDEDEPENDENCIA,
           SN.IDEDEPARTAMENTO,
           SN.TIPRANSALARIO,
           NVL(SN.IDECARGO,0) 
      INTO nCodCargo,
           nNomCargo,
           nDescCargo,
           nIdeArea,
           nNumPosic,
           nIdeDependencia,
           nIdeDepartamento,
           nTipoRangoSal,
           nIdeCargo
      FROM SOLNUEVO_CARGO SN
     WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud;
     
  
    BEGIN
      IF (nIdeCargo = 0) THEN
        SELECT IDECARGO_SQ.NEXTVAL INTO nIdeCargo_sq FROM DUAL;
        INSERT INTO CARGO
          (IDECARGO,IDESEDE,CODCARGO,NOMCARGO,DESCARGO,IDEAREA,NUMPOSICION,
           ESTACTIVO,USRCREACION,FECCREACION,PUNTTOTEDAD,
           PUNTTOTSEXO,PUNTTOTSALARIO,PUNTTOTNIVELEST,PUNTTOTCENTROEST,
           PUNTTOTEXPLABORAL,PUNTTOTOFIMATI,PUNTTOTIDIOMA,PUNTTOTCONOGEN,
           PUNTTOTDISCAPA,PUNTTOTHORARIO,PUNTTOTUBIGEO,PUNTTOTEXAMEN,PUNTMINEXAMEN,
           IDEDEPENDENCIA,IDEDEPARTAMENTO,TIPRANGOSALARIO,VERSION)
        VALUES
          (nIdeCargo_sq,p_ideSede,nCodCargo,nNomCargo,nDescCargo,nIdeArea,nNumPosic,
           'A',p_ideUsuario,SYSDATE,0,
           0, 0, 0, 0,
           0, 0, 0, 0,
           0, 0, 0, 0, 0,
           nIdeDependencia,nIdeDepartamento, nTipoRangoSal,1);
      
        nIdeCargo := nIdeCargo_sq;
        
        UPDATE SOLNUEVO_CARGO SN
           SET SN.IDECARGO = nIdeCargo_sq
         WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud;
        COMMIT;
      END IF;
    
    EXCEPTION
      WHEN OTHERS THEN
        ROLLBACK;
    END;
  
    OPEN p_cRetCursor FOR
      SELECT C.IDECARGO,
             UPPER(C.CODCARGO) CODCARGO,
             UPPER(C.NOMCARGO) NOMCARGO,
             UPPER(C.DESCARGO) DESCARGO,
             C.NUMPOSICION,
             AR.NOMAREA,
             D.NOMDEPARTAMENTO,
             DE.NOMDEPENDENCIA,
             nNumPosic NUMPOSIC
        FROM CARGO C, AREA AR, DEPARTAMENTO D,(SELECT * FROM DEPENDENCIA DE WHERE DE.IDESEDE = p_ideSede)DE
       WHERE C.IDECARGO = nIdeCargo
         AND AR.IDEDEPARTAMENTO = D.IDEDEPARTAMENTO
         AND D.IDEDEPENDENCIA = DE.IDEDEPENDENCIA
         AND AR.IDEAREA = nIdeArea
         and C.IDESEDE = p_ideSede;
  
  END SP_OBTENER_CARGO;

  /* ------------------------------------------------------------
    Nombre      : SP_CONSULTAR_DATOS_AREA
    Proposito   : Recuperarr los datos de area, departamento y dependencia
                  del usuario del sistema.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  /*PROCEDURE SP_CONSULTAR_DATOS_AREA(p_ideArea          IN AREA.IDEAREA%TYPE,
                                    p_ideDepartamento  IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                    p_ideDependencia   IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                    p_ideSede          IN SEDE.IDESEDE%TYPE,
                                    p_cRetCursor       OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT AR.IDEAREA,
             AR.NOMAREA,
             DE.IDEDEPENDENCIA,
             DE.NOMDEPENDENCIA,
             DP.IDEDEPARTAMENTO,
             DP.NOMDEPARTAMENTO
        FROM (SELECT AR.IDEAREA, AR.NOMAREA,AR.IDEDEPARTAMENTO  
              FROM AREA AR 
              WHERE AR.IDEAREA = p_ideArea) AR, 
             (SELECT DE.IDEDEPENDENCIA, DE.NOMDEPENDENCIA 
              FROM  DEPENDENCIA DE
              WHERE DE.IDEDEPENDENCIA = p_ideDependencia
              AND DE.IDESEDE = p_ideSede)DE, 
             (SELECT DP.IDEDEPARTAMENTO, DP.NOMDEPARTAMENTO, DP.IDEDEPENDENCIA 
              FROM DEPARTAMENTO DP
              WHERE DP.IDEDEPARTAMENTO = p_ideDepartamento) DP
       WHERE AR.IDEDEPARTAMENTO = DP.IDEDEPARTAMENTO
         AND DP.IDEDEPENDENCIA = DE.IDEDEPENDENCIA
         AND AR.IDEAREA = p_ideArea;
  END SP_CONSULTAR_DATOS_AREA;*/

  /* ------------------------------------------------------------
    Nombre      : SP_DATOS_COMPETENCIA_REMP
    Proposito   : Recuperar los datos competencias de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_COMPETENCIAREMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                       p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT CS.IDECOMPETENCIASOLREQ,
             PR_INTRANET.FN_VALOR_GENERAL('TIPCOMPETENCIA', CS.TIPCOMPETEN) DESCRIPCION
        FROM COMPETENCIAS_SOLREQ CS
       WHERE CS.ESTACTIVO = 'A'
         AND CS.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_COMPETENCIAREMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_OFRECIMIENTO_REMP
    Proposito   : Recuperar los datos competencias de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_OFRECIMIENTO_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                         p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT OS.IDEOFRECEMOSSOLREQ,
             PR_INTRANET.FN_VALOR_GENERAL('TIPOFRECIMIENTO',
                                          OS.TIPOFRECIMIENTO) DESCRIPCION
        FROM OFRECEMOS_SOLREQ OS
       WHERE OS.ESTACTIVO = 'A'
         AND OS.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_OFRECIMIENTO_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_HORARIO_REMP
    Proposito   : Recuperar los datos competencias de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_HORARIO_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT HS.IDEHORARIOSOLREQ,
             PR_INTRANET.FN_VALOR_GENERAL('TIPHORARIO', HS.TIPHORARIO) DESCRIPCION,
             HS.PUNTHORARIO
        FROM HORARIO_SOLREQ HS
       WHERE HS.ESTACTIVO = 'A'
         AND HS.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_HORARIO_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_UBIGEO_REMP
    Proposito   : Recuperar los datos de ubigeo de una solicitud 
                  de requerimiento de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_UBIGEO_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                   p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
  
    OPEN p_cRetCursor FOR
      SELECT US.IDEUBIGEOSOLREQ,
             US.IDEUBIGEO,
             UBI.DISTRITO,
             UBI.PROVINCIA,
             UBI.DEPARTAMENT,
             US.PUNTUBIGEO
        FROM UBIGEO_SOLREQ US, UBIGEODESCRIPCION UBI
       WHERE US.ESTACTIVO = 'A'
         AND US.IDEUBIGEO = UBI.IDUBIGEO
         AND US.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_UBIGEO_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_NIVELACAD_REMP
    Proposito   : Recuperar los datos de nivel academico de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_NIVELACAD_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                      p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT NA.IDENIVELACADESOLREQ,
             (PR_INTRANET.FN_VALOR_GENERAL('TIPEDUCACION', NA.TIPEDUCACION)) TIPEDUCACION,
             (PR_INTRANET.FN_VALOR_GENERAL('TIPAREA', NA.TIPAREAESTUDIO)) AREAESTUDIO,
             NVL((PR_INTRANET.FN_VALOR_GENERAL('NIVELALCANZADO',
                                           NA.TIPNIVELCANZADO)),'') NIVELALCANZADO,
             NVL(NA.CICLOSEMESTRE,0) CICLOSEMESTRE,
             NVL(NA.PUNTNIVESTUDIO,0) PUNTNIVESTUDIO
        FROM NIVELACADEMICO_SOLREQ NA
       WHERE NA.ESTACTIVO = 'A'
         AND NA.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_NIVELACAD_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_CENT_EST_REMP
    Proposito   : Recuperar los datos de centro de estudios de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_CENT_EST_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                     p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT CE.IDECENTESTSOLREQ,
             (PR_INTRANET.FN_VALOR_GENERAL('TIPTIPINSTIT', CE.TIPCENESTU)) TIPINST,
             (PR_INTRANET.FN_VALOR_GENERAL('TIPTIPINSTIT', CE.TIPNOMCENESTU)) NOMBINST,
             CE.PUNTACENTROEST
        FROM CENTROEST_SOLREQ CE
       WHERE CE.ESTACTIVO = 'A'
         AND CE.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_CENT_EST_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_CONOCIMIENTO_REMP
    Proposito   : Recuperar los datos de conocimientos de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_CONOCIMIENTO_REMP(p_ideSolicitud     IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                         p_tipoConocimiento IN VARCHAR2,
                                         p_cRetCursor       OUT SYS_REFCURSOR) IS
  
    c_condicion VARCHAR2(100);
    c_Consulta  VARCHAR2(1000);
  
  BEGIN
    IF (p_tipoConocimiento = 'OFIMATICA') THEN
      c_condicion := 'CG.TIPCONOFIMATICA IS NOT NULL';
    ELSIF (p_tipoConocimiento = 'IDIOMA') THEN
      c_condicion := 'CG.TIPIDIOMA IS NOT NULL';
    ELSIF (p_tipoConocimiento = 'GENERAL') THEN
      c_condicion := 'CG.TIPCONOGENERAL IS NOT NULL';
    END IF;
  
    c_Consulta := 'SELECT CG.IDECONOGENSOLREQ,(PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOFIMATICA'',CG.TIPCONOFIMATICA)) OFIMATICA, (PR_INTRANET.FN_VALOR_GENERAL(''TIPNOMOFIMATICA'',CG.TIPNOMOFIMATICA)) DESCOFIMATICA,
                 (PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOIDIOMA'',CG.TIPCONOCIDIOMA)) CONOIDIOMA,(PR_INTRANET.FN_VALOR_GENERAL(''TIPIDIOMA'',CG.TIPIDIOMA)) IDIOMA,
                 (PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOGRALES'',CG.TIPCONOGENERAL)) GENERAL, (PR_INTRANET.FN_VALOR_GENERAL(''TIPCONOGRALES'',CG.TIPNOMCONOCGRALES)) DESCGENERAL,CG.PUNTCONOCIMIENTO,
                 (PR_INTRANET.FN_VALOR_GENERAL(''TIPNIVELALCAN'',CG.TIPNIVELCONOCIMIENTO)) NIVELCONO
                 FROM CONOGENERAL_SOLREQ CG
                 WHERE CG.ESTACTIVO = ''A'' 
                 AND CG.IDESOLREQPERSONAL = ' ||
                  p_ideSolicitud || ' AND ' || c_condicion;
  
    OPEN p_cRetCursor FOR c_Consulta;
  
  END SP_OBTENER_CONOCIMIENTO_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_EXPR_REMP
    Proposito   : Recuperar los datos de experiencia de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      03/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_EXPR_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                 p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT ES.IDEEXPSOLREQ,
             PR_INTRANET.FN_VALOR_GENERAL('TIPCARGO', ES.TIPEXPLABORAL) EXPERIENCIA,
             ES.CANTANHOEXP,
             ES.CANTMESESEXP,
             ES.PUNTEXPERIENCIA
        FROM EXPERIENCIA_SOLREQ ES
       WHERE ES.ESTACTIVO = 'A'
         AND ES.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_EXPR_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_EVAL_REMP
    Proposito   : Recuperar los datos de evaluaciones de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_EVAL_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                 p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT EV.IDEEVALUACIONSOLREQ,
             EX.NOMEXAMEN,
             EX.DESCEXAMEN,
             EV.PUNTEXAMEN,
             EV.NOTAMINEXAMEN,
             PR_INTRANET.FN_VALOR_GENERAL('TIPOCRITERIO', EV.TIPEXAMEN) TIPOEXAMEN
        FROM EVALUACION_SOLREQ EV, EXAMEN EX
       WHERE EV.ESTACTIVO = 'A'
         AND EV.IDEEXAMEVAL = EX.IDEEXAMEN
         AND EV.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_EVAL_REMP;

  /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_DISCAP_REMP
    Proposito   : Recuperar los datos de discapacidades de una solicitud
                  requerimiento de solicitud de personal.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      04/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE SP_OBTENER_DISCAP_REMP(p_ideSolicitud IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                   p_cRetCursor   OUT SYS_REFCURSOR) IS
  BEGIN
    OPEN p_cRetCursor FOR
      SELECT DR.IDEDISCAPASOLREQ,
             PR_INTRANET.FN_VALOR_GENERAL('TIPDISCAPACIDAD', DR.TIPDISCAPA) DESCDISCAP,
             DR.PUNTDISCAPA
        FROM DISCAPACIDAD_SOLREQ DR
       WHERE DR.ESTACTIVO = 'A'
         AND DR.IDESOLREQPERSONAL = p_ideSolicitud;
  
  END SP_OBTENER_DISCAP_REMP;

  /* ------------------------------------------------------------
    Nombre      : FN_GET_ROL
    Proposito   : Obtiene los roles por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
  FUNCTION FN_GET_ROL(p_idUsuario IN NUMBER) RETURN VARCHAR2 IS
    rol  varchar2(2000);
    cont number;
  
    CURSOR cData IS
      SELECT DISTINCT UR.IDROL,
                      (SELECT R.DSCROL FROM ROL R WHERE R.IDROL = UR.IDROL) DSCROL
        FROM USUAROLSEDE UR
       WHERE UR.IDUSUARIO = p_idUsuario;
  
  BEGIN
    cont := 1;
    FOR C1 IN cData LOOP
    
      BEGIN
        IF cont = 1 THEN
          rol := rol || C1.DSCROL;
        ELSE
          rol := rol || ', ' || C1.DSCROL;
        END IF;
        cont := cont + 1;
      
      EXCEPTION
        WHEN OTHERS THEN
          rol := null;
      END;
    
    END LOOP;
  
    RETURN NVL(rol, '');
  
  END FN_GET_ROL;

  /* ------------------------------------------------------------
    Nombre      : FN_GET_SEDE
    Proposito   : Obtiene las sedes por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  FUNCTION FN_GET_SEDE(p_idUsuario IN NUMBER) RETURN VARCHAR2 IS
    cSede varchar2(3000);
    cont  number;
  
    CURSOR cData IS
      SELECT UR.Idesede,
             (SELECT R.DESCRIPCION FROM SEDE R WHERE R.Idesede = UR.Idesede) DSCSEDE
        FROM USUAROLSEDE UR
       WHERE UR.IDUSUARIO = p_idUsuario;
  
  BEGIN
    cont := 1;
    FOR C1 IN cData LOOP
    
      BEGIN
      
        IF C1.DSCSEDE IS NOT NULL THEN
          IF cont = 1 THEN
            cSede := cSede || C1.DSCSEDE;
          ELSE
            cSede := cSede || ', ' || C1.DSCSEDE;
          END IF;
          cont := cont + 1;
        END IF;
      
      EXCEPTION
        WHEN OTHERS THEN
          cSede := null;
      END;
    
    END LOOP;
  
    RETURN NVL(cSede, '');
  
  END FN_GET_SEDE;

  /* ------------------------------------------------------------
    Nombre      : FN_GET_ROLXUSUARIO
    Proposito   : Obtiene los roles por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE FN_GET_ROLXUSUARIO(p_nIdUsua IN NUMBER,
                               p_cRetVal OUT CUR_CURSOR) IS
  BEGIN
    OPEN p_cRetVal FOR
      SELECT DISTINCT U.IDROL,
                      (SELECT TRIM(R.CODROL)
                         FROM ROL R
                        WHERE R.IDROL = U.IDROL) CODIGOROL
        FROM USUAROLSEDE U
       WHERE (p_nIdUsua = 0 OR U.IDUSUARIO = p_nIdUsua)
      
       order by U.IDROL;
  
  END FN_GET_ROLXUSUARIO;

  /* ------------------------------------------------------------
    Nombre      : FN_GET_SEDEXUSUARIO
    Proposito   : Obtiene los roles por usuario
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE FN_GET_SEDEXUSUARIO(p_nIdUsua IN NUMBER,
                                p_nIdRol  IN NUMBER,
                                p_cRetVal OUT CUR_CURSOR) IS
  BEGIN
    OPEN p_cRetVal FOR
      SELECT E.DESCRIPCION, E.IDESEDE
        FROM USUAROLSEDE S, SEDE E
       WHERE S.IDESEDE = E.IDESEDE
         AND S.IDUSUARIO = p_nIdUsua
         AND S.IDROL = p_nIdRol
         AND S.IDESEDE <> 0
         AND S.IDESEDE IS NOT NULL
         AND E.ESTREGISTRO = 'A';
  
  END FN_GET_SEDEXUSUARIO;

  /* ------------------------------------------------------------
    Nombre      : FN_GET_OPCIONESxROL
    Proposito   : Obtiene las copciones de menu por rol
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE FN_GET_OPCIONESxROL(p_nIdRol   IN NUMBER,
                                p_ctipMenu IN varchar2,
                                p_cRetVal  OUT CUR_CURSOR) IS
  BEGIN
  
    OPEN P_CRETVAL FOR
      SELECT OP.IDOPCIONPADRE,
             OP.IDOPCION,
             OP.DESCRIPCION,
             OP.DSCURL,
             R.IDROL,
             OP.TIPMENU
        FROM OPCIONES OP, ROLOPCION R
       WHERE OP.FLGHABILITADO = 'A'
         AND R.IDOPCION = OP.IDOPCION
         AND R.IDROL = P_NIDROL
         and OP.TIPMENU = p_ctipMenu
       ORDER BY OP.IDOPCIONPADRE, OP.IDOPCION;
  
  END FN_GET_OPCIONESxROL;
  /* ------------------------------------------------------------
    Nombre      : FN_GET_OPCIONESPADRExROL
    Proposito   : Obtiene las opciones padre
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

  PROCEDURE FN_GET_OPCIONESPADRExROL(p_nIdRol   IN NUMBER,
                                     p_ctipMenu IN varchar2,
                                     p_cRetVal  OUT CUR_CURSOR) IS
  BEGIN
  
    OPEN P_CRETVAL FOR
      SELECT DISTINCT OP.IDOPCIONPADRE,
                      (SELECT P.DESCRIPCION
                         FROM OPCIONES P
                        WHERE P.IDOPCIONPADRE = OP.IDOPCIONPADRE
                          AND P.IDOPCION IS NULL
                          AND P.TIPMENU = p_ctipMenu) DESCRIPCION,
                      OP.TIPMENU
        FROM OPCIONES OP, ROLOPCION R
       WHERE OP.FLGHABILITADO = 'A'
         AND R.IDOPCION = OP.IDOPCION
         AND R.IDROL = p_nIdRol
         and OP.TIPMENU = p_ctipMenu
       ORDER BY OP.IDOPCIONPADRE;
  
  END FN_GET_OPCIONESPADRExROL;

  /* ------------------------------------------------------------
    Nombre      : FN_VALOR_GENERAL
    Proposito   : OBTIENE LA DESCRIPCION DE LA TABLA DETALLE GENERAL
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana      Creaci?n    
  ------------------------------------------------------------ */

  FUNCTION FN_VALOR_GENERAL(p_tipoTabla IN GENERAL.TIPTABLA%TYPE,
                            p_valor     IN DETALLE_GENERAL.VALOR%TYPE)
    RETURN DETALLE_GENERAL.DESCRIPCION%TYPE IS
  
    cDescripcion DETALLE_GENERAL.DESCRIPCION%TYPE;
  
  BEGIN
  
    BEGIN
      SELECT DG.DESCRIPCION
        INTO cDescripcion
        FROM DETALLE_GENERAL DG
       WHERE DG.IDEGENERAL =
             (SELECT G.IDEGENERAL
                FROM GENERAL G
               WHERE G.TIPTABLA = p_tipoTabla)
         AND DG.VALOR = p_valor;
    
    EXCEPTION
      WHEN OTHERS THEN
        cDescripcion := '';
    END;
  
    RETURN NVL(cDescripcion, '');
  END FN_VALOR_GENERAL;

  /* ------------------------------------------------------------
    Nombre      : FN_GET_CARGO
    Proposito   : Obtiene la lista de cargos o el cargo por el id de cargo
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE FN_GET_CARGO(p_nIdCargo IN NUMBER, p_cRetVal OUT CUR_CURSOR) IS
  BEGIN
  
    OPEN P_CRETVAL FOR
       SELECT DISTINCT C.IDECARGO, C.NOMCARGO
        FROM CARGO C
       WHERE C.ESTACTIVO = 'A'
         AND NVL(C.VERSION,1) = (SELECT NVL(MAX(C1.VERSION),1) FROM CARGO C1 WHERE C1.ESTACTIVO='A'
         AND C1.CODCARGO = C.CODCARGO
         )
       ORDER BY C.NOMCARGO;

  
  END FN_GET_CARGO;


  


  /* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de requerimiento
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
  /* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de requerimiento
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
  /*PROCEDURE FN_GET_LISTAREQ(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                            p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                            p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                            p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                            p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                            p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                            p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                            p_cFecIni   IN    varchar2,
                            p_cFeFin   IN     varchar2,
                            p_cRetVal         OUT CUR_CURSOR
            )IS
            
  cCONSULTA VARCHAR2(2000):=NULL;
  cWhere VARCHAR2(1000):=NULL;
  cQUERY VARCHAR2(4000):=NULL;
            
  BEGIN
    
                   
     cCONSULTA:='SELECT DISTINCT S.IDESOLREQPERSONAL,S.CODSOLREQPERSONAL,S.IDECARGO, ' 
     || ' (SELECT C.NOMCARGO ' 
     || ' FROM CARGO C '
     || ' WHERE C.IDECARGO=S.IDECARGO '
     || ' AND C.IDESEDE=S.IDESEDE) DESCARGO, '
     ||  ' S.IDEDEPENDENCIA, '
     || ' (SELECT D.NOMDEPENDENCIA '
     || ' FROM DEPENDENCIA D '
     || ' WHERE D.IDEDEPENDENCIA = S.IDEDEPENDENCIA '
     || ' AND D.IDESEDE=S.IDESEDE) DESDEPENDENCIA, '
     || ' S.IDEDEPARTAMENTO, '
     || ' (SELECT E.NOMDEPARTAMENTO  '
     || '  FROM DEPARTAMENTO E '
     || '  WHERE E.IDEDEPARTAMENTO=S.IDEDEPARTAMENTO '
     || '  AND E.IDEDEPENDENCIA = S.IDEDEPENDENCIA) DESDEPARTAMENTO, '
     || ' S.IDEAREA, '
     || ' (SELECT A.NOMAREA '
     || '  FROM AREA A '
     || '  WHERE A.IDEAREA = S.IDEAREA '
     || '  AND A.IDEDEPARTAMENTO = S.IDEDEPARTAMENTO) DESAREA, '
     || ' S.NUMVACANTES, '
     || ' 0 POSTULANTE, '
     || ' 0 PRESELECCIONADOS, '
     || ' 0 EVALUADOS, '
     || ' 0 SELECCIONADOS , '
     || ' L.ROLRESPONSABLE ROL , '
     || ' (select codRol from rol r where r.flgestado=''A'' and r.idrol =L.ROLRESPONSABLE ) DESROL, '
     || ' S.ESTACTIVO, '
     || ' L.TIPETAPA, '
     || ' S.FECPUBLICACION, '
     || ' S.FECCREACION, '
     || ' S.FECEXPIRACACION, '
     || ' L.USRESPONSABLE ID_USUARIO_RESP,'
     || ' (SELECT U.CODUSUARIO FROM USUARIO U WHERE U.IDUSUARIO = L.USRESPONSABLE AND ROWNUM<2) NOMPERSONREEMPLAZO,'
     || ' DECODE(S.FECPUBLICACION,NULL,''NO'',''SI'') PUBLICADO '
     || '  FROM SOLREQ_PERSONAL S,LOGSOLREQ_PERSONAL L '
     || '  WHERE S.IDESOLREQPERSONAL = L.IDESOLREQPERSONAL '
     || '  AND L.FECSUCESO =  (SELECT MAX(P.FECSUCESO) FROM LOGSOLREQ_PERSONAL P '
     || '  WHERE P.IDESOLREQPERSONAL=L.IDESOLREQPERSONAL) ';
     
      IF p_nIdCargo>0 THEN
      
        cWhere := cWhere || ' AND  S.IDECARGO = '||p_nIdCargo;
      
      END IF;
     
      IF p_nIdDependencia>0 THEN
          
        cWhere := cWhere || '  AND S.IDEDEPENDENCIA = '||p_nIdDependencia ;
          
      END IF;
      
      IF p_nIdDepartamento>0 THEN
          
        cWhere := cWhere || '  AND S.IDEDEPARTAMENTO = '||p_nIdDepartamento ;
          
      END IF;
  
      IF p_nIdArea>0 THEN
          
        cWhere := cWhere || ' AND S.IDEAREA = '||p_nIdArea ;
          
      END IF;
   
      IF p_cTipEtapa IS NOT NULL AND p_cTipEtapa<>'' THEN
          
        cWhere := cWhere || ' AND '''||p_cTipEtapa||''' = L.TIPETAPA ';
                              
  
          
      END IF;
  
      IF p_cTipResp IS NOT NULL AND p_cTipResp<>'' THEN
          
        cWhere := cWhere || ' AND '''||p_cTipResp||''' = L.ROL ';
          
      END IF;
      
      IF p_cEstado IS NOT NULL AND p_cEstado<>'' THEN
          
        cWhere := cWhere || ' AND '''||p_cEstado||''' = L.ESTACTIVO ';
          
      END IF;
    
      IF LENGTH(rtrim(p_cFecIni))>0 AND LENGTH(rtrim(p_cFeFin))>0 THEN
         
        cWhere := cWhere || ' AND s.FECCREACION >= to_date('''||p_cFecIni||''',''DD/MM/YYYY'')'
                         || ' AND s.FECCREACION < to_date('''||p_cFeFin||''',''DD/MM/YYYY'')+1';
          
      END IF;
      
      cWhere := cWhere || 'ORDER BY S.IDESOLREQPERSONAL ';
      
      cQUERY := cCONSULTA || cWhere;                                       
  
  DELETE FROM  LOG_MENSAJE;
  
  INSERT INTO LOG_MENSAJE 
  VALUES(cQUERY);
  COMMIT;
  
   OPEN p_cRetVal FOR cQUERY;
     
  END FN_GET_LISTAREQ; */

  /* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de requerimiento
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */
 PROCEDURE FN_GET_LISTAREQ2(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                            p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                            p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                            p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                            p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                            p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,
                            p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                            p_cFecIni         IN varchar2,
                            p_cFeFin          IN varchar2,
                            p_cTipSol         IN varchar2,
                            p_nIdRoL          IN NUMBER,
                            p_nIdUsuario      IN NUMBER,
                            p_nIdSede         IN NUMBER,
                            p_cRetVal         OUT CUR_CURSOR) IS
 
   cCONSULTA VARCHAR2(4000) := NULL;
   cWhere    VARCHAR2(1000) := NULL;
   cQUERY    VARCHAR2(5000) := NULL;
 
 BEGIN
 
   cCONSULTA := 'SELECT SA.IDESOLREQPERSONAL,' || 'SA.CODSOLREQPERSONAL,' ||
                'SA.IDECARGO,' || 'SA.DESCARGO,' || 'SA.IDEDEPENDENCIA,' ||
                'SA.DESDEPENDENCIA,' || 'SA.IDEDEPARTAMENTO,' ||
                'SA.DESDEPARTAMENTO,' || 'SA.IDEAREA,' || 'SA.DESAREA,' ||
                'SA.NUMVACANTES,' || 'SA.POSTULANTE,' ||
                'SA.PRESELECCIONADOS,' || 'SA.EVALUADOS, ' ||
                'SA.SELECCIONADOS , ' || 'SA.CONTRATADOS , ' || 'SA.ROL ,' ||
                'SA.DESROL ,' || 'SA.ESTADO ,' || 'SA.TIPETAPA,' ||
                'SA.FECPUBLICACION,' || 'SA.FECCREACION,' ||
                'SA.FECEXPIRACACION, ' || 'SA.ID_USUARIO_RESP, ' ||
                'SA.NOMPERSONREEMPLAZO,' || 'SA.PUBLICADO, ' ||
                'SA.TIPSOL, ' || 'SA.DES_ETAPA ' ||
                'FROM (SELECT DISTINCT S.IDESOLREQPERSONAL,S.CODSOLREQPERSONAL,S.IDECARGO, ' ||
                ' S.NOMCARGO DESCARGO, ' || ' S.IDEDEPENDENCIA, ' ||
                ' (SELECT D.NOMDEPENDENCIA ' || ' FROM DEPENDENCIA D ' ||
                ' WHERE D.IDEDEPENDENCIA = S.IDEDEPENDENCIA ' ||
                ' AND D.IDESEDE=S.IDESEDE) DESDEPENDENCIA, ' ||
                ' S.IDEDEPARTAMENTO, ' || ' (SELECT E.NOMDEPARTAMENTO  ' ||
                '  FROM DEPARTAMENTO E ' ||
                '  WHERE E.IDEDEPARTAMENTO=S.IDEDEPARTAMENTO ' ||
                '  AND E.IDEDEPENDENCIA = S.IDEDEPENDENCIA) DESDEPARTAMENTO, ' ||
                ' S.IDEAREA, ' || ' (SELECT A.NOMAREA ' || '  FROM AREA A ' ||
                '  WHERE A.IDEAREA = S.IDEAREA ' ||
                '  AND A.IDEDEPARTAMENTO = S.IDEDEPARTAMENTO) DESAREA, ' ||
                ' S.NUMVACANTES, ' ||
                ' (SELECT COUNT(*) 
                     FROM RECLUTAMIENTO_PERSONA RP
                     WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                       AND RP.TIPSOL = S.TIPSOL
                       AND RP.ESTACTIVO=''A'') POSTULANTE, ' ||
                ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''02''
                     AND RP.ESTACTIVO=''A'') PRESELECCIONADOS, ' ||
                ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''07''
                     AND RP.ESTACTIVO=''A'') EVALUADOS, ' ||
                ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''08''
                     AND RP.ESTACTIVO=''A'') SELECCIONADOS , ' ||
                ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''09''
                     AND RP.ESTACTIVO=''A'') CONTRATADOS , ' ||
                ' L.ROLRESPONSABLE ROL , ' ||
                ' (select codRol from rol r where r.flgestado=''A'' and r.idrol =L.ROLRESPONSABLE ) DESROL, '
                ||
                'PR_INTRANET.FN_ESTADO_SOLICITUD(S.IDESOLREQPERSONAL,''R'') ESTADO,' ||
                ' L.TIPETAPA, ' || ' S.FECPUBLICACION, ' ||
                ' (SELECT C.FECSUCESO
                  FROM LOGSOLREQ_PERSONAL C
                   WHERE C.IDESOLREQPERSONAL = S.IDESOLREQPERSONAL
                    AND  C.IDELOGSOLREQ_PERSONAL =
                           (SELECT MIN(H.IDELOGSOLREQ_PERSONAL)
                                     FROM LOGSOLREQ_PERSONAL H
                                              WHERE H.IDESOLREQPERSONAL = C.IDESOLREQPERSONAL)
                                                       )FECCREACION, ' || 
                   '(SELECT C.FECSUCESO 
                      FROM LOGSOLREQ_PERSONAL C
                       WHERE C.TIPETAPA = ''08''
                       AND C.IDESOLREQPERSONAL = S.IDESOLREQPERSONAL
                       and rownum<2
                        ) FECEXPIRACACION, ' ||
                ' L.USRESPONSABLE ID_USUARIO_RESP,' ||
                ' (SELECT U.DSCNOMBRES||'' '' ||U.DSCAPEPATERNO||'' '' ||U.DSCAPEMATERNO FROM USUARIO U WHERE U.IDUSUARIO = L.USRESPONSABLE AND ROWNUM<2) NOMPERSONREEMPLAZO,' ||
                ' DECODE(S.FECPUBLICACION,NULL,''NO'',''SI'') PUBLICADO, ' ||
                ' S.TIPSOL, ' ||
                ' (SELECT D.DESCRIPCION FROM DETALLE_GENERAL D WHERE D.IDEGENERAL=50
                    AND D.VALOR = L.TIPETAPA) DES_ETAPA ' ||
                '  FROM SOLREQ_PERSONAL S,LOGSOLREQ_PERSONAL L ' ||
                '  WHERE S.IDESOLREQPERSONAL = L.IDESOLREQPERSONAL ' ||
                '  AND S.IDESEDE  = ' || p_nIdSede ||
                '  AND L.FECSUCESO =  (SELECT MAX(P.FECSUCESO) FROM LOGSOLREQ_PERSONAL P ' ||
                '  WHERE P.IDESOLREQPERSONAL=L.IDESOLREQPERSONAL) ' ||
                '  AND ''' || p_cTipSol || ''' = S.TIPSOL ' ||
                '  AND L.USRESPONSABLE  = ' || p_nIdUsuario ||
                '  AND L.ROLRESPONSABLE = ' || p_nIdRoL || ') SA WHERE 1=1';
 
   IF p_nIdCargo > 0 THEN
   
     cWhere := cWhere || ' AND  SA.IDECARGO = ' || p_nIdCargo;
   
   END IF;
 
   IF p_nIdDependencia > 0 THEN
   
     cWhere := cWhere || '  AND SA.IDEDEPENDENCIA = ' || p_nIdDependencia;
   
   END IF;
 
   IF p_nIdDepartamento > 0 THEN
   
     cWhere := cWhere || '  AND SA.IDEDEPARTAMENTO = ' || p_nIdDepartamento;
   
   END IF;
 
   IF p_nIdArea > 0 THEN
   
     cWhere := cWhere || ' AND SA.IDEAREA = ' || p_nIdArea;
   
   END IF;
 
   IF LENGTH(TRIM(p_cTipEtapa)) > 0 AND p_cTipEtapa != '0' THEN
   
     cWhere := cWhere || ' AND ''' || p_cTipEtapa || ''' = SA.TIPETAPA ';
   
   ELSE
   
    IF p_nIdRoL=8 OR p_nIdRoL =9 THEN
      
         cWhere := cWhere || ' AND SA.TIPETAPA IN (SELECT CE.TIPETAPA FROM CONFIG_ETAPA CE WHERE CE.IDROL = ''' || p_nIdRoL || ''' AND CE.TIPSOL=''03'' AND CE.ESTACTIVO=''A'' AND CE.DEFECTO = ''S'') ';
    
    END IF;
   
   END IF;
 
   IF LENGTH(TRIM(p_cTipResp)) > 0 AND p_cTipResp != 0 THEN
   
     cWhere := cWhere || ' AND ''' || p_cTipResp || ''' = SA.ROL ';
   
   END IF;
 
   IF LENGTH(TRIM(p_cEstado)) > 0 AND p_cEstado != '0' THEN
   
     cWhere := cWhere || ' AND ''' || p_cEstado || ''' = SA.ESTADO ';
   
   END IF;
 
   IF LENGTH(rtrim(p_cFecIni)) > 0 AND LENGTH(rtrim(p_cFeFin)) > 0 THEN
   
     cWhere := cWhere || ' AND SA.FECCREACION >= to_date(''' || p_cFecIni ||
               ''',''DD/MM/YYYY'')' || ' AND SA.FECCREACION < to_date(''' ||
               p_cFeFin || ''',''DD/MM/YYYY'')+1';
   
   END IF;
 
   cWhere := cWhere || ' ORDER BY SA.IDESOLREQPERSONAL DESC';
 
   cQUERY := cCONSULTA || cWhere;
 
   DELETE FROM LOG_MENSAJE;
 
   INSERT INTO LOG_MENSAJE VALUES (cQUERY);
   COMMIT;
 
   OPEN p_cRetVal FOR cQUERY;
 
 END FN_GET_LISTAREQ2;
  
    /* ------------------------------------------------------------
    Nombre      : FN_GET_LISTACARGO
    Proposito   : obtiene la lista de solicitudes de nuevo cargo
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      25/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE FN_GET_LISTACARGO(p_nIdSolicitud    IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_nIdDependencia  IN SOLNUEVO_CARGO.Idedependencia%TYPE,
                              p_nIdDepartamento IN SOLNUEVO_CARGO.Idedepartamento%TYPE,
                              p_nIdArea         in SOLNUEVO_CARGO.Idearea%TYPE,
                              p_cTipEtapa       IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE,
                              p_cTipResp        in LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                              p_cEstado         IN SOLNUEVO_CARGO.ESTACTIVO%TYPE,
                              p_cFecIni         IN varchar2,
                              p_cFeFin          IN varchar2,
                              p_cRolResp        IN ROL.IDROL%TYPE,
                              p_cUsrResponsable IN USUARIO.IDUSUARIO%TYPE,
                              p_nIdSede         IN NUMBER,
                              p_cRetVal         OUT CUR_CURSOR) IS
  
    cCONSULTA VARCHAR2(5000) := NULL;
    cWhere    VARCHAR2(1000) := NULL;
    cQUERY    VARCHAR2(6000) := NULL;
  
  BEGIN

   
    cCONSULTA := 'SELECT SN.IDESOLNUEVOCARGO,'
                         ||'SN.ESTADO,' 
                         ||'SN.CODCARGO,'
                         ||'SN.IDECARGO,'
                         ||'SN.NOMCARGO,'
                         ||'SN.IDEDEPENDENCIA,'
                         ||'SN.NOMDEPENDENCIA,'
                         ||'SN.IDEDEPARTAMENTO,'
                         ||'SN.NOMDEPARTAMENTO,'
                         ||'SN.IDEAREA,'
                         ||'SN.NOMAREA,'
                         ||'SN.NUMPOSICIONES,'
                         ||'SN.POSTULANTES,'
                         ||'SN.PRESELECCIONADOS,'
                         ||'SN.EVALUADOS, '
                         ||'SN.SELECCIONADOS , '
                         ||'SN.CONTRATADOS , '
                         ||'SN.FECHACREACION FECCREACION,'
                         ||'SN.IDROL ,'
                         ||'SN.DSCROL ,'
                         ||'SN.NOMRESPONSABLE,'
                         ||'SN.TETAPA,'
                         ||'SN.VERSION,'
                         ||'SN.PUBLICADO, '
                         ||'SN.FECPUBLICACION, '
                         ||'SN.FECEXPIRACION, '
                         ||'SN.USRESPONSABLE '
                 ||'FROM ( SELECT S.IDESOLNUEVOCARGO, '
                           ||'PR_INTRANET.FN_ESTADO_SOLICITUD(S.IDESOLNUEVOCARGO,''01'') ESTADO, '
                           ||'S.CODCARGO,'
                           ||'NVL(S.IDECARGO,0) IDECARGO ,S.NOMBRE NOMCARGO, '
                           ||'S.IDEDEPENDENCIA, '
                           ||'(SELECT DE.NOMDEPENDENCIA FROM DEPENDENCIA DE WHERE DE.IDEDEPENDENCIA = S.IDEDEPENDENCIA AND DE.IDESEDE = S.IDESEDE AND DE.ESTACTIVO = ''A'') NOMDEPENDENCIA, '
                           ||'S.IDEDEPARTAMENTO , '
                           ||'(SELECT DP.NOMDEPARTAMENTO FROM DEPARTAMENTO DP WHERE DP.IDEDEPARTAMENTO = S.IDEDEPARTAMENTO AND DP.IDEDEPENDENCIA = S.IDEDEPENDENCIA AND DP.ESTACTIVO = ''A'') NOMDEPARTAMENTO, '
                           ||'S.IDEAREA, '
                           ||'(SELECT AR.NOMAREA FROM AREA AR WHERE AR.IDEAREA = S.IDEAREA AND AR.IDEDEPARTAMENTO = S.IDEDEPARTAMENTO AND AR.ESTACTIVO = ''A'') NOMAREA,'
                           ||' S.NUMPOSICIONES, '||
                           ' (SELECT COUNT(*) 
                             FROM RECLUTAMIENTO_PERSONA RP
                             WHERE RP.IDESOL = S.IDESOLNUEVOCARGO
                             AND RP.TIPSOL = ''01''
                             AND RP.ESTACTIVO=''A'') POSTULANTES, ' ||
                             ' (SELECT COUNT(*) 
                             FROM RECLUTAMIENTO_PERSONA RP
                             WHERE RP.IDESOL = S.IDESOLNUEVOCARGO
                             AND RP.TIPSOL = ''01''
                             AND RP.ESTPOSTULANTE =''02''
                             AND RP.ESTACTIVO=''A'') PRESELECCIONADOS, ' ||
                             ' (SELECT COUNT(*) 
                             FROM RECLUTAMIENTO_PERSONA RP
                             WHERE RP.IDESOL = S.IDESOLNUEVOCARGO
                             AND RP.TIPSOL = ''01''
                             AND RP.ESTPOSTULANTE =''07''
                             AND RP.ESTACTIVO=''A'') EVALUADOS, ' || 
                             ' (SELECT COUNT(*) 
                             FROM RECLUTAMIENTO_PERSONA RP
                             WHERE RP.IDESOL = S.IDESOLNUEVOCARGO
                              AND RP.TIPSOL = ''01''
                              AND RP.ESTPOSTULANTE =''08''
                              AND RP.ESTACTIVO=''A'') SELECCIONADOS , ' ||
                              ' (SELECT COUNT(*) 
                             FROM RECLUTAMIENTO_PERSONA RP
                             WHERE RP.IDESOL = S.IDESOLNUEVOCARGO
                              AND RP.TIPSOL = ''01''
                              AND RP.ESTPOSTULANTE =''09''
                              AND RP.ESTACTIVO=''A'') CONTRATADOS , ' ||
                              '(SELECT C.FECSUCESO
                                FROM logsolnuevo_cargo C
                                 WHERE C.Idesolnuevocargo = S.Idesolnuevocargo
                                  AND  C.Idelogsolnuevocargo =
                                         (SELECT MIN(H.Idelogsolnuevocargo)
                                                   FROM logsolnuevo_cargo H
                                                   WHERE H.Idesolnuevocargo = C.Idesolnuevocargo)
                                      ) FECHACREACION,R.IDROL ,R.DSCROL , '||
                               '(PR_REQUERIMIENTOS.FN_NOMRESPONS_SOL(S.IDESOLNUEVOCARGO,''N'')) NOMRESPONSABLE, LS.TIPETAPA,(PR_INTRANET.FN_VALOR_GENERAL(''ETAPA'',LS.TIPETAPA)) TETAPA, '||
                               '(SELECT C.VERSION FROM CARGO C WHERE C.IDECARGO = S.IDECARGO) VERSION, '||
                               'DECODE(S.FECPUBLICACION,NULL,''NO'',''SI'') PUBLICADO , '||
                               ' S.FECPUBLICACION, S.FECCREACION,
                               (SELECT C.Fecsuceso 
                                FROM logsolnuevo_cargo C
                                 WHERE C.Tipetapa = ''08''
                                 AND C.Idesolnuevocargo = S.Idesolnuevocargo
                                 and rownum<2
                                  ) FECEXPIRACION,LS.USRESPONSABLE '||
                               'FROM SOLNUEVO_CARGO S, LOGSOLNUEVO_CARGO LS LEFT JOIN ROL R ON (R.IDROL = LS.ROLRESPONSABLE)  '||
                               'WHERE LS.IDESOLNUEVOCARGO = S.IDESOLNUEVOCARGO '||
                               'AND LS.FECSUCESO = (SELECT MAX (FECSUCESO) FROM  LOGSOLNUEVO_CARGO LN '||
                               'WHERE LN.IDESOLNUEVOCARGO = S.IDESOLNUEVOCARGO) '||
                               'AND S.IDESEDE = '||p_nIdSede ||
                               ' AND R.IDROL = '|| p_cRolResp ||
                               ' AND LS.USRESPONSABLE = '||p_cUsrResponsable ||') SN WHERE 1=1' ;
    
    
    IF p_nIdSolicitud > 0 THEN
    
      cWhere := cWhere || ' AND  SN.IDESOLNUEVOCARGO = ' || p_nIdSolicitud;
    
    END IF;
  
    IF p_nIdDependencia > 0 THEN
    
      cWhere := cWhere || '  AND SN.IDEDEPENDENCIA = ' || p_nIdDependencia;
    
    END IF;
  
    IF p_nIdDepartamento > 0 THEN
    
      cWhere := cWhere || '  AND SN.IDEDEPARTAMENTO = ' || p_nIdDepartamento;
    
    END IF;
  
    IF p_nIdArea > 0 THEN
    
      cWhere := cWhere || ' AND SN.IDEAREA = ' || p_nIdArea;
    
    END IF;
  
    IF p_cTipEtapa IS NOT NULL AND p_cTipEtapa <> '0' THEN
    
      cWhere := cWhere || ' AND ''' || p_cTipEtapa || ''' = SN.TIPETAPA ';
    
    ELSE
      
      IF p_cRolResp=8 OR p_cRolResp =9 THEN
    
      cWhere := cWhere || ' AND SN.TIPETAPA IN (SELECT CE.TIPETAPA FROM CONFIG_ETAPA CE WHERE CE.IDROL = ''' || p_cRolResp || ''' AND CE.TIPSOL=''01'' AND CE.ESTACTIVO=''A'' AND CE.DEFECTO = ''S'') ';
      END IF;
    
    END IF;
  
    IF p_cTipResp IS NOT NULL AND p_cTipResp <> '0' THEN
    
      cWhere := cWhere || ' AND ''' || p_cTipResp || ''' = SN.IDROL ';
    
    END IF;
  
    IF p_cEstado IS NOT NULL AND p_cEstado <> '0' THEN
    
      cWhere := cWhere || ' AND ''' || p_cEstado || ''' = SN.ESTADO ';
    
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni)) > 0 AND LENGTH(rtrim(p_cFeFin)) > 0 THEN
    
      cWhere := cWhere || ' AND SN.FECCREACION >= to_date(''' || p_cFecIni ||
                ''',''DD/MM/YYYY'')' || ' AND SN.FECCREACION < to_date(''' ||
                p_cFeFin || ''',''DD/MM/YYYY'')+1';
    
    END IF;
  
    cWhere := cWhere || ' ORDER BY SN.TETAPA DESC, SN.IDESOLNUEVOCARGO  ';
  
    cQUERY := cCONSULTA || cWhere;
  
    DELETE FROM LOG_MENSAJE;
  
    INSERT INTO LOG_MENSAJE VALUES (cQUERY);
    COMMIT;
  
    OPEN p_cRetVal FOR cQUERY;
  
  END FN_GET_LISTACARGO;
  
  /* ------------------------------------------------------------
    Nombre      : SP_GET_LISTA_AMPLIACION
    Proposito   : obtiene la lista de requerimiento de ampliacion de cargo
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_GET_LISTA_AMPLIACION(p_nCodCargo        IN SOLREQ_PERSONAL.CODCARGO%TYPE,
                                    p_nIdDependencia   IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                                    p_nIdDepartamento  IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                                    p_nIdArea          IN SOLREQ_PERSONAL.Idearea%TYPE,
                                    p_cTipEtapa        IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                                    p_cTipResp         IN LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,
                                    p_cEstado          IN SOLREQ_PERSONAL.Estactivo%TYPE,
                                    p_cFecIni          IN varchar2,
                                    p_cFeFin           IN varchar2,
                                    p_cTipSol          IN varchar2,
                                    p_nIdRoL           IN NUMBER,
                                    p_nIdUsuario       IN NUMBER,
                                    p_nIdSede          IN NUMBER,
                                    p_cRetVal          OUT CUR_CURSOR) IS
  
    cCONSULTA VARCHAR2(6000) := NULL;
    cWhere    VARCHAR2(1000) := NULL;
    cQUERY    VARCHAR2(5000) := NULL;
  
  BEGIN
  
    cCONSULTA := 'SELECT SA.IDESOLREQPERSONAL,'
               ||'SA.CODSOLREQPERSONAL,' 
               ||'SA.IDECARGO,'
               ||'SA.CODCARGO,'
               ||'SA.DESCARGO,'
               ||'SA.IDEDEPENDENCIA,'
               ||'SA.DESDEPENDENCIA,'
               ||'SA.IDEDEPARTAMENTO,'
               ||'SA.DESDEPARTAMENTO,'               
               ||'SA.IDEAREA,'
               ||'SA.DESAREA,'
               ||'SA.NUMVACANTES,'
               ||'SA.POSTULANTE,'
               ||'SA.PRESELECCIONADOS,'
               ||'SA.EVALUADOS, '
               ||'SA.SELECCIONADOS , '
               ||'SA.CONTRATADOS , '
               ||'SA.ROL ,'
               ||'SA.DESROL ,'
               ||'SA.ESTADO ,'
               ||'SA.TIPETAPA,'
               ||'SA.FECPUBLICACION,'
               ||'SA.FECCREACION,'
               ||'SA.FECEXPIRACACION, '
               ||'SA.ID_USUARIO_RESP, '
               ||'SA.PUBLICADO, '
               ||'SA.NOMPERSONREEMPLAZO,'
               ||'SA.TIPSOL, '
               ||'SA.DES_ETAPA '
     ||'FROM (SELECT DISTINCT S.IDESOLREQPERSONAL,S.CODSOLREQPERSONAL,S.IDECARGO,S.CODCARGO, ' ||
                 ' S.NOMCARGO DESCARGO, ' ||
                 ' S.IDEDEPENDENCIA, ' || ' (SELECT D.NOMDEPENDENCIA ' ||
                 ' FROM DEPENDENCIA D ' ||
                 ' WHERE D.IDEDEPENDENCIA = S.IDEDEPENDENCIA ' ||
                 ' AND D.IDESEDE=S.IDESEDE) DESDEPENDENCIA, ' ||
                 ' S.IDEDEPARTAMENTO, ' || ' (SELECT E.NOMDEPARTAMENTO  ' ||
                 '  FROM DEPARTAMENTO E ' ||
                 '  WHERE E.IDEDEPARTAMENTO=S.IDEDEPARTAMENTO ' ||
                 '  AND E.IDEDEPENDENCIA = S.IDEDEPENDENCIA) DESDEPARTAMENTO, ' ||
                 ' S.IDEAREA, ' || ' (SELECT A.NOMAREA ' ||
                 '  FROM AREA A ' || '  WHERE A.IDEAREA = S.IDEAREA ' ||
                 '  AND A.IDEDEPARTAMENTO = S.IDEDEPARTAMENTO) DESAREA, ' ||
                 ' S.NUMVACANTES, ' ||
                 ' (SELECT COUNT(*) 
                     FROM RECLUTAMIENTO_PERSONA RP
                     WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                       AND RP.TIPSOL = S.TIPSOL
                       AND RP.ESTACTIVO=''A'') POSTULANTE, ' ||
                 ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''02''
                     AND RP.ESTACTIVO=''A'') PRESELECCIONADOS, ' ||
                 ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''07''
                     AND RP.ESTACTIVO=''A'') EVALUADOS, ' || 
                 ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''08''
                     AND RP.ESTACTIVO=''A'') SELECCIONADOS , ' ||
                     ' (SELECT COUNT(*) 
                   FROM RECLUTAMIENTO_PERSONA RP
                   WHERE RP.IDESOL = S.IDESOLREQPERSONAL
                     AND RP.TIPSOL = S.TIPSOL
                     AND RP.ESTPOSTULANTE =''09''
                     AND RP.ESTACTIVO=''A'') CONTRATADOS , ' ||
                 ' L.ROLRESPONSABLE ROL , ' ||
                 ' (select codRol from rol r where r.flgestado=''A'' and r.idrol =L.ROLRESPONSABLE ) DESROL, '
              
                 ||
                 'PR_INTRANET.FN_ESTADO_SOLICITUD(S.IDESOLREQPERSONAL,''R'') ESTADO,' ||
                 ' L.TIPETAPA, ' || ' S.FECPUBLICACION, ' ||
                 
                 
                
                ' (SELECT C.FECSUCESO
                  FROM LOGSOLREQ_PERSONAL C
                   WHERE C.IDESOLREQPERSONAL = S.IDESOLREQPERSONAL
                    AND  C.IDELOGSOLREQ_PERSONAL =
                           (SELECT MIN(H.IDELOGSOLREQ_PERSONAL)
                                     FROM LOGSOLREQ_PERSONAL H
                                              WHERE H.IDESOLREQPERSONAL = C.IDESOLREQPERSONAL)
                                                       ) FECCREACION, ' || 
                   '(SELECT C.FECSUCESO 
                      FROM LOGSOLREQ_PERSONAL C
                       WHERE C.TIPETAPA = ''08''
                       AND C.IDESOLREQPERSONAL = S.IDESOLREQPERSONAL
                       and rownum<2
                        ) FECEXPIRACACION, ' ||
                
                 
                 
                 
                 ' L.USRESPONSABLE ID_USUARIO_RESP,' ||
                 ' (SELECT U.DSCNOMBRES||'' '' ||U.DSCAPEPATERNO||'' '' ||U.DSCAPEMATERNO FROM USUARIO U WHERE U.IDUSUARIO = L.USRESPONSABLE AND ROWNUM<2) NOMPERSONREEMPLAZO,' ||
                 ' DECODE(S.FECPUBLICACION,NULL,''NO'',''SI'') PUBLICADO, ' ||
                 ' S.TIPSOL, ' ||
                 ' (SELECT D.DESCRIPCION FROM DETALLE_GENERAL D WHERE D.IDEGENERAL=50
                    AND D.VALOR = L.TIPETAPA) DES_ETAPA ' ||
                 '  FROM SOLREQ_PERSONAL S,LOGSOLREQ_PERSONAL L ' ||
                 '  WHERE S.IDESOLREQPERSONAL = L.IDESOLREQPERSONAL ' ||
                 '  AND S.IDESEDE  = ' || p_nIdSede ||
                 '  AND L.FECSUCESO =  (SELECT MAX(P.FECSUCESO) FROM LOGSOLREQ_PERSONAL P ' ||
                 '  WHERE P.IDESOLREQPERSONAL=L.IDESOLREQPERSONAL) ' ||
                 '  AND ''' || p_cTipSol || ''' = S.TIPSOL ' ||
                 '  AND L.USRESPONSABLE  = ' || p_nIdUsuario ||
                 '  AND L.ROLRESPONSABLE = ' || p_nIdRoL ||') SA WHERE 1=1' ;
  
    IF p_nCodCargo != '0' THEN
    
      cWhere := cWhere || ' AND  SA.CODCARGO = ''' || p_nCodCargo ||'''';
    
    END IF;
  
    IF p_nIdDependencia > 0 THEN
    
      cWhere := cWhere || '  AND SA.IDEDEPENDENCIA = ' || p_nIdDependencia;
    
    END IF;
  
    IF p_nIdDepartamento > 0 THEN
    
      cWhere := cWhere || '  AND SA.IDEDEPARTAMENTO = ' || p_nIdDepartamento;
    
    END IF;
  
    IF p_nIdArea > 0 THEN
    
      cWhere := cWhere || ' AND SA.IDEAREA = ' || p_nIdArea;
    
    END IF;
  
    IF LENGTH(TRIM(p_cTipEtapa))>0 AND p_cTipEtapa!= '0' THEN
    
      cWhere := cWhere || ' AND ''' || p_cTipEtapa || ''' = SA.TIPETAPA ';
    
    ELSE
    
      IF p_nIdRoL=8 OR p_nIdRoL =9 THEN
      
         cWhere := cWhere || ' AND SA.TIPETAPA IN (SELECT CE.TIPETAPA FROM CONFIG_ETAPA CE WHERE CE.IDROL = ''' || p_nIdRoL || ''' AND CE.TIPSOL=''02'' AND CE.ESTACTIVO=''A'' AND CE.DEFECTO = ''S'') ';
    
      END IF;
      
    END IF;
  
    IF  LENGTH(TRIM(p_cTipResp))>0 AND  p_cTipResp!=0 THEN
    
      cWhere := cWhere || ' AND ''' || p_cTipResp || ''' = SA.ROL ';
    
    END IF;
  
    IF  LENGTH(TRIM(p_cEstado))>0 AND p_cEstado !='0' THEN
    
      cWhere := cWhere || ' AND ''' || p_cEstado || ''' = SA.ESTADO ';
    
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni)) > 0 AND LENGTH(rtrim(p_cFeFin)) > 0 THEN
    
      cWhere := cWhere || ' AND SA.FECCREACION >= to_date(''' || p_cFecIni ||
                ''',''DD/MM/YYYY'')' || ' AND SA.FECCREACION < to_date(''' ||
                p_cFeFin || ''',''DD/MM/YYYY'')+1';
    
    END IF;
  
    cWhere := cWhere || ' ORDER BY SA.IDESOLREQPERSONAL DESC ';
  
    cQUERY := cCONSULTA || cWhere;
  
    DELETE FROM LOG_MENSAJE;
  
    INSERT INTO LOG_MENSAJE VALUES (cQUERY);
    COMMIT;
  
    OPEN p_cRetVal FOR cQUERY;
  
  END SP_GET_LISTA_AMPLIACION;
  
  /* ------------------------------------------------------------
    Nombre      : SP_ESTADO_SOLICITUD
    Proposito   : Obtiene el estado deacuerdo el tiempo 
                  transcurrido desde la solicitud
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      11/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  FUNCTION FN_ESTADO_SOLICITUD(p_idSolicitud   IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                               p_tipoSolicitud IN VARCHAR2) RETURN VARCHAR2 IS
  
    p_retVal         VARCHAR2(1);
    c_estado         VARCHAR2(1);
    c_fechaSolicitud DATE;
    c_diasTrans      NUMBER;
  BEGIN
    BEGIN
    
      IF (p_tipoSolicitud = '01') THEN
        SELECT SN.ESTACTIVO, SN.FECCREACION
          INTO c_estado, c_fechaSolicitud
          FROM SOLNUEVO_CARGO SN
         WHERE SN.IDESOLNUEVOCARGO = p_idSolicitud;
      ELSE
        SELECT SQ.ESTACTIVO, SQ.FECCREACION
          INTO c_estado, c_fechaSolicitud
          FROM SOLREQ_PERSONAL SQ
         WHERE SQ.IDESOLREQPERSONAL = p_idSolicitud;
      END IF;
    
      IF (c_estado = 'A') THEN
      
        SELECT TO_DATE(SYSDATE) - TO_DATE(c_fechaSolicitud)
          INTO c_diasTrans
          FROM DUAL;
      
        IF (c_diasTrans <= 21) THEN
          p_retVal := 'V';
        ELSIF ((c_diasTrans > 21) AND (c_diasTrans <= 35)) THEN
          p_retVal := 'M';
        ELSIF (c_diasTrans > 35) THEN
          p_retVal := 'R';
        END IF;
      
      ELSE
        p_retVal := c_estado;
      END IF;
    EXCEPTION
      WHEN OTHERS THEN
        p_retVal := 'I';
      
    END;
    RETURN p_retVal;
  
  END FN_ESTADO_SOLICITUD;

/* ------------------------------------------------------------
    Nombre      : SP_SOLICITUDES_CARGO
    Proposito   : obtiene la lista de cargo que mostrara por usuario rol
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      27/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_CARGOS_SOLICITUD(p_idSede        IN SEDE.IDESEDE%TYPE,
                                p_idUsrResp     IN USUARIO.IDUSUARIO%TYPE,
                                p_idRolResp     IN ROL.IDROL%TYPE,
                                p_cRetVal       OUT SYS_REFCURSOR)IS
  
    
  BEGIN
    BEGIN
      OPEN p_cRetVal FOR
      SELECT SN.IDESOLNUEVOCARGO,SN.NOMBRE
      FROM SOLNUEVO_CARGO SN, LOGSOLNUEVO_CARGO LS
      WHERE SN.IDESOLNUEVOCARGO = LS.IDESOLNUEVOCARGO
      AND LS.FECSUCESO = (SELECT MAX (FECSUCESO) 
                          FROM  LOGSOLNUEVO_CARGO LN 
                          WHERE LN.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO)
      AND SN.IDESEDE = p_idSede 
      AND LS.ROLRESPONSABLE = p_idRolResp
      AND LS.USRESPONSABLE = p_idUsrResp;
      
    EXCEPTION
      WHEN OTHERS THEN
        p_cRetVal := null;
    END;
    
  
  END SP_CARGOS_SOLICITUD;
  
  /* ------------------------------------------------------------
    Nombre      : SP_GENERAR_EXAMEN_CAT_RECL
    Proposito   : Obtiene los examenes que debe rendir un postulante
                  por categoria y inserta en la tabla RECL_PERSO_EXAM_CAT
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      02/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
   PROCEDURE SP_GENERAR_EXAMEN_CAT_RECL(p_idReclutaPerso   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                        p_usuario          IN USUARIO.CODUSUARIO%TYPE,
                                        p_RetVal         OUT NUMBER)IS
  
  c_idReclutaPersona     RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE;
  c_idReclutaPersExaCat  RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE;
 -- c_idPostulante         POSTULANTE.IDEPOSTULANTE%TYPE;
  CURSOR cu_examenes(c_idRecluPersona RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE)IS
    SELECT RP.IDERECLUPERSOEXAMEN,(DECODE(RP.TIPSOLICITUD,'01',(SELECT EC.IDEEXAMEVAL
                                                                FROM EVALUACION_CARGO EC
                                                                WHERE RP.IDEEVALUACION = EC.IDEEVALUACIONCARGO),
                                                               (SELECT ES.IDEEXAMEVAL
                                                                FROM EVALUACION_SOLREQ ES
                                                                WHERE ES.IDEEVALUACIONSOLREQ = RP.IDEEVALUACION))) IDEEXAMEVAL
    FROM RECLU_PERSO_EXAMEN RP
    WHERE RP.IDERECLUTAPERSONA = c_idRecluPersona;
      
  BEGIN
    DELETE T_EXAMCAT_TEMP;  

   BEGIN
    FOR REG_EVALUACIONES IN cu_examenes(p_idReclutaPerso)LOOP  
      
      INSERT INTO RECL_PERS_EXAM_CAT
      (IDERECLPERSOEXAMNCAT,
       IDERECLUTAPERSONA,
       IDERECLUPERSOEXAMEN,
       IDEEXAMENXCATEGORIA,
       ESTADO,
       ESTACTIVO,
       NOTAEXAMENCATEG,
       USRCREACION,
       FECCREACION)
      SELECT 
       IDERECLPERSOEXAMNCAT_SQ.NEXTVAL,
       p_idReclutaPerso,
       REG_EVALUACIONES.IDERECLUPERSOEXAMEN,
       EXCT.IDEEXAMENXCATEGORIA,
       P_ESTCAT_PENDIENTE,
       'A',
       0,
       p_usuario,
       SYSDATE
      FROM EXAMEN EX, CATEGORIA CAT, EXAMEN_X_CATEGORIA EXCT
      WHERE EX.IDEEXAMEN = EXCT.IDEEXAMEN
      AND CAT.IDECATEGORIA = EXCT.IDECATEGORIA
      AND ((EX.TIPEXAMEN = P_TIPEXA_EXAMEN) OR (EX.TIPEXAMEN = P_TIPEXA_EVALUACION))
      AND EX.IDEEXAMEN = REG_EVALUACIONES.IDEEXAMEVAL;
    END LOOP;  
    
    --cambiar el estado del postulante
    
    COMMIT;
     p_RetVal:=1;
    
    EXCEPTION
      WHEN OTHERS THEN
      ROLLBACK;
      p_RetVal:=0;
    END;
      
  END SP_GENERAR_EXAMEN_CAT_RECL;
  

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_EXAMENES_POST
    Proposito   : Obtiene los examenes que debe rendir un postulante
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      26/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_OBTENER_EXAMENES_POST(p_idReclutaPerso   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                      p_cuRetVal         OUT SYS_REFCURSOR)IS
      
  BEGIN  
    
    BEGIN
      OPEN p_cuRetVal FOR  
      SELECT *
      FROM ( SELECT RE.IDERECLPERSOEXAMNCAT, RE.IDEEXAMENXCATEGORIA,              
                    EX.IDEEXAMEN, CA.IDECATEGORIA, EX.NOMEXAMEN,EX.DESCEXAMEN,CA.NOMCATEGORIA, CA.ORDENIMPRESION,
                    NVL(CA.TIEMPO,0) TIEMPO, RE.ESTADO,(CASE WHEN EX.TIPEXAMEN = P_TIPEXA_EXAMEN THEN 1 
                                                            WHEN EX.TIPEXAMEN = P_TIPEXA_EVALUACION THEN 2
                                                            ELSE 3
                                                        END) ORDEN
             FROM RECL_PERS_EXAM_CAT RE, EXAMEN EX, CATEGORIA CA , EXAMEN_X_CATEGORIA ECA
             WHERE RE.IDEEXAMENXCATEGORIA = ECA.IDEEXAMENXCATEGORIA
             AND EX.IDEEXAMEN = ECA.IDEEXAMEN
             AND CA.IDECATEGORIA = ECA.IDECATEGORIA
             AND ((EX.TIPEXAMEN = '01')OR(EX.TIPEXAMEN = '04'))
             AND RE.IDERECLUTAPERSONA = p_idReclutaPerso) EC
      ORDER BY EC.ORDEN;
        
    EXCEPTION
      WHEN OTHERS THEN
      p_cuRetVal:=NULL;
    END;
      
  END SP_OBTENER_EXAMENES_POST;
  
  /* ------------------------------------------------------------
    Nombre      : SP_EXAMEN_CATEGORIA
    Proposito   : Obtiene los examenes que debe rendir un postulante
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      26/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_EXAMEN_CATEGORIA(p_ideCategoria  IN CATEGORIA.IDECATEGORIA%TYPE,
                                p_cuRetVal      OUT SYS_REFCURSOR)IS
  
   
  BEGIN

   BEGIN  
    OPEN p_cuRetVal FOR
      SELECT SC.IDESUBCATEGORIA,CSC.IDECRITERIOXSUBCATEGORIA,SC.NOMSUBCATEGORIA,SC.TIEMPO,C.IDECRITERIO,
          C.PREGUNTA,C.TIPMODO 
      FROM SUBCATEGORIA SC, CRITERIO C, CRITERIO_X_SUBCATEGORIA CSC
      WHERE SC.IDESUBCATEGORIA = CSC.IDESUBCATEGORIA
      AND C.IDECRITERIO = CSC.IDECRITERIO
      AND C.ESTACTIVO = 'A'
      AND SC.IDECATEGORIA = p_ideCategoria
      ORDER BY SC.ORDENIMPRESION ASC , C.ORDENIMPRESION ASC;
   
   EXCEPTION
     WHEN OTHERS THEN
      p_cuRetVal:=NULL;
   END;
      
  END SP_EXAMEN_CATEGORIA;
  
  /* ------------------------------------------------------------
    Nombre      : SP_GET_IDRECLU_PERSON
    Proposito   : Obtiene los examenes que debe rendir un postulante
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      01/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_GET_IDRECLU_PERSON(p_idePostulante    IN POSTULANTE.IDEPOSTULANTE%TYPE,
                                  p_ideSede          IN SEDE.IDESEDE%TYPE,
                                  p_RetVal           OUT NUMBER)IS

 BEGIN
     
   BEGIN 
    
     SELECT RP.IDERECLUTAPERSONA
     INTO p_RetVal
     FROM RECLUTAMIENTO_PERSONA RP  
     WHERE RP.IDEPOSTULANTE = p_idePostulante
     AND RP.IDSEDE = p_ideSede
     AND ((RP.ESTPOSTULANTE = P_PRESEL_AUTOMATC) OR 
          (RP.ESTPOSTULANTE = P_PRESEL_MANUAL)OR
          (RP.ESTPOSTULANTE = P_EN_EVALUACION));
      
   EXCEPTION
     WHEN OTHERS THEN
      p_RetVal:=0;
   END;
      
 END SP_GET_IDRECLU_PERSON;
  
  
    /* ------------------------------------------------------------
    Nombre      : SP_GUARDAR_ALTERNATIVA
    Proposito   : Obtiene los examenes que debe rendir un postulante
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      01/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_GUARDAR_ALTERNATIVA(p_ideReclutaPersona  IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                   p_ideCriterioSubCat  IN CRITERIO_X_SUBCATEGORIA.IDECRITERIOXSUBCATEGORIA%TYPE,
                                   p_ideReclPersExaCat  IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                                   p_ideAlternativa     IN ALTERNATIVA.IDEALTERNATIVA%TYPE,
                                   p_usuarioCreacion    IN USUARIO.CODUSUARIO%TYPE,
                                   p_RetVal             OUT NUMBER)IS
  
   
  c_ideReclutaCriterio_sq NUMBER;
  c_puntajeAlternativa    NUMBER;
  c_notaCategoria         NUMBER;
  c_tipoExamen            EXAMEN.TIPEXAMEN%TYPE;
  c_ideReclutaPersExamen  RECLU_PERSO_EXAMEN.IDERECLUPERSOEXAMEN%TYPE;
  c_notaTotal             number;
  
  BEGIN
     
     SELECT IDERECLUPERSOCRITERIO_SQ.NEXTVAL
     INTO c_ideReclutaCriterio_sq
     FROM DUAL;
     
     SELECT AL.PESO
     INTO c_puntajeAlternativa
     FROM ALTERNATIVA AL
     WHERE AL.IDEALTERNATIVA = p_ideAlternativa;
     
   BEGIN 
      
     INSERT INTO RECLU_PERSO_CRITERIO 
     (IDERECLUPERSOCRITERIO,IDERECLUTAPERSONA,IDECRITERIOXSUBCATEGORIA,IDERECLPERSOEXAMNCAT,INDRESPUESTA,PUNTTOTAL,USRCREACION,FECCREACION)
     VALUES (c_ideReclutaCriterio_sq,p_ideReclutaPersona,p_ideCriterioSubCat,p_ideReclPersExaCat,'S',c_puntajeAlternativa,p_usuarioCreacion, SYSDATE);

     INSERT INTO RECLU_PERSO_ALTERNATIVA
     (IDERECLUPERSOALTERNATIVA,IDERECLUTAPERSONA,IDERECLUPERSOCRITERIO,IDEALTERNATIVA,USRCREACION,FECCREACION)
     VALUES (IDERECLUPERSOALTERNATIVA_SQ.NEXTVAL,p_ideReclutaPersona,c_ideReclutaCriterio_sq,p_ideAlternativa,p_usuarioCreacion,SYSDATE);
     
    --Actualizar la nota por categoria
     
    SELECT REX.TIPEXAMEN
    INTO c_tipoExamen
    FROM RECLU_PERSO_EXAMEN REX
    WHERE REX.IDERECLUPERSOEXAMEN = (SELECT RCAT.IDERECLUPERSOEXAMEN
                                     FROM RECL_PERS_EXAM_CAT RCAT
                                     WHERE RCAT.IDERECLPERSOEXAMNCAT = p_ideReclPersExaCat);
     
     IF (c_tipoExamen = P_TIPEXA_EXAMEN)THEN
      
       IF (c_puntajeAlternativa >0)THEN
          c_puntajeAlternativa := 1;
       END IF;
       
      END IF;
     
      SELECT RC.NOTAEXAMENCATEG
      INTO c_notaCategoria
      FROM RECL_PERS_EXAM_CAT RC 
      WHERE RC.IDERECLPERSOEXAMNCAT = p_ideReclPersExaCat;
      
      c_notaTotal:= c_notaCategoria +  c_puntajeAlternativa;
      
      UPDATE RECL_PERS_EXAM_CAT RCAT
      SET RCAT.NOTAEXAMENCATEG = c_notaTotal
      WHERE RCAT.IDERECLPERSOEXAMNCAT = p_ideReclPersExaCat;  
     
     COMMIT;
     p_RetVal:= c_notaTotal; 
      
   EXCEPTION
     WHEN OTHERS THEN
     ROLLBACK;
     p_RetVal:=-1;

   END;
      
  END SP_GUARDAR_ALTERNATIVA;
  
  
   /* ------------------------------------------------------------
    Nombre      : SP_OBTENER_IDECATEGORIA
    Proposito   : Obtiene los examenes que debe rendir un postulante
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      01/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_OBTENER_IDECATEGORIA(p_idReclPerExaCat    IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                                  p_RetVal           OUT NUMBER)IS

 BEGIN
     
   BEGIN 
    
     SELECT EC.IDECATEGORIA
     INTO p_RetVal
     FROM EXAMEN_X_CATEGORIA EC   
     WHERE EC.IDEEXAMENXCATEGORIA = (SELECT RP.IDEEXAMENXCATEGORIA
                                     FROM RECL_PERS_EXAM_CAT RP
                                     WHERE RP.IDERECLPERSOEXAMNCAT = p_idReclPerExaCat);
      
   EXCEPTION
     WHEN OTHERS THEN
      p_RetVal:=0;
   END;
      
 END SP_OBTENER_IDECATEGORIA;
 
    /* ------------------------------------------------------------
    Nombre      : FN_OBTENER_NOMBRE_POST
    Proposito   : Obtiene el nombre concatenado de un postulante
    
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      08/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  FUNCTION FN_OBTENER_NOMBRE_POST(p_idReclutaPersona    IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE
                                   ) RETURN VARCHAR2 IS
  c_nombreCompleto   VARCHAR2(250);
 BEGIN
     
   BEGIN 
    
     SELECT P.APEPATERNO||' '||P.APEMATERNO||', '|| P.PRINOMBRE||' '||P.SEGNOMBRE
     INTO c_nombreCompleto
     FROM POSTULANTE P    
     WHERE P.IDEPOSTULANTE = (SELECT RP.IDEPOSTULANTE
                              FROM RECLUTAMIENTO_PERSONA RP
                              WHERE RP.IDERECLUTAPERSONA = p_idReclutaPersona);
      
     
   EXCEPTION
     WHEN OTHERS THEN
      c_nombreCompleto:= '';
   END;
      
   RETURN c_nombreCompleto;
   
 END FN_OBTENER_NOMBRE_POST;
 
  /* ------------------------------------------------------------
    Nombre      : SP_CONOCIMIENTOS_PUBLICA
    Proposito   : Obtiene descripcion de conocimientos del cargo para
                  la publicaci?n
    
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      08/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_CONOCIMIENTOS_PUBLICA(p_ideCargo    IN CARGO.IDECARGO%TYPE,
                                    p_cRetVal     OUT SYS_REFCURSOR) IS

 BEGIN 
    
   OPEN p_cRetVal FOR
      SELECT CG.IDECONOGENCARGO, CASE WHEN (CG.TIPCONOFIMATICA IS NOT NULL) 
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL =  27
                  AND DG.VALOR = CG.TIPCONOFIMATICA)
            WHEN (CG.TIPIDIOMA IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 30
                  AND DG.VALOR = CG.TIPIDIOMA)
            WHEN (CG.TIPCONOGENERAL IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 32
                  AND DG.VALOR = CG.TIPCONOGENERAL)
           END AS TIPOCONO, 
           
           CASE WHEN (CG.TIPNOMOFIMATICA IS NOT NULL) 
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL =  28
                  AND DG.VALOR = CG.TIPNOMOFIMATICA)
            WHEN (CG.TIPCONOCIDIOMA IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 31
                  AND DG.VALOR = CG.TIPCONOCIDIOMA)
            WHEN (CG.TIPNOMCONOCGRALES IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 32
                  AND DG.VALOR = CG.TIPNOMCONOCGRALES)
            ELSE CG.NOMCONOCGRALES
           END AS NOMBRE    
      FROM CONOGENERAL_CARGO CG
      WHERE CG.IDECARGO = p_ideCargo;
   
 END SP_CONOCIMIENTOS_PUBLICA;
 
   /* ------------------------------------------------------------
    Nombre      : SP_CONOCIMIENTOS_SOLREQ_PUBLICA
    Proposito   : Obtiene descripcion de conocimientos del cargo para
                  la publicaci?n
    
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      08/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_CONO_SOLREQ_PUBLICA(p_ideSolReq    IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                   p_cRetVal     OUT SYS_REFCURSOR) IS

 BEGIN 
    
   OPEN p_cRetVal FOR
      SELECT CG.IDECONOGENSOLREQ, CASE WHEN (CG.TIPCONOFIMATICA IS NOT NULL) 
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL =  27
                  AND DG.VALOR = CG.TIPCONOFIMATICA)
            WHEN (CG.TIPIDIOMA IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 30
                  AND DG.VALOR = CG.TIPIDIOMA)
            WHEN (CG.TIPCONOGENERAL IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 32
                  AND DG.VALOR = CG.TIPCONOGENERAL)
           END AS TIPOCONO, 
           
           CASE WHEN (CG.TIPNOMOFIMATICA IS NOT NULL) 
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL =  28
                  AND DG.VALOR = CG.TIPNOMOFIMATICA)
            WHEN (CG.TIPCONOCIDIOMA IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 31
                  AND DG.VALOR = CG.TIPCONOCIDIOMA)
            WHEN (CG.TIPNOMCONOCGRALES IS NOT NULL)
            THEN (SELECT DG.DESCRIPCION 
                  FROM DETALLE_GENERAL DG 
                  WHERE DG.IDEGENERAL = 32
                  AND DG.VALOR = CG.TIPNOMCONOCGRALES)
            ELSE CG.NOMCONOCGRALES
           END AS NOMBRE    
      FROM CONOGENERAL_SOLREQ CG
      WHERE CG.IDESOLREQPERSONAL = p_ideSolReq;
   
 END SP_CONO_SOLREQ_PUBLICA;
 
     /* ------------------------------------------------------------
    Nombre      : SP_LISTAR_USUARIOS
    Proposito   : obtiene la lista de usuarios activos por sede
    
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      15/05/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_LISTAR_USUARIOS(p_apePaterno    IN USUARIO.DSCAPEPATERNO%TYPE,
                              p_apeMaterno    IN USUARIO.DSCAPEMATERNO%TYPE,
                              p_nombres       IN USUARIO.DSCNOMBRES%TYPE,
                              p_idRol         IN ROL.IDROL%TYPE,
                              p_idSede        IN SEDE.IDESEDE%TYPE,
                              p_retVal        OUT SYS_REFCURSOR) IS


c_apePaterno    USUARIO.DSCAPEPATERNO%TYPE;
c_apeMaterno    USUARIO.DSCAPEMATERNO%TYPE;
c_nombres       USUARIO.DSCNOMBRES%TYPE;
c_idRol         ROL.IDROL%TYPE;

 BEGIN
     
   BEGIN 
    
     IF p_apePaterno IS NULL THEN
        c_apePaterno := '';
      ELSE
        c_apePaterno := p_apePaterno;
      END IF;
      
      IF p_apeMaterno IS NULL THEN
         c_apeMaterno := '';
      ELSE
         c_apeMaterno:= p_apeMaterno;
      END IF;
      
      IF p_nombres IS NULL THEN
        c_nombres := '';
      ELSE
        c_nombres:= p_nombres;
      END IF;
      
      IF p_idRol IS NULL THEN
        c_idRol := '0';
      ELSE
        c_idRol:= p_idRol;
      END IF;
      
      OPEN p_retVal FOR
         SELECT U.IDUSUARIO, 
                    U.DSCAPEPATERNO, 
                    U.DSCAPEMATERNO, 
                    U.DSCNOMBRES, 
                    UR.IDROL , 
                    (SELECT R.DSCROL 
                     FROM ROL R 
                     WHERE R.IDROL = UR.IDROL
                     ) DSCROL
             FROM USUARIO U , USUAROLSEDE UR 
             WHERE U.IDUSUARIO = UR.IDUSUARIO
             AND UR.IDESEDE = p_idSede
             AND (c_apePaterno = '' OR U.DSCAPEPATERNO LIKE '%'||c_apePaterno||'%')
             AND (c_apeMaterno = '' OR U.DSCAPEMATERNO LIKE '%'||c_apeMaterno||'%')
             AND (c_nombres = '' OR U.DSCNOMBRES LIKE '%'||c_nombres||'%')             
             AND (c_idRol = '0' OR UR.IDROL = c_idRol);
     
   EXCEPTION
     WHEN OTHERS THEN
      p_retVal:= NULL;
   END;
      
          
      
   
 END SP_LISTAR_USUARIOS;

END PR_INTRANET;
/
