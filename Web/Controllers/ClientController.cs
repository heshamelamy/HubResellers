using HubManPractices.Models;
using HubManPractices.Service;
using HubManPractices.Service.ViewModels;
using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Filters;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService ClientService;
        private readonly IRoleService RoleService;
        private readonly IResellerService ResellerService;
        private readonly ISubscriptionsService SubscriptionService;
        private readonly IActionService ActionService;

        public ClientController(IActionService As, IClientService clientservice, IResellerService resellerservice, IRoleService Rs, ISubscriptionsService SS)
        {
            this.ActionService = As;
            this.ClientService = clientservice;
            this.ResellerService = resellerservice;
            this.RoleService = Rs;
            this.SubscriptionService = SS;
        }
        // GET: Client
        [MyAuthFilter(Roles = "Global Admin , Reseller Admin")]
        public ActionResult Index(Guid ResellerID, string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            IEnumerable<Client> ResellerClients = ResellerService.GetResellerClients(ResellerID);
            foreach (var client in ResellerClients)
            {
                client.reseller = ResellerService.GetById(ResellerID);
                if (ClientService.NeedsRenewal(client))
                {
                    client.Status = "Waiting For Renewal";
                    ClientService.SaveClient();
                }
            }
            TempData["ResellerID"] = ResellerID;
            switch (sortOrder)
            {
                case "name_desc":
                    ResellerClients = ResellerClients.OrderByDescending(s => s.ClientName);
                    break;
                case "Date":
                    ResellerClients = ResellerClients.OrderBy(s => s.Expiry);
                    break;
                case "date_desc":
                    ResellerClients = ResellerClients.OrderByDescending(s => s.Expiry);
                    break;
                default:
                    ResellerClients = ResellerClients.OrderBy(s => s.ClientName);
                    break;
            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);

            if (Request.IsAjaxRequest())
            {
                return (ActionResult)PartialView("ClientListPartial", ClientService.MapToViewModel(ResellerClients).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(ClientService.MapToViewModel(ResellerClients).ToPagedList(pageNumber, pageSize));
            }
        }
        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        public ActionResult Create(Guid ResellerID)
        {
            if (HasPermission("Add Client"))
            {
                if (ResellerService.QuotaFull(ResellerID))
                {
                    TempData["QuotaFull"] = "Your Clients Quota is full";
                    return RedirectToAction("Index", new { ResellerID = ResellerID });
                }
                else
                {
                    TempData["ResellerID"] = ResellerID;
                    Client client = new Client();
                    ClientViewModel c = ClientService.MapToViewModel(client);
                    IEnumerable<OfficeSubscriptionViewModel> Subscriptions = SubscriptionService.MapToViewModel(SubscriptionService.GetAllSubscriptions());
                    var myTuple = new Tuple<ClientViewModel, IEnumerable<OfficeSubscriptionViewModel>>(c, Subscriptions);
                    return View(myTuple);
                }
            }
            return View("~/Views/Home/UnAuthorized.cshtml");
        }

        [HttpPost]
        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        public ActionResult Reactivate(FormCollection Fc)
        {
            Client ToReactivate = ClientService.GetClientByNameAndMail(Fc["ClientName"], Fc["ContactMail"]);
            ToReactivate.IsDeleted = false;
            ToReactivate.Status = "On Hold";
            ToReactivate.Expiry = null;
            ToReactivate.IsExpiryNull = true;
            var i = 0;
            foreach (var Sub in ToReactivate.ClientSubscriptions)
            {
                ToReactivate.ClientSubscriptions.ElementAt(i).IsDeleted = false;
                i++;
            }
            try
            {
                ClientService.EditClient(ToReactivate);
            }
            catch (DbUpdateException ex)
            {
                TempData["ReActivateError"] = "Error in Readding the client again..";
                return RedirectToAction("Create", new { ResellerID = ToReactivate.ResellerID });
            }

            HubManPractices.Models.Action Reactivate = new HubManPractices.Models.Action() { ActionID = Guid.NewGuid(), ActionName = "ReAactivated", Client = ToReactivate, Date = DateTime.Now };
            ActionService.CreateAction(Reactivate);
            TempData["ReactivateClient"] = "The Client has been Reactivated";
            return RedirectToAction("Index", new { ResellerID = ToReactivate.ResellerID });
        }

        [HttpPost]
        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        public ActionResult ReAdd(FormCollection Fc)
        {
            Client ToReAdd = ClientService.GetClientByNameAndMail(Fc["ClientName"], Fc["ContactMail"]);
            IEnumerable<OfficeSubscription> Subscriptions = SubscriptionService.GetAllSubscriptions();

            foreach (var Sub in ToReAdd.ClientSubscriptions)
            {
                if (Fc[Sub.OfficeSubscription.MonthlyFee.ToString()] != "")
                {
                    Sub.IsDeleted = false;
                    Sub.UsersPerSubscription = Int32.Parse(Fc[Sub.OfficeSubscription.MonthlyFee.ToString()]);
                }
            }
            ToReAdd.ContactName = Fc["ContactName"];
            ToReAdd.ContactNumber = Int32.Parse(Fc["ContactNumber"]);
            ToReAdd.ContactTitle = Fc["ContactTitle"];
            ToReAdd.Seats = Int32.Parse(Fc["Seats"]);
            ToReAdd.Expiry = null;
            ToReAdd.IsDeleted = false;
            ToReAdd.IsExpiryNull = true;
            ToReAdd.Status = Fc["Status"];
            try
            {
                ClientService.EditClient(ToReAdd);
            }
            catch (DbUpdateException ex)
            {
                TempData["ReAddError"] = "Error in Readding the client again..";
                return RedirectToAction("Create", new { ResellerID = ToReAdd.ResellerID });
            }
            return RedirectToAction("Index", new { ResellerID = ToReAdd.ResellerID });
        }

        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        [HttpPost]
        public ActionResult Delete(Guid ClientID)
        {
            if (HasPermission("Delete Client"))
            {

                Client ToBeDeleted = ClientService.GetById(ClientID);
                try
                {
                    ClientService.DeleteClient(ToBeDeleted);
                    HubManPractices.Models.Action Terminate = new HubManPractices.Models.Action() { ActionID = Guid.NewGuid(), ActionName = "Terminated", Client = ToBeDeleted, Date = DateTime.Now };
                    ActionService.CreateAction(Terminate);
                    return RedirectToAction("Index", new { ResellerID = ToBeDeleted.ResellerID });
                }
                catch (Exception ex)
                {
                    TempData["DeleteError"] = "Error in Deleting the Client";
                    return RedirectToAction("Index", new { ResellerID = ToBeDeleted.ResellerID });
                }

            }
            return View("~/Views/Home/UnAuthorized.cshtml");
        }

        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        [HttpPost]
        public ActionResult Suspend(Guid ClientID)
        {
            if (HasPermission("Suspend Client"))
            {

                Client ToBeSuspended = ClientService.GetById(ClientID);
                try
                {
                    ToBeSuspended.Status = "Suspended";
                    ClientService.SaveClient();
                    HubManPractices.Models.Action Suspend = new HubManPractices.Models.Action() { ActionID = Guid.NewGuid(), ActionName = "Suspended", Client = ToBeSuspended, Date = DateTime.Now };
                    ActionService.CreateAction(Suspend);
                    return RedirectToAction("Index", new { ResellerID = ToBeSuspended.ResellerID });
                }
                catch (Exception ex)
                {
                    TempData["SuspendError"] = "Error in Suspending the Client";
                    return RedirectToAction("Index", new { ResellerID = ToBeSuspended.ResellerID });
                }

            }
            return View("~/Views/Home/UnAuthorized.cshtml");
        }

        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        [HttpPost]
        public ActionResult Activate(Guid ClientID)
        {
            if (HasPermission("Activate Client"))
            {

                Client ToBeActivated = ClientService.GetById(ClientID);
                try
                {
                    ToBeActivated.Status = "Activated";
                    ToBeActivated.Expiry = DateTime.Now.AddMonths(1).Date;
                    ToBeActivated.IsExpiryNull = false;
                    ClientService.SaveClient();
                    HubManPractices.Models.Action Activate = new HubManPractices.Models.Action() { ActionID = Guid.NewGuid(), ActionName = "Activated", Client = ToBeActivated, Date = DateTime.Now };
                    ActionService.CreateAction(Activate);
                    return RedirectToAction("Index", new { ResellerID = ToBeActivated.ResellerID });
                }
                catch (Exception ex)
                {
                    TempData["ActivateError"] = "Error in Activating the Client";
                    return RedirectToAction("Index", new { ResellerID = ToBeActivated.ResellerID });
                }

            }
            return View("~/Views/Home/UnAuthorized.cshtml");
        }

        [MyAuthFilter(Roles = "Global Admin, Reseller Admin")]
        public ActionResult Upgrade(Guid ClientID)
        {
            if (HasPermission("Upgrade Client"))
            {
                IEnumerable<OfficeSubscriptionViewModel> Subscriptions = SubscriptionService.MapToViewModel(SubscriptionService.GetAllSubscriptions());
                ClientViewModel client = ClientService.MapToViewModel(ClientService.GetById(ClientID));
                var myTuple = new Tuple<ClientViewModel, IEnumerable<OfficeSubscriptionViewModel>>(client, Subscriptions);
                return View(myTuple);
            }
            return View("~/Views/Home/UnAuthorized.cshtml");
        }

        [MyAuthFilter(Roles = "Global Admin, Reseller Admin")]
        [HttpPost]
        public ActionResult Upgrade(FormCollection Fc)
        {
            if (HasPermission("Upgrade Client"))
            {
                Client client = ClientService.GetById(Guid.Parse(Fc["ClientUpgradeID"]));
                try
                {
                    IEnumerable<OfficeSubscription> Subscriptions = SubscriptionService.GetAllSubscriptions();
                    client.IsExpiryNull = false;
                    client.Seats += Int32.Parse(Fc["SeatsUpgrade"]);

                    ClientService.SaveClient();
                    HubManPractices.Models.Action Upgrade = new HubManPractices.Models.Action() { ActionID = Guid.NewGuid(), ActionName = "Upgraded", Client = ClientService.GetById(client.ClientID), Date = DateTime.Now };
                    ActionService.CreateAction(Upgrade);
                }
                catch (DbUpdateException ex)
                {
                    TempData["Upgrade Update Error"] = "Error while updating the entries";
                    return RedirectToAction("Upgrade", new { ClientID = client.ClientID });
                }
                catch (Exception ex)
                {
                    TempData["Upgrade Error"] = "Error in function Upgrade";
                    return RedirectToAction("Upgrade", new { ClientID = client.ClientID });
                }
                TempData["Upgrade Success"] = "The Client has been Upgraded";
                return RedirectToAction("Index", new { ResellerID = client.ResellerID });
            }

            return View("~/Views/Home/UnAuthorized.cshtml");
        }


        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        [HttpPost]
        public ActionResult Renew(Guid ClientID)
        {
            if (HasPermission("Renew Client"))
            {

                Client ToBeRenewd = ClientService.GetById(ClientID);
                try
                {
                    ToBeRenewd.Status = "Activated";
                    ToBeRenewd.Expiry = DateTime.Now.AddMonths(1).Date;
                    ToBeRenewd.IsExpiryNull = false;
                    ClientService.SaveClient();
                    HubManPractices.Models.Action Renewal = new HubManPractices.Models.Action() { ActionID = Guid.NewGuid(), ActionName = "Renewal", Client = ToBeRenewd, Date = DateTime.Now };
                    ActionService.CreateAction(Renewal);
                    return RedirectToAction("Index", new { ResellerID = ToBeRenewd.ResellerID });
                }
                catch (Exception ex)
                {
                    TempData["RenewError"] = "Error in Renwal of the Client";
                    return RedirectToAction("Index", new { ResellerID = ToBeRenewd.ResellerID });
                }

            }
            return View("~/Views/Home/UnAuthorized.cshtml");
        }

        [MyAuthFilter(Roles = "Global Admin,Reseller Admin")]
        public ActionResult GetDeleted(Guid ResellerID)
        {
            return View(ClientService.MapToViewModel(ResellerService.GetResellerDeletedClients(ResellerID)));
        }


        public bool HasPermission(string PName)
        {
            if (RoleService.HasPermission(User.Identity.GetUserId(), PName) == null)
                return false;
            return true;
        }
    }
}