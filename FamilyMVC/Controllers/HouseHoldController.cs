using FamilyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMVC.Controllers
{
    public class HouseHoldController : Controller
    {
        private readonly HttpHelper httpHelper = new HttpHelper();
        // GET: HouseHold
        public ActionResult Index()
        {
            try
            {
                List<HouseHoldMemberDetails> houseHoldMembers = new List<HouseHoldMemberDetails>();
                int Userid = int.Parse(Session["UserId"].ToString());
                houseHoldMembers = httpHelper.GetCall<List<HouseHoldMemberDetails>>("api/Home/GetHouseHoldMembers?id=" + Userid).Result;

                foreach (string key in Session.Contents)
                {
                    ViewData[key] = Session[key];
                }

                var Relations = httpHelper.GetCall<List<FamilyRelations>>("api/Family/GetFamilyRelations").Result;
                Relations = Relations.AsQueryable().Where(x => x.FamilyId == houseHoldMembers.First().FamilyId).ToList();
                if (Relations.Count == (houseHoldMembers.Count - 1) * (houseHoldMembers.Count))
                {
                    ViewBag.MessageForRelations = "All relations are successfully submitted earlier. !!";
                }

                return View(houseHoldMembers);
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }
        [HttpPost]
        public ActionResult SaveAndExit(IDictionary<string, string> jsondata)
        {
            try
            {
                var a = jsondata;

                foreach (var item in jsondata)
                {
                    Session[item.Key] = item.Value;
                }

                return Json("Done", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }
        [HttpPost]
        public ActionResult SubmitRelations(IDictionary<string, string> jsondata)
        {
            try
            {
                int FamId = int.Parse(jsondata["FamilyId"]);
                int flag = 1;
                int ApplicationID;
                jsondata.Remove("FamilyId");

                var Relations = httpHelper.GetCall<List<FamilyRelations>>("api/Family/GetFamilyRelations").Result;
                Relations = Relations.AsQueryable().Where(x => x.FamilyId == FamId).ToList();
                if (Relations.Count == jsondata.Count)
                {
                    FamilyDetails familyDetails = new FamilyDetails();
                    familyDetails.FamilyId = FamId;
                    familyDetails.isApproved = false;
                    var FamDetails = httpHelper.GetCall<List<FamilyDetails>>("api/Family/GetFamilyDetails").Result;
                    var RequiredFam = FamDetails.AsQueryable().Where(x => x.FamilyId == FamId).FirstOrDefault();
                    if (RequiredFam == null)
                    {
                        ApplicationID = httpHelper.PostDataAndGetAppId<FamilyDetails>(familyDetails, "api/Family/PostFamilyDetails");
                        return Json(ApplicationID, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(RequiredFam.ApplicationId, JsonRequestBehavior.AllowGet);
                    }
                }

                foreach (var item in jsondata)
                {
                    FamilyRelations familyRelations = new FamilyRelations();
                    familyRelations.Id = item.Key;
                    familyRelations.Relation = item.Value;
                    familyRelations.FamilyId = FamId;
                    bool status = httpHelper.PostData<FamilyRelations>(familyRelations, "api/Family/PostFamilyRelations");
                    if (status == false)
                    {
                        flag = 0;
                        return Json("Something went wrong", JsonRequestBehavior.AllowGet);
                    }
                }
                if (flag == 1)
                {
                    var FamDetails = httpHelper.GetCall<List<FamilyDetails>>("api/Family/GetFamilyDetails").Result;
                    var RequiredFam = FamDetails.AsQueryable().Where(x => x.FamilyId == FamId).FirstOrDefault();
                    if (RequiredFam == null)
                    {
                        FamilyDetails familyDetails = new FamilyDetails();
                        familyDetails.FamilyId = FamId;
                        familyDetails.isApproved = false;
                        ApplicationID = httpHelper.PostDataAndGetAppId<FamilyDetails>(familyDetails, "api/Family/PostFamilyDetails");
                        return Json(ApplicationID, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(RequiredFam.ApplicationId, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Something Went Wrong", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }

        }
        public ActionResult Confirmation(int AppId, int FamilyId)
        {
            try
            {
                ViewBag.AppId = AppId;
                ViewBag.FamilyId = FamilyId;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }

        [HttpGet]
        public JsonResult GetddValue(string id)
        {

            return Json(Session[id], JsonRequestBehavior.AllowGet);

        }
    }
}