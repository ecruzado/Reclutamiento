CREATE OR REPLACE PACKAGE PR_REQUERIMIENTOS is


FUNCTION FN_APROBACION_NUEVO(p_ideSede     IN SEDE.IDESEDE%TYPE,
                              p_ideArea     IN AREA.IDEAREA%TYPE,
                              p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_ideUsuario  IN USUARIO.IDUSUARIO%TYPE,
                              p_ideRol      IN ROL.IDROL%TYPE,
                              p_observacion IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                              p_suceso      IN DETALLE_GENERAL.VALOR%TYPE,
                              p_etapa       IN DETALLE_GENERAL.VALOR%TYPE)RETURN NUMBER;
                              
PROCEDURE SP_OBTENER_ETAPA_SOLICITUD(p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR);
                                     
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)
                            RETURN VARCHAR2;

FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)
                           RETURN VARCHAR2;




END PR_REQUERIMIENTOS;
/
CREATE OR REPLACE PACKAGE BODY PR_REQUERIMIENTOS is

/* ------------------------------------------------------------
    Nombre      : FN_APROBACION_NUEVO
    Proposito   : Actualizar el estado de la Solicitud y agregar nueva etapa
                  dependiendo del caso.
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      20/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
FUNCTION FN_APROBACION_NUEVO(p_ideSede      IN SEDE.IDESEDE%TYPE,
                              p_ideArea     IN AREA.IDEAREA%TYPE,
                              p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_ideUsuario  IN USUARIO.IDUSUARIO%TYPE,
                              p_ideRol      IN ROL.IDROL%TYPE,
                              p_observacion IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                              p_suceso      IN DETALLE_GENERAL.VALOR%TYPE,
                              p_etapa       IN DETALLE_GENERAL.VALOR%TYPE)RETURN NUMBER IS
                             
                        
c_ideUsuarioResp USUARIO.IDUSUARIO%TYPE;
c_ideLogSolicitud LOGSOLNUEVO_CARGO.IDELOGSOLNUEVOCARGO%TYPE;
c_ideRolRespSuc ROL.IDROL%TYPE;
c_etapaSgte DETALLE_GENERAL.VALOR%TYPE;    
c_ideRolResp ROL.IDROL%TYPE;
c_ideTipoReq CARGO.TIPREQUERIMIENTO%TYPE;

BEGIN   
  IF (p_etapa = '01') THEN-- 01 envio solicitud -ETAPA PENDIENTE SOLICITUD
   c_ideRolResp := 10;     --GERENTE AREA
   --c_etapaSgte :='02';
  ELSIF (p_etapa = '02')THEN --02 aprob/rechazo 
   c_ideRolResp := 5; --GGA 
   --c_etapaSgte :='03';
  ELSIF (p_etapa = '03')THEN --elaboracion perfil 
   c_ideRolResp := 6;   --jefe procesos
  -- c_etapaSgte :='04';
  ELSIF (p_etapa = '04')THEN   --pendiente aprobacion perfil
   c_ideRolResp := 3;  --jefe de area
   --c_etapaSgte :='05';
  ELSIF (p_etapa = '05')THEN   --pendiente aprobacion perfil2  
   c_ideRolResp := 7;  --encargado seleccion
  -- c_etapaSgte :='06';
  ELSIF (p_etapa = '06')THEN   --pendiente publicacion 
   c_ideRolResp := 9; --destinatario recursos 
  ELSIF (p_etapa = '07')THEN   --pendiente publicacion 
   c_ideRolResp := -1; --destinatario recursos 
 -- c_etapaSgte :='07';
  END IF;
  
  IF (c_ideRolResp <> -1) THEN
   SELECT  U.IDUSUARIO
   INTO c_ideUsuarioResp
   FROM USUARIO_NIVEL U, ROL R , USUAROLSEDE UR
   WHERE U.IDUSUARIO = UR.IDUSUARIO
   AND R.IDROL = UR.IDROL
   AND R.IDROL = c_ideRolResp
   AND U.IDEAREA = p_ideArea
   AND U.IDESEDE = p_ideSede
   AND U.FLGESTADO ='A'; -- ESTACTIVO
  ELSE
   c_ideUsuarioResp :=-1;
  END IF; 
  
   --ultimo registro de log de solicitud
   IF( p_etapa <> '01')THEN      
     SELECT LG.IDELOGSOLNUEVOCARGO, LG.ROLRESPONSABLE
     INTO c_ideLogSolicitud, c_ideRolRespSuc
     FROM LOGSOLNUEVO_CARGO LG   
     WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                        FROM  LOGSOLNUEVO_CARGO SN
                        WHERE SN.IDESOLNUEVOCARGO = p_ideSolCargo)
     AND LG.IDESOLNUEVOCARGO =  p_ideSolCargo;  
   ELSE
     c_ideRolRespSuc := p_ideRol;
   END IF;
    
   
   BEGIN
     --   AND ESTADO = "PENDIENTE"
      IF (p_ideRol = c_ideRolRespSuc) THEN
        IF( p_etapa <> '01')THEN
          UPDATE LOGSOLNUEVO_CARGO LS
          SET LS.TIPSUCESO = p_suceso
          WHERE LS.IDELOGSOLNUEVOCARGO = c_ideLogSolicitud;
        END IF;    
        IF(p_suceso <> 'R') THEN --DIFRENTE A RECHAZADO
          IF (p_etapa <> '07') THEN 
            INSERT INTO LOGSOLNUEVO_CARGO 
            (IDELOGSOLNUEVOCARGO, IDESOLNUEVOCARGO,TIPETAPA, TIPSUCESO,OBSERVACION,FECSUCESO,USRSUCESO,ROLSUCESO, USRESPONSABLE,ROLRESPONSABLE)
            VALUES(IDESOLNUEVOCARGO_SQ.NEXTVAL,p_ideSolCargo,p_etapa,'P',p_observacion,sysdate,p_ideUsuario,p_ideRol,c_ideUsuarioResp,c_ideRolResp);
         END IF;
       END IF;
     END IF;
   COMMIT;
     
   EXCEPTION
       WHEN OTHERS THEN  
       c_ideUsuarioResp:= -1;
       ROLLBACK;
   END;
   
   RETURN c_ideUsuarioResp;
END FN_APROBACION_NUEVO;

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
  SELECT LG.TIPETAPA,LG.TIPSUCESO,LG.ROLRESPONSABLE
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
FUNCTION FN_RESPONSABLE_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN
  SELECT R.DSCROL
  INTO c_responsable
  FROM LOGSOLNUEVO_CARGO LG , ROL R   
  WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                     FROM  LOGSOLNUEVO_CARGO SN
                     WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
  AND R.IDROL = LG.ROLRESPONSABLE;

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
FUNCTION FN_NOMRESPONS_SOL(p_ideSolicitud IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE)RETURN VARCHAR2

IS
c_responsable ROL.DSCROL%TYPE;
BEGIN
  SELECT US.DSCNOMBRES||' ' ||US.DSCAPEPATERNO||' '||US.DSCAPEMATERNO
  INTO c_responsable
  FROM LOGSOLNUEVO_CARGO LG , USUARIO US   
  WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                     FROM  LOGSOLNUEVO_CARGO SN
                     WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud)
  AND US.IDUSUARIO = LG.USRESPONSABLE;

  RETURN NVL(c_responsable,'');  
  
END FN_NOMRESPONS_SOL;

END PR_REQUERIMIENTOS;
/
