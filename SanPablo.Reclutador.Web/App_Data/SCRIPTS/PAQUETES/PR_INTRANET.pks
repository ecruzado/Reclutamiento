create or replace package PR_INTRANET is

  type cur_cursor is REF CURSOR;
  
  FUNCTION SP_LISTA_LVAL(p_idGeneral IN VARCHAR2,
                         p_valor     IN VARCHAR2)
  RETURN VARCHAR2;
                       
  
  FUNCTION FN_GETMAXPUNTAJE(p_nIdCriterio IN NUMBER)RETURN NUMBER;   
                          
  PROCEDURE SP_GETREPEXAMEN(P_NIDEXAMEN IN NUMBER,
                          p_cRetVal OUT CUR_CURSOR );       
                          
                          
  FUNCTION FN_DURACIONEXAMEN(p_nIdExamen IN NUMBER)RETURN NUMBER;
  
  FUNCTION FN_ELIMINA_ROL(p_nIdRol IN NUMBER)RETURN NUMBER;
  
  FUNCTION FN_ELIMINA_ROL_OPCION(p_nIdRol IN NUMBER,
                               p_nIdOp IN NUMBER)RETURN NUMBER; 
  
                               
  PROCEDURE SP_ACTUALIZAR_PUNTAJES(p_nombreCampoDestino VARCHAR2,
                                   p_ideCargo     CARGO.IDECARGO%TYPE, 
                                   p_valor       IN NUMBER,
                                   p_valorEliminar IN NUMBER); 
                                   
  PROCEDURE SP_PROMEDIO_EVAL_CARGO(p_ideCargo     IN CARGO.IDECARGO%TYPE);
  
  PROCEDURE SP_OBTENER_CARGO(p_ideSolicitud  IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                             p_ideUsuario    IN CARGO.USRCREACION%TYPE,
                             p_cRetCursor    OUT SYS_REFCURSOR);  
  
 PROCEDURE SP_CONSULTAR_DATOS_AREA(p_ideArea  IN AREA.IDEAREA%TYPE,
                                  p_cRetCursor OUT SYS_REFCURSOR);
                                  
 PROCEDURE SP_OBTENER_COMPETENCIAREMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                      p_cRetCursor OUT SYS_REFCURSOR);  
                                      
 PROCEDURE SP_OBTENER_OFRECIMIENTO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                        p_cRetCursor OUT SYS_REFCURSOR);
                                       
 PROCEDURE SP_OBTENER_HORARIO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                   p_cRetCursor OUT SYS_REFCURSOR);

 PROCEDURE SP_OBTENER_UBIGEO_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetCursor OUT SYS_REFCURSOR);
                                  
 PROCEDURE SP_OBTENER_NIVELACAD_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                     p_cRetCursor OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_CENT_EST_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                    p_cRetCursor OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_CONOCIMIENTO_REMP(p_ideSolicitud     IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                        p_tipoConocimiento IN VARCHAR2,
                                        p_cRetCursor OUT SYS_REFCURSOR);
                                        
 PROCEDURE SP_OBTENER_EXPR_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_cRetCursor OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_EVAL_REMP(p_ideSolicitud IN  SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                p_cRetCursor   OUT SYS_REFCURSOR);
                                    
 PROCEDURE SP_OBTENER_DISCAP_REMP(p_ideSolicitud  IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                                  p_cRetCursor    OUT SYS_REFCURSOR);
                           
 FUNCTION FN_GET_ROL(p_idUsuario IN NUMBER
                    )RETURN VARCHAR2;
                    
FUNCTION FN_GET_SEDE(p_idUsuario IN NUMBER
                    )RETURN VARCHAR2;
                    
PROCEDURE FN_GET_ROLXUSUARIO(
          p_nIdUsua IN NUMBER,
           p_cRetVal OUT CUR_CURSOR
          );                                                                         
          
PROCEDURE FN_GET_SEDEXUSUARIO(
           p_nIdUsua IN NUMBER,
           p_nIdRol IN NUMBER,
           p_cRetVal OUT CUR_CURSOR
          );   
          
PROCEDURE FN_GET_OPCIONESxROL(p_nIdRol IN NUMBER,
                              p_ctipMenu IN varchar2,            
                              p_cRetVal OUT CUR_CURSOR
          );
          
          
PROCEDURE FN_GET_OPCIONESPADRExROL(p_nIdRol IN NUMBER,
                                    p_ctipMenu IN varchar2,  
                                   p_cRetVal OUT CUR_CURSOR
          );          
          
             
FUNCTION FN_VALOR_GENERAL(p_tipoTabla IN GENERAL.TIPTABLA%TYPE,
                          p_valor     IN DETALLE_GENERAL.VALOR%TYPE)
                          RETURN DETALLE_GENERAL.DESCRIPCION%TYPE;  
                          
PROCEDURE FN_GET_CARGO(p_nIdCargo IN NUMBER,
                       p_cRetVal OUT CUR_CURSOR
          );
          
PROCEDURE FN_GET_LISTAREQ(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                          p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                          p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                          p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                          p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                          p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                          p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                          p_cFecIni   IN    varchar2,
                          p_cFeFin   IN     varchar2,
                          p_cRetVal         OUT CUR_CURSOR
          );
          
PROCEDURE FN_GET_LISTAREQ2(p_nIdCargo        IN SOLREQ_PERSONAL.IDECARGO%TYPE,
                          p_nIdDependencia  IN SOLREQ_PERSONAL.Idedependencia%TYPE,
                          p_nIdDepartamento IN SOLREQ_PERSONAL.Idedepartamento%TYPE,
                          p_nIdArea         in SOLREQ_PERSONAL.Idearea%TYPE,
                          p_cTipEtapa       IN LOGSOLREQ_PERSONAL.Tipetapa%TYPE,
                          p_cTipResp        in LOGSOLREQ_PERSONAL.ROLRESPONSABLE%TYPE,   
                          p_cEstado         IN SOLREQ_PERSONAL.Estactivo%TYPE,
                          p_cFecIni   IN    varchar2,
                          p_cFeFin   IN     varchar2,
                          p_cRetVal         OUT CUR_CURSOR
          );
          
FUNCTION FN_ESTADO_SOLICITUD(p_idSolicitud   IN SOLREQ_PERSONAL.IDESOLREQPERSONAL%TYPE,
                             p_tipoSolicitud IN VARCHAR2)RETURN VARCHAR2;
end PR_INTRANET;
/