using UnityEngine;
using UnityEngine.UI;

namespace Verdant.Automation
{
    /// <summary>Resolution-independent mesh illustration; no textures, videos or farm objects.</summary>
    public sealed class ClassicOpeningArt : Graphic
    {
        public float Seconds;
        VertexHelper mesh;
        Vector2 origin;
        float scale;
        static Color C(string hex,float alpha=1){var c=ClassicView.ColorOf(hex);c.a=alpha;return c;}
        Vector2 P(Vector2 p){return origin+p*scale;}
        void Quad(Vector2 a,Vector2 b,Vector2 c,Vector2 d,Color color)
        {
            int i=mesh.currentVertCount;mesh.AddVert(P(a),color,Vector2.zero);mesh.AddVert(P(b),color,Vector2.zero);mesh.AddVert(P(c),color,Vector2.zero);mesh.AddVert(P(d),color,Vector2.zero);mesh.AddTriangle(i,i+1,i+2);mesh.AddTriangle(i,i+2,i+3);
        }
        void Line(Vector2 a,Vector2 b,float width,Color color)
        {Vector2 n=new Vector2(-(b-a).y,(b-a).x).normalized*width*.5f;Quad(a-n,a+n,b+n,b-n,color);}
        void Disc(Vector2 center,float radius,Color color,int steps=20)
        {
            int i=mesh.currentVertCount;mesh.AddVert(P(center),color,Vector2.zero);for(int j=0;j<=steps;j++){float angle=j*Mathf.PI*2/steps;mesh.AddVert(P(center+new Vector2(Mathf.Cos(angle),Mathf.Sin(angle))*radius),color,Vector2.zero);if(j>0)mesh.AddTriangle(i,i+j,i+j+1);}
        }
        void Ring(Vector2 center,float radius,float width,Color color)
        {
            for(int i=0;i<64;i++){float a=i*Mathf.PI/32,b=(i+1)*Mathf.PI/32;Line(center+new Vector2(Mathf.Cos(a),Mathf.Sin(a))*radius,center+new Vector2(Mathf.Cos(b),Mathf.Sin(b))*radius,width,color);}
        }
        Vector2 Tile(float x,float y){return new Vector2(1135+(x-y)*43,430+(x+y-6)*23);}
        void Diamond(Vector2 center,float x,float y,Color color){Quad(center+Vector2.left*x,center+Vector2.up*y,center+Vector2.right*x,center+Vector2.down*y,color);}
        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear();mesh=helper;var rect=rectTransform.rect;scale=Mathf.Min(rect.width/1600,rect.height/1000);origin=rect.center-new Vector2(800,500)*scale;
            // Quiet orbital traces and a soft dawn behind the field.
            for(int i=7;i>0;i--)Disc(new Vector2(1170,515),100+i*39,C("8AAB6D",.013f));
            Ring(new Vector2(1135,470),348,1,C("73957E",.2f));Ring(new Vector2(1135,470),373,1,C("73957E",.08f));
            for(int i=0;i<26;i++)
            {
                float x=805+(i*83%690),y=210+(i*137%550)+Mathf.Sin(Seconds*.4f+i)*8;float a=(.15f+.15f*Mathf.Sin(Seconds+i))*Mathf.Clamp01(Seconds/3);
                Disc(new Vector2(x,y),i%3==0?2.2f:1.1f,C("D5DEA0",a),8);
            }
            float reveal=Mathf.SmoothStep(0,1,Mathf.Clamp01((Seconds-.5f)/2));
            Vector2 left=Tile(-.65f,6.65f),top=Tile(6.65f,6.65f),right=Tile(6.65f,-.65f),bottom=Tile(-.65f,-.65f);
            Vector2 depth=new Vector2(0,-42*reveal);
            Quad(left,bottom,bottom+depth,left+depth,C("345449",reveal));Quad(bottom,right,right+depth,bottom+depth,C("203F35",reveal));
            Quad(left,top,right,bottom,C("70875B",reveal));
            for(int sum=12;sum>=0;sum--)for(int x=0;x<7;x++)
            {
                int y=sum-x;if(y<0||y>6)continue;Vector2 p=Tile(x,y);float delay=(x+y)*.13f;
                float show=Mathf.Clamp01((Seconds-.8f-delay)*1.5f);p.y-=12*(1-show);
                Color earth=Color.Lerp(C("253D32"),C("4C6846"),.35f+((x*17+y*11)%7)*.07f);earth.a=show;
                Diamond(p,40.5f,21.5f,earth);
                // Circuit-like furrows reveal the programmable field.
                Line(p+new Vector2(-28,0),p+new Vector2(0,15),.8f,C("8DAC70",show*.3f));
                float grown=Mathf.SmoothStep(0,1,Mathf.Clamp01((Seconds-3-delay-(x*3+y*5)%4*.38f)/2.2f));
                if(grown<=0)continue;
                Vector2 root=p+new Vector2(0,3),tip=root+Vector2.up*(12+25*grown);
                Line(root,tip,2,C("B2CA7D",grown));
                Vector2 leaf=root+Vector2.up*13*grown;
                Quad(leaf,leaf+new Vector2(-12,7)*grown,leaf+new Vector2(-6,12)*grown,leaf+new Vector2(1,7)*grown,C("8FAF70",grown));
                Quad(leaf+Vector2.up*7,leaf+new Vector2(11,18)*grown,leaf+new Vector2(13,10)*grown,leaf+Vector2.up*10,C("B8CA82",grown));
                if((x+y*2)%4==0)
                {
                    for(int petal=0;petal<6;petal++){float angle=petal*Mathf.PI/3;Disc(tip+new Vector2(Mathf.Cos(angle),Mathf.Sin(angle))*5*grown,4*grown,C("E5C978",grown),8);}
                    Disc(tip,3*grown,C("887048",grown),8);
                }
                else if((x+y)%3==0)Disc(p+new Vector2(11,4),7*grown,C("C79861",grown),12);
                else {Line(tip+new Vector2(-4,-6),tip+Vector2.up*2,3,C("D7D589",grown));Line(tip+new Vector2(4,-2),tip+Vector2.up*6,3,C("D7D589",grown));}
            }
            float wake=Mathf.Clamp01((Seconds-1.8f)/2);
            for(int i=0;i<7;i++)
            {
                Vector2 a=Tile(i,-.5f)+new Vector2(0,-35),b=Tile(i,6.5f)+new Vector2(0,-35);Line(a,b,1,C("A2C38D",wake*.12f));
                Disc(a,2,C("C6D795",wake*.65f),8);
            }
            float step=Mathf.Max(0,Seconds-2)*2.4f;int cell=Mathf.FloorToInt(step)%49;int next=(cell+1)%49;
            int row=cell/7,col=row%2==0?cell%7:6-cell%7;int nextRow=next/7,nextCol=nextRow%2==0?next%7:6-next%7;
            Vector2 current=Tile(col,row),following=Tile(nextCol,nextRow);float fraction=Mathf.SmoothStep(0,1,step-Mathf.Floor(step));
            Vector2 drone=Vector2.Lerp(current,following,fraction)+Vector2.up*(68+Mathf.Sin(Seconds*2.6f)*3);
            float lift=Mathf.SmoothStep(0,1,Mathf.Clamp01(Seconds/2));drone+=Vector2.up*(1-lift)*70;
            Diamond(drone+Vector2.down*72,26,9,C("0B241E",.22f*lift));
            float pulse=(Seconds*.7f)%1;Ring(current,10+pulse*32,1,C("D6E89F",(1-pulse)*.4f*wake));
            Line(drone+Vector2.down*15,current,1,C("D7E399",.22f*wake));
            Drone(drone,lift);
            // A tiny seed glyph underpins the typographic title.
            if(Seconds>10)
            {
                float a=Mathf.Clamp01(Seconds-10);Vector2 seed=new Vector2(110,275);
                Line(seed,seed+Vector2.up*38,2,C("D6E69A",a));Quad(seed+Vector2.up*12,seed+new Vector2(-19,32),seed+new Vector2(-8,36),seed+Vector2.up*20,C("D6E69A",a));Quad(seed+Vector2.up*24,seed+new Vector2(21,43),seed+new Vector2(22,29),seed+Vector2.up*16,C("9CC293",a));
                Line(new Vector2(150,278),new Vector2(540,278),1,C("B4C792",a*.35f));
            }
        }
        void Drone(Vector2 p,float alpha)
        {
            for(int i=0;i<4;i++)
            {
                Vector2 hub=p+new Vector2(i%2==0?-29:29,i<2?10:-10);Line(p,hub,5,C("93ADA0",alpha));
                Ring(hub,14,1.2f,C("A7CDB5",alpha*.45f));
                Vector2 blade=new Vector2(Mathf.Cos(Seconds*30+i),Mathf.Sin(Seconds*30+i)*.35f)*15;Line(hub-blade,hub+blade,2,C("D5DEC7",alpha*.9f));
            }
            Diamond(p+Vector2.down*5,24,14,C("75958A",alpha));Diamond(p+Vector2.up*4,24,14,C("E4E8D3",alpha));
            Line(p+new Vector2(-10,-6),p+new Vector2(10,5),4,C("284E43",alpha));Disc(p+new Vector2(6,-1),2,C("D9E99A",alpha),8);
        }
    }
}
