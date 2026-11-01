using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Verdant.Automation
{
    // The parser and runtime are independent project code, with no access to the OS or CLR.
    public sealed class ScriptParser
    {
        sealed class Row {public string text;public int indent,line;}
        List<Row> rows;int at,loops,functions,depth;
        public Block Parse(string source)
        {
            if(source==null||source.Length>200000)throw new ScriptError(1,"Source exceeds 200000 characters.");
            rows=new List<Row>();at=loops=functions=depth=0;var lines=source.Replace("\r","").Replace("\t","    ").Split('\n');
            for(int i=0;i<lines.Length;i++){string line=Comment(lines[i]);if(string.IsNullOrWhiteSpace(line))continue;int indent=line.Length-line.TrimStart(' ').Length;if(indent%4!=0)throw new ScriptError(i+1,"Use indentation in groups of four spaces.");rows.Add(new Row{text=line.Trim(),indent=indent,line=i+1});}
            if(rows.Count==0)throw new ScriptError(1,"Enter a program first.");var result=ReadBlock(0);if(at!=rows.Count)throw new ScriptError(rows[at].line,"Unexpected indentation.");return result;
        }
        static string Comment(string source){char quote='\0';for(int i=0;i<source.Length;i++){if(source[i]=='\\'&&quote!='\0'){i++;continue;}if(source[i]=='\''||source[i]=='"'){if(quote=='\0')quote=source[i];else if(quote==source[i])quote='\0';}if(source[i]=='#'&&quote=='\0')return source.Substring(0,i);}return source;}
        Block Body(Row row){if(at>=rows.Count||rows[at].indent!=row.indent+4)throw new ScriptError(row.line,"Expected an indented body.");return ReadBlock(row.indent+4);}
        Expression Expr(string s,int line){try{return new ExpressionParser(s,line).Parse();}catch(Exception e){if(e is ScriptError)throw;throw new ScriptError(line,e.Message);}}
        Block ReadBlock(int indent)
        {
            if(++depth>48)throw new ScriptError(rows[Math.Min(at,rows.Count-1)].line,"Block nesting limit.");var block=new Block();
            while(at<rows.Count&&rows[at].indent>=indent)
            {
                var row=rows[at++];if(row.indent!=indent)throw new ScriptError(row.line,"Unexpected indentation.");string s=row.text;int l=row.line;
                if(s.StartsWith("if "))
                {
                    var node=new Conditional(l);node.tests.Add(Expr(Condition(row,3),l));node.branches.Add(Body(row));
                    while(at<rows.Count&&rows[at].indent==indent&&rows[at].text.StartsWith("elif ")){var other=rows[at++];node.tests.Add(Expr(Condition(other,5),other.line));node.branches.Add(Body(other));}
                    if(at<rows.Count&&rows[at].indent==indent&&rows[at].text=="else:")node.otherwise=Body(rows[at++]);block.statements.Add(node);continue;
                }
                if(s.StartsWith("while ")){var test=Expr(Condition(row,6),l);loops++;var body=Body(row);loops--;block.statements.Add(new LoopStatement(l,test,body));continue;}
                if(s.StartsWith("for ")||s.StartsWith("repeat "))
                {
                    string target,seq;if(s.StartsWith("repeat ")){target="_repeat_"+l;seq="range("+Condition(row,7)+")";}else {var match=Regex.Match(s,"^for (.+?) in (.+):$");if(!match.Success)throw new ScriptError(l,"Expected for target in sequence:");target=match.Groups[1].Value;seq=match.Groups[2].Value;}
                    loops++;var body=Body(row);loops--;block.statements.Add(new LoopStatement(l,Expr(target,l),Expr(seq,l),body));continue;
                }
                if(s.StartsWith("def "))
                {
                    var match=Regex.Match(s,"^def ([A-Za-z_]\\w*)\\((.*)\\):$");if(!match.Success)throw new ScriptError(l,"Expected def name(parameters):");var parameters=new List<string>();var defaults=new List<Expression>();bool optional=false;
                    foreach(var arg in Split(match.Groups[2].Value))
                    {int equal=AssignmentAt(arg);string name=equal<0?arg.Trim():arg.Substring(0,equal).Trim();if(!Regex.IsMatch(name,"^[A-Za-z_]\\w*$")||parameters.Contains(name))throw new ScriptError(l,"Invalid or duplicate parameter.");parameters.Add(name);if(equal>=0){optional=true;defaults.Add(Expr(arg.Substring(equal+1),l));}else if(optional)throw new ScriptError(l,"Required parameter after default parameter.");}
                    int previous=loops;loops=0;functions++;var body=Body(row);functions--;loops=previous;block.statements.Add(new FunctionStatement(l,match.Groups[1].Value,parameters.ToArray(),defaults.ToArray(),body));continue;
                }
                if(s=="return"||s.StartsWith("return ")){if(functions==0)throw new ScriptError(l,"return outside a function.");block.statements.Add(new FlowStatement(l,"return",s=="return"?null:Expr(s.Substring(7),l)));continue;}
                if(s=="break"||s=="continue"){if(loops==0)throw new ScriptError(l,s+" outside a loop.");block.statements.Add(new FlowStatement(l,s));continue;}
                if(s.StartsWith("global ")){var names=Split(s.Substring(7));if(names.Any(n=>!Regex.IsMatch(n,"^[A-Za-z_]\\w*$")))throw new ScriptError(l,"Invalid global name.");block.statements.Add(new GlobalStatement(l,names.ToArray()));continue;}
                if(s.StartsWith("import ")){string name=s.Substring(7).Trim();if(!Regex.IsMatch(name,"^[A-Za-z_]\\w*$"))throw new ScriptError(l,"Import a single named game module.");block.statements.Add(new ImportStatement(l,name,null));continue;}
                if(s.StartsWith("from ")){var m=Regex.Match(s,"^from ([A-Za-z_]\\w*) import (.+)$");if(!m.Success)throw new ScriptError(l,"Invalid import.");block.statements.Add(new ImportStatement(l,m.Groups[1].Value,Split(m.Groups[2].Value).ToArray()));continue;}
                if(s=="pass"){block.statements.Add(new ExprStatement(l,new Literal(l,null)));continue;}
                int assignment=AssignmentAt(s);if(assignment>=0){string left=s.Substring(0,assignment).Trim(),right=s.Substring(assignment+1).Trim();string op=left.Length>0?left.Substring(left.Length-1):"";if("+-*/%".Contains(op)&&op!=""){left=left.Substring(0,left.Length-1).Trim();block.statements.Add(new Assignment(l,Expr(left,l),new Binary(l,op,Expr(left,l),Expr(right,l))));}else block.statements.Add(new Assignment(l,Expr(left,l),Expr(right,l)));}
                else block.statements.Add(new ExprStatement(l,Expr(s,l)));
            }
            depth--;return block;
        }
        static string Condition(Row r,int skip){if(!r.text.EndsWith(":"))throw new ScriptError(r.line,"Expected ':' at the end of the statement.");return r.text.Substring(skip,r.text.Length-skip-1);}
        static int AssignmentAt(string s){int level=0;char quote='\0';for(int i=0;i<s.Length;i++){char c=s[i];if(quote!='\0'){if(c=='\\')i++;else if(c==quote)quote='\0';continue;}if(c=='\''||c=='"'){quote=c;continue;}if("([{ ".Contains(c)&&c!=' ')level++;else if(")]}".Contains(c))level--;if(c=='='&&level==0&&(i==0||"=!<>".IndexOf(s[i-1])<0)&&(i+1==s.Length||s[i+1]!='='))return i;}return -1;}
        static List<string> Split(string s){var result=new List<string>();int start=0,depth=0;char quote='\0';for(int i=0;i<s.Length;i++){char c=s[i];if(quote!='\0'){if(c=='\\')i++;else if(c==quote)quote='\0';continue;}if(c=='\''||c=='"')quote=c;else if("([{".Contains(c))depth++;else if(")]}".Contains(c))depth--;else if(c==','&&depth==0){result.Add(s.Substring(start,i-start).Trim());start=i+1;}}if(s.Substring(start).Trim()!="")result.Add(s.Substring(start).Trim());return result;}
    }
    sealed class ExpressionParser
    {
        sealed class Token {public string text;public object value;public bool literal;public Token(string s,object v=null,bool lit=false){text=s;value=v;literal=lit;}}
        List<Token> tokens=new List<Token>();int at,line,depth;
        public ExpressionParser(string source,int line)
        {
            this.line=line;
            for(int i=0;i<source.Length;)
            {
                char c=source[i];if(char.IsWhiteSpace(c)){i++;continue;}
                if(c=='\''||c=='"'){char quote=c;i++;var b=new StringBuilder();while(i<source.Length&&source[i]!=quote){if(source[i]=='\\'&&i+1<source.Length){i++;char escape=source[i++];b.Append(escape=='n'?'\n':escape=='t'?'\t':escape=='r'?'\r':escape);}else b.Append(source[i++]);}if(i>=source.Length)throw new ScriptError(line,"Unterminated string.");i++;tokens.Add(new Token("string",b.ToString(),true));continue;}
                if(char.IsDigit(c)||c=='.'&&i+1<source.Length&&char.IsDigit(source[i+1])){int start=i++;while(i<source.Length&&(char.IsDigit(source[i])||source[i]=='.'))i++;double n;if(!double.TryParse(source.Substring(start,i-start),NumberStyles.Float,CultureInfo.InvariantCulture,out n))throw new ScriptError(line,"Invalid number.");tokens.Add(new Token("number",n,true));continue;}
                if(char.IsLetter(c)||c=='_'){int start=i++;while(i<source.Length&&(char.IsLetterOrDigit(source[i])||source[i]=='_'))i++;tokens.Add(new Token(source.Substring(start,i-start)));continue;}
                string two=i+1<source.Length?source.Substring(i,2):"";if(new[]{"==","!=","<=",">=","//","**"}.Contains(two)){tokens.Add(new Token(two));i+=2;continue;}
                if("()[]{},:.+-*/%<>".IndexOf(c)<0)throw new ScriptError(line,"Unsupported character: "+c);tokens.Add(new Token(c.ToString()));i++;
            }
        }
        string Peek {get{return at<tokens.Count?tokens[at].text:"";}}
        bool Match(string s){if(Peek!=s)return false;at++;return true;}
        Token Take(){if(at>=tokens.Count)throw new ScriptError(line,"Incomplete expression.");return tokens[at++];}
        void Need(string s){if(!Match(s))throw new ScriptError(line,"Expected '"+s+"'.");}
        public Expression Parse(){if(tokens.Count==0)throw new ScriptError(line,"Missing expression.");var e=Tuple();if(at!=tokens.Count)throw new ScriptError(line,"Unexpected token: "+Peek);return e;}
        Expression Tuple(){var e=Or();if(!Match(","))return e;var items=new List<Expression>{e};while(Peek!=""&&Peek!=")"&&Peek!="]"){items.Add(Or());if(!Match(","))break;}return new Collection(line,"tuple",items);}
        Expression Or(){if(++depth>96)throw new ScriptError(line,"Expression nesting limit.");var e=And();while(Match("or"))e=new Binary(line,"or",e,And());if(Match("if")){var condition=Or();Need("else");e=new ConditionalValue(line,e,condition,Or());}depth--;return e;}
        Expression And(){var e=Not();while(Match("and"))e=new Binary(line,"and",e,Not());return e;}
        Expression Not(){if(++depth>96)throw new ScriptError(line,"Expression nesting limit.");Expression e=Match("not")?(Expression)new Unary(line,"not",Not()):Compare();depth--;return e;}
        Expression Compare(){var values=new List<Expression>{Sum()};var ops=new List<string>();while(new[]{"==","!=","<",">","<=",">=","in","is","not"}.Contains(Peek)){string op=Take().text;if(op=="not"){Need("in");op="not in";}else if(op=="is"&&Match("not"))op="is not";ops.Add(op);values.Add(Sum());}return ops.Count==0?values[0]:new CompareChain(line,values,ops);}

        Expression Sum(){var e=Product();while(Peek=="+"||Peek=="-"){string op=Take().text;e=new Binary(line,op,e,Product());}return e;}
        Expression Product(){var e=Unary();while(Peek=="*"||Peek=="/"||Peek=="//"||Peek=="%"){string op=Take().text;e=new Binary(line,op,e,Unary());}return e;}
        Expression Unary(){if(++depth>96)throw new ScriptError(line,"Expression nesting limit.");Expression e;if(Peek=="-"||Peek=="+"){string op=Take().text;e=new Unary(line,op,Unary());}else{e=Postfix();if(Match("**"))e=new Binary(line,"**",e,Unary());}depth--;return e;}

        Expression Postfix(){var e=Primary();while(true){if(Match("(")){var args=new List<Expression>();if(!Match(")")){do{args.Add(Or());}while(Match(",")&&Peek!=")");Need(")");}e=new Call(line,e,args);}else if(Match("[")){var key=Or();Need("]");e=new Index(line,e,key);}else if(Match(".")){string name=Take().text;if(!Regex.IsMatch(name,"^[A-Za-z_]\\w*$"))throw new ScriptError(line,"Invalid member name.");e=new Member(line,e,name);}else return e;}}
        Expression Primary()
        {
            var t=Take();if(t.literal)return new Literal(line,t.value);if(t.text=="("){if(Match(")"))return new Collection(line,"tuple",new List<Expression>());var e=Tuple();Need(")");return e;}
            if(t.text=="["){var items=new List<Expression>();if(!Match("]")){do{items.Add(Or());}while(Match(",")&&Peek!="]");Need("]");}return new Collection(line,"list",items);}
            if(t.text=="{"){var items=new List<Expression>();if(Match("}"))return new Collection(line,"dict",items);items.Add(Or());bool dict=Match(":");if(dict)items.Add(Or());while(Match(",")&&Peek!="}"){items.Add(Or());if(dict){Need(":");items.Add(Or());}}Need("}");return new Collection(line,dict?"dict":"set",items);}
            if(!Regex.IsMatch(t.text,"^[A-Za-z_]\\w*$"))throw new ScriptError(line,"Invalid expression.");return new Name(line,t.text);
        }
    }
}
