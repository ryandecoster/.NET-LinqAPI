using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;


namespace RapperAPI.Controllers {
    public class GroupController : Controller {
        List<Group> allGroups {get; set;}
        List<Artist> allArtists {get; set;}
        public GroupController() {
            allGroups = JsonToFile<Group>.ReadJson();
            allArtists = JsonToFile<Artist>.ReadJson();
        }
        // 1.) Create a route /groups that returns all groups as JSON
        [HttpGet]
        [Route("groups")]
        public JsonResult Groups()
        {
            return Json(allGroups);
        }
        // 2.) Create a route /groups/name/{name} that returns all groups that match the provided name
        [HttpGet]
        [Route("groups/name/{groupname}")]
        public JsonResult GroupName(string groupname)
        {
            var name = allGroups.Where(g => g.GroupName == $"{groupname}");
            return Json(name);
        }
        // 3.) Create a route /groups/id/{id} that returns all groups with the provided Id value
        [HttpGet]
        [Route("groups/id/{id}")]
        public JsonResult GroupID(int id)
        {
            var gid = allGroups.Where(g => g.Id == id).Join(allArtists, g => g.Id, a => a.GroupId, (g, a)=> {g.Members.Add(a); return g;}).ToList().Distinct();
            return Json(gid);
        }
        // 4.) (Optional) Add an extra boolean parameter to the group routes called displayArtists that will include members for all Group JSON responses
        [HttpGet]
        [Route("displayArtists")]
        [Route("displayArtists/{boolean}")]
        public JsonResult Display(bool boolean = false)
        {
            if (boolean == false)
            {
                return Json(allGroups);
            }
            else
            {
                var group = allGroups.Join(allArtists, g => g.Id, a => a.GroupId, (g, a)=> {g.Members.Add(a); return g;}).ToList().Distinct();
                return Json(group);
            }
        
            
                





                // var query =
                // (from g in allGroups
                // from a in allArtists
                // where g.Id == a.GroupId
                // select new {
                //             g.GroupName
                // }).Distinct().ToArray();
                
                
                // foreach (var i in query) {
                //     System.Console.WriteLine($"Key: {i.Key}");
                //     foreach (var j in i) {
                //         System.Console.WriteLine($"\t{j.ArtistName}");
                //     }
                // }
                
                // var query1 =
                // (from a in allArtists
                // group a by a.GroupId into g
                // select new { GroupId = g.Key, } 
                // System.Console.WriteLine(query.);
                // foreach (var j in query) {
                //     System.Console.WriteLine(j.a.ArtistName);
                // }
            
            // return Json(query);
        }
    }
}