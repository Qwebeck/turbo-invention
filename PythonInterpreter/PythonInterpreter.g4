grammar PythonInterpreter;
 
equation: INT PLUS INT;

PLUS: '+';

INT: [0-9]+;

WS: [\t\r\n]+ -> skip;