using FamilyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace FamilyMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly HttpHelper httpHelper = new HttpHelper();
        // GET: Search
        public ActionResult SearchApplication()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchApplication(SearchApplication searchApplication, int? page)
        {
            try { 
            if (ModelState.IsValid)
            {
                var houseHoldMembers = httpHelper.GetCall<List<HouseHoldMemberDetails>>("api/Home/GetAllHouseHoldMembers").Result;
                //List<ResultSet> listOfA = houseHoldMembers.Cast<ResultSet>().ToList();
                List<ResultSet> resultSets = new List<ResultSet>();
                foreach (var data in houseHoldMembers)
                {
                    ResultSet resultSet = new ResultSet();
                    resultSet.FirstName = data.FirstName;
                    resultSet.LastName = data.LastName;
                    resultSet.MiddleName = data.MiddleName;
                    resultSet.DateOfBirth = data.DateOfBirth;
                    resultSet.FamilyId = data.FamilyId;
                    resultSet.isFirstMember = data.isFirstMember;
                    resultSet.Suffix = data.Suffix;
                    resultSet.UserId = data.UserId;
                    resultSet.MemberId = data.MemberId;
                    resultSet.Gender = data.Gender;
                    resultSets.Add(resultSet);
                }
                var FamDetails = httpHelper.GetCall<List<FamilyDetails>>("api/Family/GetFamilyDetails").Result;

                foreach (var data in FamDetails)
                {
                    var res = resultSets.AsQueryable().Where(x => x.FamilyId == data.FamilyId).ToList();
                    foreach (var item in res)
                    {
                        if (data.ApplicationId == 0)
                        {
                            item.ApplicationId = "n/a";
                        }
                        else
                            item.ApplicationId = data.ApplicationId.ToString();
                        if (data.isApproved == true)
                            item.isApproved = "True";
                        else
                            item.isApproved = "False";
                    }
                }

                if (searchApplication.FirstName != null)
                {
                    resultSets = resultSets.AsQueryable().Where(x => x.FirstName == searchApplication.FirstName).ToList();
                }
                if (searchApplication.LastName != null)
                {
                    resultSets = resultSets.AsQueryable().Where(x => x.LastName == searchApplication.LastName).ToList();
                }
                if (searchApplication.DOB != null)
                {
                    resultSets = resultSets.AsQueryable().Where(x => DateTime.Parse(x.DateOfBirth.ToString()).Date == DateTime.Parse(searchApplication.DOB.ToString()).Date).ToList();
                }
                if (searchApplication.status == "Approved")
                {
                    resultSets = resultSets.AsQueryable().Where(x => x.isApproved == "True").ToList();
                }
                if (searchApplication.status == "Not Approved")
                {
                    resultSets = resultSets.AsQueryable().Where(x => x.isApproved == "False").ToList();
                }
                if (searchApplication.ApplicationId != null)
                {
                    resultSets = resultSets.AsQueryable().Where(x => x.ApplicationId == searchApplication.ApplicationId).ToList();
                }
                foreach(var item in resultSets)
                {
                    if (item.ApplicationId == null)
                        item.ApplicationId = "n/a";
                    if (item.isApproved == "True")
                        item.isApproved = "True";
                    else
                        item.isApproved = "False";
                }
                ViewData["results"] = resultSets.ToPagedList(page ?? 1, 5);
                Session["res"] = resultSets;

                if (resultSets.Count > 100)
                {
                    ViewBag.ErrorForCount = "Please refine your search criteria, more than 100 results are found!";
                }

                return View();
            }

            return View();
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }

        public ActionResult res(int? page)
        {
            try { 
            var results = Session["res"] as List<ResultSet>;
            ViewData["results"] = results.ToPagedList(page ?? 1, 5);
            return View("SearchApplication");
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }
        
        [HttpPost]
        public ActionResult SearchResults()
        {
            return PartialView();
        }


        public ActionResult Edit(int data)
        {
            try { 
            var houseHoldMembers = httpHelper.GetCall<List<HouseHoldMemberDetails>>("api/Home/GetAllHouseHoldMembers").Result;

            HouseHoldMemberDetails member = new HouseHoldMemberDetails();

            member = houseHoldMembers.AsQueryable().Where(x => x.MemberId == data).FirstOrDefault();

            return View(member);
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }
        [HttpPost]
        public ActionResult Edit(HouseHoldMemberDetails member, string submit)

        {
            try
            {
                int age = DateTime.Now.Year - DateTime.Parse(member.DateOfBirth.ToString()).Year;
                if (age > 125 && age < 0)
                {
                    ModelState.AddModelError("DateOfBirth", "Age cannot be more than 125 or less than 0");
                }
                var list = httpHelper.GetCall<List<HouseHoldMemberDetails>>("api/Home/GetAllHouseHoldMembers").Result;

                var FirstMemberOfFamily = list.AsQueryable().Where(x => x.isFirstMember == true && x.FamilyId == member.FamilyId).FirstOrDefault();

                int ageOfFirstMember = DateTime.Now.Year - DateTime.Parse(FirstMemberOfFamily.DateOfBirth.ToString()).Year;
                if (age > ageOfFirstMember)
                {
                    ModelState.AddModelError("DateOfBirth", "Age cannot be greater than First Member Of Family");
                }

                if (ModelState.IsValid)
                {
                    httpHelper.PostData<HouseHoldMemberDetails>(member, "api/Home/UpdateMember");
                    return View("SearchApplication");
                }
                return View(member);
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }

        public ActionResult View(int data)
        {
            try { 
            var houseHoldMembers = httpHelper.GetCall<List<HouseHoldMemberDetails>>("api/Home/GetAllHouseHoldMembers").Result;

            HouseHoldMemberDetails member = new HouseHoldMemberDetails();

            member = houseHoldMembers.AsQueryable().Where(x => x.MemberId == data).FirstOrDefault();

            return View(member);
            }
            catch (Exception ex)
            {
                return View("Error"); ;
            }
        }

        //public ActionResult sam()
        //{
        //    var searchResults = httpHelper.GetCall<SearchResult>("api/Search/GetAll").Result;

        //    List<ResultSet> resultSet = new List<ResultSet>();



        //    return View(searchResults);
        //}
    }
}