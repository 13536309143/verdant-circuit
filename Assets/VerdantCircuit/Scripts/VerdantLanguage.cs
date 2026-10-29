using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Verdant
{
    public sealed class FarmCodeException : Exception
    {public int Line;public FarmCodeException(int line,string message):base(L10n.T("第 ")+line+L10n.T(" 行：")+message){Line=line;}}
    public abstract class FarmExpr {public abstract object Eval(FarmVM vm);}
    sealed class LiteralExpr : FarmExpr {object value;public LiteralExpr(object v){value=v;}public override object Eval(FarmVM vm){return value;}}
    sealed class NameExpr : FarmExpr
    {
        string name; public NameExpr(string n){name=n;}
        public override object Eval(FarmVM vm)
        {
            if(name=="True"||name=="true")return true;if(name=="False"||name=="false")return false;
            if(name=="empty")return "";
            if(name=="East"||name=="West"||name=="North"||name=="South"||VerdantContent.Crop(name)!=null)return name;
            object value;if(vm.Variables.TryGetValue(name,out value))return value;
            if(vm.Globals.TryGetValue(name,out value))return value;
            throw new Exception(L10n.T("未定义变量 ")+name);
        }
    }
    sealed class UnaryExpr : FarmExpr
    {
        string op;FarmExpr expr;public UnaryExpr(string o,FarmExpr e){op=o;expr=e;}
        public override object Eval(FarmVM vm){object v=expr.Eval(vm);return op=="not"?(object)!FarmVM.Truth(v):op=="-"?-FarmVM.Number(v):FarmVM.Number(v);}
    }
    sealed class BinaryExpr : FarmExpr
    {
        string op;FarmExpr left,right;public BinaryExpr(string o,FarmExpr l,FarmExpr r){op=o;left=l;right=r;}
        public override object Eval(FarmVM vm)
        {
            object a=left.Eval(vm);if(op=="and")return FarmVM.Truth(a)&&FarmVM.Truth(right.Eval(vm));if(op=="or")return FarmVM.Truth(a)||FarmVM.Truth(right.Eval(vm));
            object b=right.Eval(vm);
            if(op=="==")return EqualsValue(a,b);if(op=="!=")return !EqualsValue(a,b);
            if(op=="+"&&(a is string||b is string))return Convert.ToString(a,CultureInfo.InvariantCulture)+Convert.ToString(b,CultureInfo.InvariantCulture);
            double x=FarmVM.Number(a),y=FarmVM.Number(b);
            switch(op){case "+":return x+y;case "-":return x-y;case "*":return x*y;case "/":if(y==0)throw new Exception(L10n.T("不能除以零"));return x/y;case "%":if(y==0)throw new Exception(L10n.T("不能除以零"));return x%y;case "<":return x<y;case ">":return x>y;case "<=":return x<=y;case ">=":return x>=y;}
            throw new Exception(L10n.T("未知运算符 ")+op);
        }
        static bool EqualsValue(object a,object b){if(a is double||b is double||a is int||b is int){try{return Math.Abs(FarmVM.Number(a)-FarmVM.Number(b))<0.000001;}catch{return false;}}return object.Equals(a,b);}
    }
    sealed class SensorExpr : FarmExpr
    {
        public string Name;public List<FarmExpr> Args;public SensorExpr(string n,List<FarmExpr> a){Name=n;Args=a;}
        public override object Eval(FarmVM vm)
        {
            var f=vm.Farm;var p=f.Here;
            if(VerdantFleet.IsSensor(Name)){var values=new object[Args.Count];for(int a=0;a<Args.Count;a++)values[a]=Args[a].Eval(vm);return f.Fleet.Sense(Name,values);}
            int expected=(Name=="count"||Name=="neighbors"||Name=="min"||Name=="max")?(Name=="min"||Name=="max"?2:1):Name=="range"?-1:0;
            if(expected>=0&&Args.Count!=expected)throw new Exception(Name+L10n.T("() 需要 ")+expected+L10n.T(" 个参数"));
            switch(Name)
            {
                case "ready":return f.Ready(p);case "crop":return p.crop;case "moisture":return (double)p.moisture;case "fertility":return (double)p.fertility;
                case "x":return (double)f.Data.x;case "y":return (double)f.Data.y;case "size":return (double)f.Data.size;
                case "energy":return (double)f.Data.energy;case "money":return (double)f.Data.coins;
                case "count":return (double)f.Count("stock:"+Args[0].Eval(vm));
                case "neighbors":return (double)f.Neighbors(Convert.ToString(Args[0].Eval(vm)));
                case "min":return Math.Min(FarmVM.Number(Args[0].Eval(vm)),FarmVM.Number(Args[1].Eval(vm)));
                case "max":return Math.Max(FarmVM.Number(Args[0].Eval(vm)),FarmVM.Number(Args[1].Eval(vm)));
                case "range":
                    if(Args.Count<1||Args.Count>3)throw new Exception(L10n.T("range() 需要 1~3 个参数"));
                    int start=Args.Count==1?0:Integer(Args[0].Eval(vm));int end=Integer(Args[Args.Count==1?0:1].Eval(vm));int step=Args.Count==3?Integer(Args[2].Eval(vm)):1;
                    if(step==0)throw new Exception(L10n.T("range() 步长不能为零"));
                    if(Math.Abs((double)end-start)/Math.Abs((double)step)>4096)throw new Exception(L10n.T("range() 单次循环上限为 4096"));
                    return new FarmRange(start,end,step);
                default:throw new Exception(L10n.T("未知传感器 ")+Name+"()");
            }
        }
        static int Integer(object o){double d=FarmVM.Number(o);if(double.IsInfinity(d)||double.IsNaN(d)||d<int.MinValue||d>int.MaxValue||d!=Math.Truncate(d))throw new Exception(L10n.T("range() 参数必须是合理的整数"));return (int)d;}
    }
    sealed class FarmRange
    {
        public int value,end,step;bool first=true;
        public FarmRange(int a,int b,int c){value=a;end=b;step=c;}
        public bool Next(){if(!first)value+=step;first=false;return step>0?value<end:value>end;}
    }
    sealed class ExprParser
    {
        List<string> tokens=new List<string>();int at;
        public ExprParser(string source)
        {
            for(int i=0;i<source.Length;)
            {
                char c=source[i];if(char.IsWhiteSpace(c)){i++;continue;}
                if(c=='\''||c=='"'){char quote=c;var b=new StringBuilder();i++;while(i<source.Length&&source[i]!=quote){if(source[i]=='\\'&&i+1<source.Length)i++;b.Append(source[i++]);}if(i>=source.Length)throw new Exception(L10n.T("字符串缺少结束引号"));i++;tokens.Add("\""+b);continue;}
                if(char.IsDigit(c)||c=='.'&&i+1<source.Length&&char.IsDigit(source[i+1])){int j=i++;while(i<source.Length&&(char.IsDigit(source[i])||source[i]=='.'))i++;tokens.Add(source.Substring(j,i-j));continue;}
                if(char.IsLetter(c)||c=='_'){int j=i++;while(i<source.Length&&(char.IsLetterOrDigit(source[i])||source[i]=='_'))i++;tokens.Add(source.Substring(j,i-j));continue;}
                if(i+1<source.Length&&(source.Substring(i,2)=="=="||source.Substring(i,2)=="!="||source.Substring(i,2)=="<="||source.Substring(i,2)==">=")){tokens.Add(source.Substring(i,2));i+=2;continue;}
                if("()+-*/%,<>".IndexOf(c)>=0){tokens.Add(c.ToString());i++;continue;}
                throw new Exception(L10n.T("不支持的字符：")+c);
            }
        }
        string Peek{get{return at<tokens.Count?tokens[at]:"";}}
        string Take(){if(at>=tokens.Count)throw new Exception(L10n.T("表达式不完整"));return tokens[at++];}
        bool Match(string s){if(Peek!=s)return false;at++;return true;}
        public FarmExpr Parse(){if(tokens.Count==0)throw new Exception(L10n.T("缺少表达式"));var e=Or();if(at!=tokens.Count)throw new Exception(L10n.T("无法识别：")+Peek);return e;}
        FarmExpr Or(){var e=And();while(Match("or"))e=new BinaryExpr("or",e,And());return e;}
        FarmExpr And(){var e=Compare();while(Match("and"))e=new BinaryExpr("and",e,Compare());return e;}
        FarmExpr Compare(){var e=Sum();while(Peek=="=="||Peek=="!="||Peek=="<"||Peek==">"||Peek=="<="||Peek==">="){string op=Take();e=new BinaryExpr(op,e,Sum());}return e;}
        FarmExpr Sum(){var e=Product();while(Peek=="+"||Peek=="-"){string op=Take();e=new BinaryExpr(op,e,Product());}return e;}
        FarmExpr Product(){var e=Unary();while(Peek=="*"||Peek=="/"||Peek=="%"){string op=Take();e=new BinaryExpr(op,e,Unary());}return e;}
        FarmExpr Unary(){if(Peek=="-"||Peek=="+"||Peek=="not"){string op=Take();return new UnaryExpr(op,Unary());}return Primary();}
        FarmExpr Primary()
        {
            string token=Take();if(token=="("){var e=Or();if(!Match(")"))throw new Exception(L10n.T("缺少右括号"));return e;}
            if(token.StartsWith("\""))return new LiteralExpr(token.Substring(1));
            double number;if(double.TryParse(token,NumberStyles.Float,CultureInfo.InvariantCulture,out number))return new LiteralExpr(number);
            if(!Regex.IsMatch(token,"^[A-Za-z_][A-Za-z0-9_]*$"))throw new Exception(L10n.T("无效名称：")+token);
            if(Match("("))
            {
                var args=new List<FarmExpr>();if(!Match(")")){do{args.Add(Or());}while(Match(","));if(!Match(")"))throw new Exception(L10n.T("缺少右括号"));}
                if(Array.IndexOf(new[]{"ready","crop","moisture","fertility","x","y","size","energy","money","count","neighbors","min","max","range"},token)<0&&!VerdantFleet.IsSensor(token))throw new Exception(L10n.T("未知传感器：")+token);
                return new SensorExpr(token,args);
            }
            return new NameExpr(token);
        }
    }
    public sealed class FarmInstruction
    {public string op,name;public FarmExpr expr;public FarmExpr[] args;public int line,target;public FarmInstruction(string op,int line,string name="",FarmExpr expr=null){this.op=op;this.line=line;this.name=name;this.expr=expr;}}
    public sealed class FarmProgram
    {public List<FarmInstruction> code=new List<FarmInstruction>();public Dictionary<string,int> functions=new Dictionary<string,int>();}
    public sealed class FarmCompiler
    {
        sealed class Line{public int number,indent;public string text;}
        sealed class Loop{public int start;public List<int> breaks=new List<int>();}
        List<Line> lines=new List<Line>();FarmProgram program;int index,unique,depth;Stack<Loop> loops=new Stack<Loop>();bool inFunction;
        public FarmProgram Compile(string text)
        {
            if(text.Length>48000)throw new FarmCodeException(1,L10n.T("程序最多 48000 字符"));
            program=new FarmProgram();string[] raw=text.Replace("\r","").Split('\n');
            for(int i=0;i<raw.Length;i++)
            {
                string row=raw[i];if(row.IndexOf('\t')>=0)throw new FarmCodeException(i+1,L10n.T("请用四个空格代替 Tab 缩进"));
                int spaces=row.Length-row.TrimStart(' ').Length;string value=StripComment(row).Trim();
                if(value=="")continue;if(spaces%4!=0)throw new FarmCodeException(i+1,L10n.T("缩进必须是四个空格的倍数"));
                lines.Add(new Line{number=i+1,indent=spaces,text=value});
            }
            if(lines.Count==0)throw new FarmCodeException(1,L10n.T("请先输入程序"));
            Block(0);if(index<lines.Count)throw new FarmCodeException(lines[index].number,L10n.T("缩进不匹配"));
            foreach(var instruction in program.code)if(instruction.op=="call"&&!program.functions.ContainsKey(instruction.name))throw new FarmCodeException(instruction.line,L10n.T("函数未定义：")+instruction.name);
            return program;
        }
        static string StripComment(string line)
        {char quote='\0';for(int i=0;i<line.Length;i++){char c=line[i];if(c=='\\'&&quote!='\0'){i++;continue;}if(c=='\''||c=='"'){if(quote=='\0')quote=c;else if(quote==c)quote='\0';}if(c=='#'&&quote=='\0')return line.Substring(0,i);}return line;}
        int Emit(string op,int line,string name="",FarmExpr expr=null){program.code.Add(new FarmInstruction(op,line,name,expr));return program.code.Count-1;}
        FarmExpr Expr(string s,int line){try{return new ExprParser(s).Parse();}catch(Exception e){throw new FarmCodeException(line,e.Message);}}
        void Body(Line parent)
        {if(index>=lines.Count||lines[index].indent!=parent.indent+4)throw new FarmCodeException(parent.number,L10n.T("代码块需要四个空格缩进的内容"));Block(parent.indent+4);}
        void Block(int indent)
        {
            if(++depth>32)throw new FarmCodeException(index<lines.Count?lines[index].number:1,L10n.T("代码嵌套过深"));
            while(index<lines.Count&&lines[index].indent>=indent)
            {
                var line=lines[index];if(line.indent>indent)throw new FarmCodeException(line.number,L10n.T("多余的缩进"));index++;
                string s=line.text;bool colon=s.EndsWith(":");
                if(s.StartsWith("if "))
                {
                    if(!colon)throw new FarmCodeException(line.number,L10n.T("if 末尾需要冒号"));
                    var exits=new List<int>();int condition=Emit("test",line.number,"",Expr(s.Substring(3,s.Length-4),line.number));Body(line);exits.Add(Emit("jump",line.number));program.code[condition].target=program.code.Count;
                    while(index<lines.Count&&lines[index].indent==indent&&lines[index].text.StartsWith("elif "))
                    {var branch=lines[index++];string t=branch.text;if(!t.EndsWith(":"))throw new FarmCodeException(branch.number,L10n.T("elif 末尾需要冒号"));condition=Emit("test",branch.number,"",Expr(t.Substring(5,t.Length-6),branch.number));Body(branch);exits.Add(Emit("jump",branch.number));program.code[condition].target=program.code.Count;}
                    if(index<lines.Count&&lines[index].indent==indent&&lines[index].text=="else:"){var otherwise=lines[index++];Body(otherwise);}
                    foreach(int e in exits)program.code[e].target=program.code.Count;continue;
                }
                if(s.StartsWith("while "))
                {
                    if(!colon)throw new FarmCodeException(line.number,L10n.T("while 末尾需要冒号"));
                    int start=program.code.Count;int condition=Emit("test",line.number,"",Expr(s.Substring(6,s.Length-7),line.number));var loop=new Loop{start=start};loops.Push(loop);Body(line);int back=Emit("jump",line.number);program.code[back].target=start;loops.Pop();program.code[condition].target=program.code.Count;foreach(int b in loop.breaks)program.code[b].target=program.code.Count;continue;
                }
                if(s.StartsWith("for ")||s.StartsWith("repeat "))
                {
                    if(!colon)throw new FarmCodeException(line.number,L10n.T("循环末尾需要冒号"));string name,expression;
                    if(s.StartsWith("repeat ")){name="_repeat"+unique;expression="range("+s.Substring(7,s.Length-8)+")";}
                    else{var match=Regex.Match(s,"^for ([A-Za-z_][A-Za-z0-9_]*) in (.+):$");if(!match.Success)throw new FarmCodeException(line.number,L10n.T("格式为 for col in range(size()):"));name=match.Groups[1].Value;expression=match.Groups[2].Value;}
                    string key="_range"+(unique++);Emit("range",line.number,key,Expr(expression,line.number));int start=Emit("next",line.number,name);program.code[start].expr=new LiteralExpr(key);var loop=new Loop{start=start};loops.Push(loop);Body(line);int back=Emit("jump",line.number);program.code[back].target=start;loops.Pop();program.code[start].target=program.code.Count;foreach(int b in loop.breaks)program.code[b].target=program.code.Count;continue;
                }
                if(s.StartsWith("def "))
                {
                    var match=Regex.Match(s,"^def ([A-Za-z_][A-Za-z0-9_]*)\\(\\):$");
                    if(!match.Success||inFunction||loops.Count>0||indent!=0)throw new FarmCodeException(line.number,L10n.T("函数必须在顶层定义，格式为 def tend():"));
                    string name=match.Groups[1].Value;if(program.functions.ContainsKey(name)||IsAction(name)||VerdantFleet.IsAction(name))throw new FarmCodeException(line.number,L10n.T("重复或保留的函数名"));
                    int skip=Emit("jump",line.number);program.functions[name]=program.code.Count;inFunction=true;Body(line);Emit("return",line.number);inFunction=false;program.code[skip].target=program.code.Count;continue;
                }
                if(s=="break"||s=="continue")
                {if(loops.Count==0)throw new FarmCodeException(line.number,s+L10n.T(" 只能写在循环中"));int jump=Emit("jump",line.number);if(s=="break")loops.Peek().breaks.Add(jump);else program.code[jump].target=loops.Peek().start;continue;}
                if(s=="return"){if(!inFunction)throw new FarmCodeException(line.number,L10n.T("return 只能写在函数中"));Emit("return",line.number);continue;}
                if(s=="pass"){Emit("pass",line.number);continue;}
                var assign=Regex.Match(s,"^([A-Za-z_][A-Za-z0-9_]*)\\s*=(?!=)\\s*(.+)$");
                if(assign.Success){Emit("set",line.number,assign.Groups[1].Value,Expr(assign.Groups[2].Value,line.number));continue;}
                var call=Regex.Match(s,"^([A-Za-z_][A-Za-z0-9_]*)\\((.*)\\)$");
                if(call.Success)
                {
                    string name=call.Groups[1].Value,arg=call.Groups[2].Value.Trim();
                    if(VerdantFleet.IsAction(name))
                    {var arguments=(SensorExpr)Expr("range("+arg+")",line.number);int at=Emit("action",line.number,name);program.code[at].args=arguments.Args.ToArray();continue;}
                    if(IsAction(name))
                    {bool needs=name=="move"||name=="plant"||name=="wait"||name=="print";if(needs&&arg==""||!needs&&arg!="")throw new FarmCodeException(line.number,name+L10n.T("() 参数数量不正确"));Emit("action",line.number,name,needs?Expr(arg,line.number):null);}
                    else{if(arg!="")throw new FarmCodeException(line.number,L10n.T("自定义函数暂不接受参数"));Emit("call",line.number,name);}continue;
                }
                throw new FarmCodeException(line.number,L10n.T("无法识别这条语句，请查看手册"));
            }
            depth--;
        }
        static bool IsAction(string n){return Array.IndexOf(new[]{"move","plant","harvest","till","water","fertilize","wait","print"},n)>=0;}
    }
    public sealed class FarmVM
    {
        sealed class Frame{public int pc;public Dictionary<string,object> locals;}
        public VerdantFarm Farm;public FarmProgram Program;
        public Dictionary<string,object> Variables=new Dictionary<string,object>();public Dictionary<string,object> Globals;
        Stack<Frame> calls=new Stack<Frame>();public int PC,CurrentLine,Executed;public bool Finished;public float Wait;
        public FarmVM(VerdantFarm farm,FarmProgram program){Farm=farm;Program=program;Globals=Variables;}
        public static double Number(object o){if(o is bool)return (bool)o?1:0;double n;if(o==null||!double.TryParse(Convert.ToString(o,CultureInfo.InvariantCulture),NumberStyles.Float,CultureInfo.InvariantCulture,out n)||double.IsNaN(n)||double.IsInfinity(n))throw new Exception(L10n.T("此处需要有限数值"));return n;}
        public static bool Truth(object o){if(o==null)return false;if(o is bool)return (bool)o;if(o is string)return ((string)o).Length>0;return Number(o)!=0;}
        public bool Step()
        {
            if(Finished)return false;if(PC>=Program.code.Count){Finished=true;return false;}
            var i=Program.code[PC];CurrentLine=i.line;Wait=0;
            try
            {
                if(++Executed>500000)throw new Exception(L10n.T("已达到 500000 条指令的安全上限，请停止后重新运行"));
                PC++;
                switch(i.op)
                {
                    case "jump":PC=i.target;break;
                    case "test":if(!Truth(i.expr.Eval(this)))PC=i.target;break;
                    case "set":Variables[i.name]=i.expr.Eval(this);break;
                    case "range":var range=i.expr.Eval(this) as FarmRange;if(range==null)throw new Exception(L10n.T("for 循环需要 range()"));Variables[i.name]=range;break;
                    case "next":var r=(FarmRange)Variables[(string)i.expr.Eval(this)];if(r.Next())Variables[i.name]=(double)r.value;else PC=i.target;break;
                    case "call":if(calls.Count>=24)throw new Exception(L10n.T("函数递归超过 24 层"));calls.Push(new Frame{pc=PC,locals=Variables});Variables=new Dictionary<string,object>();PC=Program.functions[i.name];Farm.Add("functionCalls");break;
                    case "return":if(calls.Count==0){Finished=true;break;}var f=calls.Pop();PC=f.pc;Variables=f.locals;break;
                    case "action":
                        if(VerdantFleet.IsAction(i.name))
                        {var values=new object[i.args.Length];for(int a=0;a<values.Length;a++)values[a]=i.args[a].Eval(this);Farm.Fleet.Invoke(i.name,values);Wait=.05f;return true;}
                        object value=i.expr==null?null:i.expr.Eval(this);
                        if(i.name=="print"){Farm.Say(Convert.ToString(value,CultureInfo.InvariantCulture));Wait=0.05f;}
                        else if(i.name=="wait"){Wait=Mathf.Clamp((float)Number(value),0.02f,120);Farm.Act("wait","",true);}
                        else{Farm.Act(i.name,value==null?"":Convert.ToString(value,CultureInfo.InvariantCulture),true);Wait=Farm.ActionDelay;}
                        return true;
                }
            }catch(FarmCodeException){throw;}catch(Exception e){throw new FarmCodeException(CurrentLine,e.Message);}
            return false;
        }
        public string Watch()
        {
            var b=new StringBuilder();foreach(var pair in Variables){if(pair.Key.StartsWith("_"))continue;if(b.Length>120)break;b.Append(pair.Key).Append(" = ").Append(Convert.ToString(pair.Value,CultureInfo.InvariantCulture)).Append("   ");}return b.ToString();
        }
    }
}
