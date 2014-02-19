CREATE OR REPLACE package CHSPRP.PR_INTRANET is

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
  
  PROCEDURE SP_OBTENER_CARGO(p_ideSolicitud  IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                             p_cRetCursor    OUT SYS_REFCURSOR);  
  
  PROCEDURE SP_CONSULTAR_DATOS_AREA(p_ideArea  IN AREA.IDEAREA%TYPE,
                                  p_cRetCursor OUT SYS_REFCURSOR);          
                           
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
                              p_cRetVal OUT CUR_CURSOR
          );
          
          
PROCEDURE FN_GET_OPCIONESPADRExROL(p_nIdRol IN NUMBER,
                                   p_cRetVal OUT CUR_CURSOR
          );                      

end PR_INTRANET;
/
