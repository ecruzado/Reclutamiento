CREATE OR REPLACE package body CHSPRP.PR_INTRANET is

 /* ------------------------------------------------------------
    Nombre      : SP_LISTA_LVAL
    Proposito   : devuelve una lista de acuerdo al valor del tipo del lval
    Referencias : Sistema de Reclutamiento y Selecci?n de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/01/2014  Edward Llamoca       Creaci?n    
  ------------------------------------------------------------ */

FUNCTION SP_LISTA_LVAL(p_idGeneral IN VARCHAR2,
                       p_valor     IN VARCHAR2) return VARCHAR2
IS

cDato varchar2(1000);
BEGIN                                                                                                                                                                                                                                                                                          
  
    BEGIN
      SELECT G.DESCRIPCION AS DESCRIPCION
      INTO cDato
      FROM CHSPRP.DETALLE_GENERAL  G 
      WHERE G.IDEGENERAL=P_IDGENERAL
      AND G.VALOR = P_VALOR;
    EXCEPTION
    WHEN OTHERS THEN
     cDato :=null;
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

FUNCTION FN_GETMAXPUNTAJE(p_nIdCriterio IN NUMBER)RETURN NUMBER
IS
 nMaxPuntaje NUMBER;
BEGIN

  BEGIN
   
    SELECT MAX(A.PESO) AS MAXPUNTAJE
     INTO nMaxPuntaje
    FROM ALTERNATIVA A 
    WHERE A.IDECRITERIO=P_NIDCRITERIO;
  
  EXCEPTION
  WHEN OTHERS THEN
    nMaxPuntaje:=0; 
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
                          p_cRetVal OUT CUR_CURSOR )
IS
 
 
BEGIN
 
  OPEN p_cRetVal FOR
    SELECT EX1.IDEEXAMEN,EX1.IDESEDE,EX1.NOMEXAMEN,EX1.DESCEXAMEN,S1.IDESUBCATEGORIA,
         C1.IDECATEGORIA,C1.NOMCATEGORIA,
         C1.DESCCATEGORIA,C1.TIPCATEGORIA,C1.INSTRUCCIONES,C1.TIPOEJEMPLO,
         C1.IMAGENEJEMPLO,C1.TEXTOEJEMPLO,
         S1.DESCSUBCATEGORIA,S1.NOMSUBCATEGORIA,
         A1.IDECRITERIO,A1.IDEALTERNATIVA,CR1.TIPMODO AS CODMOD,
         DECODE(CR1.TIPMODO,'01','TEXTO','02','IMAGE') AS DESMODO,
         CR1.TIPCRITERIO,
         CR1.PREGUNTA,CR1.IMAGENCRIT,A1.IMAGE,A1.ALTERNATIVA,A1.ESTACTIVO,
         s1.ordenimpresion as ORDENSUB,CS1.PRIORIDAD AS ORDENCRIT,
         (SELECT NVL(SUM(S.TIEMPO),0) 
          FROM   SUBCATEGORIA S
          WHERE  S.IDECATEGORIA= C1.IDECATEGORIA) AS TIEMPOCAT,
          PR_INTRANET.FN_DURACIONEXAMEN(ex1.ideexamen) AS TIMPOEXAMEN
    FROM CHSPRP.CRITERIO CR1,ALTERNATIVA A1,CRITERIO_X_SUBCATEGORIA CS1,
         SUBCATEGORIA S1,CATEGORIA C1, EXAMEN_X_CATEGORIA EC1,Examen EX1
    WHERE
    ex1.ideexamen = EC1.Ideexamen
    and EC1.Idecategoria = c1.idecategoria
    and C1.IDECATEGORIA = S1.IDECATEGORIA
    AND C1.TIPCATEGORIA = CR1.TIPCRITERIO  
    AND CS1.IDESUBCATEGORIA=S1.IDESUBCATEGORIA
    AND CR1.IDECRITERIO=CS1.IDECRITERIO
    AND A1.IDECRITERIO=CR1.IDECRITERIO
    and c1.estactivo='A'
    AND A1.ESTACTIVO='A'
    AND CR1.ESTACTIVO='A'
    AND CS1.ESTREGISTRO='A'
    and ec1.estactivo='A'
    and EX1.Estactivo='A'
    and ex1.ideexamen=P_NIDEXAMEN
    ORDER BY c1.idecategoria,s1.ordenimpresion,
    CS1.Prioridad,A1.IDEALTERNATIVA DESC;
 
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
FUNCTION FN_DURACIONEXAMEN(p_nIdExamen IN NUMBER)RETURN NUMBER
IS

  nCont Number;
  nTiempo Number;
  CURSOR cData IS
  select c.idecategoria from examen_x_categoria c 
  where c.ideexamen = p_nIdExamen;

BEGIN
  nCont:=0;
  nTiempo:=0;
  
  FOR C1 IN cData LOOP
    BEGIN 
      SELECT NVL(SUM(S.TIEMPO),0) 
      INTO nTiempo
      FROM   SUBCATEGORIA S
      WHERE  S.IDECATEGORIA= C1.IDECATEGORIA;
    EXCEPTION
    WHEN OTHERS THEN
      nTiempo:=0;
    END;
    
    nCont := nCont+nTiempo;
    
  END LOOP;

  RETURN nvl(nCont,0);

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
FUNCTION FN_ELIMINA_ROL(p_nIdRol IN NUMBER)RETURN NUMBER
IS
nDato NUMBER;
BEGIN


  delete from ROLOPCION where idrol=p_nIdRol;
  delete from rol r where r.idrol=p_nIdRol;
  commit;
  
  nDato :=1;
  
  
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
FUNCTION FN_ELIMINA_ROL_OPCION(p_nIdRol IN NUMBER,
                               p_nIdOp IN NUMBER)RETURN NUMBER
IS
nDato NUMBER;
BEGIN

  delete from ROLOPCION r 
  where r.idrol=p_nIdRol 
  and r.idopcion=p_nIdOp;
  commit;
  
  nDato :=1;
  
  
  RETURN nDato;

END FN_ELIMINA_ROL_OPCION;

/* ------------------------------------------------------------
    Nombre      : SP_ACTUALIZAR_PUNTAJES
    Proposito   : Actualizar los puntajes en la tabla cargo 
                  al modificar tablas asociadas
    Referencias : Sistema de Reclutamiento y Selección de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      13/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_ACTUALIZAR_PUNTAJES(p_nombreCampoDestino IN VARCHAR2,
                                 p_ideCargo     IN CARGO.IDECARGO%TYPE, 
                                 p_valor       IN NUMBER,
                                 p_valorEliminar IN NUMBER)IS

consulta01 VARCHAR2(100);
consulta02 VARCHAR2(100);                                 

BEGIN

consulta01 := 'SELECT '||p_nombreCampoDestino ||
              ' FROM Cargo FOR UPDATE';
consulta02 := 'UPDATE CARGO SET '||p_nombreCampoDestino ||' = '
                                 ||p_nombreCampoDestino||' + '
                                 ||p_valor ||' - '||p_valorEliminar;

EXECUTE IMMEDIATE consulta01;

EXECUTE IMMEDIATE consulta02;

commit;

END SP_ACTUALIZAR_PUNTAJES;   

/* ------------------------------------------------------------
    Nombre      : SP_CREAR_CARGO
    Proposito   : Verificar si el cargo esta creado  o crear el
                  registro y recuperar datos.
    Referencias : Sistema de Reclutamiento y Selección de Personal
    Parametros  :               
                                  
    Log de Cambios
      Fecha       Autor                Descripcion
      14/02/2014  Jaqueline Ccana       Creaci?n    
  ------------------------------------------------------------ */
PROCEDURE SP_CREAR_CARGO(p_ideSolicitud  IN SOLNUEVO_CARGO.IDESOLNUEVOCARGO%TYPE,
                         p_cRetCursor    OUT SYS_REFCURSOR)IS

CURSOR CR_CARGO( p_cCargo CARGO.IDECARGO%TYPE) IS
   SELECT   C.IDECARGO, C.CODCARGO, C.NOMCARGO, C.DESCARGO, 
   FROM  CARGO C
   WHERE C.CODCARGO = nCodCargo;
   
nCodCargo CARGO.CODCARGO%TYPE;
nIdeCargo CARGO.IDECARGO%TYPE;
nNomCargo CARGO.NOMCARGO%TYPE;
nDescCargo CARGO.DESCARGO%TYPE;
nIdeArea  CARGO.IDEAREA%TYPE;
BEGIN

   SELECT SN.CODCARGO, SN.NOMBRE, SN.DESCRIPCION, SN.IDEAREA 
   INTO nCodCargo, nNomCargo, nDescCargo, nIdeArea
   FROM SOLNUEVO_CARGO SN
   WHERE SN.IDESOLNUEVOCARGO = p_ideSolicitud;
   
   SELECT C.IDECARGO INTO nIdeCargo
   FROM CARGO C
   WHERE C.CODCARGO = nCodCargo;
   
   IF (nIdeCargo IS NOT NULL)
   INSERT CARGO INTO (CODCARGO,NOMCARGO,DESCARGO,IDEAREA)  VALUE (nCodCargo,nNomCargo,nDescCargo,nIdeArea);
   
        

  
EXCEPTION   
    WHEN NO_DATA_FOUND THEN
    RETURN ''  ;

END SP_CREAR_CARGO;                          

END PR_INTRANET;
/
