using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Forum
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UsersDelete",
                url: "utilizatori/{UserName}/sterge-utilizator",
                defaults: new { controller = "Users", action = "Delete", UserName = "" },
                constraints: new { UserName = @"\S+$" }
            );

            routes.MapRoute(
                name: "UsersChange",
                url: "utilizatori/{UserName}/editeaza-utilizator",
                defaults: new { controller = "Users", action = "Edit" , UserName = "" },
                constraints: new { UserName = @"\S+$"}
            );

            routes.MapRoute(
                name: "Users",
                url: "utilizatori",
                defaults: new { controller = "Users", action = "Index" }
            );

            routes.MapRoute(
                name: "CommentariesDelete",
                url: "{id1}/{id2}/{id3}/sterge-comentariu",
                defaults: new { controller = "Commentaries", action = "Delete"},
                constraints: new { id1 = "\\d+", id2 = "\\d+", id3 = "\\d+" }
            );

            routes.MapRoute(
                name: "CommentariesEdit",
                url: "{id1}/{id2}/{id3}/editeaza-comentariu",
                defaults: new { controller = "Commentaries", action = "Edit"},
                constraints: new { id1 = "\\d+", id2 = "\\d+", id3 = "\\d+" }
            );

            routes.MapRoute(
                name: "CommentariesAdd",
                url: "{id1}/{id2}/adauga-comentariu",
                defaults: new { controller = "Commentaries", action = "Create"},
                constraints: new { id1 = "\\d+", id2 = "\\d+" }
            );

            routes.MapRoute(
                name: "SubjectsDelete",
                url: "{id1}/{id2}/sterge-subiect",
                defaults: new { controller = "Subjects", action = "Delete"},
                constraints: new { id1 = "\\d+", id2 = "\\d+" }
            );

            routes.MapRoute(
                name: "SubjectsEdit",
                url: "{id1}/{id2}/editeaza-subiect",
                defaults: new { controller = "Subjects", action = "Edit"},
                constraints: new { id1 = "\\d+", id2 = "\\d+" }
            );

            routes.MapRoute(
                name: "SubjectsEditMod",
                url: "{id1}/{id2}/editeaza-subiect-mod",
                defaults: new { controller = "Subjects", action = "EditModerator"},
                constraints: new { id1 = "\\d+", id2 = "\\d+" }
            );

            routes.MapRoute(
                name: "SubjectsDropDownAdd",
                url: "adauga-subiect",
                defaults: new { controller = "Subjects", action = "CreateWithDropDown" }
            );

            routes.MapRoute(
                name: "SubjectsAdd",
                url: "{id}/adauga-subiect",
                defaults: new { controller = "Subjects", action = "Create"},
                constraints: new { id = "\\d+" }
            );

            routes.MapRoute(
                name: "SubjectsShow",
                url: "{id1}/{id2}",
                defaults: new { controller = "Subjects", action = "Details"},
                constraints: new { id1 = "\\d+", id2 = "\\d+" }
            );

            routes.MapRoute(
                name: "CategoriesShow",
                url: "{id}",//"{id}/{sortOrder}/{currentFilter}/{searchString}/{page}",
                defaults: new { controller = "Categories", action = "Details" },//,  sortOrder = "", searchString =  "", CurrentOrder = "", currentFilter = "", page = UrlParameter.Optional },
                constraints: new { id = "\\d+" }//, sortOrder = @"\S*$", currentFilter = @"\S*$", searchString = @"\S*$", page = "\\d*"}
            );

            routes.MapRoute(
                name: "CategoriesDelete",
                url: "{id}/sterge-categorie",
                defaults: new { controller = "Categories", action = "Delete" },
                constraints: new { id = "\\d+" }
            );

            routes.MapRoute(
                name: "CategoriesEdit",
                url: "{id}/editeaza-categorie",
                defaults: new { controller = "Categories", action = "Edit" },
                constraints: new { id = "\\d+" }
            );

            routes.MapRoute(
                name: "CategoriesAdd",
                url: "adauga-categorie",
                defaults: new { controller = "Categories", action = "Create" }
            );

            routes.MapRoute(
                name: "Categories",
                url: "{controller}/{action}",
                defaults: new { controller = "Categories", action = "Index" }
            );
        }
    }
}