grammar PythonInterpreter;

// parser


program : ((function_definition | statement) NEW_LINE)+ EOF
        ;

function_definition : function_signature function_body
                    ;

function_signature : 'def' ID '(' parameters ')' ':'
                   ;

parameters : ID (',' ID)*
           ;

arguments : (INT | STR | ID) ? (',' (INT | STR | ID)?)*
          ;

function_body : statement_list (RETURN statement)? 'end'
              ;

statement_list : statement (NEW_LINE statement)* 
               ;

statement : assignment_statement
	      | if_statement
          | function_call_statement
          | math_statement
          ;


assignment_statement : ID '=' expression
                     ;

expression: function_call_statement
            | INT
            | STR
            | ID
            | increment
            | decrement
            ;

increment : '++' INT
          | INT '++'
          ;

decrement : '--' INT
          | INT '--'
          ;

function_call_statement : library_func
                        | ID '(' arguments ')'
                        ;




if_statement : 'if' WS* condition WS* ':' WS* statement_list WS* (ELSE statement_list)? WS* END
             ;

condition : expression (COMPARISON_OPERATOR expression)?
          ;

math_statement : INT '+' math_statement
               | INT '-' math_statement
               | INT
               ;


// library functions
library_func : print_func
             | max_func
             | min_func
             | range_func
             ;

print_func : 'print' '(' arguments ')'
           ;

max_func : 'max' '(' arguments ')'
           ;

min_func : 'min' '(' arguments ')'
           ;

range_func : 'range' '(' INT ',' INT ')'
           ;

//lexer
ELSE : 'else'
     ;

END : 'end'
    ;

RETURN : 'return'
       ;

COMPARISON_OPERATOR :  '=='
                    |  '<='
                    |  '>='
                    |  '>'
                    |  '<'
                    |  '!='
                    ; 

NEW_LINE : '\r'?'\n' 
         ;
         
ID : [a-zA-Z_] [a-zA-Z_0-9]*
   ;

INT : [0-9]+(.[0-9])*
    ;


STR : '"' .*? '"'
    ;

WS : ' ' -> skip;