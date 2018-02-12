//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.6.4
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Dialogic.g4 by ANTLR 4.6.4

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Dialogic {
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.6.4")]
public partial class DialogicLexer : Lexer {
	public const int
		COMMAND=1, SPACE=2, DELIM=3, NEWLINE=4, WORD=5, COMMENT=6, LINE_COMMENT=7, 
		ERROR=8;
	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"COMMAND", "SPACE", "DELIM", "NEWLINE", "WORD", "COMMENT", "LINE_COMMENT", 
		"ERROR"
	};


	public DialogicLexer(ICharStream input)
		: base(input)
	{
		_interp = new LexerATNSimulator(this,_ATN);
	}

	private static readonly string[] _LiteralNames = {
	};
	private static readonly string[] _SymbolicNames = {
		null, "COMMAND", "SPACE", "DELIM", "NEWLINE", "WORD", "COMMENT", "LINE_COMMENT", 
		"ERROR"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[System.Obsolete("Use Vocabulary instead.")]
	public static readonly string[] tokenNames = GenerateTokenNames(DefaultVocabulary, _SymbolicNames.Length);

	private static string[] GenerateTokenNames(IVocabulary vocabulary, int length) {
		string[] tokenNames = new string[length];
		for (int i = 0; i < tokenNames.Length; i++) {
			tokenNames[i] = vocabulary.GetLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = vocabulary.GetSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}

		return tokenNames;
	}

	[System.Obsolete("Use IRecognizer.Vocabulary instead.")]
	public override string[] TokenNames
	{
		get
		{
			return tokenNames;
		}
	}

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Dialogic.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override string SerializedAtn { get { return _serializedATN; } }

	public static readonly string _serializedATN =
		"\x3\xAF6F\x8320\x479D\xB75C\x4880\x1605\x191C\xAB37\x2\nq\b\x1\x4\x2\t"+
		"\x2\x4\x3\t\x3\x4\x4\t\x4\x4\x5\t\x5\x4\x6\t\x6\x4\a\t\a\x4\b\t\b\x4\t"+
		"\t\t\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2"+
		"\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3"+
		"\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x3\x2\x5\x2"+
		"\x35\n\x2\x3\x3\x3\x3\x3\x4\a\x4:\n\x4\f\x4\xE\x4=\v\x4\x3\x4\x3\x4\a"+
		"\x4\x41\n\x4\f\x4\xE\x4\x44\v\x4\x3\x5\x5\x5G\n\x5\x3\x5\x3\x5\x6\x5K"+
		"\n\x5\r\x5\xE\x5L\x3\x6\x6\x6P\n\x6\r\x6\xE\x6Q\x3\a\x3\a\x3\a\x3\a\x3"+
		"\a\a\aY\n\a\f\a\xE\a\\\v\a\x3\a\x3\a\x3\a\x3\a\x3\a\x3\b\x3\b\x3\b\x3"+
		"\b\a\bg\n\b\f\b\xE\bj\v\b\x3\b\x3\b\x3\b\x3\b\x3\t\x3\t\x4Zh\x2\x2\n\x3"+
		"\x2\x3\x5\x2\x4\a\x2\x5\t\x2\x6\v\x2\a\r\x2\b\xF\x2\t\x11\x2\n\x3\x2\x4"+
		"\x4\x2\v\v\"\"\f\x2#$&\')+.\x30\x32=??\x41\x41\x43\\\x63|~~\x82\x2\x3"+
		"\x3\x2\x2\x2\x2\x5\x3\x2\x2\x2\x2\a\x3\x2\x2\x2\x2\t\x3\x2\x2\x2\x2\v"+
		"\x3\x2\x2\x2\x2\r\x3\x2\x2\x2\x2\xF\x3\x2\x2\x2\x2\x11\x3\x2\x2\x2\x3"+
		"\x34\x3\x2\x2\x2\x5\x36\x3\x2\x2\x2\a;\x3\x2\x2\x2\tJ\x3\x2\x2\x2\vO\x3"+
		"\x2\x2\x2\rS\x3\x2\x2\x2\xF\x62\x3\x2\x2\x2\x11o\x3\x2\x2\x2\x13\x14\a"+
		"\x45\x2\x2\x14\x15\aJ\x2\x2\x15\x16\a\x43\x2\x2\x16\x35\aV\x2\x2\x17\x18"+
		"\aU\x2\x2\x18\x19\a\x43\x2\x2\x19\x35\a[\x2\x2\x1A\x1B\aY\x2\x2\x1B\x1C"+
		"\a\x43\x2\x2\x1C\x1D\aK\x2\x2\x1D\x35\aV\x2\x2\x1E\x1F\a\x46\x2\x2\x1F"+
		"\x35\aQ\x2\x2 !\a\x43\x2\x2!\"\aU\x2\x2\"\x35\aM\x2\x2#$\aQ\x2\x2$%\a"+
		"R\x2\x2%\x35\aV\x2\x2&\'\aI\x2\x2\'\x35\aQ\x2\x2()\aO\x2\x2)*\aG\x2\x2"+
		"*+\aV\x2\x2+\x35\a\x43\x2\x2,-\a\x45\x2\x2-.\aQ\x2\x2./\aP\x2\x2/\x35"+
		"\a\x46\x2\x2\x30\x31\aH\x2\x2\x31\x32\aK\x2\x2\x32\x33\aP\x2\x2\x33\x35"+
		"\a\x46\x2\x2\x34\x13\x3\x2\x2\x2\x34\x17\x3\x2\x2\x2\x34\x1A\x3\x2\x2"+
		"\x2\x34\x1E\x3\x2\x2\x2\x34 \x3\x2\x2\x2\x34#\x3\x2\x2\x2\x34&\x3\x2\x2"+
		"\x2\x34(\x3\x2\x2\x2\x34,\x3\x2\x2\x2\x34\x30\x3\x2\x2\x2\x35\x4\x3\x2"+
		"\x2\x2\x36\x37\t\x2\x2\x2\x37\x6\x3\x2\x2\x2\x38:\x5\x5\x3\x2\x39\x38"+
		"\x3\x2\x2\x2:=\x3\x2\x2\x2;\x39\x3\x2\x2\x2;<\x3\x2\x2\x2<>\x3\x2\x2\x2"+
		"=;\x3\x2\x2\x2>\x42\a%\x2\x2?\x41\x5\x5\x3\x2@?\x3\x2\x2\x2\x41\x44\x3"+
		"\x2\x2\x2\x42@\x3\x2\x2\x2\x42\x43\x3\x2\x2\x2\x43\b\x3\x2\x2\x2\x44\x42"+
		"\x3\x2\x2\x2\x45G\a\xF\x2\x2\x46\x45\x3\x2\x2\x2\x46G\x3\x2\x2\x2GH\x3"+
		"\x2\x2\x2HK\a\f\x2\x2IK\a\xF\x2\x2J\x46\x3\x2\x2\x2JI\x3\x2\x2\x2KL\x3"+
		"\x2\x2\x2LJ\x3\x2\x2\x2LM\x3\x2\x2\x2M\n\x3\x2\x2\x2NP\t\x3\x2\x2ON\x3"+
		"\x2\x2\x2PQ\x3\x2\x2\x2QO\x3\x2\x2\x2QR\x3\x2\x2\x2R\f\x3\x2\x2\x2ST\a"+
		"\x31\x2\x2TU\a,\x2\x2UZ\x3\x2\x2\x2VY\x5\r\a\x2WY\v\x2\x2\x2XV\x3\x2\x2"+
		"\x2XW\x3\x2\x2\x2Y\\\x3\x2\x2\x2Z[\x3\x2\x2\x2ZX\x3\x2\x2\x2[]\x3\x2\x2"+
		"\x2\\Z\x3\x2\x2\x2]^\a,\x2\x2^_\a\x31\x2\x2_`\x3\x2\x2\x2`\x61\b\a\x2"+
		"\x2\x61\xE\x3\x2\x2\x2\x62\x63\a\x31\x2\x2\x63\x64\a\x31\x2\x2\x64h\x3"+
		"\x2\x2\x2\x65g\v\x2\x2\x2\x66\x65\x3\x2\x2\x2gj\x3\x2\x2\x2hi\x3\x2\x2"+
		"\x2h\x66\x3\x2\x2\x2ik\x3\x2\x2\x2jh\x3\x2\x2\x2kl\a\f\x2\x2lm\x3\x2\x2"+
		"\x2mn\b\b\x2\x2n\x10\x3\x2\x2\x2op\v\x2\x2\x2p\x12\x3\x2\x2\x2\r\x2\x34"+
		";\x42\x46JLQXZh\x3\b\x2\x2";
	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN.ToCharArray());
}
} // namespace Dialogic
