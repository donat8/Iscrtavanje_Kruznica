using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace kruznica2
{
   
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }
        class Kruznica
        {
            public Point pocetak, kraj;
            public double radius;
            public Rectangle rect;
            public double IzrRadius(Point poc, Point kraj)
            {
                int x1 = Math.Abs(poc.X - kraj.X);
                int y1 = Math.Abs(poc.Y - kraj.Y);

                radius = Math.Sqrt((x1 * x1) + (y1 * y1));
                return radius;
            }
            public void NacrtajKruznicu(Graphics g)
            {
                g.DrawLine(Pens.Black, pocetak, kraj);
                using (Pen pinky = new Pen(Color.Black, 4F))
                    g.DrawEllipse(pinky, rect);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        List<Kruznica> kruznice =new List<Kruznica>();
     
        class StvaranjeKruznice:Kruznica
        {        
            public Kruznica Kruznica;
        }
        StvaranjeKruznice stvaranjeKruznice;  //tu se spremaju kruznice u stvaranju       
       
        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate(); //briše formu i opet ju crta dok se prozor povećava/smanjuje
        }
              
        protected override void OnMouseDown(MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left) {
                Capture = true;
                stvaranjeKruznice = new StvaranjeKruznice
                {
                    pocetak = e.Location,
                    Kruznica = new Kruznica { }
                };
                stvaranjeKruznice.Kruznica.pocetak = e.Location;
               
            }
            base.OnMouseDown(e); 
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Capture) { 
                NovoIscrtavanjeKruznice(e.Location);
                stvaranjeKruznice.Kruznica.kraj = e.Location;
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Capture && e.Button == MouseButtons.Left) {
                Capture = false;
                NovoIscrtavanjeKruznice(e.Location);
               if (stvaranjeKruznice.Kruznica.kraj.X > 0 && stvaranjeKruznice.Kruznica.kraj.Y > 0)
                    kruznice.Add(stvaranjeKruznice.Kruznica);
                stvaranjeKruznice = null;
               // Invalidate();        //dok se miš pusti da se nacrta,nije potrebno
            }
            base.OnMouseUp(e);  //
        }

        private void NovoIscrtavanjeKruznice(Point kraj)
        {     
            Point pocetak = stvaranjeKruznice.pocetak;
            
            int Radius = (int)stvaranjeKruznice.IzrRadius(pocetak, kraj);

            stvaranjeKruznice.Kruznica.rect
                = new Rectangle(pocetak.X - Radius, pocetak.Y - Radius,Radius * 2, Radius * 2);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
           
            foreach (Kruznica kruznica in kruznice)
            {              
                kruznica.NacrtajKruznicu(g);
                kruznica.IzrRadius(kruznica.pocetak, kruznica.kraj);
                label1.Invalidate();
                label1.Text = $"površina je: {(int)((kruznica.radius) * (kruznica.radius) * Math.PI)} " +
                    $"opseg je: {(int)(2 * kruznica.radius * Math.PI)}";
            }
           
            if (stvaranjeKruznice != null)
                stvaranjeKruznice.Kruznica.NacrtajKruznicu(g);

            base.OnPaint(e);
            g.Dispose();
            
        }

    }
}
