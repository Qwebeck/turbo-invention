grammar PythonInterpreter;

// parser


program : ((function_definition | statement) NEW_LINE+)+ EOF
        ;

function_definition : function_signature NEW_LINE function_body
                    ;

function_signature : DEF WS* ID LPAREN parameters RPAREN ':'
                   ;

parameters : ID (',' ID)*
           ;

arguments : (INT | STR | ID) ? (',' (INT | STR | ID)?)*
          ;

function_body : statement_list (RETURN statement)? 'end'
              ;

statement_list : (statement NEW_LINE)+
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
                        | ID LPAREN arguments RPAREN
                        ;




if_statement : 'if' WS* condition WS* ':' WS* statement_list WS* (ELSE statement_list)? WS* END
             ;

condition : expression (COMPARISON_OPERATOR expression)?
          ;

math_statement : math_statement WS* (TIMES | DIV) WS* math_statement #MultiplicationStatement
               | math_statement WS* (PLUS | MINUS) WS* math_statement #AdditionStatement
               | LPAREN WS* math_statement WS* RPAREN #ParenthesedStatemet
               | (PLUS | MINUS)* INT #Factor
               ;

// library functions
library_func : print_func
             | max_func
             | min_func
             | range_func
             ;

print_func : 'print' LPAREN arguments RPAREN
           ;

max_func : 'max' LPAREN arguments RPAREN
           ;

min_func : 'min' LPAREN arguments RPAREN
           ;

range_func : 'range' LPAREN INT ',' INT RPAREN
           ;

//lexer
DEF : 'def' 
    ;

COMPARISON_OPERATOR : '=='
                    | '<='
                    | '>='
                    | '!='
                    | '>'
                    | '<'
                    ;

ELSE     : 'else'
         ;

RETURN   : 'return'
         ;

END      : 'end'
         ;

NEW_LINE : '\r'?'\n' 
         ;

LPAREN   : '('
         ;

RPAREN   : ')'
         ;
         
ID : [a-zA-Z_] [a-zA-Z_0-9]*
   ;



PLUS : '+'
     ;

MINUS : '-'
      ;

TIMES : '*'
      ;

DIV   :  '/'
      ;

INT : [0-9]+
    ;



STR : '"' .*? '"'
    ;

WS : ' ' -> skip;