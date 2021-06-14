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

statement_list : (WS* statement WS* )+
               ;

statement : assignment_statement NEW_LINE?
	      | if_statement NEW_LINE?
          | function_call_statement NEW_LINE?
          | math_statement NEW_LINE?
          ;


assignment_statement : ID WS* '=' WS* expression WS*
                     ;

expression: function_call_statement
            | INT WS*
            | STR WS*
            | ID WS*
            | increment WS*
            | decrement WS*
            ;

increment : '++' INT
          | INT '++'
          ;

decrement : '--' INT
          | INT '--'
          ;

function_call_statement : library_func WS*
                        | ID LPAREN arguments RPAREN WS*
                        ;




if_statement :  IF condition':' NEW_LINE statement_list ((ELSE | ELSEIF condition) ':' NEW_LINE statement_list )? WS* END
             ;

condition : WS* expression (COMPARISON_OPERATOR expression)? WS* #ExpressionCondition
          | WS* NOT? (TRUE | FALSE) WS* #LogicalCondition
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
IF       : 'if'
         ;

ELSE     : 'else'
         ;
ELSEIF   : 'elif'
         ;

RETURN   : 'return'
         ;

END      : 'end'
         ;

TRUE    : 'True'
        ;

FALSE   : 'False'
        ;

NOT     : 'not'
        ;

NEW_LINE : ('\r'?'\n' | EOF) // -> skip
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