CREATE OR REPLACE PACKAGE PR_REQUERIMIENTOS is
--Constantes de estado de reclutamiento persona examen categoria

  P_ESTCAT_PENDIENTE        CONSTANT VARCHAR2(2) := '01';
  P_ESTCAT_EVALUADO         CONSTANT VARCHAR2(2) := '02';
  P_ESTCAT_FINALIZADO       CONSTANT VARCHAR2(2) := '03';
   
  P_ESTEXAMEN_EVALUADO      CONSTANT VARCHAR2(2) := '03';
  P_ESTEXAMEN_APROBADO      CONSTANT VARCHAR2(2) := '04';
  P_ESTEXAMEN_DESAPROBADO   CONSTANT VARCHAR2(2) := '05';
  
  P_TIPEXA_EXAMEN           CONSTANT VARCHAR2(2) := '01';
  P_TIPEXA_ENTREVISTA       CONSTANT VARCHAR2(2) := '02';
  P_TIPEXA_EVALUACION       CONSTANT VARCHAR2(2) := '04';
  
  ---ETAPA DE SOLICITUDES
  P_TIPETAPA_RECHAZADO      CONSTANT VARCHAR2(2) := '09';

-----------------------------------------------------------------

PROCEDURE SP_INSERTAR_NUEVO(p_ideSede           IN SEDE.IDESEDE%TYPE,
                            p_ideArea           IN AREA.IDEAREA%TYPE,
                            p_codCargo          IN SOLNUEVO_CARGO.CODCARGO%TYPE,
                            p_nomCargo          IN SOLNUEVO_CARGO.NOMBRE%TYPE,
                            p_descCargo         IN SOLNUEVO_CARGO.DESCRIPCION%TYPE,
                            p_numPosiciones     IN SOLNUEVO_CARGO.NUMPOSICIONES%TYPE,
                            p_tipRangoSalario   IN SOLNUEVO_CARGO.TIPRANSALARIO%TYPE,
                            p_ideDependencia    IN SOLNUEVO_CARGO.IDEDEPENDENCIA%TYPE,
                            p_ideDepartamento   IN SOLNUEVO_CARGO.IDEDEPARTAMENTO%TYPE,
                            p_estudios          IN SOLNUEVO_CARGO.ESTUDIOS%TYPE,
                            p_funciones         IN SOLNUEVO_CARGO.FUNCIONES%TYPE,
                            p_competencias      IN SOLNUEVO_CARGO.COMPETENCIAS%TYPE,
                            p_observacion       IN SOLNUEVO_CARGO.OBSERVACIONES%TYPE,                            
                            p_ideUsuarioSuceso  IN USUARIO.IDUSUARIO%TYPE,
                            p_ideRolSuceso      IN ROL.IDROL%TYPE,
                            p_ideRolResponsable IN ROL.IDROL%TYPE,
                            p_indArea           IN VARCHAR2,
                            p_usuarioCreacion   IN SOLNUEVO_CARGO.USRCREACION%TYPE,
                            p_etapa             IN DETALLE_GENERAL.VALOR%TYPE,
                            p_ideUsuarioResp    OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE);
                            
PROCEDURE SP_INSERTAR_LOG(p_ideSolicitudNuevo     IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                          p_ideSede               IN SEDE.IDESEDE%TYPE,
                          p_ideArea               IN AREA.IDEAREA%TYPE,
                          p_ideUsuarioSuceso      IN LOGSOLNUEVO_CARGO.USRSUCESO%TYPE,
                          p_ideRolSuceso          IN LOGSOLNUEVO_CARGO.ROLSUCESO%TYPE,
                          p_ideRolResponsable     IN LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                          p_Observacion           IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                          p_indArea               IN VARCHAR2,
                          p_etapa                 IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE, 
                          p_ideUsuarioRespPublic  IN LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE,
                          p_ideUsuarioResp        OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE);                         
                          
PROCEDURE SP_GET_CARGOS(p_idSede   IN SEDE.IDESEDE%TYPE,
                          p_cRetVal  OUT sys_refcursor);
                              
PROCEDURE SP_OBTENER_ETAPA_SOLICITUD(p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR);
                                     
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)
                            RETURN VARCHAR2;

FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                           p_tipoSolicitud   IN VARCHAR2)
                           RETURN VARCHAR2;
                           
FUNCTION FN_VERIFICAR_CODCARGO(p_codCargo IN SOLNUEVO_CARGO.CODCARGO%TYPE,
                               p_idSede   IN SEDE.IDESEDE%TYPE)RETURN NUMBER; 

PROCEDURE COPIA_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE,
                             p_ideSolReqPersonal IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_usrCreacion IN SOLREQ_PERSONAL.USRCREACION%TYPE);
                             
PROCEDURE SP_MANTENIMIENTO_CARGO(p_ideCargo          IN CARGO.IDECARGO%TYPE,
                                 p_usrCreacion       IN SOLREQ_PERSONAL.USRCREACION%TYPE,
                                 p_idCargoCopia     OUT CARGO.IDECARGO%TYPE);
                             
PROCEDURE SP_INSERTAR_AMPLIACION(p_ideCargo         IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                                p_ideSede          IN SEDE.IDESEDE%TYPE,
                                p_ideDependencia   IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                p_ideDepartamento  IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                p_ideArea          IN AREA.IDEAREA%TYPE,
                                p_numVacantes      IN SOLREQ_PERSONAL.NUMVACANTES%TYPE,
                                p_motivo           IN SOLREQ_PERSONAL.MOTIVO%TYPE,
                                p_tipoPuesto       IN SOLREQ_PERSONAL.TIPPUESTO%TYPE, 
                                p_observacion      IN SOLREQ_PERSONAL.OBSERVACION%TYPE,
                                p_ideUsuarioSuceso IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso     IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_cEtapa           IN DETALLE_GENERAL.VALOR%TYPE,
                                p_responsableSig   IN ROL.IDROL%TYPE,
                                p_tipoSolicitud    IN SOLREQ_PERSONAL.TIPSOL%TYPE,
                                p_indicArea        IN VARCHAR2,
                                p_cRetVal          OUT NUMBER); 
                                
PROCEDURE SP_RESPONSABLE_PUBLICACION(p_idSolicitudNuevo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_idSede           IN SEDE.IDESEDE%TYPE,
                                     p_idUsuarioResp    OUT USUARIO.IDUSUARIO%TYPE,
                                     p_idRolResp        OUT ROL.IDROL%TYPE);
                                
PROCEDURE SP_DETERMINAR_RESPONSABLE(p_idSolicitud   IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_idSede        IN SEDE.IDESEDE%TYPE,
                                    p_idUsuarioResp OUT USUARIO.IDUSUARIO%TYPE,
                                    p_idRolResp     OUT ROL.IDROL%TYPE); 
                                   
PROCEDURE SP_OBTENER_ETAPA_SOLREQ(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetVal OUT SYS_REFCURSOR);
                                  
PROCEDURE SP_INSERTAR_APROB_AMP(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_ideRolResponsable   IN ROL.IDROL%TYPE,
                                p_ideSede             IN SEDE.IDESEDE%TYPE,
                                p_ideArea             IN AREA.IDEAREA%TYPE,
                                p_indicArea           IN VARCHAR2,
                                p_tipoEtapa           IN LOGSOLREQ_PERSONAL.TIPETAPA%TYPE,
                                p_usuarioSuceso       IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso        IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_observacion         IN LOGSOLREQ_PERSONAL.OBSERVACION%TYPE,
                                c_ideUsuarioResp      IN USUARIO.IDUSUARIO%TYPE,
                                c_retVal              OUT USUARIO.IDUSUARIO%TYPE);
                                
PROCEDURE SP_LISTACARGOS(p_idSede IN SEDE.IDESEDE%TYPE,
                           p_cRetVal OUT SYS_REFCURSOR);
                                
PROCEDURE SP_LISTA_SOLGRAL(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                           p_ideSede         IN SEDE.IDESEDE%TYPE,
                           p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                           p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                           p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                           p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                           p_cTipResp        in NUMBER,   
                           p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                           p_cFecIni         IN VARCHAR2,
                           p_cFeFin          IN VARCHAR2,
                           p_cTipoSolicitud  IN VARCHAR2,
                           p_cCodSolicitud   IN VARCHAR2,
                           p_cRetVal         OUT SYS_REFCURSOR);

PROCEDURE SP_LISTA_SOLNUEVO(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                            p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                            p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                            p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                            p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                            p_cTipResp        in NUMBER,   
                            p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                            p_cFecIni         IN VARCHAR2,
                            p_cFeFin          IN VARCHAR2,
                            p_cRetVal         OUT SYS_REFCURSOR);
                            
PROCEDURE SP_LISTA_CARGOS_MANT(p_nCodCargo        IN CARGO.CODCARGO%TYPE,
                               p_nIdDependencia   IN CARGO.Idedependencia%TYPE,
                               p_nIdDepartamento  IN CARGO.Idedepartamento%TYPE,
                               p_nIdArea          IN CARGO.Idearea%TYPE,                        
                               p_cFecIni          IN VARCHAR2,
                               p_cFeFin           IN VARCHAR2,
                               p_cEstado          IN CARGO.ESTACTIVO%TYPE,
                               p_cideSede         IN CARGO.IDESEDE%TYPE,
                               p_cRetCursor       OUT SYS_REFCURSOR);
                          
PROCEDURE SP_CONSULTA_EDITAR_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE,
                                   p_cRetVal OUT VARCHAR2);
                                   
PROCEDURE SP_GENERAR_EVAL_REQ_POSTUL(p_idePostulante   IN POSTULANTE.IDEPOSTULANTE%TYPE,
                                     p_ideReclutPost   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                     p_usuarioCreacion IN RECLU_PERSO_EXAMEN.USRCREACION%TYPE,
                                     p_cuExamenes      OUT SYS_REFCURSOR);
                                     
PROCEDURE SP_CALIFICAR_EXAMEN(p_ideReclPerExaCat  IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                              p_ideReclutaPersona IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                              p_usuarioCreacion   IN RECLU_PERSO_EXAMEN.USRCREACION%TYPE,
                              p_retVal            OUT NUMBER);
                              
PROCEDURE SP_RECUPERAR_EXAMEN(p_ideReclutaPersona   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                              p_iderecluPersExamen  IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                              dtExamen              OUT SYS_REFCURSOR,
                              dtCategoriaExamen     OUT SYS_REFCURSOR,
                              dtCategoriaSubCatego  OUT SYS_REFCURSOR,
                              dtCriterioAlternativa OUT SYS_REFCURSOR,
                              dtAlternativas        OUT SYS_REFCURSOR);

PROCEDURE SP_REPORTE_POSTULANTESBD(p_nombreCargo     IN CARGO.NOMCARGO%TYPE,
                                   p_areaEstudio     IN ESTUDIOS_POSTULANTE.TIPAREA%TYPE,
                                   p_rangoSalario    IN DETALLE_GENERAL.VALOR%TYPE,
                                   p_departamento    IN UBIGEO.IDEUBIGEO%TYPE,                        
                                   p_provincia       IN UBIGEO.IDEUBIGEO%TYPE,
                                   p_distrito        IN UBIGEO.IDEUBIGEO%TYPE,
                                   p_fecDesde        IN VARCHAR2,
                                   p_fecHasta        IN VARCHAR2,
                                   p_edadInicio      IN NUMBER,
                                   p_edadFin         IN NUMBER,
                                   p_cRetVal         OUT SYS_REFCURSOR);  
                                   
FUNCTION FN_TOTAL_PUNTAJE_EXAMEN(p_ideReclutaPersona IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE
                                 )RETURN NUMBER; 
                                  
PROCEDURE SP_REPORTE_POST_POTENCIAL(p_ideCargo        IN CARGO.IDECARGO%TYPE,
                                    p_areaEstudio     IN DETALLE_GENERAL.VALOR%TYPE,
                                    p_rangoSalario    IN DETALLE_GENERAL.VALOR%TYPE,
                                    p_ideSede         IN SEDE.IDESEDE%TYPE,                        
                                    p_ideDependencia  IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                    p_ideDepartamento IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                    p_ideArea         IN AREA.IDEAREA%TYPE,
                                    p_fecDesde        IN VARCHAR2,
                                    p_fecHasta        IN VARCHAR2,
                                    p_edadInicio      IN NUMBER,
                                    p_edadFin         IN NUMBER,
                                    p_cRetVal         OUT SYS_REFCURSOR);  
                                    
                                    
PROCEDURE SP_CARGOS_MANT(p_ideSede  IN SEDE.IDESEDE%TYPE,
                         p_cRetVal  OUT SYS_REFCURSOR);
                         
                         
PROCEDURE SP_CARGOS_SEDE(p_ideSede IN SEDE.IDESEDE%TYPE,
                         p_cRetVal  OUT SYS_REFCURSOR); 
                         
                         
FUNCTION FN_EXISTE_RESULTADO_EXAMEN(p_ideReclutaExamen  IN RECLU_PERSO_EXAMEN.IDERECLUPERSOEXAMEN%TYPE
                                    ) RETURN VARCHAR;  
                                    
 PROCEDURE SP_GET_CARGOXSEDE(p_nIdSede         IN NUMBER,
                              p_nIdDependencia  IN NUMBER,
                              p_nIdDepartamento IN NUMBER,
                              p_nIdArea         IN NUMBER,
                              p_cRetVal         OUT SYS_REFCURSOR);                      

END PR_REQUERIMIENTOS;
/
CREATE OR REPLACE PACKAGE BODY PR_REQUERIMIENTOS is

/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_NUEVO
    Proposito   : Actualizar el estado de la Solicitud y agregar nueva etapa
                  dependiendo del caso.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      20/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_INSERTAR_NUEVO(p_ideSede           IN SEDE.IDESEDE%TYPE,
                            p_ideArea           IN AREA.IDEAREA%TYPE,
                            p_codCargo          IN SOLNUEVO_CARGO.CODCARGO%TYPE,
                            p_nomCargo          IN SOLNUEVO_CARGO.NOMBRE%TYPE,
                            p_descCargo         IN SOLNUEVO_CARGO.DESCRIPCION%TYPE,
                            p_numPosiciones     IN SOLNUEVO_CARGO.NUMPOSICIONES%TYPE,
                            p_tipRangoSalario   IN SOLNUEVO_CARGO.TIPRANSALARIO%TYPE,
                            p_ideDependencia    IN SOLNUEVO_CARGO.IDEDEPENDENCIA%TYPE,
                            p_ideDepartamento   IN SOLNUEVO_CARGO.IDEDEPARTAMENTO%TYPE,
                            p_estudios          IN SOLNUEVO_CARGO.ESTUDIOS%TYPE,
                            p_funciones         IN SOLNUEVO_CARGO.FUNCIONES%TYPE,
                            p_competencias      IN SOLNUEVO_CARGO.COMPETENCIAS%TYPE,
                            p_observacion       IN SOLNUEVO_CARGO.OBSERVACIONES%TYPE,                            
                            p_ideUsuarioSuceso  IN USUARIO.IDUSUARIO%TYPE,
                            p_ideRolSuceso      IN ROL.IDROL%TYPE,
                            p_ideRolResponsable IN ROL.IDROL%TYPE,
                            p_indArea           IN VARCHAR2,
                            p_usuarioCreacion   IN SOLNUEVO_CARGO.USRCREACION%TYPE,
                            p_etapa             IN DETALLE_GENERAL.VALOR%TYPE,
                            p_ideUsuarioResp    OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE)IS
                             
                        
c_idesolCargo SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE;
qWhere VARCHAR2(100);
qQuery VARCHAR2(1000);

BEGIN   
  
  IF (p_indArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
    ELSE
    qWhere :='';
  END IF;
    
  qQuery := 'SELECT UR.IDUSUARIO '||
            'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
            'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
            'AND UR.IDROL = R.IDROL '||
            'AND UR.IDESEDE = '||p_ideSede||' '|| 
            'AND R.IDROL = '||p_ideRolResponsable||' '||
             qWhere ||' '||
            'AND UN.FLGESTADO = ''A'' '||
            'AND ROWNUM <= 1';
                     
  EXECUTE IMMEDIATE qQuery INTO p_ideUsuarioResp;
  
  SELECT IDESOLNUEVOCARGO_SQ.NEXTVAL
  INTO c_idesolCargo
  FROM DUAL; 
  
  BEGIN
    INSERT INTO SOLNUEVO_CARGO
    (IDESOLNUEVOCARGO,IDESEDE,CODCARGO,NOMBRE,DESCRIPCION,NUMPOSICIONES,TIPRANSALARIO,IDEDEPENDENCIA,IDEDEPARTAMENTO,IDEAREA,ESTUDIOS,FUNCIONES,COMPETENCIAS,OBSERVACIONES,ESTACTIVO,USRCREACION,FECCREACION,TIPETAPA)
    VALUES(c_idesolCargo,p_ideSede,p_codCargo,p_nomCargo,p_descCargo,p_numPosiciones,p_tipRangoSalario,p_ideDependencia,p_ideDepartamento,p_ideArea,p_estudios,p_funciones,p_competencias,p_observacion,'A',p_usuarioCreacion,SYSDATE,p_etapa);
    
    --INSERTAR EL LOG DE SOLICITUD
    INSERT INTO LOGSOLNUEVO_CARGO
    (IDELOGSOLNUEVOCARGO,IDESOLNUEVOCARGO,TIPETAPA,ROLRESPONSABLE, USRESPONSABLE,FECSUCESO,USRSUCESO,ROLSUCESO)
    VALUES (IDELOGSOLNUEVOCARGO_SQ.NEXTVAL,c_idesolCargo,p_etapa,p_ideRolResponsable,p_ideUsuarioResp,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso);
   
    COMMIT;   
     
  EXCEPTION
    WHEN OTHERS THEN  
   -- p_ideUsuarioResp:= -1;
    ROLLBACK;
  END;
   
END SP_INSERTAR_NUEVO;

/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_LOG
    Proposito   : Actualizar el estado de la Solicitud y agregar nueva etapa
                  dependiendo del caso.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      20/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_INSERTAR_LOG(p_ideSolicitudNuevo     IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                          p_ideSede               IN SEDE.IDESEDE%TYPE,
                          p_ideArea               IN AREA.IDEAREA%TYPE,
                          p_ideUsuarioSuceso      IN LOGSOLNUEVO_CARGO.USRSUCESO%TYPE,
                          p_ideRolSuceso          IN LOGSOLNUEVO_CARGO.ROLSUCESO%TYPE,
                          p_ideRolResponsable     IN LOGSOLNUEVO_CARGO.ROLRESPONSABLE%TYPE,
                          p_Observacion           IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                          p_indArea               IN VARCHAR2,
                          p_etapa                 IN LOGSOLNUEVO_CARGO.TIPETAPA%TYPE, 
                          p_ideUsuarioRespPublic  IN LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE,
                          p_ideUsuarioResp        OUT LOGSOLNUEVO_CARGO.USRESPONSABLE%TYPE)IS
                             
                        
c_idLogSolicitud LOGSOLNUEVO_CARGO.IDELOGSOLNUEVOCARGO%TYPE;
qWhere VARCHAR2(100);
qQuery VARCHAR2(1000);
c_idCargo CARGO.IDECARGO%TYPE;
c_observacion LOGSOLNUEVO_CARGO.OBSERVACION%TYPE;

BEGIN   
  
  IF (p_ideRolResponsable != 0) THEN
  
  IF (p_indArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
    ELSE
    qWhere :='';
  END IF;
    
  qQuery := 'SELECT UR.IDUSUARIO '||
            'FROM USUARIO U,USUAROLSEDE UR, USUARIO_NIVEL UN '||
            'WHERE UN.IDUSUARIO = UR.IDUSUARIO '||
            'AND U.IDUSUARIO = UR.IDUSUARIO '||
            'AND UR.IDESEDE = '||p_ideSede||' '||         
            'AND UR.IDROL = '||p_ideRolResponsable||' '||
             qWhere ||' '||
            'AND UN.FLGESTADO = ''A'' '||
            'AND ROWNUM <= 1';
  
/*  'SELECT UR.IDUSUARIO '||
            'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
            'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
            'AND UR.IDROL = R.IDROL '||
            'AND UR.IDESEDE = '||p_ideSede||' '|| 
            'AND R.IDROL = '||p_ideRolResponsable||' '||
             qWhere ||' '||
            'AND UN.FLGESTADO = ''A'' '||
            'AND ROWNUM <= 1';*/
       
                
  EXECUTE IMMEDIATE qQuery INTO p_ideUsuarioResp;
  
  ELSE
    p_ideUsuarioResp:=0;
  
  END IF;
  
  IF p_ideUsuarioRespPublic != 0 THEN
    p_ideUsuarioResp := p_ideUsuarioRespPublic;
  END IF;
  
 BEGIN
    SELECT IDELOGSOLNUEVOCARGO_SQ.NEXTVAL
    INTO c_idLogSolicitud
    FROM DUAL;   
 
    SELECT NVL(SN.IDECARGO,0)
    INTO c_idCargo
    FROM SOLNUEVO_CARGO SN
    WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitudNuevo;
    
    IF p_Observacion IS NULL THEN
         c_observacion:='';
      ELSE
        c_observacion:=p_Observacion;
      END IF;
      
    
    IF (p_etapa = P_TIPETAPA_RECHAZADO)THEN
    
      
      
      INSERT INTO LOGSOLNUEVO_CARGO
      (IDELOGSOLNUEVOCARGO,IDESOLNUEVOCARGO,TIPETAPA,OBSERVACION ,FECSUCESO,USRSUCESO,ROLSUCESO)
      VALUES (c_idLogSolicitud,p_ideSolicitudNuevo,p_etapa,c_observacion,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso);
   
      UPDATE SOLNUEVO_CARGO SN
      SET SN.TIPETAPA = p_etapa,
          SN.ESTACTIVO = 'I'
      WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitudNuevo;
    ELSE
    
      INSERT INTO LOGSOLNUEVO_CARGO
      (IDELOGSOLNUEVOCARGO,IDESOLNUEVOCARGO,TIPETAPA,OBSERVACION,ROLRESPONSABLE, USRESPONSABLE,FECSUCESO,USRSUCESO,ROLSUCESO)
      VALUES (c_idLogSolicitud,p_ideSolicitudNuevo,p_etapa,c_observacion, p_ideRolResponsable,p_ideUsuarioResp,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso);

      UPDATE SOLNUEVO_CARGO SN
      SET SN.TIPETAPA = p_etapa
      WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitudNuevo;
      
      --ACTUALIZAR EN EL CARGO
      IF (c_idCargo != 0)THEN
        UPDATE CARGO CA
        SET CA.TIPETAPA = p_etapa
        WHERE  CA.IDECARGO = c_idCargo;
      END IF;
      
      
      
    END IF;
    
    COMMIT;   
     
 EXCEPTION
    WHEN OTHERS THEN  
    p_ideUsuarioResp:= -1;
    ROLLBACK;
  END;
   
END SP_INSERTAR_LOG;

  /* ------------------------------------------------------------
    Nombre      : SP_GET_CARGOS
    Proposito   : Obtiene la lista de cargos para la lista de ampliacion
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_GET_CARGOS(p_idSede   IN SEDE.IDESEDE%TYPE,
                          p_cRetVal  OUT SYS_REFCURSOR) IS
  BEGIN
  
    OPEN P_CRETVAL FOR
       SELECT DISTINCT C.IDECARGO, C.NOMCARGO
        FROM CARGO C
       WHERE C.ESTACTIVO = 'A'
         AND NVL(C.VERSION,1) = (SELECT NVL(MAX(C1.VERSION),1) FROM CARGO C1 WHERE C1.ESTACTIVO='A')
         AND C.IDESEDE = p_idSede
       ORDER BY C.NOMCARGO;

  
  END SP_GET_CARGOS;
/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_ETAPA_SOLICITUD
    Proposito   : Obtener la etapa y estado actual de la solicitud de cargo.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_OBTENER_ETAPA_SOLICITUD(p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR)IS

BEGIN

  BEGIN
  OPEN p_cRetVal FOR
  SELECT LG.TIPETAPA,LG.ROLRESPONSABLE
  FROM LOGSOLNUEVO_CARGO LG   
  WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                     FROM  LOGSOLNUEVO_CARGO SN
                     WHERE SN.IDESOLNUEVOCARGO = p_ideSolCargo)
  AND LG.IDESOLNUEVOCARGO =  p_ideSolCargo; 

  EXCEPTION
  WHEN OTHERS THEN
    p_cRetVal:=NULL;
  END;   
  
END SP_OBTENER_ETAPA_SOLICITUD;


/* ------------------------------------------------------------
    Nombre      : FN_RESPONSABLE_SOL
    Proposito   : Obtener la etapa y estado actual de la solicitud de cargo.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud    IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN
    
    SELECT R.DSCROL
    INTO c_responsable
    FROM LOGSOLNUEVO_CARGO LG , ROL R   
    WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                       FROM  LOGSOLNUEVO_CARGO SN
                       WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
    AND R.IDROL = LG.ROLRESPONSABLE
    AND LG.IDESOLNUEVOCARGO = p_ideSolicitud;
    
  RETURN NVL(c_responsable,'');  
  
END FN_RESPONSABLE_SOL;

/* ------------------------------------------------------------
    Nombre      : FN_NOMRESPONS_SOL
    Proposito   : Obtener el nombre del responsable del suceso
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                           p_tipoSolicitud   IN VARCHAR2)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN

  IF p_tipoSolicitud = 'N' THEN
  
    SELECT US.DSCNOMBRES||' ' ||US.DSCAPEPATERNO||' '||US.DSCAPEMATERNO
    INTO c_responsable
    FROM LOGSOLNUEVO_CARGO LG , USUARIO US   
    WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                       FROM  LOGSOLNUEVO_CARGO SN
                       WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
    AND US.IDUSUARIO = LG.USRESPONSABLE
    AND LG.IDESOLNUEVOCARGO = p_ideSolicitud;
  ELSE
    
    SELECT US.DSCNOMBRES||' ' ||US.DSCAPEPATERNO||' '||US.DSCAPEMATERNO
    INTO c_responsable
    FROM LOGSOLREQ_PERSONAL LG , USUARIO US   
    WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                       FROM  LOGSOLREQ_PERSONAL SN
                       WHERE SN.IDESOLREQPERSONAL = p_ideSolicitud)
    AND US.IDUSUARIO = LG.USRESPONSABLE
    AND LG.IDESOLREQPERSONAL = p_ideSolicitud;
    
  END IF;
  RETURN NVL(c_responsable,'');  
  
END FN_NOMRESPONS_SOL;


/* ------------------------------------------------------------
    Nombre      : FN_VERIFICAR_CODCARGO
    Proposito   : verificar que el codigo de cargo este disponible
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      27/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_VERIFICAR_CODCARGO(p_codCargo IN SOLNUEVO_CARGO.CODCARGO%TYPE,
                               p_idSede   IN SEDE.IDESEDE%TYPE)RETURN NUMBER 

IS
cCount NUMBER;

BEGIN
  SELECT COUNT(SN.CODCARGO) INTO cCount
  FROM SOLNUEVO_CARGO SN   
  WHERE UPPER(SN.CODCARGO) = UPPER(p_codCargo);
  --AND   SN.IDESEDE = p_idSede;

  RETURN NVL(cCount,0);
  
END FN_VERIFICAR_CODCARGO;

/* ------------------------------------------------------------
    Nombre      : OBTENER_CONOCIMIENTOS_CARGO
    Proposito   : Obtener los conocimientos generales
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */

/*PROCEDURE OBTENER_CONOCIMIENTOS_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE)IS

CURSOR c_Conocimientos IS
   SELECT *
   FROM CONOGENERAL_CARGO CG
   WHERE CG.IDECARGO = p_ideCargo;
      
BEGIN

   OPEN c_Conocimientos;
   FETCH c_Tabla INTO cValorTabla;
   IF c_Tabla%NOTFOUND THEN 
     IF p_cCodTabla IS NOT NULL THEN 
       cValorTabla := '';
     END IF;  
   END IF;
   CLOSE c_Tabla;

     RETURN cValorTabla;
END OBTENER_CONOCIMIENTOS_CARGO;*/


/* ------------------------------------------------------------
    Nombre      : COPIA_CARGO
    Proposito   : Hacer una copia del cargo a las tablas de 
                  ampliacion de cargo
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      28/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE COPIA_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE,
                             p_ideSolReqPersonal IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_usrCreacion IN SOLREQ_PERSONAL.USRCREACION%TYPE) IS


c_ideSolReqPersonal SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE;
CURSOR c_Ofrecemos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT O.TIPOFRECIMIENTO 
  FROM OFRECEMOS_CARGO O 
  WHERE O.IDECARGO =  p_ideCargo;
  
CURSOR c_competencias(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT C.TIPCOMPETEN , C.PUNTAJE 
  FROM COMPETENCIAS_CARGO C 
  WHERE C.IDECARGO =  p_ideCargo;

CURSOR c_horarios(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT H.TIPHORARIO, H.PUNTHORARIO 
  FROM HORARIO_CARGO H 
  WHERE H.IDECARGO =  p_ideCargo;

CURSOR c_ubigeos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT UB.IDEUBIGEO, UB.PUNTUBIGEO 
  FROM UBIGEO_CARGO UB 
  WHERE UB.IDECARGO =  p_ideCargo;
  
CURSOR c_centrosEstudios(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT CE.TIPCENESTU, CE.TIPNOMCENESTU, CE.PUNTACENTROEST  
  FROM CENTROEST_CARGO CE 
  WHERE CE.IDECARGO =  p_ideCargo;

CURSOR c_nivelesAlcan(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT NA.TIPEDUCACION, NA.TIPAREAESTUDIO, NA.TIPNIVELCANZADO , NA.PUNTNIVESTUDIO, NA.CICLOSEMESTRE  
  FROM NIVELACADEMICO_CARGO NA 
  WHERE NA.IDECARGO =  p_ideCargo;

CURSOR c_conocimientos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT *  
  FROM CONOGENERAL_CARGO CG 
  WHERE CG.IDECARGO =  p_ideCargo;
  
CURSOR c_experiencias(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT *  
  FROM EXPERIENCIA_CARGO EC 
  WHERE EC.IDECARGO =  p_ideCargo;

CURSOR c_discapacidades(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT DC.TIPDISCAPA, DC.DESDISCAPA, DC.PUNTDISCAPA  
  FROM DISCAPACIDAD_CARGO DC 
  WHERE DC.IDECARGO =  p_ideCargo;
  
CURSOR c_evaluaciones(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT EC.IDEEXAMEVAL, EC.TIPEXAMEN, EC.NOTAMINEXAMEN, EC.TIPAREARESPON, EC.PUNTEXAMEN  
  FROM EVALUACION_CARGO EC 
  WHERE EC.IDECARGO =  p_ideCargo;
  
BEGIN
 c_ideSolReqPersonal:=p_ideSolReqPersonal;
 BEGIN
  UPDATE SOLREQ_PERSONAL S SET (S.IDECARGO,S.NOMCARGO,S.DESCARGO, S.SEXO,S.INDSEXO,S.EDADINICIO,S.EDADFIN,S.PUNTEDAD,S.INDEDAD,S.TIPRANGOSALARIO,S.PUNTSALARIO,S.OBJETIVOCARGO,S.FUNCIONESCARGO,S.OBSERVACIONCARGO,
         S.PUNTTOTPOSTUINTE,S.PUNTTOTEDAD,S.PUNTTOTSEXO,S.PUNTTOTSALARIO,S.PUNTTOTNIVELEST,S.PUNTTOTCENTROEST,S.PUNTTOTEXPLABORAL,S.PUNTTOTOFIMATI,S.PUNTTOTCONOIDIOMA,S.PUNTTOTCONOGEN,S.PUNTTOTDISCAPA,
         S.PUNTTOTHORARIO,S.PUNTTOTUBIGEO,S.PUNTTOTEXAMEN,S.PUNTMINEXAMEN,S.TIPREQUERIMIENTO,S.CANTPRESELEC,S.PUNTMINGRAL,S.CODCARGO)=(
         SELECT C.IDECARGO,C.NOMCARGO,C.DESCARGO ,C.SEXO,C.INDSEXO,C.EDADINICIO,C.EDADFIN,C.PUNTEDAD,C.INDEDAD,C.TIPRANGOSALARIO,C.PUNTSALARIO,C.OBJETIVOSCARGO,C.FUNCIONESCARGO,C.OBSERVACIONCARGO,
         C.PUNTTOTPOSTUINTE,C.PUNTTOTEDAD,C.PUNTTOTSEXO,C.PUNTTOTSALARIO,C.PUNTTOTNIVELEST,C.PUNTTOTCENTROEST,C.PUNTTOTEXPLABORAL,C.PUNTTOTOFIMATI,C.PUNTTOTIDIOMA,C.PUNTTOTCONOGEN,C.PUNTTOTDISCAPA,
         C.PUNTTOTHORARIO,C.PUNTTOTUBIGEO,C.PUNTTOTEXAMEN,C.PUNTMINEXAMEN,C.TIPREQUERIMIENTO,C.CANTPRESELEC,C.PUNTMINGRAL,C.CODCARGO
         FROM CARGO C WHERE C.IDECARGO = p_ideCargo)
  WHERE S.IDESOLREQPERSONAL = p_ideSolReqPersonal;
 
  FOR REG_OFRECEMOS IN c_Ofrecemos(p_ideCargo) LOOP        
     INSERT INTO OFRECEMOS_SOLREQ 
     (IDEOFRECEMOSSOLREQ,IDESOLREQPERSONAL,TIPOFRECIMIENTO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEOFRECEMOSSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_OFRECEMOS.TIPOFRECIMIENTO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_COMPETENCIAS IN c_competencias(p_ideCargo) LOOP        
     INSERT INTO COMPETENCIAS_SOLREQ 
     (IDECOMPETENCIASOLREQ,IDESOLREQPERSONAL,TIPCOMPETEN,ESTACTIVO,USRCREACION,FECCREACION,PUNTCOMPETENCIA)
     VALUES(IDEOFRECEMOSSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_COMPETENCIAS.TIPCOMPETEN,'A',p_usrCreacion,SYSDATE,REG_COMPETENCIAS.PUNTAJE);
  END LOOP;
  
  FOR REG_HORARIOS IN c_horarios(p_ideCargo) LOOP        
     INSERT INTO HORARIO_SOLREQ
     (IDEHORARIOSOLREQ,IDESOLREQPERSONAL,TIPHORARIO,PUNTHORARIO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEHORARIOSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_HORARIOS.TIPHORARIO, REG_HORARIOS.PUNTHORARIO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_UBIGEOS IN c_ubigeos(p_ideCargo) LOOP        
     INSERT INTO UBIGEO_SOLREQ
     (IDEUBIGEOSOLREQ,IDESOLREQPERSONAL,IDEUBIGEO,PUNTUBIGEO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEUBIGEOSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_UBIGEOS.IDEUBIGEO, REG_UBIGEOS.PUNTUBIGEO,'A',p_usrCreacion,SYSDATE );
  END LOOP;  
  
  FOR REG_CENTROEST IN c_centrosEstudios(p_ideCargo) LOOP        
     INSERT INTO CENTROEST_SOLREQ
     (IDECENTESTSOLREQ,IDESOLREQPERSONAL,TIPCENESTU,TIPNOMCENESTU,PUNTACENTROEST,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDECENTESTSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_CENTROEST.TIPCENESTU, REG_CENTROEST.TIPNOMCENESTU,REG_CENTROEST.PUNTACENTROEST ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_NIVELESALC IN c_nivelesAlcan(p_ideCargo) LOOP        
     INSERT INTO NIVELACADEMICO_SOLREQ
     (IDENIVELACADESOLREQ,IDESOLREQPERSONAL,TIPEDUCACION,TIPAREAESTUDIO,TIPNIVELCANZADO,CICLOSEMESTRE,PUNTNIVESTUDIO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDENIVELACADESOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_NIVELESALC.TIPEDUCACION, REG_NIVELESALC.TIPAREAESTUDIO,REG_NIVELESALC.TIPNIVELCANZADO,REG_NIVELESALC.CICLOSEMESTRE, REG_NIVELESALC.PUNTNIVESTUDIO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_CONOCIMIENTOS IN c_conocimientos(p_ideCargo) LOOP        
     INSERT INTO 
     CONOGENERAL_SOLREQ(IDECONOGENSOLREQ,IDESOLREQPERSONAL,TIPCONOFIMATICA,TIPNOMOFIMATICA,TIPIDIOMA,TIPCONOCIDIOMA,TIPCONOGENERAL,TIPNOMCONOCGRALES,NOMCONOCGRALES,TIPNIVELCONOCIMIENTO,INDCERTIFICACION,PUNTCONOCIMIENTO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDECONOGENSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_CONOCIMIENTOS.TIPCONOFIMATICA,REG_CONOCIMIENTOS.TIPNOMOFIMATICA,REG_CONOCIMIENTOS.TIPIDIOMA,REG_CONOCIMIENTOS.TIPCONOCIDIOMA,REG_CONOCIMIENTOS.TIPCONOGENERAL, REG_CONOCIMIENTOS.TIPNOMCONOCGRALES, REG_CONOCIMIENTOS.NOMCONOCGRALES, REG_CONOCIMIENTOS.TIPNIVELCONOCIMIENTO, REG_CONOCIMIENTOS.INDCERTIFICACION, REG_CONOCIMIENTOS.PUNTCONOCIMIENTO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_EXPERIENCIAS IN c_experiencias(p_ideCargo) LOOP        
     INSERT INTO EXPERIENCIA_SOLREQ
     (IDEEXPSOLREQ,IDESOLREQPERSONAL,TIPEXPLABORAL,CANTANHOEXP,CANTMESESEXP,PUNTEXPERIENCIA,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEEXPSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_EXPERIENCIAS.TIPEXPLABORAL,REG_EXPERIENCIAS.CANTANHOEXP,REG_EXPERIENCIAS.CANTMESESEXP,REG_EXPERIENCIAS.PUNTEXPERIENCIA ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_DISCAPACIDAD IN c_discapacidades(p_ideCargo) LOOP        
     INSERT INTO DISCAPACIDAD_SOLREQ
     (IDEDISCAPASOLREQ,IDESOLREQPERSONAL,TIPDISCAPA,DESDISCAPA,PUNTDISCAPA,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEDISCAPASOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_DISCAPACIDAD.TIPDISCAPA, REG_DISCAPACIDAD.DESDISCAPA, REG_DISCAPACIDAD.PUNTDISCAPA ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_EVALUACION IN c_evaluaciones(p_ideCargo) LOOP        
     INSERT INTO EVALUACION_SOLREQ
     (IDEEVALUACIONSOLREQ,IDESOLREQPERSONAL,IDEEXAMEVAL,TIPEXAMEN,NOTAMINEXAMEN,TIPAREARESPON,PUNTEXAMEN,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEEVALUACIONSOLREQ_SQ.NEXTVAL,c_ideSolReqPersonal, REG_EVALUACION.IDEEXAMEVAL, REG_EVALUACION.TIPEXAMEN, REG_EVALUACION.NOTAMINEXAMEN,REG_EVALUACION.TIPAREARESPON,REG_EVALUACION.PUNTEXAMEN,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  COMMIT;
  EXCEPTION 
     WHEN OTHERS THEN
     ROLLBACK;
  END;

END COPIA_CARGO;  


/* ------------------------------------------------------------
    Nombre      : SP_MANTENIMIENTO_CARGO
    Proposito   : Hacer una copia del cargo a las tablas de 
                  ampliacion de cargo
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      28/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_MANTENIMIENTO_CARGO(p_ideCargo          IN CARGO.IDECARGO%TYPE,
                                 p_usrCreacion       IN SOLREQ_PERSONAL.USRCREACION%TYPE,
                                 p_idCargoCopia      OUT CARGO.IDECARGO%TYPE) IS

c_ideCargo_sq       CARGO.IDECARGO%TYPE;
c_version           CARGO.VERSION%TYPE;

CURSOR c_Ofrecemos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT O.TIPOFRECIMIENTO 
  FROM OFRECEMOS_CARGO O 
  WHERE O.IDECARGO =  p_ideCargo;
  
CURSOR c_competencias(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT C.TIPCOMPETEN 
  FROM COMPETENCIAS_CARGO C 
  WHERE C.IDECARGO =  p_ideCargo;

CURSOR c_horarios(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT H.TIPHORARIO, H.PUNTHORARIO 
  FROM HORARIO_CARGO H 
  WHERE H.IDECARGO =  p_ideCargo;

CURSOR c_ubigeos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT UB.IDEUBIGEO, UB.PUNTUBIGEO 
  FROM UBIGEO_CARGO UB 
  WHERE UB.IDECARGO =  p_ideCargo;
  
CURSOR c_centrosEstudios(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT CE.TIPCENESTU, CE.TIPNOMCENESTU, CE.PUNTACENTROEST  
  FROM CENTROEST_CARGO CE 
  WHERE CE.IDECARGO =  p_ideCargo;

CURSOR c_nivelesAlcan(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT NA.TIPEDUCACION, NA.TIPAREAESTUDIO, NA.TIPNIVELCANZADO , NA.PUNTNIVESTUDIO, NA.CICLOSEMESTRE  
  FROM NIVELACADEMICO_CARGO NA 
  WHERE NA.IDECARGO =  p_ideCargo;

CURSOR c_conocimientos(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT *  
  FROM CONOGENERAL_CARGO CG 
  WHERE CG.IDECARGO =  p_ideCargo;
  
CURSOR c_experiencias(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT *  
  FROM EXPERIENCIA_CARGO EC 
  WHERE EC.IDECARGO =  p_ideCargo;

CURSOR c_discapacidades(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT DC.TIPDISCAPA, DC.DESDISCAPA, DC.PUNTDISCAPA  
  FROM DISCAPACIDAD_CARGO DC 
  WHERE DC.IDECARGO =  p_ideCargo;
  
CURSOR c_evaluaciones(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT EC.IDEEXAMEVAL, EC.TIPEXAMEN, EC.NOTAMINEXAMEN, EC.TIPAREARESPON, EC.PUNTEXAMEN  
  FROM EVALUACION_CARGO EC 
  WHERE EC.IDECARGO =  p_ideCargo;
  
BEGIN

 
 BEGIN
 
   SELECT IDECARGO_SQ.NEXTVAL
   INTO c_ideCargo_sq
   FROM DUAL;
   
   SELECT MAX(C.VERSION)+1
   INTO c_version
   FROM CARGO C
   WHERE C.CODCARGO = (SELECT CAR.CODCARGO FROM CARGO CAR WHERE CAR.IDECARGO = p_ideCargo);
      
  INSERT INTO CARGO 
  (IDECARGO,IDESEDE,CODCARGO,NOMCARGO,DESCARGO,IDEDEPENDENCIA,IDEDEPARTAMENTO, IDEAREA,NUMPOSICION,SEXO,INDSEXO,PUNTSEXO,EDADINICIO,EDADFIN,PUNTEDAD,INDEDAD, TIPRANGOSALARIO,PUNTSALARIO,TIPREQUERIMIENTO,OBJETIVOSCARGO,FUNCIONESCARGO,OBSERVACIONCARGO,PUNTTOTPOSTUINTE,
  PUNTTOTEDAD,PUNTTOTSEXO,PUNTTOTSALARIO,PUNTTOTNIVELEST,PUNTTOTCENTROEST,PUNTTOTEXPLABORAL,PUNTTOTOFIMATI,PUNTTOTIDIOMA,PUNTTOTCONOGEN,PUNTTOTDISCAPA,PUNTTOTHORARIO,PUNTTOTUBIGEO,PUNTTOTEXAMEN,PUNTMINEXAMEN,CANTPRESELEC,ESTACTIVO,
  USRCREACION,FECCREACION,TIPETAPA,PUNTMINGRAL,VERSION)
  SELECT c_ideCargo_sq,C.IDESEDE,C.CODCARGO,C.NOMCARGO,C.DESCARGO,C.IDEDEPENDENCIA, C.IDEDEPARTAMENTO, C.IDEAREA,C.NUMPOSICION,C.SEXO,C.INDSEXO,C.PUNTSEXO,C.EDADINICIO,C.EDADFIN,C.PUNTEDAD,C.INDEDAD,C.TIPRANGOSALARIO,C.PUNTSALARIO,C.TIPREQUERIMIENTO,C.OBJETIVOSCARGO,C.FUNCIONESCARGO,
  C.OBSERVACIONCARGO,C.PUNTTOTPOSTUINTE,C.PUNTTOTEDAD,C.PUNTTOTSEXO,C.PUNTTOTSALARIO,C.PUNTTOTNIVELEST,C.PUNTTOTCENTROEST,C.PUNTTOTEXPLABORAL,C.PUNTTOTOFIMATI,C.PUNTTOTIDIOMA,C.PUNTTOTCONOGEN,C.PUNTTOTDISCAPA,C.PUNTTOTHORARIO,C.PUNTTOTUBIGEO,
  C.PUNTTOTEXAMEN,C.PUNTMINEXAMEN,C.CANTPRESELEC,'A',p_usrCreacion,SYSDATE,C.TIPETAPA,C.PUNTMINGRAL, c_version 
  FROM CARGO C 
  WHERE C.IDECARGO = p_ideCargo;
 
  --cambiar el estado a las versiones anteriores excepto a la ultima creada 
  UPDATE CARGO C
  SET C.ESTACTIVO = 'I',C.USRMODIFICA = p_usrCreacion,C.FECMODIFICA = SYSDATE  
  WHERE C.CODCARGO = (SELECT CAR.CODCARGO FROM CARGO CAR WHERE CAR.IDECARGO = p_ideCargo);
 
  UPDATE CARGO C
  SET C.ESTACTIVO = 'A' 
  WHERE C.CODCARGO = (SELECT CAR.CODCARGO FROM CARGO CAR WHERE CAR.IDECARGO = p_ideCargo)
  AND C.VERSION = c_version;

  
  FOR REG_OFRECEMOS IN c_Ofrecemos(p_ideCargo) LOOP        
     INSERT INTO OFRECEMOS_CARGO 
     (IDEOFRECEMOSCARGO,IDECARGO,TIPOFRECIMIENTO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEOFRECEMOSCARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_OFRECEMOS.TIPOFRECIMIENTO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_COMPETENCIAS IN c_competencias(p_ideCargo) LOOP        
     INSERT INTO COMPETENCIAS_CARGO 
     (IDECOMPETENCIACARGO,IDECARGO,TIPCOMPETEN,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDECOMPETENCIACARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_COMPETENCIAS.TIPCOMPETEN,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_HORARIOS IN c_horarios(p_ideCargo) LOOP        
     INSERT INTO HORARIO_CARGO
     (IDEHORARIOCARGO,IDECARGO,TIPHORARIO,PUNTHORARIO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEHORARIOCARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_HORARIOS.TIPHORARIO, REG_HORARIOS.PUNTHORARIO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_UBIGEOS IN c_ubigeos(p_ideCargo) LOOP        
     INSERT INTO UBIGEO_CARGO
     (IDEUBIGEOCARGO,IDECARGO,IDEUBIGEO,PUNTUBIGEO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEUBIGEOCARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_UBIGEOS.IDEUBIGEO, REG_UBIGEOS.PUNTUBIGEO,'A',p_usrCreacion,SYSDATE );
  END LOOP;  
  
  FOR REG_CENTROEST IN c_centrosEstudios(p_ideCargo) LOOP        
     INSERT INTO CENTROEST_CARGO
     (IDECENTESTCARGO,IDECARGO,TIPCENESTU,TIPNOMCENESTU,PUNTACENTROEST,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDECENTESTCARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_CENTROEST.TIPCENESTU, REG_CENTROEST.TIPNOMCENESTU,REG_CENTROEST.PUNTACENTROEST ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_NIVELESALC IN c_nivelesAlcan(p_ideCargo) LOOP        
     INSERT INTO NIVELACADEMICO_CARGO
     (IDENIVELACADECARGO,IDECARGO,TIPEDUCACION,TIPAREAESTUDIO,TIPNIVELCANZADO,CICLOSEMESTRE,PUNTNIVESTUDIO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDENIVELACADECARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_NIVELESALC.TIPEDUCACION, REG_NIVELESALC.TIPAREAESTUDIO,REG_NIVELESALC.TIPNIVELCANZADO,REG_NIVELESALC.CICLOSEMESTRE, REG_NIVELESALC.PUNTNIVESTUDIO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_CONOCIMIENTOS IN c_conocimientos(p_ideCargo) LOOP        
     INSERT INTO CONOGENERAL_CARGO
     (IDECONOGENCARGO,IDECARGO,TIPCONOFIMATICA,TIPNOMOFIMATICA,TIPIDIOMA,TIPCONOCIDIOMA,TIPCONOGENERAL,TIPNOMCONOCGRALES,NOMCONOCGRALES,TIPNIVELCONOCIMIENTO,INDCERTIFICACION,PUNTCONOCIMIENTO,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDECONOGENCARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_CONOCIMIENTOS.TIPCONOFIMATICA,REG_CONOCIMIENTOS.TIPNOMOFIMATICA,REG_CONOCIMIENTOS.TIPIDIOMA,REG_CONOCIMIENTOS.TIPCONOCIDIOMA,REG_CONOCIMIENTOS.TIPCONOGENERAL, REG_CONOCIMIENTOS.TIPNOMCONOCGRALES, REG_CONOCIMIENTOS.NOMCONOCGRALES, REG_CONOCIMIENTOS.TIPNIVELCONOCIMIENTO, REG_CONOCIMIENTOS.INDCERTIFICACION, REG_CONOCIMIENTOS.PUNTCONOCIMIENTO,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_EXPERIENCIAS IN c_experiencias(p_ideCargo) LOOP        
     INSERT INTO EXPERIENCIA_CARGO
     (IDEEXPCARGO,IDECARGO,TIPEXPLABORAL,CANTANHOEXP,CANTMESESEXP,PUNTEXPERIENCIA,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEEXPCARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_EXPERIENCIAS.TIPEXPLABORAL,REG_EXPERIENCIAS.CANTANHOEXP,REG_EXPERIENCIAS.CANTMESESEXP,REG_EXPERIENCIAS.PUNTEXPERIENCIA ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_DISCAPACIDAD IN c_discapacidades(p_ideCargo) LOOP        
     INSERT INTO DISCAPACIDAD_CARGO
     (IDEDISCAPACARGO,IDECARGO,TIPDISCAPA,DESDISCAPA,PUNTDISCAPA,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEDISCAPACARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_DISCAPACIDAD.TIPDISCAPA, REG_DISCAPACIDAD.DESDISCAPA, REG_DISCAPACIDAD.PUNTDISCAPA ,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  FOR REG_EVALUACION IN c_evaluaciones(p_ideCargo) LOOP        
     INSERT INTO EVALUACION_CARGO
     (IDEEVALUACIONCARGO,IDECARGO,IDEEXAMEVAL,TIPEXAMEN,NOTAMINEXAMEN,TIPAREARESPON,PUNTEXAMEN,ESTACTIVO,USRCREACION,FECCREACION)
     VALUES(IDEEVALUACIONCARGO_SQ.NEXTVAL,c_ideCargo_sq, REG_EVALUACION.IDEEXAMEVAL, REG_EVALUACION.TIPEXAMEN, REG_EVALUACION.NOTAMINEXAMEN,REG_EVALUACION.TIPAREARESPON,REG_EVALUACION.PUNTEXAMEN,'A',p_usrCreacion,SYSDATE );
  END LOOP;
  
  COMMIT;
  
  p_idCargoCopia := c_ideCargo_sq;
  
  EXCEPTION 
     WHEN OTHERS THEN
     ROLLBACK;
     p_idCargoCopia:= 0;
  END;

END SP_MANTENIMIENTO_CARGO; 

/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_AMPLIACION
    Proposito   : realiza el insert de una solicitud clona los datos 
                  y registra en el log de la solicitud
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      05/05/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_INSERTAR_AMPLIACION(p_ideCargo         IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                                p_ideSede          IN SEDE.IDESEDE%TYPE,
                                p_ideDependencia   IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                p_ideDepartamento  IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                p_ideArea          IN AREA.IDEAREA%TYPE,
                                p_numVacantes      IN SOLREQ_PERSONAL.NUMVACANTES%TYPE,
                                p_motivo           IN SOLREQ_PERSONAL.MOTIVO%TYPE,
                                p_tipoPuesto       IN SOLREQ_PERSONAL.TIPPUESTO%TYPE, 
                                p_observacion      IN SOLREQ_PERSONAL.OBSERVACION%TYPE,
                                p_ideUsuarioSuceso IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso     IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_cEtapa           IN DETALLE_GENERAL.VALOR%TYPE,
                                p_responsableSig   IN ROL.IDROL%TYPE,
                                p_tipoSolicitud    IN SOLREQ_PERSONAL.TIPSOL%TYPE,
                                p_indicArea        IN VARCHAR2,
                                p_cRetVal          OUT NUMBER) 

IS
c_ideLogSequency  LOGSOLREQ_PERSONAL.IDELOGSOLREQ_PERSONAL%TYPE;
c_codAmpliacion   SOLREQ_PERSONAL.CODSOLREQPERSONAL%TYPE;
c_ideUsuarioResp  USUARIO.IDUSUARIO%TYPE;
c_descUsuario     USUARIO.CODUSUARIO%TYPE;
c_idRolResp       ROL.IDROL%TYPE;
qWhere            VARCHAR2(100);
qQuery            VARCHAR2(500);
BEGIN
  
    
  
    SELECT US.CODUSUARIO
    INTO c_descUsuario
    FROM USUARIO US
    WHERE US.IDUSUARIO = p_ideUsuarioSuceso;
    
    IF (p_indicArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
    ELSE
    qWhere :='';
    END IF;
    
    qQuery := 'SELECT UR.IDUSUARIO , UR.IDROL '||
              'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
              'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
              'AND UR.IDROL = R.IDROL '||
              'AND UR.IDESEDE = '||p_ideSede||' '|| 
              'AND R.IDROL = '||p_responsableSig||' '||
               qWhere ||' '||
              'AND UN.FLGESTADO = ''A'' '||
              'AND ROWNUM <= 1';
              
        
   EXECUTE IMMEDIATE qQuery INTO c_ideUsuarioResp, c_idRolResp ;
   
   SELECT SOLREQ_PERSONAL_SQ.NEXTVAL
    INTO c_ideLogSequency
    FROM DUAL; 
    
    SELECT IDESOLAMPLIACION_SQ.NEXTVAL
    INTO c_codAmpliacion 
    FROM DUAL;
    
   p_cRetVal := c_ideUsuarioResp;
  BEGIN 
    INSERT INTO SOLREQ_PERSONAL 
    (IDESOLREQPERSONAL,CODSOLREQPERSONAL,IDESEDE,IDEDEPENDENCIA,IDEDEPARTAMENTO,IDEAREA,NUMVACANTES,MOTIVO,OBSERVACION,ESTACTIVO,TIPSOL,USRCREACION,FECCREACION,TIPETAPA,TIPPUESTO)
    VALUES(c_ideLogSequency, c_codAmpliacion,p_ideSede, p_ideDependencia,p_ideDepartamento,p_ideArea,p_numVacantes,p_motivo,p_observacion,'A',p_tipoSolicitud,c_descUsuario,SYSDATE,p_cEtapa,p_tipoPuesto);
    --realizar la copia de cargo a la tabla ampliacion 
    PR_REQUERIMIENTOS.COPIA_CARGO(p_ideCargo,c_ideLogSequency,c_descUsuario);
    --insertar el log de solicitud
    PR_INTRANET_ED.SP_INSERT_LOG_SOLREQPERSONAL( c_ideLogSequency,p_cEtapa,SYSDATE,p_ideUsuarioSuceso,p_ideRolSuceso, c_idRolResp,c_ideUsuarioResp, NULL); 
    COMMIT; 
  EXCEPTION
    WHEN OTHERS THEN
    ROLLBACK;
    p_cRetVal := -1;
  END;
    
END SP_INSERTAR_AMPLIACION;

/* ------------------------------------------------------------
    Nombre      : SP_RESPONSABLE_PUBLICACION
    Proposito   : determinar el responsable de la publicaci?n de la solicitud
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      06/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_RESPONSABLE_PUBLICACION(p_idSolicitudNuevo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_idSede            IN SEDE.IDESEDE%TYPE,
                                     p_idUsuarioResp     OUT USUARIO.IDUSUARIO%TYPE,
                                     p_idRolResp         OUT ROL.IDROL%TYPE)

IS

c_tipoRequerimiento CARGO.TIPREQUERIMIENTO%TYPE;
BEGIN
  SELECT C.TIPREQUERIMIENTO
  INTO c_tipoRequerimiento
  FROM CARGO C , SOLNUEVO_CARGO SN
  WHERE C.CODCARGO = SN.CODCARGO
  AND SN.IDESOLNUEVOCARGO = p_idSolicitudNuevo;
  
  BEGIN
    SELECT UN.IDUSUARIO,UN.IDROL
    INTO p_idUsuarioResp,p_idRolResp
    FROM USUARIOREQ UQ, USUAROLSEDE UN
    WHERE UQ.IDUSUARIO = UN.IDUSUARIO
    AND UN.IDESEDE = p_idSede
    AND UQ.TIPREQ = c_tipoRequerimiento
    AND UN.IDROL IN (8,9)
    AND ROWNUM < 2
    ORDER BY UN.IDUSUARIO,UN.IDROL;
  EXCEPTION
    WHEN OTHERS THEN
    p_idUsuarioResp:= -1;
    p_idRolResp:=-1;
  END;
  
END SP_RESPONSABLE_PUBLICACION;

/* ------------------------------------------------------------
    Nombre      : FN_DETERMINAR_RESPONSABLE
    Proposito   : determinar el responsable de la publicaci?n de la solicitud
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      06/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_DETERMINAR_RESPONSABLE(p_idSolicitud   IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_idSede        IN SEDE.IDESEDE%TYPE,
                                    p_idUsuarioResp OUT USUARIO.IDUSUARIO%TYPE,
                                    p_idRolResp     OUT ROL.IDROL%TYPE)

IS

c_tipoRequerimiento SOLREQ_PERSONAL.TIPREQUERIMIENTO%TYPE;
BEGIN
  SELECT SQ.TIPREQUERIMIENTO
  INTO c_tipoRequerimiento
  FROM SOLREQ_PERSONAL SQ
  WHERE SQ.IDESOLREQPERSONAL = p_idSolicitud;
  
  BEGIN
    SELECT UN.IDUSUARIO,UN.IDROL
    INTO p_idUsuarioResp,p_idRolResp
    FROM USUARIOREQ UQ, USUAROLSEDE UN
    WHERE UQ.IDUSUARIO = UN.IDUSUARIO
    AND UN.IDESEDE = p_idSede
    AND UQ.TIPREQ = c_tipoRequerimiento
    AND UN.IDROL IN (8,9)
    AND ROWNUM < 2
    ORDER BY UN.IDUSUARIO,UN.IDROL;
  EXCEPTION
    WHEN OTHERS THEN
    p_idUsuarioResp:= -1;
    p_idRolResp:=-1;
  END;
  
  
END SP_DETERMINAR_RESPONSABLE;

/* ------------------------------------------------------------
    Nombre      : SP_OBTENER_ETAPA_SOLREQ
    Proposito   : Obtener la etapa y estado actual de la solicitud de requerimiento.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      07/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_OBTENER_ETAPA_SOLREQ(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetVal OUT SYS_REFCURSOR)IS

BEGIN

  BEGIN
  OPEN p_cRetVal FOR
  SELECT LG.TIPETAPA, LG.ROLRESPONSABLE
  FROM LOGSOLREQ_PERSONAL LG   
  WHERE FECSUCESO = (SELECT MAX (SR.FECSUCESO)
                     FROM  LOGSOLREQ_PERSONAL SR
                     WHERE SR.IDESOLREQPERSONAL = p_ideSolRequerimiento)
  AND LG.IDESOLREQPERSONAL =  p_ideSolRequerimiento; 

  EXCEPTION
  WHEN OTHERS THEN
    p_cRetVal:=NULL;
  END;   
  
END SP_OBTENER_ETAPA_SOLREQ;
  
/* ------------------------------------------------------------
    Nombre      : SP_INSERTAR_APROB_AMP
    Proposito   : Obtener la etapa y estado actual de la solicitud de requerimiento.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      07/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_INSERTAR_APROB_AMP(p_ideSolRequerimiento IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_ideRolResponsable   IN ROL.IDROL%TYPE,
                                p_ideSede             IN SEDE.IDESEDE%TYPE,
                                p_ideArea             IN AREA.IDEAREA%TYPE,
                                p_indicArea           IN VARCHAR2,
                                p_tipoEtapa           IN LOGSOLREQ_PERSONAL.TIPETAPA%TYPE,
                                p_usuarioSuceso       IN LOGSOLREQ_PERSONAL.USRSUCESO%TYPE,
                                p_ideRolSuceso        IN LOGSOLREQ_PERSONAL.ROLSUCESO%TYPE,
                                p_observacion         IN LOGSOLREQ_PERSONAL.OBSERVACION%TYPE,
                                c_ideUsuarioResp      IN USUARIO.IDUSUARIO%TYPE,
                                c_retVal              OUT USUARIO.IDUSUARIO%TYPE)IS

qWhere  VARCHAR2(100);
qQuery  VARCHAR2(1000);


BEGIN

  IF ((c_ideUsuarioResp IS NULL)OR(c_ideUsuarioResp = 0))THEN
  
  IF (p_indicArea = 'SI')THEN
    qWhere := 'AND UN.IDEAREA = '||p_ideArea;
  ELSE
    qWhere :='';
  END IF;
   
  qQuery := 'SELECT UR.IDUSUARIO '||
            'FROM ROL R,USUAROLSEDE UR, USUARIO_NIVEL UN '||
            'WHERE UN.IDUSUARIO = UR.IDUSUARIO '|| 
            'AND UR.IDROL = R.IDROL '||
            'AND UR.IDESEDE = '||p_ideSede||' '|| 
            'AND R.IDROL = '||p_ideRolResponsable||' '||
             qWhere ||' '||
            'AND UN.FLGESTADO = ''A'' '||
            'AND ROWNUM <= 1';
                  
  EXECUTE IMMEDIATE qQuery INTO c_retVal ;
  ELSE
    c_retVal:=c_ideUsuarioResp;
  END IF;
  
  
  BEGIN
   
  IF (p_tipoEtapa = P_TIPETAPA_RECHAZADO)THEN
  
    
    
    INSERT INTO LOGSOLREQ_PERSONAL
    (IDELOGSOLREQ_PERSONAL,IDESOLREQPERSONAL,TIPETAPA,OBSERVACION, FECSUCESO,USRSUCESO,ROLSUCESO)
    VALUES (IDELOGSOLREQ_PERSONAL_SQ.NEXTVAL,p_ideSolRequerimiento,p_tipoEtapa,p_observacion,SYSDATE,p_usuarioSuceso,p_ideRolSuceso);
      
    UPDATE SOLREQ_PERSONAL SR
    SET SR.TIPETAPA = p_tipoEtapa,
        SR.ESTACTIVO = 'I'
    WHERE SR.IDESOLREQPERSONAL = p_ideSolRequerimiento;
  ELSE
    INSERT INTO LOGSOLREQ_PERSONAL
    (IDELOGSOLREQ_PERSONAL,IDESOLREQPERSONAL,TIPETAPA,ROLRESPONSABLE,USRESPONSABLE,FECSUCESO,USRSUCESO,ROLSUCESO)
    VALUES (IDELOGSOLREQ_PERSONAL_SQ.NEXTVAL, p_ideSolRequerimiento,p_tipoEtapa,p_ideRolResponsable,c_retVal,SYSDATE, p_usuarioSuceso, p_ideRolSuceso);
    
    UPDATE SOLREQ_PERSONAL SP
    SET SP.TIPETAPA = p_tipoEtapa
    WHERE SP.IDESOLREQPERSONAL = p_ideSolRequerimiento;
   END IF;
  COMMIT; 
  
  EXCEPTION
  WHEN OTHERS THEN
    c_retVal:=-1;
    ROLLBACK;
  END;   
  
END SP_INSERTAR_APROB_AMP;

  /* ------------------------------------------------------------
    Nombre      : SP_LISTACARGOS
    Proposito   : Obtiene la lista de cargos por sede
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014 jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_LISTACARGOS(p_idSede IN SEDE.IDESEDE%TYPE,
                           p_cRetVal OUT SYS_REFCURSOR) IS
  
  BEGIN
  
    OPEN P_CRETVAL FOR
       SELECT DISTINCT C.IDECARGO, C.NOMCARGO
        FROM CARGO C 
       WHERE C.IDESEDE = p_idSede
       AND C.ESTACTIVO = 'A'
       AND C.TIPETAPA IN ('08','06','04','10')
       AND NVL(C.VERSION,1) = (SELECT NVL(MAX(C1.VERSION),1) 
                               FROM CARGO C1 WHERE C1.ESTACTIVO='A'
                               AND C1.CODCARGO = C.CODCARGO)
       ORDER BY C.NOMCARGO;

  
  END SP_LISTACARGOS;


/* ------------------------------------------------------------
    Nombre      : FN_GET_LISTAREQ
    Proposito   : obtiene la lista de todas las solicitudes
                 (Nuevo - Ampliacion - Reemplazo)
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_LISTA_SOLGRAL(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                           p_ideSede         IN SEDE.IDESEDE%TYPE,
                           p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                           p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                           p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                           p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                           p_cTipResp        in NUMBER,   
                           p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                           p_cFecIni         IN VARCHAR2,
                           p_cFeFin          IN VARCHAR2,
                           p_cTipoSolicitud  IN VARCHAR2,
                           p_cCodSolicitud   IN VARCHAR2,
                           p_cRetVal         OUT SYS_REFCURSOR)IS
          
c_Consulta1 VARCHAR2(2000):=NULL;
c_Where VARCHAR2(1000):=NULL;
c_Query VARCHAR2(4000):=NULL;
          
BEGIN
  
  DELETE T_RECLTEMP;   

  INSERT INTO T_RECLTEMP
  
  SELECT SN.IDESOLNUEVOCARGO, 
         PR_INTRANET.FN_ESTADO_SOLICITUD(SN.IDESOLNUEVOCARGO,'01') ESTADO, 
         SN.CODCARGO,
         NVL(SN.IDECARGO,0), 
         SN.NOMBRE,
         SN.IDEDEPENDENCIA,
         (SELECT DE.NOMDEPENDENCIA 
          FROM DEPENDENCIA DE 
          WHERE DE.IDEDEPENDENCIA = SN.IDEDEPENDENCIA 
          AND DE.IDESEDE = SN.IDESEDE
          AND DE.ESTACTIVO = 'A'),
         SN.IDEDEPARTAMENTO,
         (SELECT DP.NOMDEPARTAMENTO 
          FROM DEPARTAMENTO DP 
          WHERE DP.IDEDEPARTAMENTO = SN.IDEDEPARTAMENTO
          AND DP.IDEDEPENDENCIA = SN.IDEDEPENDENCIA
          AND DP.ESTACTIVO = 'A'),
         SN.IDEAREA,
         (SELECT AR.NOMAREA 
          FROM  AREA AR 
          WHERE AR.IDEAREA = SN.IDEAREA
          AND AR.IDEDEPARTAMENTO = SN.IDEDEPARTAMENTO
          AND AR.ESTACTIVO = 'A'),
         SN.NUMPOSICIONES,
         (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SN.IDESOLNUEVOCARGO
           AND RP.TIPSOL = '01'
           AND RP.ESTACTIVO='A') POSTULANTES, 
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SN.IDESOLNUEVOCARGO
           AND RP.TIPSOL = '01'
           AND RP.ESTPOSTULANTE ='02'
           AND RP.ESTACTIVO='A') PRESELECCIONADOS, 
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SN.IDESOLNUEVOCARGO
           AND RP.TIPSOL = '01'
           AND RP.ESTPOSTULANTE ='07'
           AND RP.ESTACTIVO='A') EVALUADOS,  
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SN.IDESOLNUEVOCARGO
            AND RP.TIPSOL = '01'
            AND RP.ESTPOSTULANTE ='08'
            AND RP.ESTACTIVO='A') SELECCIONADOS , 
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SN.IDESOLNUEVOCARGO
            AND RP.TIPSOL = '01'
            AND RP.ESTPOSTULANTE ='09'
            AND RP.ESTACTIVO='A') CONTRATADOS , 
         
         
         (SELECT C.FECSUCESO
                  FROM logsolnuevo_cargo C
                   WHERE C.Idesolnuevocargo = SN.Idesolnuevocargo
                    AND  C.Idelogsolnuevocargo =
                           (SELECT MIN(H.Idelogsolnuevocargo)
                                     FROM logsolnuevo_cargo H
                                              WHERE H.Idesolnuevocargo = C.Idesolnuevocargo)
                                                       ) FECCREACION,
         SN.FECCREACION CIERRE,
         
         
         NVL(R.IDROL,0), 
         NVL(R.DSCROL,'') ,
         (PR_REQUERIMIENTOS.FN_NOMRESPONS_SOL(SN.IDESOLNUEVOCARGO,'N')) NOMRESPONSABLE,
         SN.FECPUBLICACION,
         --SN.FECEXPIRACION,
         (SELECT C.Fecsuceso 
                      FROM logsolnuevo_cargo C
                       WHERE C.Tipetapa = '08'
                       AND C.Idesolnuevocargo = SN.Idesolnuevocargo
                       and rownum<2
                        ) FECEXPIRACION,
         
         
         DECODE(SN.FECPUBLICACION,NULL,'NO','SI') PUBLICADO, 
         LS.TIPETAPA,
         (PR_INTRANET.FN_VALOR_GENERAL('ETAPA',LS.TIPETAPA)) TETAPA,
         '01' TIPSOL ,
         'NUEVO' TIPOSOLICITUD
         --SN.ESTACTIVO  
  FROM SOLNUEVO_CARGO SN, LOGSOLNUEVO_CARGO LS LEFT JOIN ROL R
  ON (LS.ROLRESPONSABLE = R.IDROL)
  WHERE LS.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO
  AND SN.IDESEDE = p_ideSede
  AND LS.FECSUCESO = (SELECT MAX (FECSUCESO)
                           FROM  LOGSOLNUEVO_CARGO LN
                           WHERE LN.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO);
                           

                           
  INSERT INTO T_RECLTEMP
  SELECT SQ.IDESOLREQPERSONAL, 
         PR_INTRANET.FN_ESTADO_SOLICITUD(SQ.IDESOLREQPERSONAL ,'O') ESTADO,
         TO_CHAR(SQ.CODSOLREQPERSONAL) CODSOLICITUD, 
         NVL(SQ.IDECARGO,0), 
         SQ.NOMCARGO,
         SQ.IDEDEPENDENCIA,
         (SELECT DE.NOMDEPENDENCIA 
          FROM DEPENDENCIA DE 
          WHERE DE.IDEDEPENDENCIA = SQ.IDEDEPENDENCIA 
          AND DE.IDESEDE = SQ.IDESEDE
          AND DE.ESTACTIVO = 'A'),
         SQ.IDEDEPARTAMENTO,
         (SELECT DP.NOMDEPARTAMENTO 
          FROM DEPARTAMENTO DP 
          WHERE DP.IDEDEPARTAMENTO = SQ.IDEDEPARTAMENTO
          AND DP.IDEDEPENDENCIA = SQ.IDEDEPENDENCIA
          AND DP.ESTACTIVO = 'A'),
         SQ.IDEAREA,
         (SELECT AR.NOMAREA 
          FROM  AREA AR 
          WHERE AR.IDEAREA = SQ.IDEAREA
          AND AR.IDEDEPARTAMENTO = SQ.IDEDEPARTAMENTO
          AND AR.ESTACTIVO = 'A'),
         SQ.NUMVACANTES,
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SQ.IDESOLREQPERSONAL
           AND RP.TIPSOL = '01'
           AND RP.ESTACTIVO='A') POSTULANTES, 
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SQ.IDESOLREQPERSONAL
           AND RP.TIPSOL = '01'
           AND RP.ESTPOSTULANTE ='02'
           AND RP.ESTACTIVO='A') PRESELECCIONADOS, 
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SQ.IDESOLREQPERSONAL
           AND RP.TIPSOL = '01'
           AND RP.ESTPOSTULANTE ='07'
           AND RP.ESTACTIVO='A') EVALUADOS,  
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SQ.IDESOLREQPERSONAL
            AND RP.TIPSOL = '01'
            AND RP.ESTPOSTULANTE ='08'
            AND RP.ESTACTIVO='A') SELECCIONADOS , 
          (SELECT COUNT(*) 
           FROM RECLUTAMIENTO_PERSONA RP
           WHERE RP.IDESOL = SQ.IDESOLREQPERSONAL
            AND RP.TIPSOL = '01'
            AND RP.ESTPOSTULANTE ='09'
            AND RP.ESTACTIVO='A') CONTRATADOS ,
         --SQ.FECCREACION, 
         
         (SELECT C.FECSUCESO
                  FROM LOGSOLREQ_PERSONAL C
                   WHERE C.IDESOLREQPERSONAL = SQ.IDESOLREQPERSONAL
                    AND  C.IDELOGSOLREQ_PERSONAL =
                           (SELECT MIN(H.IDELOGSOLREQ_PERSONAL)
                                     FROM LOGSOLREQ_PERSONAL H
                                              WHERE H.IDESOLREQPERSONAL = C.IDESOLREQPERSONAL)
                                                       ) FECCREACION,
         
         
         SQ.FECCREACION CIERRE ,
         NVL(R.IDROL,0), 
         NVL(R.DSCROL,'') ,
         (PR_REQUERIMIENTOS.FN_NOMRESPONS_SOL(SQ.IDESOLREQPERSONAL,'O')) NOMRESPONSABLE,
         SQ.FECPUBLICACION,
        -- SQ.FECEXPIRACACION,
        
        (SELECT C.FECSUCESO 
                      FROM LOGSOLREQ_PERSONAL C
                       WHERE C.TIPETAPA = '08'
                       AND C.IDESOLREQPERSONAL = SQ.IDESOLREQPERSONAL
                       and rownum<2
                        ) FECEXPIRACACION,
        
         DECODE(SQ.FECPUBLICACION,NULL,'NO','SI') PUBLICADO, 
         LQ.TIPETAPA,
         (PR_INTRANET.FN_VALOR_GENERAL('ETAPA',LQ.TIPETAPA)) TETAPA,
         SQ.TIPSOL,
         (PR_INTRANET.FN_VALOR_GENERAL('TIPSOL',SQ.TIPSOL)) TIPOSOLICITUD
         --SQ.ESTACTIVO
  FROM SOLREQ_PERSONAL SQ,LOGSOLREQ_PERSONAL LQ LEFT JOIN ROL R 
  ON (LQ.ROLRESPONSABLE = R.IDROL)
  WHERE LQ.IDESOLREQPERSONAL = SQ.IDESOLREQPERSONAL
  AND SQ.IDESEDE = p_ideSede
  AND LQ.FECSUCESO = (SELECT MAX (FECSUCESO)
                           FROM  LOGSOLREQ_PERSONAL LSQ
                           WHERE LSQ.IDESOLREQPERSONAL = SQ.IDESOLREQPERSONAL);  
  
  
   IF p_nIdCargo>0 THEN
    
      c_Where := c_Where || ' AND  S.IDECARGO = '||p_nIdCargo;
    
    END IF;
   
   IF p_cCodSolicitud IS NOT NULL THEN
        
      c_Where := c_Where || '  AND S.CODIGO LIKE ''%'||p_cCodSolicitud||'%'' ' ;
        
    END IF;
   
    IF p_nIdDependencia>0 THEN
        
      c_Where := c_Where || '  AND S.IDEDEPENDENCIA = '||p_nIdDependencia ;
        
    END IF;
    
    IF p_nIdDepartamento>0 THEN
        
      c_Where := c_Where || '  AND S.IDEDEPARTAMENTO = '||p_nIdDepartamento ;
        
    END IF;

    IF p_nIdArea>0 THEN
        
      c_Where := c_Where || ' AND S.IDEAREA = '||p_nIdArea ;
        
    END IF;
 
    IF p_cTipEtapa IS NOT NULL AND p_cTipEtapa<>'0' THEN
        
      c_Where := c_Where || ' AND S.TIPETAPA = '||p_cTipEtapa ;
                            
    END IF;

    IF p_cTipResp IS NOT NULL AND p_cTipResp<> 0 THEN
        
      c_Where := c_Where || ' AND '''||p_cTipResp||''' = S.IDRESPONSABLE ';
        
    END IF;
    
    IF p_cEstado IS NOT NULL AND p_cEstado <> '0' THEN
      
        c_Where := c_Where || ' AND '''||p_cEstado||''' = S.ESTADO ';
         
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni))>0 AND LENGTH(rtrim(p_cFeFin))>0 THEN
       
      c_Where := c_Where || ' AND S.INICIO >= to_date('''||p_cFecIni||''',''DD/MM/YYYY'')'
                         || ' AND S.CIERRE < to_date('''||p_cFeFin||''',''DD/MM/YYYY'')+1';
        
    END IF;
    
    IF p_cTipoSolicitud IS NOT NULL THEN
        
      c_Where := c_Where || ' AND '''||p_cTipoSolicitud||''' = S.TIPSOL ';
        
    END IF;
    
   c_Where := c_Where || ' ORDER BY S.IDESOLICITUD ';
  
   c_Consulta1:= ' SELECT * FROM T_RECLTEMP  S WHERE 1=1 ';
   
   c_Query := c_Consulta1 || c_Where;
   
   INSERT INTO LOG_MENSAJE 
   VALUES(c_Query);
   
   OPEN p_cRetVal FOR 
   c_Query;
   
   COMMIT;
   
   
   
END SP_LISTA_SOLGRAL; 

/* ------------------------------------------------------------
    Nombre      : SP_LISTA_SOLNUEVO
    Proposito   : obtiene la lista de las nuevas solicitudes)
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      18/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_LISTA_SOLNUEVO(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                            p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                            p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                            p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                            p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                            p_cTipResp        in NUMBER,   
                            p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                            p_cFecIni         IN VARCHAR2,
                            p_cFeFin          IN VARCHAR2,
                            p_cRetVal         OUT SYS_REFCURSOR)IS
          
c_Consulta1 VARCHAR2(2000):=NULL;
c_Where VARCHAR2(1000):=NULL;
c_Query VARCHAR2(4000):=NULL;
          
BEGIN

c_Consulta1:= 'SELECT SN.IDESOLNUEVOCARGO, PR_INTRANET.FN_ESTADO_SOLICITUD(SN.IDESOLNUEVOCARGO,''N'') ESTADO, '
            ||'SN.CODCARGO,(SELECT C.IDECARGO FROM CARGO C WHERE C.CODCARGO = SN.CODCARGO) IDECARGO ,SN.NOMBRE,'
            ||'SN.IDEDEPENDENCIA, '
            ||'(SELECT DE.NOMDEPENDENCIA FROM DEPENDENCIA DE WHERE DE.IDEDEPENDENCIA = SN.IDEDEPENDENCIA AND DE.IDESEDE = SN.SEDE AND DE.ESTACTIVO = ''A'') NOMDEPENDENCIA, '
            ||'SN.IDEDEPARTAMENTO , '
            ||'(SELECT DP.NOMDEPARTAMENTO FROM DEPARTAMENTO DP WHERE DP.IDEDEPARTAMENTO = SN.IDEDEPARTAMENTO AND DP.IDEDEPENDENCIA = SN.IDEDEPENDENCIA AND DP.ESTACTIVO = ''A'') NOMDEPARTAMENTO, '
            ||'SN.IDEAREA, '
            ||'(SELECT AR.NOMAREA FROM AREA AR WHERE AR.IDEAREA = SN.IDEAREA AND AR.IDEDEPARTAMENTO = SN.IDEDEPARTAMENTO AND SN.ESTACTIVO = ''A'') NOMAREA,'
            ||'SN.NUMPOSICIONES, SN.FECCREACION,R.IDROL ,R.DSCROL ,(PR_REQUERIMIENTOS.FN_NOMRESPONS_SOL(SN.IDESOLNUEVOCARGO,''N'')) NOMRESPONSABLE, '
            ||'LS.TIPETAPA,(PR_INTRANET.FN_VALOR_GENERAL(''ETAPA'',LS.TIPETAPA)) TETAPA '
            ||'FROM SOLNUEVO_CARGO SN, LOGSOLNUEVO_CARGO LS LEFT JOIN ROL R ON (R.IDROL = LS.ROLRESPONSABLE) '
            ||'WHERE LS.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO '
            ||'AND LS.FECSUCESO = (SELECT MAX (FECSUCESO) '
                                ||'FROM  LOGSOLNUEVO_CARGO LN '
                                ||'WHERE LN.IDESOLNUEVOCARGO = SN.IDESOLNUEVOCARGO) ';


    IF p_nIdCargo > 0 THEN
    
      c_Where := c_Where || ' AND  IDECARGO = ' || p_nIdCargo;
    
    END IF;
  
    IF p_nIdDependencia > 0 THEN
    
      c_Where := c_Where || '  AND SN.IDEDEPENDENCIA = ' || p_nIdDependencia;
    
    END IF;
  
    IF p_nIdDepartamento > 0 THEN
    
      c_Where := c_Where || '  AND SN.IDEDEPARTAMENTO = ' || p_nIdDepartamento;
    
    END IF;
  
    IF p_nIdArea > 0 THEN
    
      c_Where := c_Where || ' SN.IDEAREA = ' || p_nIdArea;
    
    END IF;
  
    IF p_cEstado IS NOT NULL AND  p_cEstado <> '0' THEN
      
        c_Where := c_Where || ' AND '''||p_cEstado||''' = ESTADO ';
        
    END IF;
  
    IF p_cTipEtapa IS NOT NULL AND p_cTipEtapa<>'0' THEN
        
      c_Where := c_Where || ' AND LS.TIPETAPA = '||p_cTipEtapa ;
                            
    END IF;
    
    IF p_cTipResp IS NOT NULL AND p_cTipResp <> '' THEN
    
      c_Where := c_Where || ' AND ''' || p_cTipResp || ''' = R.IDROL ';
    
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni)) > 0 AND LENGTH(rtrim(p_cFeFin)) > 0 THEN
    
      c_Where := c_Where || ' AND SN.FECCREACION >= to_date(''' || p_cFecIni ||
                ''',''DD/MM/YYYY'')' || ' AND SN.FECCREACION < to_date(''' ||
                p_cFeFin || ''',''DD/MM/YYYY'')+1';
    
    END IF;
  
    c_Where := c_Where || 'ORDER BY SN.IDESOLNUEVOCARGO ';
  
    c_Query := c_Consulta1 || c_Where;
  
    /*DELETE FROM LOG_MENSAJE;
  
    INSERT INTO LOG_MENSAJE VALUES (c_Query);
    COMMIT;*/
  
    OPEN p_cRetVal FOR c_Query;


END SP_LISTA_SOLNUEVO;

/* ------------------------------------------------------------
    Nombre      : SP_LISTA_CARGOS_MANT
    Proposito   : obtiene la lista de los cargos existente
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      18/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_LISTA_CARGOS_MANT(p_nCodCargo        IN CARGO.CODCARGO%TYPE,
                               p_nIdDependencia   IN CARGO.Idedependencia%TYPE,
                               p_nIdDepartamento  IN CARGO.Idedepartamento%TYPE,
                               p_nIdArea          IN CARGO.Idearea%TYPE,                        
                               p_cFecIni          IN VARCHAR2,
                               p_cFeFin           IN VARCHAR2,
                               p_cEstado          IN CARGO.ESTACTIVO%TYPE,
                               p_cideSede         IN CARGO.IDESEDE%TYPE,
                               p_cRetCursor       OUT SYS_REFCURSOR)IS
          
c_Consulta1 VARCHAR2(2000):=NULL;
c_Where VARCHAR2(1000):=NULL;
c_Query VARCHAR2(4000):=NULL;
          
BEGIN

c_Consulta1:= 'SELECT C.ESTACTIVO,C.IDECARGO,C.CODCARGO,C.NOMCARGO,C.DESCARGO,C.IDEDEPENDENCIA, '
            ||'(SELECT DE.NOMDEPENDENCIA FROM DEPENDENCIA DE WHERE DE.IDEDEPENDENCIA = C.IDEDEPENDENCIA AND DE.IDESEDE = '''||p_cideSede||'''  AND DE.ESTACTIVO = ''A'') NOMDEPENDENCIA, '
            ||'C.IDEDEPARTAMENTO, '
            ||'(SELECT DP.NOMDEPARTAMENTO FROM DEPARTAMENTO DP WHERE DP.IDEDEPARTAMENTO = C.IDEDEPARTAMENTO AND DP.ESTACTIVO = ''A'') NOMDEPARTAMENTO,'
            ||'C.IDEAREA, '
            ||'(SELECT AR.NOMAREA FROM AREA AR WHERE AR.IDEAREA = C.IDEAREA AND AR.ESTACTIVO = ''A'') NOMAREA,C.VERSION '
            ||'FROM CARGO C  '
            ||'WHERE C.IDESEDE = '''||p_cideSede||''' ';

            
    IF ((p_nCodCargo != NULL)OR (p_nCodCargo != '0')) THEN
    
      c_Where := c_Where || ' AND  C.CODCARGO = '''||p_nCodCargo||''' ';
    
    END IF;
  
    IF p_nIdDependencia > 0 THEN
    
      c_Where := c_Where || '  AND C.IDEDEPENDENCIA = ' || p_nIdDependencia;
    
    END IF;
  
    IF p_nIdDepartamento > 0 THEN
    
      c_Where := c_Where || '  AND C.IDEDEPARTAMENTO = ' || p_nIdDepartamento;
    
    END IF;
  
    IF p_nIdArea > 0 THEN
    
      c_Where := c_Where || ' AND C.IDEAREA = ' || p_nIdArea;
    
    END IF;
  
    IF p_cEstado IS NOT NULL AND p_cEstado<>'0' THEN
  
        c_Where := c_Where || ' AND '''||p_cEstado||''' = C.ESTACTIVO ';
     
    END IF;
  
    IF LENGTH(rtrim(p_cFecIni)) > 0 AND LENGTH(rtrim(p_cFeFin)) > 0 THEN
    
      c_Where := c_Where || ' AND SN.FECCREACION >= to_date(''' || p_cFecIni ||
                ''',''DD/MM/YYYY'')' || ' AND SN.FECCREACION < to_date(''' ||
                p_cFeFin || ''',''DD/MM/YYYY'')+1';
    
    END IF;
  
    c_Where := c_Where || ' ORDER BY C.VERSION DESC ';
  
    c_Query := c_Consulta1 || c_Where;
  
    DELETE FROM LOG_MENSAJE;
  
    INSERT INTO LOG_MENSAJE VALUES (c_Query);
    COMMIT;
  
    OPEN p_cRetCursor FOR c_Query;


END SP_LISTA_CARGOS_MANT;

/* ------------------------------------------------------------
    Nombre      : SP_CONSULTA_EDITAR_CARGO
    Proposito   : Determinar si se puede editar el cargo.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_CONSULTA_EDITAR_CARGO(p_ideCargo IN CARGO.IDECARGO%TYPE,
                                   p_cRetVal OUT VARCHAR2)IS

c_TipoEtapa  SOLNUEVO_CARGO.TIPETAPA%TYPE;
c_contador   NUMBER;
BEGIN

  BEGIN
  
    SELECT COUNT(*)
    INTO c_contador
    FROM CARGO C
    WHERE C.CODCARGO = (SELECT CA.CODCARGO FROM CARGO CA WHERE CA.IDECARGO = p_ideCargo);

    IF (c_contador = 1 ) THEN
      SELECT S.TIPETAPA
      INTO c_TipoEtapa
      FROM SOLNUEVO_CARGO S 
      WHERE S.IDECARGO = p_ideCargo;
    END IF;
    
    IF(c_TipoEtapa IS NULL)THEN
     p_cRetVal:='version';
    ELSE
     p_cRetVal:=c_TipoEtapa;
    END IF;
  
  EXCEPTION
  WHEN OTHERS THEN
    p_cRetVal:='-1';
  END;   
  
END SP_CONSULTA_EDITAR_CARGO;

/* ------------------------------------------------------------
    Nombre      : SP_GENERAR_EVAL_REQ_POSTUL
    Proposito   : Generar evaluaciones de requerimiento
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      21/03/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_GENERAR_EVAL_REQ_POSTUL(p_idePostulante   IN POSTULANTE.IDEPOSTULANTE%TYPE,
                                     p_ideReclutPost   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                                     p_usuarioCreacion IN RECLU_PERSO_EXAMEN.USRCREACION%TYPE,
                                     p_cuExamenes      OUT SYS_REFCURSOR)IS


c_idSolicitud    RECLUTAMIENTO_PERSONA.IDESOL%TYPE;
c_tipSolicitud   RECLUTAMIENTO_PERSONA.TIPSOL%TYPE;
c_ideCargo       CARGO.IDECARGO%TYPE;
c_ideSede        SEDE.IDESEDE%TYPE;
c_contador       NUMBER;


  
CURSOR c_cuEvalReqSol(c_idSolicitud SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE) IS
  SELECT ESR.IDEEVALUACIONSOLREQ,
         ESR.IDEEXAMEVAL ,
         ESR.TIPEXAMEN
  FROM EVALUACION_SOLREQ ESR
  WHERE ESR.IDESOLREQPERSONAL = c_idSolicitud
  AND ESR.ESTACTIVO = 'A';
  
CURSOR c_cuEvalCargo(p_ideCargo CARGO.IDECARGO%TYPE) IS
  SELECT EC.IDEEVALUACIONCARGO,
         EC.IDEEXAMEVAL, 
         EC.TIPEXAMEN
  FROM EVALUACION_CARGO EC
  WHERE EC.IDECARGO = c_ideCargo
  AND EC.ESTACTIVO = 'A';
    
BEGIN
  
    SELECT RP.IDESOL, RP.TIPSOL , RP.IDECARGO, RP.IDSEDE
    INTO c_idSolicitud, c_tipSolicitud,c_ideCargo,c_ideSede
    FROM RECLUTAMIENTO_PERSONA RP
    WHERE RP.IDERECLUTAPERSONA = p_ideReclutPost;  
    
    SELECT COUNT(*) 
    INTO c_contador
    FROM RECLU_PERSO_EXAMEN RPE 
    WHERE RPE.IDERECLUTAPERSONA = p_ideReclutPost;
    
    IF (c_contador = 0) THEN
      --Determinar el tipo de solicitud 
      IF(c_tipSolicitud = '01' )THEN   
        FOR REG_EVALUACIONESCARGO IN c_cuEvalCargo(c_ideCargo) LOOP
          INSERT INTO RECLU_PERSO_EXAMEN
          (IDERECLUPERSOEXAMEN,IDERECLUTAPERSONA, IDEEVALUACION, TIPSOLICITUD,TIPESTEVALUACION,IDEEXAMEN,TIPEXAMEN,FECCREACION,USRCREACION)   
          VALUES
          (IDERECLUPERSOEXAMEN_SQ.NEXTVAL,p_ideReclutPost,REG_EVALUACIONESCARGO.IDEEVALUACIONCARGO,c_tipSolicitud,'01',REG_EVALUACIONESCARGO.IDEEXAMEVAL,REG_EVALUACIONESCARGO.TIPEXAMEN ,SYSDATE,p_usuarioCreacion);                                          
        END LOOP;
        ELSE
          FOR REG_EVALUACIONES IN c_cuEvalReqSol(c_idSolicitud) LOOP
            INSERT INTO RECLU_PERSO_EXAMEN
            (IDERECLUPERSOEXAMEN,IDERECLUTAPERSONA, IDEEVALUACION, TIPSOLICITUD,TIPESTEVALUACION,IDEEXAMEN,TIPEXAMEN,FECCREACION,USRCREACION)   
            VALUES
            (IDERECLUPERSOEXAMEN_SQ.NEXTVAL,p_ideReclutPost,REG_EVALUACIONES.IDEEVALUACIONSOLREQ,c_tipSolicitud,'01',REG_EVALUACIONES.IDEEXAMEVAL, REG_EVALUACIONES.TIPEXAMEN,SYSDATE,p_usuarioCreacion);                                          
        END LOOP;  
      END IF;
    END IF;
    
    BEGIN
      OPEN p_cuExamenes FOR
      SELECT  EP.IDERECLUPERSOEXAMEN,
              EP.IDERECLUTAPERSONA,
              EP.IDEEVALUACION,
              EP.NOMEXAMEN,
              EP.TIPEXAMEN,
              EP.TIPOEXAMEN,
              EP.IDUSUARESPONS,
              EP.USUARIORESP,
              EP.TIPESTEVALUACION,
              EP.ESTADOEVALUACION,
              EP.FECEVALUACION,
              EP.HORAEVALUACION,
              EP.INDRESUL,
              EP.NOTAFINAL,
              EP.COMENTARIORESUL,
              EP.ORDEN
      FROM (  SELECT  DISTINCT( RPE.IDERECLUPERSOEXAMEN),
                      RPE.IDERECLUTAPERSONA,
                      RPE.IDEEVALUACION,
                      EX.NOMEXAMEN,
                      EX.TIPEXAMEN,
                      PR_INTRANET.FN_VALOR_GENERAL('TIPOCRITERIO',EX.TIPEXAMEN) TIPOEXAMEN,
                      NVL(RPE.IDUSUARESPONS,0) IDUSUARESPONS,
                      NVL((US.DSCAPEPATERNO||' '||US.DSCAPEMATERNO||' '||US.DSCNOMBRES),'') USUARIORESP,
                      RPE.TIPESTEVALUACION, 
                      PR_INTRANET.FN_VALOR_GENERAL('ESTADOEVALUACIO',RPE. TIPESTEVALUACION) ESTADOEVALUACION,
                      RPE.FECEVALUACION,
                      RPE.HORAEVALUACION,
                      PR_REQUERIMIENTOS.FN_EXISTE_RESULTADO_EXAMEN(RPE.IDERECLUPERSOEXAMEN) INDRESUL ,
                      NVL(RPE.NOTAFINAL,0) NOTAFINAL,
                      NVL(RPE.COMENTARIORESUL,'') COMENTARIORESUL,
                      (CASE WHEN EX.TIPEXAMEN = P_TIPEXA_EXAMEN THEN 1
                            WHEN EX.TIPEXAMEN = P_TIPEXA_EVALUACION THEN 2
                            WHEN EX.TIPEXAMEN = P_TIPEXA_ENTREVISTA THEN 3
                            ELSE 4 END) ORDEN 
               FROM RECLU_PERSO_EXAMEN RPE LEFT  JOIN  USUARIO US ON (US.IDUSUARIO = RPE.IDUSUARESPONS), EXAMEN EX , (SELECT (DECODE(RE.TIPSOLICITUD,'01',(SELECT EC.IDEEXAMEVAL 
                                                                      FROM EVALUACION_CARGO EC 
                                                                      WHERE EC.IDEEVALUACIONCARGO = IDEEVALUACION),
                                                                      (SELECT ES.IDEEXAMEVAL 
                                                                      FROM EVALUACION_SOLREQ ES 
                                                                      WHERE ES.IDEEVALUACIONSOLREQ = IDEEVALUACION))) IDEEXAMEN,
                                                                      RE.IDEEVALUACION
                                                                      FROM RECLU_PERSO_EXAMEN RE
                                                                      WHERE RE.IDERECLUTAPERSONA = p_ideReclutPost) EXAM 
               WHERE EXAM.IDEEXAMEN = EX.IDEEXAMEN
               AND EXAM.IDEEVALUACION = RPE.IDEEVALUACION
               AND EX.IDESEDE = c_ideSede
               AND RPE.IDERECLUTAPERSONA = p_ideReclutPost) EP
      ORDER BY EP.ORDEN ; 
        
    EXCEPTION
      WHEN OTHERS THEN
      p_cuExamenes := NULL;
    END;    
  
END SP_GENERAR_EVAL_REQ_POSTUL;

/* ------------------------------------------------------------
    Nombre      : SP_CALIFICAR_EXAMEN
    Proposito   : Verificar el termino de de evaluacion 
                  y asignar calificaci?n
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      07/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_CALIFICAR_EXAMEN(p_ideReclPerExaCat  IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                              p_ideReclutaPersona IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                              p_usuarioCreacion   IN RECLU_PERSO_EXAMEN.USRCREACION%TYPE,
                              p_retVal            OUT NUMBER)IS

    
c_ideReclutaPersExamen RECLU_PERSO_EXAMEN.IDERECLUPERSOEXAMEN%TYPE;
c_ideExamenCategoria   EXAMEN_X_CATEGORIA.IDEEXAMENXCATEGORIA%TYPE;
c_tipoExamen           RECLU_PERSO_EXAMEN.TIPEXAMEN%TYPE;
c_nroCategExamFin      NUMBER;
c_nroCategoriaExamen   NUMBER;
c_nroEvaluaciones      NUMBER;
c_puntajeTotal         NUMBER;
c_nroTotalPreguntas    NUMBER;
c_puntFinalExamen      NUMBER;

c_estadoEvaluacion     VARCHAR2(2);


BEGIN
  
   -- recuperar el ideReclutapersona Examen
   SELECT RC.IDERECLUPERSOEXAMEN, RC.IDEEXAMENXCATEGORIA
   INTO c_ideReclutaPersExamen,c_ideExamenCategoria
   FROM RECL_PERS_EXAM_CAT RC
   WHERE RC.IDERECLPERSOEXAMNCAT = p_ideReclPerExaCat;
   
   SELECT COUNT(*)
   INTO c_nroCategoriaExamen
   FROM RECL_PERS_EXAM_CAT  RCA
   WHERE RCA.IDERECLUPERSOEXAMEN = c_ideReclutaPersExamen
   AND RCA.IDERECLUTAPERSONA = p_ideReclutaPersona;
   
   SELECT COUNT(*)
   INTO c_nroCategExamFin
   FROM RECL_PERS_EXAM_CAT RCAT
   WHERE RCAT.ESTADO = P_ESTCAT_FINALIZADO
   AND RCAT.IDERECLUPERSOEXAMEN = c_ideReclutaPersExamen
   AND RCAT.IDERECLUTAPERSONA = p_ideReclutaPersona;
   
   
   p_retVal:=0;
   BEGIN
   IF (c_nroCategoriaExamen = c_nroCategExamFin)THEN
     
     SELECT RPE.EVALUACION INTO c_nroEvaluaciones 
     FROM RECLUTAMIENTO_PERSONA RPE 
     WHERE RPE.IDERECLUTAPERSONA = p_ideReclutaPersona FOR UPDATE;
     UPDATE RECLUTAMIENTO_PERSONA RP
     SET RP.EVALUACION = c_nroEvaluaciones +1,
         RP.INDPROCESO = NULL  ---INDICADOR PROCESO EN NULL --nose para que- preguntar a ELLAMOCA
     WHERE RP.IDERECLUTAPERSONA = p_ideReclutaPersona;
     
     
     --Sumar los puntajes para el calculo de la 
     --nota final solo si el examen es de tipo examen
    SELECT REX.TIPEXAMEN
    INTO c_tipoExamen
    FROM RECLU_PERSO_EXAMEN REX
    WHERE REX.IDERECLUPERSOEXAMEN = c_ideReclutaPersExamen;       
     
    IF(c_tipoExamen = P_TIPEXA_EXAMEN) THEN --TIPO EXAMEN - de calificacion automatica
    
     
       --puntaje total cuenta las preguntas diferenctes  a cero, las toma como correctas                 
       SELECT SUM(RPCAT.NROPREGUNTAS), SUM(RPCAT.NOTAEXAMENCATEG)
       INTO c_nroTotalPreguntas, c_puntajeTotal    
       FROM RECL_PERS_EXAM_CAT RPCAT
       WHERE RPCAT.IDERECLUPERSOEXAMEN = c_ideReclutaPersExamen
       AND RPCAT.IDERECLUTAPERSONA = p_ideReclutaPersona;
       
       c_puntFinalExamen:= c_puntajeTotal*20/c_nroTotalPreguntas;
        
       IF (c_puntFinalExamen > 10) THEN
        c_estadoEvaluacion := P_ESTEXAMEN_APROBADO;
       ELSE
        c_estadoEvaluacion := P_ESTEXAMEN_DESAPROBADO;
       END IF; 
        
       --Actualizar la nota del examen
       UPDATE RECLU_PERSO_EXAMEN RE
       SET RE.NOTAFINAL = c_puntFinalExamen,
           RE.USRMODIFICA=p_usuarioCreacion,
           RE.FECMODIFICA=SYSDATE,
           RE.FECEVALUACION = SYSDATE,
           RE.HORAEVALUACION = SYSDATE,
           RE.TIPESTEVALUACION = c_estadoEvaluacion
       WHERE RE.IDERECLUPERSOEXAMEN = c_ideReclutaPersExamen
       AND RE.IDERECLUTAPERSONA = p_ideReclutaPersona;
     
    ELSE
      IF(c_tipoExamen = P_TIPEXA_EVALUACION) THEN
        UPDATE RECLU_PERSO_EXAMEN RE
          SET RE.USRMODIFICA=p_usuarioCreacion,
            RE.FECMODIFICA=SYSDATE,
            RE.FECEVALUACION = SYSDATE,
            RE.HORAEVALUACION = SYSDATE,
            RE.TIPESTEVALUACION = P_ESTEXAMEN_EVALUADO
          WHERE RE.IDERECLUPERSOEXAMEN = c_ideReclutaPersExamen
        AND RE.IDERECLUTAPERSONA = p_ideReclutaPersona;
      END IF;
    END IF;
     COMMIT;
     p_retVal:=1;
     
   END IF;
        
    EXCEPTION
      WHEN OTHERS THEN
      ROLLBACK;
      p_retVal := 0;
    END;    
  
END SP_CALIFICAR_EXAMEN;


/* ------------------------------------------------------------
    Nombre      : SP_RECUPERAR_EXAMEN
    Proposito   : Obtiene el examen rendido por el postulante
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      08/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_RECUPERAR_EXAMEN(p_ideReclutaPersona   IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE,
                              p_iderecluPersExamen  IN RECL_PERS_EXAM_CAT.IDERECLPERSOEXAMNCAT%TYPE,
                              dtExamen              OUT SYS_REFCURSOR,
                              dtCategoriaExamen     OUT SYS_REFCURSOR,
                              dtCategoriaSubCatego  OUT SYS_REFCURSOR,
                              dtCriterioAlternativa OUT SYS_REFCURSOR,
                              dtAlternativas        OUT SYS_REFCURSOR)IS

    
c_ideExamen              EXAMEN.IDEEXAMEN%TYPE; 
c_ideSede                SEDE.IDESEDE%TYPE;
c_idecategoria           CATEGORIA.IDECATEGORIA%TYPE; 
c_ideCritSubcategoria    CRITERIO_X_SUBCATEGORIA.IDECRITERIOXSUBCATEGORIA%TYPE;   
c_ideRecluPersoCriter    RECLU_PERSO_CRITERIO.IDERECLUPERSOCRITERIO%TYPE;   


BEGIN
  
  SELECT RP.IDSEDE
  INTO c_ideSede
  FROM RECLUTAMIENTO_PERSONA RP
  WHERE RP.IDERECLUTAPERSONA = p_ideReclutaPersona;
  
  --Cursor de cabezera del examen
  OPEN dtExamen FOR
    SELECT RPE.IDERECLUPERSOEXAMEN,UPPER(EX.NOMEXAMEN) NOMEXAMEN, UPPER(EX.DESCEXAMEN)DESCEXAMEN, EX.TIPEXAMEN, 
           RPE.NOTAFINAL, UPPER(PR_INTRANET.FN_OBTENER_NOMBRE_POST(p_ideReclutaPersona)) NOMBPOSTULANTE
    FROM RECLU_PERSO_EXAMEN RPE, EXAMEN EX
    WHERE RPE.IDEEXAMEN = EX.IDEEXAMEN
    AND RPE.IDERECLUPERSOEXAMEN = p_iderecluPersExamen
    AND RPE.IDERECLUTAPERSONA = p_ideReclutaPersona;    
  
  --Cursor de datos de la categoria
  OPEN dtCategoriaExamen FOR
    SELECT  RCAT.IDERECLPERSOEXAMNCAT, RCAT.IDERECLUPERSOEXAMEN, RCAT.NROPREGUNTAS,
           RCAT.NOTAEXAMENCATEG, UPPER(CT.NOMCATEGORIA) NOMCATEGORIA,UPPER(CT.DESCCATEGORIA) DESCCATEGORIA, CT.TIEMPO 
    FROM RECL_PERS_EXAM_CAT RCAT, EXAMEN_X_CATEGORIA EXC , CATEGORIA CT 
    WHERE RCAT.IDEEXAMENXCATEGORIA  = EXC.IDEEXAMENXCATEGORIA
    AND CT.IDECATEGORIA = EXC.IDECATEGORIA
    AND EXC.IDESEDE = c_ideSede
    AND RCAT.IDERECLUTAPERSONA = p_ideReclutaPersona
    AND RCAT.IDERECLUPERSOEXAMEN = p_iderecluPersExamen
    ORDER BY CT.ORDENIMPRESION ASC;
 
 --Cursor de datos de la subcategoria  
   
  OPEN dtCategoriaSubCatego FOR
    SELECT DISTINCT(SC.IDESUBCATEGORIA) IDESUBCATEGORIA,RCRIT.IDERECLPERSOEXAMNCAT,RCAT.IDERECLUPERSOEXAMEN, UPPER(SC.NOMSUBCATEGORIA)NOMSUBCATEGORIA, 
           UPPER(SC.DESCSUBCATEGORIA) DESCSUBCATEGORIA, SC.ORDENIMPRESION
    FROM RECLU_PERSO_CRITERIO RCRIT, CRITERIO_X_SUBCATEGORIA CSB, SUBCATEGORIA SC , RECL_PERS_EXAM_CAT RCAT
    WHERE RCRIT.IDECRITERIOXSUBCATEGORIA = CSB.IDECRITERIOXSUBCATEGORIA
    AND CSB.IDESEDE = c_ideSede
    AND SC.IDESEDE = c_ideSede
    AND CSB.IDESUBCATEGORIA = SC.IDESUBCATEGORIA 
    AND RCRIT.IDERECLPERSOEXAMNCAT = RCAT.IDERECLPERSOEXAMNCAT
    AND RCRIT.IDERECLUTAPERSONA = p_ideReclutaPersona
    AND RCAT.IDERECLUPERSOEXAMEN = p_iderecluPersExamen
    ORDER BY SC.ORDENIMPRESION ASC;
    
  
  OPEN dtCriterioAlternativa FOR
    SELECT  RCRIT.IDERECLUPERSOCRITERIO,RCRIT.IDERECLPERSOEXAMNCAT,RCRIT.IDECRITERIOXSUBCATEGORIA,CRSC.IDESUBCATEGORIA, CR.PREGUNTA,CR.TIPMODO, 
            CR.IMAGENCRIT,RCRIT.INDRESPUESTA, RCRIT.PUNTTOTAL
    FROM   CRITERIO CR, CRITERIO_X_SUBCATEGORIA CRSC, RECLU_PERSO_CRITERIO RCRIT, RECL_PERS_EXAM_CAT RCAT
    WHERE RCRIT.IDECRITERIOXSUBCATEGORIA = CRSC.IDECRITERIOXSUBCATEGORIA
    AND CR.IDESEDE = c_ideSede
    AND CRSC.IDESEDE = c_ideSede
    AND CRSC.IDECRITERIO = CR.IDECRITERIO
    AND RCRIT.IDERECLPERSOEXAMNCAT  = RCAT.IDERECLPERSOEXAMNCAT
    AND RCRIT.IDERECLUTAPERSONA = p_ideReclutaPersona
    AND RCAT.IDERECLUPERSOEXAMEN = p_iderecluPersExamen
    ORDER BY CR.ORDENIMPRESION;
    
  OPEN dtAlternativas FOR
    SELECT RC.IDERECLUPERSOCRITERIO,
           CS.IDECRITERIO, 
           CS.IDECRITERIOXSUBCATEGORIA, 
           AL.ALTERNATIVA, 
           AL.IDEALTERNATIVA, 
           AL.IMAGE, 
           AL.PESO, 
           RAL.IDEALTERNATIVA,
           RAL.IDERECLUPERSOCRITERIO, 
           CASE WHEN RAL.IDEALTERNATIVA IS NULL THEN 'N' ELSE 'S' END  RESPUESTA
    FROM RECL_PERS_EXAM_CAT  RCAT, 
         (SELECT * FROM  CRITERIO_X_SUBCATEGORIA C WHERE C.IDESEDE = c_ideSede)CS ,
         RECLU_PERSO_CRITERIO RC , 
         (SELECT * FROM  ALTERNATIVA A WHERE A.IDESEDE = c_ideSede) AL  
         LEFT JOIN (
                    SELECT * 
                    FROM RECLU_PERSO_ALTERNATIVA RA 
                    WHERE RA.IDERECLUTAPERSONA = p_ideReclutaPersona) RAL 
         ON (AL.IDEALTERNATIVA = RAL.IDEALTERNATIVA)
    WHERE AL.IDECRITERIO = CS.IDECRITERIO
    AND RC.IDECRITERIOXSUBCATEGORIA = CS.IDECRITERIOXSUBCATEGORIA
    AND RCAT.IDERECLPERSOEXAMNCAT = RC.IDERECLPERSOEXAMNCAT
    AND RCAT.IDERECLUPERSOEXAMEN = p_iderecluPersExamen
    AND RCAT.IDERECLUTAPERSONA = p_ideReclutaPersona
    AND RC.IDERECLUTAPERSONA = p_ideReclutaPersona;
    --ORDER BY RC.IDERECLUPERSOCRITERIO;
    
  
END SP_RECUPERAR_EXAMEN;


/* ------------------------------------------------------------
    Nombre      : SP_REPORTE_POSTULANTESBD
    Proposito   : obtiene la lista de los postulante no 
                  asociados a ningun cargo que cumplan cierto perfil
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      21/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_REPORTE_POSTULANTESBD(p_nombreCargo     IN CARGO.NOMCARGO%TYPE,
                                   p_areaEstudio     IN ESTUDIOS_POSTULANTE.TIPAREA%TYPE,
                                   p_rangoSalario    IN DETALLE_GENERAL.VALOR%TYPE,
                                   p_departamento    IN UBIGEO.IDEUBIGEO%TYPE,                        
                                   p_provincia       IN UBIGEO.IDEUBIGEO%TYPE,
                                   p_distrito        IN UBIGEO.IDEUBIGEO%TYPE,
                                   p_fecDesde        IN VARCHAR2,
                                   p_fecHasta        IN VARCHAR2,
                                   p_edadInicio      IN NUMBER,
                                   p_edadFin         IN NUMBER,
                                   p_cRetVal         OUT SYS_REFCURSOR)IS
          

c_nombreCargo    CARGO.NOMCARGO%TYPE;
c_areaEstudio    ESTUDIOS_POSTULANTE.TIPAREA%TYPE;
c_rangoSalario   DETALLE_GENERAL.VALOR%TYPE;
c_departamento   UBIGEO.IDEUBIGEO%TYPE;                        
c_provincia      UBIGEO.IDEUBIGEO%TYPE;
c_distrito       UBIGEO.IDEUBIGEO%TYPE;
c_edadInicio     NUMBER;
c_edadFin        NUMBER;
          
BEGIN
      
      IF p_nombreCargo IS NULL THEN
        c_nombreCargo := 'ABC';
      ELSE
        c_nombreCargo := p_nombreCargo;
      END IF;
      
   
      IF p_rangoSalario IS NULL THEN
        c_rangoSalario := '0';
      ELSE
        c_rangoSalario := p_rangoSalario;
      END IF;
       
      IF p_areaEstudio IS NULL THEN
        c_areaEstudio := '0';
      ELSE
        c_areaEstudio := p_areaEstudio;
      END IF;
      
      IF p_departamento IS NULL THEN
        c_departamento := 0;
      ELSE
        c_departamento :=  p_departamento;                                                                                        
      END IF;
      
      IF p_provincia IS NULL THEN
        c_provincia := 0;
      ELSE
        c_provincia := p_provincia;
      END IF;
      
      IF p_distrito IS NULL THEN
        c_distrito := 0;
      ELSE
        c_distrito := p_distrito;
      END IF;
      
      IF p_edadInicio IS NULL THEN
        c_edadInicio := 0;
      ELSE
        c_edadInicio:= p_edadInicio;
      END IF;
      
      IF p_edadFin IS NULL or p_edadFin=0 THEN
        c_edadFin := 100;
      ELSE
        c_edadFin:= p_edadFin;
      END IF;
      
        
     BEGIN
     OPEN  p_cRetVal FOR  
     SELECT *
     FROM ( 
        SELECT  PBD.IDEPOSTULANTE,
                TRUNC(PBD.FECCREACION) FECCREACION ,
                PBD.DEPARTAMENTO,
                PBD.PROVINCIA,
                PBD.DISTRITO,
                PBD.NOMBREAPELLIDO,
                PBD.TELEFONO,
                PBD.CORREO,
                PBD.CARGO,
                PBD.EDAD,
                PBD.TIPOESTUDIO,
                PBD.TIPOAREA,
                PBD.RANGOSALARIO,
                ROW_NUMBER()
                OVER (PARTITION BY PBD.IDEPOSTULANTE
                      ORDER BY PBD.TIPEDUCACION DESC ) AS ROWNUMBER
         FROM (SELECT  P.IDEPOSTULANTE, P.FECCREACION, 
                       UP.IDEDEPARTAMENTO , UP.DEPARTAMENTO, 
                       UP.IDEPROVINCIA, UP.PROVINCIA,
                       UP.IDEDISTRITO, UP.DISTRITO,
                       P.PRINOMBRE||' '||P.SEGNOMBRE||' '|| P.APEPATERNO||' '|| P.APEMATERNO NOMBREAPELLIDO, 
                       CASE WHEN TO_CHAR(P.TELMOVIL) IS NOT NULL THEN TO_CHAR(P.TELMOVIL) ELSE P.TELFIJO END TELEFONO , 
                       P.CORREO, 
                       CASE WHEN (EXP.TIPCARGOTRABAJO = 'XX')THEN EXP.NOMCARGOTRABAJO ELSE (SELECT DG.DESCRIPCION 
                                                                                    FROM DETALLE_GENERAL DG 
                                                                                    WHERE DG.IDEGENERAL = 24 AND DG.VALOR = EXP.TIPCARGOTRABAJO)END CARGO, 
                       TO_NUMBER(TRUNC(MONTHS_BETWEEN(SYSDATE,P.FECNACIMIENTO)/12)) EDAD, EP.TIPEDUCACION,
                       (SELECT DG.DESCRIPCION 
                        FROM DETALLE_GENERAL DG 
                        WHERE DG.IDEGENERAL = 20 AND DG.VALOR= EP.TIPEDUCACION) TIPOESTUDIO,  
                        EP.TIPAREA,(SELECT DETG.DESCRIPCION FROM DETALLE_GENERAL DETG WHERE DETG.IDEGENERAL = 19 AND DETG.VALOR = EP.TIPAREA ) TIPOAREA,       
                        P.TIPSALARIO,(SELECT DT.DESCRIPCION FROM DETALLE_GENERAL DT WHERE DT.IDEGENERAL = 11 AND DT.VALOR = P.TIPSALARIO) RANGOSALARIO
               FROM  (SELECT DEP.IDEUBIGEO IDEDEPARTAMENTO, DEP.NOMBRE DEPARTAMENTO, PROV.IDEUBIGEO IDEPROVINCIA, PROV.NOMBRE PROVINCIA , DIST.IDEUBIGEO IDEDISTRITO, DIST.NOMBRE DISTRITO
                      FROM UBIGEO PROV , ( SELECT * FROM UBIGEO U  WHERE U.IDEUBIGEOPADRE IS NULL) DEP , (SELECT * FROM UBIGEO UBI ) DIST
                      WHERE PROV.IDEUBIGEOPADRE = DEP.IDEUBIGEO
                      AND DIST.IDEUBIGEOPADRE = PROV.IDEUBIGEO) UP, 
                      EXP_POSTULANTE EXP, 
                      POSTULANTE P ,
                      ESTUDIOS_POSTULANTE EP 
               WHERE P.IDEPOSTULANTE = EXP.IDEPOSTULANTE
               AND  EP.IDEPOSTULANTE = P.IDEPOSTULANTE
               AND UP.IDEDISTRITO = P.IDEUBIGEO
               AND P.IDEPOSTULANTE NOT IN (SELECT RP.IDEPOSTULANTE FROM RECLUTAMIENTO_PERSONA RP)
               ORDER BY P.IDEPOSTULANTE , EP.TIPEDUCACION DESC ) PBD 
       WHERE UPPER(PBD.CARGO) LIKE '%'||UPPER(c_nombreCargo)||'%'
       AND  (c_departamento = 0 OR PBD.IDEDEPARTAMENTO  = c_departamento)
       AND  (c_provincia = 0 OR PBD.IDEPROVINCIA = c_provincia)
       AND  (c_distrito = 0 OR PBD.IDEDISTRITO = c_distrito)
       AND  (c_areaEstudio = '0' OR PBD.TIPAREA = c_areaEstudio)
       AND  (c_rangoSalario = '0' OR PBD.TIPSALARIO = c_rangoSalario)
       AND  (PBD.EDAD >= c_edadInicio)
       AND  (PBD.EDAD < c_edadFin+1)
       AND ( NVL(p_fecDesde,NULL) IS NULL OR  PBD.FECCREACION >= TO_DATE(p_fecDesde, 'DD/MM/YYYY'))
       AND ( NVL(p_fecHasta,NULL) IS NULL OR PBD.FECCREACION < TO_DATE(p_fecHasta, 'DD/MM/YYYY') +1)
       )
    WHERE ROWNUMBER = 1;

    EXCEPTION
      WHEN OTHERS THEN
      p_cRetVal:= NULL;
    END;

END SP_REPORTE_POSTULANTESBD;

/* ------------------------------------------------------------
    Nombre      : FN_TOTAL_PUNTAJE_EXAMEN
    Proposito   : funcion para calcular el tuntaje total de los examenes 
                  de un postulante para determinada postulacion
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_TOTAL_PUNTAJE_EXAMEN(p_ideReclutaPersona IN RECLUTAMIENTO_PERSONA.IDERECLUTAPERSONA%TYPE
                                 )RETURN NUMBER 

IS
pjteTotal NUMBER;

BEGIN
  
  SELECT SUM(RPE.NOTAFINAL) 
  INTO pjteTotal
  FROM RECLU_PERSO_EXAMEN RPE
  WHERE RPE.IDERECLUTAPERSONA = p_ideReclutaPersona;

  RETURN NVL(pjteTotal,0);
  
END FN_TOTAL_PUNTAJE_EXAMEN;



/* ------------------------------------------------------------
    Nombre      : SP_REPORTE_POST_POTENCIAL
    Proposito   : obtiene la lista de los postulante potenciales 
                  para determinado cargo
                  
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      23/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_REPORTE_POST_POTENCIAL(p_ideCargo        IN CARGO.IDECARGO%TYPE,
                                    p_areaEstudio     IN DETALLE_GENERAL.VALOR%TYPE,
                                    p_rangoSalario    IN DETALLE_GENERAL.VALOR%TYPE,
                                    p_ideSede         IN SEDE.IDESEDE%TYPE,                        
                                    p_ideDependencia  IN DEPENDENCIA.IDEDEPENDENCIA%TYPE,
                                    p_ideDepartamento IN DEPARTAMENTO.IDEDEPARTAMENTO%TYPE,
                                    p_ideArea         IN AREA.IDEAREA%TYPE,
                                    p_fecDesde        IN VARCHAR2,
                                    p_fecHasta        IN VARCHAR2,
                                    p_edadInicio      IN NUMBER,
                                    p_edadFin         IN NUMBER,
                                    p_cRetVal         OUT SYS_REFCURSOR )IS


c_ideCargo      CARGO.NOMCARGO%TYPE;
c_areaEstudio      DETALLE_GENERAL.VALOR%TYPE;
c_rangoSalario     DETALLE_GENERAL.VALOR%TYPE;
c_ideSede          SEDE.IDESEDE%TYPE;                        
c_ideDependencia   DEPENDENCIA.IDEDEPENDENCIA%TYPE;
c_ideDepartamento  DEPARTAMENTO.IDEDEPARTAMENTO%TYPE;
c_ideArea          AREA.IDEAREA%TYPE;
c_edadInicio       NUMBER;
c_edadFin          NUMBER;
          
BEGIN
      
      IF p_ideCargo IS NULL THEN
        c_ideCargo := 0;
      ELSE
        c_ideCargo := p_ideCargo;
      END IF;
      
      IF p_rangoSalario IS NULL THEN
        c_rangoSalario := '0';
      ELSE
        c_rangoSalario := p_rangoSalario;
      END IF;
       
      IF p_areaEstudio IS NULL THEN
        c_areaEstudio := '0';
      ELSE
        c_areaEstudio := p_areaEstudio;
      END IF;
      
      IF ((p_ideSede IS NULL) OR (p_ideSede = 999)) THEN
        c_ideSede := 0;
      ELSE
        c_ideSede :=  p_ideSede;                                                                                        
      END IF;
      
      IF p_ideDependencia IS NULL THEN
        c_ideDependencia := 0;
      ELSE
        c_ideDependencia :=  p_ideDependencia;                                                                                        
      END IF;
      
      IF p_ideDepartamento IS NULL THEN
        c_ideDepartamento := 0;
      ELSE
        c_ideDepartamento := p_ideDepartamento;
      END IF;
      
      IF p_ideArea IS NULL THEN
        c_ideArea := 0;
      ELSE
        c_ideArea := p_ideArea;
      END IF;
      
      IF p_edadInicio IS NULL THEN
        c_edadInicio := 0;
      ELSE
        c_edadInicio:= p_edadInicio;
      END IF;
      
      IF p_edadFin IS NULL THEN
        c_edadFin := 100;
      ELSE
        c_edadFin:= p_edadFin;
      END IF;
  
  OPEN p_cRetVal FOR
  SELECT *
  FROM (                 
        SELECT PP.IDERECLUTAPERSONA,
               PP.IDEPOSTULANTE,
               PP.FECPOTENCIAL,
               (SELECT S.DESCRIPCION FROM SEDE S WHERE S.IDESEDE = PP.IDESEDE) AS NOMBRESEDE,
               (SELECT D.NOMDEPENDENCIA 
                FROM DEPENDENCIA D 
                WHERE D.IDEDEPENDENCIA = PP.IDEDEPENDENCIA
                AND D.IDESEDE = PP.IDESEDE
                AND D.ESTACTIVO = 'A') AS NOMBREDEPENDENCIA,
               (SELECT DE.NOMDEPARTAMENTO 
                FROM DEPARTAMENTO DE 
                WHERE DE.IDEDEPARTAMENTO = PP.IDEDEPARTAMENTO
                AND DE.IDEDEPENDENCIA = PP.IDEDEPENDENCIA
                AND DE.ESTACTIVO = 'A') AS NOMBREDEPARTAMENTO,
               (SELECT A.NOMAREA 
                FROM AREA A 
                WHERE A.IDEAREA = PP.IDEAREA
                AND A.IDEDEPARTAMENTO = PP.IDEDEPARTAMENTO
                AND A.ESTACTIVO = 'A') AS NOMBREAREA,
               PP.NOMBREPOSTULANTE,
               PP.NOMBRECARGO,
               PP.TELEFONO,
               PP.CORREO,
               PP.EDAD,
               PP.TIPOEDUCACION,
               PP.PTJECV,
               FN_TOTAL_PUNTAJE_EXAMEN(PP.IDERECLUTAPERSONA) PTJESELECCION,
               PP.TIPOAREA,
               PP.RANGOSALARIO,
               ROW_NUMBER()
                      OVER (PARTITION BY PP.IDERECLUTAPERSONA
                            ORDER BY PP.TIPEDUCACION DESC ) AS ROWNUMBER
        FROM (SELECT RP.IDERECLUTAPERSONA,
                     RP.INDPOTENCIAL,
                     RP.FECPOTENCIAL,
                     P.IDEPOSTULANTE,
                     SOL.IDESEDE, 
                     SOL.IDEDEPENDENCIA,
                     SOL.IDEDEPARTAMENTO, 
                     SOL.IDEAREA,
                     P.PRINOMBRE||' '||P.SEGNOMBRE||' '||P.APEPATERNO||' '||P.APEMATERNO NOMBREPOSTULANTE,
                     SOL.NOMBRECARGO,
                     SOL.IDECARGO,
                     CASE WHEN TO_CHAR(P.TELMOVIL) IS NOT NULL THEN TO_CHAR(P.TELMOVIL) ELSE P.TELFIJO END TELEFONO ,
                     P.CORREO,
                     TO_NUMBER(TRUNC(MONTHS_BETWEEN(SYSDATE,P.FECNACIMIENTO)/12)) EDAD,
                     RP.PTOTOTAL,
                     EP.TIPEDUCACION,
                     (SELECT DT.DESCRIPCION FROM DETALLE_GENERAL DT WHERE DT.IDEGENERAL = 20 AND DT.VALOR = EP.TIPEDUCACION) TIPOEDUCACION,
                     RP.PTOTOTAL PTJECV,
                     EP.TIPAREA,
                     (SELECT DT.DESCRIPCION FROM DETALLE_GENERAL DT WHERE DT.IDEGENERAL = 19 AND DT.VALOR = EP.TIPAREA) TIPOAREA,
                     P.TIPSALARIO,
                     (SELECT DT.DESCRIPCION FROM DETALLE_GENERAL DT WHERE DT.IDEGENERAL = 11 AND DT.VALOR = P.TIPSALARIO) RANGOSALARIO 
             FROM  POSTULANTE P LEFT JOIN ESTUDIOS_POSTULANTE EP  ON P.IDEPOSTULANTE = EP.IDEPOSTULANTE , 
                   RECLUTAMIENTO_PERSONA RP , 
                   (SELECT *             
                    FROM (SELECT SN.IDECARGO, SN.IDESEDE IDESEDE,SN.IDEDEPENDENCIA IDEDEPENDENCIA,SN.IDEDEPARTAMENTO IDEDEPARTAMENTO,
                          SN.IDEAREA IDEAREA,SN.NOMBRE NOMBRECARGO,SN.IDESOLNUEVOCARGO IDESOL , '01' TIPSOL 
                          FROM SOLNUEVO_CARGO SN) 
                          UNION ALL
                         (SELECT SR.IDECARGO, SR.IDESEDE IDESEDE,SR.IDEDEPENDENCIA IDEDEPENDENCIA,SR.IDEDEPARTAMENTO IDEDEPARTAMENTO,
                          SR.IDEAREA IDEAREA, SR.NOMCARGO NOMBRECARGO, SR.IDESOLREQPERSONAL IDESOL,SR.TIPSOL 
                          FROM SOLREQ_PERSONAL SR)) SOL 
             WHERE RP.IDESOL = SOL.IDESOL
             AND RP.TIPSOL = SOL.TIPSOL
             AND P.IDEPOSTULANTE = RP.IDEPOSTULANTE
             AND RP.INDPOTENCIAL = 'S'
             ORDER BY RP.IDERECLUTAPERSONA,EP.TIPEDUCACION DESC ) PP
         WHERE PP.IDECARGO = c_ideCargo AND
          (c_ideSede = 0 OR PP.IDESEDE  = c_ideSede)
         AND  (c_ideDependencia = 0 OR PP.IDEDEPENDENCIA = c_ideDependencia)
         AND  (c_ideDepartamento = 0 OR PP.IDEDEPARTAMENTO  = c_ideDepartamento)
         AND  (c_ideArea = 0 OR PP.IDEAREA = c_ideArea)
         AND  (c_areaEstudio = '0' OR PP.TIPAREA = c_areaEstudio)
         AND  (c_rangoSalario = '0' OR PP.TIPSALARIO = c_rangoSalario)
         AND  (PP.EDAD >= c_edadInicio)
         AND  (PP.EDAD < c_edadFin+1)
         AND ( NVL(p_fecDesde,NULL) IS NULL OR  PP.FECPOTENCIAL >= TO_DATE(p_fecDesde, 'DD/MM/YYYY'))
         AND ( NVL(p_fecHasta,NULL) IS NULL OR PP.FECPOTENCIAL < TO_DATE(p_fecHasta, 'DD/MM/YYYY') +1))
     WHERE ROWNUMBER = 1;

END SP_REPORTE_POST_POTENCIAL;

/* ------------------------------------------------------------
    Nombre      : SP_CARGOS_MANT
    Proposito   : Procedimiento para listar los cargos de acuerdo 
                  a la sede seleccionada o todas
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_CARGOS_MANT(p_ideSede  IN SEDE.IDESEDE%TYPE,
                         p_cRetVal  OUT SYS_REFCURSOR)IS

c_ideSede SEDE.IDESEDE%TYPE;

BEGIN

  IF ((p_ideSede = 0) OR (p_ideSede = 999)) THEN
    c_ideSede := 0;
  ELSE
    c_ideSede := p_ideSede;
  END IF;
  
  OPEN p_cRetVal FOR  
    SELECT DISTINCT C.CODCARGO,C.IDECARGO,C.NOMCARGO
    FROM CARGO C
    WHERE 1=1
    --WHERE C.ESTACTIVO = 'A'
    AND NVL(C.VERSION, 1) =
       (SELECT NVL(MAX(C1.VERSION), 1)
        FROM CARGO C1
        WHERE C1.ESTACTIVO = 'A'
        AND C1.CODCARGO = C.CODCARGO)
    AND (c_ideSede = 0 OR C.IDESEDE = c_ideSede)
    ORDER BY C.NOMCARGO;
  
END SP_CARGOS_MANT;



/* ------------------------------------------------------------
    Nombre      : SP_CARGOS_SEDE
    Proposito   : Procedimiento para listar los cargos de acuerdo 
                  a la sede seleccionada o todas
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      24/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_CARGOS_SEDE(p_ideSede  IN SEDE.IDESEDE%TYPE,
                         p_cRetVal  OUT SYS_REFCURSOR)IS

c_ideSede SEDE.IDESEDE%TYPE;

BEGIN

  IF ((p_ideSede = 0) OR (p_ideSede = 999)) THEN
    c_ideSede := 0;
  ELSE
    c_ideSede := p_ideSede;
  END IF;
  
  OPEN p_cRetVal FOR  
    SELECT DISTINCT C.IDECARGO, C.NOMCARGO
    FROM CARGO C
    WHERE C.ESTACTIVO = 'A'
    AND NVL(C.VERSION, 1) =
       (SELECT NVL(MAX(C1.VERSION), 1)
        FROM CARGO C1
        WHERE C1.ESTACTIVO = 'A'
        AND C1.CODCARGO = C.CODCARGO)
    AND (c_ideSede = 0 OR C.IDESEDE = c_ideSede)
    ORDER BY C.NOMCARGO;
  
END SP_CARGOS_SEDE;


/* ------------------------------------------------------------
    Nombre      : FN_EXISTE_RESULTADO_EXAMEN
    Proposito   : Funci?n para determinar si un examen tiene resultado
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      29/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_EXISTE_RESULTADO_EXAMEN(p_ideReclutaExamen  IN RECLU_PERSO_EXAMEN.IDERECLUPERSOEXAMEN%TYPE
                                    ) RETURN VARCHAR IS


 c_EstadoEvaluacion   RECLU_PERSO_EXAMEN.TIPESTEVALUACION%TYPE;
 c_TipoExamen         RECLU_PERSO_EXAMEN.TIPEXAMEN%TYPE;
BEGIN
  
  BEGIN
  SELECT RE.TIPESTEVALUACION, RE.TIPEXAMEN
  INTO c_EstadoEvaluacion,c_TipoExamen
  FROM RECLU_PERSO_EXAMEN RE
  WHERE RE.IDERECLUPERSOEXAMEN = p_ideReclutaExamen;

  IF c_TipoExamen = P_TIPEXA_EXAMEN OR c_TipoExamen = P_TIPEXA_EVALUACION THEN
    IF ((c_EstadoEvaluacion = P_ESTEXAMEN_EVALUADO )OR
        (c_EstadoEvaluacion = P_ESTEXAMEN_APROBADO) OR
        (c_EstadoEvaluacion = P_ESTEXAMEN_DESAPROBADO)) THEN
        RETURN 'S';
    ELSE
        RETURN 'N';
    END IF;
  ELSE
    RETURN 'N';
  END IF;
  
  EXCEPTION
    WHEN OTHERS THEN
    RETURN 'N';
  END;
  
END FN_EXISTE_RESULTADO_EXAMEN;

/* ------------------------------------------------------------
    Nombre      : SP_GET_CARGOXSEDE
    Proposito   : Funci?n para determinar si un examen tiene resultado
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      29/04/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
  PROCEDURE SP_GET_CARGOXSEDE(p_nIdSede         IN NUMBER,
                              p_nIdDependencia  IN NUMBER,
                              p_nIdDepartamento IN NUMBER,
                              p_nIdArea         IN NUMBER,
                              p_cRetVal         OUT SYS_REFCURSOR) IS
  
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
         AND C.TIPETAPA IN ('08','06','04','10')
       ORDER BY C.NOMCARGO asc;
  
  END SP_GET_CARGOXSEDE;


END PR_REQUERIMIENTOS;
/
