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
                                     p_ideGeneralE IN DETALLE_GENERAL.IDEGENERAL%TYPE,
                                     p_ideGeneralS IN DETALLE_GENERAL.IDEGENERAL%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR);


END PR_REQUERIMIENTOS;
/
CREATE OR REPLACE PACKAGE BODY PR_REQUERIMIENTOS is


FUNCTION FN_APROBACION_NUEVO(p_ideSede      IN SEDE.IDESEDE%TYPE,
                              p_ideArea     IN AREA.IDEAREA%TYPE,
                              p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                              p_ideUsuario  IN USUARIO.IDUSUARIO%TYPE,
                              p_ideRol      IN ROL.IDROL%TYPE,
                              p_observacion IN LOGSOLNUEVO_CARGO.OBSERVACION%TYPE,
                              p_suceso      IN DETALLE_GENERAL.VALOR%TYPE,
                              p_etapa       IN DETALLE_GENERAL.VALOR%TYPE)RETURN NUMBER IS
                             -- c_ideUsuarioResp OUT USUARIO.IDUSUARIO%TYPE)IS
                        
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
   c_ideRolResp := 3;  --encargado seleccion
  -- c_etapaSgte :='06';
  ELSIF (p_etapa = '06')THEN   --pendiente publicacion 
   c_ideRolResp := 3; --destinatario recursos 
  -- c_etapaSgte :='07';
  END IF;
  
   SELECT  U.IDUSUARIO,R.IDROL
   INTO c_ideUsuarioResp, c_ideRolResp
   FROM USUARIO_NIVEL U, ROL R , USUAROLSEDE UR
   WHERE U.IDUSUARIO = UR.IDUSUARIO
   AND R.IDROL = UR.IDROL
   AND R.IDROL = c_ideRolResp
   AND U.IDEAREA = p_ideArea
   AND U.IDESEDE = p_ideSede
   AND U.FLGESTADO ='A'; -- ESTACTIVO
  
   --ultimo registro de log de solicitud
   IF( p_etapa <> '01')THEN      
     SELECT LG.IDELOGSOLNUEVOCARGO, LG.ROLRESPONSABLE
     INTO c_ideLogSolicitud, c_ideRolRespSuc
     FROM LOGSOLNUEVO_CARGO LG   
     WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                        FROM  LOGSOLNUEVO_CARGO SN
                        WHERE SN.IDESOLNUEVOCARGO = p_ideSolCargo)
     AND LG.IDESOLNUEVOCARGO =  p_ideSolCargo;  
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
          IF (p_etapa <> '06') THEN 
            INSERT INTO LOGSOLNUEVO_CARGO 
            (IDELOGSOLNUEVOCARGO, IDESOLNUEVOCARGO,TIPETAPA, TIPSUCESO,OBSERVACION,FECSUCESO,USRSUCESO,ROLSUCESO, USRESPONSABLE,ROLRESPONSABLE)
            VALUES(IDESOLNUEVOCARGO_SQ.NEXTVAL,p_ideSolCargo,p_etapa,'P',p_observacion,sysdate,p_ideUsuario,p_ideRol,c_ideUsuarioResp,c_ideRolResp);
         END IF;
       END IF;
     END IF;
   COMMIT;
     
   EXCEPTION
       WHEN OTHERS THEN  
       c_ideUsuarioResp:= 0;
       ROLLBACK;
   END;
   
   RETURN c_ideUsuarioResp;
END FN_APROBACION_NUEVO;


PROCEDURE SP_OBTENER_ETAPA_SOLICITUD(p_ideSolCargo IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                                     p_ideGeneralE IN DETALLE_GENERAL.IDEGENERAL%TYPE,
                                     p_ideGeneralS IN DETALLE_GENERAL.IDEGENERAL%TYPE,
                                     p_cRetVal OUT SYS_REFCURSOR)IS

c_tipoEtapa LOGSOLNUEVO_CARGO.TIPETAPA%TYPE;
c_ideEtapa DETALLE_GENERAL.VALOR%TYPE;
c_tipSuceso DETALLE_GENERAL.VALOR%TYPE;

BEGIN

  SELECT LG.TIPETAPA,LG.TIPSUCESO
  INTO c_tipoEtapa,c_tipSuceso
  FROM LOGSOLNUEVO_CARGO LG   
  WHERE FECSUCESO = (SELECT MAX (FECSUCESO)
                     FROM  LOGSOLNUEVO_CARGO SN
                     WHERE SN.IDESOLNUEVOCARGO = p_ideSolCargo)
  AND LG.IDESOLNUEVOCARGO =  p_ideSolCargo; 

  BEGIN  
    SELECT DG.VALOR
    INTO c_ideEtapa
    FROM DETALLE_GENERAL DG
    WHERE DG.IDEGENERAL =  p_ideGeneralE
    AND DG.VALOR =  c_tipoEtapa;
    
    SELECT DG.VALOR
    INTO c_tipSuceso
    FROM DETALLE_GENERAL DG
    WHERE DG.IDEGENERAL =  p_ideGeneralS
    AND DG.VALOR =  c_tipSuceso;
    
    OPEN p_cRetVal FOR
      SELECT c_ideEtapa ETAPA, c_tipSuceso SUCESO
      FROM DUAL;
 
  EXCEPTION
  WHEN OTHERS THEN
    p_cRetVal:=NULL;
  END;   

  
END SP_OBTENER_ETAPA_SOLICITUD;

END PR_REQUERIMIENTOS;
/
