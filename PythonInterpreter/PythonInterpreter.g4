grammar PythonInterpreter;

// parser

program : function_definition* statement
        ;

function_definition : function_signature function_body
                    ;

function_signature : 'def' ID '(' parameters ')' ':'
                   ;

parameters : ID ',' parameters
           | ID?
           ;
function_body : statement_list 'end'
              | statement_list 'return' statement 'end'
              ;

statement_list : statement NEW_LINE statement_list
		       | statement
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

function_call_statement : ID '(' arguments ')'
                        ;

arguments : LITERAL ',' arguments
          | LITERAL?
          ;


if_statement : 'if' condition ':' statement_list 'end'
             | 'if' condition ':' statement_list 'else' if_statement
             ;

condition : expression COMPARISON_OPERATOR expression
          ;

math_statement : NUMBER '+' math_statement
               | NUMBER '-' math_statement
               | NUMBER
               ;

//lexer
NEW_LINE : '\n'
         ;
ID : [A-Za-z_][A-Za-z_0-9]*
   ;

INT : [0-9]+
    ;

DECIMAL : [0-9]+.[0-9]
      ;

STR : '"' .*? '"'
    ;

LITERAL : (INT | STR | ID)
                 ;

NUMBER : (INT | DECIMAL)
                ;

COMPARISON_OPERATOR :  '=='
                    |  '<='
                    |  '>='
                    |  '>'
                    |  '<'
                    |  '!='
                    ; 