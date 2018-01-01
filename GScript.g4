grammar GScript;

//////////////////////// PARSER /////////////////////////

dialog: line+;

line: expr (NEWLINE | EOF);

expr: '[' command '] ' args; 

args: TEXT; 

command: (
		'Say'
		| 'Goto'
		| 'Wait'
		| 'Chat'
		| 'Ask'
		| 'Opt'
		| 'Do'
	);
    
/* command: (
		say
		| gotu
		| wayt
		| chat
		| ask
		| opt
		| du
	);

say: 'Say';
gotu: 'Goto';
wayt : 'Wait';
chat: 'Chat';
ask: 'Ask';
opt: 'Opt';
du: 'Do'; */
// func: funcname LPAREN expression RPAREN

//////////////////////// LEXER /////////////////////////

fragment LOWERCASE: [a-z];
fragment UPPERCASE: [A-Z];
fragment DIGIT: [0-9];
fragment PUNCT: [:;",`'!.?-];

SPACE: (' ' | '\t');
COMMENT: '/*' .*? '*/' -> skip;
NEWLINE: ('\r'? '\n' | '\r')+;
TEXT: (LOWERCASE | UPPERCASE | PUNCT | DIGIT | SPACE)+;

//WORD: (LOWERCASE | UPPERCASE | PUNCT | DIGIT)+;
// LABEL: '[' (LOWERCASE | UPPERCASE | DIGIT | '_')+ ']';
// IDENT: (LOWERCASE | UPPERCASE | '_' | DIGIT);

