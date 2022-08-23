using BestellserviceWeb.Data;
using BestellserviceWeb.Helpers;
using BestellserviceWeb.Models;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BestellserviceWeb.Controllers
{
    public class KundeController : Controller
    {
        private readonly BestellserviceDBContext _context;
        public static List<TblKunde> kundenCart = new List<TblKunde>();
        private readonly string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        private IWebHostEnvironment hostEnvironment;


        public KundeController(BestellserviceDBContext context, IWebHostEnvironment Environment)
        {
            _context = context;
            hostEnvironment = Environment;
        }

        public IActionResult Index()
        {
            List<TblKunde> kunden = _context.TblKunde.ToList();
            TempData["CartSize"] = kundenCart.Count();
            return View(kunden);
        }

        public IActionResult Details(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            return View(kunde);
        }
        
        [HttpGet]
        public IActionResult Cart()
        {
            TempData["CartSize"] = kundenCart.Count();
            return View(kundenCart);
        }

        [HttpPost]
        public IActionResult CartAdd(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            kundenCart.Add(kunde);
            TempData["CartSize"] = kundenCart.Count();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CartDelete(int id)
        {
            for (int i = 0; i < kundenCart.Count(); i++)
            {
                if (kundenCart[i].KunId == id)
                {
                    kundenCart.RemoveAt(i);
                    TempData["CartSize"] = kundenCart.Count();
                }
            }
            return RedirectToAction("Cart");
        }


        [HttpGet]
        public IActionResult UploadPic(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            List<TblBilder> bilder = _context.TblBilder.ToList();
            TblBilder hilfsBild = new TblBilder { BildKunde = kunde.KunId };
            List<TblBilder> kundenBilder = new List<TblBilder>();
            kundenBilder.Add(hilfsBild);
            
            foreach (TblBilder b in bilder)
            {
                if(b.BildKunde == kunde.KunId)
                {
                    kundenBilder.Add(b);
                    Byte[] bytes = b.BildDatei;
                }
            }

            return View(kundenBilder);
        }

        [HttpGet]
        public IActionResult UploadPdf(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            List<TblDokumente> doks = _context.TblDokumente.ToList();
            TblDokumente hilfsdok = new TblDokumente { DokKunde = kunde.KunId };
            List<TblDokumente> kundenDoks = new List<TblDokumente>();
            kundenDoks.Add(hilfsdok);

            foreach (TblDokumente b in doks)
            {
                if (b.DokKunde == kunde.KunId)
                {
                    kundenDoks.Add(b);
                }
            }

            return View(kundenDoks);
        }

        [HttpPost]
        public async Task<ActionResult> UploadPdf(int id, IFormFile file)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            List<TblDokumente> doks = _context.TblDokumente.ToList();

            string filename = file.FileName;
            BinaryReader br = new BinaryReader(file.OpenReadStream());
            Byte[] bytes = br.ReadBytes((Int32)file.Length);
            br.Close();

            TblDokumente dokToAdd = new TblDokumente();

            dokToAdd = new TblDokumente { DokKunde = kunde.KunId, DokDatei = bytes, DokName = filename};

            await _context.TblDokumente.AddAsync(dokToAdd);
            await _context.SaveChangesAsync();

            return RedirectToAction("UploadPdf", "Kunde");
        }

        [HttpPost]
        public async Task<ActionResult> UploadPic(int id, IFormFile file)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            List<TblBilder> bilder = _context.TblBilder.ToList();
            int bilderCount = 0;

            foreach (TblBilder b in bilder)
            {
                if(b.BildKunde == kunde.KunId)
                {
                    bilderCount++;
                }
            }

            var fileDic = "Files";
            string filePath = Path.Combine(hostEnvironment.WebRootPath, fileDic);

            string filename = file.FileName;
            filePath = Path.Combine(filePath, filename);

            //FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(file.OpenReadStream());

            Byte[] bytes = br.ReadBytes((Int32)file.Length);

            br.Close();

            //fs.Close();

            TblBilder bildToAdd = new TblBilder();
            

            //on purpose to always put false cause we wont use the true anymore
            if (bilderCount>=0)
            {
                bildToAdd = new TblBilder { BildKunde = kunde.KunId, BildDatei = bytes, BildName = filename, BildHaupt = false };
            }
            else
            {
                bildToAdd = new TblBilder { BildKunde = kunde.KunId, BildDatei = bytes, BildName = filename, BildHaupt = true };
            }

            await _context.TblBilder.AddAsync(bildToAdd);
            await _context.SaveChangesAsync();

            return RedirectToAction("UploadPic", "Kunde");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            return View(kunde);
        }

        [HttpPost]
        public IActionResult Edit(TblKunde kunde)
        {
            _context.Attach(kunde);
            _context.Entry(kunde).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            return View(kunde);
        }

        [HttpPost]
        public IActionResult Delete(TblKunde kunde)
        {
            _context.Attach(kunde);
            _context.Entry(kunde).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            TblKunde kunde = new TblKunde();
            return View(kunde);
        }

        [HttpPost]
        public IActionResult Create(TblKunde kunde)
        {
            _context.Attach(kunde);
            _context.Entry(kunde).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Export(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            return View(kunde);
        }

        [HttpGet]
        public IActionResult DownloadImage(int id)
        {
            TblBilder bild = _context.TblBilder.Where(p => p.BildId == id).FirstOrDefault();
            var fileExt = bild.BildName.Split(".");
            var filecount = fileExt.Count();

            using (var client = new System.Net.WebClient())
            {

                using(var memoryImage = new MemoryStream())
                {
                    MemoryStream memoryStream = new MemoryStream(bild.BildDatei);
                    HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

                    response.Content = new StreamContent(memoryStream);
                    response.Content.Headers.ContentType =new System.Net.Http.Headers.MediaTypeHeaderValue("application/"+ fileExt[filecount-1]);
                }

            }

            return File(bild.BildDatei, "application/" + fileExt[filecount - 1], bild.BildName);
        }

        [HttpGet]
        public IActionResult DownloadPdfDirect(int id)
        {
            TblDokumente dok = _context.TblDokumente.Where(p => p.DokId == id).FirstOrDefault();
            var fileExt = dok.DokName.Split(".");
            var filecount = fileExt.Count();

            using (var client = new System.Net.WebClient())
            {

                using (var memoryImage = new MemoryStream())
                {
                    MemoryStream memoryStream = new MemoryStream(dok.DokDatei);
                    HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

                    response.Content = new StreamContent(memoryStream);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                }

            }

            return File(dok.DokDatei, "application/pdf", dok.DokName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportAllAsync()
        {
            List<TblKunde> kunden = kundenCart.ToList();

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Times New Roman", 10, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            XRect rect = new XRect(40, 100, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            //tf.Alignment = ParagraphAlignment.Left;
            tf.DrawString(kunden[0].KunVorname, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(310, 100, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(kunden[0].KunGeschlecht, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(40, 400, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(kunden.Count().ToString(), font, XBrushes.Black, rect, XStringFormats.TopLeft) ;

            rect = new XRect(310, 400, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Justify;

            // Save the document...
            string path = wwwrootPath + "\\" + "test.pdf";
            document.Save(path);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var filename = "test.pdf";

            FileInfo file = new FileInfo(path);
            file.Delete();

            return File(memory, contentType, filename);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadWord(int id)
        {
            //Word begins here
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
            XWPFDocument doc = new XWPFDocument();

            XWPFParagraph p1 = doc.CreateParagraph();
            p1.Alignment = ParagraphAlignment.CENTER;
            p1.BorderBottom = Borders.Double;
            p1.BorderTop = Borders.Double;

            p1.BorderRight = Borders.Double;
            p1.BorderLeft = Borders.Double;
            p1.BorderBetween = Borders.Single;

            p1.VerticalAlignment = TextAlignment.TOP;

            XWPFRun r1 = p1.CreateRun();
            r1.SetText(kunde.KunVorname + " " + kunde.KunNachname);
            r1.IsBold = true;
            r1.FontFamily = "Courier";
            r1.SetUnderline(UnderlinePatterns.DotDotDash);
            r1.TextPosition = 100;

            XWPFParagraph p2 = doc.CreateParagraph();
            p2.Alignment = ParagraphAlignment.RIGHT;

            //BORDERS
            p2.BorderBottom = Borders.Double;
            p2.BorderTop = Borders.Double;
            p2.BorderRight = Borders.Double;
            p2.BorderLeft = Borders.Double;
            p2.BorderBetween = Borders.Single;

            XWPFRun r2 = p2.CreateRun();
            r2.SetText(kunde.KunGeschlecht);
            r2.IsStrikeThrough = true;
            r2.FontSize = 20;

            XWPFRun r3 = p2.CreateRun();
            r3.SetText(kunde.KunGeburtsdatum.ToString());
            r3.IsStrikeThrough = true;
            r3.FontSize = 20;
            r3.Subscript = VerticalAlign.SUPERSCRIPT;
            r3.SetColor("FF0000");

            XWPFParagraph p3 = doc.CreateParagraph();
            p3.IsWordWrapped = true;
            p3.IsPageBreak = true;
            p3.Alignment = ParagraphAlignment.BOTH;
            p3.SpacingLineRule = LineSpacingRule.EXACT;
            p3.IndentationFirstLine = 600;

            XWPFRun r4 = p3.CreateRun();
            r4.TextPosition = 20;
            r4.SetText("To be, or not to be: that is the question: "
                    + "Whether 'tis nobler in the mind to suffer "
                    + "The slings and arrows of outrageous fortune, "
                    + "Or to take arms against a sea of troubles, "
                    + "And by opposing end them? To die: to sleep; ");
            r4.AddBreak(BreakType.PAGE);
            r4.SetText("No more; and by a sleep to say we end "
                    + "The heart-ache and the thousand natural shocks "
                    + "That flesh is heir to, 'tis a consummation "
                    + "Devoutly to be wish'd. To die, to sleep; "
                    + "To sleep: perchance to dream: ay, there's the rub; "
                    + ".......");
            r4.IsItalic = true;
            //This would imply that this break shall be treated as a simple line break, and break the line after that word:

            XWPFRun r5 = p3.CreateRun();
            r5.TextPosition = -10;
            r5.SetText("For in that sleep of death what dreams may come");
            r5.AddCarriageReturn();
            r5.SetText("When we have shuffled off this mortal coil,"
                    + "Must give us pause: there's the respect"
                    + "That makes calamity of so long life;");
            r5.AddBreak();
            r5.SetText("For who would bear the whips and scorns of time,"
                    + "The oppressor's wrong, the proud man's contumely,");

            r5.AddBreak(BreakClear.ALL);
            r5.SetText("The pangs of despised love, the law's delay,"
                    + "The insolence of office and the spurns" + ".......");


            /*
            FileStream out1 = new FileStream(wwwrootPath + "\\" + "simple.docx", FileMode.Create);
            doc.Write(out1);
            out1.Close();


            string path = wwwrootPath + "\\" + "simple.docx";
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var filename = "simple.docx";

            FileInfo file = new FileInfo(path);
            file.Delete();

            return File(memory, contentType, filename);*/


            doc.WriteWordToResponse(HttpContext, "test.docx");

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();
           

            //Pdf begins here

            const string text =
    "Facin exeraessisit la consenim iureet dignibh eu facilluptat vercil dunt autpat. " +
    "Ecte magna faccum dolor sequisc iliquat, quat, quipiss equipit accummy niate magna " +
    "facil iure eraesequis am velit, quat atis dolore dolent luptat nulla adio odipissectet " +
    "lan venis do essequatio conulla facillandrem zzriusci bla ad minim inis nim velit eugait " +
    "aut aut lor at ilit ut nulla ate te eugait alit augiamet ad magnim iurem il eu feuissi.\n" +
    "Guer sequis duis eu feugait luptat lum adiamet, si tate dolore mod eu facidunt adignisl in " +
    "henim dolorem nulla faccum vel inis dolutpatum iusto od min ex euis adio exer sed del " +
    "dolor ing enit veniamcon vullutat praestrud molenis ciduisim doloborem ipit nulla consequisi.\n" +
    "Nos adit pratetu eriurem delestie del ut lumsandreet nis exerilisit wis nos alit venit praestrud " +
    "dolor sum volore facidui blaor erillaortis ad ea augue corem dunt nis  iustinciduis euisi.\n" +
    "Ut ulputate volore min ut nulpute dolobor sequism olorperilit autatie modit wisl illuptat dolore " +
    "min ut in ute doloboreet ip ex et am dunt at.";

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Times New Roman", 10, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);

            XRect rect = new XRect(40, 100, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            //tf.Alignment = ParagraphAlignment.Left;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(310, 100, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(40, 400, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(kunde.KunNachname + " " + kunde.KunVorname, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            rect = new XRect(310, 400, 250, 220);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            tf.Alignment = XParagraphAlignment.Justify;

            // Save the document...
            string path = wwwrootPath + "\\" + "test.pdf";
            document.Save(path);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var filename = "test.pdf";

            FileInfo file = new FileInfo(path);
            file.Delete();

            return File(memory, contentType, filename);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadExcel(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();

            // Excel begins here
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet1 = workbook.CreateSheet("Sheet1");
            sheet1.CreateRow(0).CreateCell(0).SetCellValue(kunde.KunVorname + " " + kunde.KunNachname);
            /*
            FileStream sw = new FileStream(wwwrootPath + "\\" + "test.xlsx", FileMode.Create);

            workbook.Write(sw);
            sw.Close();

            string path = wwwrootPath + "\\" + "test.xlsx";
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var filename = "test.xlsx";

            FileInfo file = new FileInfo(path);
            file.Delete();
            */

            workbook.WriteExcelToResponse(HttpContext, "test.xlsx");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadCsv(int id)
        {
            TblKunde kunde = _context.TblKunde.Where(p => p.KunId == id).FirstOrDefault();

            var records = new List<TblKunde>
            {
                new TblKunde { KunId = kunde.KunId, KunGeburtsdatum = kunde.KunGeburtsdatum, KunGeschlecht = kunde.KunGeschlecht, KunVorname = kunde.KunVorname, KunNachname = kunde.KunNachname, KunVip = kunde.KunVip },
            };

            using (var writer = new StreamWriter(wwwrootPath + "\\" + "file.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }

            string path = wwwrootPath + "\\" + "file.csv";
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var filename = "file.csv";

            FileInfo file = new FileInfo(path);
            file.Delete();

            return File(memory,contentType,filename);
        }
    }
}
