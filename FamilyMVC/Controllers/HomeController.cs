using FamilyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;

namespace FamilyMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpHelper httpHelper = new HttpHelper();

        public ActionResult Home()
        {

            return View();
        }

        public ActionResult CreateApplication()
        {
            try
            {
                HouseHoldMemberDetails retainedId = new HouseHoldMemberDetails();
                if (Session["HouseholdMemberDetails"] != null)
                {
                    retainedId = (HouseHoldMemberDetails)Session["HouseholdMemberDetails"];
                }
                return View(retainedId);
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }

        }
        [HttpPost]
        public ActionResult CreateApplication(HouseHoldMemberDetails houseHoldMemberDetails, string submit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int age = DateTime.Now.Year - DateTime.Parse(houseHoldMemberDetails.DateOfBirth.ToString()).Year;
                    if (age > 125 && age < 0)
                    {
                        ModelState.AddModelError("DateOfBirth", "Age cannot be more than 125 or not after current year");
                    }
                    int userId = int.Parse(Session["UserId"].ToString());
                    var list = httpHelper.GetCall<List<HouseHoldMemberDetails>>("api/Home/GetHouseHoldMembers?id=" + userId).Result;

                    if (list.Count == 5)
                    {
                        ViewBag.CountError = "Family cannot have more than 5 members";
                        return View();
                    }

                    var FirstMemberOfFamily = list.AsQueryable().Where(x => x.isFirstMember == true && x.UserId == userId).FirstOrDefault();

                    int ageOfFirstMember = DateTime.Now.Year - DateTime.Parse(FirstMemberOfFamily.DateOfBirth.ToString()).Year;
                    if (age > ageOfFirstMember)
                    {
                        ModelState.AddModelError("DateOfBirth", "Age cannot be greater than First Member Of Family");
                    }

                    if (ModelState.IsValid)
                    {
                        switch (submit)
                        {
                            case "AddMember":
                                houseHoldMemberDetails.UserId = int.Parse(Session["UserId"].ToString());
                                httpHelper.PostData<HouseHoldMemberDetails>(houseHoldMemberDetails, "api/Home/PostHouseHoldMember");
                                break;
                            case "Save&Exit":
                                Session["HouseholdMemberDetails"] = houseHoldMemberDetails;
                                break;
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }

        public ActionResult SearchApplication()
        {
            return View();
        }

        [HttpGet]
        [ValidateInput(false)]
        public FileResult Export(int AppId, int FamilyId)
        {

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                pdfDoc.Open();
                pdfDoc.Add(new Paragraph("Your ApplicationId is " + AppId));
                int Userid = int.Parse(Session["UserId"].ToString());

                var FamilyMembers = httpHelper.GetCall<List<HouseHoldMemberDetails>>("api/Home/GetHouseHoldMembers?id=" + Userid).Result;
                foreach (var item in FamilyMembers)
                {
                    Paragraph p = new Paragraph();
                    p.Add("First Name : " + item.FirstName + "\n  Last Name : " + item.LastName + "\n  Suffix : " + item.Suffix + " \n DateOfBith : " + DateTime.Parse(item.DateOfBirth.ToString()).Date + "\n  Gender : " + item.Gender);
                    pdfDoc.Add(p);
                }


                var Relations = httpHelper.GetCall<List<FamilyRelations>>("api/Family/GetFamilyRelations").Result;
                Relations = Relations.AsQueryable().Where(x => x.FamilyId == FamilyId).ToList();
                foreach (var item in Relations)
                {
                    Paragraph p = new Paragraph();
                    p.Add(item.Id.Split('_')[0] + " Relation to " + item.Id.Split('_')[2] + " is " + item.Relation);
                    pdfDoc.Add(p);
                }

                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                //Phrase mainPharse = new Phrase();

                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}